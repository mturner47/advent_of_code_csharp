namespace AdventOfCode.Year2023
{
    internal class Day19 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var rawInput = string.Join('\n', lines);
            var inputParts = rawInput.Split("\n\n");
            var flowDict = inputParts[0].Split("\n").Select(Flow.ParseLine).ToDictionary(f => f.Name, f => f);
            var parts = inputParts[1].Split("\n").Select(ParsePartLine).ToList();

            return parts.Sum(p => GetValue(flowDict, "in", p));
        }

        public object HardSolution(IList<string> lines)
        {
            var rawInput = string.Join('\n', lines);
            var inputParts = rawInput.Split("\n\n");
            var flowDict = inputParts[0].Split("\n").Select(Flow.ParseLine).ToDictionary(f => f.Name, f => f);
            var baseDictionary = new Dictionary<string, (long Min, long Max)>
            {
                { "x", (1, 4000) },
                { "m", (1, 4000) },
                { "a", (1, 4000) },
                { "s", (1, 4000) },
            };
            return GetNumberOfValidRanges(flowDict, flowDict["in"].Conditions, baseDictionary);
        }

        private static long GetNumCombinations(List<(long min, long max)> validRanges)
        {
            return validRanges.Select(v => v.max - v.min + 1).Aggregate(1L, (a, b) => a * b);
        }

        private static long GetNumberOfValidRanges(Dictionary<string, Flow> flowDict, List<Condition> conditions, Dictionary<string, (long min, long max)> validRanges)
        {
            var condition = conditions[0];

            long resultFunc(string result, Dictionary<string, (long min, long max)> vr)
            {
                return result switch
                {
                    "A" => GetNumCombinations(vr.Values.ToList()),
                    "R" => 0,
                    string s => GetNumberOfValidRanges(flowDict, flowDict[s].Conditions, vr),
                };
            }

            switch (condition.Operation)
            {
                case ' ':
                    return resultFunc(condition.Result, validRanges);
                default:
                    var (min, max) = validRanges[condition.RatingName];
                    var (successRange, failRange) = condition.SplitRanges(min, max);
                    var result = 0L;
                    var newValidRanges = validRanges.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    if (successRange.HasValue)
                    {
                        newValidRanges[condition.RatingName] = successRange.Value;
                        result += resultFunc(condition.Result, newValidRanges);
                    }

                    if (failRange.HasValue)
                    {
                        newValidRanges[condition.RatingName] = failRange.Value;
                        result += GetNumberOfValidRanges(flowDict, conditions.Skip(1).ToList(), newValidRanges);
                    }
                    return result;
            }
        }

        public class Flow
        {
            public string Name { get; set; } = "";
            public List<Condition> Conditions { get; set; } = new();

            public static Flow ParseLine(string line)
            {
                var parts = line.Split("{");
                var name = parts[0];
                var conditions = parts[1].Replace("}", "").Split(",").Select(Condition.Parse).ToList();

                return new Flow
                {
                    Name = name,
                    Conditions = conditions,
                };
            }
        }

        public class Condition
        {
            public char Operation { get; set; }
            public string RatingName { get; set; } = "";
            public long Rating { get; set; }
            public string Result { get; set; } = "";

            public string? Evaluate(Dictionary<string, long> partDict)
            {
                return Operation switch
                {
                    '>' => partDict[RatingName] > Rating ? Result : null,
                    '<' => partDict[RatingName] < Rating ? Result : null,
                    _ => Result,
                };
            }

            public ((long min, long max)? successRange, (long min, long max)? failRange) SplitRanges(long min, long max)
            {
                if (Operation == '<')
                {
                    var newSuccessRange = (min, Math.Min(Rating - 1, max));
                    var newFailRange = (Math.Max(Rating, min), max);
                    if (Rating <= min) return (null, newFailRange);
                    if (Rating > max) return (newSuccessRange, null);
                    return (newSuccessRange, newFailRange);
                }
                else
                {
                    var newSuccessRange = (Math.Max(Rating + 1, min), max);
                    var newFailRange = (min, Math.Min(Rating, max)); 
                    if (Rating >= max) return (null, newFailRange);
                    if (Rating < min) return (newSuccessRange, null);
                    return (newSuccessRange, newFailRange);
                }
            }

            public static Condition Parse(string conditionString)
            {
                if (!conditionString.Contains(':')) return new Condition { Result = conditionString, Operation = ' ', };

                var conditionParts = conditionString.Split(":");
                var result = conditionParts[1];
                var comparison = conditionParts[0];
                var operation = comparison.Contains('<') ? '<' : '>';
                var comparisonParts = comparison.Split(operation);
                var ratingName = comparisonParts[0];
                var rating = long.Parse(comparisonParts[1]);

                return new Condition
                {
                    Operation = operation,
                    RatingName = ratingName,
                    Rating = rating,
                    Result = result,
                };
            }
        }

        public static Dictionary<string, long> ParsePartLine(string line)
        {
            return line.Replace("{", "").Replace("}", "").Split(",").Select(s =>
            {
                var ratingParts = s.Split("=");
                return (ratingName: ratingParts[0], rating: long.Parse(ratingParts[1]));
            }).ToDictionary(r => r.ratingName, r => r.rating);
        }

        public static long GetValue(Dictionary<string, Flow> flowDict, string flowName, Dictionary<string, long> partDict)
        {
            var flow = flowDict[flowName];
            foreach (var condition in flow.Conditions)
            {
                var result = condition.Evaluate(partDict);
                if (result == null) continue;
                if (result == "A") return partDict.Values.Sum();
                if (result == "R") return 0;
                return GetValue(flowDict, result, partDict);
            }
            throw new Exception("Something went wrong");
        }
    }
}
