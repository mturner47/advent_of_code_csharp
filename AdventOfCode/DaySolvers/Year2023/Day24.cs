using Microsoft.Z3;

namespace AdventOfCode.Year2023
{
    internal class Day24 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var paths = lines.Select(Parse).ToList();
            var min = 200_000_000_000_000L;
            var max = 400_000_000_000_000L;

            var intersectionCount = 0;
            for (var i = 0; i < paths.Count - 1; i++)
            {
                var line1 = paths[i];
                for (var j = i + 1; j < paths.Count; j++)
                {
                    var line2 = paths[j];

                    var intersectionPoint = GetTwoDimensionalIntersection(line1, line2);
                    if (!intersectionPoint.HasValue) continue; // No Intersection

                    var (x, y, _) = intersectionPoint.Value;

                    if (x < min || x > max || y < min || y > max) continue; // Outside range

                    // In past
                    if (line1.Velocity.x < 0 && x > line1.Point.x) continue;
                    if (line1.Velocity.x > 0 && x < line1.Point.x) continue;
                    if (line1.Velocity.y < 0 && y > line1.Point.y) continue;
                    if (line1.Velocity.y > 0 && y < line1.Point.y) continue;
                    if (line2.Velocity.x < 0 && x > line2.Point.x) continue;
                    if (line2.Velocity.x > 0 && x < line2.Point.x) continue;
                    if (line2.Velocity.y < 0 && y > line2.Point.y) continue;
                    if (line2.Velocity.y > 0 && y < line2.Point.y) continue;

                    intersectionCount++;
                }
            }

            return intersectionCount;
        }

        public object HardSolution(IList<string> lines)
        {
            var paths = lines.Take(3).Select(Parse).ToList();

            var z3Context = new Context();
            var solver = z3Context.MkSolver();

            var rpx = z3Context.MkIntConst("rpx");
            var rpy = z3Context.MkIntConst("rpy");
            var rpz = z3Context.MkIntConst("rpz");

            var rvx = z3Context.MkIntConst("rvx");
            var rvy = z3Context.MkIntConst("rvy");
            var rvz = z3Context.MkIntConst("rvz");

            for (var i = 0; i < 3; i++)
            {
                var t = z3Context.MkIntConst($"t{i}");
                var line = paths[i];

                var hpx = z3Context.MkInt(line.Point.x);
                var hpy = z3Context.MkInt(line.Point.y);
                var hpz = z3Context.MkInt(line.Point.z);

                var hvx = z3Context.MkInt(line.Velocity.x);
                var hvy = z3Context.MkInt(line.Velocity.y);
                var hvz = z3Context.MkInt(line.Velocity.z);

                var leftSideX = z3Context.MkAdd(rpx, z3Context.MkMul(t, rvx));
                var leftSideY = z3Context.MkAdd(rpy, z3Context.MkMul(t, rvy));
                var leftSideZ = z3Context.MkAdd(rpz, z3Context.MkMul(t, rvz));

                var rightSideX = z3Context.MkAdd(hpx, z3Context.MkMul(t, hvx));
                var rightSideY = z3Context.MkAdd(hpy, z3Context.MkMul(t, hvy));
                var rightSideZ = z3Context.MkAdd(hpz, z3Context.MkMul(t, hvz));
                solver.Add(t >= 0);
                solver.Add(z3Context.MkEq(leftSideX, rightSideX));
                solver.Add(z3Context.MkEq(leftSideY, rightSideY));
                solver.Add(z3Context.MkEq(leftSideZ, rightSideZ));
            }

            solver.Check();
            var model = solver.Model;

            return long.Parse(model.Eval(rpx).ToString())
                + long.Parse(model.Eval(rpy).ToString())
                + long.Parse(model.Eval(rpz).ToString());
        }

        private class Line
        {
            public (long x, long y, long z) Point { get; set; }
            public (long x, long y, long z) Velocity { get; set; }
            public (double a, double b, double c) TwoDimensionalLineEquation { get; set; }
        }

        private static Line Parse(string line)
        {
            var parts = line.Split(" @ ");
            var pointParts = parts[0].Split(", ").Select(long.Parse).ToList();
            var point = (x:pointParts[0], y:pointParts[1], z:pointParts[2]);
            var velocityParts = parts[1].Split(", ").Select(long.Parse).ToList();
            var velocity = (x:velocityParts[0], y:velocityParts[1], z:velocityParts[2]);
            var slope = (double)velocity.y / velocity.x;
            var equation = (a:-slope, b:1, c:slope * point.x - point.y);

            return new Line
            {
                Point = point,
                Velocity = velocity,
                TwoDimensionalLineEquation = equation,
            };
        }

        private static (double x, double y, double z)? GetTwoDimensionalIntersection(Line l1, Line l2)
        {
            if (l1.Velocity.x == l2.Velocity.x && l1.Velocity.y == l2.Velocity.y) return null;

            var (a1, b1, c1) = l1.TwoDimensionalLineEquation;
            var (a2, b2, c2) = l2.TwoDimensionalLineEquation;
            return ((b1 * c2 - b2 * c1) / (a1 * b2 - a2 * b1), (c1 * a2 - c2 * a1) / (a1 * b2 - a2 * b1), 0);
        }
    }
}
