
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace AdventOfCode.DaySolvers.Year2024
{
    internal class Day14 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var robots = lines.Select(Parse).ToList();
            var numIterations = 100;
            var maxX = 100;
            var maxY = 102;
            var finalPositions = robots.Select(r => GetFinalPosition(r, numIterations, maxX, maxY)).ToList();

            var expectedResult = 228410028;
            var result = GetSecurityCode(finalPositions, maxX, maxY);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var robots = lines.Select(Parse).ToList();
            var maxX = 100;
            var maxY = 102;

            var bestScore = double.MaxValue;
            var bestIndex = 10000;
            var bestPositions = new List<(int x, int y)>();
            for (var i = 1; i < 10000; i++)
            {
                var finalPositions = robots.Select(r => GetFinalPosition(r, i, maxX, maxY)).ToList();
                var score = GetSecurityCode(finalPositions, maxX, maxY);
                if (score < bestScore)
                {
                    bestScore = score;
                    bestIndex = i;
                    bestPositions = finalPositions;
                }
            }
            //var sb = new StringBuilder();
            //for (var y = 0; y <= maxY; y++)
            //{
            //    for (var x = 0; x <= maxX; x++)
            //    {
            //        if (bestPositions.Contains((x, y))) sb.Append("O");
            //        else sb.Append(" ");
            //    }
            //    sb.AppendLine();
            //}
            //Console.Write(sb.ToString());
            var expectedResult = 8258;
            var result = bestIndex;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static ((int x, int y) position, (int x, int y) velocity) Parse(string line)
        {
            var parts = line.Split(" v=");
            var positionParts = parts[0].Split(",");
            var position = (x: int.Parse(positionParts[0].Replace("p=", "")), y: int.Parse(positionParts[1]));
            var velocityParts = parts[1].Split(",");
            var velocity = (x: int.Parse(velocityParts[0]), y: int.Parse(velocityParts[1]));
            return (position, velocity);
        }

        private static (int x, int y) GetFinalPosition(((int x, int y) position, (int x, int y) velocity) r, int numIterations, int maxX, int maxY)
        {
            var finalX = r.position.x + r.velocity.x * numIterations;
            var finalY = r.position.y + r.velocity.y * numIterations;
            if (finalX < 0)
            {
                while (finalX < 0)
                {
                    finalX += (maxX + 1);
                }
            }
            else if (finalX > maxX)
            {
                while (finalX > maxX)
                {
                    finalX -= (maxX + 1);
                }
            }

            if (finalY < 0)
            {
                while (finalY < 0)
                {
                    finalY += (maxY + 1);
                }
            }
            else if (finalY > maxY)
            {
                while (finalY > maxY)
                {
                    finalY -= (maxY + 1);
                }
            }

            return (finalX, finalY);
        }

        private static double GetSecurityCode(List<(int x, int y)> positions, int maxX, int maxY)
        {
            double sector1 = positions.Count(p => p.x < maxX / 2 && p.y < maxY / 2);
            double sector2 = positions.Count(p => p.x < maxX / 2 && p.y > maxY / 2);
            double sector3 = positions.Count(p => p.x > maxX / 2 && p.y < maxY / 2);
            double sector4 = positions.Count(p => p.x > maxX / 2 && p.y > maxY / 2);
            return sector1 * sector2 * sector3 * sector4;
        }
    }
}
