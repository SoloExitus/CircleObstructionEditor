using System.Globalization;
using System.Xml.Linq;

using BaseStruct;
using GraphStruct;

using PointFPair = System.Tuple<System.Drawing.PointF, System.Drawing.PointF>;

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

    List<CircleObstacle> m_Obstructions = new List<CircleObstacle>();

    List<CircleObstacle> m_GraphObstructions = new List<CircleObstacle>();

    List<GraphVertex> m_GraphVertexes = new List<GraphVertex>();
    List<GraphEdge> m_GraphEdges = new List<GraphEdge>();

    int m_creatingIndex = -1;
    int m_editingIndex = -1;

    bool m_isFullGraphGenerated = false;
    bool m_isPathGenerated = false;
    bool m_isStartAndEndGenerated = false;

    bool m_isDebugMode = false;

    List<int> m_ShortesPathVertexIndexes = new List<int>();

    public CircleEditor() { }

    public void AddObstruction(ref Circle obs)
    {
        m_Obstructions.Add(new CircleObstacle(obs));
        DropGenerated();
    }

    public void SetDebugMode(bool is_debugMode)
    {
        m_isDebugMode = is_debugMode;
    }

    private void ObstaclesProcess(ref List<CircleObstacle> map)
    {
        m_GraphObstructions = map;

        for (int i = 0; i < m_GraphObstructions.Count; ++i)
        {
            bool isNeeded = ObstacleNeedAndIntersect(i, m_GraphObstructions[i], out List<int> intersections);

            if (isNeeded)
            {
                FillIntersectionsForObstructions(i, intersections);
            }
            else
            {
                m_GraphObstructions[i].m_isBlocked = true;
            }
        }
    }

    private bool ObstacleNeedAndIntersect(int currentObstacle, in Circle obs, out List<int> intersectIndexes)
    {
        intersectIndexes = new List<int>();

        for (int i = 0; i < m_GraphObstructions.Count; ++i)
        {
            if (currentObstacle == i)
                continue;

            int res = CircleInteraction(obs, m_GraphObstructions[i]);

            if (res == 0)
                return false;

            if (res == 1)
                intersectIndexes.Add(i);
        }

        return true;
    }

    private void FillIntersectionsForObstructions(int obsIndex, List<int> intersect)
    {
        CircleObstacle obs = m_GraphObstructions[obsIndex];
        for (int i = 0; i < intersect.Count; ++i)
        {
            int interIndex = intersect[i];
            CircleObstacle intersectObs = m_GraphObstructions[interIndex];

            float sqrObsRad = obs.m_radius * obs.m_radius;
            float sqrInterIRad = intersectObs.m_radius * intersectObs.m_radius;
            float dist = BaseMath.Distance(in obs.m_center, in intersectObs.m_center);
            float sqrDist = dist * dist;
            float a = (sqrObsRad - sqrInterIRad + sqrDist) / (2 * dist);

            float teta = (float)Math.Acos(a / obs.m_radius);

            PointF vecAB = new PointF(intersectObs.m_center.X - obs.m_center.X, intersectObs.m_center.Y - obs.m_center.Y);
            float vecABLenght = BaseMath.VectorLenght(in vecAB);
            vecAB.X /= vecABLenght;
            vecAB.Y /= vecABLenght;

            PointF c = obs.m_center;
            c.X += vecAB.X * obs.m_radius;
            c.Y += vecAB.Y * obs.m_radius;

            PointF first = BaseMath.RotatePoint(ref c, ref obs.m_center, teta);
            PointF second = BaseMath.RotatePoint(ref c, ref obs.m_center, -teta);

            obs.m_Entersections.Add(new PointFPair(first, second));
            intersectObs.m_Entersections.Add(new PointFPair(first, second));

            obs.m_IntersectCircleIndexes.Add(interIndex);
            intersectObs.m_IntersectCircleIndexes.Add(obsIndex);
        }
    }

    private static int CircleInteraction(in Circle fc, in Circle sc)
    {
        return CircleInteraction(
            in fc.m_center, in fc.m_radius,
            in sc.m_center, in sc.m_radius
            );
    }

    private static int CircleInteraction(in PointF fCenter, in float fRadius, in PointF sCenter, in float sRadius)
    {
        float centerDist = BaseMath.Distance(in fCenter, in sCenter);
        float sumRadius = fRadius + sRadius;

        // Не пересекаются
        if (centerDist > sumRadius)
            return -1;

        // sc полностью содержит fc
        if (centerDist + fRadius <= sRadius)
            return 0;

        return 1; // пересекаются
    }

    public void Load_from_file(string file_path)
    {
        XDocument xdoc = XDocument.Load(file_path);

        Clear();

        XElement? loadMap = xdoc.Element("map");

        XElement? start = loadMap.Element("start");
        XElement? end = loadMap.Element("end");

        if (start is not null)
        {
            XAttribute? startX = start.Attribute("x");
            XAttribute? startY = start.Attribute("y");

            if (startX != null && startY != null)
            {
                m_startPoint = new PointF((float)startX, (float)startY);
                m_isStartEntered = true;
            }
        }

        if (end is not null)
        {
            XAttribute? endX = end.Attribute("x");
            XAttribute? endY = end.Attribute("y");

            if (endX != null && endY != null)
            {
                m_endPoint = new PointF((float)endX, (float)endY);
                m_isEndEntered = true;
            }
        }

        XElement? obstructions = loadMap.Element("obstructions");

        foreach (XElement obstructionElement in obstructions.Elements("obstruction"))
        {
            XElement? centerPointElement = obstructionElement.Element("center");
            XElement? radiusElement = obstructionElement.Element("radius");

            if (centerPointElement != null && radiusElement != null)
            {
                XAttribute? centerX = centerPointElement.Attribute("x");
                XAttribute? centerY = centerPointElement.Attribute("y");
                XAttribute? radius = radiusElement.Attribute("radius");

                if (centerX != null && centerY != null && radius != null)
                {
                    PointF c = new PointF((float)centerX, (float)centerY);
                    float r = float.Parse(radius.Value, CultureInfo.InvariantCulture.NumberFormat);

                    CircleObstacle obstruction = new CircleObstacle(c, r);

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
            XElement end = new ("end");

            XAttribute endX = new ("x", m_endPoint.X);
            XAttribute endY = new ("y", m_endPoint.Y);

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

        if (m_isDebugMode)
        {
            DrawGraph(ref g);
            DrawIntersection(ref g);
        }

        if (m_isPathGenerated)
        {
            DisplayPath(ref g);
        }
    }

    public void GenerateFullGraph()
    {
        if (m_isFullGraphGenerated)
            return;

        ObstaclesProcess(ref m_Obstructions);

        if (!m_isStartAndEndGenerated)
            CreatStartAndEndVertexesAndEdges();

        for (int i = 0; i < m_GraphObstructions.Count; ++i)
        {
            GenerateEdgesAndVertexes(i);
        }

        m_isFullGraphGenerated = true;
    }

    private void DisplayPath(ref Graphics g)
    {
        // Проходимся по всем вершинам, кроме последней - это начальная точка
        for (int i = 0; i < m_ShortesPathVertexIndexes.Count - 1; ++i)
        {
            int currVertexIndex = m_ShortesPathVertexIndexes[i];
            int nextVertexIndex = m_ShortesPathVertexIndexes[i + 1];

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
                g.FillEllipse(m_VertexBrush, m_GraphVertexes[currVertexIndex].m_position.X - 3, m_GraphVertexes[currVertexIndex].m_position.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
                g.FillEllipse(m_VertexBrush, m_GraphVertexes[nextVertexIndex].m_position.X - 3, m_GraphVertexes[nextVertexIndex].m_position.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
                g.DrawLine(m_DisplayPathPen, m_GraphVertexes[currVertexIndex].m_position, m_GraphVertexes[nextVertexIndex].m_position);
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

    private void DrawGraph(ref Graphics g)
    {
        foreach (GraphEdge edge in m_GraphEdges)
        {
            if (m_GraphVertexes[edge.m_firstVertexIndex].m_obstacleIndex == m_GraphVertexes[edge.m_secondVertexIndex].m_obstacleIndex)
                continue;

            g.FillEllipse(new SolidBrush(Color.Gray), m_GraphVertexes[edge.m_firstVertexIndex].m_position.X - 3, m_GraphVertexes[edge.m_firstVertexIndex].m_position.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
            g.FillEllipse(new SolidBrush(Color.Gray), m_GraphVertexes[edge.m_secondVertexIndex].m_position.X - 3, m_GraphVertexes[edge.m_secondVertexIndex].m_position.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);

            g.DrawLine(new Pen(Color.Gray, 2), m_GraphVertexes[edge.m_firstVertexIndex].m_position, m_GraphVertexes[edge.m_secondVertexIndex].m_position);
        }
    }

    private void DrawIntersection(ref Graphics g)
    {
        foreach (CircleObstacle obs in m_GraphObstructions)
        {
            List<PointFPair> lpfp = obs.m_Entersections;
            foreach (PointFPair pfp in lpfp)
            {
                g.FillEllipse(new SolidBrush(Color.Red), pfp.Item1.X - 3, pfp.Item1.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
                g.FillEllipse(new SolidBrush(Color.Red), pfp.Item2.X - 3, pfp.Item2.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
            }

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
                m_Obstructions.Add(new CircleObstacle(mouse_position));
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

        ClearGraph();
    }

    public bool RunA()
    {
        if (m_isPathGenerated)
            return true;

        if (!m_isStartEntered || !m_isEndEntered)
            return false;

        ObstaclesProcess(ref m_Obstructions);

        VertexPriorityQueue pq = new VertexPriorityQueue();
        //MyPriorityQueue Q = new MyPriorityQueue();

        if (!m_isStartAndEndGenerated)
            CreatStartAndEndVertexesAndEdges();

        //  Добавляем начальную вершину в очередь
        pq.Push(m_GraphVertexes[0]);

        while (!pq.Empty())
        {
            // Получем индекс рассматриваемой вершины из очереди с приорететами, как вершины с минимальной оценкой
            // до конечной точки (как самая перспективная)
            GraphVertex currentVertex = pq.Pop();

            // Если данная вершина является конечной точкой, то заканчиваем
            if (currentVertex == m_GraphVertexes[1])
            {
                FormPath();
                return true;
            }

            // Если вершина не начальная, то генерируем для препятствия, которому она принадлежит ребра перехода с остальными препятствмиями
            // генерируем огибающие ребра между всеми вершинами принадлежащими данному препрятствию
            if (currentVertex != m_GraphVertexes[0] && !m_isFullGraphGenerated)
                GenerateEdgesAndVertexes(currentVertex.m_obstacleIndex);

            // Обрабатываем все вершины инцентедентные с текущей рассматриваемой.
            // Устанавливаем ее как родителя для инцендентной, если пусть до нее через текущую оказался короче, чем уже имеющийся 
            // или если это вершина достигнута впервые
            foreach (int edgeIndex in currentVertex.m_incidentEdgeIndexes)
            {
                GraphEdge currentEdge = m_GraphEdges[edgeIndex];
                // Длина пути до индендентной вершины = длина пути до текущей + длина ребра
                float toNextVertexPathLength = currentVertex.m_G + currentEdge.m_lenght;

                int nextVertexIndex;
                int currentVertexIndex;
                if (m_GraphVertexes[currentEdge.m_firstVertexIndex] == currentVertex)
                {
                    nextVertexIndex = currentEdge.m_secondVertexIndex;
                    currentVertexIndex = currentEdge.m_firstVertexIndex;
                }
                else
                {
                    currentVertexIndex = currentEdge.m_secondVertexIndex;
                    nextVertexIndex = currentEdge.m_firstVertexIndex;
                }

                GraphVertex nextVertex = m_GraphVertexes[nextVertexIndex];

                // Если вершина достигнута впервые или найден более короткий путь
                // Устанавливаем текущую вершину как родителя и добавляем в очередь с приорететом, если она уже не там
                if (nextVertex.m_G == -1 || toNextVertexPathLength < nextVertex.m_G)
                {
                    nextVertex.setParent(currentVertexIndex, toNextVertexPathLength);

                    if (!pq.Have(nextVertex))
                       pq.Push(nextVertex);
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
            m_ShortesPathVertexIndexes.Add(currentVertexIndex);
            currentVertexIndex = m_GraphVertexes[currentVertexIndex].m_parentVertexIndex;
        }

        m_isPathGenerated = true;
    }

    public void ClearGraph()
    {
        m_GraphVertexes.Clear();
        m_GraphEdges.Clear();

        m_ShortesPathVertexIndexes.Clear();

        m_isFullGraphGenerated = false;
        m_isPathGenerated = false;
        m_isStartAndEndGenerated = false;

        for (int i = 0; i < m_Obstructions.Count; ++i)
        {
            m_Obstructions[i].m_isEdgesGenerated = false;
            m_Obstructions[i].m_isBlocked = false;
            m_Obstructions[i].m_VertexIndexes.Clear();
            m_Obstructions[i].m_Entersections.Clear();
            m_Obstructions[i].m_IntersectCircleIndexes.Clear();
        }
    }

    private int TryGrabObstruction(PointF location)
    {
        int count = m_Obstructions.Count();

        int index = -1;
        float minDistToObstruction = float.MaxValue;

        for (int i = 0; i < count; ++i)
        {
            float dist = BaseMath.Distance(in m_Obstructions[i].m_center, in location);
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
        PointF placeholder = new ();
        m_GraphVertexes.Add(new GraphVertex(placeholder, -1, placeholder));
        m_GraphVertexes.Add(new GraphVertex(placeholder, -1, placeholder));

        if (m_isStartEntered)
        {
            // index 0 - Начальная точка
            m_GraphVertexes[0] = new GraphVertex(m_startPoint, -1, m_endPoint);
            m_GraphVertexes[0].setParent(-1, 0);

            if (PointFree(m_startPoint))
                GenerateEdgesFromPointToAll(0);
        }

        if (m_isEndEntered)
        {
            // index 1 - конечная точка
            m_GraphVertexes[1] = new GraphVertex(m_endPoint, -1, m_endPoint);

            if (PointFree(m_endPoint))
                GenerateEdgesFromPointToAll(1);
        }

        if (m_isStartEntered && m_isEndEntered)
        {
            // ребро между начальной и конечной точками
            if (!IsObstructionsBlockEdge(new Edge(m_startPoint, m_endPoint), -1, -1))
            {
                int startToEndEdgeIndex = m_GraphEdges.Count;
                GraphEdge startToEnd = new GraphEdge(0, 1, BaseMath.Distance(in m_startPoint, in m_endPoint));
                m_GraphEdges.Add(startToEnd);

                m_GraphVertexes[0].m_incidentEdgeIndexes.Add(startToEndEdgeIndex);
                m_GraphVertexes[1].m_incidentEdgeIndexes.Add(startToEndEdgeIndex);
            }
        }

        m_isStartAndEndGenerated = true;
    }

    private bool PointFree(in PointF point)
    {
        foreach (Circle obs in m_GraphObstructions)
        {
            if (BaseMath.PointInCircle(in point, in obs.m_center, obs.m_radius))
                return false;
        }
        return true;
    }

    // Только для начальной и конечной точки маршрута
    private void GenerateEdgesFromPointToAll(int vertexIndex)
    {
        PointF point = m_GraphVertexes[vertexIndex].m_position;
        for (int i = 0; i < m_GraphObstructions.Count; ++i)
        {
            if (m_GraphObstructions[i].m_isBlocked)
                continue;

            List<Edge> edges = ExternalBitangents(point, 0, m_GraphObstructions[i].m_center, m_GraphObstructions[i].m_radius);

            foreach (Edge e in edges)
            {
                if (!IsObstructionsBlockEdge(e, -1, i))
                {
                    int newEdgeIndex = m_GraphEdges.Count;

                    GraphVertex newVertex = new GraphVertex(e.m_second, i, point);
                    newVertex.m_incidentEdgeIndexes.Add(newEdgeIndex);

                    m_GraphVertexes[vertexIndex].m_incidentEdgeIndexes.Add(newEdgeIndex);

                    int newVertexIndex = m_GraphVertexes.Count;
                    m_GraphVertexes.Add(newVertex);

                    m_GraphObstructions[i].m_VertexIndexes.Add(newVertexIndex);

                    GraphEdge newEdge = new GraphEdge(vertexIndex, newVertexIndex, BaseMath.Distance(in point, in e.m_second));

                    m_GraphEdges.Add(newEdge);
                }
            }
        }
    }

    private void GenerateEdgesAndVertexes(int obstacleIndex)
    {
        if (m_GraphObstructions[obstacleIndex].m_isEdgesGenerated || m_GraphObstructions[obstacleIndex].m_isBlocked)
            return;

        for (int i = 0; i < m_GraphObstructions.Count; ++i)
        {
            if (i == obstacleIndex || m_GraphObstructions[i].m_isEdgesGenerated || m_GraphObstructions[i].m_isBlocked)
                continue;

            List<Edge> surfing = SurfingEdges(obstacleIndex, i);

            foreach (Edge sEdge in surfing)
            {
                int currentEdgeIndex = m_GraphEdges.Count;

                GraphVertex firstVertex = new GraphVertex(sEdge.m_first, obstacleIndex, m_endPoint);
                firstVertex.m_incidentEdgeIndexes.Add(currentEdgeIndex);
                int firstVertexIndex = m_GraphVertexes.Count;
                m_GraphVertexes.Add(firstVertex);
                m_GraphObstructions[obstacleIndex].m_VertexIndexes.Add(firstVertexIndex);

                GraphVertex secondVertex = new GraphVertex(sEdge.m_second, i, m_endPoint);
                secondVertex.m_incidentEdgeIndexes.Add(currentEdgeIndex);
                int secondVertexIndex = m_GraphVertexes.Count;
                m_GraphVertexes.Add(secondVertex);
                m_GraphObstructions[i].m_VertexIndexes.Add(secondVertexIndex);

                GraphEdge newEdgeFS = new GraphEdge(firstVertexIndex, secondVertexIndex,
                    BaseMath.Distance(in firstVertex.m_position, in secondVertex.m_position));
                m_GraphEdges.Add(newEdgeFS);
            }
        }

        List<GraphEdge> hugging = CreateHuggingEdges(obstacleIndex);

        foreach (GraphEdge hEdge in hugging)
        {
            int currentEdgeIndex = m_GraphEdges.Count;
            m_GraphEdges.Add(hEdge);

            m_GraphVertexes[hEdge.m_firstVertexIndex].m_incidentEdgeIndexes.Add(currentEdgeIndex);
            m_GraphVertexes[hEdge.m_secondVertexIndex].m_incidentEdgeIndexes.Add(currentEdgeIndex);
        }

        m_GraphObstructions[obstacleIndex].m_isEdgesGenerated = true;
    }

    private static List<Edge> InternalBitangents(PointF centerA, float rA, PointF centerB, float rB)
    {
        float Q = (float)Math.Acos((double)(rA + rB) / BaseMath.Distance(in centerA, in centerB));

        float vectABX = centerB.X - centerA.X;
        float vectABY = centerB.Y - centerA.Y;

        PointF VectAB = new (vectABX, vectABY);

        float vectABLen = BaseMath.VectorLenght(in VectAB);

        VectAB.X /= vectABLen;
        VectAB.Y /= vectABLen;

        PointF G = new(centerA.X + rA * VectAB.X, centerA.Y + rA * VectAB.Y);

        PointF H = new(centerB.X + rB * (-VectAB.X), centerB.Y + rB * (-VectAB.Y));

        PointF C = BaseMath.RotatePoint(ref G, ref centerA, -Q);
        PointF D = BaseMath.RotatePoint(ref G, ref centerA, Q);

        PointF E = BaseMath.RotatePoint(ref H, ref centerB, Q);
        PointF F = BaseMath.RotatePoint(ref H, ref centerB, -Q);
        List<Edge> list = new()
        {
                new Edge(C, F),
                new Edge(D, E)
            };

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

        for (int i = 0; i < m_GraphObstructions.Count; ++i)
        {
            if (i == firstIgnore || i == secondIgnore)
                continue;

            if (Is_Block(m_GraphObstructions[i], edgeStart, edgeEnd))
                return true;
        }
        return false;
    }

    private List<Edge> SurfingEdges(int firstObstIndex, int secondObstIndex)
    {
        PointF centerA = m_GraphObstructions[firstObstIndex].m_center;
        float rA = m_GraphObstructions[firstObstIndex].m_radius;

        PointF centerB = m_GraphObstructions[secondObstIndex].m_center;
        float rB = m_GraphObstructions[secondObstIndex].m_radius;

        List<Edge> edges = new ();

        // Проверяем, что препятствия не пересекаются
        int intersectIndex = m_GraphObstructions[firstObstIndex].m_IntersectCircleIndexes.IndexOf(secondObstIndex);
        // Тогда генерируем внутренние касательные
        if (intersectIndex == -1)
        {
            List<Edge> internalBit = InternalBitangents(centerA, rA, centerB, rB);

            for (int i = 0; i < internalBit.Count; ++i)
            {
                if (!IsObstructionsBlockEdge(internalBit[i], firstObstIndex, secondObstIndex))
                    edges.Add(internalBit[i]);
            }
        }

        List<Edge> externalBit = ExternalBitangents(centerA, rA, centerB, rB);

        for (int i = 0; i < externalBit.Count; ++i)
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

    private GraphEdge CreateHuggingEdge(int obstacleIndex, int firstVertexIndex, int secondVertexIndex)
    {
        float edgeLenght = CalculateHuggingEdgeLenght(
            m_GraphObstructions[obstacleIndex].m_center,
            m_GraphObstructions[obstacleIndex].m_radius,
            m_GraphVertexes[firstVertexIndex].m_position,
            m_GraphVertexes[secondVertexIndex].m_position
            );

        GraphEdge edge = new GraphEdge(firstVertexIndex, secondVertexIndex, edgeLenght);
        return edge;
    }

    private List<GraphEdge> CreateHuggingEdges(int obstacleIndex)
    {
        List<GraphEdge> edges = new ();
        List<int> obstacleVertexes = m_GraphObstructions[obstacleIndex].m_VertexIndexes;

        PointF center = m_GraphObstructions[obstacleIndex].m_center;

        for (int i = 0; i < obstacleVertexes.Count; ++i)
            for (int j = i + 1; j < obstacleVertexes.Count; ++j)
            {
                int firstVertexIndex = obstacleVertexes[i];
                int secondVertexIndex = obstacleVertexes[j];

                bool isAchievable = true;

                PointF startV = new (
                    m_GraphVertexes[firstVertexIndex].m_position.X - center.X,
                    m_GraphVertexes[firstVertexIndex].m_position.Y - center.Y
                );

                PointF endV = new (
                    m_GraphVertexes[secondVertexIndex].m_position.X - center.X,
                    m_GraphVertexes[secondVertexIndex].m_position.Y - center.Y
                );

                float edgeAngle = BaseMath.AngleBetweenVectors(ref startV, ref endV);

                foreach (PointFPair pfp in m_GraphObstructions[obstacleIndex].m_Entersections)
                {
                    PointF ppfV = new (
                        pfp.Item1.X - center.X,
                        pfp.Item1.Y - center.Y
                    );

                    PointF ppsV = new (
                        pfp.Item2.X - center.X,
                        pfp.Item2.Y - center.Y
                    );

                    float toPpfAngle = BaseMath.AngleBetweenVectors(ref startV, ref ppfV);
                    float toPpsAngle = BaseMath.AngleBetweenVectors(ref startV, ref ppsV);

                    if (CompareAngles(edgeAngle, toPpfAngle) || CompareAngles(edgeAngle, toPpsAngle))
                    {
                        isAchievable = false;
                        break;
                    }

                }

                if (isAchievable)
                    edges.Add(CreateHuggingEdge(obstacleIndex, firstVertexIndex, secondVertexIndex));
            }

        return edges;
    }

    private static bool CompareAngles(float fAngle, float sAngle)
    {
        // Если углы имеют одинаковый знак
        if (fAngle * sAngle > 0)
        {
            return Math.Abs(fAngle) > Math.Abs(sAngle);
        }

        return false;
    }

    //private List<Edge> externalBitangents(Circle a, Circle b)
    //{
    //    PointF centerA = a.m_center;
    //    PointF centerB = b.m_center;

    //    float rA = a.m_radius;
    //    float rB = b.m_radius;

    //    return externalBitangents(centerA, rA, centerB, rB);
    //}

    private static List<Edge> ExternalBitangents(PointF centerA, float rA, PointF centerB, float rB)
    {
        float Q = (float)Math.Acos((double)Math.Abs(rA - rB) / BaseMath.Distance(in centerA, in centerB));

        float vectBLX = centerB.X - centerA.X;
        float vectBLY = centerB.Y - centerA.Y;

        if (rB > rA)
        {
            vectBLX *= -1;
            vectBLY *= -1;
        }

        // вектор из центра большего препятствия в центр меньшего, если равны то не важно
        PointF VectBL = new (vectBLX, vectBLY);

        float vectBLLen = BaseMath.VectorLenght(in VectBL);

        VectBL.X /= vectBLLen;
        VectBL.Y /= vectBLLen;

        PointF G = new (centerA.X + rA * VectBL.X, centerA.Y + rA * VectBL.Y);

        PointF H = new (centerB.X + rB * VectBL.X, centerB.Y + rB * VectBL.Y);

        PointF C = BaseMath.RotatePoint(ref G, ref centerA, -Q);
        PointF D = BaseMath.RotatePoint(ref G, ref centerA, Q);

        PointF E = BaseMath.RotatePoint(ref H, ref centerB, Q);
        PointF F = BaseMath.RotatePoint(ref H, ref centerB, -Q);
        List<Edge> list = new ()
        {
            new Edge(C, F),
            new Edge(D, E)
        };

        return list;
    }

    private static bool Is_Block(Circle obstruct, PointF A, PointF B)
    {
        PointF C = obstruct.m_center;

        float u = ((C.X - A.X) * (B.X - A.X) + (C.Y - A.Y) * (B.Y - A.Y)) / BaseMath.Square_distance(in B, in A);

        float clamp_u = BaseMath.Clamp(u, 0, 1);

        float Ex = A.X + clamp_u * (B.X - A.X);
        float Ey = A.Y + clamp_u * (B.Y - A.Y);

        PointF E = new PointF(Ex, Ey);

        float Sd = BaseMath.Square_distance(in E, in C);

        return Sd < (obstruct.m_radius * obstruct.m_radius);
    }

    private static float CalculateHuggingEdgeLenght(PointF circleCenter, float circleRadius, PointF firstPoint, PointF secondPoint)
    {
        PointF firstVector = new(
            firstPoint.X - circleCenter.X,
            firstPoint.Y - circleCenter.Y
        );

        PointF secondVector = new(
            secondPoint.X - circleCenter.X,
            secondPoint.Y - circleCenter.Y
        );

        float angle = Math.Abs(BaseMath.AngleBetweenVectors(ref firstVector, ref secondVector));

        float huggingEdgeLenght = circleRadius * angle;
        return huggingEdgeLenght;
    }

}