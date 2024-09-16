namespace AdventOfCode.Year2022
{
    internal class Day03 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            return lines.Select(GetCommonCharacter).Select(ConvertLetterToNumberValue).Sum();
        }

        public object HardSolution(IList<string> lines)
        {
            var sharedCharacters = new List<char>();
            for (var i = 0; i < lines.Count; i+= 3)
            {
                sharedCharacters.Add(lines[i].Intersect(lines[i + 1]).Intersect(lines[i + 2]).First());
            }
            return sharedCharacters.Select(ConvertLetterToNumberValue).Sum();
        }

        private static char GetCommonCharacter(string line)
        {
            var lineLength = line.Length;
            var firstHalf = line.Take(lineLength / 2);
            var secondHalf = line.Skip(lineLength / 2);
            var shared = firstHalf.Intersect(secondHalf);
            return shared.First();
        }

        private static int ConvertLetterToNumberValue(char letter)
        {
            var val = (int)letter;
            return val <= 91
                ? val - 38
                : val - 96;
        }
    }
}
