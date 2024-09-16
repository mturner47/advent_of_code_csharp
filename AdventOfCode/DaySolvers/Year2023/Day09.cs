namespace AdventOfCode.Year2023
{
    internal class Day09 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var groups = lines.Select(l => l.Split(" ").Select(double.Parse).ToList()).ToList();
            return groups.Select(GetNextValue).Sum();
        }

        public object HardSolution(IList<string> lines)
        {
            var groups = lines.Select(l => l.Split(" ").Select(double.Parse).ToList()).ToList();
            return groups.Select(GetPreviousValue).Sum();
        }

        private static double GetNextValue(List<double> values)
        {
            var valueLayers = new List<List<double>>
            {
                values,
            };

            while (valueLayers.Last().Any(v => v != 0))
            {
                var currentLayer = valueLayers.Last();
                var nextLayer = new List<double>();
                for (var i = 0; i < currentLayer.Count - 1; i++)
                {
                    nextLayer.Add(currentLayer[i + 1] - currentLayer[i]);
                }
                valueLayers.Add(nextLayer);
            }
            valueLayers.Last().Add(0);

            for (var i = valueLayers.Count - 1; i > 0; i--)
            {
                valueLayers[i - 1].Add(valueLayers[i - 1].Last() + valueLayers[i].Last());
            }
            return valueLayers[0].Last();
        }

        private static double GetPreviousValue(List<double> values)
        {
            var valueLayers = new List<List<double>>
            {
                values,
            };

            while (valueLayers.Last().Any(v => v != 0))
            {
                var currentLayer = valueLayers.Last();
                var nextLayer = new List<double>();
                for (var i = 0; i < currentLayer.Count - 1; i++)
                {
                    nextLayer.Add(currentLayer[i + 1] - currentLayer[i]);
                }
                valueLayers.Add(nextLayer);
            }
            valueLayers.Last().Insert(0, 0);

            for (var i = valueLayers.Count - 1; i > 0; i--)
            {
                valueLayers[i - 1].Insert(0, valueLayers[i - 1].First() - valueLayers[i].First());
            }
            return valueLayers[0].First();
        }
    }
}
