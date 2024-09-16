namespace AdventOfCode.Year2023
{
    internal class Day02 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var games = lines.Select(ParseLine);
            var sum = 0;
            foreach (var game in games)
            {
                if (game.Pulls.All(IsValidPull)) sum += game.GameNumber;
            }
            return sum;
        }

        public object HardSolution(IList<string> lines)
        {
            var games = lines.Select(ParseLine);
            return games.Sum(GetPowerOfCubes);
        }

        private Game ParseLine(string l)
        {
            l = l.Replace("Game ", "");
            var parts = l.Split(":");
            var gameNumber = int.Parse(parts[0]);
            var rawPulls = parts[1].Trim().Split(";");
            var pulls = rawPulls.Select(p => new Pull
            {
                Cubes = p.Split(",").Select(g =>
                    {
                        var gParts = g.Trim().Split(" ");
                        return new CubeGroup { Color = gParts[1], Count = int.Parse(gParts[0]) };
                    }).ToList()
            }).ToList();
            return new Game { GameNumber = gameNumber, Pulls = pulls };
        }

        private readonly Dictionary<string, int> _maximums = new Dictionary<string, int>
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 },
        };

        private bool IsValidPull(Pull pull)
        {
            foreach (var cubeGroup in pull.Cubes)
            {
                if (!_maximums.ContainsKey(cubeGroup.Color) || _maximums[cubeGroup.Color] < cubeGroup.Count)
                {
                    return false;
                }
            }
            return true;
        }

        private int GetPowerOfCubes(Game game)
        {
            var minimums = new Dictionary<string, int>();
            foreach (var pull in game.Pulls)
            {
                foreach (var cubeGroup in pull.Cubes)
                {
                    if (!minimums.ContainsKey(cubeGroup.Color))
                    {
                        minimums.Add(cubeGroup.Color, cubeGroup.Count);
                    }
                    else if (minimums[cubeGroup.Color] < cubeGroup.Count)
                    {
                        minimums[cubeGroup.Color] = cubeGroup.Count;
                    }
                }
            }
            return minimums.Values.Aggregate(1, (x, y) => x*y);
        }

        private class Game
        {
            public int GameNumber { get; set; }
            public List<Pull> Pulls { get; set; } = new();
        }

        private class Pull
        {
            public List<CubeGroup> Cubes { get; set; } = new();
        }

        private class CubeGroup
        {
            public string Color { get; set; } = "";
            public int Count { get; set; }
        }
    }
}
