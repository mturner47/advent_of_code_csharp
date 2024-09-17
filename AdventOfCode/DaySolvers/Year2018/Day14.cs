namespace AdventOfCode.Year2018
{
    internal class Day14 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var input = int.Parse(lines[0]);
            List<int> scoreboard = [3, 7];
            var elf1Index = 0;
            var elf2Index = 1;

            while (scoreboard.Count < input + 10)
            {
                var sum = scoreboard[elf1Index] + scoreboard[elf2Index];
                if (sum >= 10)
                {
                    scoreboard.Add(1);
                }
                scoreboard.Add(sum % 10);
                elf1Index = (elf1Index + scoreboard[elf1Index] + 1)%scoreboard.Count;
                elf2Index = (elf2Index + scoreboard[elf2Index] + 1) % scoreboard.Count;
            }
            var expectedResult = "1413131339";
            var result = string.Join("", scoreboard.Skip(input).Take(10));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var input = lines[0];
            List<int> scoreboard = [3, 7, 1, 0, 1, 0];
            var elf1Index = 4;
            var elf2Index = 3;
            var lastSix = "371010";

            while (true)
            {
                var sum = scoreboard[elf1Index] + scoreboard[elf2Index];
                if (sum >= 10)
                {
                    scoreboard.Add(1);
                    lastSix = lastSix[1..] + "1";
                    if (lastSix == input) break;
                }
                scoreboard.Add(sum % 10);
                lastSix = lastSix[1..] + (sum%10).ToString();
                if (lastSix == input) break;
                elf1Index = (elf1Index + scoreboard[elf1Index] + 1) % scoreboard.Count;
                elf2Index = (elf2Index + scoreboard[elf2Index] + 1) % scoreboard.Count;
            }

            var expectedResult = 20254833;
            var result = scoreboard.Count - 6;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
