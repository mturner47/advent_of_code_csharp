namespace AdventOfCode.Year2018
{
    internal class Day02 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var pairCount = 0;
            var tripleCount = 0;
            foreach (var line in lines)
            {
                var groups = line.GroupBy(c => c);
                if (groups.Any(g => g.Count() == 2)) pairCount++;
                if (groups.Any(g => g.Count() == 3)) tripleCount++;
            }
            var expectedResult = 8820;
            var result = pairCount*tripleCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = "";
            var result = SolveHard(lines);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static string SolveHard(IList<string> lines)
        {
            for (var i = 0; i < lines.Count - 1; i++)
            {
                var line1 = lines[i];
                for (var j = i; j < lines.Count; j++)
                {
                    var line2 = lines[j];
                    var diffCount = 0;
                    var sharedLine = "";
                    for (var c = 0; c < line1.Length; c++)
                    {
                        if (line1[c] != line2[c]) diffCount++;
                        else sharedLine += line1[c];
                    }

                    if (diffCount == 1)
                    {
                        return sharedLine;
                    }
                }
            }
            throw new NotImplementedException();
        }
    }
}
