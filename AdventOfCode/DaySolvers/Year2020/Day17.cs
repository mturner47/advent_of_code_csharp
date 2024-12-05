namespace AdventOfCode.Year2020
{
    internal class Day17 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var currentCells = new HashSet<(long x, long y, long z)>();
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#') currentCells.Add((x, y, 0));
                }
            }

            for (var t = 0; t < 6; t++)
            {
                var nextCells = new HashSet<(long x, long y, long z)>();
                var minZ = currentCells.Min(c => c.z) - 1;
                var maxZ = currentCells.Max(c => c.z) + 1;
                var minY = currentCells.Min(c => c.y) - 1;
                var maxY = currentCells.Max(c => c.y) + 1;
                var minX = currentCells.Min(c => c.x) - 1;
                var maxX = currentCells.Max(c => c.x) + 1;
                for (var z = minZ; z <= maxZ; z++)
                {
                    for (var y = minY; y <= maxY; y++)
                    {
                        for (var x = minX; x <= maxX; x++)
                        {
                            var currentCellIsActive = currentCells.Contains((x, y, z));
                            var activeCount = 0;
                            for (var az = Math.Max(z - 1, minZ + 1); az <= Math.Min(z + 1, maxZ - 1); az++)
                            {
                                for (var ay = Math.Max(y - 1, minY + 1); ay <= Math.Min(y + 1, maxY - 1); ay++)
                                {
                                    for (var ax = Math.Max(x - 1, minX + 1); ax <= Math.Min(x + 1, maxX - 1); ax++)
                                    {
                                        if (ax == x && ay == y && az == z) continue;
                                        if (currentCells.Contains((ax, ay, az))) activeCount++;
                                    }
                                }
                            }
                            var nextIsActive = false;
                            if (currentCellIsActive && (activeCount == 2 || activeCount == 3)) nextIsActive = true;
                            else if (!currentCellIsActive && activeCount == 3) nextIsActive = true;

                            if (nextIsActive) nextCells.Add((x, y, z));
                        }
                    }
                }
                currentCells = nextCells;
            }

            var expectedResult = 382;
            var result = currentCells.Count;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var currentCells = new HashSet<(long x, long y, long z, long w)>();
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#') currentCells.Add((x, y, 0, 0));
                }
            }

            for (var t = 0; t < 6; t++)
            {
                var nextCells = new HashSet<(long x, long y, long z, long w)>();
                var minZ = currentCells.Min(c => c.z) - 1;
                var maxZ = currentCells.Max(c => c.z) + 1;
                var minY = currentCells.Min(c => c.y) - 1;
                var maxY = currentCells.Max(c => c.y) + 1;
                var minX = currentCells.Min(c => c.x) - 1;
                var maxX = currentCells.Max(c => c.x) + 1;
                var minW = currentCells.Min(c => c.w) - 1;
                var maxW = currentCells.Max(c => c.w) + 1;
                for (var z = minZ; z <= maxZ; z++)
                {
                    for (var y = minY; y <= maxY; y++)
                    {
                        for (var x = minX; x <= maxX; x++)
                        {
                            for (var w = minW; w <= maxW; w++)
                            {
                                var currentCellIsActive = currentCells.Contains((x, y, z, w));
                                var activeCount = 0;
                                for (var az = Math.Max(z - 1, minZ + 1); az <= Math.Min(z + 1, maxZ - 1); az++)
                                {
                                    for (var ay = Math.Max(y - 1, minY + 1); ay <= Math.Min(y + 1, maxY - 1); ay++)
                                    {
                                        for (var ax = Math.Max(x - 1, minX + 1); ax <= Math.Min(x + 1, maxX - 1); ax++)
                                        {
                                            for (var aw = Math.Max(w - 1, minW + 1); aw <= Math.Min(w + 1, maxW - 1); aw++)
                                            {
                                                if (ax == x && ay == y && az == z && aw == w) continue;
                                                if (currentCells.Contains((ax, ay, az, aw))) activeCount++;
                                            }
                                        }
                                    }
                                }
                                var nextIsActive = false;
                                if (currentCellIsActive && (activeCount == 2 || activeCount == 3)) nextIsActive = true;
                                else if (!currentCellIsActive && activeCount == 3) nextIsActive = true;

                                if (nextIsActive) nextCells.Add((x, y, z, w));
                            }
                        }
                    }
                }
                currentCells = nextCells;
            }

            var expectedResult = 2552;
            var result = currentCells.Count; var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
