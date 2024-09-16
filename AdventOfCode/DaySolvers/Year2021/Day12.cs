namespace AdventOfCode.Year2021
{
    internal class Day12 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var edges = lines.Select(ConvertLineToEdge).ToList();
            var startingPaths = edges
                .Where(e => EdgeContainsPoint(e, "start"))
                .Select(e => new List<string> { "start", GetOtherPoint(e, "start") })
                .ToList();

            var openPaths = new Stack<List<string>>(startingPaths);

            var completedPaths = new List<List<string>>();
            while (openPaths.Any())
            {
                var path = openPaths.Pop();
                var lastPoint = path.Last();
                var connectedPoints = edges
                    .Where(e => EdgeContainsPoint(e, lastPoint))
                    .Select(e => GetOtherPoint(e, lastPoint))
                    .Where(p => p.ToUpper() == p || !path.Contains(p))
                    .ToList();

                foreach (var connectedPoint in connectedPoints)
                {
                    var newPath = path.Concat(new List<string> { connectedPoint }).ToList();
                    if (connectedPoint == "end")
                    {
                        completedPaths.Add(newPath);
                    }
                    else
                    {
                        openPaths.Push(newPath);
                    }
                }
            }

            return completedPaths.Count;
        }

        public object HardSolution(IList<string> lines)
        {
            var edges = lines.Select(ConvertLineToEdge).ToList();
            var startingPaths = edges
                .Where(e => EdgeContainsPoint(e, "start"))
                .Select(e => new List<string> { "start", GetOtherPoint(e, "start") })
                .ToList();

            var openPaths = new Stack<List<string>>(startingPaths);

            var completedPaths = new List<List<string>>();
            while (openPaths.Any())
            {
                var path = openPaths.Pop();
                var lastPoint = path.Last();
                var lowerCasePointsInPath = path.Where(p => p.ToUpper() != p).ToList();
                var hasDoubledBackThroughSmallCave = lowerCasePointsInPath.Count != lowerCasePointsInPath.Distinct().Count();

                var connectedPoints = edges
                    .Where(e => EdgeContainsPoint(e, lastPoint))
                    .Select(e => GetOtherPoint(e, lastPoint))
                    .Where(p => p != "start" && (p.ToUpper() == p || !hasDoubledBackThroughSmallCave || !path.Contains(p)))
                    .ToList();

                foreach (var connectedPoint in connectedPoints)
                {
                    var newPath = path.Concat(new List<string> { connectedPoint }).ToList();
                    if (connectedPoint == "end")
                    {
                        completedPaths.Add(newPath);
                    }
                    else
                    {
                        openPaths.Push(newPath);
                    }
                }
            }

            return completedPaths.Count;
        }

        public record Edge (string Start, string End);

        private Edge ConvertLineToEdge(string line)
        {
            var lineParts = line.Split("-");
            return new Edge(lineParts[0], lineParts[1]);
        }

        private static string GetOtherPoint(Edge edge, string point)
        {
            return edge.Start == point ? edge.End : edge.Start;
        }

        private static bool EdgeContainsPoint(Edge edge, string point)
        {
            return edge.Start == point || edge.End == point;
        }
    }
}
