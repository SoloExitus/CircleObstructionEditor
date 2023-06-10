using BaseStruct;
using GraphStruct;

using PointFPair = System.Tuple<System.Drawing.PointF, System.Drawing.PointF>;

class Graph
{
    public Graph()
    {
        m_OstructionsPen.DashPattern = new float[]{ 4.0f, 4.0f, 4.0f, 4.0f};
    }

    List<CircleObstacle> m_GraphObstructions = new List<CircleObstacle>();

    PointF m_StartPoint;
    PointF m_EndPoint;

    List<GraphVertex> m_GraphVertexes = new List<GraphVertex>();
    List<GraphEdge> m_GraphEdges = new List<GraphEdge>();

    bool m_isStartEntered = false;
    bool m_isEndEntered = false;

    bool m_isFullGraphGenerated = false;
    bool m_isShortestPathFind = false;
    bool m_isStartAndEndGenerated = false;

    // Debug Flags
    bool m_isDrawFullGraph = false;
    bool m_isDrawTrueObstacles = false;
    bool m_isDrawIntersectionPoints = false;

    List<int> m_ShortesPathVertexIndexes = new List<int>();

    int m_ActorRadius = 0;

    //! ----------------------------------- Start_Draw_Section ----------------------------------------
    SolidBrush m_startPointBrush = new SolidBrush(Color.Green);
    SolidBrush m_endPointBrush = new SolidBrush(Color.Red);

    SolidBrush m_DebugObstructionsCentersBrush = new SolidBrush(Color.Brown);
    SolidBrush m_DebugIntersectionsPointsBrush = new SolidBrush(Color.Coral);

    Pen m_DisplayPathPen = new Pen(Color.DarkRed, 4);

    Pen m_OstructionsPen = new Pen(Color.Blue, 4);

    Pen m_EdgesPen = new Pen(Color.Gray, 3);

    SolidBrush m_AllVertexBrush = new SolidBrush(Color.Gray);

    SolidBrush m_PathVertexBrush = new SolidBrush(Color.Blue);
    float m_VertexDispalayDiameter = 6;
    //! ----------------------------------- End_Draw_Section ------------------------------------------

    public void SetStart(ref PointF start)
    {
        m_StartPoint = start;
        m_isStartEntered = true;
    }

    public void SetEnd(ref PointF end)
    {
        m_EndPoint = end;
        m_isEndEntered = true;
    }

