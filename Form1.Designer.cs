
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
            this.newbutton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rb_Mode_Remove = new System.Windows.Forms.RadioButton();
            this.rb_Mode_Create = new System.Windows.Forms.RadioButton();
            this.rb_Mode_None = new System.Windows.Forms.RadioButton();
            this.rb_Mode_Edit = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.rb_set_start = new System.Windows.Forms.RadioButton();
            this.rb_set_end = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // newbutton
            // 
            this.newbutton.Location = new System.Drawing.Point(6, 19);
            this.newbutton.Name = "newbutton";
            this.newbutton.Size = new System.Drawing.Size(69, 21);
            this.newbutton.TabIndex = 0;
            this.newbutton.Text = "New";
            this.newbutton.UseVisualStyleBackColor = true;
            this.newbutton.Click += new System.EventHandler(this.Newbutton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(624, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 210);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.newbutton);
            this.groupBox3.Location = new System.Drawing.Point(11, 117);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(147, 79);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Map";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(81, 47);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(58, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(81, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(58, 22);
            this.button2.TabIndex = 1;
            this.button2.Text = "Load";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Loadbutton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb_set_end);
            this.groupBox2.Controls.Add(this.rb_set_start);
            this.groupBox2.Controls.Add(this.rb_Mode_Remove);
            this.groupBox2.Controls.Add(this.rb_Mode_Create);
            this.groupBox2.Controls.Add(this.rb_Mode_None);
            this.groupBox2.Controls.Add(this.rb_Mode_Edit);
            this.groupBox2.Location = new System.Drawing.Point(11, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(147, 92);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mode";
            // 
            // rb_Mode_Remove
            // 
            this.rb_Mode_Remove.AutoSize = true;
            this.rb_Mode_Remove.Location = new System.Drawing.Point(7, 42);
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
            this.rb_Mode_Create.Location = new System.Drawing.Point(76, 19);
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
            this.rb_Mode_Edit.Location = new System.Drawing.Point(76, 42);
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // rb_set_start
            // 
            this.rb_set_start.AutoSize = true;
            this.rb_set_start.Location = new System.Drawing.Point(6, 65);
            this.rb_set_start.Name = "rb_set_start";
            this.rb_set_start.Size = new System.Drawing.Size(63, 17);
            this.rb_set_start.TabIndex = 5;
            this.rb_set_start.TabStop = true;
            this.rb_set_start.Text = "SetStart";
            this.rb_set_start.UseVisualStyleBackColor = true;
            this.rb_set_start.CheckedChanged += new System.EventHandler(this.rb_set_start_CheckedChanged);
            // 
            // rb_set_end
            // 
            this.rb_set_end.AutoSize = true;
            this.rb_set_end.Location = new System.Drawing.Point(76, 66);
            this.rb_set_end.Name = "rb_set_end";
            this.rb_set_end.Size = new System.Drawing.Size(60, 17);
            this.rb_set_end.TabIndex = 6;
            this.rb_set_end.TabStop = true;
            this.rb_set_end.Text = "SetEnd";
            this.rb_set_end.UseVisualStyleBackColor = true;
            this.rb_set_end.CheckedChanged += new System.EventHandler(this.rb_set_end_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ModePoint_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ModePoint_MouseMove);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button newbutton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rb_Mode_Create;
        private System.Windows.Forms.RadioButton rb_Mode_None;
        private System.Windows.Forms.RadioButton rb_Mode_Edit;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton rb_Mode_Remove;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RadioButton rb_set_end;
        private System.Windows.Forms.RadioButton rb_set_start;
    }
}

