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
            public List<int> m_ConnectWithCircles = new List<int>();
            public List<int> m_VertexIndexes = new List<int>();
            //public List<int> m_EdgeIndexes = new List<int>();
            public bool m_isEdgesGenerated = false;

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

        class GraphVertex
        {
            public PointF m_position;
            public int m_obstacleIndex;
            public List<int> m_incidentEdgeIndexes = new List<int>();
            public int m_parentVertexIndex = -1;
            public float m_G = -1;
            public float m_H = -1;
            public float m_F = -1;
            public bool m_isViewed = false;

            private float distance(PointF a, PointF b)
            {
                float dX = a.X - b.X;
                float dY = a.Y - b.Y;
                return (float)Math.Sqrt((double)(dX * dX + dY * dY));
            }

            public GraphVertex(PointF point, int obstacleIndex, PointF endPoint)
            {
                m_position = point;
                m_obstacleIndex = obstacleIndex;
                m_H = distance(m_position, endPoint);
            }

            public GraphVertex(float x, float y, int obstacleIndex, float g, PointF endPoint) :
                this(new PointF(x, y), obstacleIndex, endPoint)
            {
            }

            public void setParent(int parentIndex, float pathCost)
            {
                m_G = pathCost;
                m_parentVertexIndex = parentIndex;
                m_F = m_G + m_H;
            }
            
        }

        class GraphEdge
        {
            public int m_firstVertexIndex;
            public int m_secondVertexIndex;
            public float m_lenght;


            public GraphEdge(int firstVertexIndex, int secondVertexIndex, float lengnt)
            {
                m_firstVertexIndex = firstVertexIndex;
                m_secondVertexIndex = secondVertexIndex;
                m_lenght = lengnt;
            }
        }




        class PriorityQueue
        {
            private List<int> Queue;

            public PriorityQueue()
            {
                Queue = new List<int>();
            }

            public bool have(int index)
            {
                for(int i=0; i< Queue.Count; i++)
                {
                    if (Queue[i] == index)
                        return true;
                }
                
                return false;
            }

            public void push(int index)
            {
                Queue.Add(index);
            }

            public int pop(List<GraphVertex> Vertexes)
            {
                if (this.empty())
                    return -1;

                var (minValue, minIndex) = Queue.Select((x, i) => (Vertexes[x].m_F, x)).Min();

                //int tminIndex = Queue[0];
                //float tmin = Vertexes[Queue[0]].m_F;
                //for (int i = 1; i < Queue.Count; i++)
                //{
                //    if (Vertexes[Queue[i]].m_F < tmin)
                //    {
                //        tminIndex = Queue[i];
                //        tmin = Vertexes[Queue[i]].m_F;
                //    }
                //}

                //int er;
                //if (tmin != minValue)
                //    er = 0;

                Queue.Remove(minIndex);
                return minIndex;
            }

            public bool empty()
            {
                return Queue.Count == 0;
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

        List<GraphVertex> m_GraphVertexes = new List<GraphVertex>();
        List<GraphEdge> m_GraphEdges = new List<GraphEdge>();

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
            int PointsRadius = 8;

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
                G.DrawEllipse(new Pen(Color.DeepSkyBlue, 2), center.X - radius, center.Y - radius, radius * 2, radius * 2);
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

        private void GenerateEdgesAndVertexes(int obstacleIndex)
        {
            if (m_Obstructions[obstacleIndex].m_isEdgesGenerated)
                return;

            for(int i = 0; i < m_Obstructions.Count; i++)
            {
                if (i == obstacleIndex)
                    continue;

                bool is_newLink = true;
                for (int j = 0; j < m_Obstructions[obstacleIndex].m_ConnectWithCircles.Count; j++ )
                {
                    if (i == j)
                    {
                        is_newLink = false;
                        break;
                    }
                }

                m_Obstructions[obstacleIndex].m_ConnectWithCircles.Add(i);

                if (is_newLink)
                {
                    List<Edge> surfing = surfingEdges(obstacleIndex, i);

                    foreach(Edge sEdge in surfing)
                    {
                        int currentEdgeIndex = m_GraphEdges.Count;

                        GraphVertex firstVertex = new GraphVertex(sEdge.m_first, obstacleIndex, m_endPoint);
                        firstVertex.m_incidentEdgeIndexes.Add(currentEdgeIndex);
                        int firstVertexIndex = m_GraphVertexes.Count;
                        m_GraphVertexes.Add(firstVertex);
                        m_Obstructions[obstacleIndex].m_VertexIndexes.Add(firstVertexIndex);

                        GraphVertex secondVertex = new GraphVertex(sEdge.m_second, i, m_endPoint);
                        secondVertex.m_incidentEdgeIndexes.Add(currentEdgeIndex);
                        int secondVertexIndex = m_GraphVertexes.Count;
                        m_GraphVertexes.Add(secondVertex);
                        m_Obstructions[i].m_VertexIndexes.Add(secondVertexIndex);

                        GraphEdge newEdgeFS = new GraphEdge(firstVertexIndex, secondVertexIndex, distance(firstVertex.m_position, secondVertex.m_position));
                        m_GraphEdges.Add(newEdgeFS);

                        //m_Obstructions[obstacleIndex].m_EdgeIndexes.Add(currentEdgeIndex);
                        //m_Obstructions[i].m_EdgeIndexes.Add(currentEdgeIndex);
                    }
                }
            }

            List<GraphEdge> hugging = createHuggingEdges(obstacleIndex);

            foreach (GraphEdge hEdge in hugging)
            {
                int currentEdgeIndex = m_GraphEdges.Count;
                m_GraphEdges.Add(hEdge);
                //m_Obstructions[obstacleIndex].m_EdgeIndexes.Add(currentEdgeIndex);

                m_GraphVertexes[hEdge.m_firstVertexIndex].m_incidentEdgeIndexes.Add(currentEdgeIndex);
                m_GraphVertexes[hEdge.m_secondVertexIndex].m_incidentEdgeIndexes.Add(currentEdgeIndex);
            }

            m_Obstructions[obstacleIndex].m_isEdgesGenerated = true;
        }



        private void CreatStartAndEndVertexesAndEdges()
        {
            // index 0 - start
            GraphVertex startVertex = new GraphVertex(m_startPoint, -1, m_endPoint);
            m_GraphVertexes.Add(startVertex);
            startVertex.setParent(-1, 0);

            // index 1 - finish
            GraphVertex endVertex = new GraphVertex(m_endPoint, -1, m_endPoint);
            m_GraphVertexes.Add(endVertex);

            // ребро между начальной и конечной точками
            if (!IsObstructionsBlockEdge(new Edge(m_startPoint, m_endPoint), -1, -1))
            {
                int startToEndIndex = m_GraphEdges.Count;
                GraphEdge startToEnd = new GraphEdge(0, 1, distance(m_startPoint, m_endPoint));
                m_GraphEdges.Add(startToEnd);

                m_GraphVertexes[0].m_incidentEdgeIndexes.Add(startToEndIndex);
                m_GraphVertexes[1].m_incidentEdgeIndexes.Add(startToEndIndex);

            }


            for(int i = 0; i < m_Obstructions.Count; i++)
            {
                List<Edge> edges = internalBitangents(m_startPoint, 0, m_Obstructions[i].m_center, m_Obstructions[i].m_radius);

                foreach(Edge e in edges)
                {
                    if (!IsObstructionsBlockEdge(e, -1, i))
                    {
                        int newEdgeIndex = m_GraphEdges.Count;

                        GraphVertex newVertex = new GraphVertex(e.m_second, i, m_endPoint);



                        newVertex.m_incidentEdgeIndexes.Add(newEdgeIndex);

                        m_GraphVertexes[0].m_incidentEdgeIndexes.Add(newEdgeIndex);

                        int newVertexIndex = m_GraphVertexes.Count;
                        m_GraphVertexes.Add(newVertex);

                        m_Obstructions[i].m_VertexIndexes.Add(newVertexIndex);

                        GraphEdge newEdge = new GraphEdge(0, newVertexIndex, distance(m_startPoint, e.m_second));

                        m_GraphEdges.Add(newEdge);

                        //m_Obstructions[i].m_EdgeIndexes.Add(newEdgeIndex);
                    }
                }

            }

            for (int i = 0; i < m_Obstructions.Count; i++)
            {
                List<Edge> edges = internalBitangents(m_endPoint, 0, m_Obstructions[i].m_center, m_Obstructions[i].m_radius);

                foreach (Edge e in edges)
                {
                    if (!IsObstructionsBlockEdge(e, -1, i))
                    {
                        int newEdgeIndex = m_GraphEdges.Count;

                        GraphVertex newVertex = new GraphVertex(e.m_second, i, m_endPoint);
                        newVertex.m_incidentEdgeIndexes.Add(newEdgeIndex);

                        m_GraphVertexes[1].m_incidentEdgeIndexes.Add(newEdgeIndex);

                        int newVertexIndex = m_GraphVertexes.Count;
                        m_GraphVertexes.Add(newVertex);

                        m_Obstructions[i].m_VertexIndexes.Add(newVertexIndex);

                        GraphEdge newEdge = new GraphEdge(1, newVertexIndex, distance(m_endPoint, e.m_second));

                        m_GraphEdges.Add(newEdge);

                        //m_Obstructions[i].m_EdgeIndexes.Add(newEdgeIndex);
                    }
                }

            }

        }

        private void ClearGraph()
        {
            m_GraphVertexes.Clear();
            m_GraphEdges.Clear();

            for (int i = 0; i < m_Obstructions.Count; i++)
            {
                m_Obstructions[i].m_isEdgesGenerated = false;
                m_Obstructions[i].m_VertexIndexes.Clear();
                m_Obstructions[i].m_ConnectWithCircles.Clear();
                //m_Obstructions[i].m_EdgeIndexes.Clear();

            }
        }

        private bool RunA()
        {
            ClearGraph();

            PriorityQueue Q = new PriorityQueue();

            CreatStartAndEndVertexesAndEdges();

            Q.push(0);

            int current = -1;

            while (!Q.empty())
            {
                current = Q.pop(m_GraphVertexes);
                if (current == 1)       // т.е. конечная точка
                    return true;

                m_GraphVertexes[current].m_isViewed = true;

                if (current != 0)
                {
                    GenerateEdgesAndVertexes(m_GraphVertexes[current].m_obstacleIndex);
                }

                foreach (int edgeIndex in m_GraphVertexes[current].m_incidentEdgeIndexes)
                {
                    float pathLength = m_GraphVertexes[current].m_G + m_GraphEdges[edgeIndex].m_lenght;

                    int nextVertexIndex = m_GraphEdges[edgeIndex].m_firstVertexIndex == current ? m_GraphEdges[edgeIndex].m_secondVertexIndex : m_GraphEdges[edgeIndex].m_firstVertexIndex;

                    /*if (m_GraphVertexes[nextVertexIndex].m_isViewed && pathLength >= m_GraphVertexes[nextVertexIndex].m_G)
                        continue;*/

                    if (m_GraphVertexes[nextVertexIndex].m_G == -1 || pathLength < m_GraphVertexes[nextVertexIndex].m_G)
                    {
                        m_GraphVertexes[nextVertexIndex].setParent(current, pathLength);

                        if (!Q.have(nextVertexIndex))
                            Q.push(nextVertexIndex);
                    }
                }

            }

            return false;
        }

        private void Test_Click(object sender, EventArgs e)
        {
            if (!m_isStartEntered || !m_isEndEntered)
                return;

            bool result = RunA();

            if (result)
            {
                List<int> vertexPathFromFinish = new List<int>();

                int currentVertexIndex = 1;
                while (currentVertexIndex != -1)
                {
                    vertexPathFromFinish.Add(currentVertexIndex);
                    G.FillEllipse(new SolidBrush(Color.Blue), m_GraphVertexes[currentVertexIndex].m_position.X - 3, m_GraphVertexes[currentVertexIndex].m_position.Y - 3, 6, 6);
                    currentVertexIndex = m_GraphVertexes[currentVertexIndex].m_parentVertexIndex;
                }

                /*for(int i = 1; i < vertexPathFromFinish.Count; i++)
                {
                    int firstVertexIndex = vertexPathFromFinish[i - 1];
                    int secondVertexIndex = vertexPathFromFinish[i];

                    int currentEdgeIndex = -1;
                    foreach (int incidentEdgeIndex in m_GraphVertexes[firstVertexIndex].m_incidentEdgeIndexes)
                    {
                        if (((m_GraphEdges[incidentEdgeIndex].m_firstVertexIndex == firstVertexIndex) && (m_GraphEdges[incidentEdgeIndex].m_secondVertexIndex == secondVertexIndex)) ||
                            ((m_GraphEdges[incidentEdgeIndex].m_secondVertexIndex == firstVertexIndex) && (m_GraphEdges[incidentEdgeIndex].m_firstVertexIndex == secondVertexIndex)))
                        {
                            currentEdgeIndex = incidentEdgeIndex;
                            break;
                        }

                    }

                    if (currentEdgeIndex != -1)
                        edgesPath.Add(currentEdgeIndex);
                    else
                        throw new Exception();

                }*/

                DisplayPath(vertexPathFromFinish);
            }
        }

        private float ConvertradToDegrees(float rad)
        {
            return rad * 180 / (float)Math.PI;
        }

        private float convert(float rad)
        {
            if (rad < 0)
            {
                return (float)Math.Abs(rad);
            }

            return (float)Math.PI * 2 - rad;
        }

        private void DisplayPath(List<int> path)
        {
            for(int i = 0; i < path.Count - 1; i++)
            {
                if (m_GraphVertexes[path[i]].m_obstacleIndex == m_GraphVertexes[path[i + 1]].m_obstacleIndex && path[i] != 1 && path[i] != 0)
                {
                    Circle currentObst = m_Obstructions[m_GraphVertexes[path[i]].m_obstacleIndex];
                    PointF center = currentObst.m_center;

                    PointF OX = new PointF(1, 0);

                    PointF firstVector = new PointF(m_GraphVertexes[path[i + 1]].m_position.X - center.X, m_GraphVertexes[path[i + 1]].m_position.Y - center.Y);
                    PointF secondVector = new PointF(m_GraphVertexes[path[i]].m_position.X - center.X, m_GraphVertexes[path[i]].m_position.Y - center.Y);

                    float firstAngleRad = AngleBetweenVectors(ref firstVector, ref OX);
                    float secondAngleRad = AngleBetweenVectors(ref secondVector, ref OX);

                    

                    float firstAngle = ConvertradToDegrees(convert(firstAngleRad));
                    float secondAngle = ConvertradToDegrees(convert(secondAngleRad));

                    G.DrawArc(new Pen(Color.DarkRed, 3), center.X - currentObst.m_radius, center.Y - currentObst.m_radius,
                        currentObst.m_radius * 2, currentObst.m_radius * 2, firstAngle, secondAngle - firstAngle);
                }
                else
                    G.DrawLine(new Pen(Color.DarkRed), m_GraphVertexes[path[i+1]].m_position, m_GraphVertexes[path[i]].m_position);
            }

            /*G.FillEllipse(m_startPointBrush, intern[0].m_first.X - 4, intern[0].m_first.Y - 4, 8, 8);
            G.FillEllipse(m_startPointBrush, intern[0].m_second.X - 4, intern[0].m_second.Y - 4, 8, 8);

            G.FillEllipse(m_endPointBrush, intern[1].m_first.X - 4, intern[1].m_first.Y - 4, 8, 8);
            G.FillEllipse(m_endPointBrush, intern[1].m_second.X - 4, intern[1].m_second.Y - 4, 8, 8);*/
        }

        private float AngleBetweenVectors(ref PointF f, ref PointF s)
        {
            return (float)Math.Atan2(f.X * s.Y - f.Y * s.X, f.X * s.X + f.Y * s.Y);
        }

        private float calculateHuggingEdgeLenght(PointF circleCenter,float circleRadius, PointF firstPoint, PointF secondPoint)
        {
            PointF firstVector = new PointF();
            firstVector.X= firstPoint.X - circleCenter.X;
            firstVector.Y = firstPoint.Y - circleCenter.Y;

            PointF secondVector = new PointF();
            secondVector.X = secondPoint.X - circleCenter.X;
            secondVector.Y = secondPoint.Y - circleCenter.Y;

            float angle = Math.Abs(AngleBetweenVectors(ref firstVector, ref secondVector));

            float huggingEdgeLenght = circleRadius * angle;

            return huggingEdgeLenght;
        }

        private GraphEdge createHuggingEdge(int obstacleIndex, int firstVertexIndex, int secondVertexIndex)
        {
            GraphEdge edge = new GraphEdge(firstVertexIndex, secondVertexIndex, calculateHuggingEdgeLenght(m_Obstructions[obstacleIndex].m_center, m_Obstructions[obstacleIndex].m_radius, m_GraphVertexes[firstVertexIndex].m_position, m_GraphVertexes[secondVertexIndex].m_position));

            return edge;
        }

        private List<GraphEdge> createHuggingEdges(int obstacleIndex)
        {
            List<GraphEdge> edges = new List<GraphEdge>();

            List<int> obstacleVertexes = m_Obstructions[obstacleIndex].m_VertexIndexes;

            for (int i = 0; i < obstacleVertexes.Count; i++)
                for (int j = i + 1; j < obstacleVertexes.Count; j++)
                    edges.Add(createHuggingEdge(obstacleIndex, obstacleVertexes[i], obstacleVertexes[j]));

            return edges;
        }

        private bool IsObstructionsBlockEdge(Edge edge, int firstIgnore, int secondIgnore)
        {
            PointF edgeStart = edge.m_first;
            PointF edgeEnd = edge.m_second;

            for(int i = 0; i < m_Obstructions.Count; i++)
            {
                if (i == firstIgnore || i == secondIgnore)
                    continue;

                if (is_Block(m_Obstructions[i], edgeStart, edgeEnd))
                {
                    return true;
                }
            }

            /*Pen pen = new Pen(Color.Green);

            if (block)
            {
                pen = new Pen(Color.Red);
            }

            G.DrawLine(pen, m_startPoint, m_endPoint);*/

            return false;
        }

        /*private List<Edge> surfingEdges(int circleAIndex, int circleBIndex)
        {
            PointF centerA = m_Obstructions[circleAIndex].m_center;
            float rA = m_Obstructions[circleAIndex].m_radius;

            PointF centerB = m_Obstructions[circleBIndex].m_center;
            float rB = m_Obstructions[circleBIndex].m_radius;

            return surfingEdges(centerA, rA, centerB, rB, circleAIndex, circleBIndex);
        }*/

        private List<Edge> surfingEdges(int firstObstIndex, int secondObstIndex)
        {
            PointF centerA = m_Obstructions[firstObstIndex].m_center;
            float rA = m_Obstructions[firstObstIndex].m_radius;

            PointF centerB = m_Obstructions[secondObstIndex].m_center;
            float rB = m_Obstructions[secondObstIndex].m_radius;

            List<Edge> edges = new List<Edge>();

            List<Edge> internalBit = internalBitangents(centerA, rA, centerB, rB);
            List<Edge> externalBit = externalBitangents(centerA, rA, centerB, rB);

            for(int i=0; i < internalBit.Count; i++)
            {
                if (!IsObstructionsBlockEdge(internalBit[i], firstObstIndex, secondObstIndex))
                    edges.Add(internalBit[i]);
            }

            for (int i = 0; i < externalBit.Count; i++)
            {
                if (!IsObstructionsBlockEdge(externalBit[i], firstObstIndex, secondObstIndex))
                    edges.Add(externalBit[i]);
            }

            return edges;
        }

        private List<Edge> internalBitangents(Circle a, Circle b)
        {
            PointF centerA = a.m_center;
            PointF centerB = b.m_center;

            float rA = a.m_radius;
            float rB = b.m_radius;
            return internalBitangents(centerA, rA, centerB, rB);
        }

        private List<Edge> externalBitangents(Circle a, Circle b)
        {
            PointF centerA = a.m_center;
            PointF centerB = b.m_center;

            float rA = a.m_radius;
            float rB = b.m_radius;

            return externalBitangents(centerA, rA, centerB, rB);
        }

        private List<Edge> internalBitangents(PointF centerA, float rA, PointF centerB, float rB)
        {
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


        private List<Edge> externalBitangents(PointF centerA, float rA, PointF centerB, float rB)
        {
            float Q = (float)Math.Acos((double)Math.Abs(rA - rB) / distance(centerA, centerB));

            float vectBLX = centerB.X - centerA.X;
            float vectBLY = centerB.Y - centerA.Y;

            if (rB > rA)
            {
                vectBLX *= -1;
                vectBLY *= -1;
            }

            // вектор из центра большего препятствия в центр меньшего, если равны то не важно
            PointF VectBL = new PointF(vectBLX, vectBLY);

            float vectBLNorm = norm(VectBL);

            VectBL.X /= vectBLNorm;
            VectBL.Y /= vectBLNorm;


            PointF G = new PointF(centerA.X + rA * VectBL.X, centerA.Y + rA * VectBL.Y);

            PointF H = new PointF( centerB.X + rB * VectBL.X, centerB.Y + rB * VectBL.Y);

            PointF C = rotatePoint(G, centerA, -Q);
            PointF D = rotatePoint(G, centerA, Q);

            PointF E = rotatePoint(H, centerB, Q);
            PointF F = rotatePoint(H, centerB, -Q);

            List<Edge> list = new List<Edge>();
            list.Add(new Edge(C, F));
            list.Add(new Edge(D, E));

            return list;
        }

        private bool InterseptionWithOthers(Circle newObs)
        {
            foreach( Circle curObstr in m_Obstructions)
            {
                float sqrDist = SquareDistance(curObstr.m_center, newObs.m_center);
                float radiusSum = (curObstr.m_radius + newObs.m_radius);
                if (sqrDist < radiusSum * radiusSum)
                {
                    return true;
                }
            }

            return false;
        }

        private void bt_createRandom_Click(object sender, EventArgs e)
        {
            int createNum = (int)(numUD_numberCreate.Value);

            int max_X = this.ClientSize.Width;
            int max_Y = this.ClientSize.Height;

            Random rnd = new Random();

            for (int i = 0; i < createNum; i++)
            { 
                Circle newObstruct;

                do
                {
                    float center_X = rnd.Next(0, max_X + 1);
                    float center_Y = rnd.Next(0, max_Y + 1);

                    float radius = rnd.Next(1, (int)(max_Y / 2) + 1);

                    newObstruct = new Circle(new PointF(center_X, center_Y), radius);

                } while (InterseptionWithOthers(newObstruct));

                m_Obstructions.Add(newObstruct);
            }

            m_isUpdate = true;
        }
    }
}

