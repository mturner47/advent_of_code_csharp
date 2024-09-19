namespace AdventOfCode.Year2018
{
    internal class Day23 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var nanobots = lines.Select(Parse).ToList();

            var (position, range) = nanobots.OrderByDescending(n => n.range).First();

            var expectedResult = 602;
            var result = nanobots.Count(n => InRange(n.position, position, range));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var nanobots = lines.Select(ParseToBot).ToList();

            var expectedResult = 110620102;
            var result = Solve(nanobots);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private ((long x, long y, long z) position, long range) Parse(string line)
        {
            var parts = line.Replace("pos=<", "").Split(">, r=");
            var range = long.Parse(parts[1]);
            var posParts = parts[0].Split(',');
            var position = (long.Parse(posParts[0]), long.Parse(posParts[1]), long.Parse(posParts[2]));
            return (position, range);
        }

        private static Bot ParseToBot(string line)
        {
            var parts = line.Replace("pos=<", "").Split(">, r=");
            var range = int.Parse(parts[1]);
            var posParts = parts[0].Split(',');
            var position = (int.Parse(posParts[0]), int.Parse(posParts[1]), int.Parse(posParts[2]));
            return new Bot(position, range);
        }


        private static bool InRange((long x, long y, long z) a, (long x, long y, long z) b, long range)
        {
            return range >= (Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z));
        }

        private static int Solve(List<Bot> bots)
        {
            var bestLocation = (x:0, y:0, z:0);
            var bestSum = 0;
            int chunkSize = (int)Math.Pow(2, 26);

            var minX = bots.Min(bot => bot.Location.X);
            var minY = bots.Min(bot => bot.Location.Y);
            var minZ = bots.Min(bot => bot.Location.Z);
            var maxX = bots.Max(bot => bot.Location.X);
            var maxY = bots.Max(bot => bot.Location.Y);
            var maxZ = bots.Max(bot => bot.Location.Z);

            int xRange = maxX - minX, yRange = maxY - minY, zRange = maxZ - minZ;

            while (chunkSize >= 1)
            {
                var maxInRange = 0;
                bestSum = int.MaxValue;
                for (int x = minX; x < maxX; x += chunkSize)
                {
                    for (int y = minY; y < maxY; y += chunkSize)
                    {
                        for (int z = minZ; z < maxZ; z += chunkSize)
                        {
                            var inRange = bots.Count(bot => bot.InRange(x, y, z));

                            if (inRange > maxInRange || inRange == maxInRange && Math.Abs(x) + Math.Abs(y) + Math.Abs(z) < bestSum)
                            {
                                maxInRange = inRange;
                                bestLocation = (x, y, z);
                                bestSum = Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
                            }
                        }
                    }
                }

                chunkSize /= 2;
                xRange /= 2;
                yRange /= 2;
                zRange /= 2;
                minX = bestLocation.x - xRange / 2;
                minY = bestLocation.y - yRange / 2;
                minZ = bestLocation.z - zRange / 2;
                maxX = bestLocation.x + xRange / 2;
                maxY = bestLocation.y + yRange / 2;
                maxZ = bestLocation.z + zRange / 2;
            }

            return bestSum;
        }

        private class Bot((int x, int y, int z) location, int radius)
        {
            public (int X, int Y, int Z) Location { get; private set; } = location; public int Radius { get; private set; } = radius;
            public bool InRange(int x, int y, int z) => Distance(x, y, z) <= Radius;
            public bool InRange(Bot otherBot) => InRange(otherBot.Location.X, otherBot.Location.Y, otherBot.Location.Z);
            public int Distance(int x, int y, int z) => Math.Abs(Location.X - x) + Math.Abs(Location.Y - y) + Math.Abs(Location.Z - z);
            public int Distance(Bot otherBot) => Distance(otherBot.Location.X, otherBot.Location.Y, otherBot.Location.Z);
        }
    }
}
