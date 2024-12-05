namespace AdventOfCode.Year2020
{
    internal class Day20 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var tiles = string.Join("\n", lines).Split("\n\n").Select(Parse).ToList();

            var matches = new HashSet<((int tn, string en) t1, (int tn, string en) t2)>();
            for (var i = 0; i < tiles.Count - 1; i++)
            {
                var (tn1, edges1) = tiles[i];
                for (var j = i + 1; j < tiles.Count; j++)
                {
                    var (tn2, edges2) = tiles[j];
                    foreach (var (en1, pattern1) in edges1)
                    {
                        foreach (var (en2, pattern2) in edges2)
                        {
                            if (pattern1 == pattern2) matches.Add(((tn1, en1), (tn2, en2)));
                        }
                    }
                }
            }

            var cornerTiles = tiles
                .Where(t => matches.Count(m => m.t1.tn == t.id || m.t2.tn == t.id) == 4)
                .Select(t => t.id)
                .ToList();
            var expectedResult = 27798062994017;
            var result = cornerTiles.Aggregate(1L, (a, b) => a*b);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var tiles = string.Join("\n", lines).Split("\n\n").Select(Parse).ToList();

            var matches = new HashSet<((int tn, string en) t1, (int tn, string en) t2)>();
            for (var i = 0; i < tiles.Count - 1; i++)
            {
                var (tn1, edges1) = tiles[i];
                for (var j = i + 1; j < tiles.Count; j++)
                {
                    var (tn2, edges2) = tiles[j];
                    foreach (var (en1, pattern1) in edges1)
                    {
                        foreach (var (en2, pattern2) in edges2)
                        {
                            if (pattern1 == pattern2) matches.Add(((tn1, en1), (tn2, en2)));
                        }
                    }
                }
            }

            var cornerTiles = tiles.Where(t => matches.Count(m => m.t1.tn == t.id || m.t2.tn == t.id) == 4).ToList();
            for (var y = 0; y < 12; y++)
            {
                for (var x = 0; x < 12; x++)
                {

                }
            }

            var expectedResult = -1;
            var result = 0;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (int id, List<(string id, string pattern)> edges) Parse(string line)
        {
            var parts = line.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var id = int.Parse(parts[0].Replace("Tile ", "").Replace(":", ""));
            var tileLines = parts.Skip(1).ToList();

            var topEdge = tileLines.First();
            var bottomEdge = tileLines.Last();
            var rightEdge = "";
            var leftEdge = "";
            for (var i = 0; i < tileLines.Count; i++)
            {
                leftEdge += tileLines[i].First();
                rightEdge += tileLines[i].Last();
            }

            var edges = new List<(string id, string pattern)>
            {
                ("NT", topEdge),
                ("NR", rightEdge),
                ("NB", bottomEdge),
                ("NL", leftEdge),
                ("FT", new string(topEdge.Reverse().ToArray())),
                ("FR", new string(rightEdge.Reverse().ToArray())),
                ("FB", new string(bottomEdge.Reverse().ToArray())),
                ("FL", new string(leftEdge.Reverse().ToArray())),
            };
            return (id, edges);
        }
    }
}
