namespace AdventOfCode.Year2023
{
    internal class Day15 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var line = lines[0];
            return line.Split(',').Sum(ComputeHash);
        }

        public object HardSolution(IList<string> lines)
        {
            var line = lines[0];
            var dict = Enumerable.Range(0, 256).ToDictionary(i => i, i => new List<(string label, int value)>());
            foreach (var step in line.Split(','))
            {
                var label = step.Replace("-", "").Split("=").First();
                var hash = ComputeHash(label);

                var firstIndex = dict[hash].FindIndex(v => v.label == label);
                if (step.EndsWith('-'))
                {
                    if (firstIndex != -1)
                    {
                        dict[hash].RemoveAt(firstIndex);
                    }
                }
                else
                {
                    var value = int.Parse(step.Split('=')[1]);
                    if (firstIndex == -1)
                    {
                        dict[hash].Add((label, value));
                    }
                    else
                    {
                        dict[hash][firstIndex] = (label, value);
                    }
                }
            }

            var sum = 0;
            foreach (var kvp in dict)
            {
                for (var i = 0; i < kvp.Value.Count; i++)
                {
                    var value = kvp.Value[i].value;
                    sum += (kvp.Key + 1) * (i + 1) * value;
                }
            }
            return sum;
        }

        private int ComputeHash(string step)
        {
            var currentValue = 0;
            for (var i = 0; i < step.Length; i++)
            {
                currentValue += (int)step[i];
                currentValue *= 17;
                currentValue %= 256;
            }
            return currentValue;
        }
    }
}
