using Helpers.Extensions;

namespace AdventOfCode.Year2021
{
    internal class Day13 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            (var points, var folds) = ParseLines(lines);

            var maxX = points.Select(p => p.Item1).Max();
            var maxY = points.Select(p => p.Item2).Max();

            (points, _, _) = HandleFold(points, folds[0], maxX, maxY);

            return points.Count;
        }

        public object HardSolution(IList<string> lines)
        {
            (var points, var folds) = ParseLines(lines);

            var maxX = points.Select(p => p.Item1).Max();
            var maxY = points.Select(p => p.Item2).Max();

            foreach (var fold in folds)
            {
                (points, maxX, maxY) = HandleFold(points, fold, maxX, maxY);
            }

            PrintPoints(points, maxX, maxY);
            return points.Count;
        }

        private static (List<(int, int)>, int, int) HandleFold(List<(int, int)> points, Fold fold, int maxX, int maxY)
        {
            var newPoints = new List<(int, int)>();
            var pointsToMove = new List<(int, int)>();
            if (fold.Axis == Axis.X)
            {
                pointsToMove = points.Where(p => p.Item1 > fold.FoldIndex).ToList();
                foreach (var (x, y) in pointsToMove)
                {
                    newPoints.Add((x - 2 * (x - fold.FoldIndex), y));
                }
                maxX = fold.FoldIndex - 1;
            }
            else
            {
                pointsToMove = points.Where(p => p.Item2 > fold.FoldIndex).ToList();
                foreach (var (x, y) in pointsToMove)
                {
                    newPoints.Add((x, y - 2 * (y - fold.FoldIndex)));
                }
                maxY = fold.FoldIndex - 1;
            }
            return (points.Except(pointsToMove).Concat(newPoints).Distinct().ToList(), maxX, maxY);
        }

        private static (List<(int, int)>, List<Fold>) ParseLines(IList<string> lines)
        {
            var splitLine = lines.IndexOf("");
            var points = lines
                .Take(splitLine)
                .Select(ConvertLineToPoint)
                .ToList();

            var folds = lines
                .Skip(splitLine + 1)
                .Select(ConvertLineToFold)
                .ToList();

            return (points, folds);
        }

        private static (int, int) ConvertLineToPoint(string line)
        {
            var lineParts = line.Split(",");
            var x = lineParts[0].ToNullableInt();
            var y = lineParts[1].ToNullableInt();
            if (!x.HasValue || !y.HasValue)
            {
                throw new Exception($"Invalid Point Line: {line}");
            }
            return (x.Value, y.Value);
        }

        private static Fold ConvertLineToFold(string line)
        {
            var lineParts = line.Replace("fold along ", "").Split("=");
            var axis = lineParts[0] == "x" ? Axis.X : Axis.Y;
            var foldIndex = lineParts[1].ToNullableInt();
            if (!foldIndex.HasValue)
            {
                throw new Exception($"Invalid Fold Line: {line}");
            }

            return new Fold(axis, foldIndex.Value);
        }

        private static void PrintPoints(List<(int, int)> points, int maxX, int maxY)
        {
            for (var y = 0; y < maxY + 1; y++)
            {
                for (var x = 0; x < maxX + 1; x++)
                {
                    if (points.Contains((x, y)))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        private record Fold(Axis Axis, int FoldIndex);

        private enum Axis
        {
            X,
            Y,
        }
    }
}
