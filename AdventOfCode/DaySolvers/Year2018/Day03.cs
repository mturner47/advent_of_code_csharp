
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018
{
    internal partial class Day03 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var grid = Enumerable.Range(0, 1000).Select(r => Enumerable.Range(0, 1000).Select(c => 0).ToList()).ToList();

            var areas = lines.Select(Parse).ToList();

            foreach (var (num, (startX, startY), (sizeX, sizeY)) in areas)
            {
                for (var y = startY; y < startY + sizeY; y++)
                {
                    for (var x = startX; x < startX + sizeX; x++)
                    {
                        grid[y][x]++;
                    }
                }
            }

            var expectedResult = 121259;
            var result = grid.Sum(r => r.Count(i => i > 1));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var grid = Enumerable.Range(0, 1000).Select(r => Enumerable.Range(0, 1000).Select(c => new List<int>()).ToList()).ToList();

            var areas = lines.Select(Parse).ToList();
            var freeAreas = areas.Select(a => a.num).ToList();

            foreach (var (num, (startX, startY), (sizeX, sizeY)) in areas)
            {
                for (var y = startY; y < startY + sizeY; y++)
                {
                    for (var x = startX; x < startX + sizeX; x++)
                    {
                        foreach (var seen in grid[y][x])
                        {
                            freeAreas.Remove(seen);
                        }
                        if (grid[y][x].Count > 0) freeAreas.Remove(num);
                        grid[y][x].Add(num);
                    }
                }
            }

            var expectedResult = 239;
            var result = freeAreas.First();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private (int num, (int x, int y) start, (int x, int y) size) Parse(string line)
        {
            var regex = AreaRegex();
            var groups = regex.Matches(line)[0].Groups;
            var num = int.Parse(groups[1].Value);
            var start = (int.Parse(groups[2].Value), int.Parse(groups[3].Value));
            var size = (int.Parse(groups[4].Value), int.Parse(groups[5].Value));

            return (num, start, size);
        }

        [GeneratedRegex(@"^#(\d+) @ (\d+),(\d+): (\d+)x(\d+)$")]
        private static partial Regex AreaRegex();
    }
}
