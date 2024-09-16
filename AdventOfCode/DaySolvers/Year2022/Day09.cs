namespace AdventOfCode.Year2022
{
    internal class Day09 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var visitedLocations = new List<(int, int)>
            {
                (0,0),
            };

            var headLocation = (0, 0);
            var tailLocation = (0, 0);
            foreach (var line in lines)
            {
                var parts = line.Split(" ");
                var direction = parts[0];
                var stepCount = int.Parse(parts[1]);
                for (var i = 0; i < stepCount; i++)
                {
                    var newHeadLocation = GetNewHeadLocation(headLocation, direction);
                    tailLocation = GetNewTailLocation(tailLocation, newHeadLocation);
                    visitedLocations.Add(tailLocation);
                    headLocation = newHeadLocation;
                }
            }

            return visitedLocations.Distinct().Count();
        }

        public object HardSolution(IList<string> lines)
        {
            var visitedLocations = new List<(int, int)>
            {
                (0,0),
            };
            var knotLocations = Enumerable.Range(0, 10).Select(x => (0, 0)).ToList();

            foreach (var line in lines)
            {
                var parts = line.Split(" ");
                var direction = parts[0];
                var stepCount = int.Parse(parts[1]);
                for (var i = 0; i < stepCount; i++)
                {
                    var newKnotLocations = Enumerable.Range(0, 10).Select(x => (0, 0)).ToList();
                    newKnotLocations[0] = GetNewHeadLocation(knotLocations[0], direction);

                    for (var j = 1; j < knotLocations.Count; j++)
                    {
                        newKnotLocations[j] = GetNewTailLocation(knotLocations[j], newKnotLocations[j - 1]);
                    }
                    visitedLocations.Add(newKnotLocations.Last());
                    knotLocations = newKnotLocations;
                }
            }

            return visitedLocations.Distinct().Count();

        }

        private static (int, int) GetNewHeadLocation((int, int) headLocation, string direction)
        {
            var (x, y) = headLocation;
            return direction switch
            {
                "R" => (x + 1, y),
                "L" => (x - 1, y),
                "U" => (x, y + 1),
                "D" => (x, y - 1),
                _ => headLocation,
            };
        }

        private static (int, int) GetNewTailLocation((int, int) tailLocation, (int, int) newHeadLocation)
        {
            var (tailX, tailY) = tailLocation;
            var (newHeadX, newHeadY) = newHeadLocation;

            if (newHeadX - tailX > 1 && newHeadY - tailY > 1) return (newHeadX - 1, newHeadY - 1);
            if (tailX - newHeadX > 1 && tailY - newHeadY > 1) return (newHeadX + 1, newHeadY + 1);
            if (newHeadX - tailX > 1 && tailY - newHeadY > 1) return (newHeadX - 1, newHeadY + 1);
            if (tailX - newHeadX > 1 && newHeadY - tailY > 1) return (newHeadX + 1, newHeadY - 1);

            if (newHeadX - tailX > 1) return (newHeadX - 1, newHeadY);
            if (newHeadY - tailY > 1) return (newHeadX, newHeadY - 1);

            if (tailX - newHeadX > 1) return (newHeadX + 1, newHeadY);
            if (tailY - newHeadY > 1) return (newHeadX, newHeadY + 1);
            return tailLocation;
        }
    }
}
