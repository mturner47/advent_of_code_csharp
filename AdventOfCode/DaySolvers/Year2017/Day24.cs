namespace AdventOfCode.Year2017
{
    internal class Day24 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var components = lines.Select(Parse).ToList();

            var expectedResult = 1906;
            var result = GetStrongestBridgeStrength(0, components);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var components = lines.Select(Parse).ToList();
            var expectedResult = 1824;
            var result = GetLongestBridge(0, components).strength;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (int a, int b) Parse(string line)
        {
            var parts = line.Split("/");
            var a = int.Parse(parts[0]);
            var b = int.Parse(parts[1]);
            if (b < a) (b, a) = (a, b);
            return (a, b);
        }

        private static int GetStrongestBridgeStrength(int currentEnd, List<(int a, int b)> remainingComponents)
        {
            var validComponents = remainingComponents.Where(c => c.a == currentEnd || c.b == currentEnd).ToList();

            if (validComponents.Count == 0) return 0;
            var bestSum = 0;
            foreach (var validComponent in validComponents)
            {
                var newEnd = validComponent.a == currentEnd ? validComponent.b : validComponent.a;
                var newComponents = remainingComponents.Except([validComponent]).ToList();
                var sum = validComponent.a + validComponent.b + GetStrongestBridgeStrength(newEnd, newComponents);
                if (bestSum < sum) bestSum = sum;
            }
            return bestSum;
        }

        private static (int length, int strength) GetLongestBridge(int currentEnd, List<(int a, int b)> remainingComponents)
        {
            var validComponents = remainingComponents.Where(c => c.a == currentEnd || c.b == currentEnd).ToList();
            if (validComponents.Count == 0) return (0, 0);
            var best = (length:0, strength:0);

            foreach (var validComponent in validComponents)
            {
                var newEnd = validComponent.a == currentEnd ? validComponent.b : validComponent.a;
                var newComponents = remainingComponents.Except([validComponent]).ToList();
                var (length, strength) = GetLongestBridge(newEnd, newComponents);
                var fullStrength = strength + validComponent.a + validComponent.b;
                if (best.length < length || (best.length == length && best.strength < fullStrength))
                {
                    best = (length, fullStrength);
                }
            }
            return (best.length + 1, best.strength);
        }
    }
}
