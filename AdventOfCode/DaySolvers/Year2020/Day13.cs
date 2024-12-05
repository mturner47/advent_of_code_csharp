namespace AdventOfCode.Year2020
{
    internal class Day13 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var startTime = long.Parse(lines[0]);
            var (id, timeToWait) = lines[1].Split(',')
                .Where(s => s != "x")
                .Select(long.Parse)
                .Select(i => (id:i, timeToWait:i - startTime%i))
                .OrderBy(i => i.timeToWait)
                .First();

            var expectedResult = 203;
            var result = id * timeToWait;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var departures = lines[1].Split(',');
            var modulos = new List<(long mod, long remainder)>();
            var t = 0;
            for (var i = 0; i < departures.Length; i++)
            {
                if (departures[i] != "x")
                {
                    var busID = int.Parse(departures[i]);
                    modulos.Add((busID, busID - t%busID));
                }
                t++;
            }

            var expectedResult = 905694340256752;
            var result = ChineseRemainderTheory(modulos);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static long ChineseRemainderTheory(List<(long mod, long remainder)> modulos)
        {
            var fullProduct = modulos.Select(m => m.mod).Aggregate(1L, (a, b) => a * b);
            var sum = modulos.Sum(m =>
            {
                var bigMod = fullProduct / m.mod;
                var inverse = FindInverse(bigMod, m.mod);
                return bigMod * inverse * m.remainder;
            });
            return sum % fullProduct;
        }

        private static long FindInverse(long bigMod, long mod)
        {
            bigMod %= mod;
            var i = 1;
            while (true)
            {
                if ((bigMod * i) % mod == 1) return i;
                i++;
            }
        }
    }
}
