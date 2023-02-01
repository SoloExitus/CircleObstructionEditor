using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;



namespace CircleEditor
{
    public partial class Form1 : Form
    {
        Graphics G;
        Bitmap Image;
        bool m_isUpdate = false;

        CircleEditor Editor = new CircleEditor();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Редактор циркулярных препятствий";

            openFileDialog1.Filter = "Text files(*.xml)|*.xml|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.xml)|*.xml|All files(*.*)|*.*";

            Image = new Bitmap(pb_ViewPort.Width, pb_ViewPort.Height);
            G = Graphics.FromImage(Image);
            G.Clear(Color.White);

            m_isUpdate = true;
            timer1.Start();
        }

        private void Render()
        {
            pb_ViewPort.Image?.Dispose();
            pb_ViewPort.Image = (Bitmap)Image.Clone();
            G.Clear(Color.White);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_isUpdate)
            {
                Editor.Draw(ref G);
                Render();
                m_isUpdate = false;
            }
        }

        private void rb_Mode_Remove_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.Remove;
        }

        private void rb_Mode_Edit_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.Edit;
        }

        private void rb_Mode_None_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.None;
        }

        private void rb_Mode_Create_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.Create;
        }

        private void rb_set_start_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Mode = CircleEditor.EditorMode.SetStart;
        }

        private void rb_set_end_CheckedChanged(object sender, EventArgs e)
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

        private void Newbutton_Click(object sender, EventArgs e)
        {
            Editor.Clear();
            m_isUpdate = true;
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = saveFileDialog1.FileName;

            Editor.Save_to_file(filename);
        }

        private void Loadbutton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = openFileDialog1.FileName;
            Editor.Load_from_file(filename);

            m_isUpdate = true;
        }

        private void bt_createRandom_Click(object sender, EventArgs e)
        {
            int createNum = (int)(numUD_numberCreate.Value);

            int max_X = pb_ViewPort.Width;
            int max_Y = pb_ViewPort.Height;

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

                } while (Editor.AddObstruction(ref newObstruct));
            }

            m_isUpdate = true;
        }

        private void bt_runAstar_Click(object sender, EventArgs e)
        {
            // Запускаем алгорим А* 
            Editor.RunA();
            m_isUpdate = true;
        }

        private void btn_DrawGraph_Click(object sender, EventArgs e)
        {
            // Сгенерировать весь граф целиком
            Editor.GenerateFullGraph();
            m_isUpdate = true;
        }

        private void pb_ViewPort_SizeChanged(object sender, EventArgs e)
        {
            Image = new Bitmap(pb_ViewPort.Width, pb_ViewPort.Height);
            G = Graphics.FromImage(Image);
            m_isUpdate = true;
        }
    }

    // Базовые математические функции
    public static class BaseMath
    {
        // Квадрат расстояния между точками
        public static float Square_distance(ref PointF a, ref PointF b)
        {
            float dX = a.X - b.X;
            float dY = a.Y - b.Y;
            return (dX * dX + dY * dY);
        }

        public static float Distance(ref PointF a, ref PointF b)
        {
            float sd = Square_distance(ref a, ref b);
            return (float)Math.Sqrt(sd);
        }

        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        // Перевод радианы в градусы
        public static float To_degre(float rad)
        {
            return rad * 180 / (float)Math.PI;
        }

        // Поворот точки p вокруг центра с на угол angle в радианах
        public static PointF rotatePoint(ref PointF p, ref PointF c, float angle)
        {
            float nX = (float)(c.X + (p.X - c.X) * Math.Cos((double)angle) - (p.Y - c.Y) * Math.Sin((double)angle));
            float nY = (float)(c.Y + (p.X - c.X) * Math.Sin((double)angle) + (p.Y - c.Y) * Math.Cos((double)angle));

            return new PointF(nX, nY);
        }

        // Угол между двумя векторами в радианах от 0 до Pi
        public static float AngleBetweenVectors(ref PointF f, ref PointF s)
        {
            return (float)Math.Atan2(f.X * s.Y - f.Y * s.X, f.X * s.X + f.Y * s.Y);
        }

        // длина вектора
        public static float vectorLenght(ref PointF vect)
        {
            return (float)Math.Sqrt(vect.X * vect.X + vect.Y * vect.Y);
        }
    }

    class Circle
    {
        public PointF m_center;
        public float m_radius = 0;
        public List<int> m_VertexIndexes = new List<int>();
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

        public void setRadius(PointF e)
        {
            m_radius = BaseMath.Distance(ref m_center, ref e);
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

        public GraphVertex(PointF point, int obstacleIndex, PointF endPoint)
        {
            m_position = point;
            m_obstacleIndex = obstacleIndex;
            m_H = BaseMath.Distance(ref m_position, ref endPoint);
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
            for (int i = 0; i < Queue.Count; i++)
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
            if (empty())
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

    class CircleEditor
    {
        public enum EditorMode
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

        EditorMode m_currentMode = EditorMode.None;

        public EditorMode Mode
        {
            get { return m_currentMode; }   // get method
            set { m_currentMode = value; }  // set method
        }

        bool m_isStartEntered = false;
        bool m_isEndEntered = false;

        PointF m_startPoint;
        PointF m_endPoint;

        List<Circle> m_Obstructions = new List<Circle>();

        List<GraphVertex> m_GraphVertexes = new List<GraphVertex>();
        List<GraphEdge> m_GraphEdges = new List<GraphEdge>();

        int m_creatingIndex = -1;
        int m_editingIndex = -1;

        bool m_isFullGraphGenerated = false;
        bool m_isPathGenerated = false;

        List<int> path = new List<int>();

        public CircleEditor() {}

        public bool AddObstruction(ref Circle obs)
        {
            if (InterseptionWithOthers(ref obs))
            {
                return true;
            }

            m_Obstructions.Add(obs);
            return false;
        }

        private bool InterseptionWithOthers(ref Circle newObs)
        {
            foreach (Circle curObstr in m_Obstructions)
            {
                float sqrDist = BaseMath.Square_distance(ref curObstr.m_center, ref newObs.m_center);
                float radiusSum = (curObstr.m_radius + newObs.m_radius);
                if (sqrDist < radiusSum * radiusSum)
                {
                    return true;
                }
            }

            return false;
        }

        public void Load_from_file(string file_path)
        {
            XDocument xdoc = XDocument.Load(file_path);

            Clear();

            XElement loadMap = xdoc.Element("map");

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

                if (endX != null && endY != null)
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
        }

        public void Save_to_file(string file_path)
        {

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

            map.Save(file_path);
        }

        public void Clear()
        {
            m_Obstructions.Clear();

            ClearGraph();

            m_isStartEntered = false;
            m_isEndEntered = false;

            m_creatingIndex = -1;
            m_editingIndex = -1;
        }

        //! Draw START

        Pen m_obstructionsPen = new Pen(Color.DeepSkyBlue, 2);

        SolidBrush m_startPointBrush = new SolidBrush(Color.Green);
        SolidBrush m_endPointBrush = new SolidBrush(Color.Red);

        Pen m_DisplayPathPen = new Pen(Color.DarkRed, 2);

        SolidBrush m_VertexBrush = new SolidBrush(Color.Blue);
        float m_VertexDispalayDiameter = 6;

        public void Draw(ref Graphics g)
        {
            DrawStartAndEndPoints(ref g);
            DrawObstructions(ref g);

            if (m_isFullGraphGenerated)
            {
                DrawFullGraph(ref g);
            }

            if (m_isPathGenerated)
            {
                DisplayPath(ref g);
            }
        }

        public void GenerateFullGraph()
        {
            ClearGraph();

            CreatStartAndEndVertexesAndEdges();

            for (int i = 0; i < m_Obstructions.Count; i++)
            {
                GenerateEdgesAndVertexes(i);
            }

            m_isFullGraphGenerated = true;
        }

        private void DisplayPath(ref Graphics g)
        {
            // Проходимся по всем вершинам, кроме последней - это начальная точка
            for (int i = 0; i < path.Count - 1; i++)
            {
                int currVertexIndex = path[i];
                int nextVertexIndex = path[i + 1];

                // Отрисовываем ребро между двумя вершинами, которое может быть ребром перехода или огибающем ребром
                if (m_GraphVertexes[currVertexIndex].m_obstacleIndex == m_GraphVertexes[nextVertexIndex].m_obstacleIndex && currVertexIndex != 1 && currVertexIndex != 0)
                {
                    Circle currentObst = m_Obstructions[m_GraphVertexes[currVertexIndex].m_obstacleIndex];
                    PointF center = currentObst.m_center;

                    PointF OX = new PointF(1, 0);

                    PointF firstVector = new PointF(m_GraphVertexes[nextVertexIndex].m_position.X - center.X, m_GraphVertexes[nextVertexIndex].m_position.Y - center.Y);
                    PointF secondVector = new PointF(m_GraphVertexes[currVertexIndex].m_position.X - center.X, m_GraphVertexes[currVertexIndex].m_position.Y - center.Y);

                    float startAngleRad = BaseMath.AngleBetweenVectors(ref OX, ref firstVector);
                    float sweepAngleRad = BaseMath.AngleBetweenVectors(ref firstVector, ref secondVector);

                    float startAngle = BaseMath.To_degre(startAngleRad);
                    float sweepAngle = BaseMath.To_degre(sweepAngleRad);

                    g.DrawArc(m_DisplayPathPen, center.X - currentObst.m_radius, center.Y - currentObst.m_radius,
                        currentObst.m_radius * 2, currentObst.m_radius * 2, startAngle, sweepAngle);
                }
                else
                {
                    g.DrawLine(m_DisplayPathPen, m_GraphVertexes[currVertexIndex].m_position, m_GraphVertexes[nextVertexIndex].m_position);
                    g.FillEllipse(m_VertexBrush, m_GraphVertexes[currVertexIndex].m_position.X - 3, m_GraphVertexes[currVertexIndex].m_position.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
                    g.FillEllipse(m_VertexBrush, m_GraphVertexes[nextVertexIndex].m_position.X - 3, m_GraphVertexes[nextVertexIndex].m_position.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
                }
            }
        }

        private void DrawStartAndEndPoints(ref Graphics g)
        {
            int PointsRadius = 8;

            if (m_isStartEntered)
                g.FillEllipse(m_startPointBrush, m_startPoint.X - PointsRadius, m_startPoint.Y - PointsRadius, PointsRadius * 2, PointsRadius * 2);

            if (m_isEndEntered)
                g.FillEllipse(m_endPointBrush, m_endPoint.X - PointsRadius, m_endPoint.Y - PointsRadius, PointsRadius * 2, PointsRadius * 2);
        }

        private void DrawObstructions(ref Graphics g)
        {
            foreach (Circle obstruction in m_Obstructions)
            {
                PointF center = obstruction.m_center;
                float radius = obstruction.m_radius;
                g.DrawEllipse(m_obstructionsPen, center.X - radius, center.Y - radius, radius * 2, radius * 2);
            }
        }

        private void DrawFullGraph(ref Graphics g)
        {
            foreach (GraphEdge edge in m_GraphEdges)
            {
                if (m_GraphVertexes[edge.m_firstVertexIndex].m_obstacleIndex == m_GraphVertexes[edge.m_secondVertexIndex].m_obstacleIndex)
                    continue;

                g.FillEllipse(new SolidBrush(Color.Gray), m_GraphVertexes[edge.m_firstVertexIndex].m_position.X - 3, m_GraphVertexes[edge.m_firstVertexIndex].m_position.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
                g.FillEllipse(new SolidBrush(Color.Gray), m_GraphVertexes[edge.m_firstVertexIndex].m_position.X - 3, m_GraphVertexes[edge.m_firstVertexIndex].m_position.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);

                g.DrawLine(new Pen(Color.Gray, 2), m_GraphVertexes[edge.m_firstVertexIndex].m_position, m_GraphVertexes[edge.m_secondVertexIndex].m_position);
            }
        }

        //! Draw END

        public void MouseMove(PointF mouse_position)
        {
            switch (m_currentMode)
            {
                case EditorMode.Creating:
                    m_Obstructions[m_creatingIndex].setRadius(mouse_position);
                    DropGenerated();
                    break;
                case EditorMode.Moving:
                    m_Obstructions[m_editingIndex].setCenter(mouse_position);
                    DropGenerated();
                    break;
                case EditorMode.Editing:
                    m_Obstructions[m_editingIndex].setRadius(mouse_position);
                    DropGenerated();
                    break;
            }
        }

        public void MouseDownLeft(PointF mouse_position)
        {
            switch (m_currentMode)
            {
                case EditorMode.Create:
                    m_creatingIndex = m_Obstructions.Count();
                    m_Obstructions.Add(new Circle(mouse_position));
                    m_currentMode = EditorMode.Creating;
                    DropGenerated();
                    break;
                case EditorMode.Creating:
                    m_Obstructions[m_creatingIndex].setRadius(mouse_position);
                    m_creatingIndex = -1;
                    m_currentMode = EditorMode.Create;
                    DropGenerated();
                    break;
                case EditorMode.Remove:
                    int removeIndex = TryGrabObstruction(mouse_position);
                    if (removeIndex > -1)
                    {
                        m_Obstructions.Remove(m_Obstructions[removeIndex]);
                        DropGenerated();
                    }
                    break;
                case EditorMode.Edit:
                    m_editingIndex = TryGrabObstruction(mouse_position);
                    if (m_editingIndex > -1)
                    {
                        m_currentMode = EditorMode.Moving;
                        DropGenerated();
                    }
                    break;
                case EditorMode.Moving:
                    m_Obstructions[m_editingIndex].setCenter(mouse_position);
                    m_editingIndex = -1;
                    m_currentMode = EditorMode.Edit;
                    DropGenerated();
                    break;
                case EditorMode.SetStart:
                    m_startPoint = mouse_position;
                    m_isStartEntered = true;
                    DropGenerated();
                    break;
                case EditorMode.SetEnd:
                    m_endPoint = mouse_position;
                    m_isEndEntered = true;
                    DropGenerated();
                    break;
            }
        }

        public void MouseDownRight(PointF mouse_position)
        {
            switch (m_currentMode)
            {
                case EditorMode.Edit:
                    m_editingIndex = TryGrabObstruction(mouse_position);
                    if (m_editingIndex > -1)
                    {
                        m_currentMode = EditorMode.Editing;
                        DropGenerated();
                    }
                    break;
                case EditorMode.Editing:
                    m_Obstructions[m_editingIndex].setRadius(mouse_position);
                    m_currentMode = EditorMode.Edit;
                    DropGenerated();
                    break;
            }
        }

        private void DropGenerated()
        {
            m_isPathGenerated = false;
            m_isFullGraphGenerated = false;
        }

        public bool RunA()
        {
            ClearGraph();

            PriorityQueue Q = new PriorityQueue();

            CreatStartAndEndVertexesAndEdges();

            Q.push(0);

            int current = -1;

            while (!Q.empty())
            {
                // Получем индекс рассматриваемой вершины из очереди с приорететами, как вершины с минимальной оценкой
                // до конечной точки (как самая перспективная)
                current = Q.pop(m_GraphVertexes);

                // Если данная вершина является конечной точкой, то заканчиваем
                if (current == 1)
                {
                    FormPath();
                    return true;
                }

                // Если вершина не начальная, то генерируем для препятствия, которому она принадлежит ребра перехода с остальными препятствмиями
                // генерируем огибающие ребра между всеми вершинами принадлежащими данному препрятствию
                if (current != 0)
                    GenerateEdgesAndVertexes(m_GraphVertexes[current].m_obstacleIndex);

                // Обрабатываем все вершины инцентедентные с текущей рассматриваемой.
                // Устанавливаем ее как родителя для инцендентной, если пусть до нее через текущую оказался короче, чем уже имеющийся 
                // или если это вершина достигнута впервые
                foreach (int edgeIndex in m_GraphVertexes[current].m_incidentEdgeIndexes)
                {
                    // Длина пути до индендентной вершины = длина пути до текущей + длина ребра
                    float pathLength = m_GraphVertexes[current].m_G + m_GraphEdges[edgeIndex].m_lenght;

                    int nextVertexIndex = m_GraphEdges[edgeIndex].m_firstVertexIndex == current ?
                        m_GraphEdges[edgeIndex].m_secondVertexIndex :
                        m_GraphEdges[edgeIndex].m_firstVertexIndex;

                    // Если вершина достигнута впервые или наден более короткий путь
                    // Устанавливаем текущую вершину как родителя и добавляем в очередь с приорететом, если она уже не там
                    if (m_GraphVertexes[nextVertexIndex].m_G == -1 || pathLength < m_GraphVertexes[nextVertexIndex].m_G)
                    {
                        m_GraphVertexes[nextVertexIndex].setParent(current, pathLength);

                        if (!Q.have(nextVertexIndex))
                            Q.push(nextVertexIndex);
                    }
                }

            }

            // Не удалось достич конечной точки из стартовой
            return false;
        }

        private void FormPath()
        {
            int currentVertexIndex = 1;
            while (currentVertexIndex != -1)
            {
                path.Add(currentVertexIndex);
                currentVertexIndex = m_GraphVertexes[currentVertexIndex].m_parentVertexIndex;
            }

            m_isPathGenerated = true;
        }

        private void ClearGraph()
        {
            m_GraphVertexes.Clear();
            m_GraphEdges.Clear();

            path.Clear();

            m_isFullGraphGenerated = false;
            m_isPathGenerated = false;

            for (int i = 0; i < m_Obstructions.Count; i++)
            {
                m_Obstructions[i].m_isEdgesGenerated = false;
                m_Obstructions[i].m_VertexIndexes.Clear();
            }
        }

        private int TryGrabObstruction(PointF location)
        {
            int count = m_Obstructions.Count();

            int index = -1;
            float minDistToObstruction = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                float dist = BaseMath.Distance(ref m_Obstructions[i].m_center, ref location);
                if (dist < minDistToObstruction && dist < m_Obstructions[i].m_radius)
                {
                    minDistToObstruction = dist;
                    index = i;
                }
            }
            return index;
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
                GraphEdge startToEnd = new GraphEdge(0, 1, BaseMath.Distance(ref m_startPoint, ref m_endPoint));
                m_GraphEdges.Add(startToEnd);

                m_GraphVertexes[0].m_incidentEdgeIndexes.Add(startToEndIndex);
                m_GraphVertexes[1].m_incidentEdgeIndexes.Add(startToEndIndex);

            }


            for (int i = 0; i < m_Obstructions.Count; i++)
            {
                List<Edge> edges = internalBitangents(m_startPoint, 0, m_Obstructions[i].m_center, m_Obstructions[i].m_radius);

                foreach (Edge e in edges)
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

                        GraphEdge newEdge = new GraphEdge(0, newVertexIndex, BaseMath.Distance(ref m_startPoint, ref e.m_second));

                        m_GraphEdges.Add(newEdge);
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

                        GraphEdge newEdge = new GraphEdge(1, newVertexIndex, BaseMath.Distance(ref m_endPoint, ref e.m_second));

                        m_GraphEdges.Add(newEdge);
                    }
                }
            }
        }

        private void GenerateEdgesAndVertexes(int obstacleIndex)
        {
            if (m_Obstructions[obstacleIndex].m_isEdgesGenerated)
                return;

            for (int i = 0; i < m_Obstructions.Count; i++)
            {
                if (i == obstacleIndex || m_Obstructions[i].m_isEdgesGenerated)
                    continue;

                List<Edge> surfing = surfingEdges(obstacleIndex, i);

                foreach (Edge sEdge in surfing)
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

                    GraphEdge newEdgeFS = new GraphEdge(firstVertexIndex, secondVertexIndex, BaseMath.Distance(ref firstVertex.m_position, ref secondVertex.m_position));
                    m_GraphEdges.Add(newEdgeFS);
                }
            }

            List<GraphEdge> hugging = createHuggingEdges(obstacleIndex);

            foreach (GraphEdge hEdge in hugging)
            {
                int currentEdgeIndex = m_GraphEdges.Count;
                m_GraphEdges.Add(hEdge);

                m_GraphVertexes[hEdge.m_firstVertexIndex].m_incidentEdgeIndexes.Add(currentEdgeIndex);
                m_GraphVertexes[hEdge.m_secondVertexIndex].m_incidentEdgeIndexes.Add(currentEdgeIndex);
            }

            m_Obstructions[obstacleIndex].m_isEdgesGenerated = true;
        }

        private List<Edge> internalBitangents(PointF centerA, float rA, PointF centerB, float rB)
        {
            float Q = (float)Math.Acos((double)(rA + rB) / BaseMath.Distance(ref centerA, ref centerB));

            float vectABX = centerB.X - centerA.X;
            float vectABY = centerB.Y - centerA.Y;

            PointF VectAB = new PointF(vectABX, vectABY);

            float vectABLen = BaseMath.vectorLenght(ref VectAB);

            VectAB.X /= vectABLen;
            VectAB.Y /= vectABLen;

            PointF G = new PointF(centerA.X + rA * VectAB.X, centerA.Y + rA * VectAB.Y);

            PointF H = new PointF(centerB.X + rB * (-VectAB.X), centerB.Y + rB * (-VectAB.Y));

            PointF C = BaseMath.rotatePoint(ref G, ref centerA, -Q);
            PointF D = BaseMath.rotatePoint(ref G, ref centerA, Q);

            PointF E = BaseMath.rotatePoint(ref H, ref centerB, Q);
            PointF F = BaseMath.rotatePoint(ref H, ref centerB, -Q);

            List<Edge> list = new List<Edge>();
            list.Add(new Edge(C, F));
            list.Add(new Edge(D, E));

            return list;
        }

        //private List<Edge> internalBitangents(Circle a, Circle b)
        //{
        //    PointF centerA = a.m_center;
        //    PointF centerB = b.m_center;

        //    float rA = a.m_radius;
        //    float rB = b.m_radius;
        //    return internalBitangents(centerA, rA, centerB, rB);
        //}

        private bool IsObstructionsBlockEdge(Edge edge, int firstIgnore, int secondIgnore)
        {
            PointF edgeStart = edge.m_first;
            PointF edgeEnd = edge.m_second;

            for (int i = 0; i < m_Obstructions.Count; i++)
            {
                if (i == firstIgnore || i == secondIgnore)
                    continue;

                if (is_Block(m_Obstructions[i], edgeStart, edgeEnd))
                    return true;

            }
            return false;
        }

        private List<Edge> surfingEdges(int firstObstIndex, int secondObstIndex)
        {
            PointF centerA = m_Obstructions[firstObstIndex].m_center;
            float rA = m_Obstructions[firstObstIndex].m_radius;

            PointF centerB = m_Obstructions[secondObstIndex].m_center;
            float rB = m_Obstructions[secondObstIndex].m_radius;

            List<Edge> edges = new List<Edge>();

            List<Edge> internalBit = internalBitangents(centerA, rA, centerB, rB);
            List<Edge> externalBit = externalBitangents(centerA, rA, centerB, rB);

            for (int i = 0; i < internalBit.Count; i++)
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

        /*private List<Edge> surfingEdges(int circleAIndex, int circleBIndex)
        {
            PointF centerA = m_Obstructions[circleAIndex].m_center;
            float rA = m_Obstructions[circleAIndex].m_radius;

            PointF centerB = m_Obstructions[circleBIndex].m_center;
            float rB = m_Obstructions[circleBIndex].m_radius;

            return surfingEdges(centerA, rA, centerB, rB, circleAIndex, circleBIndex);
        }*/

        private GraphEdge createHuggingEdge(int obstacleIndex, int firstVertexIndex, int secondVertexIndex)
        {
            float edgeLenght = calculateHuggingEdgeLenght(
                m_Obstructions[obstacleIndex].m_center,
                m_Obstructions[obstacleIndex].m_radius,
                m_GraphVertexes[firstVertexIndex].m_position,
                m_GraphVertexes[secondVertexIndex].m_position
                );

            GraphEdge edge = new GraphEdge(firstVertexIndex, secondVertexIndex, edgeLenght);
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

        //private List<Edge> externalBitangents(Circle a, Circle b)
        //{
        //    PointF centerA = a.m_center;
        //    PointF centerB = b.m_center;

        //    float rA = a.m_radius;
        //    float rB = b.m_radius;

        //    return externalBitangents(centerA, rA, centerB, rB);
        //}

        private List<Edge> externalBitangents(PointF centerA, float rA, PointF centerB, float rB)
        {
            float Q = (float)Math.Acos((double)Math.Abs(rA - rB) / BaseMath.Distance(ref centerA, ref centerB));

            float vectBLX = centerB.X - centerA.X;
            float vectBLY = centerB.Y - centerA.Y;

            if (rB > rA)
            {
                vectBLX *= -1;
                vectBLY *= -1;
            }

            // вектор из центра большего препятствия в центр меньшего, если равны то не важно
            PointF VectBL = new PointF(vectBLX, vectBLY);

            float vectBLLen = BaseMath.vectorLenght(ref VectBL);

            VectBL.X /= vectBLLen;
            VectBL.Y /= vectBLLen;

            PointF G = new PointF(centerA.X + rA * VectBL.X, centerA.Y + rA * VectBL.Y);

            PointF H = new PointF(centerB.X + rB * VectBL.X, centerB.Y + rB * VectBL.Y);

            PointF C = BaseMath.rotatePoint(ref G, ref centerA, -Q);
            PointF D = BaseMath.rotatePoint(ref G, ref centerA, Q);

            PointF E = BaseMath.rotatePoint(ref H, ref centerB, Q);
            PointF F = BaseMath.rotatePoint(ref H, ref centerB, -Q);

            List<Edge> list = new List<Edge>
            {
                new Edge(C, F),
                new Edge(D, E)
            };

            return list;
        }

        private bool is_Block(Circle obstruct, PointF A, PointF B)
        {
            PointF C = obstruct.m_center;

            float u = ((C.X - A.X) * (B.X - A.X) + (C.Y - A.Y) * (B.Y - A.Y)) / BaseMath.Square_distance(ref B, ref A);

            float clamp_u = BaseMath.Clamp(u, 0, 1);

            float Ex = A.X + clamp_u * (B.X - A.X);
            float Ey = A.Y + clamp_u * (B.Y - A.Y);

            PointF E = new PointF(Ex, Ey);

            float Sd = BaseMath.Square_distance(ref E, ref C);

            return Sd < (obstruct.m_radius * obstruct.m_radius);
        }

        private float calculateHuggingEdgeLenght(PointF circleCenter, float circleRadius, PointF firstPoint, PointF secondPoint)
        {
            PointF firstVector = new PointF();
            firstVector.X = firstPoint.X - circleCenter.X;
            firstVector.Y = firstPoint.Y - circleCenter.Y;

            PointF secondVector = new PointF();
            secondVector.X = secondPoint.X - circleCenter.X;
            secondVector.Y = secondPoint.Y - circleCenter.Y;

            float angle = Math.Abs(BaseMath.AngleBetweenVectors(ref firstVector, ref secondVector));

            float huggingEdgeLenght = circleRadius * angle;

            return huggingEdgeLenght;
        }

    }


    
}

