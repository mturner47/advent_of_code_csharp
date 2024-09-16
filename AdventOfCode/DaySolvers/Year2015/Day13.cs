using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
    internal partial class Day13 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var regex = ParseRegex();

            var neighbors = lines.SelectMany(l =>
            {
                var groups = regex.Matches(l)[0].Groups;
                var sourceName = groups[1].Value;
                var happinessAmount = int.Parse(groups[3].Value);
                if (groups[2].Value == "lose") happinessAmount = -happinessAmount;
                var destinationName = groups[4].Value;
                return new List<((string sourceName, string destinationName) neighbors, int happinessAmount)>
                {
                    ((sourceName, destinationName), happinessAmount), ((destinationName, sourceName), happinessAmount)
                };
            }).ToList();

            var edgeDict = neighbors.GroupBy(n => n.neighbors, n => n.happinessAmount).ToDictionary(n => n.Key, n => n.Sum());
            var nodes = edgeDict.Keys.Select(n => n.sourceName).Distinct().ToList();
            var nodeHashes = new Dictionary<string, ulong>();

            ulong i = 0b1;
            ulong goal = 0b1;
            foreach (var node in nodes)
            {
                nodeHashes[node] = i;
                goal |= i;
                i <<= 1;
            }

            var startingNeighbor = nodes[0];
            var startingHash = nodeHashes[startingNeighbor];

            var queue = new PriorityQueue<(string currentNode, ulong hash, int happiness), int>();
            foreach (var (sourceName, destinationName) in edgeDict.Keys.Where(e => e.sourceName == startingNeighbor))
            {
                var destHash = nodeHashes[destinationName];
                var hash = startingHash | destHash;
                var happiness = edgeDict[(sourceName, destinationName)];
                queue.Enqueue((destinationName, hash, happiness), 100000 - happiness);
            }

            var maxHappiness = 0;
            while (queue.Count > 0)
            {
                var (currentNode, hash, happiness) = queue.Dequeue();
                if (hash == goal)
                {
                    happiness += edgeDict[(startingNeighbor, currentNode)];
                    if (happiness > maxHappiness)
                    {
                        maxHappiness = happiness;
                    }
                    continue;
                }

                foreach (var (sourceName, destinationName) in edgeDict.Keys.Where(e => e.sourceName == currentNode))
                {
                    var destHash = nodeHashes[destinationName];
                    var nextHash = hash | destHash;
                    if (nextHash == hash) continue;

                    var nextHappiness = happiness + edgeDict[(sourceName, destinationName)];
                    queue.Enqueue((destinationName, nextHash, nextHappiness), 100000 - nextHappiness);
                }
            }

            var expectedResult = 618;
            var result = maxHappiness;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var regex = ParseRegex();

            var neighbors = lines.SelectMany(l =>
            {
                var groups = regex.Matches(l)[0].Groups;
                var sourceName = groups[1].Value;
                var happinessAmount = int.Parse(groups[3].Value);
                if (groups[2].Value == "lose") happinessAmount = -happinessAmount;
                var destinationName = groups[4].Value;
                return new List<((string sourceName, string destinationName) neighbors, int happinessAmount)>
                {
                    ((sourceName, destinationName), happinessAmount), ((destinationName, sourceName), happinessAmount)
                };
            }).ToList();

            var edgeDict = neighbors.GroupBy(n => n.neighbors, n => n.happinessAmount).ToDictionary(n => n.Key, n => n.Sum());
            var nodes = edgeDict.Keys.Select(n => n.sourceName).Distinct().ToList();
            foreach (var node in nodes)
            {
                edgeDict.Add(("me", node), 0);
            }
            nodes.Add("me");

            var nodeHashes = new Dictionary<string, ulong>();

            ulong i = 0b1;
            ulong goal = 0b1;
            foreach (var node in nodes)
            {
                nodeHashes[node] = i;
                goal |= i;
                i <<= 1;
            }

            var startingNeighbor = "me";
            var startingHash = nodeHashes[startingNeighbor];

            var queue = new PriorityQueue<(string currentNode, ulong hash, int happiness), int>();
            foreach (var (sourceName, destinationName) in edgeDict.Keys.Where(e => e.sourceName == startingNeighbor))
            {
                var destHash = nodeHashes[destinationName];
                var hash = startingHash | destHash;
                var happiness = edgeDict[(sourceName, destinationName)];
                queue.Enqueue((destinationName, hash, happiness), 100000 - happiness);
            }

            var maxHappiness = 0;
            while (queue.Count > 0)
            {
                var (currentNode, hash, happiness) = queue.Dequeue();
                if (hash == goal)
                {
                    happiness += edgeDict[(startingNeighbor, currentNode)];
                    if (happiness > maxHappiness)
                    {
                        maxHappiness = happiness;
                    }
                    continue;
                }

                foreach (var (sourceName, destinationName) in edgeDict.Keys.Where(e => e.sourceName == currentNode))
                {
                    var destHash = nodeHashes[destinationName];
                    var nextHash = hash | destHash;
                    if (nextHash == hash) continue;

                    var nextHappiness = happiness + edgeDict[(sourceName, destinationName)];
                    queue.Enqueue((destinationName, nextHash, nextHappiness), 100000 - nextHappiness);
                }
            }

            var expectedResult = 601;
            var result = maxHappiness;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private class Neighbor
        {
            public string DestinationName { get; set; } = "";
            public int HappinessAmount { get; set; }
            public int NeighborHappinessAmount { get; set; }
            public int TotalHappiness => HappinessAmount + NeighborHappinessAmount;
        }

        [GeneratedRegex(@"^([^ ]+).*(lose|gain) (\d+).* ([^.]+).")]
        private static partial Regex ParseRegex();
    }
}
