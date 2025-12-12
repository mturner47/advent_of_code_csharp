using Helpers.Extensions;
using Microsoft.Z3;

namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var buttons = lines.Select(Parse).ToList();

            var expectedResult = 524;
            var result = buttons.Sum(b => FewestPresses(b.buttonGoal, b.buttonCombos));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var buttons = lines.Select(Parse).ToList();
            var expectedResult = 21696;
            var result = buttons.Sum(b => FewestPressesForJoltageZ3(b.buttonCombos, b.joltageRequirements));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private (int buttonGoal, List<List<bool>> buttonCombos, int[] joltageRequirements) Parse(string line)
        {
            var partsA = line.Split("] (");
            var boolArray = partsA[0].Replace("[", "").Select(c => c == '#').ToArray();
            var buttonCount = boolArray.Length;
            var buttonGoal = boolArray.Aggregate(0, (sum, val) => (sum * 2) + (val ? 1 : 0));

            var partsB = partsA[1].Split(") {");
            var joltageRequirements = partsB[1].Replace("}", "").Split(",").Select(s => s.ToInt()).ToArray();

            var buttonCombos = partsB[0].Split(") (").Select(i => i.Split(",")).Select(bc =>
            {
                var boolArray = new bool[buttonCount];
                foreach (var s in bc)
                {
                    var i = s.ToInt();
                    boolArray[i] = true;
                }
                return boolArray.ToList();
            }).ToList();
            return (buttonGoal, buttonCombos, joltageRequirements);
        }

        private static int FewestPresses(int buttonGoal, List<List<bool>> buttonCombos)
        {
            var knownMinimums = new HashSet<int> { 0 };
            var buttonComboInts = buttonCombos.Select(bc => bc.Aggregate(0, (sum, val) => (sum * 2) + (val ? 1 : 0)));
            var queue = new PriorityQueue<(int current, int numPresses), int>();
            queue.Enqueue((0, 0), 0);

            while (queue.Count > 0)
            {
                var (current, numPresses) = queue.Dequeue();
                var nextNumPresses = numPresses + 1;
                foreach (var bc in buttonComboInts)
                {
                    var next = current ^ bc;
                    if (next == buttonGoal) return nextNumPresses;

                    if (!knownMinimums.Contains(next))
                    {
                        knownMinimums.Add(next);
                        queue.Enqueue((next, nextNumPresses), nextNumPresses);
                    }
                }
            }
            throw new NotImplementedException();
        }

        public static int FewestPressesForJoltageZ3(List<List<bool>> buttonCombos, int[] joltageRequirements)
        {
            var context = new Context();
            var opt = context.MkOptimize();

            var buttonVars = new IntExpr[buttonCombos.Count];
            for (var b = 0; b < buttonCombos.Count; b++)
            {
                buttonVars[b] = context.MkIntConst($"b_{b}");
                opt.Add(context.MkGe(buttonVars[b], context.MkInt(0)));
            }

            for (var j = 0; j < joltageRequirements.Length; j++)
            {
                var terms = new List<ArithExpr>();
                for (var b = 0; b < buttonCombos.Count; b++)
                {
                    if (buttonCombos[b][j]) terms.Add(buttonVars[b]);
                }

                var se = context.MkAdd([.. terms]);
                var te = context.MkInt(joltageRequirements[j]);

                opt.Add(context.MkEq(se, te));
            }

            opt.MkMinimize(context.MkAdd([.. buttonVars.Cast<ArithExpr>()]));

            var status = opt.Check();

            return Enumerable.Range(0, buttonCombos.Count).Sum(id => ((IntNum)opt.Model.Evaluate(buttonVars[id])).Int);
        }
    }
}
