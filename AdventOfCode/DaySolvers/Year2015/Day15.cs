using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
    internal partial class Day15 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var ingredients = GetIngredients(lines).ToDictionary(i => i.Name, i => i);
            var totalIngredientCount = 100;

            var getIngredientCombinations = GetIngredientCombinations([.. ingredients.Keys], totalIngredientCount);
            var scores = getIngredientCombinations.Select(c => (combo: c, score: GetScore(c.Select(i => (ingredients[i.name], i.amount)).ToList(), false))).ToList();
            var (combo, score) = scores.OrderByDescending(s => s.score).First();
            var expectedResult = 18965440;
            var result = score;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var ingredients = GetIngredients(lines).ToDictionary(i => i.Name, i => i);
            var totalIngredientCount = 100;

            var getIngredientCombinations = GetIngredientCombinations([.. ingredients.Keys], totalIngredientCount);
            var scores = getIngredientCombinations.Select(c => (combo: c, score: GetScore(c.Select(i => (ingredients[i.name], i.amount)).ToList(), true))).ToList();
            var (combo, score) = scores.OrderByDescending(s => s.score).First();
            var expectedResult = 15862900;
            var result = score;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private List<Ingredient> GetIngredients(IList<string> lines)
        {
            var regex = ParseRegex();
            return lines.Select(l =>
            {
                var groups = regex.Matches(l)[0].Groups;
                return new Ingredient
                {
                    Name = groups["name"].Value,
                    Capacity = int.Parse(groups["capacity"].Value),
                    Durability = int.Parse(groups["durability"].Value),
                    Flavor = int.Parse(groups["flavor"].Value),
                    Texture = int.Parse(groups["texture"].Value),
                    Calories = int.Parse(groups["calories"].Value),
                };
            }).ToList();
        }

        private static long GetScore (List<(Ingredient ingredient, int amount)> ingredients, bool shouldCheckCalories)
        {
            if (shouldCheckCalories)
            {
                var totalCalories = ingredients.Sum(i => i.ingredient.Calories * i.amount);
                if (totalCalories != 500) return 0;
            }

            long totalCapacity = Math.Max(0, ingredients.Sum(i => i.ingredient.Capacity * i.amount));
            long totalDurability = Math.Max(0, ingredients.Sum(i => i.ingredient.Durability * i.amount));
            long totalFlavor = Math.Max(0, ingredients.Sum(i => i.ingredient.Flavor * i.amount));
            long totalTexture = Math.Max(0, ingredients.Sum(i => i.ingredient.Texture * i.amount));
            return totalCapacity * totalDurability * totalFlavor * totalTexture;
        }

        private class Ingredient
        {
            public required string Name;
            public int Capacity;
            public int Durability;
            public int Flavor;
            public int Texture;
            public int Calories;
        }

        private List<List<(string name, int amount)>> GetIngredientCombinations(List<string> ingredients, int totalNeeded)
        {
            var returnList = new List<List<(string name, int amount)>>();
            var ingredient = ingredients.First();
            var rest = ingredients.Skip(1).ToList();
            if (rest.Count == 0)
            {
                returnList.Add([(ingredient, totalNeeded)]);
                return returnList;
            }

            for (var i = 1; i <= totalNeeded - ingredients.Count + 1; i++)
            {
                var myAmount = (ingredient, i);
                var restAmounts = GetIngredientCombinations(rest, totalNeeded - i);
                var allAmounts = restAmounts.Select(b => b.Concat([myAmount]).ToList()).ToList();
                returnList.AddRange(allAmounts);
            }
            return returnList;
        }

        [GeneratedRegex(@"^(?<name>[^:]+)[^-\d]+(?<capacity>-?\d+)[^-\d]+(?<durability>-?\d+)[^-\d]+(?<flavor>-?\d+)[^-\d]+(?<texture>-?\d+)[^-\d]+(?<calories>-?\d+)")]
        private static partial Regex ParseRegex();
    }
}
