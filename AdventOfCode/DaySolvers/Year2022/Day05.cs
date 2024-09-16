namespace AdventOfCode.Year2022
{
    internal class Day05 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var length = (lines[0].Length / 4) + 1;
            var lists = Enumerable.Range(1, length).ToDictionary(x => x, x => new List<char>());
            var i = 0;
            while (i < lines.Count)
            {
                var line = lines[i];
                if (!line.Contains('['))
                {
                    break;
                }
                var stackIndex = 1;
                for (var j = 0; j < line.Length; j += 4)
                {
                    if (!string.IsNullOrWhiteSpace(line.Substring(j, 3)))
                    {
                        lists[stackIndex].Add(line[j + 1]);
                    }
                    stackIndex++;
                }
                i++;
            }
            i += 2;

            var stacks = lists.ToDictionary(l => l.Key, l => new Stack<char>(Enumerable.Reverse(l.Value)));
            while (i < lines.Count)
            {
                var line = lines[i];
                line = line.Replace("move ", "");
                var parts = line.Split(" from ");
                var countToMove = int.Parse(parts[0]);
                parts = parts[1].Split(" to ");
                var oldStack = int.Parse(parts[0]);
                var newStack = int.Parse(parts[1]);
                for (var j = 0; j < countToMove; j++)
                {
                    stacks[newStack].Push(stacks[oldStack].Pop());
                }
                i++;
            }

            var outputString = "";
            foreach (var stack in stacks)
            {
                outputString += stack.Value.Peek();
            }
            return outputString;
        }

        public object HardSolution(IList<string> lines)
        {
            var length = (lines[0].Length / 4) + 1;
            var lists = Enumerable.Range(1, length).ToDictionary(x => x, x => new List<char>());
            var i = 0;
            while (i < lines.Count)
            {
                var line = lines[i];
                if (!line.Contains('['))
                {
                    break;
                }
                var stackIndex = 1;
                for (var j = 0; j < line.Length; j += 4)
                {
                    if (!string.IsNullOrWhiteSpace(line.Substring(j, 3)))
                    {
                        lists[stackIndex].Add(line[j + 1]);
                    }
                    stackIndex++;
                }
                i++;
            }
            i += 2;

            while (i < lines.Count)
            {
                var line = lines[i];
                line = line.Replace("move ", "");
                var parts = line.Split(" from ");
                var countToMove = int.Parse(parts[0]);
                parts = parts[1].Split(" to ");
                var oldStack = int.Parse(parts[0]);
                var newStack = int.Parse(parts[1]);
                var boxesToMove = lists[oldStack].Take(countToMove).ToList();
                lists[oldStack] = lists[oldStack].Skip(countToMove).ToList();
                lists[newStack] = boxesToMove.Concat(lists[newStack]).ToList();
                i++;
            }

            var outputString = "";
            foreach (var list in lists)
            {
                outputString += list.Value[0];
            }
            return outputString;
        }
    }
}
