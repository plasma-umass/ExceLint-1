﻿using System.IO;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using System;
using System.Text.RegularExpressions;
using BugClass = System.Collections.Generic.HashSet<AST.Address>;
using Microsoft.FSharp.Core;

namespace ExceLintFileFormats
{
    public struct BugAnnotation
    {
        public BugKind BugKind;
        public string Note;

        public BugAnnotation(BugKind bugKind, string note)
        {
            BugKind = bugKind;
            Note = note;
        }

        public string Comment
        {
            get { return BugKind.ToString() + ": " + Note; }
        }
    };

    public class ExceLintGroundTruth: IDisposable
    {
        private string _dbpath;
        private Dictionary<AST.Address, BugKind> _bugs = new Dictionary<AST.Address, BugKind>();
        private Dictionary<AST.Address, string> _notes = new Dictionary<AST.Address, string>();
        private HashSet<AST.Address> _added = new HashSet<AST.Address>();
        private HashSet<AST.Address> _changed = new HashSet<AST.Address>();
        private Dictionary<AST.Address, BugClass> _bugclass_lookup = new Dictionary<AST.Address, BugClass>();
        private Dictionary<AST.Address, BugClass> _dual_lookup = new Dictionary<AST.Address, BugClass>();
        private Dictionary<BugClass, BugClass> _bugclass_dual_lookup = new Dictionary<BugClass, BugClass>();

        /// <summary>
        /// Get the AST.Address for a row.
        /// </summary>
        /// <param name="addrStr"></param>
        /// <param name="worksheetName"></param>
        /// <param name="workbookName"></param>
        /// <returns></returns>
        private AST.Address Address(string addrStr, string worksheetName, string workbookName)
        {
            return AST.Address.FromA1String(
                addrStr.ToUpper(),
                worksheetName,
                workbookName,
                ""  // we don't care about paths
            );
        } 

        private FSharpOption<BugClass> DualsFor(AST.Address addr)
        {
            // extract address environment
            var env = new AST.Env(addr.Path, addr.WorkbookName, addr.WorksheetName);

            // duals regexp
            var r = new Regex(@".*dual\s*=\s*((?<AddrOrRange>[A-Z]+[0-9]+(:[A-Z]+[0-9]+)?)(\s?,\s?)?)+", RegexOptions.Compiled);

            // get note for this address
            var note = _notes[addr];

            MatchCollection ms = r.Matches(note);
            if (ms.Count == 0)
            {
                if (note.Contains("dual"))
                {
                    Console.Out.WriteLine("Malformed dual annotation: " + note);
                }
                return FSharpOption<BugClass>.None;
            } else
            {
                // init duals list
                var duals = new List<AST.Address>();

                foreach (Match m in ms)
                {
                    // get group
                    string addrOrRange = m.Groups["AddrOrRange"].Value;

                    AST.Reference xlref = null;

                    try
                    {
                        // parse
                        xlref = Parcel.simpleReferenceParser(addrOrRange, env);
                    } catch (Exception e)
                    {
                        var msg = "Bad reference: '" + addrOrRange + "'";
                        Console.Out.WriteLine(msg);
                        throw new Exception(msg);
                    }
                    
                    // figure out the reference type
                    if (xlref.Type == AST.ReferenceType.ReferenceRange)
                    {
                        var rrref = (AST.ReferenceRange)xlref;
                        duals.AddRange(rrref.Range.Addresses());
                    } else if (xlref.Type == AST.ReferenceType.ReferenceAddress)
                    {
                        var aref = (AST.ReferenceAddress)xlref;
                        duals.Add(aref.Address);
                    } else
                    {
                        throw new Exception("Unsupported address reference type.");
                    }
                }

                var bugclass = new BugClass(duals);

                return FSharpOption<BugClass>.Some(bugclass);
            }
        }

