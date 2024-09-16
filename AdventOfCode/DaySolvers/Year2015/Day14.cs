using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
    internal partial class Day14 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var reindeer = GetReindeer(lines);
            var timeToTravel = 2503;
            var distancesTraveled = reindeer.Select(rd =>
            {
                var cycleDuration = rd.FlyDuration + rd.RestDuration;
                var cycleValue = rd.FlyDuration * rd.Velocity;
                var numFullCycles = timeToTravel / cycleDuration;
                var distanceTraveled = numFullCycles * cycleValue;
                var remaining = timeToTravel % cycleDuration;
                return (rd, distance: distanceTraveled + Math.Min(remaining, rd.FlyDuration) * rd.Velocity);
            }).OrderByDescending(x => x.distance).ToList();

            var expectedResult = 2655;
            var result = distancesTraveled.First().distance;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var reindeer = GetReindeer(lines);

            var timeToTravel = 2503;

            for (var i = 0; i < timeToTravel; i++)
            {
                foreach (var rd in reindeer)
                {
                    if (rd.IsCurrentlyFlying)
                    {
                        if (rd.TimeSpentFlying < rd.FlyDuration)
                        {
                            rd.TimeSpentFlying++;
                            rd.TotalDistanceTraveled += rd.Velocity;
                        }
                        else
                        {
                            rd.IsCurrentlyFlying = false;
                            rd.TimeSpentResting = 1;
                        }
                    }
                    else
                    {
                        if (rd.TimeSpentResting < rd.RestDuration)
                        {
                            rd.TimeSpentResting++;
                        }
                        else
                        {
                            rd.IsCurrentlyFlying = true;
                            rd.TimeSpentFlying = 1;
                            rd.TotalDistanceTraveled += rd.Velocity;
                        }
                    }
                }
                var maxDistanceTraveled = reindeer.Max(rd => rd.TotalDistanceTraveled);
                foreach (var rd in reindeer.Where(rd => rd.TotalDistanceTraveled == maxDistanceTraveled))
                {
                    rd.Points++;
                }
            }

            var expectedResult = 1059;
            var result = reindeer.Max(rd => rd.Points);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<Reindeer> GetReindeer(IList<string> lines)
        {
            var regex = ParseRegex();
            return lines.Select(l =>
            {
                var groups = regex.Matches(l)[0].Groups;
                return new Reindeer
                {
                    Name = groups["name"].Value,
                    Velocity = int.Parse(groups["velocity"].Value),
                    FlyDuration = int.Parse(groups["flyDuration"].Value),
                    RestDuration = int.Parse(groups["restDuration"].Value),
                };
            }).ToList();
        }

        private class Reindeer
        {
            public required string Name;
            public int Velocity;
            public int FlyDuration;
            public int RestDuration;
            public int TotalDistanceTraveled = 0;
            public int TimeSpentFlying = 0;
            public int TimeSpentResting = 0;
            public bool IsCurrentlyFlying = true;
            public int Points = 0;
        }

        [GeneratedRegex(@"^(?<name>[^ ]*) [^\d]+(?<velocity>\d+)[^\d]+(?<flyDuration>\d+)[^\d]+(?<restDuration>\d+).*")]
        private static partial Regex ParseRegex();
    }
}
