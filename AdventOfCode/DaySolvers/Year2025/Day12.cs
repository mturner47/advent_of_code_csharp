using Helpers.Extensions;

namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day12 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var (shapes, containers) = Parse(lines);

            var expectedResult = 443;
            var result = containers.Count(c => CanFit(c, shapes));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = -1;
            var result = 0;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (List<List<string>> shapes, List<(int width, int height, int[] packagesRequired)> containers) Parse(IList<string> lines)
        {
            var joinedInput = string.Join("~", lines);
            var sectionParts = joinedInput.Split("~~");

            var shapes = sectionParts.Take(sectionParts.Length - 1).Select(x => x.Split("~").ToList()).ToList();

            var containerParts = sectionParts.Last().Split("~");
            var containers = containerParts.Select(x =>
            {
                var xParts = x.Split(": ");
                var dimensionParts = xParts[0].Split("x");
                var width = dimensionParts[0].ToInt();
                var height = dimensionParts[1].ToInt();
                var packagesRequired = xParts[1].Split(" ").Select(s => s.ToInt()).ToArray();
                return (width, height, packagesRequired);
            }).ToList();

            return (shapes, containers);
        }

        private static bool CanFit((int width, int height, int[] packagesRequired) container, List<List<string>> shapes)
        {
            var (width, height, packagesRequired) = container;
            var shapeWidth = shapes[0].Count;
            var shapeHeight = shapes[0][0].Length;
            var numShapesFitWidth = width / shapeWidth;
            var numShapesFitHeight = height / shapeHeight;
            var totalPackagesRequired = packagesRequired.Sum();
            if (totalPackagesRequired < numShapesFitHeight * numShapesFitWidth) return true;
            return false;
        }
    }
}
