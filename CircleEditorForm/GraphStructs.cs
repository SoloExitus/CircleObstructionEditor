﻿using BaseStruct;
using PointFPair = System.Tuple<System.Drawing.PointF, System.Drawing.PointF>;

namespace GraphStruct
{
    class CircleObstacle : Circle
    {
        public List<int> m_VertexIndexes = new();
        public List<int> m_IntersectCircleIndexes = new ();
        public List<PointFPair> m_Intersections = new ();
        public bool m_isEdgesGenerated = false;
        public bool m_isBlocked = false;

        public CircleObstacle(PointF center):
            base(center)
        {
            m_center = center;
        }

        public CircleObstacle(PointF center, float radius):
            base(center, radius)
        {
            m_center = center;
            m_radius = radius;
        }

        public CircleObstacle(Circle c) :
            base(c)
        {
        }
    }

    class GraphVertex: IComparable<GraphVertex>
    {
        public PointF m_position;
        public int m_obstacleIndex;
        public List<int> m_incidentEdgeIndexes = new List<int>();
        public int m_parentVertexIndex = -1;
        public float m_G = -1;
        public float m_H = -1;
        public float m_F = -1;

        public int CompareTo(GraphVertex? other)
        {
            if (other == null) return 1;
            return m_F.CompareTo(other.m_F);
        }

        public GraphVertex(PointF point, int obstacleIndex, PointF endPoint)
        {
            m_position = point;
            m_obstacleIndex = obstacleIndex;
            m_H = BaseMath.Distance(in m_position, in endPoint);
        }

        public GraphVertex(float x, float y, int obstacleIndex, PointF endPoint) :
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
}
