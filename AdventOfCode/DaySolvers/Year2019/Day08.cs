namespace AdventOfCode.Year2019
{
    internal class Day08 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var line = lines[0];
            var width = 25;
            var height = 6;
            var layers = GetImage(lines[0], width, height);

            var selectedLayer = layers.OrderBy(l => l.Sum(r => r.Count(c => c == '0'))).First();
            var oneCount = selectedLayer.Sum(r => r.Count(c => c == '1'));
            var twoCount = selectedLayer.Sum(r => r.Count(c => c == '2'));
            return oneCount * twoCount;
        }

        public object HardSolution(IList<string> lines)
        {
            var width = 25;
            var height = 6;

            var layers = GetImage(lines[0], width, height);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    foreach (var layer in layers)
                    {
                        var c = layer[y][x];
                        if (c == '2') continue;

                        Console.BackgroundColor = c == '0'
                            ? ConsoleColor.Black
                            : ConsoleColor.White;
                        Console.Write(' ');
                        break;
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
            return 0;
        }

        private List<List<List<char>>> GetImage(string line, int width, int height)
        {
            var layers = new List<List<List<char>>>();
            var currentLayer = new List<List<char>>();
            foreach (var c in line)
            {
                if (currentLayer.Count == 0)
                {
                    currentLayer.Add([]);
                }
                var currentRow = currentLayer.Last();
                if (currentRow.Count == width)
                {
                    if (currentLayer.Count == height)
                    {
                        layers.Add(currentLayer);
                        currentLayer = [[]];
                    }
                    else
                    {
                        currentLayer.Add([]);
                    }
                    currentRow = currentLayer.Last();
                }
                currentRow.Add(c);
            }
            layers.Add(currentLayer);
            return layers;
        }
    }
}
