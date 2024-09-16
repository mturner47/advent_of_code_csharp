namespace AdventOfCode.Year2023
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var emptyRows = new List<int>();
            var emptyCols = new List<int>();
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].All(c => c == '.')) emptyRows.Add(i);
            }

            for (var i = 0; i < lines[0].Length; i++)
            {
                if (lines.All(l => l[i] == '.')) emptyCols.Add(i);
            }

            var galaxyLocations = new List<(int x, int y)>();
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#') galaxyLocations.Add((x, y));
                }
            }

            var sumDistances = 0;
            for (var i = 0; i < galaxyLocations.Count; i++)
            {
                for (var j = i + 1; j < galaxyLocations.Count; j++)
                {
                    var (x1, y1) = galaxyLocations[i];
                    var (x2, y2) = galaxyLocations[j];

                    var xDistance = Math.Abs(x2 - x1) + emptyCols.Count(c => c > Math.Min(x1, x2) && c < Math.Max(x1, x2));
                    var yDistance = Math.Abs(y2 - y1) + emptyRows.Count(c => c > Math.Min(y1, y2) && c < Math.Max(y1, y2));
                    sumDistances += xDistance + yDistance;
                }
            }

            return sumDistances;
        }

        public object HardSolution(IList<string> lines)
        {
            var emptyRows = new List<int>();
            var emptyCols = new List<int>();
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].All(c => c == '.')) emptyRows.Add(i);
            }

            for (var i = 0; i < lines[0].Length; i++)
            {
                if (lines.All(l => l[i] == '.')) emptyCols.Add(i);
            }

            var galaxyLocations = new List<(int x, int y)>();
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#') galaxyLocations.Add((x, y));
                }
            }

            double sumDistances = 0;
            for (var i = 0; i < galaxyLocations.Count; i++)
            {
                for (var j = i + 1; j < galaxyLocations.Count; j++)
                {
                    var (x1, y1) = galaxyLocations[i];
                    var (x2, y2) = galaxyLocations[j];

                    var xDistance = Math.Abs(x2 - x1) + ((double)1000000 - 1)*emptyCols.Count(c => c > Math.Min(x1, x2) && c < Math.Max(x1, x2));
                    var yDistance = Math.Abs(y2 - y1) + ((double)1000000 - 1)*emptyRows.Count(c => c > Math.Min(y1, y2) && c < Math.Max(y1, y2));
                    sumDistances += xDistance + yDistance;
                }
            }

            return sumDistances;
        }
    }
}
