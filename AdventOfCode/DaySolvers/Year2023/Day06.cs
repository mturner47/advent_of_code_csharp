namespace AdventOfCode.Year2023
{
    internal class Day06 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var races = ConvertToRaces(lines);
            return races.Select(NumberOfRecordBeaters).Aggregate((double)1, (x, y) => x * y);
        }

        public object HardSolution(IList<string> lines)
        {
            var time = double.Parse(lines[0].Replace("Time:", "").Replace(" ", ""));
            var distance = double.Parse(lines[1].Replace("Distance:", "").Replace(" ", ""));
            var race = new Race { Time = time, Distance = distance };
            return NumberOfRecordBeaters(race);
        }

        public static List<Race> ConvertToRaces(IList<string> lines)
        {
            var times = lines[0].Replace("Time:", "").Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            var distances = lines[1].Replace("Distance:", "").Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            return times.Zip(distances).Select(t => new Race { Time = double.Parse(t.First), Distance = double.Parse(t.Second) }).ToList();
        }

        public static double NumberOfRecordBeaters(Race race)
        {
            var count = 0;
            for (var waitTime = 0; waitTime < race.Time; waitTime++)
            {
                var moveTime = race.Time - waitTime;
                if (waitTime * moveTime > race.Distance) count++;
            }
            return count;
        }

        public class Race
        {
            public double Time { get; set; }
            public double Distance { get; set; }
        }
    }
}
