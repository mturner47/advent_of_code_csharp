using AdventOfCode.DaySolvers.Year2019;
using Helpers.Helpers;

namespace AdventOfCode.Year2019
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var program = IntCodeComputer.ParseProgram(lines);

            var panels = new Dictionary<(int x, int y), long>();

            var direction = Direction.North;
            var position = (x: 0, y: 0);
            var black = 0;

            var output = IntCodeComputer.RunProgram(program);
            while (output.PausedAtIndex.HasValue)
            {
                for (var i = 0; i < output.Outputs.Count - 1; i += 2)
                {
                    var paintColor = output.Outputs[i];
                    panels[position] = paintColor;
                    direction = output.Outputs[i + 1] == 0 ? direction.GetCCW() : direction.GetCW();
                    position = direction.GetMovement(position);
                }

                var inputColor = panels.TryGetValue(position, out long value) ? value : black;
                output = IntCodeComputer.RunProgram(output.FinalProgramState, [inputColor], output.PausedAtIndex.Value);
            }

            var expectedResult = 2478;
            var result = panels.Count;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var debugMode = DebugMode.Off;
            var program = IntCodeComputer.ParseProgram(lines, debugMode: debugMode);

            var panels = new Dictionary<(int x, int y), long>();

            var direction = Direction.North;
            var position = (x: 0, y: 0);
            var black = 0;
            var white = 1;
            panels.Add(position, white);

            var output = IntCodeComputer.RunProgram(program);
            while (output.PausedAtIndex.HasValue)
            {
                for (var i = 0; i < output.Outputs.Count - 1; i += 2)
                {
                    var paintColor = output.Outputs[i];
                    panels[position] = paintColor;

                    direction = output.Outputs[i + 1] == 0 ? direction.GetCCW() : direction.GetCW();
                    position = direction.GetMovement(position);
                }

                var inputColor = panels.TryGetValue(position, out long value) ? value : black;
                output = IntCodeComputer.RunProgram(output.FinalProgramState, [inputColor], output.PausedAtIndex.Value, relativeBase: output.RelativeBase, debugMode: debugMode);
            }

            List<(int x, int y)> expectedWhitePanels = [(1, 0), (1, 1), (4, 1), (4, 0), (6, 1), (7, 0), (8, 0), (9, 1), (11, 0), (12, 0), (13, 0), (14, 1), (14, 0), (16, 1), (16, 0), (17, 0), (18, 0), (19, 1), (21, 0), (21, 1), (24, 1), (24, 0), (26, 1), (27, 0), (28, 0), (29, 1), (31, 1), (32, 0), (33, 0), (34, 1), (36, 0), (37, 0), (38, 0), (39, 0), (39, 1), (38, 2), (37, 3), (34, 3), (34, 2), (33, 3), (32, 3), (31, 2), (31, 3), (29, 3), (28, 3), (26, 3), (26, 2), (24, 3), (24, 2), (21, 2), (21, 3), (19, 2), (18, 3), (17, 3), (16, 3), (16, 2), (13, 2), (12, 3), (6, 3), (6, 2), (4, 3), (4, 2), (3, 2), (2, 2), (1, 2), (1, 3), (1, 4), (1, 5), (4, 5), (4, 4), (6, 4), (7, 5), (8, 5), (9, 4), (11, 4), (11, 5), (12, 5), (13, 5), (14, 5), (16, 5), (16, 4), (18, 4), (19, 5), (21, 4), (22, 5), (23, 5), (24, 4), (26, 4), (27, 5), (28, 5), (29, 4), (29, 5), (31, 4), (31, 5), (34, 5), (34, 4), (36, 5), (36, 4), (37, 5), (38, 5), (39, 5)];

            var pass = expectedWhitePanels.All(ewp => panels[ewp] == white) ? "Pass" : "Fail";
            //DisplayGrid(panels);

            return $"{pass}";
        }

        private static void DisplayGrid(Dictionary<(int x, int y), long> panels)
        {
            var black = 0;
            var minX = panels.Keys.Min(k => k.x);
            var maxX = panels.Keys.Max(k => k.x);
            var minY = panels.Keys.Min(k => k.y);
            var maxY = panels.Keys.Max(k => k.y);

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var colorCode = panels.TryGetValue((x, y), out long value) ? value : black;
                    Console.BackgroundColor = colorCode == black ? ConsoleColor.Black : ConsoleColor.White;

                    Console.Write(" ");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
        }
    }
}
