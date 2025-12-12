using Helpers.Extensions;
using Helpers.Helpers;

namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day08 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var points = lines.Select(Parse).ToArray();

            var queue = new PriorityQueue<(int a, int b), double>();
            for (var a = 0; a < points.Length; a++)
            {
                for (var b = a + 1; b < points.Length; b++)
                {
                    queue.Enqueue((a, b), MathHelpers.Get3dDistance(points[a], points[b]));
                }
            }

            var connectedPoints = Enumerable.Range(0, points.Length).Select(p => new List<int> { p }).ToList();

            var count = 0;
            while (queue.Count > 0)
            {
                var (a, b) = queue.Dequeue();

                var setWithA = -1;
                var setWithB = -1;
                for (var i = 0; i < connectedPoints.Count; i++)
                {
                    var cp = connectedPoints[i];
                    if (cp.Contains(a))
                    {
                        setWithA = i;
                    }

                    if (cp.Contains(b))
                    {
                        setWithB = i;
                    }
                }

                if (setWithA == -1 && setWithB == -1)
                {
                    connectedPoints.Add([a, b]);
                }
                else if (setWithA == -1)
                {
                    connectedPoints[setWithB].Add(a);
                }
                else if (setWithB == -1)
                {
                    connectedPoints[setWithA].Add(b);
                }
                else if (setWithA != setWithB)
                {
                    connectedPoints[setWithA].AddRange(connectedPoints[setWithB]);
                    connectedPoints.RemoveAt(setWithB);
                }

                count++;
                if (count == 1000) break;
            }
            var sizes = connectedPoints.Select(cp => cp.Count).OrderByDescending(x => x).Take(3);

            var expectedResult = 69192;
            var result = sizes.Product();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var points = lines.Select(Parse).ToArray();

            var queue = new PriorityQueue<(int a, int b), double>();
            for (var a = 0; a < points.Length; a++)
            {
                for (var b = a + 1; b < points.Length; b++)
                {
                    queue.Enqueue((a, b), MathHelpers.Get3dDistance(points[a], points[b]));
                }
            }

            var connectedPoints = Enumerable.Range(0, points.Length).Select(p => new List<int> { p }).ToList();

            var answer = 0d;
            while (queue.Count > 0)
            {
                var (a, b) = queue.Dequeue();

                var setWithA = -1;
                var setWithB = -1;
                for (var i = 0; i < connectedPoints.Count; i++)
                {
                    var cp = connectedPoints[i];
                    if (cp.Contains(a))
                    {
                        setWithA = i;
                    }

                    if (cp.Contains(b))
                    {
                        setWithB = i;
                    }
                }

                if (setWithA == -1 && setWithB == -1)
                {
                    connectedPoints.Add([a, b]);
                }
                else if (setWithA == -1)
                {
                    connectedPoints[setWithB].Add(a);
                }
                else if (setWithB == -1)
                {
                    connectedPoints[setWithA].Add(b);
                }
                else if (setWithA != setWithB)
                {
                    connectedPoints[setWithA].AddRange(connectedPoints[setWithB]);
                    connectedPoints.RemoveAt(setWithB);
                }

                if (connectedPoints.Count == 1)
                {
                    answer = points[a].x * points[b].x;
                    break;
                }
            }

            var expectedResult = 7264308110d;
            var result = answer;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (double x, double y, double z) Parse(string line)
        {
            var parts = line.Split(",");
            return (parts[0].ToDouble(), parts[1].ToDouble(), parts[2].ToDouble());
        }
    }
}
