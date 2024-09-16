namespace AdventOfCode.Year2023
{
    internal class Day07 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            double sum = 0;
            var hands = lines.Select(l => Hand.ParseLine(l, false)).OrderBy(x => x.StrengthString).ToList();
            for (var i = 0; i < hands.Count; i++)
            {
                sum += (i + 1) * hands[i].BidAmount;
            }
            return sum;
        }

        public object HardSolution(IList<string> lines)
        {
            double sum = 0;
            var hands = lines.Select(l => Hand.ParseLine(l, true)).OrderBy(x => x.StrengthString).ToList();
            for (var i = 0; i < hands.Count; i++)
            {
                sum += (i + 1) * hands[i].BidAmount;
            }
            return sum;
        }

        private class Hand
        {
            public int BidAmount { get; set; }
            public string StrengthString { get; set; } = "";
            public string Cards { get; set; } = "";

            public static Hand ParseLine(string line, bool useJokers)
            {
                var parts = line.Split(" ");
                var cards = parts[0];
                var bidAmount = int.Parse(parts[1]);
                var strengthString = GetStrength(cards, useJokers);
                return new Hand
                {
                    Cards = cards,
                    BidAmount = bidAmount,
                    StrengthString = strengthString,
                };
            }

            public static string GetStrength(string cards, bool useJokers)
            {
                var sourceString = useJokers ? "J23456789TQKA" : "23456789TJQKA";
                var pointsString = "abcdefghijklm";
                var typeStrength = useJokers ? GetTypeStrengthWithJokers(cards) : GetTypeStrength(cards);
                return ((int)typeStrength).ToString() + new string(cards.Select(c => pointsString[sourceString.IndexOf(c)]).ToArray());
            }

            public static HandType GetTypeStrength(string cards)
            {
                var distinctCount = cards.Distinct().Count();
                if (distinctCount == 1) return HandType.FiveOfAKind;
                if (distinctCount == 5) return HandType.HighCard;
                if (distinctCount == 4) return HandType.OnePair;
                var firstCardCount = cards.Count(c => c == cards[0]);

                if (distinctCount == 2)
                {
                    if (firstCardCount == 1 || firstCardCount == 4) return HandType.FourOfAKind;
                    return HandType.FullHouse;
                }

                if (firstCardCount == 3) return HandType.ThreeOfAKind;
                if (firstCardCount == 2) return HandType.TwoPair;

                var secondCardCount = cards.Count(c => c == cards[1]);
                if (secondCardCount == 1 || secondCardCount == 3) return HandType.ThreeOfAKind;
                return HandType.TwoPair;
            }

            public static HandType GetTypeStrengthWithJokers(string cards)
            {
                var jokerCount = cards.Count(c => c == 'J');
                if (jokerCount == 0) return GetTypeStrength(cards);
                if (jokerCount == 4 || jokerCount == 5) return HandType.FiveOfAKind;

                var remainingCards = new string(cards.Where(c => c != 'J').ToArray());
                var distinctCount = remainingCards.Distinct().Count();

                if (distinctCount == 1) return HandType.FiveOfAKind;
                if (distinctCount == 4) return HandType.OnePair;
                if (jokerCount == 3) return HandType.FourOfAKind;
                if (jokerCount == 2)
                {
                    if (distinctCount == 2) return HandType.FourOfAKind;
                    if (distinctCount == 3) return HandType.ThreeOfAKind;
                }
                if (distinctCount == 3) return HandType.ThreeOfAKind;
                var firstCardCount = remainingCards.Count(c => c == remainingCards[0]);
                if (firstCardCount == 2) return HandType.FullHouse;
                return HandType.FourOfAKind;
            }

            public enum HandType
            {
                HighCard = 1,
                OnePair = 2,
                TwoPair = 3,
                ThreeOfAKind = 4,
                FullHouse = 5,
                FourOfAKind = 6,
                FiveOfAKind = 7,
            }
        }
    }
}
