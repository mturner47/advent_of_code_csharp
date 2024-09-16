namespace AdventOfCode.Year2022
{
    internal class Day04 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            return lines.Count(OneContainsTheOther);
        }

        public object HardSolution(IList<string> lines)
        {
            return lines.Count(OneOverlapsTheOther);
        }

        private static bool OneContainsTheOther(string line)
        {
            var groups = line.Split(",");
            var pairOne = groups[0].Split("-");
            var pairTwo = groups[1].Split("-");
            var minOne = int.Parse(pairOne[0]);
            var maxOne = int.Parse(pairOne[1]);
            var minTwo = int.Parse(pairTwo[0]);
            var maxTwo = int.Parse(pairTwo[1]);

            return (minOne <= minTwo && maxOne >= maxTwo) || (minTwo <= minOne && maxTwo >= maxOne);
        }

        private static bool OneOverlapsTheOther(string line)
        {
            var groups = line.Split(",");
            var pairOne = groups[0].Split("-");
            var pairTwo = groups[1].Split("-");
            var minOne = int.Parse(pairOne[0]);
            var maxOne = int.Parse(pairOne[1]);
            var minTwo = int.Parse(pairTwo[0]);
            var maxTwo = int.Parse(pairTwo[1]);

            return !((minOne < minTwo && maxOne < minTwo) || (minTwo < minOne && maxTwo < minOne));
        }
    }
}
