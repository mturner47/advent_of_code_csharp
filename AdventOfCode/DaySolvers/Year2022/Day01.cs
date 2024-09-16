namespace AdventOfCode.Year2022
{
    internal class Day01 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var elves = new List<List<int>>
            {
                new List<int>()
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (i == 0 || lines[i - 1] == "")
                {
                    elves.Add(new List<int>());
                }
                if (lines[i] == "")
                {
                    continue;
                }

                elves[^1].Add(int.Parse(lines[i]));
            }

            return elves.Select(x => x.Sum()).Max();
        }

        public object HardSolution(IList<string> lines)
        {
            var elves = new List<List<int>>
            {
                new List<int>()
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (i == 0 || lines[i - 1] == "")
                {
                    elves.Add(new List<int>());
                }
                if (lines[i] == "")
                {
                    continue;
                }

                elves[^1].Add(int.Parse(lines[i]));
            }

            return elves.Select(x => x.Sum()).OrderByDescending(x => x).Take(3).Sum();
        }
    }
}
