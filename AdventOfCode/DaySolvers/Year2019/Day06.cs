
namespace AdventOfCode.Year2019
{
    internal class Day06 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var directOrbits = lines.Select(Parse).ToList();
            var nodes = directOrbits.Select(o => o.inner)
                .Union(directOrbits.Select(o => o.outer)).Distinct()
                .ToDictionary(n => n, n => new Node { Name = n });

            var startingNode = "COM";
            nodes[startingNode].NumOrbits = 0;

            while (nodes.Values.Any(n => !n.IsFound))
            {
                var minNode = nodes.Values.Where(n => !n.IsFound).OrderBy(n => n.NumOrbits).First();
                var directOrbittingNodes = directOrbits.Where(d => d.inner == minNode.Name);
                foreach (var (_, outer) in directOrbittingNodes)
                {
                    nodes[outer].NumOrbits = minNode.NumOrbits + 1;
                }
                minNode.IsFound = true;
            }
            return nodes.Values.Sum(n => n.NumOrbits);
        }

        public object HardSolution(IList<string> lines)
        {
            var directOrbits = lines.Select(Parse).ToList();
            var nodes = directOrbits.Select(o => o.inner)
                .Union(directOrbits.Select(o => o.outer)).Distinct()
                .ToDictionary(n => n, n => new Node { Name = n });

            var startingNode = directOrbits.FirstOrDefault(o => o.outer == "YOU").inner;
            var endingNode = directOrbits.FirstOrDefault(o => o.outer == "SAN").inner;
            nodes[startingNode].NumOrbits = 0;

            while (!nodes[endingNode].IsFound)
            {
                var minNode = nodes.Values.Where(n => !n.IsFound).OrderBy(n => n.NumOrbits).First();
                var nearbyNodes = directOrbits.Where(d => d.inner == minNode.Name).Select(d => d.outer)
                    .Union(directOrbits.Where(d => d.outer == minNode.Name).Select(d => d.inner));
                foreach (var nearbyNode in nearbyNodes)
                {
                    if (nodes[nearbyNode].IsFound) continue;
                    nodes[nearbyNode].NumOrbits = minNode.NumOrbits + 1;
                }
                minNode.IsFound = true;
            }

            return nodes[endingNode].NumOrbits;
        }

        private (string inner, string outer) Parse(string line)
        {
            var parts = line.Split(")");
            return (parts[0], parts[1]);
        }

        private class Node
        {
            public string Name = "";
            public bool IsFound = false;
            public int NumOrbits = int.MaxValue;
        }
    }
}
