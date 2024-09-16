namespace Helpers.Helpers
{
    public static class MathHelpers
    {
        public static string ConvertHexStringToBitString(string hexString)
        {
            return string.Concat(hexString.SelectMany(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }

        public static double LeastCommonMultiplier(double a, double b)
        {
            return a / GreatestCommonFactor(a, b) * b;
        }

        public static long LeastCommonMultiplier(long a, long b)
        {
            return a / GreatestCommonFactor(a, b) * b;
        }

        public static double GreatestCommonFactor(double a, double b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static long Factorial(long a)
        {
            long result = 1;
            for (var i = a; i > 0; i--)
            {
                result *= i;
            }
            return result;
        }

        public static long GreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static long ManhattanDistance((long x, long y) p1, (long x, long y) p2)
        {
            return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
        }

        public static List<List<T>> GetPermutations<T>(List<T> input) where T : notnull
        {
            if (input.Count == 0) throw new NotImplementedException();
            if (input.Count == 1) return [input];
            var results = new List<List<T>>();
            for (var i = 0; i < input.Count; i++)
            {
                var baseResult = new List<T> { input[i] };

                var rest = input.Take(i).Concat(input.Skip(i + 1)).ToList();
                foreach (var restPermutation in GetPermutations(rest))
                {
                    var result = baseResult.Concat(restPermutation);
                    results.Add(result.ToList());
                }
            }
            return results;
        }

        public static List<List<T>> GetCombinations<T>(List<T> input) where T : notnull
        {
            ulong key = 0b1;
            var dict = new Dictionary<ulong, T>();
            foreach (var item in input)
            {
                dict[key] = item;
                key <<= 1;
            }

            var combinations = new List<List<T>>();
            for (ulong i = 1L; i < Math.Pow(2, input.Count); i++)
            {
                var combination = new List<T>();
                foreach (var k in dict.Keys)
                {
                    if ((k | i) == i)
                    {
                        combination.Add(dict[k]);
                    }
                }
                combinations.Add(combination);
            }
            return combinations;
        }
    }
}