        private ExceLintGroundTruth(string dbpath, ExceLintGroundTruthRow[] rows)
        {
            Console.WriteLine("Indexing ExceLint bug database...");

            _dbpath = dbpath;

            foreach (var row in rows)
            {
                if (row.Address != "Address")
                {
                    AST.Address addr = Address(row.Address, row.Worksheet, row.Workbook);
                    _bugs.Add(addr, BugKind.ToKind(row.BugKind));
                    _notes.Add(addr, row.Notes);
                }
            }

            // find all bugclasses
            foreach (KeyValuePair<AST.Address,BugKind> kvp in _bugs)
            {
                // get address
                var addr = kvp.Key;

                // get duals, if there are any
                var dual_opt = DualsFor(addr);
                if (FSharpOption<BugClass>.get_IsSome(dual_opt))
                {
                    // dual bugclass
                    var duals = dual_opt.Value;

                    // store each dual address in a bugclass if it hasn't already been stored
                    foreach (AST.Address caddr in duals)
                    {
                        // case 1: no bugclass stored for address
                        if (!_bugclass_lookup.ContainsKey(caddr))
                        {
                            // add it
                            _bugclass_lookup.Add(caddr, duals);
                        }
                    }

                    // get all the addresses in dual and saved bugclasses
                    var classaddrs = duals.SelectMany(caddr => _bugclass_lookup[caddr]);

                    // get an arbitrary bugclass
                    var fstbugclass = _bugclass_lookup[classaddrs.First()];

                    // is every address in this class?  if not, add them
                    foreach (AST.Address caddr in classaddrs)
                    {
                        if (!fstbugclass.Contains(caddr))
                        {
                            fstbugclass.Add(caddr);
                        }
                    }

                    // ensure that every address refers to the very same bugclass object
                    foreach (AST.Address caddr in classaddrs)
                    {
                        _bugclass_lookup[caddr] = fstbugclass;
                    }

                    // now make sure that the dual lookup for addr points to fstbugclass
                    _dual_lookup.Add(addr, fstbugclass);
                }
            }

            // make sure that all of the bugs in the dual bugclasses have lookups for their own bugclasses
            foreach (var kvp in _dual_lookup)
            {
                var bugclass = kvp.Value;

                foreach (var addr in bugclass)
                {
                    if (!_bugclass_lookup.ContainsKey(addr))
                    {
                        _bugclass_lookup.Add(addr, bugclass);
                    }
                }
            }

            // make sure that every bug remaining (i.e., those without duals) is in a bugclass
            foreach (var kvp in _bugs)
            {
                var addr = kvp.Key;
                if (!_bugclass_lookup.ContainsKey(addr))
                {
                    var bc = new HashSet<AST.Address>();
                    bc.Add(addr);
                    _bugclass_lookup.Add(addr, bc);
                }
            }

            // now index bugclass -> bugclass dual lookup
            foreach (var kvp in _bugclass_lookup)
            {
                var addr = kvp.Key;
                var bugclass = kvp.Value;

                // did we already save the dual for this bugclass?
                if (!_bugclass_dual_lookup.ContainsKey(bugclass))
                {
                    // grab the dual bugclass for this bugclass, if it has one
                    if (_dual_lookup.ContainsKey(addr))
                    {
                        var dual = _dual_lookup[addr];

                        // since the bugclass should be the same for all
                        // addresses in the bugclass, just lookup the bugclass
                        // by an arbitrary representative address
                        var fstdual = dual.First();
                        var dual_bugclass = _bugclass_lookup[fstdual];

                        // now save it
                        _bugclass_dual_lookup.Add(bugclass, dual_bugclass);
                    }
                }
            }

            // sanity check
            foreach (var kvp in _bugclass_lookup)
            {
                var addr = kvp.Key;
                if (!_bugs.ContainsKey(addr))
                {
                    Console.WriteLine("WARNING: Address " + addr.A1FullyQualified() + " referenced in bug notes but not annotated.");
                }
            }

            Console.WriteLine("Done indexing ExceLint bug database.");
        }

        public BugAnnotation AnnotationFor(AST.Address addr)
        {
            if (_bugs.ContainsKey(addr))
            {
                return new BugAnnotation(_bugs[addr], _notes[addr]);
            } else
            {
                return new BugAnnotation(BugKind.NotABug, "");
            }
        }

        public List<Tuple<AST.Address,BugAnnotation>> AnnotationsFor(string workbookname)
        {
            var output = new List<Tuple<AST.Address, BugAnnotation>>();

            foreach (var bug in _bugs)
            {
                var addr = bug.Key;

                if (addr.WorkbookName == workbookname)
                {
                    output.Add(new Tuple<AST.Address, BugAnnotation>(addr, new BugAnnotation(bug.Value, _notes[bug.Key])));
                }
            }

            return output;
        }

        /// <summary>
        /// Insert or update an annotation for a given cell.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="annot"></param>
        public void SetAnnotationFor(AST.Address addr, BugAnnotation annot)
        {
            if (_bugs.ContainsKey(addr))
            {
                _bugs[addr] = annot.BugKind;
                _notes[addr] = annot.Note;
                _changed.Add(addr);
            }
            else
            {
                _bugs.Add(addr, annot.BugKind);
                _notes.Add(addr, annot.Note);
                _added.Add(addr);
            }
        }

