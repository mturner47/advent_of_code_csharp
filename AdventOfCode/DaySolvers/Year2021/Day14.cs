using System.Text;

namespace AdventOfCode.Year2021
{
    internal class Day14 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var templateLine = lines[0];
            var insertionRuleDictionary = lines
                .Skip(2)
                .Select(ConvertLineToInsertionRule)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            for (var i = 0; i < 10; i++)
            {
                var sb = new StringBuilder();
                for (var j = 0; j < templateLine.Length; j++)
                {
                    var currentElement = templateLine[j];
                    sb.Append(currentElement);
                    if (j != templateLine.Length - 1)
                    {
                        var newElement = insertionRuleDictionary[(currentElement, templateLine[j + 1])];
                        sb.Append(newElement);
                    }
                }
                templateLine = sb.ToString();
            }

            var letterGroups = templateLine.GroupBy(c => c).OrderBy(c => c.Count()).ToList();

            return letterGroups.Last().Count() - letterGroups.First().Count();
        }

        public object HardSolution(IList<string> lines)
        {
            var templateLine = lines[0];
            var replacmentRuleDictionary = lines
                .Skip(2)
                .Select(ConvertLineToReplacementRule)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var amounts = replacmentRuleDictionary.Keys.ToDictionary(k => k, _ => 0d);

            for (var i = 0; i < templateLine.Length - 1; i++)
            {
                var firstLetter = templateLine[i];
                var secondLetter = templateLine[i + 1];
                amounts[(firstLetter, secondLetter)] += 1;
            }

            for (var i = 0; i < 40; i++)
            {
                var newAmounts = replacmentRuleDictionary.Keys.ToDictionary(k => k, _ => 0d);
                foreach (var key in amounts.Keys)
                {
                    if (amounts[key] == 0) continue;

                    var replacements = replacmentRuleDictionary[key];
                    foreach (var replacement in replacements)
                    {
                        newAmounts[replacement] += amounts[key];
                    }
                }
                amounts = newAmounts;
            }

            var finalLetterCounts = new Dictionary<char, double>();
            foreach (var kvp in amounts)
            {
                var (first, second) = kvp.Key;
                if (!finalLetterCounts.ContainsKey(first))
                {
                    finalLetterCounts[first] = 0;
                }
                finalLetterCounts[first] += kvp.Value;

                if (!finalLetterCounts.ContainsKey(second))
                {
                    finalLetterCounts[second] = 0;
                }
                finalLetterCounts[second] += kvp.Value;
            }

            finalLetterCounts[templateLine[0]] += 1;
            finalLetterCounts[templateLine[^1]] += 1;

            var finalLetterCountsList = finalLetterCounts.ToList().OrderBy(x => x.Value).Select(x => x.Value/2).ToList();

            return finalLetterCountsList.Last() - finalLetterCountsList.First();
        }

        private static KeyValuePair<(char, char), char> ConvertLineToInsertionRule(string line)
        {
            var lineParts = line.Split(" -> ");
            var insertionPair = (lineParts[0][0], lineParts[0][1]);
            var resultingElement = lineParts[1][0];
            return new KeyValuePair<(char, char), char>(insertionPair, resultingElement);
        }

        private static KeyValuePair<(char, char), IList<(char, char)>> ConvertLineToReplacementRule(string line)
        {
            var lineParts = line.Split(" -> ");
            var insertionPair = (lineParts[0][0], lineParts[0][1]);
            var resultingElements = new List<(char, char)>
            {
                (lineParts[0][0], lineParts[1][0]),
                (lineParts[1][0], lineParts[0][1]),
            };
            return new KeyValuePair<(char, char), IList<(char, char)>>(insertionPair, resultingElements);
        } 
    }
}
