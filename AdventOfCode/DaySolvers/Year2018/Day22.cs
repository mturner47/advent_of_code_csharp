using Helpers.Helpers;
using System.Runtime.InteropServices;

namespace AdventOfCode.Year2018
{
    internal class Day22 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var depth = 7305;
            var target = (x: 13, y: 734);

            var regions = new Dictionary<(int x, int y), (int erosionLevel, int riskLevel)>();
            for (var y = 0; y <= target.y; y++)
            {
                for (var x = 0; x <= target.x; x++)
                {
                    int geologicIndex;
                    if ((x, y) == target) geologicIndex = 0;
                    else if (x == 0 && y == 0) geologicIndex = 0;
                    else if (x == 0 && y > 0) geologicIndex = y * 48271;
                    else if (y == 0 && x > 0) geologicIndex = x * 16807;
                    else geologicIndex = regions[(x - 1, y)].erosionLevel * regions[(x, y - 1)].erosionLevel;
                    var erosionLevel = (geologicIndex + depth) % 20183;
                    var riskLevel = erosionLevel % 3;
                    regions[(x, y)] = (erosionLevel, riskLevel);
                }
            }

            var expectedResult = 10204;
            var result = regions.Values.Sum(r => r.riskLevel);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var depth = 7305;
            var target = (x: 13, y: 734);

            var regions = new Dictionary<(int x, int y), (int erosionLevel, RegionType regionType)>();
            for (var y = 0; y <= target.y*2; y++)
            {
                for (var x = 0; x <= target.x*2; x++)
                {
                    int geologicIndex;
                    if ((x, y) == target) geologicIndex = 0;
                    else if (x == 0 && y == 0) geologicIndex = 0;
                    else if (x == 0 && y > 0) geologicIndex = y * 48271;
                    else if (y == 0 && x > 0) geologicIndex = x * 16807;
                    else geologicIndex = regions[(x - 1, y)].erosionLevel * regions[(x, y - 1)].erosionLevel;
                    var erosionLevel = (geologicIndex + depth) % 20183;
                    var regionType = erosionLevel % 3;
                    regions[(x, y)] = (erosionLevel, (RegionType)regionType);
                }
            }
            var regionTypes = regions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.regionType);

            var position = (x: 0, y: 0);
            var tool = Tool.Torch;
            var timeSpent = 0;
            var seenCombos = new Dictionary<((int x, int y) position, Tool tool), int> { { (position, tool), timeSpent } };

            var queue = new PriorityQueue<((int x, int y) position, Tool tool, int timeSpent), int>();
            queue.Enqueue((position, tool, timeSpent), timeSpent);
            while (true)
            {
                (position, tool, timeSpent) = queue.Dequeue();
                if (position == target) break;
                var currentRegionType = regionTypes[position];
                foreach (var newPosition in DirectionExtensions.GetAllMovements(position))
                {
                    if (!regionTypes.ContainsKey(newPosition)) continue;
                    var newRegionType = regionTypes[newPosition];

                    var newTool = (currentRegionType, newRegionType, tool) switch
                    {
                        (RegionType.Rocky, RegionType.Wet, Tool.Torch) => Tool.ClimbingGear,
                        (RegionType.Rocky, RegionType.Narrow, Tool.ClimbingGear) => Tool.Torch,
                        (RegionType.Wet, RegionType.Rocky, Tool.Neither) => Tool.ClimbingGear,
                        (RegionType.Wet, RegionType.Narrow, Tool.ClimbingGear) => Tool.Neither,
                        (RegionType.Narrow, RegionType.Rocky, Tool.Neither) => Tool.Torch,
                        (RegionType.Narrow, RegionType.Wet, Tool.Torch) => Tool.Neither,
                        (_, _, _) => tool,
                    };

                    var newTimeSpent = timeSpent + (newTool == tool ? 1 : 8);

                    if (seenCombos.ContainsKey((newPosition, newTool)) && seenCombos[(newPosition, newTool)] <= newTimeSpent) continue;
                    seenCombos[(newPosition, newTool)] = newTimeSpent;

                    if (newPosition == target && newTool == Tool.ClimbingGear) newTimeSpent += 7;

                    queue.Enqueue((newPosition, newTool, newTimeSpent), newTimeSpent);
                }
            }

            var expectedResult = 1004;
            var result = timeSpent;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private enum RegionType
        {
            Rocky = 0,
            Wet = 1,
            Narrow = 2,
        };

        private enum Tool
        {
            Neither,
            Torch,
            ClimbingGear,
        }
    }
}
