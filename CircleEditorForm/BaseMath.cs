
// Базовые математические функции
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
    public static PointF RotatePoint(ref PointF p, ref PointF c, float angle)
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
    public static float VectorLenght(in PointF vect)
    {
        return (float)Math.Sqrt(vect.X * vect.X + vect.Y * vect.Y);
    }

    public static bool PointInCircle(in PointF point, in PointF center, float radius)
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
}
