using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Xml.Linq;

namespace CircleEditor
{

    public partial class Form1 : Form
    {

        enum Mode
        {
            None = 0,
            Create,
            Creating,
            Edit,
            Moving,
            Editing,
            Remove
        }


        class Circle
        {

            public PointF m_center;
            public float m_radius = 0;

            public Circle()
            {
                m_center = new PointF();
            }

            public Circle(PointF center)
            {
                m_center = center;
            }

            public Circle(PointF center, float radius)
            {
                m_center = center;
                m_radius = radius;
            }

            public void setRadius(float radius)
            {
                m_radius = radius;
            }

            private float distance(PointF a, PointF b)
            {
                float dX = a.X - b.X;
                float dY = a.Y - b.Y;
                return (float)Math.Sqrt((double)(dX * dX + dY * dY));
            }

            public void setRadius(PointF e)
            {
                m_radius = distance(m_center, e);
            }

            public void setCenter(PointF center)
            {
                m_center = center;
            }


        }



        Mode m_currentMode = Mode.None;

        Graphics G;
        bool m_isUpdate = false;

        List<Circle> m_Obstructions;

        int m_creatingIndex = -1;
        int m_editingIndex = -1;

        System.Drawing.SolidBrush m_obstructionsBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_currentMode = Mode.None;

            openFileDialog1.Filter = "Text files(*.xml)|*.xml|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.xml)|*.xml|All files(*.*)|*.*";

            G = CreateGraphics();

            m_Obstructions = new List<Circle>();

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_isUpdate)
            {
                G.Clear(Color.White);
                Draw();
                m_isUpdate = false;
            }
        }

        private void Draw()
        {
            foreach (Circle obstruction in m_Obstructions)
            {
                PointF center = obstruction.m_center;
                float radius = obstruction.m_radius;
                G.FillEllipse(m_obstructionsBrush, center.X - radius, center.Y - radius, radius * 2, radius * 2);
            }
        }

        private void rb_Mode_Remove_CheckedChanged(object sender, EventArgs e)
        {
            m_currentMode = Mode.Remove;
        }

        private void rb_Mode_Edit_CheckedChanged(object sender, EventArgs e)
        {
            m_currentMode = Mode.Edit;
        }

        private void rb_Mode_None_CheckedChanged(object sender, EventArgs e)
        {
            m_currentMode = Mode.None;
        }

        private void rb_Mode_Create_CheckedChanged(object sender, EventArgs e)
        {
            m_currentMode = Mode.Create;
        }

        private float distance(PointF a, PointF b)
        {
            float dX = a.X - b.X;
            float dY = a.Y - b.Y;
            return (float)Math.Sqrt((double)(dX * dX + dY * dY));
        }

        private int TryGrabObstruction(PointF location)
        {
            int count = m_Obstructions.Count();

            int index = -1;
            float minDistToObstruction = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                float dist = distance(m_Obstructions[i].m_center, location);
                if (dist < minDistToObstruction && dist < m_Obstructions[i].m_radius)
                {
                    minDistToObstruction = dist;
                    index = i;
                }
            }
            return index;
        }

        private void ModePoint_MouseMove(object sender, MouseEventArgs e)
        {
            switch (m_currentMode)
            {
                case Mode.Creating:
                    m_Obstructions[m_creatingIndex].setRadius(new PointF(e.Location.X, e.Location.Y));
                    m_isUpdate = true;
                    break;
                case Mode.Moving:
                    m_Obstructions[m_editingIndex].setCenter(new PointF(e.Location.X, e.Location.Y));
                    m_isUpdate = true;
                    break;
                case Mode.Editing:
                    m_Obstructions[m_editingIndex].setRadius(new PointF(e.Location.X, e.Location.Y));
                    m_isUpdate = true;
                    break;

            }
        }

        private void ModePoint_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (m_currentMode)
                {
                    case Mode.Create:
                        m_creatingIndex = m_Obstructions.Count();
                        m_Obstructions.Add(new Circle(new PointF(e.Location.X, e.Location.Y)));
                        m_currentMode = Mode.Creating;
                        break;
                    case Mode.Creating:
                        m_Obstructions[m_creatingIndex].setRadius(new PointF(e.Location.X, e.Location.Y));
                        m_creatingIndex = -1;
                        m_currentMode = Mode.Create;
                        m_isUpdate = true;
                        break;
                    case Mode.Remove:
                        int removeIndex = TryGrabObstruction(new PointF(e.Location.X, e.Location.Y));
                        if (removeIndex > -1)
                        {
                            m_Obstructions.Remove(m_Obstructions[removeIndex]);
                        }
                        m_isUpdate = true;
                        break;
                    case Mode.Edit:
                        m_editingIndex = TryGrabObstruction(new PointF(e.Location.X, e.Location.Y));
                        if (m_editingIndex > -1)
                        {
                            m_currentMode = Mode.Moving;
                        }

                        break;
                    case Mode.Moving:
                        m_Obstructions[m_editingIndex].setCenter(new PointF(e.Location.X, e.Location.Y));
                        m_editingIndex = -1;
                        m_currentMode = Mode.Edit;
                        m_isUpdate = true;
                        break;

                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                switch (m_currentMode)
                {
                    case Mode.Edit:
                        m_editingIndex = TryGrabObstruction(new PointF(e.Location.X, e.Location.Y));
                        if (m_editingIndex > -1)
                        {
                            m_currentMode = Mode.Editing;
                        }
                        break;
                    case Mode.Editing:
                        m_Obstructions[m_editingIndex].setRadius(new PointF(e.Location.X, e.Location.Y));
                        m_currentMode = Mode.Edit;
                        m_isUpdate = true;
                        break;


                }

            }
        }

        private void Newbutton_Click(object sender, EventArgs e)
        {
            m_Obstructions.Clear();
            m_isUpdate = true;
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = saveFileDialog1.FileName;

            XDocument map = new XDocument();
            
            XElement obstructions = new XElement("obstructions");

            foreach(Circle obstruct in m_Obstructions)
            {
                XElement obstruction = new XElement("obstruction");

                XElement centerPointElement = new XElement("center");
                XElement radiusElement = new XElement("radius");

                XAttribute centerX = new XAttribute("X", obstruct.m_center.X.ToString());
                XAttribute centerY = new XAttribute("Y", obstruct.m_center.Y.ToString());

                XAttribute radius = new XAttribute("radius", obstruct.m_radius);

                centerPointElement.Add(centerX);
                centerPointElement.Add(centerY);

                radiusElement.Add(radius);

                obstruction.Add(centerPointElement);
                obstruction.Add(radiusElement);

                obstructions.Add(obstruction);
            }

            obstructions.Save(filename);
        }

        private void Loadbutton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = openFileDialog1.FileName;
            XDocument xdoc = XDocument.Load(filename);

            m_Obstructions.Clear();

            foreach (XElement obstructionElement in xdoc.Element("obstructions").Elements("obstruction"))
            {
                XElement centerPointElement = obstructionElement.Element("center");
                XElement radiusElement = obstructionElement.Element("radius");

                XAttribute centerX = centerPointElement.Attribute("X");
                XAttribute centerY = centerPointElement.Attribute("Y");
                XAttribute radius = radiusElement.Attribute("radius");

                if (centerX != null && centerY != null && radius != null)
                {
                    PointF c = new PointF((float)centerX, (float)centerY);
                    float r = float.Parse(radius.Value);
                    //float r = float.Parse(radius.Value);
                    Circle obstruction = new Circle(c, r);
                    m_Obstructions.Add(obstruction);
                }
            }

            m_isUpdate = true;
        }
    }
}

