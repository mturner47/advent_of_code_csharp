namespace AdventOfCode.Year2016
{
    internal class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var bots = GetStartingState(lines);
            var outputs = new Dictionary<int, List<int>>();
            var magicBot = 0;
            while (true)
            {
                var botKvp = bots.First(b => b.Value.heldValues.Count == 2);
                var botNumber = botKvp.Key;
                var (heldValues, low, high) = botKvp.Value;
                var lowValue = heldValues.Min();
                var highValue = heldValues.Max();
                if (lowValue == 17 && highValue == 61)
                {
                    magicBot = botNumber;
                    break;
                }

                var (lowType, lowIndex) = low;
                if (lowType == "output")
                {
                    if (!outputs.ContainsKey(lowIndex)) outputs.Add(lowIndex, []);
                    outputs[lowIndex].Add(lowValue);
                }
                else
                {
                    bots[lowIndex].heldValues.Add(lowValue);
                }
                heldValues.Remove(lowValue);

                var (highType, highIndex) = high;
                if (highType == "output")
                {
                    if (!outputs.ContainsKey(highIndex)) outputs.Add(highIndex, []);
                    outputs[highIndex].Add(highValue);
                }
                else
                {
                    bots[highIndex].heldValues.Add(highValue);
                }
                heldValues.Remove(highValue);
            }
            var expectedResult = 113;
            var result = magicBot;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var bots = GetStartingState(lines);
            var outputs = new Dictionary<int, List<int>>();

            while (bots.Any(b => b.Value.heldValues.Count == 2))
            {
                var botKvp = bots.First(b => b.Value.heldValues.Count == 2);
                var botNumber = botKvp.Key;
                var (heldValues, low, high) = botKvp.Value;
                var lowValue = heldValues.Min();
                var highValue = heldValues.Max();

                var (lowType, lowIndex) = low;
                if (lowType == "output")
                {
                    if (!outputs.ContainsKey(lowIndex)) outputs.Add(lowIndex, []);
                    outputs[lowIndex].Add(lowValue);
                }
                else
                {
                    bots[lowIndex].heldValues.Add(lowValue);
                }
                heldValues.Remove(lowValue);

                var (highType, highIndex) = high;
                if (highType == "output")
                {
                    if (!outputs.ContainsKey(highIndex)) outputs.Add(highIndex, []);
                    outputs[highIndex].Add(highValue);
                }
                else
                {
                    bots[highIndex].heldValues.Add(highValue);
                }
                heldValues.Remove(highValue);
            }
            var expectedResult = 12803;
            var result = outputs[0][0] * outputs[1][0] * outputs[2][0];
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public static Dictionary<int, (List<int> heldValues, (string type, int index) low, (string type, int index) high)> GetStartingState(IList<string> lines)
        {
            var bots = new Dictionary<int, (List<int> heldValues, (string type, int index) low, (string type, int index) high)>();
            static (List<int>, (string, int), (string, int)) makeEmptyBot() => (new List<int>(), ("bot", 0), ("bot", 0));
            foreach (var line in lines)
            {
                if (line.StartsWith("value "))
                {
                    var parts = line.Replace("value ", "").Split("goes to bot ");
                    var value = int.Parse(parts[0]);
                    var botNumber = int.Parse(parts[1]);
                    if (!bots.ContainsKey(botNumber)) bots[botNumber] = makeEmptyBot();
                    bots[botNumber].heldValues.Add(value);
                }
                else
                {
                    var parts = line.Split(" gives low to ");
                    var botNumber = int.Parse(parts[0].Replace("bot ", ""));
                    var moreParts = parts[1].Split(" and high to ");
                    var lowParts = moreParts[0].Split(" ");
                    var low = (lowParts[0], int.Parse(lowParts[1]));
                    var highParts = moreParts[1].Split(" ");
                    var high = (highParts[0], int.Parse(highParts[1]));
                    if (!bots.ContainsKey(botNumber)) bots[botNumber] = makeEmptyBot();
                    bots[botNumber] = (bots[botNumber].heldValues, low, high);
                }
            }
            return bots;
        }
    }
}
