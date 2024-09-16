using System;
using System.Text;

namespace AdventOfCode.Year2017
{
    internal class Day16 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var danceMoves = lines[0].Split(',').ToList();
            var programs = "abcdefghijklmnop";
            programs = RunDance(programs, danceMoves);

            var expectedResult = "namdgkbhifpceloj";
            var result = programs;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var danceMoves = lines[0].Split(',').ToList();
            var programs = "abcdefghijklmnop";
            var seenPrograms = new List<string>();

            while (!seenPrograms.Contains(programs))
            {
                seenPrograms.Add(programs);
                programs = RunDance(programs, danceMoves);
            }

            var cycleSize = seenPrograms.Count;

            var expectedResult = "ibmchklnofjpdeag";
            var result = seenPrograms[1_000_000_000%cycleSize];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static string RunDance(string programs, List<string> moves)
        {
            foreach (var move in moves)
            {
                if (move.StartsWith('s'))
                {
                    programs = Spin(programs, move);
                }

                if (move.StartsWith('x'))
                {
                    programs = Exchange(programs, move);
                }

                if (move.StartsWith('p'))
                {
                    programs = Partner(programs, move);
                }
            }
            return programs;
        }

        private static string Spin(string input, string move)
        {
            var count = int.Parse(move[1..]);
            count %= input.Length;
            if (count == 0 || count == input.Length) return input;

            return input[(input.Length - count)..] + input[..(input.Length - count)];
        }

        private static string Exchange(string input, string move)
        {
            var parts = move[1..].Split("/");
            var a = int.Parse(parts[0]);
            var b = int.Parse(parts[1]);
            var charArray = input.ToArray();
            (charArray[a], charArray[b]) = (charArray[b], charArray[a]);
            return new string(charArray);
        }

        private static string Partner(string input, string move)
        {
            var parts = move[1..].Split("/");
            var a = input.IndexOf(parts[0]);
            var b = input.IndexOf(parts[1]);
            var charArray = input.ToArray();
            (charArray[a], charArray[b]) = (charArray[b], charArray[a]);
            return new string(charArray);
        }
    }
}
