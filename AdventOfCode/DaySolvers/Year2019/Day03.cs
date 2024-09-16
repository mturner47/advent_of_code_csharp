using Helpers.Helpers;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2019
{
    internal partial class Day03 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var wire1Path = lines[0].Split(",").Select(ParseMovement).ToList();
            var wire2Path = lines[1].Split(",").Select(ParseMovement).ToList();

            var startingPoint = (x:0L, y:0L);
            var wire1Edges = GetPoints(wire1Path).ToList();
            var wire2Edges = GetPoints(wire2Path).ToList();

            var minDistance = long.MaxValue;
            foreach (var w1Edge in wire1Edges)
            {
                foreach (var w2Edge in wire2Edges)
                {
                    var intersection = FindIntersection(w1Edge, w2Edge);
                    if (intersection.HasValue)
                    {
                        var distance = MathHelpers.ManhattanDistance(startingPoint, intersection.Value);
                        if (distance > 0 && distance < minDistance)
                        {
                            minDistance = distance;
                        }
                    }
                }
            }

            return minDistance;
        }

        public object HardSolution(IList<string> lines)
        {
            var wire1Path = lines[0].Split(",").Select(ParseMovement).ToList();
            var wire2Path = lines[1].Split(",").Select(ParseMovement).ToList();

            var wire1Edges = GetPoints(wire1Path).ToList();
            var wire2Edges = GetPoints(wire2Path).ToList();

            var minSteps = long.MaxValue;
            var e1Length = 0L;
            foreach (var w1Edge in wire1Edges)
            {
                var e2Length = 0L;
                foreach (var w2Edge in wire2Edges)
                {
                    var intersection = FindIntersection(w1Edge, w2Edge);
                    if (intersection.HasValue)
                    {
                        var currentEdge1Steps = MathHelpers.ManhattanDistance((w1Edge.x1, w1Edge.y1), intersection.Value);
                        var currentEdge2Steps = MathHelpers.ManhattanDistance((w2Edge.x1, w2Edge.y1), intersection.Value);
                        var totalSteps = e1Length + e2Length + currentEdge1Steps + currentEdge2Steps;
                        if (totalSteps > 0 && totalSteps < minSteps)
                        {
                            minSteps = totalSteps;
                        }
                    }
                    e2Length += w2Edge.distance;
                }
                e1Length += w1Edge.distance;
            }

            return minSteps;
        }

        private static (Direction direction, int distance) ParseMovement(string input)
        {
            var regex = MyRegex();
            var matches = regex.Matches(input)[0].Groups;
            var distance = int.Parse(matches[2].Value);
            var direction = matches[1].Value switch
            {
                "U" => Direction.North,
                "D" => Direction.South,
                "R" => Direction.East,
                "L" => Direction.West,
                _ => throw new NotImplementedException(),
            };
            return (direction, distance);
        }

        private static IEnumerable<(long x1, long y1, long x2, long y2, long distance)> GetPoints(List<(Direction direction, int distance)> movements)
        {
            var point = (x: 0L, y: 0L);

            foreach (var (direction, distance) in movements)
            {
                var nextPoint = direction.GetMovement(point, distance);
                yield return (point.x, point.y, nextPoint.x, nextPoint.y, distance);
                point = nextPoint;
            }
        }

        private (long x, long y)? FindIntersection((long x1, long y1, long x2, long y2, long distance) edge1,
            (long x1, long y1, long x2, long y2, long distance) edge2)
        {
            if (edge1.x1 == edge1.x2 && edge2.x1 == edge2.x2) return null;
            if (edge1.y1 == edge1.y2 && edge2.y1 == edge2.y2) return null;

            var (hx1, hy, hx2, _, _) = edge1.x1 == edge1.x2 ? edge2 : edge1;
            var (vx1, vy1, _, vy2, _) = edge1.x1 == edge1.x2 ? edge1 : edge2;

            var hEdgeMinX = Math.Min(hx1, hx2);
            var hEdgeMaxX = Math.Max(hx1, hx2);
            var vEdgeMinY = Math.Min(vy1, vy2);
            var vEdgeMaxY = Math.Max(vy1, vy2);

            if (vx1 >= hEdgeMinX && vx1 <= hEdgeMaxX && hy >= vEdgeMinY && hy <= vEdgeMaxY)
            {
                return (vx1, hy);
            }
            return null;
        }

        [GeneratedRegex("([URDL])(\\d+)")]
        private static partial Regex MyRegex();
    }
}
