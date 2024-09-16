using Helpers.Helpers;

namespace AdventOfCode.Year2019
{
    internal class Day24 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var history = new List<string> { string.Join("", lines) };

            var lineList = lines.ToList();
            string newState;
            while (true)
            {
                var newLineList = new List<string>();
                for (var y = 0; y < lineList.Count; y++)
                {
                    var newLine = "";
                    for (var x = 0; x < lineList[y].Length; x++)
                    {
                        var bugCount = 0;
                        if (x > 0) bugCount += (lineList[y][x - 1] == '#' ? 1 : 0);
                        if (y > 0) bugCount += (lineList[y - 1][x] == '#' ? 1 : 0);
                        if (x < lineList[y].Length - 1) bugCount += (lineList[y][x + 1] == '#' ? 1 : 0);
                        if (y < lineList.Count - 1) bugCount += (lineList[y + 1][x] == '#' ? 1 : 0);
                        var currentC = lineList[y][x];
                        var newC = (currentC == '#' && bugCount != 1) || (currentC == '.' && (bugCount != 1 && bugCount != 2))
                            ? '.'
                            : '#';
                        newLine += newC;
                    }
                    newLineList.Add(newLine);
                }

                newState = string.Join("", newLineList);
                if (history.Contains(newState)) break;
                history.Add(newState);
                lineList = newLineList;
            }

            var sum = 0L;
            var currentVal = 1L;
            for (var x = 0; x < newState.Length; x++)
            {
                if (newState[x] == '#') sum |= currentVal;
                currentVal <<= 1;
            }
            var expectedResult = 28717468;
            var result = sum;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var initalLayer = lines.Select(l => l.Select(c => c == '#' ? 1 : 0).ToList()).ToList();
            var layers = new Dictionary<int, List<List<int>>> { { 0, initalLayer } };

            for (var i = 0; i < 200; i++)
            {
                var newLayers = new Dictionary<int, List<List<int>>>();

                foreach (var kvp in layers)
                {
                    var layer = layers[kvp.Key];
                    var innerLayer = layers.ContainsKey(kvp.Key + 1) ? layers[kvp.Key + 1] : null;
                    var outerLayer = layers.ContainsKey(kvp.Key - 1) ? layers[kvp.Key - 1] : null;

                    var newLayer = new List<List<int>>();
                    for (var y = 0; y < layer.Count; y++)
                    {
                        var newRow = new List<int>();
                        for (var x = 0; x < layer[0].Count; x++)
                        {
                            if (x == 2 && y == 2)
                            {
                                newRow.Add(0);
                                continue;
                            }

                            var bugCount = 0;
                            if (x == 0 && outerLayer != null) bugCount += outerLayer[2][1];
                            else if (x == 3 && y == 2 && innerLayer != null) bugCount += CountBugsOnOuterEdge(innerLayer, Direction.East);
                            else if (x > 0) bugCount += layer[y][x - 1];

                            if (x == 4 && outerLayer != null) bugCount += outerLayer[2][3];
                            else if (x == 1 && y == 2 && innerLayer != null) bugCount += CountBugsOnOuterEdge(innerLayer, Direction.West);
                            else if (x < 4) bugCount += layer[y][x + 1];

                            if (y == 0 && outerLayer != null) bugCount += outerLayer[1][2];
                            else if (x == 2 && y == 3 && innerLayer != null) bugCount += CountBugsOnOuterEdge(innerLayer, Direction.South);
                            else if (y > 0) bugCount += layer[y - 1][x];

                            if (y == 4 && outerLayer != null) bugCount += outerLayer[3][2];
                            else if (x == 2 && y == 1 && innerLayer != null) bugCount += CountBugsOnOuterEdge(innerLayer, Direction.North);
                            else if (y < 4) bugCount += layer[y + 1][x];

                            var isBug = layer[y][x];
                            if (isBug == 1 && bugCount != 1) isBug = 0;
                            else if (isBug == 0 && (bugCount == 1 || bugCount == 2)) isBug = 1;
                            newRow.Add(isBug);
                        }
                        newLayer.Add(newRow);
                    }

                    newLayers.Add(kvp.Key, newLayer);

                    if (innerLayer == null)
                    {
                        var leftSquare = layer[2][1];
                        var rightSquare = layer[2][3];
                        var topSquare = layer[1][2];
                        var bottomSquare = layer[3][2];

                        if (leftSquare + rightSquare + topSquare + bottomSquare > 0)
                        {
                            var newInnerLayer = MakeEmptyLayer();
                            if (leftSquare == 1 || rightSquare == 1)
                            {
                                foreach (var row in newInnerLayer)
                                {
                                    row[0] = leftSquare;
                                    row[4] = rightSquare;
                                }
                            }

                            if (topSquare == 1 || bottomSquare == 1)
                            {
                                for (var x = 0; x < 5; x++)
                                {
                                    newInnerLayer[0][x] = Math.Max(newInnerLayer[0][x], topSquare);
                                    newInnerLayer[4][x] = Math.Max(newInnerLayer[4][x], bottomSquare);
                                }
                            }
                            newLayers.Add(kvp.Key + 1, newInnerLayer);
                        }
                    }

                    if (outerLayer == null)
                    {
                        var leftBugs = CountBugsOnOuterEdge(layer, Direction.West);
                        var rightBugs = CountBugsOnOuterEdge(layer, Direction.East);
                        var topBugs = CountBugsOnOuterEdge(layer, Direction.North);
                        var bottomBugs = CountBugsOnOuterEdge(layer, Direction.South);

                        if (leftBugs == 1 || leftBugs == 2
                            || rightBugs == 1 || rightBugs == 2
                            || topBugs == 1 || topBugs == 2
                            || bottomBugs == 1 || bottomBugs == 2)
                        {
                            var newOuterLayer = MakeEmptyLayer();
                            if (leftBugs == 1 || leftBugs == 2) newOuterLayer[2][1] = 1;
                            if (rightBugs == 1 || rightBugs == 2) newOuterLayer[2][3] = 1;
                            if (topBugs == 1 || topBugs == 2) newOuterLayer[1][2] = 1;
                            if (bottomBugs == 1 || bottomBugs == 2) newOuterLayer[3][2] = 1;
                            newLayers.Add(kvp.Key - 1, newOuterLayer);
                        }
                    }
                }
                layers = newLayers;
            }

            var expectedResult = 2014;
            var result = layers.Values.Sum(l => l.Sum(r => r.Sum()));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static List<List<int>> MakeEmptyLayer()
        {
            return Enumerable.Range(0, 5).Select(_ => Enumerable.Range(0, 5).Select(_ => 0).ToList()).ToList();
        }

        private static int CountBugsOnOuterEdge(List<List<int>> layer, Direction d)
        {
            return d switch
            {
                Direction.North => layer[0].Sum(),
                Direction.South => layer[4].Sum(),
                Direction.West => layer.Sum(l => l[0]),
                Direction.East => layer.Sum(l => l[4]),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
