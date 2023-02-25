using BaseStruct;
using System.Diagnostics;

namespace CircleEditorForm
{
    public partial class MainForm : Form
    {
        Graphics G = null;
        Bitmap Image;
        bool m_isUpdate = false;

        CircleEditor Editor = new CircleEditor();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = "Редактор циркулярных препятствий";

            openMapFileDialog.FileName = "Map.xml";
            saveMapFileDialog.FileName = "NewMap.xml";
            openMapFileDialog.Filter = "Text files(*.xml)|*.xml|All files(*.*)|*.*";
            saveMapFileDialog.Filter = "Text files(*.xml)|*.xml|All files(*.*)|*.*";

            timerRender.Interval = 18;

            Image = new Bitmap(pb_ViewPort.Width, pb_ViewPort.Height);
            G = Graphics.FromImage(Image);
            G.Clear(Color.White);

            m_isUpdate = true;
            timerRender.Start();
        }

        private void Render()
        {
            pb_ViewPort.Image?.Dispose();
            pb_ViewPort.Image = (Bitmap)Image.Clone();
            G.Clear(Color.White);
        }

        private void timerRender_Tick(object sender, EventArgs e)
        {
            if (m_isUpdate)
            {
                Editor.Draw(ref G);
                Render();
                m_isUpdate = false;
            }
        }

        private void rb_RemoveMode_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.Remove;
        }

        private void rb_EditMode_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.Edit;
        }

        private void rb_NoneMode_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.None;
        }

        private void rb_CreateMode_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.Create;
        }

        private void rb_SetStart_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.SetStart;
        }

        private void rb_SetEnd_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.SetEnd;
        }

        private void ModePoint_MouseMove(object sender, MouseEventArgs e)
        {
            Editor.MouseMove(new PointF((float)e.Location.X, (float)e.Location.Y));
            m_isUpdate = true;
        }

        private void ModePoint_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Editor.MouseDownLeft(new PointF((float)e.X, (float)e.Y));
            }
            else if (e.Button == MouseButtons.Right)
            {
                Editor.MouseDownRight(new PointF((float)e.Location.X, (float)e.Location.Y));
            }
            m_isUpdate = true;
        }

        private void btn_NewMap_Click(object sender, EventArgs e)
        {
            Editor.Clear();
            m_isUpdate = true;
        }

        private void btn_SaveMap_Click(object sender, EventArgs e)
        {
            if (saveMapFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = saveMapFileDialog.FileName;

            Editor.Save_to_file(filename);
        }

        private void btn_LoadMap_Click(object sender, EventArgs e)
        {
            if (openMapFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = openMapFileDialog.FileName;
            Editor.Load_from_file(filename);

            m_isUpdate = true;
        }

        private void btn_MapRandomFill_Click(object sender, EventArgs e)
        {
            int createNum = (int)(nud_CountGenerateObstructions.Value);

            int max_X = pb_ViewPort.Width;
            int max_Y = pb_ViewPort.Height;

            Random rnd = new Random();

            int count = 0;
            do
            {

                float center_X = rnd.Next(0, max_X + 1);
                float center_Y = rnd.Next(0, max_Y + 1);

                float radius = rnd.Next(1, (int)(max_Y / 2) + 1);

                Circle newObstruct = new Circle(new PointF(center_X, center_Y), radius);

                if (Editor.AddObstruction(ref newObstruct))
                    count++;

            } while(count < createNum);

            m_isUpdate = true;
        }

        private void btn_runA_Click(object sender, EventArgs e)
        {
            // Запускаем алгорим А* 
            Editor.RunA();
            m_isUpdate = true;
        }

        private void btn_GenerateFullGraph_Click(object sender, EventArgs e)
        {
            // Сгенерировать весь граф целиком
            Editor.GenerateFullGraph();
            m_isUpdate = true;
        }

        private void pb_ViewPort_SizeChanged(object sender, EventArgs e)
        {
            if (pb_ViewPort.Width > 0 && pb_ViewPort.Height > 0)
                Image = new Bitmap(pb_ViewPort.Width, pb_ViewPort.Height);

            G = Graphics.FromImage(Image);
            m_isUpdate = true;
        }

        private void cb_DebugView_CheckedChanged(object sender, EventArgs e)
        {
            Editor.SetDebugMode(cb_DebugView.Checked);
            m_isUpdate = true;
        }

        private void btn_GenerateFullGraphBenchmark_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            int count = 5;
            sw.Start();
            for (int i = 0; i < count; ++i)
            {
                Editor.GenerateFullGraph();
                Editor.RunA();
                Editor.ResetMap();
            }
            sw.Stop();
            double res = sw.ElapsedMilliseconds / count;

            MessageBox.Show($"Full graph generate time: {res}", "Full graph benchmark");
        }

        private void btn_RunABenchmark_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            int count = 5;
            sw.Start();
            for (int i = 0; i < count; ++i)
            {
                Editor.RunA();
                Editor.ResetMap();
            }
            sw.Stop();
            double res = sw.ElapsedMilliseconds / count;

            MessageBox.Show($"Step by step graph generate time: {res}", "A* benchmark");
        }

    }

}

