namespace AdventOfCode.Year2018
{
    internal class Day07 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var orderings = lines.Select(Parse).ToList();
            var prereqs = orderings.Select(o => o.a)
                .Union(orderings.Select(o => o.b))
                .Distinct()
                .ToDictionary(c => c, c => orderings.Where(o => o.b == c).Select(o => o.a).ToList());
            var completedSteps = "";

            while (completedSteps.Length < prereqs.Keys.Count)
            {
                var nextStep = prereqs
                    .Where(p => !completedSteps.Contains(p.Key) && completedSteps.Intersect(p.Value).Count() == p.Value.Count)
                    .OrderBy(p => p.Key)
                    .First()
                    .Key;
                completedSteps += nextStep;
            }
            var expectedResult = "IBJTUWGFKDNVEYAHOMPCQRLSZX";
            var result = completedSteps;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var orderings = lines.Select(Parse).ToList();
            var prereqs = orderings.Select(o => o.a)
                .Union(orderings.Select(o => o.b))
                .Distinct()
                .ToDictionary(c => c, c => orderings.Where(o => o.b == c).Select(o => o.a).ToList());
            var completedSteps = "";
            var numWorkers = 5;
            var offset = 60;
            var workers = new List<Worker>();
            var t = 0;
            while (completedSteps.Length < prereqs.Keys.Count)
            {
                foreach (var worker in workers)
                {
                    worker.SecondsLeft--;

                    if (worker.SecondsLeft <= 0)
                    {
                        completedSteps += worker.Step;
                    }
                }

                workers = workers.Where(w => w.SecondsLeft > 0).ToList();

                if (workers.Count < numWorkers)
                {
                    workers.AddRange(prereqs
                        .Where(p => !workers.Any(w => w.Step == p.Key) && !completedSteps.Contains(p.Key) && completedSteps.Intersect(p.Value).Count() == p.Value.Count)
                        .OrderBy(p => p.Key)
                        .Take(numWorkers - workers.Count)
                        .Select(p => new Worker { Step = p.Key, SecondsLeft = p.Key - 64 + offset }));
                }
                t++;
            }

            var expectedResult = 0;
            var result = t - 1;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (char a, char b) Parse(string line)
        {
            var parts = line.Replace("Step ", "").Replace(" can begin.", "").Split(" must be finished before step ");
            return (parts[0][0], parts[1][0]);
        }

        private class Worker
        {
            public char Step;
            public int SecondsLeft;
        }
    }
}
