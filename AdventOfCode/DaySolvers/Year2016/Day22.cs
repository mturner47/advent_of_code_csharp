
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016
{
    internal partial class Day22 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var nodes = lines.Skip(2).Select(ParseLine).ToList();
            var viablePairCount = 0;
            for (var i = 0; i < nodes.Count - 1; i++)
            {
                for (var j = i; j < nodes.Count; j++)
                {
                    if (nodes[i].Available >= nodes[j].Used && nodes[j].Used > 0) viablePairCount++;
                    if (nodes[j].Available >= nodes[i].Used && nodes[i].Used > 0) viablePairCount++;
                }
            }
            var expectedResult = -1;
            var result = viablePairCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var nodes = lines.Skip(2).Select(ParseLine).OrderBy(n => n.Position.y).ThenBy(n => n.Position.x).ToList();

            var expectedResult = 249;
            var result = 64+37*5;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private Node ParseLine(string line)
        {
            var regex = NodeRegex();
            var matches = regex.Matches(line)[0];
            var x = int.Parse(matches.Groups[1].Value);
            var y = int.Parse(matches.Groups[2].Value);
            var size = int.Parse(matches.Groups[3].Value);
            var used = int.Parse(matches.Groups[4].Value);
            var available = int.Parse(matches.Groups[5].Value);

            return new Node
            {
                Position = (x, y),
                Size = size,
                Used = used,
                Available = available,
            };
        }

        private class Node
        {
            public (int x, int y) Position;
            public int Size;
            public int Used;
            public int Available;
        }

        //private void PrintGrid(List<Node> nodes)
        //{
        //    Node? priorNode = null;
        //    foreach (var node in nodes)
        //    {
        //        if (priorNode == null || priorNode.Position.y != node.Position.y)
        //        {
        //            Console.WriteLine();
        //        }
        //        if (node.Used == 0) Console.Write("O");
        //        else if (node.Used > 100) Console.Write("X");
        //        else Console.Write("_");
        //        priorNode = node;
        //    }
        //    Console.WriteLine();
        //}

        [GeneratedRegex(@"/dev/grid/node-x(\d+)-y(\d+) +(\d+)T +(\d+)T +(\d+)T.*")]
        private static partial Regex NodeRegex();
    }
}
