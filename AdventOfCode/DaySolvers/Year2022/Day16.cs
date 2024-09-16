namespace AdventOfCode.Year2022
{
    internal class Day16 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var valves = lines.Select(Valve.ParseInput).ToDictionary(v => v.Name, v => v);
            var edges = GetEdges(valves);

            var importantValves = valves.Values.Where(v => v.FlowRate > 0).Select(v => v.Name).ToList();
            var startingValve = valves["AA"];
            var timeAvailable = 30;
            var startingState = new RouteState
            {
                CurrentValve = startingValve,
                ImportantValves = importantValves,
                Edges = edges,
                Valves = valves,
                TimeLeft = timeAvailable,
            };

            var routes = GetRoutes(startingState);
            var bestRoute = routes.OrderByDescending(x => x.Item2).FirstOrDefault();
            return bestRoute.Item2;
        }

        public object HardSolution(IList<string> lines)
        {
            var valves = lines.Select(Valve.ParseInput).ToDictionary(v => v.Name, v => v);
            var edges = GetEdges(valves);

            var importantValves = valves.Values.Where(v => v.FlowRate > 0).Select(v => v.Name).ToList();
            var startingValve = valves["AA"];
            var timeAvailable = 26;
            var startingState = new RouteState
            {
                CurrentValve = startingValve,
                ImportantValves = importantValves,
                Edges = edges,
                Valves = valves,
                TimeLeft = timeAvailable,
            };

            var routes = GetRoutes(startingState).OrderByDescending(r => r.Item2).ToList();
            var bestRouteCombo = (new List<string>(), new List<string>(), (double)0);
            for (var i = 0; i < routes.Count; i++)
            {
                for (var j = i + 1; j < routes.Count; j++)
                {
                    var (iPath, iCost) = routes[i];
                    var (jPath, jCost) = routes[j];
                    if (iCost + jCost > bestRouteCombo.Item3)
                    {
                        var combinedPaths = iPath.Concat(jPath).ToList();
                        if (combinedPaths.Distinct().Count() == combinedPaths.Count)
                        {
                            bestRouteCombo = (iPath, jPath, iCost + jCost);
                        }
                    }
                }
            }
            Console.WriteLine("Path 1: " + string.Join(", ", bestRouteCombo.Item1));
            Console.WriteLine("Path 2: " + string.Join(", ", bestRouteCombo.Item2));
            return bestRouteCombo.Item3;
        }

        private class Valve
        {
            public string Name { get; set; } = "";
            public List<string> ConnectedValves { get; set; } = [];
            public int FlowRate { get; set; }
            public bool IsOpened { get; set; } = false;

            public static Valve ParseInput(string line)
            {
                line = line.Replace("Valve ", "");
                var parts = line.Split(" has flow rate=");
                var name = parts[0];
                parts[1] = parts[1].Replace("tunnels", "tunnel").Replace("leads", "lead").Replace("valves", "valve");
                parts = parts[1].Split("; tunnel lead to valve ");
                var flowRate = int.Parse(parts[0]);
                var connectedValves = parts[1].Split(", ").ToList();
                return new Valve
                {
                    Name = name,
                    FlowRate = flowRate,
                    ConnectedValves = connectedValves,
                };
            }
        }

        private static Dictionary<(string, string), int> GetEdges(Dictionary<string, Valve> valves)
        {
            var vertices = valves.Keys.OrderBy(v => v).ToList();
            var edges = new Dictionary<(string, string), int>();
            for (var i = 0; i < vertices.Count; i++)
            {
                for (var j = 0; j < vertices.Count; j++)
                {
                    var valve1Name = vertices[i];
                    var valve1 = valves[valve1Name];
                    var valve2Name = vertices[j];
                    var length = 99999999;
                    if (i == j)
                    {
                        length = 0;
                    }
                    else if (valve1.ConnectedValves.Contains(valve2Name))
                    {
                        length = 1;
                    }
                    edges.Add((valve1Name, valve2Name), length);
                }
            }

            var vertexCount = vertices.Count;

            for (var k = 0; k < vertexCount; k++)
            {
                var kName = vertices[k];
                for (var i = 0; i < vertexCount; i++)
                {
                    var iName = vertices[i];
                    for (var j = 0; j < vertexCount; j++)
                    {
                        var jName = vertices[j];
                        if (edges[(iName, jName)] > edges[(iName, kName)] + edges[(kName, jName)])
                        {
                            edges[(iName, jName)] = edges[(iName, kName)] + edges[(kName, jName)];
                        }
                    }
                }
            }
            return edges;
        }

        private class RouteState
        {
            public Valve CurrentValve { get; set; } = new Valve();
            public List<string> ImportantValves { get; set; } = [];
            public Dictionary<(string, string), int> Edges { get; set; } = [];
            public Dictionary<string, Valve> Valves { get; set; } = [];
            public int TimeLeft { get; set; }
        }

        private static List<(List<string>, double)> GetRoutes(RouteState state)
        {
            var cvName = state.CurrentValve.Name;
            var possibleImportantValves = state.ImportantValves.Where(v => state.TimeLeft > state.Edges[(cvName, v)]).ToList();
            if (possibleImportantValves.Count == 0)
            {
                return [(new List<string>(), 0)];
            }

            var routes = possibleImportantValves.Select(v =>
            {
                var distance = state.Edges[(cvName, v)];
                var valve = state.Valves[v];
                var timeToOpen = distance + 1;
                var timeLeft = state.TimeLeft - timeToOpen;
                var pressureReleasedByCurrentValve = timeLeft * valve.FlowRate;
                var newState = new RouteState
                {
                    CurrentValve = valve,
                    ImportantValves = possibleImportantValves.Except(new List<string> { v }).ToList(),
                    Edges = state.Edges,
                    Valves = state.Valves,
                    TimeLeft = timeLeft,
                };
                var results = GetRoutes(newState);
                return results.Select(r =>
                {
                    var (bestRoute, pressureReleased) = r;
                    bestRoute.Insert(0, v);
                    pressureReleased += pressureReleasedByCurrentValve;
                    return (bestRoute, pressureReleased);
                }).ToList();
            }).SelectMany(r => r)
            .ToList();

            routes.Add((new List<string>(), 0));

            return routes;
        }
    }
}
