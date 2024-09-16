namespace AdventOfCode.Year2019
{
    internal class Day14 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var conversions = lines.Select(ParseLine).ToDictionary(c => c.OutputName, c => c);

            var expectedResult = 1582325;
            var result = CalculateOreRequirement(conversions, 1);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var conversions = lines.Select(ParseLine).ToDictionary(c => c.OutputName, c => c);
            var worstCaseOre = CalculateOreRequirement(conversions, 1);
            var oreAvailable = 1000000000000L;
            var minimumFuel = oreAvailable / worstCaseOre;
            var maximumFuel = minimumFuel * 10;
            double fuelToCheck = 0;

            while (true)
            {
                fuelToCheck = Math.Ceiling(((double)maximumFuel + minimumFuel) / 2);
                var oreRequired = CalculateOreRequirement(conversions, (long)fuelToCheck);
                if (oreRequired < oreAvailable)
                {
                    minimumFuel = (long)fuelToCheck;
                }
                else if (oreRequired > oreAvailable)
                {
                    maximumFuel = (long)fuelToCheck;
                }

                if (maximumFuel == minimumFuel + 1)
                {
                    fuelToCheck = minimumFuel;
                    break;
                }
            }

            var expectedResult = 2267486;
            var result = (long)fuelToCheck;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static long CalculateOreRequirement(Dictionary<string, Conversion> conversions, long fuelRequired)
        {
            var inventory = conversions.ToDictionary(c => c.Key, c => new InventoryItem
            {
                Name = c.Key,
                RequiredAmount = c.Key == "FUEL" ? fuelRequired : 0,
            });

            inventory.Add("ORE", new InventoryItem { Name = "ORE" });

            while (inventory.Values.Any(x => x.Name != "ORE" && x.RequiredAmount > 0))
            {
                var item = inventory.Values
                    .Where(x => x.Name != "ORE" && x.RequiredAmount > 0)
                    .OrderByDescending(x => x.RequiredAmount).First();

                var newRequired = item.RequiredAmount;
                var newOnHand = item.OnHand;
                if (item.OnHand > 0)
                {
                    var amountToReduce = Math.Min(item.OnHand, item.RequiredAmount);
                    item.OnHand -= amountToReduce;
                    item.RequiredAmount -= amountToReduce;
                }

                if (item.RequiredAmount == 0)
                {
                    break;
                }

                var conversion = conversions[item.Name];
                var conversionsToDo = item.RequiredAmount / conversion.OutputAmount;
                if (item.RequiredAmount % conversion.OutputAmount > 0) conversionsToDo += 1;
                item.OnHand = (conversionsToDo * conversion.OutputAmount) - item.RequiredAmount;
                item.RequiredAmount = 0;

                foreach (var (amount, inputName) in conversion.Inputs)
                {
                    var inputItem = inventory[inputName];
                    var requiredAmount = amount * conversionsToDo;
                    if (inputItem.OnHand >= requiredAmount)
                    {
                        inputItem.OnHand -= requiredAmount;
                    }
                    else
                    {
                        requiredAmount -= inputItem.OnHand;
                        inputItem.OnHand = 0;
                        inputItem.RequiredAmount += requiredAmount;
                    }
                }
            }

            return inventory["ORE"].RequiredAmount;
        }

        private class InventoryItem
        {
            public string Name { get; set; } = "";
            public long RequiredAmount { get; set; } = 0;
            public long OnHand { get; set; } = 0;
        }

        private class Conversion
        {
            public string OutputName { get; set; } = "";
            public long OutputAmount { get; set; }
            public List<(long amount, string name)> Inputs { get; set; } = [];
        }

        private static Conversion ParseLine(string line)
        {
            var lineParts = line.Split(" => ");
            var inputs = lineParts[0].Split(", ").Select(i =>
            {
                var inputParts = i.Split(" ");
                return (long.Parse(inputParts[0]), inputParts[1]);
            }).ToList();
            var outputParts = lineParts[1].Split(" ");
            var outputAmount = long.Parse(outputParts[0]);
            var outputName = outputParts[1];

            return new Conversion
            {
                OutputName = outputName,
                OutputAmount = outputAmount,
                Inputs = inputs,
            };
        }
    }
}
