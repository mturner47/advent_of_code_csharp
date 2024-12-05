namespace AdventOfCode.Year2020
{
    internal class Day16 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var (rules, _, nearbyTickets) = Parse(lines);

            var invalidValues = new List<int>();
            foreach (var ticketValues in nearbyTickets)
            {
                foreach (var ticketValue in ticketValues)
                {
                    var foundValidRange = false;
                    foreach (var ruleRanges in rules.Values)
                    {
                        foreach (var (lower, upper) in ruleRanges)
                        {
                            if (lower <= ticketValue && upper >= ticketValue)
                            {
                                foundValidRange = true;
                                break;
                            }
                        }
                        if (foundValidRange) break;
                    }
                    if (!foundValidRange) invalidValues.Add(ticketValue);
                }
            }
            
            var expectedResult = 27898;
            var result = invalidValues.Sum();
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var (rules, myTicket, nearbyTickets) = Parse(lines);
            var validTickets = nearbyTickets.Where(nb => IsValidTicket(nb, rules.Values.ToList())).ToList();
            validTickets.Add(myTicket);

            var definiteIndices = new Dictionary<string, List<int>>();
            foreach (var kvp in rules)
            {
                var ruleName = kvp.Key;
                var ruleRanges = kvp.Value;
                var possibleIndicies = Enumerable.Range(0, myTicket.Count).ToList();
                for (var i = 0; i < myTicket.Count; i++)
                {
                    var isValidForIndex = true;
                    foreach (var ticket in validTickets)
                    {
                        var ticketValue = ticket[i];
                        var isValidForTicket = false;
                        foreach (var (lower, upper) in ruleRanges)
                        {
                            if (lower <= ticketValue && upper >= ticketValue)
                            {
                                isValidForTicket = true;
                                break;
                            }
                        }
                        if (!isValidForTicket)
                        {
                            isValidForIndex = false;
                            break;
                        }
                    }
                    if (!isValidForIndex) possibleIndicies.Remove(i);
                }
                definiteIndices.Add(ruleName, possibleIndicies);
            }

            var queue = new Queue<string>();
            foreach (var kvp in definiteIndices.Where(kvp => kvp.Value.Count == 1))
            {
                queue.Enqueue(kvp.Key);
            }

            while (queue.Count > 0)
            {
                var name = queue.Dequeue();
                var definiteIndex = definiteIndices[name][0];
                foreach (var kvp in definiteIndices.Where(kvp => kvp.Key != name))
                {
                    if (kvp.Value.Contains(definiteIndex))
                    {
                        kvp.Value.Remove(definiteIndex);
                        if (kvp.Value.Count == 1) queue.Enqueue(kvp.Key);
                    }
                }
            }

            var product = 1L;
            foreach (var index in definiteIndices.Where(kvp => kvp.Key.StartsWith("departure")).Select(kvp => kvp.Value[0]))
            {
                product *= myTicket[index];
            }
            var expectedResult = 2766491048287;
            var result = product;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static bool IsValidTicket(List<int> ticket, List<List<(int lower, int upper)>> rules)
        {
            foreach (var ticketValue in ticket)
            {
                var isValid = false;
                foreach (var ruleRanges in rules)
                {
                    foreach (var (lower, upper) in ruleRanges)
                    {
                        if (lower <= ticketValue && upper >= ticketValue) isValid = true;
                    }
                }
                if (!isValid) return false;
            }
            return true;
        }

        private static (Dictionary<string, List<(int lower, int upper)>> rules, List<int> myTicket, List<List<int>> nearbyTickets) Parse(IList<string> lines)
        {
            var rules = new Dictionary<string, List<(int lower, int upper)>>();
            var groups = string.Join("\n", lines).Split("\n\n");
            var rulesGroup = groups[0].Split("\n");
            foreach (var rule in rulesGroup)
            {
                var ruleParts = rule.Split(": ");
                var ruleName = ruleParts[0];
                var ruleRanges = ruleParts[1].Split(" or ");
                var ruleList = ruleRanges.Select(rr =>
                {
                    var ruleRangeParts = rr.Split("-");
                    return (lower: int.Parse(ruleRangeParts[0]), upper: int.Parse(ruleRangeParts[1]));
                }).ToList();
                rules.Add(ruleName, ruleList);
            }

            var myTicket = groups[1].Split("\n")[1].Split(",").Select(int.Parse).ToList();

            var nearbyTickets = groups[2].Split("\n").Skip(1).Select(s => s.Split(",").Select(int.Parse).ToList()).ToList();

            return (rules, myTicket, nearbyTickets);
        }
    }
}
