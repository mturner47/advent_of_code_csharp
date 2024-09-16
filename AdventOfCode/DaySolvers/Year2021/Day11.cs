namespace AdventOfCode.Year2021
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var map = lines.Select(l => l.AsEnumerable().Select(char.GetNumericValue).ToList()).ToList();
            var state = new State(map, 0);
            for (var i = 0; i < 100; i++)
            {
                state = UpdateState(state);
            }
            return state.FlashCount;
        }

        public object HardSolution(IList<string> lines)
        {
            var map = lines.Select(l => l.AsEnumerable().Select(char.GetNumericValue).ToList()).ToList();
            var state = new State(map, 0);
            var step = 0;
            while (!state.Map.All(r => r.All(c => c == 0)))
            {
                state = UpdateState(state);
                step++;
            }
            return step;
        }

        private static State UpdateState(State state)
        {
            var map = state.Map.Select(r => r.Select(c => c).ToList()).ToList();
            var flashesToHandle = new Stack<(int, int)>();
            var maxX = map.Count;
            var maxY = map[0].Count;
            for (var x = 0; x < map.Count; x++)
            {
                var row = map[x];
                for (var y = 0; y < row.Count; y++)
                {
                    map[x][y] += 1;
                    if (map[x][y] > 9)
                    {
                        flashesToHandle.Push((x, y));
                        map[x][y] = 0;
                    }
                }
            }

            var handledFlashes = new List<(int, int)>();
            while (flashesToHandle.Any())
            {
                (var x, var y) = flashesToHandle.Pop();
                handledFlashes.Add((x, y));
                var locationsToCheck = new List<(int, int)>
                {
                    (x - 1, y - 1), (x - 1, y), (x - 1, y + 1),
                    (x, y - 1), (x, y + 1),
                    (x + 1, y - 1), (x + 1, y), (x + 1, y + 1),
                };

                foreach (var location in locationsToCheck)
                {
                    if (handledFlashes.Contains(location) || flashesToHandle.Contains(location))
                    {
                        continue;
                    }
                    (var innerX, var innerY) = location;
                    if (innerX < 0 || innerY < 0 || innerX >= maxX || innerY >= maxY)
                    {
                        continue;
                    }

                    map[innerX][innerY] += 1;
                    if (map[innerX][innerY] > 9)
                    {
                        flashesToHandle.Push(location);
                        map[innerX][innerY] = 0;
                    }
                }
            }

            return new State(map, state.FlashCount + handledFlashes.Count);
        }

        public record State (List<List<double>> Map, double FlashCount);
    }
}
