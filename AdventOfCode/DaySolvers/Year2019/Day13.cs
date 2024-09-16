using AdventOfCode.DaySolvers.Year2019;

namespace AdventOfCode.Year2019
{
    internal class Day13 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var output = IntCodeComputer.ParseAndRunProgram(lines).Outputs;
            var map = new Dictionary<(long x, long y), long>();
            for (var i = 0; i < output.Count - 2; i += 3)
            {
                var x = output[i];
                var y = output[i + 1];
                var tile = output[i + 2];
                map[(x, y)] = tile;
            }
            var expectedResult = 270;
            var result = map.Values.Count(t => t == 2);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var debugMode = DebugMode.Off;
            var program = IntCodeComputer.ParseProgram(lines, debugMode:debugMode);
            program.Program[0] = 2;
            var map = new Dictionary<(long x, long y), long>();
            var output = IntCodeComputer.RunProgram(program);
            var currentScore = 0L;
            var ballPosition = (x:0L, y:0L);
            var paddlePosition = (x: 0L, y: 0L);

            while (true)
            {
                for (var i = 0; i < output.Outputs.Count - 2; i += 3)
                {
                    var x = output.Outputs[i];
                    var y = output.Outputs[i + 1];
                    var tile = output.Outputs[i + 2];

                    if (x == -1 && y == 0)
                    {
                        currentScore = tile;
                    }
                    else
                    {
                        if (tile == (long)TileType.Ball)
                        {
                            var oldBallPosition = ballPosition;
                            ballPosition = (x, y);
                            if (map.ContainsKey(oldBallPosition))
                            {
                                map[oldBallPosition] = (long)TileType.Empty;
                            }
                        }
                        else if (tile == (long)TileType.Paddle)
                        {
                            var oldPaddlePosition = paddlePosition;
                            paddlePosition = (x, y);
                            if (map.ContainsKey(oldPaddlePosition))
                            {
                                map[oldPaddlePosition] = (long)TileType.Empty;
                            }
                        }

                        map[(x, y)] = tile;
                    }
                }
                if (debugMode != DebugMode.Off) DrawMap(map);

                if (currentScore > 0 && !map.Values.Any(t => t == 2))
                {
                    break;
                }

                var joystickTilt = 0L;
                if (ballPosition.x < paddlePosition.x) joystickTilt = -1L;
                if (ballPosition.x > paddlePosition.x) joystickTilt = 1L;
                output = IntCodeComputer.RunProgram(output.FinalProgramState, [joystickTilt], output.PausedAtIndex ?? 0, output.RelativeBase, debugMode: debugMode);
            }
            var expectedResult = 12535;
            var result = currentScore;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static void DrawMap(Dictionary<(long x, long y), long> map)
        {
            var keys = map.Keys;
            var minX = keys.Min(p => p.x);
            var maxX = keys.Max(p => p.x);
            var minY = keys.Min(p => p.y);
            var maxY = keys.Max(p => p.y);
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var tile = map.ContainsKey((x, y)) ? map[(x, y)] : 0;
                    var c = tile switch
                    {
                        0 => ' ',
                        1 => y == minY ? '-' : '|',
                        2 => 'X',
                        3 => '_',
                        4 => 'o',
                        _ => throw new NotImplementedException(),
                    };
                    Console.Write(c);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private enum TileType
        {
            Empty,
            Wall,
            Block,
            Paddle,
            Ball,
        }
    }
}
