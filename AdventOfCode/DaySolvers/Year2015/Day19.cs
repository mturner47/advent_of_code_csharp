namespace AdventOfCode.Year2015
{
    internal class Day19 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var medicineMolecule = lines.Last();
            var conversions = lines.Take(lines.Count - 2).Select(l => l.Split(" => "))
                .GroupBy(s => s[0], s => s[1])
                .ToDictionary(g => g.Key, g => g.ToList());
            var newMolecules = GetNewMolecules(medicineMolecule, conversions);

            var expectedResult = 535;
            var result = newMolecules.Count;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var goalMolecule = lines.Last();
            var conversions = lines.Take(lines.Count - 2).Select(l => l.Split(" => "))
                .GroupBy(s => s[0], s => s[1])
                .ToDictionary(g => g.Key, g => g.ToList());

            var commaCount = goalMolecule.Count(c => c == 'Y');
            var parenthesesCount = goalMolecule.Replace("Rn", "(").Replace("Ar", "(").Count(c => c == '(');
            var totalElementCount = goalMolecule.Count(c => char.ToUpper(c) == c);


            var expectedResult = 212;
            var result = totalElementCount - parenthesesCount - (2 * commaCount) - 1;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public static List<string> GetNewMolecules(string molecule, Dictionary<string, List<string>> conversions)
        {
            var result = new List<string>();
            for (var i = 0; i < molecule.Length; i++)
            {
                var before = i > 0 ? molecule[..i] : "";
                if (conversions.TryGetValue(molecule[i].ToString(), out List<string>? oneLetterConversions))
                {
                    var after = (i + 1 < molecule.Length) ? molecule[(i + 1)..] : "";
                    foreach (var conversion in oneLetterConversions)
                    {
                        result.Add(before + conversion + after);
                    }
                }

                if (i + 1 < molecule.Length - 1)
                {
                    if (conversions.TryGetValue(molecule.Substring(i, 2), out List<string>? twoLetterConversions))
                    {
                        var after = (i + 2 < molecule.Length) ? molecule[(i + 2)..] : "";
                        foreach (var conversion in twoLetterConversions)
                        {
                            result.Add(before + conversion + after);
                        }
                    }
                }
            }
            return result.Distinct().ToList();
        }
    }
}
