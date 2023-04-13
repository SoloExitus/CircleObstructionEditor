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
            components = new System.ComponentModel.Container();
            timerRender = new System.Windows.Forms.Timer(components);
            openMapFileDialog = new OpenFileDialog();
            saveMapFileDialog = new SaveFileDialog();
            pb_ViewPort = new PictureBox();
            gb_Control = new GroupBox();
            gb_Debug = new GroupBox();
            ll_MousePosition = new Label();
            l_MousePosition = new Label();
            cb_DebugView = new CheckBox();
            gb_Actor = new GroupBox();
            nud_ActorRadius = new NumericUpDown();
            l_ActorRadius = new Label();
            gb_Algorithm = new GroupBox();
            btn_RunA = new Button();
            btn_GenerateFullGraphBenchmark = new Button();
            btn_RunABenchmark = new Button();
            btn_GenerateFullGraph = new Button();
            gb_EditorMode = new GroupBox();
            rb_SetEndMode = new RadioButton();
            rb_SetStartMode = new RadioButton();
            rb_EditMode = new RadioButton();
            rb_RemoveMode = new RadioButton();
            rb_CreateMode = new RadioButton();
            rb_NoneMode = new RadioButton();
            gb_MapRandomFill = new GroupBox();
            btn_MapRandomFill = new Button();
            nud_CountGenerateObstructions = new NumericUpDown();
            gb_MapControl = new GroupBox();
            btn_LoadMap = new Button();
            btn_SaveMap = new Button();
            btn_NewMap = new Button();
            ((System.ComponentModel.ISupportInitialize)pb_ViewPort).BeginInit();
            gb_Control.SuspendLayout();
            gb_Debug.SuspendLayout();
            gb_Actor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_ActorRadius).BeginInit();
            gb_Algorithm.SuspendLayout();
            gb_EditorMode.SuspendLayout();
            gb_MapRandomFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_CountGenerateObstructions).BeginInit();
            gb_MapControl.SuspendLayout();
            SuspendLayout();
            // 
            // timerRender
            // 
            timerRender.Tick += timerRender_Tick;
            // 
            // openMapFileDialog
            // 
            openMapFileDialog.FileName = "map";
            // 
            // saveMapFileDialog
            // 
            saveMapFileDialog.FileName = "map";
            // 
            // pb_ViewPort
            // 
            pb_ViewPort.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pb_ViewPort.Location = new Point(3, 0);
            pb_ViewPort.Name = "pb_ViewPort";
            pb_ViewPort.Size = new Size(500, 873);
            pb_ViewPort.TabIndex = 0;
            pb_ViewPort.TabStop = false;
            pb_ViewPort.MouseDown += ModePoint_MouseDown;
            pb_ViewPort.MouseMove += ModePoint_MouseMove;
            // 
            // gb_Control
            // 
            gb_Control.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gb_Control.Controls.Add(gb_Debug);
            gb_Control.Controls.Add(gb_Actor);
            gb_Control.Controls.Add(gb_Algorithm);
            gb_Control.Controls.Add(gb_EditorMode);
            gb_Control.Controls.Add(gb_MapRandomFill);
            gb_Control.Controls.Add(gb_MapControl);
            gb_Control.Location = new Point(539, 0);
            gb_Control.Name = "gb_Control";
            gb_Control.Size = new Size(163, 553);
            gb_Control.TabIndex = 1;
            gb_Control.TabStop = false;
            gb_Control.Text = "Control";
            // 
            // gb_Debug
            // 
            gb_Debug.Controls.Add(ll_MousePosition);
            gb_Debug.Controls.Add(l_MousePosition);
            gb_Debug.Controls.Add(cb_DebugView);
            gb_Debug.Location = new Point(7, 370);
            gb_Debug.Name = "gb_Debug";
            gb_Debug.Size = new Size(149, 97);
            gb_Debug.TabIndex = 4;
            gb_Debug.TabStop = false;
            gb_Debug.Text = "Debug";
            // 
            // ll_MousePosition
            // 
            ll_MousePosition.AutoSize = true;
            ll_MousePosition.Location = new Point(11, 44);
            ll_MousePosition.Name = "ll_MousePosition";
            ll_MousePosition.Size = new Size(86, 15);
            ll_MousePosition.TabIndex = 4;
            ll_MousePosition.Text = "MousePosition";
            // 
            // l_MousePosition
            // 
            l_MousePosition.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            l_MousePosition.AutoSize = true;
            l_MousePosition.Location = new Point(11, 69);
            l_MousePosition.Name = "l_MousePosition";
            l_MousePosition.Size = new Size(86, 15);
            l_MousePosition.TabIndex = 2;
            l_MousePosition.Text = "MousePosition";
            // 
            // cb_DebugView
            // 
            cb_DebugView.AutoSize = true;
            cb_DebugView.Location = new Point(11, 22);
            cb_DebugView.Name = "cb_DebugView";
            cb_DebugView.Size = new Size(86, 19);
            cb_DebugView.TabIndex = 0;
            cb_DebugView.Text = "DebugView";
            cb_DebugView.UseVisualStyleBackColor = true;
            cb_DebugView.CheckedChanged += cb_DebugView_CheckedChanged;
            // 
            // gb_Actor
            // 
            gb_Actor.Controls.Add(nud_ActorRadius);
            gb_Actor.Controls.Add(l_ActorRadius);
            gb_Actor.Location = new Point(7, 473);
            gb_Actor.Name = "gb_Actor";
            gb_Actor.Size = new Size(150, 73);
            gb_Actor.TabIndex = 2;
            gb_Actor.TabStop = false;
            gb_Actor.Text = "Actor";
            // 
            // nud_ActorRadius
            // 
            nud_ActorRadius.Location = new Point(11, 40);
            nud_ActorRadius.Name = "nud_ActorRadius";
            nud_ActorRadius.Size = new Size(120, 23);
            nud_ActorRadius.TabIndex = 5;
            nud_ActorRadius.ValueChanged += nud_ActorRadius_ValueChanged;
            // 
            // l_ActorRadius
            // 
            l_ActorRadius.AutoSize = true;
            l_ActorRadius.Location = new Point(11, 22);
            l_ActorRadius.Name = "l_ActorRadius";
            l_ActorRadius.Size = new Size(71, 15);
            l_ActorRadius.TabIndex = 4;
            l_ActorRadius.Text = "ActorRadius";
            // 
            // gb_Algorithm
            // 
            gb_Algorithm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gb_Algorithm.Controls.Add(btn_RunA);
            gb_Algorithm.Controls.Add(btn_GenerateFullGraphBenchmark);
            gb_Algorithm.Controls.Add(btn_RunABenchmark);
            gb_Algorithm.Controls.Add(btn_GenerateFullGraph);
            gb_Algorithm.Location = new Point(7, 281);
            gb_Algorithm.Name = "gb_Algorithm";
            gb_Algorithm.Size = new Size(150, 83);
            gb_Algorithm.TabIndex = 8;
            gb_Algorithm.TabStop = false;
            gb_Algorithm.Text = "Algorithm";
            // 
            // btn_RunA
            // 
            btn_RunA.Location = new Point(11, 22);
            btn_RunA.Name = "btn_RunA";
            btn_RunA.Size = new Size(55, 23);
            btn_RunA.TabIndex = 4;
            btn_RunA.Text = "A*";
            btn_RunA.UseVisualStyleBackColor = true;
            btn_RunA.Click += btn_runA_Click;
            // 
            // btn_GenerateFullGraphBenchmark
            // 
            btn_GenerateFullGraphBenchmark.Location = new Point(74, 51);
            btn_GenerateFullGraphBenchmark.Name = "btn_GenerateFullGraphBenchmark";
            btn_GenerateFullGraphBenchmark.Size = new Size(72, 23);
            btn_GenerateFullGraphBenchmark.TabIndex = 3;
            btn_GenerateFullGraphBenchmark.Text = "Bench Full";
            btn_GenerateFullGraphBenchmark.UseVisualStyleBackColor = true;
            btn_GenerateFullGraphBenchmark.Click += btn_GenerateFullGraphBenchmark_Click;
            // 
            // btn_RunABenchmark
            // 
            btn_RunABenchmark.Location = new Point(11, 51);
            btn_RunABenchmark.Name = "btn_RunABenchmark";
            btn_RunABenchmark.Size = new Size(61, 23);
            btn_RunABenchmark.TabIndex = 2;
            btn_RunABenchmark.Text = "Bench A";
            btn_RunABenchmark.UseVisualStyleBackColor = true;
            btn_RunABenchmark.Click += btn_RunABenchmark_Click;
            // 
            // btn_GenerateFullGraph
            // 
            btn_GenerateFullGraph.Location = new Point(72, 22);
            btn_GenerateFullGraph.Name = "btn_GenerateFullGraph";
            btn_GenerateFullGraph.Size = new Size(74, 23);
            btn_GenerateFullGraph.TabIndex = 1;
            btn_GenerateFullGraph.Text = "Full Graph";
            btn_GenerateFullGraph.UseVisualStyleBackColor = true;
            btn_GenerateFullGraph.Click += btn_GenerateFullGraph_Click;
            // 
            // gb_EditorMode
            // 
            gb_EditorMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gb_EditorMode.Controls.Add(rb_SetEndMode);
            gb_EditorMode.Controls.Add(rb_SetStartMode);
            gb_EditorMode.Controls.Add(rb_EditMode);
            gb_EditorMode.Controls.Add(rb_RemoveMode);
            gb_EditorMode.Controls.Add(rb_CreateMode);
            gb_EditorMode.Controls.Add(rb_NoneMode);
            gb_EditorMode.Location = new Point(7, 22);
            gb_EditorMode.Name = "gb_EditorMode";
            gb_EditorMode.Size = new Size(150, 102);
            gb_EditorMode.TabIndex = 0;
            gb_EditorMode.TabStop = false;
            gb_EditorMode.Text = "EditorMode";
            // 
            // rb_SetEndMode
            // 
            rb_SetEndMode.AutoSize = true;
            rb_SetEndMode.Location = new Point(85, 72);
            rb_SetEndMode.Name = "rb_SetEndMode";
            rb_SetEndMode.Size = new Size(64, 19);
            rb_SetEndMode.TabIndex = 5;
            rb_SetEndMode.TabStop = true;
            rb_SetEndMode.Text = "Set End";
            rb_SetEndMode.UseVisualStyleBackColor = true;
            rb_SetEndMode.CheckedChanged += rb_SetEnd_CheckedChanged;
            // 
            // rb_SetStartMode
            // 
            rb_SetStartMode.AutoSize = true;
            rb_SetStartMode.Location = new Point(11, 72);
            rb_SetStartMode.Name = "rb_SetStartMode";
            rb_SetStartMode.Size = new Size(68, 19);
            rb_SetStartMode.TabIndex = 4;
            rb_SetStartMode.TabStop = true;
            rb_SetStartMode.Text = "Set Start";
            rb_SetStartMode.UseVisualStyleBackColor = true;
            rb_SetStartMode.CheckedChanged += rb_SetStart_CheckedChanged;
            // 
            // rb_EditMode
            // 
            rb_EditMode.AutoSize = true;
            rb_EditMode.Location = new Point(85, 47);
            rb_EditMode.Name = "rb_EditMode";
            rb_EditMode.Size = new Size(45, 19);
            rb_EditMode.TabIndex = 3;
            rb_EditMode.TabStop = true;
            rb_EditMode.Text = "Edit";
            rb_EditMode.UseVisualStyleBackColor = true;
            rb_EditMode.CheckedChanged += rb_EditMode_CheckedChanged;
            // 
            // rb_RemoveMode
            // 
            rb_RemoveMode.AutoSize = true;
            rb_RemoveMode.Location = new Point(11, 47);
            rb_RemoveMode.Name = "rb_RemoveMode";
            rb_RemoveMode.Size = new Size(68, 19);
            rb_RemoveMode.TabIndex = 2;
            rb_RemoveMode.TabStop = true;
            rb_RemoveMode.Text = "Remove";
            rb_RemoveMode.UseVisualStyleBackColor = true;
            rb_RemoveMode.CheckedChanged += rb_RemoveMode_CheckedChanged;
            // 
            // rb_CreateMode
            // 
            rb_CreateMode.AutoSize = true;
            rb_CreateMode.Location = new Point(85, 22);
            rb_CreateMode.Name = "rb_CreateMode";
            rb_CreateMode.Size = new Size(59, 19);
            rb_CreateMode.TabIndex = 1;
            rb_CreateMode.TabStop = true;
            rb_CreateMode.Text = "Create";
            rb_CreateMode.UseVisualStyleBackColor = true;
            rb_CreateMode.CheckedChanged += rb_CreateMode_CheckedChanged;
            // 
            // rb_NoneMode
            // 
            rb_NoneMode.AutoSize = true;
            rb_NoneMode.Location = new Point(11, 22);
            rb_NoneMode.Name = "rb_NoneMode";
            rb_NoneMode.Size = new Size(54, 19);
            rb_NoneMode.TabIndex = 0;
            rb_NoneMode.TabStop = true;
            rb_NoneMode.Text = "None";
            rb_NoneMode.UseVisualStyleBackColor = true;
            rb_NoneMode.CheckedChanged += rb_NoneMode_CheckedChanged;
            // 
            // gb_MapRandomFill
            // 
            gb_MapRandomFill.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gb_MapRandomFill.Controls.Add(btn_MapRandomFill);
            gb_MapRandomFill.Controls.Add(nud_CountGenerateObstructions);
            gb_MapRandomFill.Location = new Point(7, 219);
            gb_MapRandomFill.Name = "gb_MapRandomFill";
            gb_MapRandomFill.Size = new Size(150, 56);
            gb_MapRandomFill.TabIndex = 6;
            gb_MapRandomFill.TabStop = false;
            gb_MapRandomFill.Text = "Random Fill";
            // 
            // btn_MapRandomFill
            // 
            btn_MapRandomFill.Location = new Point(85, 22);
            btn_MapRandomFill.Name = "btn_MapRandomFill";
            btn_MapRandomFill.Size = new Size(59, 23);
            btn_MapRandomFill.TabIndex = 1;
            btn_MapRandomFill.Text = "Fill";
            btn_MapRandomFill.UseVisualStyleBackColor = true;
            btn_MapRandomFill.Click += btn_MapRandomFill_Click;
            // 
            // nud_CountGenerateObstructions
            // 
            nud_CountGenerateObstructions.Location = new Point(11, 22);
            nud_CountGenerateObstructions.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nud_CountGenerateObstructions.Name = "nud_CountGenerateObstructions";
            nud_CountGenerateObstructions.Size = new Size(61, 23);
            nud_CountGenerateObstructions.TabIndex = 0;
            // 
            // gb_MapControl
            // 
            gb_MapControl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gb_MapControl.Controls.Add(btn_LoadMap);
            gb_MapControl.Controls.Add(btn_SaveMap);
            gb_MapControl.Controls.Add(btn_NewMap);
            gb_MapControl.Location = new Point(7, 130);
            gb_MapControl.Name = "gb_MapControl";
            gb_MapControl.Size = new Size(150, 83);
            gb_MapControl.TabIndex = 7;
            gb_MapControl.TabStop = false;
            gb_MapControl.Text = "Map Control";
            // 
            // btn_LoadMap
            // 
            btn_LoadMap.Location = new Point(11, 51);
            btn_LoadMap.Name = "btn_LoadMap";
            btn_LoadMap.Size = new Size(61, 23);
            btn_LoadMap.TabIndex = 2;
            btn_LoadMap.Text = "Load";
            btn_LoadMap.UseVisualStyleBackColor = true;
            btn_LoadMap.Click += btn_LoadMap_Click;
            // 
            // btn_SaveMap
            // 
            btn_SaveMap.Location = new Point(83, 51);
            btn_SaveMap.Name = "btn_SaveMap";
            btn_SaveMap.Size = new Size(61, 23);
            btn_SaveMap.TabIndex = 1;
            btn_SaveMap.Text = "Save";
            btn_SaveMap.UseVisualStyleBackColor = true;
            btn_SaveMap.Click += btn_SaveMap_Click;
            // 
            // btn_NewMap
            // 
            btn_NewMap.Location = new Point(11, 22);
            btn_NewMap.Name = "btn_NewMap";
            btn_NewMap.Size = new Size(61, 23);
            btn_NewMap.TabIndex = 0;
            btn_NewMap.Text = "New";
            btn_NewMap.UseVisualStyleBackColor = true;
            btn_NewMap.Click += btn_NewMap_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 681);
            Controls.Add(pb_ViewPort);
            Controls.Add(gb_Control);
            Name = "MainForm";
            Text = "Form1";
            Load += MainForm_Load;
            Resize += pb_ViewPort_SizeChanged;
            ((System.ComponentModel.ISupportInitialize)pb_ViewPort).EndInit();
            gb_Control.ResumeLayout(false);
            gb_Debug.ResumeLayout(false);
            gb_Debug.PerformLayout();
            gb_Actor.ResumeLayout(false);
            gb_Actor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_ActorRadius).EndInit();
            gb_Algorithm.ResumeLayout(false);
            gb_EditorMode.ResumeLayout(false);
            gb_EditorMode.PerformLayout();
            gb_MapRandomFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nud_CountGenerateObstructions).EndInit();
            gb_MapControl.ResumeLayout(false);
            ResumeLayout(false);
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
        private GroupBox gb_Debug;
        public Label ll_MousePosition;
        private GroupBox gb_Actor;
        private Label l_ActorRadius;
        private NumericUpDown nud_ActorRadius;
    }
}