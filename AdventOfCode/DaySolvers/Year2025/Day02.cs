
using Helpers.Extensions;
using System.Text;

namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day02 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var ranges = lines[0].Split(",");

            var expectedResult = 34826702005d;
            var result = ranges.Sum(GetSumOfInvalidIdsEasy);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var ranges = lines[0].Split(",");
            var invalidIds = ranges.SelectMany(GetInvalidIdsHard).ToHashSet();

            var expectedResult = 43287141963d;
            var result = invalidIds.Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private double GetSumOfInvalidIdsEasy(string range)
        {
            var parts = range.Split("-");
            var lowerString = parts[0];
            var upperString = parts[1];
            var sum = 0d;

            if (lowerString.Length == upperString.Length && lowerString.Length % 2 == 1) return sum;

            if (lowerString.Length % 2 == 1)
            {
                lowerString = "1" + new string('0', lowerString.Length);
            }

            if (upperString.Length % 2 == 1)
            {
                upperString = new string('9', upperString.Length - 1);
            }

            var lowerDouble = lowerString.ToDouble();
            var upperDouble = upperString.ToDouble();
            while (lowerDouble < upperDouble)
            {
                var innerLowerString = lowerDouble.ToString();

                var tempUpperDouble = upperDouble;
                if (upperDouble / lowerDouble >= 10)
                {
                    tempUpperDouble = new string('9', innerLowerString.Length).ToDouble();
                }

                var innerLowerFirstHalf = innerLowerString[..(innerLowerString.Length / 2)].ToDouble();
                var innerUpperString = tempUpperDouble.ToString();
                var innerUpperFirstHalf = innerUpperString[..(innerUpperString.Length / 2)].ToDouble();
                for (var i = innerLowerFirstHalf; i <= innerUpperFirstHalf; i++)
                {
                    var innerDouble = (i.ToString() + i.ToString()).ToDouble();
                    if (innerDouble >= lowerDouble && innerDouble <= upperDouble) sum += innerDouble;
                }

                lowerDouble = ("1" + new string('0', innerLowerString.Length + 1)).ToDouble();
            }

            return sum;
        }

        private HashSet<double> GetInvalidIdsHard(string range)
        {
            var parts = range.Split("-");
            var lowerString = parts[0];
            var upperString = parts[1];

            var lowerDouble = lowerString.ToDouble();
            var upperDouble = upperString.ToDouble();
            var upperStringLength = upperDouble.ToString().Length;

            var hashSet = new HashSet<double>();
            while (lowerDouble < upperDouble)
            {
                var innerLowerString = lowerDouble.ToString();
                var innerLowerLength = innerLowerString.Length;

                var tempUpperDouble = upperDouble;
                if (innerLowerString.Length < upperStringLength)
                {
                    tempUpperDouble = new string('9', innerLowerString.Length).ToDouble();
                }

                for (var i = 1; i <= innerLowerLength / 2; i++)
                {
                    if (innerLowerLength % i == 0)
                    {
                        var timesToRepeat = innerLowerLength / i;
                        var min = ("1" + new string('0', i - 1)).ToDouble();
                        var max = (new string('9', i)).ToDouble();
                        for (var j = min; j <= max; j++)
                        {
                            var jString = j.ToString();
                            var valueToCheck = new StringBuilder(jString.Length * timesToRepeat).Insert(0, jString, timesToRepeat).ToString().ToDouble();
                            if (valueToCheck >= lowerDouble && valueToCheck <= upperDouble) hashSet.Add(valueToCheck);
                        }
                    }
                }

                lowerDouble = ("1" + new string('0', innerLowerString.Length)).ToDouble();
            }

            return hashSet;
        }
    }
}
