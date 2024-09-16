namespace AdventOfCode.Year2016
{
    internal class Day03 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var triangles = lines.Select(l =>
            {
                return l.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            });

            var expectedResult = 983;
            var result = triangles.Count(t => t.Sum() - t.Max() > t.Max());
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var triangles = lines.Select(l =>
            {
                return l.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            }).ToList();

            var newTriangles = new List<List<int>>();
            for (var i = 0; i < triangles.Count - 2; i += 3)
            {
                newTriangles.Add([triangles[i][0], triangles[i + 1][0], triangles[i + 2][0]]);
                newTriangles.Add([triangles[i][1], triangles[i + 1][1], triangles[i + 2][1]]);
                newTriangles.Add([triangles[i][2], triangles[i + 1][2], triangles[i + 2][2]]);
            }

            var expectedResult = 1836;
            var result = newTriangles.Count(t => t.Sum() - t.Max() > t.Max());
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
