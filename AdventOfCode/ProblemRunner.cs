using System.Reflection;

namespace AdventOfCode
{
    internal record Problem (int Year, int Day, Difficulty Difficulty);
    internal static class ProblemRunner
    {
        public static string GetSolution(Problem problem)
        {
            var inputPath = $"Inputs/{problem.Year}/Day{problem.Day:00}.txt";
            var lines = File.ReadAllLines(inputPath).ToList();
            var daySolver = GetDaySolver(problem);

            var result = problem.Difficulty == Difficulty.Easy
                ? daySolver.EasySolution(lines)
                : daySolver.HardSolution(lines);

            return $"{problem.Year}-{problem.Day}-{problem.Difficulty}: {result}";
        }

        public static IDaySolver GetDaySolver(Problem problem)
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly.CreateInstance($"AdventOfCode.Year{problem.Year}.Day{problem.Day:00}") is not IDaySolver solver)
            {
                throw new NotImplementedException();
            }
            return solver;
        }
    }

    internal enum Difficulty
    {
        Easy,
        Hard,
    }
}
