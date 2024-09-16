namespace AdventOfCode.Year2018
{
    internal class Day04 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var guardSleepMinutes = new Dictionary<string, Dictionary<int, int>>();
            var currentGuard = "";
            var currentSleepStart = -1;
            foreach (var line in lines.OrderBy(l => l))
            {
                var minute = int.Parse(line[15..17]);
                var record = line[19..];
                if (record.StartsWith("Guard"))
                {
                    currentGuard = record.Replace("Guard #", "").Replace(" begins shift", "");
                    if (!guardSleepMinutes.ContainsKey(currentGuard))
                    {
                        var minutesDict = Enumerable.Range(0, 60).ToDictionary(i => i, _ => 0);
                        guardSleepMinutes.Add(currentGuard, minutesDict);
                    }
                }

                if (record.StartsWith("falls"))
                {
                    currentSleepStart = minute;
                }

                if (record.StartsWith("wakes"))
                {
                    for (var i = currentSleepStart; i < minute; i++)
                    {
                        guardSleepMinutes[currentGuard][i]++;
                    }
                }
            }
            var worstGuard = guardSleepMinutes.OrderByDescending(g => g.Value.Values.Sum()).First();
            var worstMinute = worstGuard.Value.OrderByDescending(m => m.Value).First();

            var expectedResult = 138280;
            var result = int.Parse(worstGuard.Key) * worstMinute.Key;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var guardSleepMinutes = new Dictionary<string, Dictionary<int, int>>();
            var currentGuard = "";
            var currentSleepStart = -1;
            foreach (var line in lines.OrderBy(l => l))
            {
                var minute = int.Parse(line[15..17]);
                var record = line[19..];
                if (record.StartsWith("Guard"))
                {
                    currentGuard = record.Replace("Guard #", "").Replace(" begins shift", "");
                    if (!guardSleepMinutes.ContainsKey(currentGuard))
                    {
                        var minutesDict = Enumerable.Range(0, 60).ToDictionary(i => i, _ => 0);
                        guardSleepMinutes.Add(currentGuard, minutesDict);
                    }
                }

                if (record.StartsWith("falls"))
                {
                    currentSleepStart = minute;
                }

                if (record.StartsWith("wakes"))
                {
                    for (var i = currentSleepStart; i < minute; i++)
                    {
                        guardSleepMinutes[currentGuard][i]++;
                    }
                }
            }

            var worstMinutes = guardSleepMinutes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.OrderByDescending(m => m.Value).First());
            var worstGuard = worstMinutes.OrderByDescending(kvp => kvp.Value.Value).First();

            var expectedResult = 89347;
            var result = int.Parse(worstGuard.Key)*worstGuard.Value.Key;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
