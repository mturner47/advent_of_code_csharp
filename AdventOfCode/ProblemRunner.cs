using System.Net;
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
            if (lines.Count == 0)
            {
                GetMissingInputFile(problem, inputPath);
                lines = File.ReadAllLines(inputPath).ToList();
            }
            var daySolver = GetDaySolver(problem);

            var result = problem.Difficulty == Difficulty.Easy
                ? daySolver.EasySolution(lines)
                : daySolver.HardSolution(lines);

            return $"{problem.Year}-{problem.Day}-{problem.Difficulty}: {result}";
        }

        private static void GetMissingInputFile(Problem problem, string inputPath)
        {
            var sourcePath = $"../../../Inputs/{problem.Year}/Day{problem.Day:00}.txt";
            DownloadFileToSourceControl(problem, sourcePath);
            File.Copy(sourcePath, inputPath, true);
        }

        private static void DownloadFileToSourceControl(Problem problem, string sourcePath)
        {
            var baseAddress = new Uri("https://adventofcode.com/");
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie("session", GetSessionCookie()));

            using var handler = new HttpClientHandler { CookieContainer = cookieContainer };
            using var client = new HttpClient(handler) { BaseAddress = baseAddress };
            using var stream = client.GetStreamAsync($"{problem.Year}/day/{problem.Day}/input");
            using var fs = new FileStream(sourcePath, FileMode.OpenOrCreate);
            stream.Result.CopyTo(fs);
        }

        private static string GetSessionCookie()
        {
            var cookiePath = "cookie.txt";

            string latestSessionCookie;
            if (!File.Exists(cookiePath))
            {
                Console.WriteLine("Missing Session Cookie. Please input: ");
                latestSessionCookie = Console.ReadLine() ?? "";
                File.WriteAllText(cookiePath, latestSessionCookie);
            }
            else latestSessionCookie = File.ReadAllText(cookiePath);
            return latestSessionCookie;
        }

        public static IDaySolver GetDaySolver(Problem problem)
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly.CreateInstance($"AdventOfCode.DaySolvers.Year{problem.Year}.Day{problem.Day:00}") is not IDaySolver solver)
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
