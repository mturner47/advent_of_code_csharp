using Helpers.Extensions;
using Helpers.Helpers;

namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day09 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var points = lines.Select(l => l.To2dPoint()).ToArray();

            var biggestArea = 0d;
            for (var i = 0; i < points.Length - 1; i++)
            {
                var (iX, iY) = points[i];
                for (var j = i + 1; j < points.Length; j++)
                {
                    var (jX, jY) = points[j];
                    var length = Math.Abs(jX - iX) + 1;
                    var height = Math.Abs(jY - iY) + 1;
                    var area = length * height;
                    if (area > biggestArea) biggestArea = area;
                }
            }

            var expectedResult = 4755064176d;
            var result = biggestArea;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var points = lines.Select(l => l.To2dPoint()).ToArray();
            var minY = points.Min(p => p.y);
            var maxY = points.Max(p => p.y);
            var minX = points.Min(p => p.x);
            var maxX = points.Max(p => p.x);

            var horizontalEdges = new List<((double min, double max) x, double y)>();
            var verticalEdges = new List<((double min, double max) y, double x)>();

            for (var i = 0; i < points.Length; i++)
            {
                var (x1, y1) = points[i];
                var (x2, y2) = points[(i + 1) % points.Length];
                if (x1 == x2) verticalEdges.Add(((Math.Min(y1, y2), Math.Max(y1, y2)), x1));
                else horizontalEdges.Add(((Math.Min(x1, x2), Math.Max(x1, x2)), y1));
            }

            var maxArea = (maxX - minX) * (maxY - minY);
            var pointPairs = new PriorityQueue<((double x, double y) p1, (double x, double y) p2, double area), double>();
            foreach (var p1 in points)
            {
                foreach (var p2 in points)
                {
                    var area = GetArea(p1, p2);
                    pointPairs.Enqueue((p1, p2, area), maxArea - area);
                }
            }


            var biggestArea = 0d;
            while (pointPairs.Count > 0)
            {
                var ((x1, y1), (x2, y2), area) = pointPairs.Dequeue();

                var innerMinX = Math.Min(x1, x2);
                var innerMinY = Math.Min(y1, y2);
                var innerMaxX = Math.Max(x1, x2);
                var innerMaxY = Math.Max(y1, y2);

                var minYCrossings = verticalEdges.Where(v => v.y.max > innerMinY && v.y.min <= innerMinY && v.x > innerMinX && v.x < innerMaxX).ToList();
                if (minYCrossings.Count > 0) continue;

                var maxYCrossings = verticalEdges.Where(v => v.y.min < innerMaxY && v.y.max >= innerMaxY && v.x > innerMinX && v.x < innerMaxX).ToList();
                if (maxYCrossings.Count > 0) continue;

                var minXCrossings = horizontalEdges.Where(v => v.x.max > innerMinX && v.x.min <= innerMinX && v.y > innerMinY && v.y < innerMaxY).ToList();
                if (minXCrossings.Count > 0) continue;

                var maxXCrossings = horizontalEdges.Where(v => v.x.min < innerMaxX && v.x.max >= innerMaxX && v.y > innerMinY && v.y < innerMaxY).ToList();
                if (maxXCrossings.Count > 0) continue;

                biggestArea = area;
                break;
            }

            var expectedResult = -1;
            var result = biggestArea;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static double GetArea((double x, double y) point1, (double x, double y) point2)
        {
            var (x1, y1) = point1;
            var (x2, y2) = point2;
            var length = Math.Abs(x2 - x1) + 1;
            var height = Math.Abs(y2 - y1) + 1;
            var area = length * height;
            return area;
        }
    }
}
