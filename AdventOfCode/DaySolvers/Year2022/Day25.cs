using System;
using System.Text;

namespace AdventOfCode.Year2022
{
    internal class Day25 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var base10Sum = lines.Select(ConvertSnafuToBase10).Sum();
            var base5 = ConvertBase10ToBase5(base10Sum);
            var snafu = ConvertBaseFiveToSnafu(base5);
            var base10Again = ConvertSnafuToBase10(snafu);
            return base10Sum + " => " + base5 + " => "+ snafu + " => " + base10Again;
        }

        public object HardSolution(IList<string> lines)
        {
            return 0;
        }

        private double ConvertSnafuToBase10(string s)
        {
            double sum = 0;
            double positionMultiplier = 1;
            for (var i = s.Length - 1; i >= 0; i--)
            {
                var multiplier = s[i] switch
                {
                    '2' => 2,
                    '1' => 1,
                    '0' => 0,
                    '-' => -1,
                    '=' => -2,
                    _ => throw new NotImplementedException(),
                };
                sum += positionMultiplier * multiplier;
                positionMultiplier *= 5;
            }
            return sum;
        }

        private static string ConvertBase10ToBase5(double d)
        {
            var s = "";
            while (true)
            {
                var newD = Math.Truncate(d / 5);
                var remainder = d % 5;
                s = remainder.ToString() + s;
                if (newD == 0)
                {
                    break;
                }
                d = newD;
            }
            return s;
        }

        private static string ConvertBaseFiveToSnafu(string base5s)
        {
            var shouldIncrease = false;
            var s = "";
            for (var i = base5s.Length - 1; i >= 0; i--)
            {
                var c = base5s[i];
                if (shouldIncrease && c == '0')
                {
                    shouldIncrease = false;
                    s = '1' + s;
                }
                else if (shouldIncrease && c == '1')
                {
                    shouldIncrease = false;
                    s = '2' + s;
                }
                else if (shouldIncrease && c == '2')
                {
                    shouldIncrease = true;
                    s = '=' + s;
                }
                else if (shouldIncrease && c == '3')
                {
                    shouldIncrease = true;
                    s = '-' + s;
                }
                else if (shouldIncrease && c == '4')
                {
                    shouldIncrease = true;
                    s = '0' + s;
                }
                else if (c == '0' || c == '1' || c == '2')
                {
                    shouldIncrease = false;
                    s = c + s;
                }
                else if (c == '3')
                {
                    shouldIncrease = true;
                    s = '=' + s;
                }
                else if (c == '4')
                {
                    shouldIncrease = true;
                    s = '-' + s;
                }
            }
            if (shouldIncrease)
            {
                s = '1' + s;
            }
            return s;
        }
    }
}
