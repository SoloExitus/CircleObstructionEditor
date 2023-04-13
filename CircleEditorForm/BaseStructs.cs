namespace BaseStruct
{
    public class Circle
    {
        public PointF m_center;
        public float m_radius = 0;

        public Circle(Circle rhs):
            this(rhs.m_center, rhs.m_radius)
        {}

        public Circle(PointF center):
            this(center, 0)
        {}

        public Circle(PointF center, float radius)
        {
            m_center = center;
            m_radius = radius;
        }

        public void SetRadius(PointF e)
        {
            m_radius = BaseMath.Distance(in m_center, in e);
        }

        public void SetCenter(PointF center)
        {
            m_center = center;
        }

        public int Interaction(in Circle rhs)
        {
            return BaseMath.CircleInteraction(
                m_center, m_radius,
                rhs.m_center, rhs.m_radius
                );
        }
    }

    public class Edge
    {
        public PointF m_first;
        public PointF m_second;

        public Edge(float fx, float fy, float sx, float sy)
        {
            this.m_first = new PointF(fx, fy);
            this.m_second = new PointF(sx, sy);
        }

        public Edge(PointF fpf, PointF spf)
        {
            this.m_first = fpf;
            this.m_second = spf;
        }
    }

}