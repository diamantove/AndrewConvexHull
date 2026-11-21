using System.Drawing;

namespace AndrewHull;

public static class ConvexHull
{
    public static List<Point> Build(List<Point> points)
    {
        List<Point> sortedPoints = points
            .Distinct()
            .OrderBy(point => point.X)
            .ThenBy(point => point.Y)
            .ToList();

        if (sortedPoints.Count <= 1)
        {
            return sortedPoints;
        }

        List<Point> lower = new List<Point>();

        foreach (Point point in sortedPoints)
        {
            while (lower.Count >= 2 &&
                   Cross(lower[lower.Count - 2], lower[lower.Count - 1], point) <= 0)
            {
                lower.RemoveAt(lower.Count - 1);
            }

            lower.Add(point);
        }

        List<Point> upper = new List<Point>();

        for (int i = sortedPoints.Count - 1; i >= 0; i--)
        {
            Point point = sortedPoints[i];

            while (upper.Count >= 2 &&
                   Cross(upper[upper.Count - 2], upper[upper.Count - 1], point) <= 0)
            {
                upper.RemoveAt(upper.Count - 1);
            }

            upper.Add(point);
        }

        lower.RemoveAt(lower.Count - 1);
        upper.RemoveAt(upper.Count - 1);
        lower.AddRange(upper);

        return lower;
    }

    private static long Cross(Point a, Point b, Point c)
    {
        return ((long)b.X - a.X) * ((long)c.Y - a.Y)
             - ((long)b.Y - a.Y) * ((long)c.X - a.X);
    }
}
