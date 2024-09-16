using System.Text;

namespace AdventOfCode.Year2018
{
    internal class Day05 : IDaySolver
    {
        private const string _lower = "abcdefghijklmnopqrstuvwxyz";
        private const string _upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public object EasySolution(IList<string> lines)
        {
            var polymer = lines[0];

            var expectedResult = 9390;
            var result = ReducePolymer(polymer).Length;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var polymer = lines[0];

            var minLength = polymer.Length;
            for (var i = 0; i < _lower.Length; i++)
            {
                var currentPolymer = polymer.Replace(_lower[i].ToString(), "").Replace(_upper[i].ToString(), "");
                var length = ReducePolymer(currentPolymer).Length;
                if (length < minLength) minLength = length;
            }

            var expectedResult = -1;
            var result = minLength;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static string ReducePolymer(string polymer)
        {
            while (true)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < polymer.Length; i++)
                {
                    var a = polymer[i];
                    if (polymer.Length == i + 1) sb.Append(a);
                    else
                    {
                        var b = polymer[i + 1];
                        if (_lower.Contains(a) && _lower.Contains(b)) sb.Append(a);
                        else if (_upper.Contains(a) && _upper.Contains(b)) sb.Append(a);
                        else if (char.ToUpper(a) != char.ToUpper(b)) sb.Append(a);
                        else i++;
                    }
                }

                var newPolymer = sb.ToString();
                if (polymer == newPolymer) return newPolymer;
                polymer = newPolymer;
            }
        }
    }
}
