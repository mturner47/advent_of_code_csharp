using System.Numerics;

namespace AdventOfCode.Year2019
{
    internal class Day22 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var deckCount = 10007;
            var deck = Enumerable.Range(0, deckCount).ToList();
            foreach (var line in lines)
            {
                if (line.StartsWith("deal with increment "))
                {
                    var increment = int.Parse(line.Replace("deal with increment ", ""));
                    deck = DealWithIncrement(deck, increment);
                }
                else if (line.StartsWith("deal into"))
                {
                    deck = DealNew(deck);
                }
                else
                {
                    var cutAmount = int.Parse(line.Replace("cut ", ""));
                    deck = Cut(deck, cutAmount);
                }
            }

            var expectedResult = 6061;
            var result = deck.IndexOf(2019);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var expectedResult = 79490866971571;
            var result = Solve(lines);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        static BigInteger Pow(long nnn, long times, long m)
        {
            return (long)BigInteger.ModPow(new BigInteger(nnn), new BigInteger(times), new BigInteger(m));
        }

        const Int64 _factory = 119315717514047;
        const Int64 _times = 101741582076661;
        static BigInteger _a = 1;
        static BigInteger _b = 0;
        internal static long ReversePlay(string line, long CardPosition)
        {
            if (line == "deal into new stack")
            {
                _b += 1;
                _a *= -1;
                _b *= -1;
                _a %= _factory;
                _b %= _factory;
                return _factory - 1 - CardPosition;
            }
            if (line.StartsWith("cut "))
            {
                var n = long.Parse(line[4..]);
                _b += n;
                _a %= _factory;
                _b %= _factory;
                if (n < 0)
                {
                    n = _factory + n;
                }
                n %= _factory;
                return (CardPosition + n) % _factory;
            }
            if (line.StartsWith("deal with increment "))
            {
                var n = long.Parse(line["deal with increment ".Length..]);
                var p = (Pow(n, _factory - 2, _factory) + _factory) % _factory;
                _a *= p;
                _b *= p;
                _a %= _factory;
                _b %= _factory;
                if (CardPosition % n == 0) return CardPosition / n;
                long start = 0;
                long div_acc = 0;
                long test = -1;
                for (long smart = 0; smart < 1000000; smart++)
                {
                    long div = ((_factory - start) / n) + 1;
                    div_acc += div;
                    long rest = start + div * n - _factory;
                    long diff = CardPosition - rest;
                    if (diff % n == 0)
                    {
                        test = (diff / n) + div_acc;
                        break;
                    }
                    start = rest;
                }

                if ((test * n) % _factory == CardPosition) return test;
                long ret = -1;
                for (long scan = 0; scan < _factory; scan++)
                {
                    if (scan % 1000000 == 0) Console.WriteLine("slow scanning {0}", scan);
                    if ((scan * n) % _factory == CardPosition)
                    {
                        ret = n;
                        break;
                    }
                }
                if (ret < 0) throw new Exception("incrment not found");
                return ret;
            }
            return CardPosition;
        }

        private static BigInteger Solve(IList<string> lines)
        {
            lines = lines.Reverse().ToArray();
            long CardPosition = 2020;
            foreach (var line in lines)
            {
                CardPosition = ReversePlay(line, CardPosition);
            }

            long a1 = (long)(_a + _factory) % _factory;
            long b1 = (long)(_b + _factory) % _factory;
            return (
                Pow(a1, _times, _factory) * 2020 +
                b1 * (Pow(a1, _times, _factory) + _factory - 1)
                  * (Pow(a1 - 1, _factory - 2, _factory))
                + _factory) % _factory;
        }

        private static List<int> Cut(List<int> cards, int amount)
        {
            if (amount < 0) amount = cards.Count + amount;
            return cards.Skip(amount).Concat(cards.Take(amount)).ToList();
        }

        private static List<int> DealNew(List<int> cards)
        {
            return cards.AsEnumerable().Reverse().ToList();
        }

        private static List<int> DealWithIncrement(List<int> cards, int increment)
        {
            var dict = new Dictionary<int, int>();
            var newList = new List<int>();
            var positionToAdd = 0;
            for (var i = 0; i < cards.Count; i++)
            {
                dict[positionToAdd%cards.Count] = cards[i];
                positionToAdd += increment;
            }
            return dict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
        }
    }
}
