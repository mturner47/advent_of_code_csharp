namespace AdventOfCode.DaySolvers.Year2024
{
    internal class Day13 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var groups = Parse(lines, false);
            var expectedResult = 26299;
            var result = groups.Select(GetTokenCostEasy).Where(t => t.HasValue).Sum(t => t.Value);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var groups = Parse(lines, true);
            var expectedResult = 107824497933339;
            var result = groups.Select(GetTokenCostHard).Where(t => t.HasValue).Sum(t => t.Value);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<((double x, double y) buttonA, (double x, double y) buttonB, (double x, double y) prizeLocation)> Parse(IList<string> lines, bool isHard)
        {
            var groups = string.Join("\n", lines).Split("\n\n").ToList();
            return groups.Select(g =>
            {
                var parts = g.Split("\n");
                var buttonAParts = parts[0].Split(", Y+");
                var buttonA = (x: double.Parse(buttonAParts[0].Replace("Button A: X+", "")), y: double.Parse(buttonAParts[1]));
                var buttonBParts = parts[1].Split(", Y+");
                var buttonB = (x: double.Parse(buttonBParts[0].Replace("Button B: X+", "")), y: double.Parse(buttonBParts[1]));
                var prizeParts = parts[2].Split(", Y=");
                var offset = isHard ? 10000000000000 : 0;
                var prizeLocation = (x: offset + double.Parse(prizeParts[0].Replace("Prize: X=", "")), y: offset + double.Parse(prizeParts[1]));
                return (buttonA, buttonB, prizeLocation);
            }).ToList();
        }

        private static double? GetTokenCostEasy(((double x, double y) buttonA, (double x, double y) buttonB, (double x, double y) prizeLocation) machine)
        {
            var minCost = (double?)null;
            var ((aX, aY), (bX, bY), (pX, pY)) = machine;
            for (var a = 1; a <= 100; a++)
            {
                var (aDistanceX, aDistanceY) = (a * aX, a * aY);
                if (aDistanceX > pX || aDistanceY > pY) break;
                var aCost = a * 3;
                for (var b = 1; b <= 100; b++)
                {
                    var (finalX, finalY) = (aDistanceX + b*bX, aDistanceY + b*bY);
                    if (finalX == pX && finalY == pY)
                    {
                        var totalCost = aCost + b;
                        if (!minCost.HasValue || minCost.Value > totalCost) minCost = totalCost;
                        break;
                    }
                    if (finalX > pX || finalY > pY) break;
                }
            }
            return minCost;
        }

        private static double? GetTokenCostHard(((double x, double y) buttonA, (double x, double y) buttonB, (double x, double y) prizeLocation) machine)
        {
            var ((aX, aY), (bX, bY), (pX, pY)) = machine;
            var a = (bX*pY - bY*pX)/(bX*aY - aX*bY);
            var b = (pX - a * aX) / bX;
            if (a % 1 > 0 || b % 1 > 0) return null;
            return a * 3 + b;
        }
    }
}
