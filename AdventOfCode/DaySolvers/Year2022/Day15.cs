namespace AdventOfCode.Year2022
{
    internal class Day15 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var sensors = lines.Select(Sensor.ParseLine).ToList();
            var blockedRanges = new List<(double, double)>();
            var yToCheck = 2000000;
            foreach (var sensor in sensors)
            {
                var (x, y) = sensor.SensorCoords;
                var yDistance = Math.Abs(yToCheck - y);
                var maxXDistance = sensor.Distance - yDistance;
                if (maxXDistance > 0)
                {
                    blockedRanges.Add((x - maxXDistance, x + maxXDistance));
                }
            }

            var numBeaconsInCheckedLine = sensors
                .Select(s => s.ClosestBeaconCoords)
                .Where(cb => cb.Item2 == yToCheck)
                .Select(cb => cb.Item1)
                .Where(cbx => blockedRanges.Any(br => br.Item1 <= cbx && br.Item2 >= cbx))
                .Distinct()
                .Count();

            var ranges = new List<(double, double)>();
            foreach (var br in blockedRanges)
            {
                ranges = ranges.Concat(GetUncoveredRange(br, ranges)).ToList();
            }
            var totalBlockedSpots = ranges.Select(r => 1 + (r.Item2 - r.Item1)).Sum();
            return totalBlockedSpots - numBeaconsInCheckedLine;
        }

        public object HardSolution(IList<string> lines)
        {
            var sensors = lines.Select(Sensor.ParseLine).ToList();
            var maxY = 4000000;
            for (var yToCheck = 0; yToCheck <= maxY; yToCheck++)
            {
                var blockedRanges = new List<(double, double)>();
                foreach (var sensor in sensors)
                {
                    var (x, y) = sensor.SensorCoords;
                    var yDistance = Math.Abs(yToCheck - y);
                    var maxXDistance = sensor.Distance - yDistance;
                    if (maxXDistance > 0)
                    {
                        var lowerRange = Math.Max(x - maxXDistance, 0);
                        var upperRange = Math.Min(x + maxXDistance, maxY);
                        blockedRanges.Add((lowerRange, upperRange));
                    }
                }
                var ranges = new List<(double, double)>();
                foreach (var br in blockedRanges)
                {
                    ranges = ranges.Concat(GetUncoveredRange(br, ranges)).ToList();
                }
                var totalBlockedSpots = ranges.Select(r => 1 + (r.Item2 - r.Item1)).Sum();
                if (totalBlockedSpots < maxY + 1)
                {
                    ranges = ranges.OrderBy(r => r.Item1).ToList();
                    for (var rIndex = 0; rIndex < ranges.Count - 1; rIndex++)
                    {
                        if (ranges[rIndex].Item2 + 1 < ranges[rIndex + 1].Item1)
                        {
                            return (ranges[rIndex].Item2 + 1) * 4000000 + yToCheck;
                        }
                    }
                }
            }
            return 0;
        }

        private class Sensor
        {
            public (double, double) SensorCoords { get; set; }
            public (double, double) ClosestBeaconCoords { get; set; }
            public double Distance { get; set; }

            public static double GetManhattanDistance((double, double) c1, (double, double) c2)
            {
                var (x1, y1) = c1;
                var (x2, y2) = c2;
                return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
            }

            public static Sensor ParseLine(string line)
            {
                line = line.Replace("Sensor at x=", "");
                var parts = line.Split(": closest beacon is at x=");
                var sensorCoordsParts = parts[0].Split(", y=");
                var sensorCoords = (double.Parse(sensorCoordsParts[0]), double.Parse(sensorCoordsParts[1]));

                var cbCoordsParts = parts[1].Split(", y=");
                var cbCoords = (double.Parse(cbCoordsParts[0]), double.Parse(cbCoordsParts[1]));

                return new Sensor
                {
                    SensorCoords = sensorCoords,
                    ClosestBeaconCoords = cbCoords,
                    Distance = Sensor.GetManhattanDistance(sensorCoords, cbCoords),
                };
            }
        }

        private List<(double, double)> GetUncoveredRange((double, double) range, List<(double, double)> countedRanges)
        {
            if (!countedRanges.Any()) return new List<(double, double)> { range };
            var restOfRanges = countedRanges.Skip(1).ToList();
            var (rx1, rx2) = range;
            var (cx1, cx2) = countedRanges[0];
            if (rx1 >= cx1 && rx2 <= cx2) return new List<(double, double)>();
            if (rx2 < cx1 || rx1 > cx2) return GetUncoveredRange(range, restOfRanges);
            if (rx1 < cx1 && rx2 > cx2)
            {
                var newRange1 = (rx1, cx1 - 1);
                var newRange2 = (cx2 + 1, rx2);
                return GetUncoveredRange(newRange1, restOfRanges).Concat(GetUncoveredRange(newRange2, restOfRanges)).ToList();
            }

            if (rx1 < cx1)
            {
                var newRange = (rx1, cx1 - 1);
                return GetUncoveredRange(newRange, restOfRanges);
            }

            if (rx2 > cx2)
            {
                var newRange = (cx2 + 1, rx2);
                return GetUncoveredRange(newRange, restOfRanges);
            }

            return new List<(double, double)> { range };
        }
    }
}
