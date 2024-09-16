namespace AdventOfCode.Year2023
{
    internal class Day04 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            return lines
                .Select(ScratchCard.ParseLine)
                .Sum(sc => sc.MatchCount == 0 ? 0 : Math.Pow(2, sc.MatchCount - 1));
        }

        public object HardSolution(IList<string> lines)
        {
            var scratchCards = lines.Select(ScratchCard.ParseLine).Reverse().ToList();
            var scratchCardDictionary = scratchCards.ToDictionary(sc => sc.CardNumber, sc => sc);

            foreach (var scratchCard in scratchCards)
            {
                if (scratchCard.MatchCount == 0) scratchCard.NumCreated = 1;
                else
                {
                    var numCreated = 1;
                    for (var i = 1; i <= scratchCard.MatchCount; i++)
                    {
                        var num = scratchCard.CardNumber + i;
                        if (scratchCardDictionary.ContainsKey(num))
                        {
                            numCreated += scratchCardDictionary[num].NumCreated;
                        }
                    }
                    scratchCard.NumCreated = numCreated;
                }
            }

            return scratchCards.Sum(sc => sc.NumCreated);
        }

        private class ScratchCard
        {
            public int CardNumber { get; set; }
            public int MatchCount { get; set; }
            public int NumCreated { get; set; }

            public static ScratchCard ParseLine(string line)
            {
                var data = line.Split(": ");
                var cardNumber = int.Parse(data[0].Replace("Card", "").Trim());
                var dataParts = data[1].Split(" | ");
                var winningNumbers = dataParts[0].Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                var ownedNumbers = dataParts[1].Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                var matchCount = winningNumbers.Intersect(ownedNumbers).Distinct().Count();
                return new ScratchCard
                {
                    CardNumber = cardNumber,
                    MatchCount = matchCount,
                };
            }
        }
    }
}
