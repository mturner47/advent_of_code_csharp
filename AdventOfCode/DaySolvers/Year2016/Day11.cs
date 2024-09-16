using System.Runtime;
using System.Text;

namespace AdventOfCode.Year2016
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var initialFloors = GetInitialEasyFloors();

            var expectedResult = 33;
            var result = Solve(initialFloors);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var initialFloors = GetInitialHardFloors();

            var expectedResult = 57;
            var result = Solve(initialFloors);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int Solve(List<Floor> initialFloors)
        {
            var targetCount = initialFloors.Sum(f => f.Generators.Length);

            var visitedStates = new List<string>();
            var queue = new PriorityQueue<(List<Floor> floors, int elevatorFloor, int stepsTaken), int>();
            visitedStates.Add(GetRepresentativeState(initialFloors, 0));
            queue.Enqueue((initialFloors, 0, 0), 0);

            while (true)
            {
                var (floors, elevatorFloor, stepsTaken) = queue.Dequeue();
                if (MetGoal(floors, targetCount))
                {
                    return stepsTaken;
                }

                if (IsIllegalState(floors)) continue;

                var currentFloor = floors[elevatorFloor];
                var newElevatorFloors = new List<int> { elevatorFloor - 1, elevatorFloor + 1 };
                foreach (var newElevatorFloor in newElevatorFloors)
                {
                    if (newElevatorFloor < 0 || newElevatorFloor > 3) continue;
                    var possibleGeneratorMoves = GetPossibleMoves(currentFloor.Generators);
                    foreach (var possibleMove in possibleGeneratorMoves)
                    {
                        var nextFloors = CloneFloors(floors);
                        var nextGenerators = nextFloors[elevatorFloor].Generators;
                        foreach (var c in possibleMove) nextGenerators = nextGenerators.Replace(c.ToString(), "");
                        nextFloors[elevatorFloor].Generators = nextGenerators;
                        var targetFloorNextGenerators = nextFloors[newElevatorFloor].Generators;
                        foreach (var c in possibleMove) targetFloorNextGenerators += c;
                        nextFloors[newElevatorFloor].Generators = targetFloorNextGenerators;
                        if (!IsIllegalState(nextFloors))
                        {
                            var hash = GetRepresentativeState(nextFloors, newElevatorFloor);
                            if (!visitedStates.Contains(hash))
                            {
                                visitedStates.Add(hash);
                                queue.Enqueue((nextFloors, newElevatorFloor, stepsTaken + 1), stepsTaken + 1);
                            }
                        }
                    }

                    var possibleMicrochipMoves = GetPossibleMoves(currentFloor.Microchips);
                    foreach (var possibleMove in possibleMicrochipMoves)
                    {
                        var nextFloors = CloneFloors(floors);
                        var nextMicrochips = nextFloors[elevatorFloor].Microchips;
                        foreach (var c in possibleMove) nextMicrochips = nextMicrochips.Replace(c.ToString(), "");
                        nextFloors[elevatorFloor].Microchips = nextMicrochips;
                        var targetFloorNextMicrochips = nextFloors[newElevatorFloor].Microchips;
                        foreach (var c in possibleMove) targetFloorNextMicrochips += c;
                        nextFloors[newElevatorFloor].Microchips = targetFloorNextMicrochips;
                        if (!IsIllegalState(nextFloors))
                        {
                            var hash = GetRepresentativeState(nextFloors, newElevatorFloor);
                            if (!visitedStates.Contains(hash))
                            {
                                visitedStates.Add(hash);
                                queue.Enqueue((nextFloors, newElevatorFloor, stepsTaken + 1), stepsTaken + 1);
                            }
                        }
                    }

                    var pairs = currentFloor.Microchips.Intersect(currentFloor.Generators).ToList();
                    if (pairs.Count != 0)
                    {
                        var pairToMove = pairs.First().ToString();
                        var nextFloors = CloneFloors(floors);
                        nextFloors[elevatorFloor].Microchips = nextFloors[elevatorFloor].Microchips.Replace(pairToMove, "");
                        nextFloors[newElevatorFloor].Microchips += pairToMove;
                        nextFloors[elevatorFloor].Generators = nextFloors[elevatorFloor].Generators.Replace(pairToMove, "");
                        nextFloors[newElevatorFloor].Generators += pairToMove;

                        if (!IsIllegalState(nextFloors))
                        {
                            var hash = GetRepresentativeState(nextFloors, newElevatorFloor);
                            if (!visitedStates.Contains(hash))
                            {
                                visitedStates.Add(hash);
                                queue.Enqueue((nextFloors, newElevatorFloor, stepsTaken + 1), stepsTaken + 1);
                            }
                        }
                    }
                }
            }
        }

        private static List<Floor> CloneFloors(List<Floor> floors)
        {
            return floors.Select(f => new Floor { Generators = f.Generators, Microchips = f.Microchips }).ToList();
        }

        private static IEnumerable<List<char>> GetPossibleMoves(string s)
        {
            for(var i = 0; i < s.Length; i++)
            {
                yield return [s[i]];
                for (var j = i + 1; j < s.Length; j++)
                {
                    yield return [s[i], s[j]];
                }
            }
        }

        private static bool MetGoal(List<Floor> floors, int targetCount)
        {
            return floors[3].Generators.Length == targetCount && floors[3].Microchips.Length == targetCount;
        }

        private static string GetRepresentativeState(List<Floor> floors, int elevatorFloor)
        {
            var sb = new StringBuilder();
            sb.Append(elevatorFloor);

            foreach (var floor in floors)
            {
                var microchipCount = floor.Microchips.Length;
                var generatorCount = floor.Generators.Length;
                var pairCount = Math.Min(microchipCount, generatorCount);
                microchipCount -= pairCount;
                generatorCount -= pairCount;
                sb.Append($"{pairCount}{microchipCount}{generatorCount}");
            }
            return sb.ToString();
        }

        private static bool IsIllegalState(List<Floor> floors)
        {
            return floors.Any(f =>
            {
                return f.Microchips.Any(m => !f.Generators.Contains(m) && f.Generators.Length > 0);
            });
        }

        private List<Floor> GetInitialEasyFloors()
        {
            return
            [
                new Floor { Generators = "1", Microchips = "1" },
                new Floor { Generators = "2345", Microchips = "" },
                new Floor { Generators = "", Microchips = "2345" },
                new Floor { Generators = "", Microchips = "" },
            ];
        }

        private List<Floor> GetInitialHardFloors()
        {
            return
            [
                new Floor { Generators = "167", Microchips = "167" },
                new Floor { Generators = "2345", Microchips = "" },
                new Floor { Generators = "", Microchips = "2345" },
                new Floor { Generators = "", Microchips = "" },
            ];
        }

        private class Floor
        {
            public string Generators { get; set; } = "";
            public string Microchips { get; set; } = "";
        }
    }
}
