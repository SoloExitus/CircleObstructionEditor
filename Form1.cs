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

using System.Globalization;

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
            Remove,
            SetStart,
            SetEnd,
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

        class Edge
        {
            public PointF m_first;
            public PointF m_second;

            public Edge(float fx, float fy, float sx, float sy)
            {
                this.m_first = new PointF(fx, fy);
                this.m_second = new PointF(sx, sy);
            }

            public Edge(PointF f, PointF s)
            {
                this.m_first = f;
                this.m_second = s;
            }
        }

        Mode m_currentMode = Mode.None;

        Graphics G;
        bool m_isUpdate = false;

        bool m_isStartEntered = false;
        bool m_isEndEntered = false;

        PointF m_startPoint;
        PointF m_endPoint;

        List<Circle> m_Obstructions;

        int m_creatingIndex = -1;
        int m_editingIndex = -1;

        System.Drawing.SolidBrush m_obstructionsBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);

        System.Drawing.SolidBrush m_startPointBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
        System.Drawing.SolidBrush m_endPointBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            DrawStartAndEndPoints();

            DrawObstructions();
        }

        private void DrawStartAndEndPoints()
        {
            int PointsRadius = 4;

            if (m_isStartEntered)
                G.FillEllipse(m_startPointBrush, m_startPoint.X - PointsRadius, m_startPoint.Y - PointsRadius, PointsRadius * 2, PointsRadius * 2);

            if (m_isEndEntered)
                G.FillEllipse(m_endPointBrush, m_endPoint.X - PointsRadius, m_endPoint.Y - PointsRadius, PointsRadius * 2, PointsRadius * 2);
        }

        private void DrawObstructions()
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

        private void rb_set_start_CheckedChanged(object sender, EventArgs e)
        {
            m_currentMode = Mode.SetStart;
        }

        private void rb_set_end_CheckedChanged(object sender, EventArgs e)
        {
            m_currentMode = Mode.SetEnd;
        }

        private float SquareDistance(PointF a, PointF b)
        {
            float dX = a.X - b.X;
            float dY = a.Y - b.Y;
            return (dX * dX + dY * dY);
        }

        private float distance(PointF a, PointF b)
        {
            float dX = a.X - b.X;
            float dY = a.Y - b.Y;
            return (float)Math.Sqrt((double)SquareDistance(a, b));
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
                    case Mode.SetStart:
                        m_startPoint = new PointF(e.Location.X, e.Location.Y);
                        m_isStartEntered = true;
                        m_isUpdate = true;
                        break;
                    case Mode.SetEnd:
                        m_endPoint = new PointF(e.Location.X, e.Location.Y);
                        m_isEndEntered = true;
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
            m_isStartEntered = false;
            m_isEndEntered = false;
            m_isUpdate = true;
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = saveFileDialog1.FileName;
            
            XElement map = new XElement("map");

            XElement obstructions = new XElement("obstructions");

            foreach (Circle obstruct in m_Obstructions)
            {
                XElement obstruction = new XElement("obstruction");

                XElement centerPointElement = new XElement("center");
                XElement radiusElement = new XElement("radius");

                XAttribute centerX = new XAttribute("x", obstruct.m_center.X.ToString());
                XAttribute centerY = new XAttribute("y", obstruct.m_center.Y.ToString());

                XAttribute radius = new XAttribute("radius", obstruct.m_radius);

                centerPointElement.Add(centerX);
                centerPointElement.Add(centerY);

                radiusElement.Add(radius);

                obstruction.Add(centerPointElement);
                obstruction.Add(radiusElement);

                obstructions.Add(obstruction);
            }

            if (m_isStartEntered)
            {
                XElement start = new XElement("start");

                XAttribute startX = new XAttribute("x", m_startPoint.X.ToString());
                XAttribute startY = new XAttribute("y", m_startPoint.Y.ToString());

                start.Add(startX);
                start.Add(startY);

                map.Add(start);
            }

            if (m_isEndEntered)
            {
                XElement end = new XElement("end");

                XAttribute endX = new XAttribute("x", m_endPoint.X);
                XAttribute endY = new XAttribute("y", m_endPoint.Y);

                end.Add(endX);
                end.Add(endY);

                map.Add(end);
            }

            map.Add(obstructions);

            map.Save(filename);
        }

        private void Loadbutton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = openFileDialog1.FileName;
            XDocument xdoc = XDocument.Load(filename);
            XElement loadMap = xdoc.Element("map");

            m_Obstructions.Clear();

            XElement start = loadMap.Element("start");
            XElement end = loadMap.Element("end");

            if (start != null)
            {
                XAttribute startX = start.Attribute("x");
                XAttribute startY = start.Attribute("y");

                if (startX != null && startY != null)
                {
                    m_startPoint = new PointF((float)startX, (float)startY);
                    m_isStartEntered = true;
                }
            }

            if (end != null)
            {
                XAttribute endX = end.Attribute("x");
                XAttribute endY = end.Attribute("y");

                if (endX != null && endY !=null)
                {
                    m_endPoint = new PointF((float)endX, (float)endY);
                    m_isEndEntered = true;
                }
            }

            XElement obstructions = loadMap.Element("obstructions");

            foreach (XElement obstructionElement in obstructions.Elements("obstruction"))
            {
                XElement centerPointElement = obstructionElement.Element("center");
                XElement radiusElement = obstructionElement.Element("radius");

                if (centerPointElement != null && radiusElement != null)
                {
                    XAttribute centerX = centerPointElement.Attribute("x");
                    XAttribute centerY = centerPointElement.Attribute("y");
                    XAttribute radius = radiusElement.Attribute("radius");

                    if (centerX != null && centerY != null && radius != null)
                    {
                        PointF c = new PointF((float)centerX, (float)centerY);
                        float r = float.Parse(radius.Value, CultureInfo.InvariantCulture.NumberFormat);

                        Circle obstruction = new Circle(c, r);

                        m_Obstructions.Add(obstruction);
                    }
                }
            }

            m_isUpdate = true;
        }

        private static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        private bool is_Block(Circle obstruct, PointF A, PointF B)
        {
            PointF C = obstruct.m_center;

            float u = ((C.X - A.X)*(B.X - A.X) + (C.Y - A.Y)*(B.Y - A.Y)) / SquareDistance(B,A);

            float Ex = A.X + Clamp(u, 0, 1) * (B.X - A.X);
            float Ey = A.Y + Clamp(u, 0, 1) * (B.Y - A.Y);

            PointF E = new PointF(Ex, Ey);

            float Sd = SquareDistance(E, C);

            return Sd < (obstruct.m_radius * obstruct.m_radius);
        }

        private float norm(PointF vect)
        {
            return (float)Math.Sqrt((double)vect.X * vect.X + vect.Y * vect.Y);
        }

        private PointF rotatePoint(PointF p, PointF c, float angle)
        {
            float nX = (float)( c.X + (p.X - c.X) * Math.Cos((double)angle) - (p.Y - c.Y) * Math.Sin((double)angle));
            float nY = (float)( c.Y + (p.X - c.X) * Math.Sin((double)angle) + (p.Y - c.Y) * Math.Cos((double)angle));

            return new PointF(nX, nY);
        }

        private bool IsObstructionsBlock()
        {
            bool block = false;

            foreach (Circle obstruct in m_Obstructions)
            {
                if (is_Block(obstruct, m_startPoint, m_endPoint))
                {
                    block = true;
                    break;
                }
            }

            Pen pen = new Pen(Color.Green);


            if (block)
            {
                pen = new Pen(Color.Red);
            }

            G.DrawLine(pen, m_startPoint, m_endPoint);
            return block;
        }

        private void Test_Click(object sender, EventArgs e)
        {
            List<Edge> intern = externalTangents(m_Obstructions[0], m_Obstructions[1]);
            G.FillEllipse(m_startPointBrush, intern[0].m_first.X - 4, intern[0].m_first.Y - 4, 8, 8);
            G.FillEllipse(m_startPointBrush, intern[0].m_second.X - 4, intern[0].m_second.Y - 4, 8, 8);

            G.FillEllipse(m_endPointBrush, intern[1].m_first.X - 4, intern[1].m_first.Y - 4, 8, 8);
            G.FillEllipse(m_endPointBrush, intern[1].m_second.X - 4, intern[1].m_second.Y - 4, 8, 8);
        }
        private List<Edge> internalTangents(Circle a, Circle b)
        {
            PointF centerA = a.m_center;
            PointF centerB = b.m_center;

            float rA = a.m_radius;
            float rB = b.m_radius;

            float Q = (float)Math.Acos((double)(rA + rB) / distance(centerA, centerB));

            float vectABX = centerB.X - centerA.X;
            float vectABY = centerB.Y - centerA.Y;

            PointF VectAB = new PointF(vectABX, vectABY);

            float vectABNorm = norm(VectAB);

            VectAB.X /= vectABNorm;
            VectAB.Y /= vectABNorm;


            PointF G = new PointF(centerA.X + rA* VectAB.X, centerA.Y + rA* VectAB.Y);

            PointF H = new PointF(centerB.X + rB* (-VectAB.X), centerB.Y + rB* (-VectAB.Y));

            PointF C = rotatePoint(G, centerA, -Q);
            PointF D = rotatePoint(G, centerA, Q);

            PointF E = rotatePoint(H, centerB, Q);
            PointF F = rotatePoint(H, centerB, -Q);

            List<Edge> list = new List<Edge>();
            list.Add(new Edge(C,F));
            list.Add(new Edge(D,E));
            
            return list;
        }

        private List<Edge> externalTangents(Circle a, Circle b)
        {
            PointF centerA = a.m_center;
            PointF centerB = b.m_center;

            float rA = a.m_radius;
            float rB = b.m_radius;

            float Q = (float)Math.Acos((double)Math.Abs(rA - rB) / distance(centerA, centerB));

            float vectABX = centerB.X - centerA.X;
            float vectABY = centerB.Y - centerA.Y;

            PointF VectAB = new PointF(vectABX, vectABY);

            float vectABNorm = norm(VectAB);

            VectAB.X /= vectABNorm;
            VectAB.Y /= vectABNorm;


            PointF G = new PointF(centerA.X + rA * VectAB.X, centerA.Y + rA * VectAB.Y);

            PointF H = new PointF(centerB.X + rB * VectAB.X, centerB.Y + rB * VectAB.Y);

            PointF C = rotatePoint(G, centerA, -Q);
            PointF D = rotatePoint(G, centerA, Q);

            PointF E = rotatePoint(H, centerB, Q);
            PointF F = rotatePoint(H, centerB, -Q);

            List<Edge> list = new List<Edge>();
            list.Add(new Edge(C, F));
            list.Add(new Edge(D, E));

            return list;
        }
    }
}

