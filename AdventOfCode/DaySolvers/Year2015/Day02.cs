using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
    internal partial class Day02 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 1588178;
            var result = lines.Select(ParseLine).Sum(GetWrappingPaperRequired);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 3783758;
            var result = lines.Select(ParseLine).Sum(GetRibbonRequired);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int GetWrappingPaperRequired((int length, int width, int height) dimensions)
        {
            var (l, w, h) = dimensions;
            var lw = l * w;
            var wh = w * h;
            var lh = l * h;
            List<int> sideAreas = [lw, wh, lh];
            return 2 * lw + 2 * wh + 2 * lh + sideAreas.Min();
        }

        private static int GetRibbonRequired((int length, int width, int height) dimensions)
        {
            var (l, w, h) = dimensions;
            List<int> sideLengths = [l, w, h];
            var wrappingLength = (l + w + h - sideLengths.Max()) * 2;
            return l * w * h + wrappingLength;
        }

        private static (int length, int width, int height) ParseLine(string line)
        {
            var regex = ParseRegex();
            var groups = regex.Matches(line)[0].Groups;
            return (int.Parse(groups["length"].Value), int.Parse(groups["width"].Value), int.Parse(groups["height"].Value));
        }

        [GeneratedRegex(@"(?<length>\d+)x(?<width>\d+)x(?<height>\d+)")]
        private static partial Regex ParseRegex();
    }
}
