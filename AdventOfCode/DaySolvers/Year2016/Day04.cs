namespace AdventOfCode.Year2016
{
    internal class Day04 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 158835;
            var result = lines.Select(EasyParseLine).Where(IsRealRoom).Sum(r => r.sectorID);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var desiredPhrase = "NORTHPOLE OBJECT STORAGE";
            var expectedResult = 993;
            var result = lines.Select(HardParseLine).Where(r => DecodePhrase(r.phrase, r.sectorID) == desiredPhrase).First().sectorID;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (Dictionary<char, int> letters, int sectorID, string checksum) EasyParseLine(string line)
        {
            var letters = new Dictionary<char, int>();
            var parts = line.Split('[');
            var checksum = parts[1].Replace("]", "");
            var nextParts = parts[0].Split("-").Reverse();
            var sectorID = int.Parse(nextParts.First());
            var letterString = string.Join("", nextParts.Skip(1));
            foreach (var c in letterString)
            {
                if (letters.ContainsKey(c)) letters[c]++;
                else letters[c] = 1;
            }
            return (letters, sectorID, checksum);
        }

        private static (string phrase, int sectorID) HardParseLine(string line)
        {
            var parts = line.Split('[');
            var nextParts = parts[0].Split("-").Reverse();
            var sectorID = int.Parse(nextParts.First());
            var phrase = string.Join("-", nextParts.Skip(1).Select(s => s.ToUpper()).Reverse());
            return (phrase, sectorID);
        }

        private static string DecodePhrase(string phrase, int sectorID)
        {
            var newPhrase = "";
            var offset = sectorID % 26;
            foreach (var c in phrase)
            {
                if (c == '-') newPhrase += ' ';
                else
                {
                    var newC = (c + offset);
                    if (newC > 90) newC += 6;
                    newPhrase += (char)newC;
                }
            }
            return newPhrase.ToUpper();
        }

        private static bool IsRealRoom((Dictionary<char, int> letters, int sectorID, string checksum) room)
        {
            var (letters, sectorID, checksum) = room;
            var calculatedChecksum = new string(letters.OrderByDescending(kvp => kvp.Value).ThenBy(kvp => kvp.Key).Select(kvp => kvp.Key).Take(5).ToArray());
            return calculatedChecksum == checksum;
        }
    }
}
