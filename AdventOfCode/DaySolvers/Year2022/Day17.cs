namespace AdventOfCode.Year2022
{
    internal class Day17 : IDaySolver
    {
        private const string EmptyLine = ".......";
        public object EasySolution(IList<string> lines)
        {
            bool shouldPrint = false;

            var streamDirections = lines[0].Select(c => c == '>' ? Dir.Right : Dir.Left).ToList();
            var minX = 0;
            var maxX = 6;
            var startingXIndex = minX + 2;
            var rockFormations = new List<RockFormation>
            {
                RockFormation.HorizontalLine(),
                RockFormation.Cross(),
                RockFormation.LShape(),
                RockFormation.VerticalLine(),
                RockFormation.Square(),
            };

            var grid = new List<string>();
            var currentFormationIndex = 0;
            var currentStreamDirectionIndex = 0;
            RockFormation? currentRockFormation = null;
            var numRocksDropped = 0;
            var maxRocks = 2022;
            var leftMove = (-1, 0);
            var rightMove = (1, 0);
            var downMove = (0, -1);

            while (true)
            {
                if (currentRockFormation == null)
                {
                    var startingYIndex = grid.Count() + 3;
                    currentRockFormation = rockFormations[currentFormationIndex];
                    currentRockFormation.Corner = (startingXIndex, startingYIndex);
                    currentFormationIndex++;
                    if (currentFormationIndex%rockFormations.Count == 0)
                    {
                        currentFormationIndex = 0;
                    }
                }
                if (shouldPrint) PrintState(grid, currentRockFormation);

                var direction = streamDirections[currentStreamDirectionIndex];
                currentStreamDirectionIndex++;
                if (currentStreamDirectionIndex%streamDirections.Count == 0)
                {
                    currentStreamDirectionIndex = 0;
                }

                var (currentCornerX, currentCornerY) = currentRockFormation.Corner;
                if (direction == Dir.Left)
                {
                    if (currentCornerX != minX)
                    {
                        if (currentRockFormation.RockCanMove(grid, leftMove))
                        {
                            currentRockFormation.Move(leftMove);
                        }
                    }
                }
                else
                {
                    if (currentRockFormation.FarthestRockX() != maxX)
                    {
                        if (currentRockFormation.RockCanMove(grid, rightMove))
                        {
                            currentRockFormation.Move(rightMove);
                        }
                    }
                }

                if (shouldPrint) PrintState(grid, currentRockFormation);

                var rockShouldStop = false;
                if (currentCornerY == 0)
                {
                    rockShouldStop = true;
                }
                else
                {
                    if (currentRockFormation.RockCanMove(grid, downMove))
                    {
                        currentRockFormation.Move(downMove);
                    }
                    else
                    {
                        rockShouldStop = true;
                    }
                }

                if (rockShouldStop)
                {
                    var rockLocations = currentRockFormation.RockLocationsAtCorner(currentRockFormation.Corner);
                    foreach (var rockLocation in rockLocations)
                    {
                        var (x, y) = rockLocation;
                        if (grid.Count - 1 < y)
                        {
                            grid.Add(EmptyLine);
                        }
                        var newLine = grid[y].ToList();
                        newLine[x] = '#';
                        grid[y] = string.Concat(newLine);
                    }
                    currentRockFormation = null;

                    numRocksDropped++;
                    if (numRocksDropped == maxRocks) break;
                }
            }

            return grid.Count;
        }

        public object HardSolution(IList<string> lines)
        {
            return new Tunnel(lines[0]).AddRocks(1000000000000).height;
        }

        private class RockFormation
        {
            public RockFormation(List<(int, int)> rocks)
            {
                Rocks = rocks;
            }

            public List<(int, int)> Rocks { get; set; }
            public (int, int) Corner { get; set; }

            public List<(int, int)> RockLocationsAtCorner((int, int) corner)
            {
                return Rocks.Select(r =>
                {
                    var (x, y) = r;
                    var (cornerX, cornerY) = corner;
                    return (x + cornerX, y + cornerY);
                }).ToList();
            }

            public bool RockCanMove(List<string> grid, (int, int) movement)
            {
                var (cornerX, cornerY) = Corner;
                var (movementX, movementY) = movement;
                var newRockLocations = RockLocationsAtCorner((cornerX + movementX, cornerY + movementY));
                foreach (var r in newRockLocations)
                {
                    var (x, y) = r;
                    if (grid.Count >= y+1)
                    {
                        if (grid[y][x] == '#') return false;
                    }
                }
                return true;
            }

            public int FarthestRockX()
            {
                return RockLocationsAtCorner(Corner).Select(r => r.Item1).Max();
            }

            public void Move((int, int) movement)
            {
                var (cornerX, cornerY) = Corner;
                var (movementX, movementY) = movement;
                Corner = (cornerX + movementX, cornerY + movementY);
            }

            public static RockFormation HorizontalLine() => new(new List<(int, int)> { (0, 0), (1, 0), (2, 0), (3, 0) });
            public static RockFormation Cross() => new(new List<(int, int)> { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) });
            public static RockFormation LShape() => new(new List<(int, int)> { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) });
            public static RockFormation VerticalLine() => new(new List<(int, int)> { (0, 0), (0, 1), (0, 2), (0, 3) });
            public static RockFormation Square() => new(new List<(int, int)> { (0, 0), (1, 0), (0, 1), (1, 1) });
        }

        private static void PrintState(List<string> grid, RockFormation currentRockFormation)
        {
            var rockLocations = currentRockFormation.RockLocationsAtCorner(currentRockFormation.Corner);
            var printGrid = grid.Select(x => x).ToList();
            foreach (var rockLocation in rockLocations)
            {
                var (x, y) = rockLocation;
                while (printGrid.Count - 1 < y)
                {
                    printGrid.Add(EmptyLine);
                }
                var newLine = printGrid[y].ToList();
                newLine[x] = '@';
                printGrid[y] = string.Concat(newLine);
            }

            printGrid.Reverse();
            foreach (var line in printGrid)
            {
                Console.WriteLine("|" + string.Concat(line) + "|");
            }
            Console.WriteLine("+-------+");
            Console.WriteLine();
            Console.ReadLine();
        }

        private enum Dir
        {
            Left,
            Right,
        }

        class Tunnel
        {
            // preserve just the top of the whole cave this is a practical 
            // constant, there is NO THEORY BEHIND it.
            const int linesToStore = 100;
            List<string> lines;
            long linesNotStored;

            public long height => lines.Count + linesNotStored - 1;

            IEnumerator<string[]> rocks;
            IEnumerator<char> jets;

            public Tunnel(string jets)
            {
                var rocks = new[]{
                "####".Split("\n"),
                " # \n###\n # ".Split("\n"),
                "  #\n  #\n###".Split("\n"),
                "#\n#\n#\n#".Split("\n"),
                "##\n##".Split("\n")
            };

                this.rocks = Loop(rocks).GetEnumerator();
                this.jets = Loop(jets.Trim()).GetEnumerator();
                this.lines = new List<string>() { "+-------+" };
            }

            public Tunnel AddRocks(long rocks)
            {
                // We are adding rocks one by one until we find a recurring pattern.

                // Then we can jump forward full periods with just increasing the height 
                // of the cave: the top of the cave should look the same after a full period
                // so no need to simulate he rocks anymore. 

                // Then we just add the remaining rocks. 

                var seen = new Dictionary<string, (long rocks, long height)>();
                while (rocks > 0)
                {
                    var hash = string.Join("\n", lines);
                    if (seen.TryGetValue(hash, out var cache))
                    {
                        // we have seen this pattern.
                        // compute length of the period, and how much does it
                        // add to the height of the cave:
                        var heightOfPeriod = this.height - cache.height;
                        var periodLength = cache.rocks - rocks;

                        // advance forwad as much as possible
                        linesNotStored += (rocks / periodLength) * heightOfPeriod;
                        rocks = rocks % periodLength;
                        break;
                    }
                    else
                    {
                        seen[hash] = (rocks, this.height);
                        this.AddRock();
                        rocks--;
                    }
                }

                while (rocks > 0)
                {
                    this.AddRock();
                    rocks--;
                }
                return this;
            }

            public Tunnel AddRock()
            {
                // Adds one rock to the cave
                rocks.MoveNext();
                var rock = rocks.Current;

                // make room: 3 lines + the height of the rock
                for (var i = 0; i < rock.Length + 3; i++)
                {
                    lines.Insert(0, "|       |");
                }

                // simulate falling
                var (rockX, rockY) = (3, 0);
                while (true)
                {
                    jets.MoveNext();
                    if (jets.Current == '>' && !Hit(rock, rockX + 1, rockY))
                    {
                        rockX++;
                    }
                    else if (jets.Current == '<' && !Hit(rock, rockX - 1, rockY))
                    {
                        rockX--;
                    }
                    if (Hit(rock, rockX, rockY + 1))
                    {
                        break;
                    }
                    rockY++;
                }

                Draw(rock, rockX, rockY);
                return this;
            }

            bool Hit(string[] rock, int x, int y)
            {
                // tells if a rock hits the walls of the cave or some other rock

                var (crow, ccol) = (rock.Length, rock[0].Length);
                for (var irow = 0; irow < crow; irow++)
                {
                    for (var icol = 0; icol < ccol; icol++)
                    {
                        if (rock[irow][icol] == '#' && lines[irow + y][icol + x] != ' ')
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            void Draw(string[] rock, int rockX, int rockY)
            {
                // draws a rock pattern into the cave at the given x,y coordinates,

                var (crow, ccol) = (rock.Length, rock[0].Length);
                for (var irow = 0; irow < crow; irow++)
                {
                    var chars = lines[irow + rockY].ToArray();
                    for (var icol = 0; icol < ccol; icol++)
                    {

                        if (rock[irow][icol] == '#')
                        {
                            if (chars[icol + rockX] != ' ')
                            {
                                throw new Exception();
                            }
                            chars[icol + rockX] = '#';
                        }
                    }
                    lines[rockY + irow] = string.Join("", chars);
                }

                // remove empty lines from the top
                while (!lines[0].Contains('#'))
                {
                    lines.RemoveAt(0);
                }

                // keep the tail
                if (lines.Count > linesToStore)
                {
                    var r = lines.Count - linesToStore - 1;
                    lines.RemoveRange(linesToStore, r);
                    linesNotStored += r;
                }
            }

            IEnumerable<T> Loop<T>(IEnumerable<T> items)
            {
                while (true)
                {
                    foreach (var item in items)
                    {
                        yield return item;
                    }
                }
            }

        }
    }
}
