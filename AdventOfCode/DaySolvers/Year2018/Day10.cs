using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018
{
    internal partial class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var points = lines.Select(Parse).ToList();
            foreach (var point in points) Move(point, 10000);

            var smallestY = int.MaxValue;
            for (var i = 0; i < 1000; i++)
            {
                foreach (var point in points) Move(point, 1);
                var positions = points.Select(p => p.Position).ToList();
                var minY = positions.Min(p => p.y);
                var minX = positions.Min(p => p.x);
                var maxY = positions.Max(p => p.y);
                var maxX = positions.Max(p => p.x);
                var ySize = maxY - minY;
                if (ySize < smallestY) smallestY = ySize;
                Draw(points);
            }

            var expectedResult = 9;
            var result = smallestY;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var points = lines.Select(Parse).ToList();
            foreach (var point in points) Move(point, 10000);

            var smallestY = int.MaxValue;
            var t = 0;
            for (var i = 0; i < 1000; i++)
            {
                foreach (var point in points) Move(point, 1);
                var positions = points.Select(p => p.Position).ToList();
                var minY = positions.Min(p => p.y);
                var minX = positions.Min(p => p.x);
                var maxY = positions.Max(p => p.y);
                var maxX = positions.Max(p => p.x);
                var ySize = maxY - minY;
                if (ySize < smallestY) smallestY = ySize;
                if (Draw(points))
                {
                    t = 10000 + i + 1;
                    break;
                }
            }

            var expectedResult = 10459;
            var result = t;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static MovingPoint Parse(string line)
        {
            var regex = PointRegex();
            var groups = regex.Matches(line)[0].Groups;
            var position = (x: int.Parse(groups[1].Value), y: int.Parse(groups[2].Value));
            var velocity = (x: int.Parse(groups[3].Value), y: int.Parse(groups[4].Value));
            var point = new MovingPoint { Position = position, Velocity = velocity };
            return point;
        }

        private static void Move(MovingPoint point, int t)
        {
            point.Position.x += t * point.Velocity.x;
            point.Position.y += t * point.Velocity.y;
        }

        private bool Draw(List<MovingPoint> points)
        {
            var positions = points.Select(p => p.Position).ToList();
            var minY = positions.Min(p => p.y);
            var minX = positions.Min(p => p.x);
            var maxY = positions.Max(p => p.y);
            var maxX = positions.Max(p => p.x);

            if (maxY - minY != 9) return false;

            for (var y = minY; y < maxY + 1; y++)
            {
                for (var x = minX; x < maxX + 1; x++)
                {
                    //Console.Write(positions.Contains((x, y)) ? "#" : ".");
                }
                //Console.WriteLine();
            }
            return true;
        }

        [GeneratedRegex(@"position=<(-? ?\d+), (-? ?\d+)> velocity=<(-? ?\d+), (-? ?\d+)>")]
        private static partial Regex PointRegex();

        private class MovingPoint
        {
            public (int x, int y) Position;
            public (int x, int y) Velocity;
        }
    }
}
