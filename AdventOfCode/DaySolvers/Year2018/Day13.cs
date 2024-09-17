using Helpers.Helpers;

namespace AdventOfCode.Year2018
{
    internal class Day13 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var (grid, carts) = Parse(lines);
            (int x, int y)? crashSite = null;

            while (!crashSite.HasValue)
            {
                var nextCarts = new List<(int currentIndex, (int x, int y) position, Direction direction, int numTurns)>();
                foreach (var (currentIndex, position, direction, numTurns) in carts)
                {
                    var (x, y) = direction.GetMovement(position);
                    if (direction == Direction.East || direction == Direction.South)
                    {
                        if (carts.Any(c => c.position == (x, y)))
                        {
                            crashSite = (x, y);
                            break;
                        }
                    }
                    else
                    {
                        if (nextCarts.Any(c => c.position == (x, y)))
                        {
                            crashSite = (x, y);
                            break;
                        }
                    }
                    var newDirection = direction;
                    var newNumTurns = numTurns;
                    var track = grid[y][x];
                    if (track == '+')
                    {
                        if (numTurns == 0) newDirection = direction.GetCCW();
                        if (numTurns == 2) newDirection = direction.GetCW();
                        newNumTurns = (numTurns + 1) % 3;
                    }
                    else if (track != '-' && track != '|')
                    {
                        switch (track, direction)
                        {
                            case ('/', Direction.North):
                            case ('\\', Direction.South):
                                newDirection = Direction.East;
                                break;
                            case ('/', Direction.West):
                            case ('\\', Direction.East):
                                newDirection = Direction.South;
                                break;
                            case ('/', Direction.South):
                            case ('\\', Direction.North):
                                newDirection = Direction.West;
                                break;
                            case ('/', Direction.East):
                            case ('\\', Direction.West):
                                newDirection = Direction.North;
                                break;
                        }
                    }
                    nextCarts.Add((currentIndex, (x, y), newDirection, newNumTurns));
                }
                carts = nextCarts;
            }

            var expectedResult = "80,100";
            var result = $"{crashSite?.x},{crashSite?.y}";
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var (grid, carts) = Parse(lines);

            while (carts.Count > 1)
            {
                var nextCarts = new List<(int currentIndex, (int x, int y) position, Direction direction, int numTurns)>();
                var cartsToSkip = new HashSet<int>();
                foreach (var (currentIndex, position, direction, numTurns) in carts)
                {
                    if (cartsToSkip.Contains(currentIndex)) continue;
                    var (x, y) = direction.GetMovement(position);
                    if (direction == Direction.East || direction == Direction.South)
                    {
                        var collidingCart = carts.FirstOrDefault(c => c.position == (x, y));
                        if (collidingCart != default)
                        {
                            cartsToSkip.Add(collidingCart.index);
                            continue;
                        }
                    }
                    else
                    {
                        var collidingCart = nextCarts.FirstOrDefault(c => c.position == (x, y));
                        if (collidingCart != default)
                        {
                            nextCarts.Remove(collidingCart);
                            continue;
                        }
                    }
                    var newDirection = direction;
                    var newNumTurns = numTurns;
                    var track = grid[y][x];
                    if (track == '+')
                    {
                        if (numTurns == 0) newDirection = direction.GetCCW();
                        if (numTurns == 2) newDirection = direction.GetCW();
                        newNumTurns = (numTurns + 1) % 3;
                    }
                    else if (track != '-' && track != '|')
                    {
                        switch (track, direction)
                        {
                            case ('/', Direction.North):
                            case ('\\', Direction.South):
                                newDirection = Direction.East;
                                break;
                            case ('/', Direction.West):
                            case ('\\', Direction.East):
                                newDirection = Direction.South;
                                break;
                            case ('/', Direction.South):
                            case ('\\', Direction.North):
                                newDirection = Direction.West;
                                break;
                            case ('/', Direction.East):
                            case ('\\', Direction.West):
                                newDirection = Direction.North;
                                break;
                        }
                    }
                    nextCarts.Add((currentIndex, (x, y), newDirection, newNumTurns));
                }
                carts = nextCarts.OrderBy(c => c.position.y).ThenBy(c => c.position.x).ToList();
            }

            var expectedResult = "16,99";
            var result = $"{carts[0].position.x},{carts[0].position.y}";
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (List<List<char>> grid, List<(int index, (int x, int y) position, Direction direction, int numTurns)> carts) Parse(IList<string> lines)
        {
            var grid = lines.Select(l => l.Select(c => c).ToList()).ToList();
            var carts = new List<(int index, (int x, int y) position, Direction direction, int numTurns)>();
            var currentIndex = 0;
            for (var y = 0; y < grid.Count; y++)
            {
                var line = grid[y];
                for (var x = 0; x < grid[y].Count; x++)
                {
                    var c = line[x];
                    if (c == '^')
                    {
                        carts.Add((currentIndex, (x, y), Direction.North, 0));
                        currentIndex++;
                        grid[y][x] = '|';
                    }
                    if (c == 'v')
                    {
                        carts.Add((currentIndex, (x, y), Direction.South, 0));
                        currentIndex++;
                        grid[y][x] = '|';
                    }
                    if (c == '>')
                    {
                        carts.Add((currentIndex, (x, y), Direction.East, 0));
                        currentIndex++;
                        grid[y][x] = '-';
                    }
                    if (c == '<')
                    {
                        carts.Add((currentIndex, (x, y), Direction.West, 0));
                        currentIndex++;
                        grid[y][x] = '-';
                    }
                }
            }
            return (grid, carts);
        }
    }
}
