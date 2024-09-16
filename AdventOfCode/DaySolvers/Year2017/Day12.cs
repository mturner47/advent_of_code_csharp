
namespace AdventOfCode.Year2017
{
    internal class Day12 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var connections = lines.Select(Parse).ToDictionary(t => t.source, t => t.destinations);
            var connectedNodes = new List<string> { "0" };
            var nodesToCheck = new Queue<string>();
            nodesToCheck.Enqueue("0");
            while (nodesToCheck.Count > 0)
            {
                var node = nodesToCheck.Dequeue();
                var nodeConnections = connections[node];
                foreach (var nodeConnection in nodeConnections)
                {
                    if (!connectedNodes.Contains(nodeConnection))
                    {
                        connectedNodes.Add(nodeConnection);
                        nodesToCheck.Enqueue(nodeConnection);
                    }
                }
            }

            var expectedResult = 141;
            var result = connectedNodes.Count;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var connections = lines.Select(Parse).ToDictionary(t => t.source, t => t.destinations);
            var groupCount = 0;
            while (connections.Count > 0)
            {
                groupCount++;
                var startingNode = connections.Keys.First();
                var connectedNodes = new List<string> { startingNode };
                var nodesToCheck = new Queue<string>();
                nodesToCheck.Enqueue(startingNode);
                while (nodesToCheck.Count > 0)
                {
                    var node = nodesToCheck.Dequeue();
                    var nodeConnections = connections[node];
                    foreach (var nodeConnection in nodeConnections)
                    {
                        if (!connectedNodes.Contains(nodeConnection))
                        {
                            connectedNodes.Add(nodeConnection);
                            nodesToCheck.Enqueue(nodeConnection);
                        }
                    }
                }
                foreach (var node in connectedNodes)
                {
                    connections.Remove(node);
                }
            }

            var expectedResult = 171;
            var result = groupCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private (string source, List<string> destinations) Parse(string line)
        {
            var parts = line.Split(" <-> ");
            var source = parts[0];
            var destinations = parts[1].Split(", ").ToList();
            return (source, destinations);
        }
    }
}
