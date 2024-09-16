using Helpers.Helpers;

namespace AdventOfCode.Year2023
{
    internal class Day17 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            return GetSolution(lines, 1, 3);
        }

        public object HardSolution(IList<string> lines)
        {
            return GetSolution(lines, 4, 10);
        }

        private static long GetSolution(IList<string> lines, int minBlocksTraveled, int maxBlocksTraveled)
        {
            var grid = lines.Select(l => l.Select(Block.ConvertFromChar).ToList()).ToList();
            var maxX = grid[0].Count - 1;
            var maxY = grid.Count - 1;

            var routes = new PriorityQueue<((int x, int y) position, long heatLost, Direction direction, int blocksTraveledInDirection, List<(int x, int y)> path), long>();
            routes.Enqueue(((1, 0), grid[0][1].Loss, Direction.East, 1, new List<(int x, int y)> { (1, 0) }), grid[0][1].Loss);
            routes.Enqueue(((0, 1), grid[1][0].Loss, Direction.South, 1, new List<(int x, int y)> { (0, 1) }), grid[1][0].Loss);

            while (routes.Count > 0)
            {
                var (position, heatLost, direction, blocksTraveledInDirection, path) = routes.Dequeue();
                var (x, y) = position;
                var block = grid[y][x];
                var key = (direction, blocksTraveledInDirection);
                if (!block.LeastLoss.TryGetValue(key, out var v) || v.leastLoss > heatLost)
                {
                    block.LeastLoss[key] = (heatLost, path);
                }
                else continue;

                foreach (var nextDirection in NextDirections(direction, blocksTraveledInDirection, minBlocksTraveled, maxBlocksTraveled))
                {
                    var nextPosition = nextDirection.GetMovement(position);
                    var (nextX, nextY) = nextPosition;
                    if (nextX < 0 || nextX > maxX || nextY < 0 || nextY > maxY) continue;

                    var nextBlocksTraveled = nextDirection == direction ? (blocksTraveledInDirection + 1) : 1;

                    if (nextX == maxX && nextY == maxY && nextBlocksTraveled < minBlocksTraveled) continue;

                    var nextBlock = grid[nextY][nextX];
                    var nextHeatLost = heatLost + nextBlock.Loss;
                    var nextKey = (nextDirection, nextBlocksTraveled);
                    if (nextBlock.LeastLoss.TryGetValue(nextKey, out var llv) && llv.leastLoss < nextHeatLost) continue;

                    var nextPath = path.Concat(new List<(int x, int y)> { nextPosition }).ToList();
                    routes.Enqueue((nextPosition, nextHeatLost, nextDirection, nextBlocksTraveled, nextPath), nextHeatLost);
                }
            }

            var lastBlock = grid.Last().Last();
            var leastLoss = lastBlock.LeastLoss.Values.OrderBy(llv => llv.leastLoss).First();
            return leastLoss.leastLoss;
        }

        private class Block
        {
            public long Loss { get; set; }
            public Dictionary<(Direction direction, int blocksTraveled), (long leastLoss, List<(int x, int y)> bestPath)> LeastLoss { get; set; } = new();

            public static Block ConvertFromChar(char c)
            {
                var loss = long.Parse(c.ToString());
                return new Block
                {
                    Loss = loss,
                };
            }
        }

        private static List<Direction> NextDirections(Direction d, int numTraveled, int minAllowed, int maxAllowed)
        {
            var nextDirections = new List<Direction>();
            if (numTraveled < maxAllowed) nextDirections.Add(d);
            if (numTraveled > minAllowed - 1)
            {
                if (d == Direction.North || d == Direction.South)
                {
                    nextDirections.Add(Direction.East);
                    nextDirections.Add(Direction.West);
                }
                else
                {
                    nextDirections.Add(Direction.South);
                    nextDirections.Add(Direction.North);
                }
            }
            return nextDirections;
        }
    }
}
