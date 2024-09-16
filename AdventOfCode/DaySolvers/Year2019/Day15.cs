using AdventOfCode.DaySolvers.Year2019;
using Helpers.Helpers;

namespace AdventOfCode.Year2019
{
    internal class Day15 : IDaySolver
    {
        private readonly List<Direction> _directions = DirectionExtensions.EnumerateDirections();

        public object EasySolution(IList<string> lines)
        {
            var output = IntCodeComputer.ParseAndRunProgram(lines);

            var startingPosition = (0, 0);
            var currentPosition = startingPosition;
            (int x, int y)? goalLocation = null;
            var knownLocations = new Dictionary<(int x, int y), (bool isWall, bool isGoal, int minDistance)>
            {
                { currentPosition, (false, false, 0) }
            };

            var lastDirectionAttempted = Direction.North;

            while (true)
            {
                if (output.Outputs.Count > 0)
                {
                    var (_, _, priorMinDistance) = knownLocations[currentPosition];
                    switch (output.Outputs[0])
                    {
                        case 0:
                            var wallLocation = lastDirectionAttempted.GetMovement(currentPosition);
                            knownLocations.Add(wallLocation, (true, false, 0));
                            break;
                        case 1:
                            currentPosition = lastDirectionAttempted.GetMovement(currentPosition);
                            if (!knownLocations.TryGetValue(currentPosition, out (bool isWall, bool isGoal, int minDistance) value))
                            {
                                knownLocations.Add(currentPosition, (false, false, priorMinDistance + 1));
                            }
                            else
                            {
                                var (_, _, minDistance) = value;
                                if (minDistance > priorMinDistance + 1)
                                {
                                    knownLocations[currentPosition] = (false, false, priorMinDistance + 1);
                                }
                            }
                            break;
                        case 2:
                            currentPosition = lastDirectionAttempted.GetMovement(currentPosition);
                            goalLocation = currentPosition;
                            if (!knownLocations.TryGetValue(currentPosition, out (bool isWall, bool isGoal, int minDistance) goalValue))
                            {
                                knownLocations.Add(currentPosition, (false, true, priorMinDistance + 1));
                            }
                            else
                            {
                                var (_, _, minDistance) = goalValue;
                                if (minDistance > priorMinDistance + 1)
                                {
                                    knownLocations[currentPosition] = (false, true, priorMinDistance + 1);
                                }
                            }

                            if (knownLocations[currentPosition].minDistance > priorMinDistance + 1)
                            {
                                knownLocations[currentPosition] = (false, true, priorMinDistance + 1);
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                if (goalLocation.HasValue)
                {
                    break;
                }

                lastDirectionAttempted = _directions
                    .Select(d => (direction: d, newPosition: d.GetMovement(currentPosition)))
                    .Where(d => !knownLocations.ContainsKey(d.newPosition) || !knownLocations[d.newPosition].isWall)
                    .OrderBy(d => knownLocations.ContainsKey(d.newPosition))
                    .ThenByDescending(d => d.direction == lastDirectionAttempted.GetCCW())
                    .ThenByDescending(d => d.direction == lastDirectionAttempted)
                    .ThenByDescending(d => d.direction == lastDirectionAttempted.GetCW())
                    .ThenBy(d => (int)d.direction)
                    .First().direction;

                var directionInput = lastDirectionAttempted switch
                {
                    Direction.North => 1,
                    Direction.South => 2,
                    Direction.West => 3,
                    Direction.East => 4,
                    _ => throw new NotImplementedException(),
                };

                output = IntCodeComputer.RunProgram(output.FinalProgramState, [directionInput], output.PausedAtIndex ?? 0, output.RelativeBase);
            }

            var expectedResult = 204;
            var result = knownLocations[goalLocation.Value].minDistance;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var output = IntCodeComputer.ParseAndRunProgram(lines);

            var startingPosition = (0, 0);
            var currentPosition = startingPosition;
            (int x, int y)? goalPosition = null;
            var knownLocations = new Dictionary<(int x, int y), bool>
            {
                { currentPosition, false }
            };

            var unknownLocations = _directions.Select(d => d.GetMovement(currentPosition)).ToList();

            var direction = Direction.North;

            while (true)
            {
                if (output.Outputs.Count > 0)
                {
                    switch (output.Outputs[0])
                    {
                        case 0:
                            var wallLocation = direction.GetMovement(currentPosition);
                            knownLocations.Add(wallLocation, true);
                            unknownLocations.Remove(wallLocation);
                            break;
                        case 1:
                        case 2:
                            currentPosition = direction.GetMovement(currentPosition);
                            if (output.Outputs[0] == 2)
                            {
                                goalPosition = currentPosition;
                            }

                            if (unknownLocations.Contains(currentPosition))
                            {
                                knownLocations.Add(currentPosition, false);
                                unknownLocations.Remove(currentPosition);
                                foreach (var adjacentPosition in _directions.Select(d => d.GetMovement(currentPosition)))
                                {
                                    if (!knownLocations.ContainsKey(adjacentPosition) && !unknownLocations.Contains(adjacentPosition))
                                    {
                                        unknownLocations.Add(adjacentPosition);
                                    }
                                }
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                if (unknownLocations.Count == 0)
                {
                    break;
                }

                direction = FindPathToClosestUnknownLocation(currentPosition, knownLocations, unknownLocations);

                var directionInput = direction switch
                {
                    Direction.North => 1,
                    Direction.South => 2,
                    Direction.West => 3,
                    Direction.East => 4,
                    _ => throw new NotImplementedException(),
                };

                output = IntCodeComputer.RunProgram(output.FinalProgramState, [directionInput], output.PausedAtIndex ?? 0, output.RelativeBase);
            }

            //Draw(currentPosition, goalPosition, knownLocations);
            var expectedResult = 340;
            var result = FindTimeToFill(goalPosition, knownLocations);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private Direction FindPathToClosestUnknownLocation((int, int) currentPosition, Dictionary<(int x, int y), bool> knownLocations, List<(int x, int y)> unknownLocations)
        {
            var pathsToCheck = new PriorityQueue<((int x, int y) currentPosition, Direction? initialDirection, List<(int x, int y)> path), int>();
            pathsToCheck.Enqueue((currentPosition, null, []), 0);
            while (pathsToCheck.Count > 0)
            {
                var (position, initialDirection, path) = pathsToCheck.Dequeue();
                var adjacentPositions = _directions.Select(d => (d, d.GetMovement(position))).ToList();
                foreach (var (direction, adjacentPosition) in adjacentPositions)
                {
                    if (unknownLocations.Contains(adjacentPosition)) return initialDirection ?? direction;
                    if (knownLocations[adjacentPosition]) continue;
                    if (path.Contains(adjacentPosition)) continue;

                    path = [.. path, position];
                    pathsToCheck.Enqueue((adjacentPosition, initialDirection ?? direction, path), path.Count);
                }
            }
            throw new NotImplementedException();
        }

        private static void Draw((int x, int y) currentPosition, (int x, int y)? goalPosition, Dictionary<(int x, int y), bool> knownLocations)
        {
            var positions = knownLocations.Keys;
            var minX = positions.Min(k => k.x);
            var maxX = positions.Max(k => k.x);
            var minY = positions.Min(k => k.y);
            var maxY = positions.Max(k => k.y);

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var position = (x, y);
                    if (currentPosition == position)
                    {
                        Console.Write("D");
                    }
                    else if (goalPosition == position)
                    {
                        Console.Write("G");
                    }
                    else if (!knownLocations.TryGetValue(position, out bool isWall))
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        if (isWall)
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private int FindTimeToFill((int x, int y)? startingPosition, Dictionary<(int x, int y), bool> knownLocations)
        {
            if (!startingPosition.HasValue) throw new NotImplementedException();

            var locations = knownLocations.Where(kvp => !kvp.Value).ToDictionary(kvp => kvp.Key, _ => false);
            var tick = 0;
            locations[startingPosition.Value] = true;
            var locationsToCheck = _directions.Select(d => d.GetMovement(startingPosition.Value)).Where(locations.ContainsKey).ToList();
            while (locations.Values.Any(v => !v))
            {
                var nextLocationsToCheck = new List<(int x, int y)>();
                foreach (var location in locationsToCheck)
                {
                    locations[location] = true;
                    foreach (var adjacentLocation in _directions.Select(d => d.GetMovement(location)))
                    {
                        if (nextLocationsToCheck.Contains(adjacentLocation)) continue;
                        if (!locations.ContainsKey(adjacentLocation)) continue;
                        nextLocationsToCheck.Add(adjacentLocation);
                    }
                }
                locationsToCheck = nextLocationsToCheck;
                tick++;
            }
            return tick;
        }
    }
}
