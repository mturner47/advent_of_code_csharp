using System.Text.RegularExpressions;

namespace EverybodyCodes.DaySolvers.Year2024
{
    internal class Day02 : IDaySolver
    {
        public object Part1(IList<string> lines)
        {
            var runicWords = lines[0].Replace("WORDS:", "").Split(",");
            var inscription = lines[2];

            var sum = 0;
            foreach (var word in runicWords)
            {
                sum += Regex.Matches(inscription, word).Count;
            }
            var expectedResult = -1;
            var result = sum;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object Part2(IList<string> lines)
        {
            var runicWords = lines[0].Replace("WORDS:", "").Split(",").ToHashSet();
            var inscription = lines.Skip(2).ToList();

            var reverseRunicWords = runicWords.Select(r => new string(r.Reverse().ToArray())).ToList();
            foreach (var rw in reverseRunicWords)
            {
                runicWords.Add(rw);
            }

            var sum = inscription.Sum(i => GetRuneSymbolCount(i, runicWords));

            var expectedResult = 5287;
            var result = sum;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object Part3(IList<string> lines)
        {
            var runicWords = lines[0].Replace("WORDS:", "").Split(",").ToHashSet();
            var inscription = lines.Skip(2).ToList();

            var takenSpots = inscription.Select(i => i.Select(c => false).ToArray()).ToArray();

            var reverseRunicWords = runicWords.Select(r => new string(r.Reverse().ToArray())).ToList();
            foreach (var rw in reverseRunicWords)
            {
                runicWords.Add(rw);
            }

            var expectedResult = -1;
            var result = 0;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static int GetRuneSymbolCount(string input, HashSet<string> words)
        {
            var takenSpots = input.Select(i => false).ToList();
            for (var i = 0; i < input.Length; i++)
            {
                foreach (var word in words)
                {
                    if (input.Substring(i).StartsWith(word))
                    {
                        for (var j = 0; j < word.Length; j++)
                        {
                            takenSpots[i + j] = true;
                        }
                    }
                }
            }
            return takenSpots.Count(t => t);
        }
    }
}

