namespace AdventOfCode.Year2023
{
    internal class Day05 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var fullInput = string.Join(Environment.NewLine, lines);
            var sections = fullInput.Split(Environment.NewLine + Environment.NewLine);
            var seeds = sections[0].Replace("seeds: ", "").Split(" ").Select(double.Parse);
            var mappingSections = sections[1..].Select(ConvertSectionToMap).ToDictionary(ms => ms.SourceType);

            var locations = seeds.Select(s =>
            {
                var type = "seed";

                while (type != "location")
                {
                    var ms = mappingSections[type];
                    s = ms.ConvertFromSourceToDestination(s);
                    type = ms.DestinationType;
                }
                return s;
            }).ToList();
            return locations.Min();
        }

        public object HardSolution(IList<string> lines)
        {
            var fullInput = string.Join(Environment.NewLine, lines);
            var sections = fullInput.Split(Environment.NewLine + Environment.NewLine);
            var individualSeeds = sections[0].Replace("seeds: ", "").Split(" ");

            var ranges = new List<SourceRange>();
            for (var i = 0; i < individualSeeds.Length - 1; i += 2)
            {
                var startSeed = double.Parse(individualSeeds[i]);
                var seedRange = double.Parse(individualSeeds[i + 1]);
                ranges.Add(new SourceRange { Start = startSeed, End = startSeed + seedRange });
            }

            var mappingSections = sections[1..].Select(ConvertSectionToMap).ToDictionary(ms => ms.SourceType);

            var type = "seed";

            while (type != "location")
            {
                var ms = mappingSections[type];
                ranges = ms.ConvertRangesFromSourceToDestination(ranges);
                type = ms.DestinationType;
            }

            return ranges.Select(sr => sr.Start).Min();
        }

        private MappingSection ConvertSectionToMap(string sectionString)
        {
            var sectionStrings = sectionString.Split(Environment.NewLine);
            var mapLabel = sectionStrings[0];
            var types = mapLabel.Replace(" map:", "").Split("-to-");
            var sourceType = types[0];
            var destinationType = types[1];

            var mappings = sectionStrings[1..].Select(s =>
            {
                var parts = s.Split(" ");
                var destinationStart = double.Parse(parts[0]);
                var sourceStart = double.Parse(parts[1]);
                var rangeLength = double.Parse(parts[2]);

                return new Mapping
                {
                    DestinationStart = double.Parse(parts[0]),
                    DestinationEnd = destinationStart + rangeLength,
                    SourceStart = double.Parse(parts[1]),
                    SourceEnd = sourceStart + rangeLength,
                    RangeLength = double.Parse(parts[2]),
                };
            }).ToList();

            return new MappingSection
            {
                SourceType = sourceType,
                DestinationType = destinationType,
                Mappings = mappings,
            };
        }

        private class MappingSection
        {
            public string SourceType { get; set; } = "";
            public string DestinationType { get; set; } = "";
            public List<Mapping> Mappings {get; set; } = new();

            public double ConvertFromSourceToDestination(double sourceValue)
            {
                var matchingMapping = Mappings.FirstOrDefault(m => sourceValue >= m.SourceStart && sourceValue < m.SourceStart + m.RangeLength);
                if (matchingMapping != null)
                {
                    return matchingMapping.DestinationStart + (sourceValue - matchingMapping.SourceStart);
                }

                return sourceValue;
            }

            public List<SourceRange> ConvertRangesFromSourceToDestination(List<SourceRange> ranges)
            {
                var newRanges = new List<SourceRange>();
                foreach (var range in ranges)
                {
                    var overlaps = Mappings
                        .Where(m => (m.SourceStart >= range.Start && m.SourceStart <= range.End) || (range.Start >= m.SourceStart && range.Start <= m.SourceEnd))
                        .Select(m => (m, new SourceRange { Start = Math.Max(range.Start, m.SourceStart), End = Math.Min(range.End, m.SourceEnd) }))
                        .OrderBy(x => x.Item2.Start)
                        .ToList();

                    var start = range.Start;
                    foreach (var overlap in overlaps)
                    {
                        var mapping = overlap.Item1;
                        var sourceRange = overlap.Item2;
                        if (start < sourceRange.Start)
                        {
                            newRanges.Add(new SourceRange { Start = start, End = sourceRange.Start - 1 });
                        }
                        newRanges.Add(new SourceRange { Start = mapping.DestinationStart + (sourceRange.Start - mapping.SourceStart), End = mapping.DestinationStart + (sourceRange.End - mapping.SourceStart) });
                        start = sourceRange.End + 1;
                    }
                    if (start <= range.End)
                    {
                        newRanges.Add(new SourceRange { Start = start, End = range.End });
                    }
                }
                return newRanges;
            }
        }

        private class SourceRange
        {
            public double Start { get; set; }
            public double End { get; set; }
        }

        private class Mapping
        {
            public double SourceStart { get; set; }
            public double SourceEnd { get; set; }
            public double DestinationStart { get; set; }
            public double DestinationEnd { get; set; }
            public double RangeLength { get; set; }
        }
    }
}
