using System.Text.RegularExpressions;

namespace AdventOfCode.Year2017
{
    internal partial class Day20 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var particles = lines
                .Select((l, i) => (num: i, stats: Parse(l)))
                .OrderBy(c => ManhattanDistance(c.stats.acceleration));
            var expectedResult = 364;
            var result = particles.First().num;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var particles = lines.Select(Parse).ToList();

            for (var t = 0; t < 1000; t++)
            {
                var groups = particles.GroupBy(g => g.position);
                foreach (var groupWithCollision in groups.Where(g => g.Count() > 1))
                {
                    particles = particles.Where(p => p.position != groupWithCollision.Key).ToList();
                }

                for (var i = 0; i < particles.Count; i++)
                {
                    var (position, velocity, acceleration) = particles[i];
                    velocity = (velocity.x + acceleration.x, velocity.y + acceleration.y, velocity.z + acceleration.z);
                    position = (position.x + velocity.x, position.y + velocity.y, position.z + velocity.z);
                    particles[i] = (position, velocity, acceleration);
                }
            }

            var expectedResult = 420;
            var result = particles.Count;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private ((long x, long y, long z) position, (long x, long y, long z) velocity, (long x, long y, long z) acceleration) Parse(string line)
        {
            var regex = ParticleRegex();
            var matches = regex.Matches(line)[0];
            return (
                (long.Parse(matches.Groups[1].Value), long.Parse(matches.Groups[2].Value), long.Parse(matches.Groups[3].Value)),
                (long.Parse(matches.Groups[4].Value), long.Parse(matches.Groups[5].Value), long.Parse(matches.Groups[6].Value)),
                (long.Parse(matches.Groups[7].Value), long.Parse(matches.Groups[8].Value), long.Parse(matches.Groups[9].Value))
            );
        }

        private static long ManhattanDistance((long x, long y, long z) coordinate, (long x, long y, long z)? other = null)
        {
            other ??= (0, 0, 0);
            return Math.Abs(other.Value.x - coordinate.x) + Math.Abs(other.Value.y - coordinate.y) + Math.Abs(other.Value.z - coordinate.z);
        }

        [GeneratedRegex(@"p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>")]
        private static partial Regex ParticleRegex();
    }
}
