namespace AdventOfCode.Year2020
{
    internal class Day07 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var bags = lines.Select(Parse).ToDictionary(c => c.container, c => c.contents);
            var bagsToCheck = new Queue<string>();
            var containersFound = new HashSet<string>();
            bagsToCheck.Enqueue("shiny gold");
            while (bagsToCheck.Count > 0)
            {
                var bag = bagsToCheck.Dequeue();
                foreach (var container in bags.Where(b => !containersFound.Contains(b.Key) && b.Value.Any(v => v.style == bag)))
                {
                    containersFound.Add(container.Key);
                    bagsToCheck.Enqueue(container.Key);
                }
            }

            var expectedResult = 112;
            var result = containersFound.Count;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var bags = lines.Select(Parse).ToDictionary(c => c.container, c => c.contents);

            var expectedResult = 6260;
            var result = Count(bags, "shiny gold") - 1;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static readonly Dictionary<string, int> _knownBags = [];

        private static int Count(Dictionary<string, HashSet<(int count, string style)>> bags, string bag)
        {
            var childBags = bags[bag];
            if (childBags.Count == 0)
            {
                _knownBags.Add(bag, 1);
                return 1;
            }

            var total = 1;
            foreach (var (count, style) in childBags)
            {
                if (_knownBags.ContainsKey(style)) total += (count * _knownBags[style]);
                else total += (count * Count(bags, style));
            }
            _knownBags.Add(bag, total);
            return total;
        }

        public static (string container, HashSet<(int count, string style)> contents) Parse(string line)
        {
            var parts = line.Split(" bags contain ");
            var container = parts[0];
            var contents = new HashSet<(int count, string style)>();
            var contentParts = parts[1].Replace(".", "").Split(", ");
            foreach (var contentPart in contentParts)
            {
                if (contentPart == "no other bags") continue;
                var bagParts = contentPart.Replace(" bags", "").Replace(" bag", "").Split(" ");
                var count = int.Parse(bagParts[0]);
                var style = string.Join(" ", bagParts.Skip(1));
                contents.Add((count, style));
            }
            return (container, contents);
        }
    }
}
