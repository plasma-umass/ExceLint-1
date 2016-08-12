﻿open COMWrapper
open System
open System.IO
open System.Collections.Generic
open ExceLint

    type Stats = {
        shortname: string;
        threshold: double;
        except_excelint: HashSet<AST.Address>;
        true_smells_this_wb : HashSet<AST.Address>;
        true_smells_not_found_by_excelint: HashSet<AST.Address>;
        true_smells_not_found_by_custodes: HashSet<AST.Address>;
        true_smells_not_found: HashSet<AST.Address>;
        excel_this_wb: HashSet<AST.Address>;
        excelint_true_smells: HashSet<AST.Address>;
        custodes_true_smells: HashSet<AST.Address>;
        excelint_excel_intersect: HashSet<AST.Address>;
        custodes_excel_intersect: HashSet<AST.Address>;
    }

    let hs_difference<'a>(hs1: HashSet<'a>)(hs2: HashSet<'a>) : HashSet<'a> =
        let hs3 = new HashSet<'a>(hs1)
        hs3.ExceptWith(hs2)
        hs3

    let hs_intersection<'a>(hs1: HashSet<'a>)(hs2: HashSet<'a>) : HashSet<'a> =
        let hs3 = new HashSet<'a>(hs1)
        hs3.IntersectWith(hs2)
        hs3

    let hs_union<'a>(hs1: HashSet<'a>)(hs2: HashSet<'a>) : HashSet<'a> =
        let hs3 = new HashSet<'a>(hs1)
        hs3.UnionWith(hs2)
        hs3

    let per_append_excelint(sw: StreamWriter)(csv: CSV.WorkbookStats)(truth: CUSTODES.GroundTruth)(custodes: CUSTODES.Output)(model: ErrorModel)(ranking: Pipeline.Ranking) : unit =
        // append all ExceLint flagged cells
        Array.mapi (fun i (kvp: KeyValuePair<AST.Address,double>) ->
            let addr = kvp.Key
            let per_row = CSV.WorkbookStats.Row(
                                path = addr.A1Path(),
                                workbook = addr.WorkbookName,
                                worksheet = addr.WorksheetName,
                                addr = addr.A1Local(),
                                flaggedByExcelint = (i <= model.Cutoff),
                                flaggedByCustodes = custodes.Smells.Contains addr,
                                flaggedByExcel = truth.Excel.Contains addr,
                                cliSameAsV1 = truth.differs addr (custodes.Smells.Contains addr),
                                rank = i,
                                score = kvp.Value,
                                custodesTrueSmell = truth.isTrueSmell addr
                            )

            // append to streamwriter
            sw.Write (csv.Append([per_row]).SaveToString())
        ) ranking |> ignore

    let per_append_custodes(sw: StreamWriter)(csv: CSV.WorkbookStats)(truth: CUSTODES.GroundTruth)(custodes: CUSTODES.Output)(model: ErrorModel)(ranking: Pipeline.Ranking)(except_excelint: HashSet<AST.Address>) : unit =
        // append all remaining CUSTODES cells
        Array.map (fun (addr: AST.Address) ->
            let per_row = CSV.WorkbookStats.Row(
                                path = addr.A1Path(),
                                workbook = addr.WorkbookName,
                                worksheet = addr.WorksheetName,
                                addr = addr.A1Local(),
                                flaggedByExcelint = false,
                                flaggedByCustodes = true,
                                flaggedByExcel = truth.Excel.Contains addr,
                                cliSameAsV1 = truth.differs addr (custodes.Smells.Contains addr),
                                rank = 999999999,
                                score = 0.0,
                                custodesTrueSmell = truth.isTrueSmell addr
                            )

            // append to streamwriter
            sw.Write (csv.Append([per_row]).SaveToString())
        ) (except_excelint |> Seq.toArray) |> ignore

    let per_append_true_smells(sw: StreamWriter)(csv: CSV.WorkbookStats)(truth: CUSTODES.GroundTruth)(custodes: CUSTODES.Output)(model: ErrorModel)(ranking: Pipeline.Ranking)(true_smells_not_found: HashSet<AST.Address>) : unit =
        // append all true smells found by neither tool
        Array.map (fun (addr: AST.Address) ->
            let per_row = CSV.WorkbookStats.Row(
                                path = addr.A1Path(),
                                workbook = addr.WorkbookName,
                                worksheet = addr.WorksheetName,
                                addr = addr.A1Local(),
                                flaggedByExcelint = false,
                                flaggedByCustodes = false,
                                flaggedByExcel = truth.Excel.Contains addr,
                                cliSameAsV1 = truth.differs addr (custodes.Smells.Contains addr),
                                rank = 999999999,
                                score = 0.0,
                                custodesTrueSmell = true
                            )

            // append to streamwriter
            sw.Write (csv.Append([per_row]).SaveToString())
        ) (true_smells_not_found |> Seq.toArray) |> ignore

    let per_append_debug(sw: StreamWriter)(csv: CSV.DebugInfo)(graph: Depends.DAG)(except_excelint: HashSet<AST.Address>) : unit =
        // warn user if CUSTODES analysis contains cells not analyzed by ExceLint
        if except_excelint.Count <> 0 then
            let all_comp = new HashSet<AST.Address>(graph.allComputationCells())
            let ee_formula_related = Seq.filter (fun addr -> all_comp.Contains addr) except_excelint |> Seq.toArray
            printfn "WARNING: CUSTODES analysis contains %d cells not analyzed by ExceLint," (except_excelint.Count)
            printfn "         %d of which are formula-related (either inputs or formulas)." ee_formula_related.Length
            if ee_formula_related.Length > 0 then
                printfn "         Writing to debug.csv."

                // append all true smells found by neither tool
                Array.map (fun (addr: AST.Address) ->
                    let per_row = CSV.DebugInfo.Row(
                                        path = addr.A1Path(),
                                        workbook = addr.WorkbookName,
                                        worksheet = addr.WorksheetName,
                                        addr = addr.A1Local()
                                    )

                    // append to streamwriter
                    sw.Write (csv.Append([per_row]).SaveToString())
                ) (ee_formula_related |> Seq.toArray) |> ignore

    let append_stats(stats: Stats)(sw: StreamWriter)(csv: CSV.ExceLintStats)(custodes: CUSTODES.Output)(model: ErrorModel)(config: Args.Config) : unit =
        // write stats
        let row = CSV.ExceLintStats.Row(
                    benchmarkName = stats.shortname,
                    numCells = model.DependenceGraph.allCells().Length,
                    numFormulas = model.DependenceGraph.getAllFormulaAddrs().Length,
                    sigThresh = stats.threshold,
                    depTimeMs = model.DependenceGraph.AnalysisMilliseconds,
                    scoreTimeMs = model.ScoreTimeInMilliseconds,
                    freqTimeMs = model.FrequencyTableTimeInMilliseconds,
                    rankingTimeMs = model.RankingTimeInMilliseconds,
                    causesTimeMs = model.CausesTimeInMilliseconds,
                    conditioningSetSzTimeMs = model.ConditioningSetSizeTimeInMilliseconds,
                    numAnom = model.Cutoff,
                    numCustodesSmells = custodes.NumSmells,
                    trueSmells = stats.true_smells_this_wb.Count,
                    excelintTrueSmellsFound = stats.excelint_true_smells.Count,
                    custodesTrueSmellsFound = stats.custodes_true_smells.Count,
                    excelintCustodesTrueSmellIntersect = (hs_intersection stats.excelint_true_smells stats.custodes_true_smells).Count,
                    trueSmellsMissedByBoth = (hs_difference stats.true_smells_this_wb (hs_union stats.excelint_true_smells stats.custodes_true_smells)).Count,
                    excelFlags = stats.excel_this_wb.Count,
                    excelintExcelIntersect = stats.excelint_excel_intersect.Count,
                    custodesExcelIntersect = stats.custodes_excel_intersect.Count,
                    excelFlagsMissedByBoth = (hs_difference stats.excel_this_wb (hs_union stats.excelint_excel_intersect stats.custodes_excel_intersect)).Count,
                    optCondAllCells = config.FeatureConf.IsEnabledOptCondAllCells,
                    optCondRows = config.FeatureConf.IsEnabledOptCondRows,
                    optCondCols = config.FeatureConf.IsEnabledOptCondCols,
                    optCondLevels = config.FeatureConf.IsEnabledOptCondLevels,
                    optAddrmodeInference = config.FeatureConf.IsEnabledOptAddrmodeInference,
                    optWeightIntrinsicAnom = config.FeatureConf.IsEnabledOptWeightIntrinsicAnomalousness,
                    optWeightConditionSetSz = config.FeatureConf.IsEnabledOptWeightConditioningSetSize
                    )

        // append to streamwriter & flush stream
        sw.Write (csv.Append([row]).SaveToString())
        sw.Flush()

    let rankToSet(ranking: Pipeline.Ranking)(model: ErrorModel) : HashSet<AST.Address> =
        Array.mapi (fun i (kvp: KeyValuePair<AST.Address,double>) -> (i, kvp.Key)) ranking
        |> Array.filter (fun (i,e) -> i <= model.Cutoff)
        |> Array.map (fun (i,e) -> e)
        |> (fun arr -> new HashSet<AST.Address>(arr))

    [<EntryPoint>]
    let main argv = 
        let config = Args.processArgs argv

        Console.CancelKeyPress.Add(
            (fun _ ->
                printfn "Ctrl-C received.  Cancelling..."
                System.Environment.Exit(1)
            )
        )

        using(new Application()) (fun app ->

            let thresh = 0.05

            let csv = new CSV.ExceLintStats([])
            let debug_csv = new CSV.DebugInfo([])

            let truth = new CUSTODES.GroundTruth(config.InputDirectory)

            using (new StreamWriter(config.csv)) (fun sw ->
                using (new StreamWriter(IO.Path.Combine(config.OutputPath, "debug.csv"))) (fun debug_sw ->
            
                    // write headers
                    sw.Write(CSV.ExceLintStatsHeaders)
                    sw.Flush()

                    debug_sw.Write(CSV.DebugInfoHeaders)
                    debug_sw.Flush()

                    for file in config.files do
                        let shortf = (System.IO.Path.GetFileName file)

                        printfn "Opening: %A" shortf
                        let wb = app.OpenWorkbook(file)
            
                        printfn "Building dependence graph: %A" shortf
                        let graph = wb.buildDependenceGraph()

                        printfn "Running ExceLint analysis: %A" shortf
                        let model_opt = ExceLint.ModelBuilder.analyze (app.XLApplication()) config.FeatureConf graph thresh (Depends.Progress.NOPProgress())

                        printfn "Running CUSTODES analysis: %A" shortf
                        let custodes = CUSTODES.Output(file, config.CustodesPath, config.JavaPath)

                        match model_opt with
                        | Some(model) ->
                            // per-workbook stats
                            let per_csv = new CSV.WorkbookStats([])

                            using (new StreamWriter(config.verbose_csv shortf)) (fun per_sw ->
                                // write header
                                // write headers
                                per_sw.Write(CSV.WorkbookStatsHeaders)
                                per_sw.Flush()

                                let ranking = model.rankByFeatureSum()

                                // get the set of cells flagged by ExceLint
                                let excelint_flags = rankToSet ranking model

                                // find difference between ExceLint and CUSTODES
                                let except_excelint = hs_difference excelint_flags custodes.Smells;

                                // find true smells found by neither tool
                                let true_smells_this_wb = hs_intersection truth.TrueSmells model.AllCells
                                let true_smells_not_found_by_excelint = hs_difference true_smells_this_wb excelint_flags
                                let true_smells_not_found_by_custodes = hs_difference true_smells_this_wb custodes.Smells
                                let true_smells_not_found = hs_intersection true_smells_not_found_by_excelint true_smells_not_found_by_custodes

                                // overall stats
                                let excel_this_wb = hs_intersection truth.Excel model.AllCells
                                let excelint_true_smells = hs_intersection excelint_flags true_smells_this_wb
                                let custodes_true_smells = hs_intersection custodes.Smells true_smells_this_wb
                                let excelint_excel_intersect = hs_intersection excelint_flags excel_this_wb
                                let custodes_excel_intersect = hs_intersection custodes.Smells excel_this_wb

                                let stats = {
                                    shortname = shortf;
                                    threshold = thresh;
                                    except_excelint = except_excelint;
                                    true_smells_this_wb = true_smells_this_wb;
                                    true_smells_not_found_by_excelint = true_smells_not_found_by_excelint;
                                    true_smells_not_found_by_custodes = true_smells_not_found_by_custodes;
                                    true_smells_not_found = true_smells_not_found;
                                    excel_this_wb =  excel_this_wb;
                                    excelint_true_smells = excelint_true_smells;
                                    custodes_true_smells = custodes_true_smells;
                                    excelint_excel_intersect = excelint_excel_intersect;
                                    custodes_excel_intersect = custodes_excel_intersect;
                                }

                                // write to per-workbook CSV
                                per_append_excelint per_sw per_csv truth custodes model ranking
                                per_append_custodes per_sw per_csv truth custodes model ranking except_excelint
                                per_append_true_smells per_sw per_csv truth custodes model ranking true_smells_not_found

                                // write overall stats to CSV
                                append_stats stats sw csv custodes model config

                                // sanity check
                                per_append_debug debug_sw debug_csv graph except_excelint
                            )

                            printfn "Analysis complete: %A" shortf
                                
                        | None ->
                            printfn "Analysis failed: %A" shortf
                )
            )
        )

        printfn "Press Enter to continue."
        Console.ReadLine() |> ignore

        0
