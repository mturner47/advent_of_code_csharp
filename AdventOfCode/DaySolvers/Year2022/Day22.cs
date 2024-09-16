using Helpers.Extensions;
using static AdventOfCode.Year2021.Day11;

namespace AdventOfCode.Year2022
{
    internal class Day22 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var fullInput = string.Join("\n", lines);
            var parts = fullInput.Split("\n\n");
            var mapLines = parts[0].Split("\n");
            var maxLengthLine = mapLines.Select(l => l.Length).Max();
            for (var i = 0; i < mapLines.Length; i++)
            {
                var line = mapLines[i];
                if (line.Length < maxLengthLine)
                {
                    mapLines[i] = line.PadRight(maxLengthLine, ' ');
                }
            }

            var instructionsLine = parts[1];

            var instructions = Instruction.ParseLine(instructionsLine).ToList();

            var currentY = 0;
            var currentX = mapLines[currentY].IndexOf('.');
            var currentDirection = 'R';

            var directionMovements = new Dictionary<char, (int, int)>
            {
                { 'R', (1, 0) },
                { 'D', (0, 1) },
                { 'L', (-1, 0) },
                { 'U', (0, -1) },
            };

            foreach (var instruction in instructions)
            {
                if (!instruction.NumSteps.HasValue)
                {
                    currentDirection = instruction.GetNewDirection(currentDirection);
                    continue;
                }
                else
                {
                    var (moveX, moveY) = directionMovements[currentDirection];
                    for (var i = 0; i < instruction.NumSteps.Value; i++)
                    {
                        var newX = currentX + moveX;
                        var newY = currentY + moveY;
                        if (newY >= 0 && newY < mapLines.Length && newX >= 0 && newX < mapLines[newY].Length)
                        {
                            var charAtNewLocation = mapLines[newY][newX];
                            if (charAtNewLocation == '#') break;
                            if (charAtNewLocation == '.')
                            {
                                currentX = newX;
                                currentY = newY;
                                continue;
                            }

                            if (currentDirection == 'L') newX = -1;
                            if (currentDirection == 'D') newY = mapLines.Length;
                            if (currentDirection == 'R') newX = mapLines[newY].Length;
                            if (currentDirection == 'U') newY = -1;
                        }

                        if (newX == -1) newX = mapLines[newY].Length;
                        else if (newY == -1) newY = mapLines.Length;
                        else if (newY == mapLines.Length) newY = -1;
                        else if (newX == mapLines[newY].Length) newX = -1;

                        var hitWall = false;

                        while (true)
                        {
                            newX += moveX;
                            newY += moveY;

                            var charAtNewLocation = mapLines[newY][newX];
                            if (charAtNewLocation == '#')
                            {
                                hitWall = true;
                                break;
                            }

                            if (charAtNewLocation == '.')
                            {
                                currentX = newX;
                                currentY = newY;
                                break;
                            }
                        }

                        if (hitWall) break;
                    }
                }
            }

            var directionValue = currentDirection switch
            {
                'R' => 0,
                'D' => 1,
                'L' => 2,
                'U' => 3,
                _ => throw new NotImplementedException(),
            };

