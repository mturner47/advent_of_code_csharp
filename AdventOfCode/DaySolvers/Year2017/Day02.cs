namespace AdventOfCode.Year2017
{
    internal class Day02 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var expectedResult = 45351;
            var result = lines.Sum(l =>
            {
                var values = l.Split("\t").Select(int.Parse).ToList();
                return values.Max() - values.Min();
            });
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 275;
            var result = lines.Sum(l =>
            {
                var values = l.Split("\t").Select(int.Parse).ToList();
                for (var i = 0; i < values.Count - 1; i++)
                {
                    var v1 = values[i];
                    for (var j = i + 1; j < values.Count; j++)
                    {
                        var v2 = values[j];
                        if (v2 % v1 == 0) return v2 / v1;
                        if (v1 % v2 == 0) return v1 / v2;
                    }
                }
                throw new NotImplementedException();
            });
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
