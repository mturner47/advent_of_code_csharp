using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
    internal class Day04 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            List<string> requiredFields = ["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];
            var passportGroups = string.Join('\n', lines).Split("\n\n").Select(lg =>
            {
                return lg.Replace("\n", " ").Split(" ").Select(p => p.Split(":")[0]).ToList();
            }).ToList();

            var expectedResult = 235;
            var result = passportGroups.Count(pg => pg.Intersect(requiredFields).Count() == requiredFields.Count);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            List<string> requiredFields = ["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];
            var passportGroups = string.Join('\n', lines).Split("\n\n").Select(lg =>
            {
                return lg.Replace("\n", " ").Split(" ").Select(p =>
                {
                    var parts = p.Split(":");
                    return (name: parts[0], value:parts[1]);
                }).ToList();
            }).ToList();

            var validCount = 0;
            foreach (var passportGroup in passportGroups)
            {
                if (passportGroup.Select(pg => pg.name).Intersect(requiredFields).Count() != requiredFields.Count) continue;
                if (passportGroup.All(pg => IsValid(pg.name, pg.value))) validCount++;
            }

            var expectedResult = 194;
            var result = validCount;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static bool IsValid(string name, string value)
        {
            if (name == "cid") return true;
            if (name == "byr") return int.Parse(value) >= 1920 && int.Parse(value) <= 2002;
            if (name == "iyr") return int.Parse(value) >= 2010 && int.Parse(value) <= 2020;
            if (name == "eyr") return int.Parse(value) >= 2020 && int.Parse(value) <= 2030;
            if (name == "hgt")
            {
                if (value.EndsWith("cm"))
                {
                    var height = int.Parse(value.Replace("cm", ""));
                    return height >= 150 && height <= 193;
                }
                else if (value.EndsWith("in"))
                {
                    var height = int.Parse(value.Replace("in", ""));
                    return height >= 59 && height <= 76;
                }
                else return false;
            }
            if (name == "hcl") return value.StartsWith('#') && value.Length == 7 && value.Skip(1).All("0123456789abcdef".Contains);
            if (name == "ecl") return "amb blu brn gry grn hzl oth".Split(" ").Contains(value);
            if (name == "pid") return value.Length == 9 && value.All("0123456789".Contains);
            return false;
        }
    }
}
