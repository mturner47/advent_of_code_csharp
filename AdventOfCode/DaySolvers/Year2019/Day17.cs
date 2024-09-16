using AdventOfCode.DaySolvers.Year2019;
using Helpers.Helpers;

namespace AdventOfCode.Year2019
{
    internal class Day17 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var output = IntCodeComputer.ParseAndRunProgram(lines);
            var grid = new List<List<char>>();
            var row = new List<char>();
            foreach (var outputItem in output.Outputs)
            {
                if (outputItem == 10)
                {
                    grid.Add(row);
                    row = [];
                }
                else
                {
                    row.Add((char)outputItem);
                }
            }

            grid.RemoveAt(grid.Count - 1);

            var sum = 0;
            var validScaffoldCharacters = "<^>v#";
            var directions = DirectionExtensions.EnumerateDirections();
            var maxY = grid.Count - 1;
            var maxX = grid[0].Count - 1;
            for (var y = 0; y < grid.Count; y++)
            {
                for (var x = 0; x < grid[y].Count; x++)
                {
                    if (validScaffoldCharacters.Contains(grid[y][x]))
                    {
                        var isScaffold = true;
                        foreach (var (adjX, adjY) in directions.Select(d => d.GetMovement((x, y))))
                        {
                            if (adjX < 0 || adjX > maxX || adjY < 0 || adjY > maxY)
                            {
                                isScaffold = false;
                                break;
                            }

                            if (!validScaffoldCharacters.Contains(grid[adjY][adjX]))
                            {
                                isScaffold = false;
                                break;
                            }
                        }

                        if (isScaffold) sum += x * y;
                    }
                }
            }
            var expectedResult = 4408;
            var result = sum;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var programInput = IntCodeComputer.ParseProgram(lines);
            programInput.Program[0] = 2;
            var output = IntCodeComputer.RunProgram(programInput);

            var functionString = "A,B,B,A,C,A,A,C,B,C\nR,8,L,12,R,8\nR,12,L,8,R,10\nR,8,L,8,L,8,R,8,R,10\nn\n";
            var input = functionString.Select(c => (long)c);
            output = IntCodeComputer.RunProgram(output.FinalProgramState, input, output.PausedAtIndex ?? 0, output.RelativeBase);

            var expectedResult = 862452;
            var result = output.Outputs.Last();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static void Draw(List<List<char>> grid)
        {
            foreach (var line in grid)
            {
                Console.WriteLine(new string(line.ToArray()));
            }
        }
    }
}
