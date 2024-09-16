
namespace AdventOfCode.Year2022
{
    internal class Day19 : IDaySolver
    {
        delegate void ActionRef1<T1, T2>(ref T1 arg1, T2 arg2);

        private readonly record struct Blueprint(
            int oreBotOreCost,
            int clayBotOreCost,
            int obsidianBotOreCost,
            int obsidianBotClayCost,
            int geodeBotOreCost,
            int geodeBotObsidianCost,
            int maxOreCost
        );

        public object EasySolution(IList<string> lines)
        {
            var blueprints = ParseInput(lines);
            int totalQuality = 0;
            foreach (var (blueprint, i) in blueprints.Select((blueprint, i) => (blueprint, i)))
            {
                totalQuality = checked(totalQuality + (i + 1) * RunBluePrint(blueprint));
            }
            return totalQuality;

        }

        public object HardSolution(IList<string> lines)
        {
            var blueprints = ParseInput(lines);
            int totalQuality = 1;
            foreach (var (blueprint, i) in blueprints.Take(3).Select((blueprint, i) => (blueprint, i)))
            {
                totalQuality *= RunBluePrint(blueprint, 32);
            }
            return totalQuality;
        }

        private static List<Blueprint> ParseInput(IList<string> lines)
        {
            var allBluePrints = new List<Blueprint>();
            foreach (string line in lines)
            {
                string[] parts = line.Split(": ")[1].Split(". ").Select(x => x.Split(" costs ")[1]).ToArray();
                int oreBotOreCost = int.Parse(parts[0].Split(" ")[0]);
                int clayBotOreCost = int.Parse(parts[1].Split(" ")[0]);
                int obsidianBotOreCost = int.Parse(parts[2].Split(" ")[0]);
                int geodeBotOreCost = int.Parse(parts[3].Split(" ")[0]);
                allBluePrints.Add(new Blueprint(
                    oreBotOreCost,
                    clayBotOreCost,
                    obsidianBotOreCost,
                    int.Parse(parts[2].Split(" ")[3]),
                    geodeBotOreCost,
                    int.Parse(parts[3].Split(" ")[3]),
                    Math.Max(oreBotOreCost, Math.Max(clayBotOreCost, Math.Max(obsidianBotOreCost, geodeBotOreCost)))
                ));
            }
            return allBluePrints;
        }