            var finalValue = (1000 * (currentY + 1)) + (4 * (currentX + 1)) + directionValue;
            return $"X: {currentX}; Y: {currentY}; Dir: {currentDirection}; Final Value: {finalValue}";
        }

        public object HardSolution(IList<string> lines)
        {
            var faces = new Dictionary<char, List<string>>
            {
                { 'A', lines.Take(50).Select(l => string.Concat(l.Skip(50).Take(50))).ToList() },
                { 'B', lines.Take(50).Select(l => string.Concat(l.Skip(100).Take(50))).ToList() },
                { 'C', lines.Skip(50).Take(50).Select(l => string.Concat(l.Skip(50).Take(50))).ToList() },
                { 'D', lines.Skip(100).Take(50).Select(l => string.Concat(l.Skip(50).Take(50))).ToList() },
                { 'E', lines.Skip(100).Take(50).Select(l => string.Concat(l.Take(50))).ToList() },
                { 'F', lines.Skip(150).Take(50).Select(l => string.Concat(l.Take(50))).ToList() },
            };

            var instructions = Instruction.ParseLine(lines.Skip(201).First());

            var startingLocation = (faces['A'][0].IndexOf('.'), 0);
            var currentState = new State { Face = 'A', Position = startingLocation, Direction = 'R' };

            var faceTransitions = new Dictionary<(char, char), Func<State, State>>
            {
                {('A', 'R'), s => new State { Position = (0, s.Position.y), Direction = 'R', Face = 'B' } },
                {('A', 'L'), s => new State { Position = (0, 49 - s.Position.y), Direction = 'R', Face = 'E' } },
                {('A', 'U'), s => new State { Position = (0, s.Position.x), Direction = 'R', Face = 'F'} },
                {('A', 'D'), s => new State { Position = (s.Position.x, 0), Direction = 'D', Face = 'C'} },
                {('B', 'R'), s => new State { Position = (49, 49 - s.Position.y), Direction = 'L', Face = 'D'} },
                {('B', 'L'), s => new State { Position = (49, s.Position.y), Direction = 'L', Face = 'A' } },
                {('B', 'U'), s => new State { Position = (s.Position.x, 49), Direction = 'U', Face = 'F' } },
                {('B', 'D'), s => new State { Position = (49, s.Position.x), Direction = 'L', Face = 'C' } },
                {('C', 'R'), s => new State { Position = (s.Position.y, 49), Direction = 'U', Face = 'B' } },
                {('C', 'L'), s => new State { Position = (s.Position.y, 0), Direction = 'D', Face = 'E' } },
                {('C', 'U'), s => new State { Position = (s.Position.x, 49), Direction = 'U', Face = 'A' } },
                {('C', 'D'), s => new State { Position = (s.Position.x, 0), Direction = 'D', Face = 'D' } },
                {('D', 'R'), s => new State { Position = (49, 49 - s.Position.y), Direction = 'L', Face = 'B' } },
                {('D', 'L'), s => new State { Position = (49, s.Position.y), Direction = 'L', Face = 'E' } },
                {('D', 'U'), s => new State { Position = (s.Position.x, 49), Direction = 'U', Face = 'C' } },
                {('D', 'D'), s => new State { Position = (49, s.Position.x), Direction = 'L', Face = 'F' } },
                {('E', 'R'), s => new State { Position = (0, s.Position.y), Direction = 'R', Face = 'D' } },
                {('E', 'L'), s => new State { Position = (0, 49 - s.Position.y), Direction = 'R', Face = 'A' } },
                {('E', 'U'), s => new State { Position = (0, s.Position.x), Direction = 'R', Face = 'C' } },
                {('E', 'D'), s => new State { Position = (s.Position.x, 0), Direction = 'D', Face = 'F' } },
                {('F', 'R'), s => new State { Position = (s.Position.y, 49), Direction = 'U', Face = 'D' } },
                {('F', 'L'), s => new State { Position = (s.Position.y, 0), Direction = 'D', Face = 'A' } },
                {('F', 'U'), s => new State { Position = (s.Position.x, 49), Direction = 'U', Face = 'E' } },
                {('F', 'D'), s => new State { Position = (s.Position.x, 0), Direction = 'D', Face = 'B' } },
            };

            var directionMovements = new Dictionary<char, (int, int)>
            {
                { 'R', (1, 0) },
                { 'D', (0, 1) },
                { 'L', (-1, 0) },
                { 'U', (0, -1) },
            };

            foreach (var instruction in instructions)
            {
                if (!instruction.NumSteps.HasValue)
                {
                    currentState.Direction = instruction.GetNewDirection(currentState.Direction);
                    continue;
                }

                for (var i = 0; i < instruction.NumSteps.Value; i++)
                {
                    var (x, y) = currentState.Position;
                    var (moveX, moveY) = directionMovements[currentState.Direction];
                    var (newX, newY) = (x + moveX, y + moveY);
                    var nextState = newX >= 0 && newX < 50 && newY >= 0 && newY < 50
                        ? new State { Position = (newX, newY), Direction = currentState.Direction, Face = currentState.Face }
                        : faceTransitions[(currentState.Face, currentState.Direction)](currentState);
                    var nextStateFace = faces[nextState.Face];
                    var (nextStateX, nextStateY) = nextState.Position;
                    if (nextStateFace[nextStateY][nextStateX] == '#') break;
                    currentState = nextState;
                }
            }

            var (finalX, finalY) = GetFinalPosition(currentState);

            var directionValue = currentState.Direction switch
            {
                'R' => 0,
                'D' => 1,
                'L' => 2,
                'U' => 3,
                _ => throw new NotImplementedException(),
            };

            return 1000*finalY + 4*finalX + directionValue;
        }

        private static (int x, int y) GetFinalPosition(State state)
        {
            var currentX = state.Position.x + 1;
            var currentY = state.Position.y + 1;
            return state.Face switch
            {
                'A' => (50 + currentX, 0 + currentY),
                'B' => (100 + currentX, 0 + currentY),
                'C' => (50 + currentX, 50 + currentY),
                'D' => (50 + currentX, 100 + currentY),
                'E' => (0 + currentX, 100 + currentY),
                'F' => (0 + currentX, 150 + currentY),
                _ => throw new NotImplementedException(),
            };
        }

        private class Instruction
        {
            private Dir? _turnDirection;
            public int? NumSteps { get; set; }

            public static IEnumerable<Instruction> ParseLine(string line)
            {
                while (line.Length > 0)
                {
                    if (line[0] == 'R' || line[0] == 'L')
                    {
                        var dirChar = line[0];
                        line = line[1..];

                        yield return new Instruction { _turnDirection = dirChar == 'R' ? Dir.CW : Dir.CCW };
                    }
                    else
                    {
                        var numbers = string.Concat(line.TakeWhile(c => c != 'R' && c != 'L'));
                        line = line[numbers.Length..];
                        yield return new Instruction { NumSteps = int.Parse(numbers) };
                    }
                }
            }

            public char GetNewDirection(char currentDirection)
            {
                return currentDirection switch
                {
                    'R' => _turnDirection == Dir.CW ? 'D' : 'U',
                    'D' => _turnDirection == Dir.CW ? 'L' : 'R',
                    'L' => _turnDirection == Dir.CW ? 'U' : 'D',
                    'U' => _turnDirection == Dir.CW ? 'R' : 'L',
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private class State
        {
            public char Face { get; set; }
            public (int x, int y) Position { get; set; }
            public char Direction { get; set; }
        }

        private enum Dir
        {
            CW,
            CCW,
        }
    }
}
