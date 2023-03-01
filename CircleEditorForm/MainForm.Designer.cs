namespace CircleEditorForm
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerRender = new System.Windows.Forms.Timer(this.components);
            this.openMapFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveMapFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.pb_ViewPort = new System.Windows.Forms.PictureBox();
            this.gb_Control = new System.Windows.Forms.GroupBox();
            this.gb_Algorithm = new System.Windows.Forms.GroupBox();
            this.btn_RunA = new System.Windows.Forms.Button();
            this.btn_GenerateFullGraphBenchmark = new System.Windows.Forms.Button();
            this.btn_RunABenchmark = new System.Windows.Forms.Button();
            this.btn_GenerateFullGraph = new System.Windows.Forms.Button();
            this.cb_DebugView = new System.Windows.Forms.CheckBox();
            this.gb_EditorMode = new System.Windows.Forms.GroupBox();
            this.rb_SetEndMode = new System.Windows.Forms.RadioButton();
            this.rb_SetStartMode = new System.Windows.Forms.RadioButton();
            this.rb_EditMode = new System.Windows.Forms.RadioButton();
            this.rb_RemoveMode = new System.Windows.Forms.RadioButton();
            this.rb_CreateMode = new System.Windows.Forms.RadioButton();
            this.rb_NoneMode = new System.Windows.Forms.RadioButton();
            this.gb_MapRandomFill = new System.Windows.Forms.GroupBox();
            this.btn_MapRandomFill = new System.Windows.Forms.Button();
            this.nud_CountGenerateObstructions = new System.Windows.Forms.NumericUpDown();
            this.gb_MapControl = new System.Windows.Forms.GroupBox();
            this.btn_LoadMap = new System.Windows.Forms.Button();
            this.btn_SaveMap = new System.Windows.Forms.Button();
            this.btn_NewMap = new System.Windows.Forms.Button();
            this.l_MousePosition = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ViewPort)).BeginInit();
            this.gb_Control.SuspendLayout();
            this.gb_Algorithm.SuspendLayout();
            this.gb_EditorMode.SuspendLayout();
            this.gb_MapRandomFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CountGenerateObstructions)).BeginInit();
            this.gb_MapControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerRender
            // 
            this.timerRender.Tick += new System.EventHandler(this.timerRender_Tick);
            // 
            // openMapFileDialog
            // 
            this.openMapFileDialog.FileName = "map";
            // 
            // saveMapFileDialog
            // 
            this.saveMapFileDialog.FileName = "map";
            // 
            // pb_ViewPort
            // 
            this.pb_ViewPort.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_ViewPort.Location = new System.Drawing.Point(3, 0);
            this.pb_ViewPort.Name = "pb_ViewPort";
            this.pb_ViewPort.Size = new System.Drawing.Size(500, 873);
            this.pb_ViewPort.TabIndex = 0;
            this.pb_ViewPort.TabStop = false;
            this.pb_ViewPort.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ModePoint_MouseDown);
            this.pb_ViewPort.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ModePoint_MouseMove);
            // 
            // gb_Control
            // 
            this.gb_Control.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Control.Controls.Add(this.gb_Algorithm);
            this.gb_Control.Controls.Add(this.gb_EditorMode);
            this.gb_Control.Controls.Add(this.gb_MapRandomFill);
            this.gb_Control.Controls.Add(this.gb_MapControl);
            this.gb_Control.Location = new System.Drawing.Point(539, 0);
            this.gb_Control.Name = "gb_Control";
            this.gb_Control.Size = new System.Drawing.Size(163, 399);
            this.gb_Control.TabIndex = 1;
            this.gb_Control.TabStop = false;
            this.gb_Control.Text = "Control";
            // 
            // gb_Algorithm
            // 
            this.gb_Algorithm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Algorithm.Controls.Add(this.btn_RunA);
            this.gb_Algorithm.Controls.Add(this.btn_GenerateFullGraphBenchmark);
            this.gb_Algorithm.Controls.Add(this.btn_RunABenchmark);
            this.gb_Algorithm.Controls.Add(this.btn_GenerateFullGraph);
            this.gb_Algorithm.Controls.Add(this.cb_DebugView);
            this.gb_Algorithm.Location = new System.Drawing.Point(7, 281);
            this.gb_Algorithm.Name = "gb_Algorithm";
            this.gb_Algorithm.Size = new System.Drawing.Size(150, 110);
            this.gb_Algorithm.TabIndex = 8;
            this.gb_Algorithm.TabStop = false;
            this.gb_Algorithm.Text = "Algorithm";
            // 
            // btn_RunA
            // 
            this.btn_RunA.Location = new System.Drawing.Point(9, 47);
            this.btn_RunA.Name = "btn_RunA";
            this.btn_RunA.Size = new System.Drawing.Size(55, 23);
            this.btn_RunA.TabIndex = 4;
            this.btn_RunA.Text = "A*";
            this.btn_RunA.UseVisualStyleBackColor = true;
            this.btn_RunA.Click += new System.EventHandler(this.btn_runA_Click);
            // 
            // btn_GenerateFullGraphBenchmark
            // 
            this.btn_GenerateFullGraphBenchmark.Location = new System.Drawing.Point(72, 76);
            this.btn_GenerateFullGraphBenchmark.Name = "btn_GenerateFullGraphBenchmark";
            this.btn_GenerateFullGraphBenchmark.Size = new System.Drawing.Size(72, 23);
            this.btn_GenerateFullGraphBenchmark.TabIndex = 3;
            this.btn_GenerateFullGraphBenchmark.Text = "Bench Full";
            this.btn_GenerateFullGraphBenchmark.UseVisualStyleBackColor = true;
            this.btn_GenerateFullGraphBenchmark.Click += new System.EventHandler(this.btn_GenerateFullGraphBenchmark_Click);
            // 
            // btn_RunABenchmark
            // 
            this.btn_RunABenchmark.Location = new System.Drawing.Point(9, 76);
            this.btn_RunABenchmark.Name = "btn_RunABenchmark";
            this.btn_RunABenchmark.Size = new System.Drawing.Size(61, 23);
            this.btn_RunABenchmark.TabIndex = 2;
            this.btn_RunABenchmark.Text = "Bench A";
            this.btn_RunABenchmark.UseVisualStyleBackColor = true;
            this.btn_RunABenchmark.Click += new System.EventHandler(this.btn_RunABenchmark_Click);
            // 
            // btn_GenerateFullGraph
            // 
            this.btn_GenerateFullGraph.Location = new System.Drawing.Point(70, 47);
            this.btn_GenerateFullGraph.Name = "btn_GenerateFullGraph";
            this.btn_GenerateFullGraph.Size = new System.Drawing.Size(74, 23);
            this.btn_GenerateFullGraph.TabIndex = 1;
            this.btn_GenerateFullGraph.Text = "Full Graph";
            this.btn_GenerateFullGraph.UseVisualStyleBackColor = true;
            this.btn_GenerateFullGraph.Click += new System.EventHandler(this.btn_GenerateFullGraph_Click);
            // 
            // cb_DebugView
            // 
            this.cb_DebugView.AutoSize = true;
            this.cb_DebugView.Location = new System.Drawing.Point(11, 22);
            this.cb_DebugView.Name = "cb_DebugView";
            this.cb_DebugView.Size = new System.Drawing.Size(86, 19);
            this.cb_DebugView.TabIndex = 0;
            this.cb_DebugView.Text = "DebugView";
            this.cb_DebugView.UseVisualStyleBackColor = true;
            this.cb_DebugView.CheckedChanged += new System.EventHandler(this.cb_DebugView_CheckedChanged);
            // 
            // gb_EditorMode
            // 
            this.gb_EditorMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_EditorMode.Controls.Add(this.rb_SetEndMode);
            this.gb_EditorMode.Controls.Add(this.rb_SetStartMode);
            this.gb_EditorMode.Controls.Add(this.rb_EditMode);
            this.gb_EditorMode.Controls.Add(this.rb_RemoveMode);
            this.gb_EditorMode.Controls.Add(this.rb_CreateMode);
            this.gb_EditorMode.Controls.Add(this.rb_NoneMode);
            this.gb_EditorMode.Location = new System.Drawing.Point(7, 22);
            this.gb_EditorMode.Name = "gb_EditorMode";
            this.gb_EditorMode.Size = new System.Drawing.Size(150, 102);
            this.gb_EditorMode.TabIndex = 0;
            this.gb_EditorMode.TabStop = false;
            this.gb_EditorMode.Text = "EditorMode";
            // 
            // rb_SetEndMode
            // 
            this.rb_SetEndMode.AutoSize = true;
            this.rb_SetEndMode.Location = new System.Drawing.Point(85, 72);
            this.rb_SetEndMode.Name = "rb_SetEndMode";
            this.rb_SetEndMode.Size = new System.Drawing.Size(64, 19);
            this.rb_SetEndMode.TabIndex = 5;
            this.rb_SetEndMode.TabStop = true;
            this.rb_SetEndMode.Text = "Set End";
            this.rb_SetEndMode.UseVisualStyleBackColor = true;
            this.rb_SetEndMode.CheckedChanged += new System.EventHandler(this.rb_SetEnd_CheckedChanged);
            // 
            // rb_SetStartMode
            // 
            this.rb_SetStartMode.AutoSize = true;
            this.rb_SetStartMode.Location = new System.Drawing.Point(11, 72);
            this.rb_SetStartMode.Name = "rb_SetStartMode";
            this.rb_SetStartMode.Size = new System.Drawing.Size(68, 19);
            this.rb_SetStartMode.TabIndex = 4;
            this.rb_SetStartMode.TabStop = true;
            this.rb_SetStartMode.Text = "Set Start";
            this.rb_SetStartMode.UseVisualStyleBackColor = true;
            this.rb_SetStartMode.CheckedChanged += new System.EventHandler(this.rb_SetStart_CheckedChanged);
            // 
            // rb_EditMode
            // 
            this.rb_EditMode.AutoSize = true;
            this.rb_EditMode.Location = new System.Drawing.Point(85, 47);
            this.rb_EditMode.Name = "rb_EditMode";
            this.rb_EditMode.Size = new System.Drawing.Size(45, 19);
            this.rb_EditMode.TabIndex = 3;
            this.rb_EditMode.TabStop = true;
            this.rb_EditMode.Text = "Edit";
            this.rb_EditMode.UseVisualStyleBackColor = true;
            this.rb_EditMode.CheckedChanged += new System.EventHandler(this.rb_EditMode_CheckedChanged);
            // 
            // rb_RemoveMode
            // 
            this.rb_RemoveMode.AutoSize = true;
            this.rb_RemoveMode.Location = new System.Drawing.Point(11, 47);
            this.rb_RemoveMode.Name = "rb_RemoveMode";
            this.rb_RemoveMode.Size = new System.Drawing.Size(68, 19);
            this.rb_RemoveMode.TabIndex = 2;
            this.rb_RemoveMode.TabStop = true;
            this.rb_RemoveMode.Text = "Remove";
            this.rb_RemoveMode.UseVisualStyleBackColor = true;
            this.rb_RemoveMode.CheckedChanged += new System.EventHandler(this.rb_RemoveMode_CheckedChanged);
            // 
            // rb_CreateMode
            // 
            this.rb_CreateMode.AutoSize = true;
            this.rb_CreateMode.Location = new System.Drawing.Point(85, 22);
            this.rb_CreateMode.Name = "rb_CreateMode";
            this.rb_CreateMode.Size = new System.Drawing.Size(59, 19);
            this.rb_CreateMode.TabIndex = 1;
            this.rb_CreateMode.TabStop = true;
            this.rb_CreateMode.Text = "Create";
            this.rb_CreateMode.UseVisualStyleBackColor = true;
            this.rb_CreateMode.CheckedChanged += new System.EventHandler(this.rb_CreateMode_CheckedChanged);
            // 
            // rb_NoneMode
            // 
            this.rb_NoneMode.AutoSize = true;
            this.rb_NoneMode.Location = new System.Drawing.Point(11, 22);
            this.rb_NoneMode.Name = "rb_NoneMode";
            this.rb_NoneMode.Size = new System.Drawing.Size(54, 19);
            this.rb_NoneMode.TabIndex = 0;
            this.rb_NoneMode.TabStop = true;
            this.rb_NoneMode.Text = "None";
            this.rb_NoneMode.UseVisualStyleBackColor = true;
            this.rb_NoneMode.CheckedChanged += new System.EventHandler(this.rb_NoneMode_CheckedChanged);
            // 
            // gb_MapRandomFill
            // 
            this.gb_MapRandomFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_MapRandomFill.Controls.Add(this.btn_MapRandomFill);
            this.gb_MapRandomFill.Controls.Add(this.nud_CountGenerateObstructions);
            this.gb_MapRandomFill.Location = new System.Drawing.Point(7, 219);
            this.gb_MapRandomFill.Name = "gb_MapRandomFill";
            this.gb_MapRandomFill.Size = new System.Drawing.Size(150, 56);
            this.gb_MapRandomFill.TabIndex = 6;
            this.gb_MapRandomFill.TabStop = false;
            this.gb_MapRandomFill.Text = "Random Fill";
            // 
            // btn_MapRandomFill
            // 
            this.btn_MapRandomFill.Location = new System.Drawing.Point(85, 22);
            this.btn_MapRandomFill.Name = "btn_MapRandomFill";
            this.btn_MapRandomFill.Size = new System.Drawing.Size(59, 23);
            this.btn_MapRandomFill.TabIndex = 1;
            this.btn_MapRandomFill.Text = "Fill";
            this.btn_MapRandomFill.UseVisualStyleBackColor = true;
            this.btn_MapRandomFill.Click += new System.EventHandler(this.btn_MapRandomFill_Click);
            // 
            // nud_CountGenerateObstructions
            // 
            this.nud_CountGenerateObstructions.Location = new System.Drawing.Point(11, 22);
            this.nud_CountGenerateObstructions.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nud_CountGenerateObstructions.Name = "nud_CountGenerateObstructions";
            this.nud_CountGenerateObstructions.Size = new System.Drawing.Size(61, 23);
            this.nud_CountGenerateObstructions.TabIndex = 0;
            // 
            // gb_MapControl
            // 
            this.gb_MapControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_MapControl.Controls.Add(this.btn_LoadMap);
            this.gb_MapControl.Controls.Add(this.btn_SaveMap);
            this.gb_MapControl.Controls.Add(this.btn_NewMap);
            this.gb_MapControl.Location = new System.Drawing.Point(7, 130);
            this.gb_MapControl.Name = "gb_MapControl";
            this.gb_MapControl.Size = new System.Drawing.Size(150, 83);
            this.gb_MapControl.TabIndex = 7;
            this.gb_MapControl.TabStop = false;
            this.gb_MapControl.Text = "Map Control";
            // 
            // btn_LoadMap
            // 
            this.btn_LoadMap.Location = new System.Drawing.Point(11, 51);
            this.btn_LoadMap.Name = "btn_LoadMap";
            this.btn_LoadMap.Size = new System.Drawing.Size(61, 23);
            this.btn_LoadMap.TabIndex = 2;
            this.btn_LoadMap.Text = "Load";
            this.btn_LoadMap.UseVisualStyleBackColor = true;
            this.btn_LoadMap.Click += new System.EventHandler(this.btn_LoadMap_Click);
            // 
            // btn_SaveMap
            // 
            this.btn_SaveMap.Location = new System.Drawing.Point(83, 51);
            this.btn_SaveMap.Name = "btn_SaveMap";
            this.btn_SaveMap.Size = new System.Drawing.Size(61, 23);
            this.btn_SaveMap.TabIndex = 1;
            this.btn_SaveMap.Text = "Save";
            this.btn_SaveMap.UseVisualStyleBackColor = true;
            this.btn_SaveMap.Click += new System.EventHandler(this.btn_SaveMap_Click);
            // 
            // btn_NewMap
            // 
            this.btn_NewMap.Location = new System.Drawing.Point(11, 22);
            this.btn_NewMap.Name = "btn_NewMap";
            this.btn_NewMap.Size = new System.Drawing.Size(61, 23);
            this.btn_NewMap.TabIndex = 0;
            this.btn_NewMap.Text = "New";
            this.btn_NewMap.UseVisualStyleBackColor = true;
            this.btn_NewMap.Click += new System.EventHandler(this.btn_NewMap_Click);
            // 
            // l_MousePosition
            // 
            this.l_MousePosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.l_MousePosition.AutoSize = true;
            this.l_MousePosition.Location = new System.Drawing.Point(616, 402);
            this.l_MousePosition.Name = "l_MousePosition";
            this.l_MousePosition.Size = new System.Drawing.Size(86, 15);
            this.l_MousePosition.TabIndex = 2;
            this.l_MousePosition.Text = "MousePosition";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 681);
            this.Controls.Add(this.l_MousePosition);
            this.Controls.Add(this.pb_ViewPort);
            this.Controls.Add(this.gb_Control);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.pb_ViewPort_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pb_ViewPort)).EndInit();
            this.gb_Control.ResumeLayout(false);
            this.gb_Algorithm.ResumeLayout(false);
            this.gb_Algorithm.PerformLayout();
            this.gb_EditorMode.ResumeLayout(false);
            this.gb_EditorMode.PerformLayout();
            this.gb_MapRandomFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_CountGenerateObstructions)).EndInit();
            this.gb_MapControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerRender;
        private OpenFileDialog openMapFileDialog;
        private SaveFileDialog saveMapFileDialog;
        private PictureBox pb_ViewPort;
        private GroupBox gb_Control;
        private GroupBox gb_EditorMode;
        private GroupBox gb_Algorithm;
        private Button btn_GenerateFullGraphBenchmark;
        private Button btn_RunABenchmark;
        private Button btn_GenerateFullGraph;
        private CheckBox cb_DebugView;
        private RadioButton rb_SetEndMode;
        private RadioButton rb_SetStartMode;
        private RadioButton rb_EditMode;
        private RadioButton rb_RemoveMode;
        private RadioButton rb_CreateMode;
        private RadioButton rb_NoneMode;
        private GroupBox gb_MapRandomFill;
        private Button btn_MapRandomFill;
        private NumericUpDown nud_CountGenerateObstructions;
        private GroupBox gb_MapControl;
        private Button btn_LoadMap;
        private Button btn_SaveMap;
        private Button btn_NewMap;
        private Button btn_RunA;
        private Label l_MousePosition;
    }
}