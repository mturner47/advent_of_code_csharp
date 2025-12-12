using Helpers.Extensions;

namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day05 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var (ranges, values) = Parse(lines);

            var expectedResult = 511;
            var result = values.Where(v => ranges.Any(r => v >= r.min && v <= r.max)).Count();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var (ranges, _) = Parse(lines);
            var rangesQueue = new PriorityQueue<(double min, double max), double>();
            foreach (var range in ranges) rangesQueue.Enqueue(range, range.min);

            var validRanges = new List<(double min, double max)>();
            while (rangesQueue.Count > 0)
            {
                var (newMin, newMax) = rangesQueue.Dequeue();
                var shouldAdd = true;
                for (var i = 0; i < validRanges.Count; i++)
                {
                    var (oldMin, oldMax) = validRanges[i];
                    if (oldMin >= newMax && oldMax <= newMin) continue;
                    if (oldMin <= newMin && oldMax >= newMax)
                    {
                        shouldAdd = false;
                        break;
                    }

                    if (newMin <= oldMax && oldMax < newMax)
                    {
                        shouldAdd = false;
                        rangesQueue.Enqueue((oldMax + 1, newMax), oldMax + 1);
                        break;
                    }

                    if (oldMin <= newMax && newMin < oldMin)
                    {
                        shouldAdd = false;
                        rangesQueue.Enqueue((newMin, oldMin - 1), newMin);
                        break;
                    }

                }

                if (shouldAdd)
                {
                    validRanges.Add((newMin, newMax));
                }
            }

            var expectedResult = 350939902751909d;
            var result = validRanges.Sum(r => r.max - r.min + 1);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (List<(double min, double max)> ranges, List<double> values) Parse(IList<string> lines)
        {
            var lineString = string.Join("##", lines);
            var parts = lineString.Split("####");
            var rangeLines = parts[0].Split("##");
            var values = parts[1].Split("##").Select(l => l.ToDouble()).ToList();
            var ranges = rangeLines.Select(rl =>
            {
                var rangeParts = rl.Split("-");
                return (min: rangeParts[0].ToDouble(), max: rangeParts[1].ToDouble());
            }).ToList();
            return (ranges, values);
        }


    }
}
