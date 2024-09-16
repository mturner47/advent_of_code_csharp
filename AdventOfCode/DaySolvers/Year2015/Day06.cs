using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
    internal partial class Day06 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var instructions = lines.Select(ParseLine).ToList();

            var countOn = 0;
            for (var x = 0; x < 1000; x++)
            {
                for (var y = 0; y < 1000; y++)
                {
                    var isOn = false;
                    foreach (var instruction in instructions)
                    {
                        if (x >= instruction.Start.x && x <= instruction.End.x && y >= instruction.Start.y && y <= instruction.End.y)
                        {
                            isOn = instruction.Type switch
                            {
                                InstructionType.On => true,
                                InstructionType.Off => false,
                                InstructionType.Toggle => !isOn,
                                _ => throw new NotImplementedException(),
                            };
                        }
                    }
                    if (isOn) countOn++;
                }
            }

            var expectedResult = 569999;
            var result = countOn;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var instructions = lines.Select(ParseLine).ToList();

            var totalBrightness = 0;
            for (var x = 0; x < 1000; x++)
            {
                for (var y = 0; y < 1000; y++)
                {
                    var brightness = 0;
                    foreach (var instruction in instructions)
                    {
                        if (x >= instruction.Start.x && x <= instruction.End.x && y >= instruction.Start.y && y <= instruction.End.y)
                        {
                            brightness = instruction.Type switch
                            {
                                InstructionType.On => brightness + 1,
                                InstructionType.Off => brightness == 0 ? 0 : brightness - 1,
                                InstructionType.Toggle => brightness + 2,
                                _ => throw new NotImplementedException(),
                            };
                        }
                    }
                    totalBrightness += brightness;
                }
            }
            var expectedResult = 17836115;
            var result = totalBrightness;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static Instruction ParseLine(string line)
        {
            var regex = ParseRegex();
            var groups = regex.Matches(line)[0].Groups;
            var type = groups[1].Value switch
            {
                "toggle" => InstructionType.Toggle,
                "turn on" => InstructionType.On,
                "turn off" => InstructionType.Off,
                _ => throw new NotImplementedException(),
            };
            var start = (int.Parse(groups[2].Value), int.Parse(groups[3].Value));
            var end = (int.Parse(groups[4].Value), int.Parse(groups[5].Value));

            return new Instruction
            {
                Type = type,
                Start = start,
                End = end,
            };
        }

        private class Instruction
        {
            public InstructionType Type;
            public (int x, int y) Start;
            public (int x, int y) End;
        }

        private enum InstructionType
        {
            On,
            Off,
            Toggle,
        }

        [GeneratedRegex(@"(toggle|turn on|turn off) (\d+),(\d+) through (\d+),(\d+)")]
        private static partial Regex ParseRegex();
    }
}
