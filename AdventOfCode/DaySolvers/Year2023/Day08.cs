using Helpers.Extensions;
using Helpers.Helpers;

namespace AdventOfCode.Year2023
{
    internal class Day08 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var movements = lines[0].Select(x => x == 'L' ? 0 : 1).ToList();
            var mappings = lines.Skip(2).Select(Mapping.ConvertFromLine).ToDictionary(m => m.BaseNode, m => m.ChildNodes);
            return GetCount(movements, mappings, "AAA", true);
        }

        public object HardSolution(IList<string> lines)
        {
            var movements = lines[0].Select(x => x == 'L' ? 0 : 1).ToList();
            var mappings = lines.Skip(2).Select(Mapping.ConvertFromLine).ToDictionary(m => m.BaseNode, m => m.ChildNodes);
            var nodes = mappings.Keys.Where(k => k.EndsWith("A"))
                .Select(n => GetCount(movements, mappings, n, false)).ToList();

            return nodes.Aggregate((double)1, MathHelpers.LeastCommonMultiplier);
        }

        private static double GetCount(List<int> movements, Dictionary<string, List<string>> mappings, string startingNode, bool allZ)
        {
            var node = startingNode;
            double count = 0;
            while (!((allZ && node == "ZZZ") || (!allZ && node.EndsWith("Z"))))
            {
                var movement = movements[(int)(count % movements.Count)];
                node = mappings[node][movement];
                count++;
            }

            return count;
        }

        private class Mapping
        {
            public string BaseNode { get; set; } = "";
            public List<string> ChildNodes { get; set; } = new();

            public static Mapping ConvertFromLine(string line)
            {
                var parts = line.Split(" = ");
                var baseNode = parts[0];
                var childNodes = parts[1].Replace(")", "").Replace("(", "").Split(", ").ToList();
                return new Mapping
                {
                    BaseNode = baseNode,
                    ChildNodes = childNodes,
                };
            }
        }
    }
}
