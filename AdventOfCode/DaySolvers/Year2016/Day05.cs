using Helpers.Helpers;

namespace AdventOfCode.Year2016
{
    internal class Day05 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var baseInput = "reyedfim";
            var password = "";
            var currentCount = 0;
            while (password.Length < 8)
            {
                var input = baseInput + currentCount.ToString();
                var hash = MD5Helper.HashString(input);
                if (hash.StartsWith("00000")) password += hash[5];
                currentCount++;
            }
            var expectedResult = "f97c354d";
            var result = password;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var baseInput = "reyedfim";
            var password = "________".ToArray();
            var currentCount = 0;
            while (true)
            {
                var input = baseInput + currentCount.ToString();
                var hash = MD5Helper.HashString(input);
                if (hash.StartsWith("00000"))
                {
                    if ("01234567".Contains(hash[5]))
                    {
                        var index = int.Parse(hash[5].ToString());
                        if (password[index] == '_') password[index] = hash[6];
                        if (password.All(c => c != '_')) break;
                    }
                }
                currentCount++;
            }
            var expectedResult = "863DDE27";
            var result = new string(password);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
