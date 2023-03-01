
// Базовые математические функции
using BaseStruct;

public static class BaseMath
{
    // Квадрат расстояния между точками
    public static float Square_distance(in PointF a, in PointF b)
    {
        float dX = a.X - b.X;
        float dY = a.Y - b.Y;
        return (dX * dX + dY * dY);
    }

    public static float Distance(in PointF a, in PointF b)
    {
        float sd = Square_distance(in a, in b);
        return MathF.Sqrt(sd);
    }

    //public static T Clamp<T>(in T val, in T min, in T max) where T : IComparable<T>
    //{
    //    if (val.CompareTo(min) < 0) return min;
    //    else if (val.CompareTo(max) > 0) return max;
    //    else return val;
    //}

    // Перевод радианы в градусы
    public static float To_degre(in float rad)
    {
        return rad * 180 / MathF.PI;
    }

    // Поворот точки p вокруг центра с на угол angle в радианах
    public static PointF RotatePoint(in PointF p, in PointF c, in float angle)
    {
        float nX = c.X + (p.X - c.X) * MathF.Cos(angle) - (p.Y - c.Y) * MathF.Sin(angle);
        float nY = c.Y + (p.X - c.X) * MathF.Sin(angle) + (p.Y - c.Y) * MathF.Cos(angle);

        return new PointF(nX, nY);
    }

    // Угол между двумя векторами в радианах от 0 до Pi
    public static float AngleBetweenVectors(in PointF f, in PointF s)
    {
        return MathF.Atan2(f.X * s.Y - f.Y * s.X, f.X * s.X + f.Y * s.Y);
    }

    public static float ClockwiseAngleBetweenVectors(in PointF f, in PointF s)
    {
        float angle = AngleBetweenVectors(in f, in s);
        if (angle < 0)
            angle += 2 * MathF.PI;
        return angle;
    }

    public static float CounterClockwiseAngleBetweenVectors(in PointF f, in PointF s)
    {
        float angle = AngleBetweenVectors(in f, in s);
        if (angle > 0)
            angle -= 2 * MathF.PI;
        return angle;
    }

    public static float AngleBetweenPointsOnCircle(in Circle c, in PointF first, in PointF second, int direction)
    {
        if (Math.Abs(direction) != 1)
            return 0;

        PointF center = c.m_center;
        PointF firstVec = new(
            first.X - center.X,
            first.Y - center.Y
            );

        PointF secondVec = new(
            second.X - center.X,
            second.Y - center.Y
            );

        if (direction == 1)
            return ClockwiseAngleBetweenVectors(in firstVec, in secondVec);

        return CounterClockwiseAngleBetweenVectors(in firstVec, in secondVec);
    }

    public static float CircleArcLenght(in float radius, in float angle)
    {
        float angleAbs = MathF.Abs(angle);
        return radius * angleAbs;
    }

    // длина вектора
    public static float VectorLenght(in PointF vect)
    {
        return MathF.Sqrt(vect.X * vect.X + vect.Y * vect.Y);
    }

    public static bool PointInCircle(in PointF point, in PointF center, in float radius)
    {
        float dist = Distance(point, center);
        return dist < radius;
    }

    public static int CircleInteraction(in PointF fCenter, in float fRadius, in PointF sCenter, in float sRadius)
    {
        float centerDist = Distance(in fCenter, in sCenter);
        float sumRadius = fRadius + sRadius;

        // Не пересекаются
        if (centerDist > sumRadius)
            return -1;

        // sc полностью содержит fc
        if (centerDist + fRadius <= sRadius)
            return 0;

        return 1; // пересекаются
    }

    public static List<Edge> ExternalBitangents(in Circle a, in Circle b)
    {
        PointF centerA = a.m_center;
        PointF centerB = b.m_center;

        float rA = a.m_radius;
        float rB = b.m_radius;

        return ExternalBitangents(centerA, rA, centerB, rB);
    }

