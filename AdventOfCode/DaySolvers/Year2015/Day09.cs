using Helpers.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
    internal partial class Day09 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 251;
            var result = FindDistance(lines, true);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 898;
            var result = FindDistance(lines, false);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private long FindDistance(IList<string> lines, bool findShortest)
        {
            var nodes = new List<string>();
            var edges = new List<Edge>();
            foreach (var line in lines)
            {
                var matchGroups = MyRegex().Matches(line)[0].Groups;
                var node1 = matchGroups[1].Value;
                var node2 = matchGroups[2].Value;
                var distance = long.Parse(matchGroups[3].Value);
                nodes.Add(node1);
                nodes.Add(node2);
                edges.Add(new Edge
                {
                    SourceNode = node1,
                    DestinationNode = node2,
                    Distance = distance,
                });
                edges.Add(new Edge
                {
                    SourceNode = node2,
                    DestinationNode = node1,
                    Distance = distance,
                });
            }

            nodes = nodes.Distinct().ToList();
            var nodeDictionary = nodes.ToDictionary(n => n, n => new Node());

            ulong i = 1;
            ulong goal = 0;
            long goalDistance = findShortest ? long.MaxValue : 0;
            foreach (var node in nodes)
            {
                nodeDictionary[node].Flag = i;
                goal |= i;
                i <<= 1;
            }

            foreach (var edge in edges)
            {
                nodeDictionary[edge.SourceNode].Edges.Add(edge);
            }

            var queue = new PriorityQueue<(string nodeName, ulong visitedNodes, long distance, string path), long>();
            foreach (var node in nodeDictionary.Keys)
            {
                queue.Enqueue((node, nodeDictionary[node].Flag, 0, node), 10000 - 0);
            }

            while (queue.Count > 0)
            {
                var (nodeName, visitedNodes, distance, path) = queue.Dequeue();
                var node = nodeDictionary[nodeName];
                foreach (var edge in node.Edges)
                {
                    var connectedNode = nodeDictionary[edge.DestinationNode];
                    var newVisitedNodes = visitedNodes | connectedNode.Flag;
                    if (newVisitedNodes == visitedNodes) continue;

                    var newPath = path + ", " + edge.DestinationNode;
                    var newDistance = distance + edge.Distance;
                    if (newVisitedNodes == goal)
                    {
                        if (findShortest && goalDistance > newDistance)
                        {
                            goalDistance = newDistance;
                        }

                        if (!findShortest && goalDistance < newDistance)
                        {
                            goalDistance = newDistance;
                        }
                        continue;
                    }

                    queue.Enqueue((edge.DestinationNode, newVisitedNodes, newDistance, newPath), 10000 - newDistance);
                }
            }
            return goalDistance;
        }

        private class Edge
        {
            public string SourceNode { get; set; } = "";
            public string DestinationNode { get; set; } = "";
            public long Distance { get; set; }
        }

        private class Node
        {
            public List<Edge> Edges { get; set; } = [];
            public ulong Flag { get; set; }
        }

        [GeneratedRegex("([^ ]+) to ([^ ]+) = (\\d+)")]
        private static partial Regex MyRegex();
    }
}
