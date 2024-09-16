using AdventOfCode.DaySolvers.Year2019;

namespace AdventOfCode.Year2019
{
    internal class Day23 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var computers = new Dictionary<long, IntCodeOutput>();
            var packets = Enumerable.Range(0, 50).ToDictionary(i => (long)i, i => new Queue<(long x, long y)>());
            packets.Add(255, []);
            for (var i = 0; i < 50; i++)
            {
                var output = IntCodeComputer.ParseAndRunProgram(lines, [i]);
                for (var j = 0; j < output.Outputs.Count - 2; j++)
                {
                    packets[output.Outputs[j]].Enqueue((output.Outputs[j + 1], output.Outputs[j + 2]));
                }
                computers.Add(i, output);
            }

            while (packets[255].Count == 0)
            {
                for (var i = 0; i < 50; i++)
                {
                    List<long> input = [-1];
                    if (packets[i].Count > 0)
                    {
                        var (x, y) = packets[i].Dequeue();
                        input = [x, y];
                    }
                    var newOutput = IntCodeComputer.RunProgram(computers[i].FinalProgramState, input, computers[i].PausedAtIndex ?? 0, computers[i].RelativeBase);
                    for (var j = 0; j < newOutput.Outputs.Count - 2; j += 3)
                    {
                        packets[newOutput.Outputs[j]].Enqueue((newOutput.Outputs[j + 1], newOutput.Outputs[j + 2]));
                    }
                    computers[i] = newOutput;
                }
            }

            var expectedResult = 22134;
            var result = packets[255].Dequeue().y;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var computers = new Dictionary<long, IntCodeOutput>();
            var packets = Enumerable.Range(0, 50).ToDictionary(i => (long)i, i => new Queue<(long x, long y)>());
            long? lastSentNatY = null;
            var natValues = (x: 0L, y: 0L);
            for (var i = 0; i < 50; i++)
            {
                var output = IntCodeComputer.ParseAndRunProgram(lines, [i]);
                for (var j = 0; j < output.Outputs.Count - 2; j++)
                {
                    packets[output.Outputs[j]].Enqueue((output.Outputs[j + 1], output.Outputs[j + 2]));
                }
                computers.Add(i, output);
            }

            while (true)
            {
                for (var i = 0; i < 50; i++)
                {
                    List<long> input = [-1];
                    if (packets[i].Count > 0)
                    {
                        var (x, y) = packets[i].Dequeue();
                        input = [x, y];
                    }
                    var newOutput = IntCodeComputer.RunProgram(computers[i].FinalProgramState, input, computers[i].PausedAtIndex ?? 0, computers[i].RelativeBase);
                    for (var j = 0; j < newOutput.Outputs.Count - 2; j += 3)
                    {
                        var destination = newOutput.Outputs[j];
                        var x = newOutput.Outputs[j + 1];
                        var y = newOutput.Outputs[j + 2];
                        if (destination == 255) natValues = (x, y);
                        else
                        {
                            packets[newOutput.Outputs[j]].Enqueue((newOutput.Outputs[j + 1], newOutput.Outputs[j + 2]));
                        }
                    }
                    computers[i] = newOutput;
                }
                if (packets.Values.All(p => p.Count == 0))
                {
                    if (lastSentNatY == natValues.y) break;
                    packets[0].Enqueue(natValues);
                    lastSentNatY = natValues.y;
                }
            }

            var expectedResult = 16084;
            var result = natValues.y;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