    public static List<Edge> ExternalBitangents(in PointF centerA, in float rA, in PointF centerB, in float rB)
    {
        float Q = MathF.Acos(MathF.Abs(rA - rB) / Distance(in centerA, in centerB));

        float vectBLX = centerB.X - centerA.X;
        float vectBLY = centerB.Y - centerA.Y;

        bool changeOrder = false;

        if (rB > rA)
        {
            vectBLX *= -1;
            vectBLY *= -1;

            changeOrder = true;
        }

        // вектор из центра большего препятствия в центр меньшего, если равны то не важно
        PointF VectBL = new(vectBLX, vectBLY);

        float vectBLLen = VectorLenght(in VectBL);

        VectBL.X /= vectBLLen;
        VectBL.Y /= vectBLLen;

        PointF G = new(centerA.X + rA * VectBL.X, centerA.Y + rA * VectBL.Y);

        PointF H = new(centerB.X + rB * VectBL.X, centerB.Y + rB * VectBL.Y);

        PointF C = RotatePoint(in G, in centerA, -Q);
        PointF D = RotatePoint(in G, in centerA, Q);

        PointF E = RotatePoint(in H, in centerB, Q);
        PointF F = RotatePoint(in H, in centerB, -Q);

        Edge firstEdge = new (C, F);
        Edge secondEdge = new (D, E);

        if (changeOrder)
        {
            return new()
            {
                secondEdge,
                firstEdge
            };
        }

        return new()
        {
            firstEdge,
            secondEdge
        };
    }

    public static List<Edge> InternalBitangents(Circle a, Circle b)
    {
        PointF centerA = a.m_center;
        PointF centerB = b.m_center;

        float rA = a.m_radius;
        float rB = b.m_radius;
        return InternalBitangents(centerA, rA, centerB, rB);
    }

    public static List<Edge> InternalBitangents(PointF centerA, float rA, PointF centerB, float rB)
    {
        float Q = (float)Math.Acos((double)(rA + rB) / Distance(in centerA, in centerB));

        float vectABX = centerB.X - centerA.X;
        float vectABY = centerB.Y - centerA.Y;

        PointF VectAB = new(vectABX, vectABY);

        float vectABLen = VectorLenght(in VectAB);

        VectAB.X /= vectABLen;
        VectAB.Y /= vectABLen;

        PointF G = new(centerA.X + rA * VectAB.X, centerA.Y + rA * VectAB.Y);

        PointF H = new(centerB.X + rB * (-VectAB.X), centerB.Y + rB * (-VectAB.Y));

        PointF C = RotatePoint(G, centerA, -Q);
        PointF D = RotatePoint(G, centerA, Q);

        PointF E = RotatePoint(H, centerB, Q);
        PointF F = RotatePoint(H, centerB, -Q);
        List<Edge> list = new()
        {
                new Edge(C, F),
                new Edge(D, E)
            };

        return list;
    }

    public static bool CompareAngles( in float fAngle, in float sAngle)
    {
        // Если углы имеют одинаковый знак
        if (fAngle * sAngle > 0)
        {
            return MathF.Abs(fAngle) > MathF.Abs(sAngle);
        }

        return false;
    }

    public static bool Is_Block(in Circle obstruct, in PointF A, in PointF B)
    {
        PointF C = obstruct.m_center;

        float u = ((C.X - A.X) * (B.X - A.X) + (C.Y - A.Y) * (B.Y - A.Y)) / Square_distance(in B, in A);

        float clamp_u = Math.Clamp(u , 0, 1);

        float Ex = A.X + clamp_u * (B.X - A.X);
        float Ey = A.Y + clamp_u * (B.Y - A.Y);

        PointF E = new(Ex, Ey);

        float Sd = Square_distance(in E, in C);

        return Sd < (obstruct.m_radius * obstruct.m_radius);
    }
}
