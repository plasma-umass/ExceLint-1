﻿namespace ExceLintUI
{
    partial class ExceLintRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ExceLintRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceLintRibbon));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.tab2 = this.Factory.CreateRibbonTab();
            this.CheckCellGroup = this.Factory.CreateRibbonGroup();
            this.box1 = this.Factory.CreateRibbonBox();
            this.AnalyzeButton = this.Factory.CreateRibbonButton();
            this.MarkAsOKButton = this.Factory.CreateRibbonButton();
            this.StartOverButton = this.Factory.CreateRibbonButton();
            this.showHeatmap = this.Factory.CreateRibbonButton();
            this.stepModel = this.Factory.CreateRibbonButton();
            this.ClusterBox = this.Factory.CreateRibbonCheckBox();
            this.useResultant = this.Factory.CreateRibbonCheckBox();
            this.normSSCheckBox = this.Factory.CreateRibbonCheckBox();
            this.normRefCheckBox = this.Factory.CreateRibbonCheckBox();
            this.significanceTextBox = this.Factory.CreateRibbonEditBox();
            this.spectralRanking = this.Factory.CreateRibbonCheckBox();
            this.showFixes = this.Factory.CreateRibbonCheckBox();
            this.separator2 = this.Factory.CreateRibbonSeparator();
            this.allCellsFreq = this.Factory.CreateRibbonCheckBox();
            this.columnCellsFreq = this.Factory.CreateRibbonCheckBox();
            this.rowCellsFreq = this.Factory.CreateRibbonCheckBox();
            this.levelsFreq = this.Factory.CreateRibbonCheckBox();
            this.sheetFreq = this.Factory.CreateRibbonCheckBox();
            this.separator3 = this.Factory.CreateRibbonSeparator();
            this.DebugOutput = this.Factory.CreateRibbonCheckBox();
            this.forceBuildDAG = this.Factory.CreateRibbonCheckBox();
            this.inferAddrModes = this.Factory.CreateRibbonCheckBox();
            this.allCells = this.Factory.CreateRibbonCheckBox();
            this.weightByIntrinsicAnomalousness = this.Factory.CreateRibbonCheckBox();
            this.conditioningSetSize = this.Factory.CreateRibbonCheckBox();
            this.separator4 = this.Factory.CreateRibbonSeparator();
            this.annotate = this.Factory.CreateRibbonButton();
            this.annotateThisCell = this.Factory.CreateRibbonButton();
            this.LSHTest = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.tab2.SuspendLayout();
            this.CheckCellGroup.SuspendLayout();
            this.box1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // tab2
            // 
            this.tab2.Groups.Add(this.CheckCellGroup);
            this.tab2.Label = "ExceLint";
            this.tab2.Name = "tab2";
            // 
            // CheckCellGroup
            // 
            this.CheckCellGroup.Items.Add(this.box1);
            this.CheckCellGroup.Items.Add(this.ClusterBox);
            this.CheckCellGroup.Items.Add(this.useResultant);
            this.CheckCellGroup.Items.Add(this.normSSCheckBox);
            this.CheckCellGroup.Items.Add(this.normRefCheckBox);
            this.CheckCellGroup.Items.Add(this.significanceTextBox);
            this.CheckCellGroup.Items.Add(this.spectralRanking);
            this.CheckCellGroup.Items.Add(this.showFixes);
            this.CheckCellGroup.Items.Add(this.separator2);
            this.CheckCellGroup.Items.Add(this.allCellsFreq);
            this.CheckCellGroup.Items.Add(this.columnCellsFreq);
            this.CheckCellGroup.Items.Add(this.rowCellsFreq);
            this.CheckCellGroup.Items.Add(this.levelsFreq);
            this.CheckCellGroup.Items.Add(this.sheetFreq);
            this.CheckCellGroup.Items.Add(this.separator3);
            this.CheckCellGroup.Items.Add(this.DebugOutput);
            this.CheckCellGroup.Items.Add(this.forceBuildDAG);
            this.CheckCellGroup.Items.Add(this.inferAddrModes);
            this.CheckCellGroup.Items.Add(this.allCells);
            this.CheckCellGroup.Items.Add(this.weightByIntrinsicAnomalousness);
            this.CheckCellGroup.Items.Add(this.conditioningSetSize);
            this.CheckCellGroup.Items.Add(this.separator4);
            this.CheckCellGroup.Items.Add(this.annotate);
            this.CheckCellGroup.Items.Add(this.annotateThisCell);
            this.CheckCellGroup.Name = "CheckCellGroup";
            // 
            // box1
            // 
            this.box1.Items.Add(this.AnalyzeButton);
            this.box1.Items.Add(this.MarkAsOKButton);
            this.box1.Items.Add(this.StartOverButton);
            this.box1.Items.Add(this.showHeatmap);
            this.box1.Items.Add(this.stepModel);
            this.box1.Items.Add(this.LSHTest);
            this.box1.Name = "box1";
            // 
            // AnalyzeButton
            // 
            this.AnalyzeButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.AnalyzeButton.Image = global::ExceLintUI.Properties.Resources.analyze_small;
            this.AnalyzeButton.Label = "Audit";
            this.AnalyzeButton.Name = "AnalyzeButton";
            this.AnalyzeButton.ShowImage = true;
            this.AnalyzeButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.AnalyzeButton_Click);
            // 
            // MarkAsOKButton
            // 
            this.MarkAsOKButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.MarkAsOKButton.Image = global::ExceLintUI.Properties.Resources.mark_as_ok_small;
            this.MarkAsOKButton.Label = "Next Cell";
            this.MarkAsOKButton.Name = "MarkAsOKButton";
            this.MarkAsOKButton.ShowImage = true;
            this.MarkAsOKButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MarkAsOKButton_Click);
            // 
            // StartOverButton
            // 
            this.StartOverButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.StartOverButton.Image = global::ExceLintUI.Properties.Resources.clear_small;
            this.StartOverButton.Label = "Start Over";
            this.StartOverButton.Name = "StartOverButton";
            this.StartOverButton.ShowImage = true;
            this.StartOverButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.StartOverButton_Click);
            // 
            // showHeatmap
            // 
            this.showHeatmap.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.showHeatmap.Image = ((System.Drawing.Image)(resources.GetObject("showHeatmap.Image")));
            this.showHeatmap.Label = "Heat Map";
            this.showHeatmap.Name = "showHeatmap";
            this.showHeatmap.ShowImage = true;
            this.showHeatmap.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.showHeatmap_Click);
            // 
            // stepModel
            // 
            this.stepModel.Label = "Step Cluster Model";
            this.stepModel.Name = "stepModel";
            this.stepModel.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.stepModel_Click);
            // 
            // ClusterBox
            // 
            this.ClusterBox.Label = "Cluster";
            this.ClusterBox.Name = "ClusterBox";
            // 
            // useResultant
            // 
            this.useResultant.Label = "Use Resultant";
            this.useResultant.Name = "useResultant";
            // 
            // normSSCheckBox
            // 
            this.normSSCheckBox.Label = "Normalize Sheet";
            this.normSSCheckBox.Name = "normSSCheckBox";
            this.normSSCheckBox.Visible = false;
            // 
            // normRefCheckBox
            // 
            this.normRefCheckBox.Label = "Normalize Refs";
            this.normRefCheckBox.Name = "normRefCheckBox";
            this.normRefCheckBox.Visible = false;
            // 
            // significanceTextBox
            // 
            this.significanceTextBox.Label = "Error Rate %";
            this.significanceTextBox.Name = "significanceTextBox";
            this.significanceTextBox.SizeString = "100.0";
            this.significanceTextBox.Text = "5";
            this.significanceTextBox.TextChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.significanceTextBox_TextChanged);
            // 
            // spectralRanking
            // 
            this.spectralRanking.Label = "Use Spectral Rank";
            this.spectralRanking.Name = "spectralRanking";
            this.spectralRanking.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.spectralRanking_Click);
            // 
            // showFixes
            // 
            this.showFixes.Enabled = false;
            this.showFixes.Label = "Show Fixes";
            this.showFixes.Name = "showFixes";
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            // 
            // allCellsFreq
            // 
            this.allCellsFreq.Checked = true;
            this.allCellsFreq.Label = "All Cells Freq";
            this.allCellsFreq.Name = "allCellsFreq";
            this.allCellsFreq.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.allCellsFreq_Click);
            // 
            // columnCellsFreq
            // 
            this.columnCellsFreq.Checked = true;
            this.columnCellsFreq.Label = "Column Cells Freq";
            this.columnCellsFreq.Name = "columnCellsFreq";
            this.columnCellsFreq.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.columnCellsFreq_Click);
            // 
            // rowCellsFreq
            // 
            this.rowCellsFreq.Checked = true;
            this.rowCellsFreq.Label = "Row Cells Freq";
            this.rowCellsFreq.Name = "rowCellsFreq";
            this.rowCellsFreq.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.rowCellsFreq_Click);
            // 
            // levelsFreq
            // 
            this.levelsFreq.Checked = true;
            this.levelsFreq.Label = "Levels Freq";
            this.levelsFreq.Name = "levelsFreq";
            this.levelsFreq.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.levelsFreq_Click);
            // 
            // sheetFreq
            // 
            this.sheetFreq.Checked = true;
            this.sheetFreq.Label = "Sheet Freq";
            this.sheetFreq.Name = "sheetFreq";
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            // 
            // DebugOutput
            // 
            this.DebugOutput.Label = "Show Debug Output";
            this.DebugOutput.Name = "DebugOutput";
            this.DebugOutput.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.DebugOutput_Click);
            // 
            // forceBuildDAG
            // 
            this.forceBuildDAG.Label = "Force DAG Rebuild";
            this.forceBuildDAG.Name = "forceBuildDAG";
            this.forceBuildDAG.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.forceBuildDAG_Click);
            // 
            // inferAddrModes
            // 
            this.inferAddrModes.Label = "Infer Address Modes";
            this.inferAddrModes.Name = "inferAddrModes";
            this.inferAddrModes.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.inferAddrModes_Click);
            // 
            // allCells
            // 
            this.allCells.Label = "Analyze All Cells";
            this.allCells.Name = "allCells";
            this.allCells.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.allCells_Click);
            // 
            // weightByIntrinsicAnomalousness
            // 
            this.weightByIntrinsicAnomalousness.Label = "Reweight by Intrinsic Anomalousness";
            this.weightByIntrinsicAnomalousness.Name = "weightByIntrinsicAnomalousness";
            this.weightByIntrinsicAnomalousness.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.weightByIntrinsicAnomalousness_Click);
            // 
            // conditioningSetSize
            // 
            this.conditioningSetSize.Checked = true;
            this.conditioningSetSize.Label = "Weigh by Conditioning Set Size";
            this.conditioningSetSize.Name = "conditioningSetSize";
            this.conditioningSetSize.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.conditioningSetSize_Click);
            // 
            // separator4
            // 
            this.separator4.Name = "separator4";
            // 
            // annotate
            // 
            this.annotate.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.annotate.Image = global::ExceLintUI.Properties.Resources.correct_small;
            this.annotate.Label = "Annotate";
            this.annotate.Name = "annotate";
            this.annotate.ShowImage = true;
            this.annotate.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.annotate_Click);
            // 
            // annotateThisCell
            // 
            this.annotateThisCell.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.annotateThisCell.Image = global::ExceLintUI.Properties.Resources.correct_small;
            this.annotateThisCell.Label = "Annotate Current Cell";
            this.annotateThisCell.Name = "annotateThisCell";
            this.annotateThisCell.ShowImage = true;
            this.annotateThisCell.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.annotateThisCell_Click);
            // 
            // LSHTest
            // 
            this.LSHTest.Label = "LSH Test";
            this.LSHTest.Name = "LSHTest";
            this.LSHTest.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.LSHTest_Click);
            // 
            // ExceLintRibbon
            // 
            this.Name = "ExceLintRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Tabs.Add(this.tab2);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ExceLintRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.tab2.ResumeLayout(false);
            this.tab2.PerformLayout();
            this.CheckCellGroup.ResumeLayout(false);
            this.CheckCellGroup.PerformLayout();
            this.box1.ResumeLayout(false);
            this.box1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        private Microsoft.Office.Tools.Ribbon.RibbonTab tab2;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup CheckCellGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonBox box1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton AnalyzeButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MarkAsOKButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton StartOverButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator2;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox allCellsFreq;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox columnCellsFreq;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox rowCellsFreq;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox significanceTextBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator3;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox DebugOutput;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton showHeatmap;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox forceBuildDAG;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox inferAddrModes;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox allCells;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox weightByIntrinsicAnomalousness;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox levelsFreq;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox conditioningSetSize;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox spectralRanking;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox showFixes;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator4;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton annotate;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton annotateThisCell;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox normRefCheckBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox normSSCheckBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox useResultant;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox sheetFreq;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox ClusterBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton stepModel;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton LSHTest;
    }

    partial class ThisRibbonCollection
    {
        internal ExceLintRibbon ExceLintRibbon
        {
            get { return this.GetRibbon<ExceLintRibbon>(); }
        }
    }
}
