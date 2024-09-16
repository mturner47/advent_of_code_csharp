using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Year2022
{
    internal class Day23 : IDaySolver
    {
        private static readonly List<(int x, int y)> _possibleMovements = GetPossibleMovements();
        private static readonly List<List<int>> _possibleIndices = GetPossibleIndices();

        public object EasySolution(IList<string> lines)
        {
            var elfLocations = ParseLines(lines).ToList();
            var numRounds = 10;
            var startingDirectionIndex = 0;

            for (var i = 0; i < numRounds; i++)
            {
                //DrawElves(elfLocations);
                (elfLocations, _) = RunRound(elfLocations, startingDirectionIndex);
                startingDirectionIndex = (startingDirectionIndex + 1) % 4;
            }

            var minY = elfLocations.Min(el => el.y);
            var maxY = elfLocations.Max(el => el.y) + 1;
            var minX = elfLocations.Min(el => el.x);
            var maxX = elfLocations.Max(el => el.x) + 1;

            return (maxX - minX)*(maxY - minY) - elfLocations.Count;
        }

        public object HardSolution(IList<string> lines)
        {
            var elfLocations = ParseLines(lines).ToList();
            var startingDirectionIndex = 0;
            var elvesMoved = true;
            var round = 0;

            while (elvesMoved)
            {
                round++;
                (elfLocations, elvesMoved) = RunRound(elfLocations, startingDirectionIndex);
                startingDirectionIndex = (startingDirectionIndex + 1) % 4;
            }

            return round;
        }

        private static IEnumerable<(double x, double y)> ParseLines(IList<string> lines)
        {
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    if (c == '#')
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        private (List<(double x, double y)>, bool) RunRound(List<(double x, double y)> elfLocations, int startingDirectionIndex)
        {
            var proposedLocations = GetProposedLocations(elfLocations, startingDirectionIndex).ToList();
            var numElvesMoved = 0;
            var newLocations = new List<(double x, double y)>();
            for (var i = 0; i < elfLocations.Count; i++)
            {
                var el = elfLocations[i];
                var pl = proposedLocations[i];
                if (el.x == pl.x && el.y == pl.y)
                {
                    newLocations.Add(el);
                }
                else if (proposedLocations.Count(p => p.x == pl.x && p.y == pl.y) > 1)
                {
                    newLocations.Add(el);
                }
                else
                {
                    newLocations.Add(pl);
                    numElvesMoved++;
                }
            }
            return (newLocations, numElvesMoved > 0);
        }

        private IEnumerable<(double x, double y)> GetProposedLocations(List<(double x, double y)> elfLocations, int startingDirectionIndex)
        {
            for (var ei = 0; ei < elfLocations.Count; ei++)
            {
                var (eX, eY) = elfLocations[ei];
                var elvesInPositions = _possibleMovements.Select(p => elfLocations.Contains((eX + p.x, eY + p.y))).ToList();

                if (elvesInPositions.All(e => !e))
                {
                    yield return (eX, eY);
                    continue;
                }

                var foundDirection = false;
                for (var di = 0; di < 4; di++)
                {
                    var correctedIndex = (startingDirectionIndex + di) % 4;
                    var possibleIndices = _possibleIndices[correctedIndex];
                    if (!possibleIndices.Any(pi => elvesInPositions[pi]))
                    {
                        var cardinalDirection = _possibleMovements[_possibleIndices[correctedIndex][1]];
                        foundDirection = true;
                        yield return (eX + cardinalDirection.x, eY + cardinalDirection.y);
                        break;
                    }
                }
                if (!foundDirection) yield return (eX, eY);
            }
        }

        private static List<(int x, int y)> GetPossibleMovements()
        {
            return new List<(int x, int y)>
            {
                (0, -1), // N
                (1, -1), // NE
                (1, 0), // E
                (1, 1), // SE
                (0, 1), // S
                (-1, 1), // SW
                (-1, 0), // W
                (-1, -1), // NW
            };
        }

        private static List<List<int>> GetPossibleIndices()
        {
            return new List<List<int>>
            {
                new List<int> { 7, 0, 1 }, // NW, N, NE
                new List<int> { 3, 4, 5 }, // SE, S, SW
                new List<int> { 5, 6, 7 }, // SW, W, NW
                new List<int> { 1, 2, 3 }, // NE, E, SE
            };
        }

        private void DrawElves(List<(double x, double y)> elfLocations)
        {
            var minY = elfLocations.Min(el => el.y);
            var maxY = elfLocations.Max(el => el.y);
            var minX = elfLocations.Min(el => el.x);
            var maxX = elfLocations.Max(el => el.x);
            var sb = new StringBuilder();
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    sb.Append(elfLocations.Contains((x, y)) ? '#' : '.');
                }
                sb.Append('\n');
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
