using System.Text;

namespace AdventOfCode.Year2018
{
    internal class Day12 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var pots = GetInitialPots(lines[0].Replace("initial state: ", ""));

            var transformations = lines.Skip(2).Select(ParseTransformation).ToDictionary(t => t.input, t => t.output);

            for (var i = 0; i < 20; i++)
            {
                pots = RunGeneration(pots, transformations);
            }

            var expectedResult = 2930;
            var result = pots.Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var pots = GetInitialPots(lines[0].Replace("initial state: ", ""));

            var transformations = lines.Skip(2).Select(ParseTransformation).ToDictionary(t => t.input, t => t.output);

            var initialState = GetState(pots, 0);
            var seenStates = new List<(long t, string s, long offset)> { initialState };

            var loopSize = 0L;
            var loopOffset = 0L;
            var loopStart = 0L;
            var initialOffset = 0L;
            var loopState = "";

            var t = 0;
            while (true)
            {
                t++;
                pots = RunGeneration(pots, transformations);
                var state = GetState(pots, t);
                var seenState = seenStates.FirstOrDefault(s => s.s == state.s);
                if (seenState != default)
                {
                    loopSize = t - seenState.t;
                    loopOffset = state.offset - seenState.offset;
                    loopStart = seenState.t;
                    initialOffset = seenState.offset;
                    loopState = seenState.s;
                    break;
                }
                seenStates.Add(state);
            }

            var remainingIterations = 50_000_000_000 - loopStart;
            var finalOffset = initialOffset + (loopOffset*(remainingIterations / loopSize));
            var finalSum = 0L;
            foreach (var i in loopState.Split(",").Select(long.Parse))
            {
                finalSum += i + finalOffset;
            }

            var expectedResult = 3099999999491;
            var result = finalSum;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static ((bool, bool, bool, bool, bool) input, bool output) ParseTransformation(string line)
        {
            var parts = line.Split(" => ");
            return ((parts[0][0] == '#', parts[0][1] == '#', parts[0][2] == '#', parts[0][3] == '#', parts[0][4] == '#'), parts[1][0] == '#');
        }

        private static HashSet<long> GetInitialPots(string line)
        {
            var pots = new HashSet<long>();
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == '#') pots.Add(i);
            }
            return pots;
        }

        private static HashSet<long> RunGeneration(HashSet<long> pots, Dictionary<(bool, bool, bool, bool, bool), bool> transformations)
        {
            var newPots = new HashSet<long>();
            for (var p = pots.Min() - 2; p <= pots.Max() + 2; p++)
            {
                var input = (pots.Contains(p - 2), pots.Contains(p - 1), pots.Contains(p), pots.Contains(p + 1), pots.Contains(p + 2));
                if (transformations[input]) newPots.Add(p);
            }
            return newPots;
        }

        private static (long t, string s, long offset) GetState(HashSet<long> pots, long t)
        {
            var potList = pots.OrderBy(p => p).ToList();
            var offset = potList.Min();
            var s = string.Join(",", pots.Select(p => p - offset));

            return (t, s, offset);
        }
    }
}
