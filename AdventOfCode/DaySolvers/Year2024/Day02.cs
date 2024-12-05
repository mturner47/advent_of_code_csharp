namespace AdventOfCode.Year2024
{
    internal class Day02 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var reports = lines.Select(l => l.Split(' ').Select(int.Parse).ToList()).ToList();
            var expectedResult = 624;
            var result = reports.Count(IsSafeEasy);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var reports = lines.Select(l => l.Split(' ').Select(int.Parse).ToList()).ToList();
            var expectedResult = -1;
            var result = reports.Count(IsSafeHard);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static bool IsSafeEasy(List<int> report)
        {
            int? lastNum = null;
            bool? isDescending = null;
            for (var i = 0; i < report.Count; i++)
            {
                var num = report[i];
                if (!lastNum.HasValue)
                {
                    lastNum = num;
                    continue;
                }

                if (!isDescending.HasValue)
                {
                    var diff = Math.Abs(num - lastNum.Value);
                    if (diff < 1 || diff > 3) return false;

                    isDescending = num < lastNum.Value;
                    lastNum = num;
                    continue;
                }

                if (isDescending.Value && num >= lastNum.Value) return false;
                if (!isDescending.Value && num <= lastNum.Value) return false;

                var difference = Math.Abs(num - lastNum.Value);
                if (difference < 1 || difference > 3) return false;

                lastNum = num;
            }
            return true;
        }

        private static bool IsSafeHard(List<int> report)
        {
            for (var i = 0; i < report.Count; i++)
            {
                var partialReport = report.Take(i).Concat(report.Skip(i + 1)).ToList();
                if (IsSafeEasy(partialReport)) return true;
            }
            return false;
        }
    }
}
