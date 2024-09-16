namespace AdventOfCode.Year2023
{
    internal class Day22 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var bricks = lines.Select(Brick.ParseLine).OrderBy(b => Math.Min(b.StartZ, b.EndZ)).ToList();

            LetAllBricksFall(bricks);

            return FindRemovableBricks(bricks).Count;
        }

        public object HardSolution(IList<string> lines)
        {
            var bricks = lines.Select(Brick.ParseLine).OrderBy(b => Math.Min(b.StartZ, b.EndZ)).ToList();
            for (var i = 0; i < bricks.Count; i++)
            {
                bricks[i].Index = i;
            }

            LetAllBricksFall(bricks);

            var removableBricks = FindRemovableBricks(bricks);

            var sumBricksThatWouldFall = 0;
            foreach (var brick in bricks.Except(removableBricks))
            {
                var startingBricks = bricks.Select(Brick.CloneForPart2).Where(b => b.Index != brick.Index).ToList();
                LetAllBricksFall(startingBricks);
                sumBricksThatWouldFall += startingBricks.Count(b => b.HasFallen);
            }

            return sumBricksThatWouldFall;
        }

        private static void LetAllBricksFall(List<Brick> bricks)
        {
            while (!bricks.All(b => b.HasSettled))
            {
                for (var i = 0; i < bricks.Count; i++)
                {
                    var brick = bricks[i];
                    if (brick.StartZ == 1)
                    {
                        brick.HasSettled = true;
                        continue;
                    }

                    var bricksLandedOn = bricks.Where(b => DoesBrickLandOnBrick(brick, b)).ToList();
                    if (!bricksLandedOn.Any())
                    {
                        brick.Fall();
                        continue;
                    }

                    if (bricksLandedOn.Any(b => b.HasSettled))
                    {
                        brick.HasSettled = true;
                        continue;
                    }
                }
            }
        }

        private static List<Brick> FindRemovableBricks(List<Brick> bricks)
        {
            return bricks.Where(b =>
            {
                var dependentBricks = bricks.Where(b2 => DoesBrickLandOnBrick(b2, b)).ToList();
                if (!dependentBricks.Any()) return true;

                return dependentBricks.All(b2 => bricks.Count(b3 => DoesBrickLandOnBrick(b2, b3)) > 1);
            }).ToList();
        }

        private static bool DoesBrickLandOnBrick(Brick topBrick, Brick bottomBrick)
        {
            var topLowestPoint = topBrick.StartZ;
            var bottomHighestPoint = bottomBrick.EndZ;
            if (topLowestPoint != bottomHighestPoint + 1) return false;
            return topBrick.Points.Intersect(bottomBrick.Points).Any();
        }

        private class Brick
        {
            public int Index { get; set; }
            public int StartZ { get; set; }
            public int EndZ { get; set; }
            public bool HasFallen { get; set; }

            public List<(int x, int y)> Points { get; set; } = new();

            public bool HasSettled { get; set; }

            public void Fall()
            {
                StartZ--;
                EndZ--;
                HasFallen = true;
            }

            public static Brick CloneForPart2(Brick brick)
            {
                return new Brick
                {
                    Index = brick.Index,
                    StartZ = brick.StartZ,
                    EndZ = brick.EndZ,
                    Points = brick.Points,
                    HasSettled = false,
                    HasFallen = false,
                };
            }

            public static Brick ParseLine(string line)
            {
                var parts = line.Split("~");
                var startParts = parts[0].Split(",").Select(int.Parse).ToList();
                var originalStart = (x: startParts[0], y: startParts[1], z: startParts[2]);
                var endParts = parts[1].Split(",").Select(int.Parse).ToList();
                var originalEnd = (x: endParts[0], y: endParts[1], z: endParts[2]);

                if (originalStart.z > originalEnd.z)
                {
                    (originalStart, originalEnd) = (originalEnd, originalStart);
                }

                var points = new List<(int x, int y)>();
                for (var x = Math.Min(originalStart.x, originalEnd.x); x <= Math.Max(originalStart.x, originalEnd.x); x++)
                {
                    for (var y = Math.Min(originalStart.y, originalEnd.y); y <= Math.Max(originalStart.y, originalEnd.y); y++)
                    {
                        points.Add((x, y));
                    }
                }

                return new Brick
                {
                    StartZ = originalStart.z,
                    EndZ = originalEnd.z,
                    HasSettled = false,
                    Points = points,
                    HasFallen = false,
                };
            }
        }
    }
}
