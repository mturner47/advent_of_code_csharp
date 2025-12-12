using System.Reflection;

namespace EverybodyCodes
{
    internal record Problem(int Year, int Day, Part Part);

    internal static class ProblemRunner
    {
        public static string GetSolution(Problem problem)
        {
            var inputPath = $"Inputs/{problem.Year}/{problem.Day:00}-{PartToString(problem.Part)}.txt";

            var lines = File.ReadAllLines(inputPath).ToList();
            if (lines.Count == 0)
            {
                Console.WriteLine($"Missing input for problem {problem.Year} - {problem.Day} - Part {PartToString(problem.Part)}");
                return "";
            }

            var daySolver = GetDaySolver(problem);

            var result = problem.Part switch
            {
                Part.One => daySolver.Part1(lines),
                Part.Two => daySolver.Part2(lines),
                Part.Three => daySolver.Part3(lines),
                _ => throw new NotImplementedException(),
            };
            return $"{problem.Year}-{problem.Day}-{PartToString(problem.Part)}: {result}";
        }

        public static IDaySolver GetDaySolver(Problem problem)
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly.CreateInstance($"EverybodyCodes.DaySolvers.Year{problem.Year}.Day{problem.Day:00}") is not IDaySolver solver)
            {
                throw new NotImplementedException();
            }
            return solver;
        }

        public static string PartToString(Part part)
        {
            return part switch
            {
                Part.One => "01",
                Part.Two => "02",
                Part.Three => "03",
                _ => throw new NotImplementedException(),
            };
        }
    }


    internal enum Part
    {
        One,
        Two,
        Three,
    }
}
