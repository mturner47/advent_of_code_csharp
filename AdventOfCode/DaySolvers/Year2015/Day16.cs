using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
    internal partial class Day16 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var sues = lines.Select(ParseSue).ToDictionary(s => s.sueNumber, s => s.items);

            var itemRequirements = new Dictionary<string, int>
            {
                ["children"] = 3,
                ["cats"] = 7,
                ["samoyeds"] = 2,
                ["pomeranians"] = 3,
                ["akitas"] = 0,
                ["vizslas"] = 0,
                ["goldfish"] = 5,
                ["trees"] = 3,
                ["cars"] = 2,
                ["perfumes"] = 1,
            };

            var sendingSue = 0;
            foreach (var sue in sues)
            {
                if (sue.Value.All(kvp => kvp.Value == itemRequirements[kvp.Key]))
                {
                    sendingSue = sue.Key;
                    break;
                }
            }

            var expectedResult = 40;
            var result = sendingSue;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var sues = lines.Select(ParseSue).ToDictionary(s => s.sueNumber, s => s.items);

            var itemRequirements = new Dictionary<string, int>
            {
                ["children"] = 3,
                ["cats"] = 7,
                ["samoyeds"] = 2,
                ["pomeranians"] = 3,
                ["akitas"] = 0,
                ["vizslas"] = 0,
                ["goldfish"] = 5,
                ["trees"] = 3,
                ["cars"] = 2,
                ["perfumes"] = 1,
            };

            var sendingSue = 0;
            foreach (var sue in sues)
            {
                var allFit = sue.Value.All(kvp =>
                {
                    var amountRequired = itemRequirements[kvp.Key];
                    if (kvp.Key == "cats" || kvp.Key == "trees")
                    {
                        return kvp.Value > amountRequired;
                    }

                    if (kvp.Key == "pomeranians" || kvp.Key == "goldfish")
                    {
                        return kvp.Value < amountRequired;
                    }
                    return kvp.Value == amountRequired;
                });

                if (allFit)
                {
                    sendingSue = sue.Key;
                    break;
                }
            }
            var expectedResult = 241;
            var result = sendingSue;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (int sueNumber, Dictionary<string, int> items) ParseSue(string line)
        {
            var regex = ParseRegex();
            var groups = regex.Matches(line)[0].Groups;
            var sueNumber = int.Parse(groups[1].Value);
            var items = new Dictionary<string, int>
            {
                [groups[2].Value] = int.Parse(groups[3].Value),
                [groups[4].Value] = int.Parse(groups[5].Value),
                [groups[6].Value] = int.Parse(groups[7].Value),
            };
            return (sueNumber, items);
        }

        [GeneratedRegex(@"^Sue (\d+): ([^:]+): (\d+), ([^:]+): (\d+), ([^:]+): (\d+)")]
        private static partial Regex ParseRegex();
    }
}
