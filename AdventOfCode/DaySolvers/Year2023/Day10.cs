using System.Data;

namespace AdventOfCode.Year2023
{
    internal class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var mapDict = new Dictionary<(int, int), MapPoint>();
            var startingPoint = (0, 0);
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    mapDict.Add((x, y), ConvertChar(c));
                    if (c == 'S')
                    {
                        startingPoint = (x, y);
                    }
                }
            }

            return DeterminePath(mapDict, startingPoint).Count / 2;
        }

        public object HardSolution(IList<string> lines)
        {
            var mapDict = new Dictionary<(int x, int y), MapPoint>();
            var startingPoint = (0, 0);
            var maxY = lines.Count;
            var maxX = lines[0].Length;
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    mapDict.Add((x, y), ConvertChar(c));
                    if (c == 'S')
                    {
                        startingPoint = (x, y);
                    }
                }
            }

            var path = DeterminePath(mapDict, startingPoint);

            var enclosureDict = mapDict.ToDictionary(kvp => kvp.Key, kvp => MapEnclosure.Unknown);
            foreach (var p in path)
            {
                enclosureDict[p] = MapEnclosure.Pipe;
            }

            foreach (var kvp in enclosureDict.Where(kvp => kvp.Value == MapEnclosure.Unknown))
            {
                var countNorthFacing = 0;
                for (var x = 0; x < kvp.Key.x; x++)
                {
                    if (!path.Contains((x, kvp.Key.y))) continue;
                    var mapPoint = mapDict[(x, kvp.Key.y)];
                    if (mapPoint == MapPoint.NW || mapPoint == MapPoint.NE || mapPoint == MapPoint.Vertical || mapPoint == MapPoint.Start)
                    {
                        countNorthFacing++;
                    }
                }
                enclosureDict[kvp.Key] = countNorthFacing % 2 == 0 ? MapEnclosure.Outside : MapEnclosure.Inside;
            }

            return enclosureDict.Values.Count(v => v == MapEnclosure.Inside);
        }

        private static List<(int, int)> DeterminePath(Dictionary<(int, int), MapPoint> mapDict, (int, int) startingPoint)
        {
            var path = new List<(int, int)> { startingPoint };
            var point = startingPoint;

            var pointsToCheck = new List<(int x, int y, List<MapPoint> validConnectors, List<MapPoint> pointsToCheckFrom)>
            {
                (
                    -1, 0,
                    new List<MapPoint> { MapPoint.Start, MapPoint.Horizontal, MapPoint.NE, MapPoint.SE },
                    new List<MapPoint> { MapPoint.Start, MapPoint.Horizontal, MapPoint.NW, MapPoint.SW }
                ),
                (
                    0, -1,
                    new List<MapPoint> { MapPoint.Start, MapPoint.Vertical, MapPoint.SW, MapPoint.SE },
                    new List<MapPoint> { MapPoint.Start, MapPoint.Vertical, MapPoint.NW, MapPoint.NE }
                ),
                (
                    1, 0,
                    new List<MapPoint> { MapPoint.Start, MapPoint.Horizontal, MapPoint.NW, MapPoint.SW },
                    new List<MapPoint> { MapPoint.Start, MapPoint.Horizontal, MapPoint.NE, MapPoint.SE }
                ),
                (
                    0, 1,
                    new List<MapPoint> { MapPoint.Start, MapPoint.Vertical, MapPoint.NW, MapPoint.NE },
                    new List<MapPoint> { MapPoint.Start, MapPoint.Vertical, MapPoint.SW, MapPoint.SE }
                ),
            };

            while (true)
            {
                var (x, y) = point;
                var currentMapPoint = mapDict[point];
                foreach (var ptc in pointsToCheck.Where(ptc => ptc.pointsToCheckFrom.Contains(currentMapPoint)))
                {
                    var newPoint = (x + ptc.x, y + ptc.y);
                    if (mapDict.TryGetValue(newPoint, out MapPoint value) && ptc.validConnectors.Contains(value))
                    {
                        if (!path.Contains(newPoint) || (mapDict[newPoint] == MapPoint.Start && path.Count > 3))
                        {
                            if (mapDict[newPoint] != MapPoint.Start)
                            {
                                path.Add(newPoint);
                            }
                            point = newPoint;
                            break;
                        }
                    }
                }
                if (mapDict[point] == MapPoint.Start)
                {
                    break;
                }
            }
            return path;
        }

        private static MapPoint ConvertChar(char c)
        {
            return c switch
            {
                '|' => MapPoint.Vertical,
                '-' => MapPoint.Horizontal,
                'L' => MapPoint.NE,
                'J' => MapPoint.NW,
                '7' => MapPoint.SW,
                'F' => MapPoint.SE,
                'S' => MapPoint.Start,
                _ => MapPoint.Nothing,
            };
        }

        private enum MapPoint
        {
            Vertical,
            Horizontal,
            NE,
            NW,
            SW,
            SE,
            Nothing,
            Start,
        }

        private enum MapEnclosure
        {
            Unknown,
            Pipe,
            Outside,
            Inside,
        }
    }
}
