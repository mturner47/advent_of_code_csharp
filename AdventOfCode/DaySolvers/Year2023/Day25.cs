using System.Runtime.ConstrainedExecution;

namespace AdventOfCode.Year2023
{
    internal class Day25 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var componentDict = lines.Select(ParseLine).ToDictionary(c => c.Name, c => c);
            //foreach (var name in componentDict.Keys.ToList())
            //{
            //    var component = componentDict[name];
            //    foreach (var connectedComponentName in component.ConnectedComponents)
            //    {
            //        if (!componentDict.TryGetValue(connectedComponentName, out Component? connectedComponent))
            //        {
            //            connectedComponent = new Component { Name = connectedComponentName };
            //            componentDict.Add(connectedComponentName, connectedComponent);
            //        }

            //        if (!connectedComponent.ConnectedComponents.Contains(name)) connectedComponent.ConnectedComponents.Add(name);
            //    }
            //}

            var sourceComponent = componentDict.First().Value;
            var edges = componentDict.Values.SelectMany(c => c.ConnectedComponents.Select(cc => (c1: c.Name, c2: cc))).ToList();

            List<List<(string c1, string c2)>> pathsContainingEdgesToRemove = [];
            var sourceName = sourceComponent.Name;
            var destinationName = "";
            foreach (var destinationComponent in componentDict.Values.ToList())
            {
                if (destinationComponent.Name == sourceComponent.Name) continue;
                destinationName = destinationComponent.Name;
                var innerEdges = edges.Select(e => e).ToList();
                pathsContainingEdgesToRemove = [];

                for (var i = 0; i < 3; i++)
                {
                    var pathDict = FindShortestPath(innerEdges, sourceName, destinationName);
                    var destinationShortestPathNode = pathDict[destinationName];
                    var path = destinationShortestPathNode.ShortestPath;
                    pathsContainingEdgesToRemove.Add(path);
                    foreach (var (c1, c2) in path)
                    {
                        innerEdges.Remove((c1, c2));
                        innerEdges.Remove((c2, c1));
                    }
                }

                var finalPath = FindShortestPath(innerEdges, sourceName, destinationName);
                if (!finalPath[destinationName].Found) break;
            }

            foreach (var (e1Source, e1Dest) in pathsContainingEdgesToRemove[0])
            {
                foreach (var (e2Source, e2Dest) in pathsContainingEdgesToRemove[1])
                {
                    foreach (var (e3Source, e3Dest) in pathsContainingEdgesToRemove[2])
                    {
                        var innerEdges = edges.Select(e => e).ToList();
                        innerEdges.Remove((e1Source, e1Dest));
                        innerEdges.Remove((e1Dest, e1Source));
                        innerEdges.Remove((e2Source, e2Dest));
                        innerEdges.Remove((e2Dest, e2Source));
                        innerEdges.Remove((e3Source, e3Dest));
                        innerEdges.Remove((e3Dest, e3Source));
                        var pathDict = FindShortestPath(innerEdges, sourceName, destinationName);
                        if (!pathDict[destinationName].Found)
                        {
                            return pathDict.Values.Count(n => n.Found) * pathDict.Values.Count(n => !n.Found);
                        }
                    }
                }
            }
            throw new NotImplementedException();
        }

        public object HardSolution(IList<string> lines)
        {
            return 0;
        }

        private class Component
        {
            public string Name { get; set; } = "";
            public List<string> ConnectedComponents { get; set; } = [];
        }

        private static Component ParseLine(string line)
        {
            var parts = line.Split(": ");
            var name = parts[0];
            var connectedComponents = parts[1].Split(" ").ToList();
            return new Component
            {
                Name = name,
                ConnectedComponents = connectedComponents,
            };
        }

        private static Dictionary<string, ShortestPathNode> FindShortestPath(List<(string c1, string c2)> edges, string source, string destination)
        {
            var nodeDictionary = edges.Select(e => e.c1).Union(edges.Select(e => e.c2)).Distinct()
                .ToDictionary(c => c, c => new ShortestPathNode
                {
                    Name = c,
                    Found = false,
                    MinLength = c == source ? 0 : int.MaxValue,
                    ShortestPath = [],
                });

            while (!nodeDictionary[destination].Found)
            {
                var minNode = nodeDictionary.Values.Where(n => !n.Found && n.MinLength < int.MaxValue).OrderBy(n => n.MinLength).FirstOrDefault();
                if (minNode == null) break;
                nodeDictionary[minNode.Name].Found = true;

                var connectedNodes = edges.Where(e => e.c1 == minNode.Name).Select(e => e.c2)
                    .Union(edges.Where(e => e.c2 == minNode.Name).Select(e => e.c1)).Distinct();
                foreach (var connectedNodeName in connectedNodes)
                {
                    var connectedNode = nodeDictionary[connectedNodeName];
                    if (connectedNode.MinLength > (minNode.MinLength + 1))
                    {
                        connectedNode.MinLength = minNode.MinLength + 1;
                        var path = minNode.ShortestPath.ToList();
                        path.Add((minNode.Name, connectedNode.Name));
                        connectedNode.ShortestPath = path;
                    }
                }
            }

            return nodeDictionary;
        }

        private class ShortestPathNode
        {
            public string Name { get; set; } = "";
            public bool Found { get; set; } = false;
            public int MinLength = int.MaxValue;
            public List<(string c1, string c2)> ShortestPath = [];
        }
    }
}
