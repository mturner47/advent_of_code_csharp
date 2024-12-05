namespace AdventOfCode.Year2024
{
    internal class Day01 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var list1 = new List<int>();
            var list2 = new List<int>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                list1.Add(int.Parse(parts[0]));
                list2.Add(int.Parse(parts[1]));
            }

            list1 = list1.OrderBy(i => i).ToList();
            list2 = list2.OrderBy(i => i).ToList();

            var expectedResult = 1341714;
            var result = list1.Zip(list2).Sum(t => Math.Abs(t.Second - t.First));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var list1 = new List<int>();
            var list2 = new List<int>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                list1.Add(int.Parse(parts[0]));
                list2.Add(int.Parse(parts[1]));
            }

            var sum = 0;

            foreach (var num in list1)
            {
                sum += (num * list2.Count(i => i == num));
            }

            var expectedResult = 27384707;
            var result = sum;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
