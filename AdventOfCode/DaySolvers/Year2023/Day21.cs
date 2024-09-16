using Helpers.Helpers;
using System.Drawing;

namespace AdventOfCode.Year2023
{
    internal class Day21 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var grid = lines.Select(l => l.Select(c => new Cell { Obj = c, StepSeen = Step.Unknown }).ToList()).ToList();
            var maxX = lines[0].Length - 1;
            var maxY = grid.Count - 1;
            var startingPoint = FindStartingPoint(grid);
            var pointsToCheck = new List<(int x, int y)> { startingPoint };
            var stepCount = 64;
            var directions = new List<Direction> { Direction.North, Direction.South, Direction.West, Direction.East };
            var movements = directions.Select(d => d.GetMovement()).ToList();
            for (var i = 1; i <= stepCount; i++)
            {
                var newPointsToCheck = new List<(int x, int y)>();
                foreach (var (x, y) in pointsToCheck)
                {
                    var currentCell = grid[y][x];
                    foreach (var (moveX, moveY) in movements)
                    {
                        var (newX, newY) = (x + moveX, y + moveY);
                        if (newX < 0 || newX > maxX || newY < 0 || newY > maxY) continue;
                        var nextCell = grid[newY][newX];
                        if (nextCell.Obj == '#') continue;
                        if (nextCell.StepSeen != Step.Unknown) continue;
                        nextCell.StepSeen = (currentCell.StepSeen == Step.Even || currentCell.Obj == 'S') ? Step.Odd : Step.Even;
                        newPointsToCheck.Add((newX, newY));
                    }
                }
                pointsToCheck = newPointsToCheck;
            }

            var stepToCheck = (stepCount % 2 == 0) ? Step.Even : Step.Odd;
            return grid.Sum(l => l.Count(c => c.StepSeen == stepToCheck));
        }

        public object HardSolution(IList<string> lines)
        {
            var input = lines.ToList();
            var gridSize = input.Count == input[0].Length ? input.Count : throw new ArgumentOutOfRangeException();

            var start = Enumerable.Range(0, gridSize)
                .SelectMany(i => Enumerable.Range(0, gridSize)
                    .Where(j => input[i][j] == 'S')
                    .Select(j => (i, j)))
                .Single();

            var grids = 26501365 / gridSize;
            var rem = 26501365 % gridSize;

            // By inspection, the grid is square and there are no barriers on the direct horizontal / vertical path from S
            // So, we'd expect the result to be quadratic in (rem + n * gridSize) steps, i.e. (rem), (rem + gridSize), (rem + 2 * gridSize), ...
            // Use the code from Part 1 to calculate the first three values of this sequence, which is enough to solve for ax^2 + bx + c
            var sequence = new List<int>();
            var work = new HashSet<(int i, int j)> { start };
            var steps = 0;
            for (var n = 0; n < 3; n++)
            {
                for (; steps < n * gridSize + rem; steps++)
                {
                    // Funky modulo arithmetic bc modulo of a negative number is negative, which isn't what we want here
                    work = new HashSet<(int i, int j)>(work
                        .SelectMany(it => new[] { Direction.North, Direction.South, Direction.East, Direction.West }.Select(dir => dir.GetMovement(it)))
                        .Where(dest => input[((dest.x % 131) + 131) % 131][((dest.y % 131) + 131) % 131] != '#'));
                }

                sequence.Add(work.Count);
            }

            // Solve for the quadratic coefficients
            var c = sequence[0];
            var aPlusB = sequence[1] - c;
            var fourAPlusTwoB = sequence[2] - c;
            var twoA = fourAPlusTwoB - (2 * aPlusB);
            var a = twoA / 2;
            var b = aPlusB - a;

            long F(long n)
            {
                return a * (n * n) + b * n + c;
            }

            return F(grids);
        }

        private static (int x, int y) FindStartingPoint(List<List<Cell>> grid)
        {
            for (var y = 0; y < grid.Count; y++)
            {
                var row = grid[y];
                for (var x = 0; x < row.Count; x++)
                {
                    if (row[x].Obj == 'S') return (x, y);
                }
            }
            throw new Exception("No Starting point found.");
        }

        private class Cell
        {
            public char Obj { get; set; }
            public Step StepSeen { get; set; }
        }

        private enum Step
        {
            Unknown,
            Even,
            Odd,
        }
    }
}
