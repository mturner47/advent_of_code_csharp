namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var devices = lines.Select(Parse).ToDictionary(d => d.input, d => d.outputs);
            var knownCounts = new Dictionary<string, double>();

            var expectedResult = 506;
            var result = FindNumPaths(devices, "you", "out", [], knownCounts);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var devices = lines.Select(Parse).ToDictionary(d => d.input, d => d.outputs);

            Dictionary<string, double> knownCounts;
            var avoid = new List<string> { "out", "fft", "dac" };

            knownCounts = [];
            var svrToDac = FindNumPaths(devices, "svr", "dac", avoid, knownCounts);
            knownCounts = [];
            var dacToFft = FindNumPaths(devices, "dac", "fft", avoid, knownCounts);
            knownCounts = [];
            var fftToOut = FindNumPaths(devices, "fft", "out", avoid, knownCounts);

            knownCounts = [];
            var svrToFft = FindNumPaths(devices, "svr", "fft", avoid, knownCounts);
            knownCounts = [];
            var FftToDac = FindNumPaths(devices, "fft", "dac", avoid, knownCounts);
            knownCounts = [];
            var DacToOut = FindNumPaths(devices, "dac", "out", avoid, knownCounts);

            var expectedResult = 385912350172800d;
            var result = (svrToDac*dacToFft*fftToOut) + (svrToFft*FftToDac*DacToOut);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private (string input, List<string> outputs) Parse(string line)
        {
            var parts = line.Split(": ");
            var input = parts[0];
            var output = parts[1].Split(" ").ToList();
            return (input, output);
        }

        private static double FindNumPaths(Dictionary<string, List<string>> devices, string input, string target, List<string> avoid, Dictionary<string, double> knownCounts)
        {
            var totalCount = 0d;
            foreach (var output in devices[input])
            {
                if (output == target)
                {
                    totalCount = 1;
                    break;
                }

                if (avoid.Contains(output)) continue;

                if (knownCounts.TryGetValue(output, out double knownCount))
                {
                    totalCount += knownCount;
                }
                else
                {
                    totalCount += FindNumPaths(devices, output, target, avoid, knownCounts);
                }
            }

            knownCounts[input] = totalCount;
            return totalCount;
        }
    }
}
