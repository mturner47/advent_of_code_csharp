
namespace AdventOfCode.Year2017
{
    internal class Day07 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var programs = lines.Select(Parse).ToList();
            var (name, _, _) = programs.Where(p => p.heldPrograms.Count > 0).First(p => programs.All(p1 => !p1.heldPrograms.Contains(p.name)));
            var expectedResult = "rqwgj";
            var result = name;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var programs = lines.Select(Parse).ToList();
            var (baseProgramName, _, _) = programs.Where(p => p.heldPrograms.Count > 0).First(p => programs.All(p1 => !p1.heldPrograms.Contains(p.name)));
            var programDict = programs.ToDictionary(p => p.name, p => p);
            var expectedResult = (int?)0;
            var result = FindGoodWeight(programDict, baseProgramName).correctedWeight;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (string name, int weight, List<string> heldPrograms) Parse(string line)
        {
            var heldPrograms = new List<string>();
            if (line.Contains("->"))
            {
                var arrowParts = line.Split(" -> ");
                line = arrowParts[0];
                heldPrograms = arrowParts[1].Split(", ").ToList();
            }

            var parts = line.Split(" (");
            var name = parts[0];
            var weight = int.Parse(parts[1].Replace(")", ""));
            return (name, weight, heldPrograms);
        }

        private static (int weight, int childWeights, int? correctedWeight) FindGoodWeight(Dictionary<string, (string name, int weight, List<string> heldPrograms)> programDict, string name)
        {
            var currentProgram = programDict[name];
            if (currentProgram.heldPrograms.Count == 0) return (currentProgram.weight, 0, null);

            var subProgramSums = currentProgram.heldPrograms.Select(p => FindGoodWeight(programDict, p)).ToList();
            var correctedWeight = subProgramSums.Select(s => s.correctedWeight).FirstOrDefault(cw => cw.HasValue);
            if (correctedWeight.HasValue) return (0, 0, correctedWeight);

            if (subProgramSums.Select(s => s.weight + s.childWeights).Distinct().Count() == 1)
            {
                return (currentProgram.weight, subProgramSums.Sum(s => (s.weight + s.childWeights)), null);
            }
            else
            {
                var groups = subProgramSums.GroupBy(s => s.weight + s.childWeights);
                var badWeight = groups.Where(g => g.Count() == 1).First().First();
                var goodWeight = groups.Where(g => g.Count() > 1).First().Key;
                return (0, 0, badWeight.weight + (goodWeight - (badWeight.weight + badWeight.childWeights)));
            }
        }
    }
}
