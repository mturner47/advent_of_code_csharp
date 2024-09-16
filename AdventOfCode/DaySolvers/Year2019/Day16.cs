namespace AdventOfCode.Year2019
{
    internal class Day16 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var inputSignal = lines[0].Select(c => int.Parse(c.ToString())).ToList();
            var pattern = new List<int> { 0, 1, 0, -1 };

            var numTimes = 100;
            var history = new List<List<int>> { inputSignal };

            var patterns = new List<List<int>>();
            for (var i = 0; i < inputSignal.Count; i++)
            {
                patterns.Add(MakePattern(pattern, i, inputSignal.Count));
            }

            for (var time = 0; time < numTimes; time++)
            {
                var newSignal = new List<int>();
                for (var i = 0; i < inputSignal.Count; i++)
                {
                    var innerPattern = patterns[i];
                    var sum = 0;
                    for (var j = 0; j < inputSignal.Count; j++)
                    {
                        var patternValue = innerPattern[j];
                        var jValue = inputSignal[j];
                        sum += jValue * patternValue;
                    }
                    var finalValue = Math.Abs(sum) % 10;
                    newSignal.Add(finalValue);
                }
                history.Add(newSignal);
                inputSignal = newSignal;
            }

            var expectedResult = "89576828";
            var result = string.Join("", inputSignal.Take(8));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var baseSignal = lines[0].Select(c => int.Parse(c.ToString())).ToList();
            var offset = int.Parse(lines[0][..7]);
            var inputSignal = Enumerable.Repeat(baseSignal, 10000).SelectMany(l => l).Skip(offset).ToList();

            var numTimes = 100;

            for (var time = 0; time < numTimes; time++)
            {
                var newSignal = new List<int>();
                var priorValue = 0;
                for (var i = inputSignal.Count - 1; i >= 0; i--)
                {
                    priorValue += inputSignal[i];
                    var finalValue = Math.Abs(priorValue) % 10;
                    newSignal.Add(finalValue);
                }
                inputSignal = newSignal.AsEnumerable().Reverse().ToList();
            }

            var expectedResult = "23752579";
            var result = string.Join("", inputSignal.Take(8));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<int> MakePattern(List<int> originalPattern, int index, int length)
        {
            var newPattern = new List<int>();
            while (newPattern.Count < length + 1)
            {
                foreach (var patternValue in originalPattern)
                {
                    for (var i = 0; i < index + 1; i++)
                    {
                        newPattern.Add(patternValue);
                    }
                }
            }
            return newPattern.Skip(1).Take(length).ToList();
        }
    }
}
