using Helpers.Helpers;

namespace AdventOfCode.Year2023
{
    internal class Day20 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var nodes = Node.ParseLines(lines);

            var globalStateHistory = new List<GlobalState> { GlobalState.InitialState(nodes) };
            var buttonPresses = 0;

            while (buttonPresses < 1000)
            {
                buttonPresses++;
                var pulses = new List<(bool isHigh, string toName)>
                {
                    (false, "broadcaster"),
                };

                while (pulses.Count > 0)
                {
                    var nextPulses = new List<(bool isHigh, string toName)>();

                    var currentGlobalState = globalStateHistory.Last();
                    var nextGlobalState = GlobalState.NextGlobalState(currentGlobalState);
                    globalStateHistory.Add(nextGlobalState);
                    currentGlobalState.IncomingHighPulseCount = pulses.Count(p => p.isHigh);
                    currentGlobalState.IncomingLowPulseCount = pulses.Count(p => !p.isHigh);

                    foreach (var (isHigh, toName) in pulses)
                    {
                        if (!nodes.ContainsKey(toName)) continue;
                        var node = nodes[toName];
                        var nodeState = currentGlobalState.NodeStates[toName];
                        if (node.NodeType == NodeType.Broadcaster)
                        {
                            foreach (var name in node.Destinations)
                            {
                                nextPulses.Add((isHigh, name));
                                if (!nodes.ContainsKey(name)) continue;

                                if (nodes[name].NodeType == NodeType.Conjunction)
                                {
                                    nextGlobalState.NodeStates[name].ConjunctionHighPulsesSeen[toName] = isHigh;
                                }
                                else if (!isHigh && nodes[name].NodeType == NodeType.FlipFlop)
                                {
                                    nextGlobalState.NodeStates[name].FlipFlopIsOn = !currentGlobalState.NodeStates[name].FlipFlopIsOn;
                                }
                            }
                        }
                        else if (node.NodeType == NodeType.FlipFlop)
                        {
                            if (!isHigh)
                            {
                                var flipFlopIsOn = nodeState.FlipFlopIsOn;
                                foreach (var name in node.Destinations)
                                {
                                    nextPulses.Add((flipFlopIsOn, name));

                                    if (!nodes.ContainsKey(name)) continue;
                                    if (nodes[name].NodeType == NodeType.Conjunction)
                                    {
                                        nextGlobalState.NodeStates[name].ConjunctionHighPulsesSeen[toName] = flipFlopIsOn;
                                    }
                                    else if (!flipFlopIsOn && nodes[name].NodeType == NodeType.FlipFlop)
                                    {
                                        nextGlobalState.NodeStates[name].FlipFlopIsOn = !currentGlobalState.NodeStates[name].FlipFlopIsOn;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var allHigh = nodeState.ConjunctionHighPulsesSeen.Values.Any(v => v == false);
                            foreach (var name in node.Destinations)
                            {
                                nextPulses.Add((allHigh, name));

                                if (!nodes.ContainsKey(name)) continue;
                                if (nodes[name].NodeType == NodeType.Conjunction)
                                {
                                    nextGlobalState.NodeStates[name].ConjunctionHighPulsesSeen[toName] = allHigh;
                                }
                                else if (!allHigh && nodes[name].NodeType == NodeType.FlipFlop)
                                {
                                    nextGlobalState.NodeStates[name].FlipFlopIsOn = !currentGlobalState.NodeStates[name].FlipFlopIsOn;
                                }
                            }
                        }
                    }
                    pulses = nextPulses;
                }
                if (GlobalState.AreEqual(globalStateHistory[0], globalStateHistory.Last())) break;
            }

            var numRepeats = 1000 / buttonPresses;
            long numLowPulses = numRepeats * globalStateHistory.Sum(gs => gs.IncomingLowPulseCount);
            long numHighPulses = numRepeats * globalStateHistory.Sum(gs => gs.IncomingHighPulseCount);
            return numLowPulses * numHighPulses;
        }

        public object HardSolution(IList<string> lines)
        {
            var nodes = Node.ParseLines(lines);
            var keyComponents = FindKeyComponents(nodes);
            var parentNodes = new[] { "gt", "db" };
            var numPresses = keyComponents.Keys.Select(pn => GetNumPresses(nodes, pn)).ToList();

            return numPresses.Aggregate(1L, MathHelpers.LeastCommonMultiplier);
        }

        private static Dictionary<string, long> FindKeyComponents(Dictionary<string, Node> nodes)
        {
            var keyComponents = new Dictionary<string, long>();
            var queue = new Queue<string>();
            queue.Enqueue("rx");

            while (queue.Count > 0)
            {
                var nodeName = queue.Dequeue();
                var node = nodes[nodeName];
                if (!node.Inputs.Any(s => nodes[s].NodeType == NodeType.Conjunction))
                {
                    keyComponents.Add(nodeName, 0);
                }
                else
                {
                    foreach (var nodeInput in node.Inputs)
                    {
                        queue.Enqueue(nodeInput);
                    }
                }
            }
            return keyComponents;
        }

        private static long GetNumPresses(Dictionary<string, Node> nodes, string input)
        {
            var globalStateHistory = new List<GlobalState> { GlobalState.InitialState(nodes) };
            var buttonPresses = 0;

            while (true)
            {
                buttonPresses++;
                var pulses = new List<(bool isHigh, string toName)>
                {
                    (false, "broadcaster"),
                };

                while (pulses.Count > 0)
                {
                    var nextPulses = new List<(bool isHigh, string toName)>();
                    var currentGlobalState = globalStateHistory.Last();
                    var nextGlobalState = GlobalState.NextGlobalState(currentGlobalState);
                    globalStateHistory.Add(nextGlobalState);
                    currentGlobalState.IncomingHighPulseCount = pulses.Count(p => p.isHigh);
                    currentGlobalState.IncomingLowPulseCount = pulses.Count(p => !p.isHigh);

                    foreach (var (isHigh, toName) in pulses)
                    {
                        if (!nodes.ContainsKey(toName)) continue;
                        var node = nodes[toName];
                        var nodeState = currentGlobalState.NodeStates[toName];
                        if (node.NodeType == NodeType.Broadcaster)
                        {
                            foreach (var name in node.Destinations)
                            {
                                nextPulses.Add((isHigh, name));
                                if (!nodes.ContainsKey(name)) continue;

                                if (nodes[name].NodeType == NodeType.Conjunction)
                                {
                                    nextGlobalState.NodeStates[name].ConjunctionHighPulsesSeen[toName] = isHigh;
                                }
                                else if (!isHigh && nodes[name].NodeType == NodeType.FlipFlop)
                                {
                                    nextGlobalState.NodeStates[name].FlipFlopIsOn = !currentGlobalState.NodeStates[name].FlipFlopIsOn;
                                }
                            }
                        }
                        else if (node.NodeType == NodeType.FlipFlop)
                        {
                            if (!isHigh)
                            {
                                var flipFlopIsOn = nodeState.FlipFlopIsOn;
                                foreach (var name in node.Destinations)
                                {
                                    nextPulses.Add((flipFlopIsOn, name));

                                    if (!nodes.ContainsKey(name)) continue;
                                    if (nodes[name].NodeType == NodeType.Conjunction)
                                    {
                                        nextGlobalState.NodeStates[name].ConjunctionHighPulsesSeen[toName] = flipFlopIsOn;
                                    }
                                    else if (!flipFlopIsOn && nodes[name].NodeType == NodeType.FlipFlop)
                                    {
                                        nextGlobalState.NodeStates[name].FlipFlopIsOn = !currentGlobalState.NodeStates[name].FlipFlopIsOn;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var allHigh = nodeState.ConjunctionHighPulsesSeen.Values.All(v => v);
                            var sendHigh = !allHigh;
                            if (toName == input && allHigh)
                            {
                                return buttonPresses;
                            }
                            foreach (var name in node.Destinations)
                            {
                                nextPulses.Add((sendHigh, name));

                                if (!nodes.ContainsKey(name)) continue;
                                if (nodes[name].NodeType == NodeType.Conjunction)
                                {
                                    nextGlobalState.NodeStates[name].ConjunctionHighPulsesSeen[toName] = sendHigh;
                                }
                                else if (!sendHigh && nodes[name].NodeType == NodeType.FlipFlop)
                                {
                                    nextGlobalState.NodeStates[name].FlipFlopIsOn = !currentGlobalState.NodeStates[name].FlipFlopIsOn;
                                }
                            }
                        }
                    }
                    pulses = nextPulses;
                }
            }
        }

        private class GlobalState
        {
            public Dictionary<string, NodeState> NodeStates { get; set; } = new();
            public int IncomingLowPulseCount { get; set; }
            public int IncomingHighPulseCount { get; set; }

            public static GlobalState InitialState(Dictionary<string, Node> nodes)
            {
                return new GlobalState
                {
                    NodeStates = nodes.ToDictionary(n => n.Key, n => NodeState.GetInitialState(n.Value)),
                    IncomingLowPulseCount = 0,
                    IncomingHighPulseCount = 0,
                };
            }

            public static GlobalState NextGlobalState(GlobalState original)
            {
                var nodeStates = original.NodeStates.ToDictionary(ns => ns.Key, ns => NodeState.CopyNodeState(ns.Value));
                return new GlobalState
                {
                    NodeStates = nodeStates,
                    IncomingLowPulseCount = 0,
                    IncomingHighPulseCount = 0,
                };
            }

            public static bool AreEqual(GlobalState first, GlobalState second)
            {
                foreach (var key in first.NodeStates.Keys)
                {
                    if (!NodeState.AreEqual(first.NodeStates[key], second.NodeStates[key])) return false;
                }
                return true;
            }
        }

        private class NodeState
        {
            public NodeType NType { get; set; }
            public bool FlipFlopIsOn { get; set; }
            public Dictionary<string, bool> ConjunctionHighPulsesSeen = new();

            public static NodeState GetInitialState(Node node)
            {
                if (node.NodeType == NodeType.FlipFlop)
                {
                    return new NodeState { NType = node.NodeType, FlipFlopIsOn = false };
                }

                if (node.NodeType == NodeType.Conjunction)
                {
                    return new NodeState { NType = node.NodeType, ConjunctionHighPulsesSeen = node.Inputs.ToDictionary(i => i, i => false) };
                }

                return new NodeState { NType = node.NodeType };
            }

            public static NodeState CopyNodeState(NodeState state)
            {
                if (state.NType == NodeType.FlipFlop) return new NodeState { NType = state.NType, FlipFlopIsOn = state.FlipFlopIsOn };
                if (state.NType == NodeType.Conjunction) return new NodeState { NType = state.NType, ConjunctionHighPulsesSeen = state.ConjunctionHighPulsesSeen.ToDictionary(c => c.Key, c => c.Value) };

                return new NodeState { NType = NodeType.Broadcaster };
            }

            public static bool AreEqual(NodeState first, NodeState second)
            {
                if (first.NType == NodeType.FlipFlop && first.FlipFlopIsOn == second.FlipFlopIsOn) return true;
                if (first.NType == NodeType.Conjunction)
                {
                    foreach (var key in first.ConjunctionHighPulsesSeen.Keys)
                    {
                        if (first.ConjunctionHighPulsesSeen[key] != second.ConjunctionHighPulsesSeen[key]) return false;
                    }
                    return true;
                }
                return true;
            }
        }

        private class Node
        {
            public string Name { get; set; } = "";
            public NodeType NodeType { get; set; }
            public List<string> Destinations { get; set; } = new();

            public List<string> Inputs { get; set; } = new();
            public List<(int tick, bool isHigh, bool sentPulse)> History = new();

            public static Dictionary<string, Node> ParseLines(IList<string> lines)
            {
                var nodes = lines.Select(Node.ParseLine).ToDictionary(n => n.Name);
                nodes.Add("rx", new Node { Destinations = new(), Name = "rx", NodeType = NodeType.Broadcaster });
                foreach (var node in nodes.Values)
                {
                    foreach (var destination in node.Destinations)
                    {
                        if (nodes.TryGetValue(destination, out Node? destinationNode))
                        {
                            destinationNode.Inputs.Add(node.Name);
                        }
                    }
                }
                return nodes;
            }

            private static Node ParseLine(string line)
            {
                var parts = line.Split(" -> ");
                var namePart = parts[0];
                var nodeType = NodeType.Broadcaster;
                var name = namePart;
                if (namePart.StartsWith("%"))
                {
                    nodeType = NodeType.FlipFlop;
                    name = namePart[1..];
                }

                if (namePart.StartsWith("&"))
                {
                    nodeType = NodeType.Conjunction;
                    name = namePart[1..];
                }

                var destinations = parts[1].Split(", ").ToList();
                return new Node
                {
                    Name = name,
                    NodeType = nodeType,
                    Destinations = destinations,
                    History = new List<(int tick, bool isHigh, bool sentPulse)> { (0, false, false) },
                };
            }
        }

        private enum NodeType
        {
            FlipFlop,
            Conjunction,
            Broadcaster,
        }
    }
}
