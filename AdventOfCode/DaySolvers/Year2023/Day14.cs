using Helpers.Extensions;

namespace AdventOfCode.Year2023
{
    internal class Day14 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            return lines.ToList().RotateCCW().Sum(l => GetWeight(Roll(l)));
        }

        public object HardSolution(IList<string> lines)
        {
            var position = lines.ToList().RotateCCW();

            var numTargetCycles = 1_000_000_000;
            var seenPositions = new Dictionary<string, (int cycle, long result)>();
            var repeatsAt = -1;
            var firstRepeatIteration = -1;
            var patternLength = -1;
            for (var i = 0; i < numTargetCycles; i++)
            {
                position = Cycle(position);
                var key = string.Join("", position);
                if (!seenPositions.TryGetValue(key, out (int iteration, long result) previousIteration))
                {
                    seenPositions.Add(key, (i, position.Sum(GetWeight)));
                }
                else
                {
                    if (repeatsAt == -1)
                    {
                        repeatsAt = i;
                        firstRepeatIteration = previousIteration.iteration;
                    }
                    else if (previousIteration.iteration == firstRepeatIteration)
                    {
                        patternLength = i - repeatsAt;
                        break;
                    }
                }
            }

            var targetIteration = firstRepeatIteration + ((numTargetCycles - repeatsAt) % patternLength);
            return seenPositions.Values.First(v => v.cycle == targetIteration - 1).result;
        }

        private static List<string> Cycle(List<string> lines)
        {
            for (var i = 0; i < 4; i++)
            {
                lines = lines.Select(Roll).ToList().RotateCW().ToList();
            }
            return lines;
        }

        private static string Roll(string row)
        {
            var sArray = row.ToArray();
            for (var i = 0; i < sArray.Length; i++)
            {
                if (sArray[i] == 'O')
                {
                    var j = i - 1;
                    while (j >= 0)
                    {
                        if (sArray[j] == '.')
                        {
                            sArray[j] = 'O';
                            sArray[j + 1] = '.';
                        }
                        else
                        {
                            break;
                        }
                        j--;
                    }
                }
            }
            return new string(sArray);
        }

        private static long GetWeight(string row)
        {
            long sum = 0;
            for (var i = 0; i < row.Length; i++)
            {
                if (row[i] == 'O')
                {
                    sum += row.Length - i;
                }
            }

            return sum;
        }
    }
}
