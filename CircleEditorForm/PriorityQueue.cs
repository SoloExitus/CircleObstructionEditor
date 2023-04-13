using GraphStruct;

class VertexPriorityQueue
{
    private List<GraphVertex> m_Vertexes = new ();
    public VertexPriorityQueue(){}

    public int Find(in GraphVertex v)
    {
        return m_Vertexes.IndexOf(v);
    }

    public bool Empty()
    {
        return m_Vertexes.Count == 0;
    }

    public int Size()
    {
        return m_Vertexes.Count;
    }

    public GraphVertex Pop()
    {
        if (Empty())
            throw new IndexOutOfRangeException();

        GraphVertex t = m_Vertexes.Min()!;
        m_Vertexes.Remove(t);
        return t;
    }

    public void Push(GraphVertex v)
    {
        m_Vertexes.Add(v);
    }

    public bool Have(in GraphVertex v) 
    {
        return m_Vertexes.IndexOf(v) != -1;
    }

}