    public void SetMap(List<Circle> map)
    {
        Drop();
        foreach(Circle c in map)
        {
            CircleObstacle newCircleObstacle = new(c);
            newCircleObstacle.m_radius += m_ActorRadius;
            m_GraphObstructions.Add(newCircleObstacle);
        }

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

    public void DrawFullGraph(bool draw)
    {
        m_isDrawFullGraph = draw;
    }

    public void DrawTrueObstacles(bool draw)
    {
        m_isDrawTrueObstacles = draw;
    }

    public void DrawIntersectionPoints(bool draw)
    {
        m_isDrawIntersectionPoints = draw;
    }

    public void SetActorRadius(int radius)
    {
        m_ActorRadius = radius;
    }

    public void DropStartAndEnd()
    {
        m_isStartEntered = false;
        m_isEndEntered = false;
    }

    public void Drop()
    {
        m_GraphObstructions.Clear();

        m_GraphVertexes.Clear();
        m_GraphEdges.Clear();

        m_ShortesPathVertexIndexes.Clear();

        m_isFullGraphGenerated = false;
        m_isStartAndEndGenerated = false;
        m_isShortestPathFind = false;
    }

    public void Clear()
    {
        m_GraphVertexes.Clear();
        m_GraphEdges.Clear();

        m_ShortesPathVertexIndexes.Clear();

        m_isFullGraphGenerated = false;
        m_isShortestPathFind = false;
        m_isStartAndEndGenerated = false;

        foreach (CircleObstacle co in m_GraphObstructions)
        {
            co.m_isEdgesGenerated = false;
            co.m_VertexIndexes.Clear();
        }
    }

    private bool ObstacleNeedAndIntersect(int currentObstacle, in Circle obs, out List<int> intersectIndexes)
    {
        intersectIndexes = new List<int>();

        for (int i = 0; i < m_GraphObstructions.Count; ++i)
        {
            if (currentObstacle == i)
                continue;

            int res = obs.Interaction(m_GraphObstructions[i]);

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

            float teta = MathF.Acos(a / obs.m_radius);

            PointF vecAB = new (intersectObs.m_center.X - obs.m_center.X, intersectObs.m_center.Y - obs.m_center.Y);
            float vecABLenght = BaseMath.VectorLenght(vecAB);
            vecAB.X /= vecABLenght;
            vecAB.Y /= vecABLenght;

            PointF c = obs.m_center;
            c.X += vecAB.X * obs.m_radius;
            c.Y += vecAB.Y * obs.m_radius;

            PointF first = BaseMath.RotatePoint(c, obs.m_center, teta);
            PointF second = BaseMath.RotatePoint(c, obs.m_center, -teta);

            obs.m_Intersections.Add(new PointFPair(first, second));
            obs.m_IntersectCircleIndexes.Add(interIndex);
        }
    }

    public void GenerateFull()
    {
        if (m_isFullGraphGenerated)
            return;

        if (!m_isStartAndEndGenerated)
            CreatStartAndEndVertexesAndEdges();

        for (int i = 0; i < m_GraphObstructions.Count; ++i)
        {
            GenerateEdgesAndVertexes(i);
        }

        m_isFullGraphGenerated = true;
    }

    public int GetEdgesCount()
    {
        return m_GraphEdges.Count;
    }

    public int GetVertexesCount()
    {
        return m_GraphVertexes.Count;
    }

    //! ------------------------------------------- Start_Draw_Section ------------------------------------------

    public void Draw(ref Graphics g)
    {
        DrawStartAndEndPoints(ref g);

        if (m_isDrawTrueObstacles)
        {
            DrawObstructions(ref g);
        }

        if (m_isDrawFullGraph)
        {
            DrawGraph(ref g);
        }

        if (m_isDrawIntersectionPoints)
        {
            DrawIntersection(ref g);
        }

        if (m_isShortestPathFind)
        {
            DisplayPath(ref g);
        }
    }

    private void DisplayPath(ref Graphics g)
    {
        int currVertexIndex = 1;

        PointF OX = new(1, 0);

        while (currVertexIndex != 0)
        {
            GraphVertex currentVertex = m_GraphVertexes[currVertexIndex];
            int prevVertexIndex = currentVertex.m_parentVertexIndex;
            GraphVertex previousVertex = m_GraphVertexes[prevVertexIndex];

            // Рисуем ребро между двумя вершинами, которое может быть ребром перехода или огибающем ребром
            if (currentVertex.m_obstacleIndex == previousVertex.m_obstacleIndex && prevVertexIndex != 0)
            {
                CircleObstacle obs = m_GraphObstructions[currentVertex.m_obstacleIndex];
                PointF center = obs.m_center;
                float radius = obs.m_radius;

                PointF firstVector = new(previousVertex.m_position.X - center.X, previousVertex.m_position.Y - center.Y);
                PointF secondVector = new(currentVertex.m_position.X - center.X, currentVertex.m_position.Y - center.Y);

                float startAngleRad;
                float sweepAngleRad;

                startAngleRad = BaseMath.AngleBetweenVectors(OX, firstVector);
                sweepAngleRad = BaseMath.AngleBetweenVectors(firstVector, secondVector);

                float startAngle = BaseMath.ToDegrees(startAngleRad);
                float sweepAngle = BaseMath.ToDegrees(sweepAngleRad);

                if ( MathF.Abs(sweepAngle) > 1.0)
                {
                    g.DrawArc(m_DisplayPathPen,
                    center.X - radius, center.Y - radius,
                    radius * 2, radius * 2,
                    startAngle, sweepAngle
                    );
                }
            }
            else
            {
                g.DrawLine(m_DisplayPathPen, previousVertex.m_position, currentVertex.m_position);
                g.FillEllipse(m_PathVertexBrush, currentVertex.m_position.X - 3, currentVertex.m_position.Y - m_VertexDispalayDiameter / 2,
                    m_VertexDispalayDiameter, m_VertexDispalayDiameter);
                g.FillEllipse(m_PathVertexBrush, previousVertex.m_position.X - 3, previousVertex.m_position.Y - m_VertexDispalayDiameter / 2,
                    m_VertexDispalayDiameter, m_VertexDispalayDiameter);
            }

            currVertexIndex = prevVertexIndex;
        }
    }

    private void DrawStartAndEndPoints(ref Graphics g)
    {
        int PointsRadius = 8;

        if (m_isStartEntered)
            g.FillEllipse(m_startPointBrush, m_StartPoint.X - PointsRadius, m_StartPoint.Y - PointsRadius, PointsRadius * 2, PointsRadius * 2);

        if (m_isEndEntered)
            g.FillEllipse(m_endPointBrush, m_EndPoint.X - PointsRadius, m_EndPoint.Y - PointsRadius, PointsRadius * 2, PointsRadius * 2);
    }

    private void DrawGraph(ref Graphics g)
    {
        foreach (GraphEdge edge in m_GraphEdges)
        {
            int firstObstacleIndex = m_GraphVertexes[edge.m_firstVertexIndex].m_obstacleIndex;
            int secondObstacleIndex = m_GraphVertexes[edge.m_secondVertexIndex].m_obstacleIndex;
            if (firstObstacleIndex == secondObstacleIndex && firstObstacleIndex != -1)
                continue;

            g.FillEllipse(m_AllVertexBrush, m_GraphVertexes[edge.m_firstVertexIndex].m_position.X - m_VertexDispalayDiameter / 2, m_GraphVertexes[edge.m_firstVertexIndex].m_position.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
            g.FillEllipse(m_AllVertexBrush, m_GraphVertexes[edge.m_secondVertexIndex].m_position.X - m_VertexDispalayDiameter / 2, m_GraphVertexes[edge.m_secondVertexIndex].m_position.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);

            g.DrawLine(m_EdgesPen, m_GraphVertexes[edge.m_firstVertexIndex].m_position, m_GraphVertexes[edge.m_secondVertexIndex].m_position);
        }
    }

    private void DrawObstructions(ref Graphics g)
    {
        foreach(CircleObstacle obs in m_GraphObstructions)
        {
            PointF center = obs.m_center;
            float radius = obs.m_radius;
            g.DrawEllipse(m_OstructionsPen, center.X - radius, center.Y - radius, radius * 2, radius * 2);
        }
    }

    private void DrawIntersection(ref Graphics g)
    {
        foreach (CircleObstacle obs in m_GraphObstructions)
        {
            List<PointFPair> lpfp = obs.m_Intersections;

            g.FillEllipse(m_DebugObstructionsCentersBrush, obs.m_center.X - m_VertexDispalayDiameter / 2, obs.m_center.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);

            foreach (PointFPair pfp in lpfp)
            {
                g.FillEllipse(m_DebugIntersectionsPointsBrush, pfp.Item1.X - m_VertexDispalayDiameter / 2, pfp.Item1.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
                g.FillEllipse(m_DebugIntersectionsPointsBrush, pfp.Item2.X - m_VertexDispalayDiameter / 2, pfp.Item2.Y - m_VertexDispalayDiameter / 2, m_VertexDispalayDiameter, m_VertexDispalayDiameter);
            }

        }
    }
    //! ------------------------------------------- End_Draw_Section ------------------------------------------

    public bool RunA()
    {
        if (m_isShortestPathFind)
            return true;

        if (!m_isStartEntered || !m_isEndEntered)
            return false;

        VertexPriorityQueue pq = new VertexPriorityQueue();

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
                m_isShortestPathFind = true;
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

                int currentVertexIndex = currentEdge.m_firstVertexIndex;
                int nextVertexIndex = currentEdge.m_secondVertexIndex;

                if (m_GraphVertexes[nextVertexIndex] == currentVertex)
                {
                    int temp = nextVertexIndex;
                    nextVertexIndex = currentVertexIndex;
                    currentVertexIndex = temp;
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

    private void CreatStartAndEndVertexesAndEdges()
    {
        PointF placeholder = new();

        m_GraphVertexes.Add(new GraphVertex(placeholder, -1, placeholder));
        m_GraphVertexes.Add(new GraphVertex(placeholder, -1, placeholder));

        if (m_isStartEntered)
        {
            // index 0 - Начальная точка
            m_GraphVertexes[0] = new GraphVertex(m_StartPoint, -1, m_EndPoint);
            m_GraphVertexes[0].setParent(-1, 0);

            if (PointFree(m_StartPoint))
                CreateEdgesAndVertexesFromVertex(0);
        }

        if (m_isEndEntered)
        {
            // index 1 - конечная точка
            m_GraphVertexes[1] = new GraphVertex(m_EndPoint, -1, m_EndPoint);

            if (PointFree(m_EndPoint))
                CreateEdgesAndVertexesFromVertex(1);
        }

        if (m_isStartEntered && m_isEndEntered)
        {
            // ребро между начальной и конечной точками
            if (!IsObstructionsBlockEdge(new Edge(m_StartPoint, m_EndPoint), -1, -1))
            {
                int startToEndEdgeIndex = m_GraphEdges.Count;
                GraphEdge startToEnd = new GraphEdge(0, 1, BaseMath.Distance(in m_StartPoint, in m_EndPoint));
                m_GraphEdges.Add(startToEnd);

                m_GraphVertexes[0].m_incidentEdgeIndexes.Add(startToEndEdgeIndex);
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

    private void CreateEdgesAndVertexesFromVertex(int vertexIndex)
    {
        GraphVertex vertex = m_GraphVertexes[vertexIndex];
        PointF point = vertex.m_position;

        for (int i = 0; i < m_GraphObstructions.Count; ++i)
        {
            CircleObstacle obs = m_GraphObstructions[i];

            if (obs.m_isBlocked)
                continue;

            List<Edge> edges = BaseMath.InternalBitangents(point, 0, obs.m_center, obs.m_radius);

            for(int j=0; j < edges.Count; ++j)
            {
                Edge e = edges[j];

                if (IsObstructionsBlockEdge(e, -1, i))
                    continue;

                PointF secondPoint = e.m_second;

                int secondVertexIndex = AddGraphVertex(secondPoint, i);

                float edgeLenght = BaseMath.Distance(point, secondPoint);

                AddGraphEdge(vertexIndex, secondVertexIndex, edgeLenght);
            }
        }
    }

    private void CreateExternalEdgesAndVertexes(int firstObsIndex, int secondObsIndex)
    {
        CircleObstacle firstObs = m_GraphObstructions[firstObsIndex];
        CircleObstacle secondObs = m_GraphObstructions[secondObsIndex];

        List<Edge> edges = BaseMath.ExternalBitangents(firstObs, secondObs);

        for(int i =0; i < edges.Count; ++i)
        {
            Edge e = edges[i];
            if (IsObstructionsBlockEdge(e, firstObsIndex, secondObsIndex))
                continue;

            PointF firstPoint = e.m_first;
            PointF secondPoint = e.m_second;

            int firstVertexIndex = AddGraphVertex(firstPoint, firstObsIndex);
            int secondVertexIndex = AddGraphVertex(secondPoint, secondObsIndex);

            float edgeLenght = BaseMath.Distance(firstPoint, secondPoint);

            AddGraphEdge(firstVertexIndex, secondVertexIndex, edgeLenght);
        }
    }

    private int AddGraphVertex(PointF position, int obstacleIndex)
    {
        int vertexIndex = m_GraphVertexes.Count;
        GraphVertex vertex = new(position, obstacleIndex, m_EndPoint);
        m_GraphVertexes.Add(vertex);

        m_GraphObstructions[obstacleIndex].m_VertexIndexes.Add(vertexIndex);

        return vertexIndex;
    }

    private int AddGraphEdge(int firstVertexIndex, int secondVertexIndex, float edgeLenght)
    {
        int newEdgeIndex = m_GraphEdges.Count;

        m_GraphVertexes[firstVertexIndex].m_incidentEdgeIndexes.Add(newEdgeIndex);
        m_GraphVertexes[secondVertexIndex].m_incidentEdgeIndexes.Add(newEdgeIndex);

        GraphEdge newEdge = new(firstVertexIndex, secondVertexIndex, edgeLenght);

        m_GraphEdges.Add(newEdge);

        return newEdgeIndex;
    }

    private void CreateInternalEdgesAndVertexes(int firstObsIndex, int secondObsIndex)
    {
        CircleObstacle firstObs = m_GraphObstructions[firstObsIndex];
        CircleObstacle secondObs = m_GraphObstructions[secondObsIndex];

        List<Edge> edges = BaseMath.InternalBitangents(firstObs, secondObs);

        for (int i = 0; i < edges.Count; ++i)
        {
            Edge e = edges[i];
            if (IsObstructionsBlockEdge(e, firstObsIndex, secondObsIndex))
                continue;

            PointF firstPoint = e.m_first;
            PointF secondPoint = e.m_second;

            int firstVertexIndex = AddGraphVertex(firstPoint, firstObsIndex);
            int secondVertexIndex = AddGraphVertex(secondPoint, secondObsIndex);

            float edgeLenght = BaseMath.Distance(firstPoint, secondPoint);

            AddGraphEdge(firstVertexIndex, secondVertexIndex, edgeLenght);
        }
    }

    private void CreateHuggingEdges(int obstacleIndex)
    {
        CircleObstacle obs = m_GraphObstructions[obstacleIndex];

        List<int> obsVertexIndexes = obs.m_VertexIndexes;

        for (int i = 0; i < obsVertexIndexes.Count; ++i)
        {
            int firstVertexIndex = obsVertexIndexes[i];
            for (int j = i + 1; j < obsVertexIndexes.Count; ++j)
            {
                int secondVertexIndex = obsVertexIndexes[j];
                CreateHuggingEdges(obstacleIndex, firstVertexIndex, secondVertexIndex);
            }
        }
    }

    private void CreateHuggingEdges(int obstacleIndex, int firstVertexIndex, int secondVertexIndex)
    {
        CircleObstacle obs = m_GraphObstructions[obstacleIndex];
        float radius = obs.m_radius;

        GraphVertex firstVertex = m_GraphVertexes[firstVertexIndex];
        GraphVertex secondVertex = m_GraphVertexes[secondVertexIndex];

        PointF firstPoint = firstVertex.m_position;
        PointF secondPoint = secondVertex.m_position;

        float angleBetweenPoints = BaseMath.AngleBetweenPointsOnCircle(obs, firstPoint, secondPoint);

        bool isAchivable = true;

        foreach (PointFPair pfp in obs.m_Intersections)
        {
            PointF fp = pfp.Item1;
            PointF sp = pfp.Item2;
            
            float startToFpAngle = BaseMath.AngleBetweenPointsOnCircle(obs, firstPoint, fp);
            float startToSpAngle = BaseMath.AngleBetweenPointsOnCircle(obs, firstPoint, sp);

            if (BaseMath.CompareAngles(angleBetweenPoints, startToFpAngle) || BaseMath.CompareAngles(angleBetweenPoints, startToSpAngle))
            {
                isAchivable = false;
                break;
            }
            
        }

        if (isAchivable)
        {
            int newEdgeIndex = m_GraphEdges.Count;
            GraphEdge huggingEdge = new(firstVertexIndex, secondVertexIndex, BaseMath.CircleArcLenght(radius, angleBetweenPoints));
            m_GraphEdges.Add(huggingEdge);
            firstVertex.m_incidentEdgeIndexes.Add(newEdgeIndex);
            secondVertex.m_incidentEdgeIndexes.Add(newEdgeIndex);
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

            CreateExternalEdgesAndVertexes(obstacleIndex, i);

            //Проверяем, что препятствия не пересекаются
            int intersectIndex = m_GraphObstructions[obstacleIndex].m_IntersectCircleIndexes.IndexOf(i);
            // Тогда генерируем внутренние касательные
            if (intersectIndex == -1)
                CreateInternalEdgesAndVertexes(obstacleIndex, i);
        }

        CreateHuggingEdges(obstacleIndex);
        m_GraphObstructions[obstacleIndex].m_isEdgesGenerated = true;
    }

    private bool IsObstructionsBlockEdge(in Edge edge, int firstIgnore, int secondIgnore)
    {
        PointF edgeStart = edge.m_first;
        PointF edgeEnd = edge.m_second;
        for (int i = 0; i < m_GraphObstructions.Count; ++i)
        {
            if (i == firstIgnore || i == secondIgnore)
                continue;
            if (BaseMath.Is_Block(m_GraphObstructions[i], edgeStart, edgeEnd))
                return true;
        }
        return false;
    }
}