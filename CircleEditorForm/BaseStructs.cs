namespace BaseStruct
{
    class Circle
    {
        public PointF m_center;
        public float m_radius = 0;

        public Circle()
        {
            m_center = new PointF();
        }

        public Circle(Circle rhs)
        {
            m_center = rhs.m_center;
            m_radius = rhs.m_radius;
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
            m_radius = BaseMath.Distance(in m_center, in e);
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

        public Edge(PointF fpf, PointF spf)
        {
            this.m_first = fpf;
            this.m_second = spf;
        }
    }

}