        public void Write()
        {
            // always append if any existing line changed
            var noAppend = _changed.Count() > 0;

            using (StreamWriter sw = new StreamWriter(
                path: _dbpath,
                append: !noAppend,
                encoding: System.Text.Encoding.UTF8))
            {
                using (CsvWriter cw = new CsvWriter(sw))
                {
                    // if any annotation changed, we need to write
                    // the entire file over again.
                    if (noAppend)
                    {
                        // write header
                        cw.WriteHeader<ExceLintGroundTruthRow>();

                        // write the rest of the file
                        foreach (var pair in _bugs)
                        {
                            var addr = pair.Key;
                            var bugAnnotation = AnnotationFor(addr);

                            var row = new ExceLintGroundTruthRow();
                            row.Address = addr.A1Local();
                            row.Worksheet = addr.A1Worksheet();
                            row.Workbook = addr.A1Workbook();
                            row.BugKind = bugAnnotation.BugKind.ToLog();
                            row.Notes = bugAnnotation.Note;

                            cw.WriteRecord(row);
                        }

                        _changed.Clear();
                        _added.Clear();
                    } else
                    {
                        // if the total number of bugs is not the same
                        // as the number of changes, then it's because 
                        // some annotations came from a file and thus
                        // we are only appending; do not write a new
                        // header when appending because we already 
                        // have one.
                        if (_bugs.Count() == _added.Count())
                        {
                            cw.WriteHeader<ExceLintGroundTruthRow>();
                        }

                        foreach (var addr in _added)
                        {
                            var bugAnnotation = AnnotationFor(addr);

                            var row = new ExceLintGroundTruthRow();
                            row.Address = addr.A1Local();
                            row.Worksheet = addr.A1Worksheet();
                            row.Workbook = addr.A1Workbook();
                            row.BugKind = bugAnnotation.BugKind.ToLog();
                            row.Notes = bugAnnotation.Note;

                            cw.WriteRecord(row);
                        }

                        _added.Clear();
                    }
                }
            }
        }

        public bool IsABug(AST.Address addr)
        {
            return _bugs.ContainsKey(addr) && _bugs[addr] != BugKind.NotABug;
        }

        private bool IsTrueRefBug(BugKind b)
        {
            return
                b == BugKind.FormulaWhereConstantExpected ||
                b == BugKind.ConstantWhereFormulaExpected ||
                b == BugKind.ReferenceBug ||
                b == BugKind.ReferenceBugInverse;
        }

        private bool IsTrueRefBugOrSuspicious(BugKind b)
        {
            return
                IsTrueRefBug(b) ||
                b == BugKind.SuspiciousCell;
        }

        //public HashSet<AST.Address> TrueRefBugsByWorkbook(string wbname)
        //{
        //    return new HashSet<AST.Address>(
        //        _bugs
        //            .Where(pair => pair.Key.A1Workbook() == wbname && IsTrueRefBug(pair.Value))
        //            .Select(pair => pair.Key)
        //        );
        //}

        //public HashSet<AST.Address> TrueRefBugsOrSuspiciousByWorkbook(string wbname)
        //{
        //    return new HashSet<AST.Address>(
        //        _bugs
        //            .Where(pair => pair.Key.A1Workbook() == wbname && IsTrueRefBugOrSuspicious(pair.Value))
        //            .Select(pair => pair.Key)
        //        );
        //}

        public static ExceLintGroundTruth Load(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var rows = new CsvReader(sr).GetRecords<ExceLintGroundTruthRow>().ToArray();

                return new ExceLintGroundTruth(path, rows);
            }
        }

        public static ExceLintGroundTruth Create(string gtpath)
        {
            using (StreamWriter sw = new StreamWriter(gtpath))
            {
                using (CsvWriter cw = new CsvWriter(sw))
                {
                    cw.WriteHeader<ExceLintGroundTruthRow>();
                }
            }

            return Load(gtpath);
        }

        public void Dispose()
        {
            Write();
        }
    }

    class ExceLintGroundTruthRow
    {
        public string Workbook { get; set; }
        public string Worksheet { get; set; }
        public string Address { get; set; }
        public string BugKind { get; set; }
        public string Notes { get; set; }
    }
}
