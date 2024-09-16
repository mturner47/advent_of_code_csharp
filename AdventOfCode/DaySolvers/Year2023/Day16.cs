using Helpers.Helpers;

namespace AdventOfCode.Year2023
{
    internal class Day16 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            return NumEnergizedFromStartingPoint(lines, (0, 0), Direction.East);
        }

        public object HardSolution(IList<string> lines)
        {
            var max = 0;
            var maxY = lines.Count - 1;
            var maxX = lines[0].Length - 1;
            for(var y = 0; y < lines.Count; y++)
            {
                var numEnergizedEast = NumEnergizedFromStartingPoint(lines, (0, y), Direction.East);
                if (numEnergizedEast > max) max = numEnergizedEast;

                var numEnergizedWest = NumEnergizedFromStartingPoint(lines, (maxX, y), Direction.West);
                if (numEnergizedWest > max) max = numEnergizedWest;
            }

            for (var x = 0; x < lines[0].Length; x++)
            {
                var numEnergizedSouth = NumEnergizedFromStartingPoint(lines, (x, 0), Direction.South);
                if (numEnergizedSouth > max) max = numEnergizedSouth;

                var numEnergizedNorth = NumEnergizedFromStartingPoint(lines, (x, maxY), Direction.North);
                if (numEnergizedNorth > max) max = numEnergizedNorth;
            }

            return max;
        }

        private static int NumEnergizedFromStartingPoint(IList<string> lines, (int x, int y) position, Direction d)
        {
            var tiles = lines.Select(l => l.Select(c => (value: c, energized: false)).ToList()).ToList();

            var seenBeams = new List<((int x, int y) postiion, Direction direction)>();
            var beams = new Stack<((int x, int y) postiion, Direction direction, int tick)>();
            beams.Push((position, d, 0));

            while (beams.Any())
            {
                ((var x, var y), var direction, var tick) = beams.Pop();
                if (y < 0 || x < 0 || y >= tiles.Count || x >= tiles[0].Count) continue;
                var c = tiles[y][x].value;
                tiles[y][x] = (c, true);
                var newDirections = GetNewDirections(direction, c);
                foreach (var newDirection in newDirections)
                {
                    var newPosition = newDirection.GetMovement((x, y));

                    if (!seenBeams.Contains((newPosition, newDirection)))
                    {
                        seenBeams.Add((newPosition, newDirection));
                        beams.Push((newPosition, newDirection, tick + 1));
                    }
                }
            }

            return tiles.Sum(l => l.Count(c => c.energized));
        }

        private static List<Direction> GetNewDirections(Direction d, char c)
        {
            return (d, c) switch
            {
                (Direction.East, '|') or (Direction.West, '|') => new List<Direction> { Direction.North, Direction.South },
                (Direction.North, '-') or (Direction.South, '-') => new List<Direction> { Direction.East, Direction.West },
                (Direction.East, '/') or (Direction.West, '\\') => new List<Direction> { Direction.North },
                (Direction.West, '/') or (Direction.East, '\\') => new List<Direction> { Direction.South },
                (Direction.North, '/') or (Direction.South, '\\') => new List<Direction> { Direction.East },
                (Direction.South, '/') or (Direction.North, '\\') => new List<Direction> { Direction.West },
                _ => new List<Direction> { d },
            };
        }
    }
}