        private int RunBluePrint(Blueprint blueprint, int duration = 24)
        {
            var shouldDebug = false;
            if (shouldDebug) Console.WriteLine(string.Format("Running {0}", blueprint));
            HashSet<((int ore, int clay, int obsidian, int geodes) resources, (int oreBot, int clayBot, int obsidianBot, int geodeBot) botCounts)> allStates = new() {
                ((0, 0, 0, 0), (1, 0, 0, 0))
            };
            HashSet<((int ore, int clay, int obsidian, int geodes) resources, (int oreBot, int clayBot, int obsidianBot, int geodeBot) botCounts)> nextStates = new() { };
            List<ActionRef1<((int, int, int, int), (int, int, int, int)), Blueprint>> buildableBots = new();
            int bestGeodesSoFar = 0;
            int bestGeoBotsSoFar = 0;
            int bestBotsSoFar = 0;

            for (int time = 1; time <= duration; time++)
            {
                foreach (var state in allStates.ToList())
                {
                    buildableBots.Clear();

                    // Work out what bots we can and should build
                    if (state.resources.ore >= blueprint.geodeBotOreCost && state.resources.obsidian >= blueprint.geodeBotObsidianCost)
                    {
                        buildableBots.Add(BuildGeodeBot);
                    }
                    if (
                        state.resources.ore >= blueprint.obsidianBotOreCost &&
                        state.resources.clay >= blueprint.obsidianBotClayCost &&
                        state.botCounts.obsidianBot < blueprint.geodeBotObsidianCost
                    )
                    {
                        buildableBots.Add(BuildObsidianBot);
                    }
                    if (state.resources.ore >= blueprint.clayBotOreCost && state.botCounts.oreBot < blueprint.obsidianBotClayCost)
                    {
                        buildableBots.Add(BuildClayBot);
                    }
                    if (state.resources.ore >= blueprint.oreBotOreCost && state.botCounts.oreBot < blueprint.maxOreCost)
                    {
                        buildableBots.Add(BuildOreBot);
                    }

                    // Work out if we make it into the next generation
                    int currStateBots = state.botCounts.clayBot + state.botCounts.obsidianBot + state.botCounts.geodeBot;
                    if (bestBotsSoFar <= currStateBots || bestBotsSoFar - currStateBots < 10)
                    {
                        nextStates.Add(((
                            state.resources.ore + state.botCounts.oreBot,
                            state.resources.clay + state.botCounts.clayBot,
                            state.resources.obsidian + state.botCounts.obsidianBot,
                            state.resources.geodes + state.botCounts.geodeBot
                        ), (
                            state.botCounts.oreBot,
                            state.botCounts.clayBot,
                            state.botCounts.obsidianBot,
                            state.botCounts.geodeBot
                        )));
                    }

                    // Work out bests
                    bestGeodesSoFar = Math.Max(bestGeodesSoFar, state.resources.geodes);
                    bestGeoBotsSoFar = Math.Max(bestGeoBotsSoFar, state.botCounts.geodeBot);
                    bestBotsSoFar = Math.Max(bestBotsSoFar, currStateBots);

                    foreach (var func in buildableBots)
                    { // bots get built (in new states)
                        ((int ore, int clay, int obsidian, int geodes) resources, (int, int, int, int) botCounts) newState = (
                            (state.resources.ore, state.resources.clay, state.resources.obsidian, state.resources.geodes),
                            (state.botCounts.oreBot, state.botCounts.clayBot, state.botCounts.obsidianBot, state.botCounts.geodeBot)
                        );

                        newState.resources.ore += state.botCounts.oreBot;
                        newState.resources.clay += state.botCounts.clayBot;
                        newState.resources.obsidian += state.botCounts.obsidianBot;
                        newState.resources.geodes += state.botCounts.geodeBot;

                        func(ref newState, blueprint);
                        int newStateBots = state.botCounts.clayBot + state.botCounts.obsidianBot + state.botCounts.geodeBot;
                        if (bestBotsSoFar <= newStateBots || bestBotsSoFar - newStateBots < 10)
                        {
                            nextStates.Add(newState);
                        }
                    } // not building a new bot is also a valid option, that's what we do
                }
                allStates = nextStates;
                nextStates = new();
                allStates.RemoveWhere(state =>
                    bestGeodesSoFar - state.resources.geodes >= 2 ||
                    bestGeoBotsSoFar - state.botCounts.geodeBot >= 2
                );
            }

            var bestState = allStates.MaxBy(x => x.resources.geodes);
            if (shouldDebug) Console.WriteLine("\tBest state " + StateToString(bestState));
            return bestState.resources.geodes;
        }

        private void BuildOreBot(ref ((int ore, int, int, int) resources, (int oreBot, int, int, int) botCounts) state, Blueprint blueprint)
        {
            state.resources.ore -= blueprint.oreBotOreCost;
            state.botCounts.oreBot += 1;
        }

        private void BuildClayBot(ref ((int ore, int, int, int) resources, (int, int clayBot, int, int) botCounts) state, Blueprint blueprint)
        {
            state.resources.ore -= blueprint.clayBotOreCost;
            state.botCounts.clayBot += 1;
        }

        private void BuildObsidianBot(ref ((int ore, int clay, int, int) resources, (int, int, int obsidianBot, int) botCounts) state, Blueprint blueprint)
        {
            state.resources.ore -= blueprint.obsidianBotOreCost;
            state.resources.clay -= blueprint.obsidianBotClayCost;
            state.botCounts.obsidianBot += 1;
        }

        private void BuildGeodeBot(ref ((int ore, int, int obsidian, int) resources, (int, int, int, int geodeBot) botCounts) state, Blueprint blueprint)
        {
            state.resources.ore -= blueprint.geodeBotOreCost;
            state.resources.obsidian -= blueprint.geodeBotObsidianCost;
            state.botCounts.geodeBot += 1;
        }

        protected string StateToString(((int, int, int, int) resources, (int, int, int, int) botCounts) state)
        {
            return "{ bots: " + string.Join(", ", state.botCounts) + "; resources: " + string.Join(", ", state.resources) + " }";
        }
    }
}
