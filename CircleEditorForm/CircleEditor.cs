using System.Globalization;
using System.Xml.Linq;

using BaseStruct;
using GraphStruct;

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

    bool m_isMapChanged = false;

    int m_creatingIndex = -1;
    int m_editingIndex = -1;

    Graph m_Graph = new ();

    Pen m_obstructionsPen = new Pen(Color.DeepSkyBlue, 2);

    public CircleEditor() { }

    public void GenerateFullGraph()
    {
        UpdateGraphMap();
        m_Graph.GenerateFull();
    }

    public void RunA()
    {
        UpdateGraphMap();
        m_Graph.RunA();
    }

    public void ResetMap()
    {
        m_isMapChanged = true;
    }

    public void Clear()
    {
        m_Obstructions.Clear();

        m_Graph.Drop();

        m_isStartEntered = false;
        m_isEndEntered = false;

        m_creatingIndex = -1;
        m_editingIndex = -1;

        m_Graph.DropStartAndEnd();

        m_isMapChanged = true;
    }

    public void MapIsChanged()
    {
        m_isMapChanged = true;
        m_Graph.Clear();
    }

    public bool AddObstruction(ref Circle obs)
    {
        foreach(Circle mapObs in m_Obstructions)
        {
            if (obs.Interaction(mapObs) != -1)
                return false;
        }

        m_Obstructions.Add(obs);
        m_isMapChanged = true;

        return true;
    }

    public void SetDebugMode(bool isDebugMode)
    {
        m_Graph.SetDebug(isDebugMode);
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
                m_Graph.SetStart(ref m_startPoint);
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
                m_Graph.SetEnd(ref m_endPoint);
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

        m_isMapChanged = true;
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

    //! Draw START

    public void Draw(ref Graphics g)
    {
        DrawObstructions(ref g);
        m_Graph.Draw(ref g);
    }

    private void UpdateGraphMap()
    {
        if (m_isMapChanged)
        {
            m_Graph.SetMap(m_Obstructions);
            m_isMapChanged = false;
        }
        else
        {
            m_Graph.Clear();
        }
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

    public bool MouseMove(PointF mouse_position)
    {
        switch (m_currentMode)
        {
            case EditorMode.Creating:
                m_Obstructions[m_creatingIndex].SetRadius(mouse_position);
                return true;
                
            case EditorMode.Moving:
                m_Obstructions[m_editingIndex].SetCenter(mouse_position);
                //MapIsChanged();
                return true;
            case EditorMode.Editing:
                m_Obstructions[m_editingIndex].SetRadius(mouse_position);
                //MapIsChanged();
                return true;
        }

        return false;
    }

    public void MouseDownLeft(PointF mouse_position)
    {
        switch (m_currentMode)
        {
            case EditorMode.Create:
                m_creatingIndex = m_Obstructions.Count();
                m_Obstructions.Add(new Circle(mouse_position));
                m_currentMode = EditorMode.Creating;
                MapIsChanged();
                break;
            case EditorMode.Creating:
                m_Obstructions[m_creatingIndex].SetRadius(mouse_position);
                m_creatingIndex = -1;
                m_currentMode = EditorMode.Create;
                MapIsChanged();
                break;
            case EditorMode.Remove:
                int removeIndex = TryGrabObstruction(mouse_position);
                if (removeIndex > -1)
                {
                    m_Obstructions.Remove(m_Obstructions[removeIndex]);
                }
                MapIsChanged();
                break;
            case EditorMode.Edit:
                m_editingIndex = TryGrabObstruction(mouse_position);
                if (m_editingIndex > -1)
                {
                    m_currentMode = EditorMode.Moving;
                }
                MapIsChanged();
                break;
            case EditorMode.Moving:
                m_Obstructions[m_editingIndex].SetCenter(mouse_position);
                m_editingIndex = -1;
                m_currentMode = EditorMode.Edit;
                MapIsChanged();
                break;
            case EditorMode.SetStart:
                m_startPoint = mouse_position;
                m_Graph.SetStart(ref m_startPoint);
                m_isStartEntered = true;
                MapIsChanged();
                break;
            case EditorMode.SetEnd:
                m_endPoint = mouse_position;
                m_Graph.SetEnd(ref m_endPoint);
                m_isEndEntered = true;
                MapIsChanged();
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
                }
                MapIsChanged();
                break;
            case EditorMode.Editing:
                m_Obstructions[m_editingIndex].SetRadius(mouse_position);
                m_currentMode = EditorMode.Edit;
                MapIsChanged();
                break;
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

}