
namespace CircleEditor
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_NewMap = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gb_randomGeneration = new System.Windows.Forms.GroupBox();
            this.numUD_numberCreate = new System.Windows.Forms.NumericUpDown();
            this.bt_createRandom = new System.Windows.Forms.Button();
            this.gb_Algorithms = new System.Windows.Forms.GroupBox();
            this.btn_DrawGraph = new System.Windows.Forms.Button();
            this.bt_runAstar = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_SaveMap = new System.Windows.Forms.Button();
            this.btn_LoadMap = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rb_set_end = new System.Windows.Forms.RadioButton();
            this.rb_set_start = new System.Windows.Forms.RadioButton();
            this.rb_Mode_Remove = new System.Windows.Forms.RadioButton();
            this.rb_Mode_Create = new System.Windows.Forms.RadioButton();
            this.rb_Mode_None = new System.Windows.Forms.RadioButton();
            this.rb_Mode_Edit = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pb_ViewPort = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.gb_randomGeneration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUD_numberCreate)).BeginInit();
            this.gb_Algorithms.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ViewPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_NewMap
            // 
            this.btn_NewMap.Location = new System.Drawing.Point(6, 19);
            this.btn_NewMap.Name = "btn_NewMap";
            this.btn_NewMap.Size = new System.Drawing.Size(69, 21);
            this.btn_NewMap.TabIndex = 0;
            this.btn_NewMap.Text = "New";
            this.btn_NewMap.UseVisualStyleBackColor = true;
            this.btn_NewMap.Click += new System.EventHandler(this.Newbutton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gb_randomGeneration);
            this.groupBox1.Controls.Add(this.gb_Algorithms);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(730, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(163, 362);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control";
            // 
            // gb_randomGeneration
            // 
            this.gb_randomGeneration.Controls.Add(this.numUD_numberCreate);
            this.gb_randomGeneration.Controls.Add(this.bt_createRandom);
            this.gb_randomGeneration.Location = new System.Drawing.Point(7, 127);
            this.gb_randomGeneration.Name = "gb_randomGeneration";
            this.gb_randomGeneration.Size = new System.Drawing.Size(147, 50);
            this.gb_randomGeneration.TabIndex = 2;
            this.gb_randomGeneration.TabStop = false;
            this.gb_randomGeneration.Text = "Random generation";
            // 
            // numUD_numberCreate
            // 
            this.numUD_numberCreate.Location = new System.Drawing.Point(7, 18);
            this.numUD_numberCreate.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numUD_numberCreate.Name = "numUD_numberCreate";
            this.numUD_numberCreate.Size = new System.Drawing.Size(68, 20);
            this.numUD_numberCreate.TabIndex = 9;
            // 
            // bt_createRandom
            // 
            this.bt_createRandom.Location = new System.Drawing.Point(83, 19);
            this.bt_createRandom.Name = "bt_createRandom";
            this.bt_createRandom.Size = new System.Drawing.Size(62, 20);
            this.bt_createRandom.TabIndex = 8;
            this.bt_createRandom.Text = "Create";
            this.bt_createRandom.UseVisualStyleBackColor = true;
            this.bt_createRandom.Click += new System.EventHandler(this.bt_createRandom_Click);
            // 
            // gb_Algorithms
            // 
            this.gb_Algorithms.Controls.Add(this.btn_DrawGraph);
            this.gb_Algorithms.Controls.Add(this.bt_runAstar);
            this.gb_Algorithms.Location = new System.Drawing.Point(7, 268);
            this.gb_Algorithms.Name = "gb_Algorithms";
            this.gb_Algorithms.Size = new System.Drawing.Size(147, 82);
            this.gb_Algorithms.TabIndex = 3;
            this.gb_Algorithms.TabStop = false;
            this.gb_Algorithms.Text = "Algorithms";
            // 
            // btn_DrawGraph
            // 
            this.btn_DrawGraph.Location = new System.Drawing.Point(7, 19);
            this.btn_DrawGraph.Name = "btn_DrawGraph";
            this.btn_DrawGraph.Size = new System.Drawing.Size(69, 23);
            this.btn_DrawGraph.TabIndex = 2;
            this.btn_DrawGraph.Text = "Full Graph";
            this.btn_DrawGraph.UseVisualStyleBackColor = true;
            this.btn_DrawGraph.Click += new System.EventHandler(this.btn_DrawGraph_Click);
            // 
            // bt_runAstar
            // 
            this.bt_runAstar.Location = new System.Drawing.Point(7, 48);
            this.bt_runAstar.Name = "bt_runAstar";
            this.bt_runAstar.Size = new System.Drawing.Size(69, 23);
            this.bt_runAstar.TabIndex = 0;
            this.bt_runAstar.Text = "A*";
            this.bt_runAstar.UseVisualStyleBackColor = true;
            this.bt_runAstar.Click += new System.EventHandler(this.bt_runAstar_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_SaveMap);
            this.groupBox3.Controls.Add(this.btn_LoadMap);
            this.groupBox3.Controls.Add(this.btn_NewMap);
            this.groupBox3.Location = new System.Drawing.Point(7, 183);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(151, 79);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Map";
            // 
            // btn_SaveMap
            // 
            this.btn_SaveMap.Location = new System.Drawing.Point(79, 45);
            this.btn_SaveMap.Name = "btn_SaveMap";
            this.btn_SaveMap.Size = new System.Drawing.Size(66, 23);
            this.btn_SaveMap.TabIndex = 2;
            this.btn_SaveMap.Text = "Save";
            this.btn_SaveMap.UseVisualStyleBackColor = true;
            this.btn_SaveMap.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // btn_LoadMap
            // 
            this.btn_LoadMap.Location = new System.Drawing.Point(6, 46);
            this.btn_LoadMap.Name = "btn_LoadMap";
            this.btn_LoadMap.Size = new System.Drawing.Size(69, 22);
            this.btn_LoadMap.TabIndex = 1;
            this.btn_LoadMap.Text = "Load";
            this.btn_LoadMap.UseVisualStyleBackColor = true;
            this.btn_LoadMap.Click += new System.EventHandler(this.Loadbutton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb_set_end);
            this.groupBox2.Controls.Add(this.rb_set_start);
            this.groupBox2.Controls.Add(this.rb_Mode_Remove);
            this.groupBox2.Controls.Add(this.rb_Mode_Create);
            this.groupBox2.Controls.Add(this.rb_Mode_None);
            this.groupBox2.Controls.Add(this.rb_Mode_Edit);
            this.groupBox2.Location = new System.Drawing.Point(7, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(147, 102);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mode";
            // 
            // rb_set_end
            // 
            this.rb_set_end.AutoSize = true;
            this.rb_set_end.Location = new System.Drawing.Point(75, 78);
            this.rb_set_end.Name = "rb_set_end";
            this.rb_set_end.Size = new System.Drawing.Size(60, 17);
            this.rb_set_end.TabIndex = 6;
            this.rb_set_end.TabStop = true;
            this.rb_set_end.Text = "SetEnd";
            this.rb_set_end.UseVisualStyleBackColor = true;
            this.rb_set_end.CheckedChanged += new System.EventHandler(this.rb_set_end_CheckedChanged);
            // 
            // rb_set_start
            // 
            this.rb_set_start.AutoSize = true;
            this.rb_set_start.Location = new System.Drawing.Point(6, 78);
            this.rb_set_start.Name = "rb_set_start";
            this.rb_set_start.Size = new System.Drawing.Size(63, 17);
            this.rb_set_start.TabIndex = 5;
            this.rb_set_start.TabStop = true;
            this.rb_set_start.Text = "SetStart";
            this.rb_set_start.UseVisualStyleBackColor = true;
            this.rb_set_start.CheckedChanged += new System.EventHandler(this.rb_set_start_CheckedChanged);
            // 
            // rb_Mode_Remove
            // 
            this.rb_Mode_Remove.AutoSize = true;
            this.rb_Mode_Remove.Location = new System.Drawing.Point(6, 42);
            this.rb_Mode_Remove.Name = "rb_Mode_Remove";
            this.rb_Mode_Remove.Size = new System.Drawing.Size(65, 17);
            this.rb_Mode_Remove.TabIndex = 4;
            this.rb_Mode_Remove.TabStop = true;
            this.rb_Mode_Remove.Text = "Remove";
            this.rb_Mode_Remove.UseVisualStyleBackColor = true;
            this.rb_Mode_Remove.CheckedChanged += new System.EventHandler(this.rb_Mode_Remove_CheckedChanged);
            // 
            // rb_Mode_Create
            // 
            this.rb_Mode_Create.AutoSize = true;
            this.rb_Mode_Create.Location = new System.Drawing.Point(75, 19);
            this.rb_Mode_Create.Name = "rb_Mode_Create";
            this.rb_Mode_Create.Size = new System.Drawing.Size(56, 17);
            this.rb_Mode_Create.TabIndex = 3;
            this.rb_Mode_Create.TabStop = true;
            this.rb_Mode_Create.Text = "Create";
            this.rb_Mode_Create.UseVisualStyleBackColor = true;
            this.rb_Mode_Create.CheckedChanged += new System.EventHandler(this.rb_Mode_Create_CheckedChanged);
            // 
            // rb_Mode_None
            // 
            this.rb_Mode_None.AutoSize = true;
            this.rb_Mode_None.Location = new System.Drawing.Point(6, 19);
            this.rb_Mode_None.Name = "rb_Mode_None";
            this.rb_Mode_None.Size = new System.Drawing.Size(51, 17);
            this.rb_Mode_None.TabIndex = 1;
            this.rb_Mode_None.TabStop = true;
            this.rb_Mode_None.Text = "None";
            this.rb_Mode_None.UseVisualStyleBackColor = true;
            this.rb_Mode_None.CheckedChanged += new System.EventHandler(this.rb_Mode_None_CheckedChanged);
            // 
            // rb_Mode_Edit
            // 
            this.rb_Mode_Edit.AutoSize = true;
            this.rb_Mode_Edit.Location = new System.Drawing.Point(75, 42);
            this.rb_Mode_Edit.Name = "rb_Mode_Edit";
            this.rb_Mode_Edit.Size = new System.Drawing.Size(43, 17);
            this.rb_Mode_Edit.TabIndex = 2;
            this.rb_Mode_Edit.TabStop = true;
            this.rb_Mode_Edit.Text = "Edit";
            this.rb_Mode_Edit.UseVisualStyleBackColor = true;
            this.rb_Mode_Edit.CheckedChanged += new System.EventHandler(this.rb_Mode_Edit_CheckedChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pb_ViewPort
            // 
            this.pb_ViewPort.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_ViewPort.Location = new System.Drawing.Point(0, 0);
            this.pb_ViewPort.MinimumSize = new System.Drawing.Size(20, 20);
            this.pb_ViewPort.Name = "pb_ViewPort";
            this.pb_ViewPort.Size = new System.Drawing.Size(720, 720);
            this.pb_ViewPort.TabIndex = 2;
            this.pb_ViewPort.TabStop = false;
            this.pb_ViewPort.SizeChanged += new System.EventHandler(this.pb_ViewPort_SizeChanged);
            this.pb_ViewPort.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ModePoint_MouseDown);
            this.pb_ViewPort.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ModePoint_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 766);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pb_ViewPort);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.gb_randomGeneration.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numUD_numberCreate)).EndInit();
            this.gb_Algorithms.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ViewPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_NewMap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rb_Mode_Create;
        private System.Windows.Forms.RadioButton rb_Mode_None;
        private System.Windows.Forms.RadioButton rb_Mode_Edit;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_SaveMap;
        private System.Windows.Forms.Button btn_LoadMap;
        private System.Windows.Forms.RadioButton rb_Mode_Remove;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RadioButton rb_set_end;
        private System.Windows.Forms.RadioButton rb_set_start;
        private System.Windows.Forms.GroupBox gb_Algorithms;
        private System.Windows.Forms.Button bt_runAstar;
        private System.Windows.Forms.Button bt_createRandom;
        private System.Windows.Forms.GroupBox gb_randomGeneration;
        private System.Windows.Forms.NumericUpDown numUD_numberCreate;
        private System.Windows.Forms.Button btn_DrawGraph;
        private System.Windows.Forms.PictureBox pb_ViewPort;
    }
}

