namespace AdventOfCode.Year2023
{
    internal class Day03 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var sum = 0;
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    if (char.IsNumber(c))
                    {
                        var numberStart = x;
                        while (x + 1 < line.Length && char.IsNumber(line[x + 1]))
                        {
                            x++;
                        }

                        if (IsSchematic(lines, y, numberStart, x))
                        {
                            sum += int.Parse(line.Substring(numberStart, x - numberStart + 1));
                        }
                    }
                }
            }

            return sum;
        }

        public object HardSolution(IList<string> lines)
        {
            var possibleGears = new Dictionary<(int x, int y), List<int>>();
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    if (char.IsNumber(c))
                    {
                        var numberStart = x;
                        while (x + 1 < line.Length && char.IsNumber(line[x + 1]))
                        {
                            x++;
                        }

                        MarkPossibleGears(lines, possibleGears, y, numberStart, x);
                    }
                }
            }

            return possibleGears.Values.Where(g => g.Count == 2).Select(g => g[0] * g[1]).Sum();
        }

        private static bool IsSchematic(IList<string> lines, int yIndex, int xStart, int xEnd)
        {
            var xLength = lines[0].Length;
            var yLength = lines.Count;
            for (var y = yIndex - 1; y <= yIndex + 1; y++)
            {
                for (var x = xStart - 1; x <= xEnd + 1; x++)
                {
                    if (x >= 0 && x < xLength && y >= 0 && y < yLength)
                    {
                        if (IsSpecialSymbol(lines[y][x])) return true;
                    }
                }
            }

            return false;
        }

        private static void MarkPossibleGears(IList<string> lines, Dictionary<(int x, int y), List<int>> possibleGears, int yIndex, int xStart, int xEnd)
        {
            var xLength = lines[0].Length;
            var yLength = lines.Count;
            for (var y = yIndex - 1; y <= yIndex + 1; y++)
            {
                for (var x = xStart - 1; x <= xEnd + 1; x++)
                {
                    if (x >= 0 && x < xLength && y >= 0 && y < yLength)
                    {
                        if (lines[y][x] == '*')
                        {
                            var gearKey = (x, y);
                            var number = int.Parse(lines[yIndex].Substring(xStart, xEnd - xStart + 1));
                            if (possibleGears.ContainsKey(gearKey))
                            {
                                possibleGears[gearKey].Add(number);
                            }
                            else
                            {
                                possibleGears.Add(gearKey, new List<int> { number });
                            }
                        }
                    }
                }
            }
        }

        private static bool IsSpecialSymbol(char c)
        {
            return !char.IsNumber(c) && c != '.';
        }
    }
}
