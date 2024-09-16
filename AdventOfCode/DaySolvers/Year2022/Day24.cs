using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Year2022
{
    internal class Day24 : IDaySolver
    {
        private static List<List<(int x, int y, Direction d)>> _blizzardPositionCache = new();

        public object EasySolution(IList<string> lines)
        {
            var (grid, blizzards) = ParseLines(lines);
            return FindPath(grid, blizzards);
        }

        public object HardSolution(IList<string> lines)
        {
            var (grid, blizzards) = ParseLines(lines);
            return FindHardPath(grid, blizzards);
        }

        private class Grid
        {
            public (int x, int y) StartingPosition { get; set; }
            public (int x, int y) FinishPosition { get; set; }
            public int MaxX { get; set; }
            public int MaxY { get; set; }
            public int MinTimeFromStartToFinish { get; set; }
        }

        private static (Grid, List<(int x, int y, Direction d)>) ParseLines(IList<string> lines)
        {
            var maxY = lines.Count - 3;
            var maxX = lines[0].Length - 3;
            var startingPosition = (0, -1);
            var finishPosition = (maxX, maxY + 1);
            var minTime = MDistance(startingPosition, finishPosition);
            var blizzards = new List<(int x, int y, Direction d)>();

            for (var y = 1; y < lines.Count - 1; y++)
            {
                var line = lines[y];
                for (var x = 1; x < line.Length - 1; x++)
                {
                    var c = line[x];
                    if (c == '.') continue;

                    var direction = c switch
                    {
                        '>' => Direction.Right,
                        'v' => Direction.Down,
                        '<' => Direction.Left,
                        '^' => Direction.Up,
                        _ => throw new NotImplementedException(),
                    };
                    blizzards.Add((x - 1, y - 1, direction));
                }
            }

            var grid = new Grid
            {
                StartingPosition = startingPosition,
                FinishPosition = finishPosition,
                MaxX = maxX,
                MaxY = maxY,
                MinTimeFromStartToFinish = minTime,
            };
            return (grid, blizzards);
        }

        private static int MDistance((int x, int y) position1, (int x, int y) position2)
        {
            return Math.Abs(position2.x - position1.x) + Math.Abs(position2.y - position1.y);
        }

        private static bool AreEqualPositions((int x, int y) position1, (int x, int y) position2)
        {
            return position1.x == position2.x && position1.y == position2.y;
        }

        private static int FindPath(Grid grid, List<(int x, int y, Direction d)> startingBlizzardPositions)
        {
            var statesToCheck = new List<State> { new State { Position = grid.StartingPosition, Time = 0, TripNumber = 1 } };
            _blizzardPositionCache = new List<List<(int x, int y, Direction d)>> { startingBlizzardPositions };
            var lowestTime = 999999999;
            var possibleMoves = new List<(int x, int y)> { (1, 0), (0, 1), (0, 0), (-1, 0), (0, -1), };
            var equalityComparer = new StateEqualityComparer();
            while (statesToCheck.Count > 0)
            {
                var state = statesToCheck.First();
                statesToCheck = statesToCheck.Skip(1).ToList();
                if (AreEqualPositions(state.Position, grid.FinishPosition))
                {
                    if (state.Time < lowestTime)
                    {
                        lowestTime = state.Time;
                        continue;
                    }
                }

                var blizzardPositions = GetNextBlizzardPositions(grid, state.Time + 1);
                var (x, y) = state.Position;
                var nextStates = possibleMoves
                    .Select(pm =>
                    {
                        var newPosition = (x + pm.x, y + pm.y);
                        var newTime = state.Time + 1;
                        var minPossibleTime = MDistance(newPosition, grid.FinishPosition) + newTime;
                        return new State
                        {
                            Position = newPosition,
                            Time = newTime,
                            MinPossibleTime = minPossibleTime,
                        };
                    })
                    .Where(s => AreEqualPositions(s.Position, grid.FinishPosition) || (s.Position.x >= 0 && s.Position.x <= grid.MaxX && s.Position.y >= 0 && s.Position.y <= grid.MaxY))
                    .Where(s => !blizzardPositions.Any(nbp => nbp.x == s.Position.x && nbp.y == s.Position.y))
                    .ToList();

                statesToCheck = statesToCheck.Concat(nextStates)
                    .Where(s => s.MinPossibleTime < lowestTime)
                    .Distinct(equalityComparer)
                    .OrderBy(s => s.MinPossibleTime)
                    .ToList();
            }
            return lowestTime;
        }

        private static int FindHardPath(Grid grid, List<(int x, int y, Direction d)> startingBlizzardPositions)
        {
            var statesToCheck = new List<State> { new State { Position = grid.StartingPosition, Time = 0, TripNumber = 1 } };
            _blizzardPositionCache = new List<List<(int x, int y, Direction d)>> { startingBlizzardPositions };
            var lowestTime = 999999999;
            var possibleMoves = new List<(int x, int y)> { (1, 0), (0, 1), (0, 0), (-1, 0), (0, -1), };
            var equalityComparer = new StateEqualityComparer();
            while (statesToCheck.Count > 0)
            {
                var state = statesToCheck.First();
                statesToCheck = statesToCheck.Skip(1).ToList();
                if (state.TripNumber == 3 && AreEqualPositions(state.Position, grid.FinishPosition))
                {
                    if (state.Time < lowestTime)
                    {
                        lowestTime = state.Time;
                        continue;
                    }
                }

                var blizzardPositions = GetNextBlizzardPositions(grid, state.Time + 1);
                var (x, y) = state.Position;
                var nextStates = possibleMoves
                    .Select(pm =>
                    {
                        var newPosition = (x + pm.x, y + pm.y);
                        var newTime = state.Time + 1;
                        var tripNumber = state.TripNumber;
                        if (tripNumber == 1 && AreEqualPositions(newPosition, grid.FinishPosition)) tripNumber = 2;
                        if (tripNumber == 2 && AreEqualPositions(newPosition, grid.StartingPosition)) tripNumber = 3;
                        var newState = new State
                        {
                            Position = newPosition,
                            Time = newTime,
                            TripNumber = tripNumber,
                        };
                        newState.MinPossibleTime = HardMinTime(newState, grid);
                        return newState;
                    })
                    .Where(s => IsValidState(s, grid))
                    .Where(s => !blizzardPositions.Any(nbp => nbp.x == s.Position.x && nbp.y == s.Position.y))
                    .ToList();

                statesToCheck = statesToCheck.Concat(nextStates)
                    .Where(s => s.MinPossibleTime < lowestTime)
                    .Distinct(equalityComparer)
                    .OrderBy(s => s.MinPossibleTime)
                    .ThenByDescending(s => s.TripNumber)
                    .ToList();
            }
            return lowestTime;
        }

        private static bool IsValidState(State state, Grid grid)
        {
            var position = state.Position;
            if (AreEqualPositions(position, grid.StartingPosition)) return true;
            if (AreEqualPositions(position, grid.FinishPosition)) return true;
            if (position.x >= 0 && position.x <= grid.MaxX && position.y >= 0 && position.y <= grid.MaxY) return true;
            return false;
        }

        private static int HardMinTime(State state, Grid grid)
        {
            if (state.TripNumber == 1)
            {
                return state.Time + grid.MinTimeFromStartToFinish * 2 + MDistance(state.Position, grid.FinishPosition);
            }

            if (state.TripNumber == 2)
            {
                return state.Time + grid.MinTimeFromStartToFinish + MDistance(state.Position, grid.StartingPosition);
            }

            return state.Time + MDistance(state.Position, grid.FinishPosition);
        }

        private static List<(int x, int y, Direction d)> GetNextBlizzardPositions(Grid grid, int time)
        {
            if (_blizzardPositionCache.Count >= time + 1)
            {
                return _blizzardPositionCache[time];
            }

            var newBlizzardPositions = _blizzardPositionCache[time - 1].Select(blizzard =>
            {
                var x = blizzard.x;
                var y = blizzard.y;
                switch (blizzard.d)
                {
                    case Direction.Left:
                        x = x == 0 ? grid.MaxX : x - 1;
                        break;
                    case Direction.Right:
                        x = x == grid.MaxX ? 0 : x + 1;
                        break;
                    case Direction.Up:
                        y = y == 0 ? grid.MaxY : y - 1;
                        break;
                    case Direction.Down:
                        y = y == grid.MaxY ? 0 : y + 1;
                        break;
                }
                return (x, y, blizzard.d);
            }).ToList();
            _blizzardPositionCache.Add(newBlizzardPositions);
            return newBlizzardPositions;
        }

        private class State
        {
            public (int x, int y) Position { get; set; }
            public int Time { get; set; }
            public int MinPossibleTime { get; set; }
            public int TripNumber { get; set; }
        }

        private class StateEqualityComparer : IEqualityComparer<State>
        {
            public bool Equals(State? x, State? y)
            {
                if (x == null || y == null) return false;
                return x.Position.x == y.Position.x
                    && x.Position.y == y.Position.y
                    && x.Time == y.Time
                    && x.MinPossibleTime == y.MinPossibleTime
                    && x.TripNumber == y.TripNumber;
            }

            public int GetHashCode([DisallowNull] State obj)
            {
                return obj.Position.x * 2 + obj.Position.y * 3 + obj.Time * 5 + obj.MinPossibleTime * 7 + obj.TripNumber * 11;
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right,
        }
    }
}
