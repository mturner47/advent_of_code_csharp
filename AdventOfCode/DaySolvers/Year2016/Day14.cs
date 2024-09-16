using Helpers.Helpers;

namespace AdventOfCode.Year2016
{
    internal class Day14 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var salt = lines[0];

            var expectedResult = 23769;
            var result = Solve(salt, false);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var salt = lines[0];

            var expectedResult = 20606;
            var result = Solve(salt, true);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public static int Solve(string salt, bool useHashStretching)
        {
            var foundCount = 0;
            var currentIndex = 0;
            var knownHashes = new Dictionary<int, string>();

            while (true)
            {
                if (!knownHashes.ContainsKey(currentIndex))
                {
                    var possibleKey = salt + currentIndex.ToString();
                    var hash = GetHash(possibleKey, useHashStretching);
                    knownHashes[currentIndex] = hash;
                }
                var storedHash = knownHashes[currentIndex];

                var foundTripleChar = FindDuplicateRun(storedHash, 3);
                if (foundTripleChar.HasValue)
                {
                    for (var i = currentIndex + 1; i <= currentIndex + 1000; i++)
                    {
                        if (!knownHashes.ContainsKey(i))
                        {
                            var futurePossibleKey = salt + i.ToString();
                            var futureHash = GetHash(futurePossibleKey, useHashStretching);
                            knownHashes[i] = futureHash;
                        }
                        var futureStoredHash = knownHashes[i];
                        if (futureStoredHash.Contains(new string(Enumerable.Repeat(foundTripleChar.Value, 5).ToArray())))
                        {
                            foundCount++;
                            if (foundCount == 64)
                            {
                                return currentIndex;
                            }
                            break;
                        }
                    }
                }

                currentIndex++;
            }
        }

        private static string GetHash(string input, bool useHashStretching)
        {
            var timesToHash = useHashStretching ? 2017 : 1;
            for (var i = 0; i < timesToHash; i++)
            {
                input = MD5Helper.HashString(input).ToLower();
            }
            return input;
        }

        private static char? FindDuplicateRun(string s, int runLength, char? c = null)
        {
            for (var i = 0; i < s.Length - runLength + 1; i++)
            {
                if (c.HasValue && c.Value != s[i]) continue;
                var found = true;
                for (var j = i + 1; j < i + runLength; j++)
                {
                    if (s[i] != s[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found) return s[i];
            }
            return null;
        }
    }
}
