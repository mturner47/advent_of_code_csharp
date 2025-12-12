
using Helpers.Extensions;

namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day01 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var spinAmounts = lines.Select(ParseLine).ToList();
            var currentSpace = 50;
            var minSpace = 0;
            var maxSpace = 99;
            var timesHitZero = 0;

            for (var i = 0; i < spinAmounts.Count; i++)
            {
                currentSpace += spinAmounts[i];
                while (currentSpace > maxSpace) currentSpace -= 100;

                while (currentSpace < minSpace) currentSpace += 100;

                if (currentSpace == 0) timesHitZero++;
            }

            var expectedResult = 1036;
            var result = timesHitZero;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var spinAmounts = lines.Select(ParseLine).ToList();
            var currentSpace = 50;
            var minSpace = 0;
            var maxSpace = 99;
            var timesHitZero = 0;

            for (var i = 0; i < spinAmounts.Count; i++)
            {
                var oldSpace = currentSpace;
                currentSpace += spinAmounts[i];

                if (currentSpace > minSpace && currentSpace <= maxSpace) continue;
                if (currentSpace == 0)
                {
                    timesHitZero++;
                    continue;
                }

                if (currentSpace > maxSpace)
                {
                    while (currentSpace > maxSpace)
                    {
                        currentSpace -= 100;
                        timesHitZero++;
                    }
                    continue;
                }

                var isFirstTime = true;
                while (currentSpace < minSpace)
                {
                    currentSpace += 100;
                    if (!isFirstTime || oldSpace != 0) timesHitZero++;
                    isFirstTime = false;
                }

                if (currentSpace == 0) timesHitZero++;
            }

            var expectedResult = 6228;
            var result = timesHitZero;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
        private int ParseLine(string line)
        {
            var amount = line.Replace("L", "").Replace("R", "").ToInt();
            return line.StartsWith('L') ? -amount : amount;
        }
    }
}
