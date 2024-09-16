using AdventOfCode.DaySolvers.Year2019;
using Helpers.Helpers;

namespace AdventOfCode.Year2019
{
    internal class Day25 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var inputs = new List<string>
            {
                "south",
                "east",
                "take space heater",
                "west",
                "south",
                "south",
                "east",
                "east",
                "take planetoid",
                "west",
                "west",
                "north",
                "north",
                "north",
                "east",
                "take spool of cat6",
                "north",
                "north",
                "take hypercube",
                "south",
                "south",
                "west",
                "north",
                "take festive hat",
                "west",
                "take dark matter",
                "north",
                "east",
                "take semiconductor",
                "east",
                "take sand",
                "north",
            }.SelectMany(ConvertStringToInstructions).ToList();
            var itemList = new List<string>
            {
                "space heater",
                "semiconductor",
                "planetoid",
                "hypercube",
                "spool of cat6",
                "sand",
                "festive hat",
                "dark matter",
            };

            var dropEverything = itemList.Select(i => "drop " + i).SelectMany(ConvertStringToInstructions);

            var takeItemCommands = itemList.Select(i => (item:i, inputList: ConvertStringToInstructions("take " + i))).ToList();
            var invCommand = ConvertStringToInstructions("inv");
            var westCommand = ConvertStringToInstructions("west");

            inputs = [.. inputs, .. dropEverything];

            var output = IntCodeComputer.ParseAndRunProgram(lines, inputs);
            var commands = new List<string>();

            var combinations = MathHelpers.GetCombinations(takeItemCommands);

            var i = 0;

            while (!output.HaltedExecution)
            {
                var fullOutput = ConvertOutputToString(output.Outputs);
                //var outputAsString = fullOutput.Split("\n").ToList();
                //foreach (var outString in outputAsString)
                //{
                //    if (!outString.Contains("Command")
                //        && !outString.Contains("You take")
                //        && !outString.Contains("You drop"))
                //    {
                //        Console.WriteLine(outString);
                //    }
                //}

                if (fullOutput.Contains("Pressure-Sensitive Floor"))
                {
                    var priorCombinationList = string.Join(", ", combinations[i - 1].Select(c => c.item).OrderBy(i => i));
                    if (fullOutput.Contains("heavier than")) Console.WriteLine("Too light: " + priorCombinationList);
                    else if (fullOutput.Contains("lighter than")) Console.WriteLine("Too heavy: " + priorCombinationList);
                    else Console.WriteLine(fullOutput);
                }

                //var inputString = Console.ReadLine() ?? "";
                //commands.Add(inputString);
                //var input = ConvertStringToInstructions(inputString);
                var combination = combinations[i];
                i++;
                if (i >= combinations.Count) break;

                var takeCommands = combination.SelectMany(l => l.inputList).ToList();

                var input = dropEverything.Concat(takeCommands).Concat(invCommand).Concat(westCommand).ToList();
                output = IntCodeComputer.RunProgram(output.FinalProgramState, input, output.PausedAtIndex ?? 0, output.RelativeBase);
            }

            foreach (var o in output.Outputs)
            {
                Console.Write((char)o);
            }

            foreach (var command in commands)
            {
                Console.WriteLine(command);
            }
            var expectedResult = -1;
            var result = 0;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = -1;
            var result = 0;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<long> ConvertStringToInstructions(string inputString)
        {
            return (inputString + '\n').Select(c => (long)c).ToList();
        }

        private static string ConvertOutputToString(List<long> outputList)
        {
            return new string(outputList.Select(i => (char)i).ToArray());
        }
    }
}
