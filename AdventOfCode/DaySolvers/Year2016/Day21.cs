namespace AdventOfCode.Year2016
{
    internal class Day21 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var input = "abcdefgh".ToList();
            foreach (var line in lines)
            {
                if (line.StartsWith("swap position"))
                {
                    var parts = line.Replace("swap position ", "").Split(" with position ");
                    var x = int.Parse(parts[0]);
                    var y = int.Parse(parts[1]);
                    (input[y], input[x]) = (input[x], input[y]);
                }
                else if (line.StartsWith("swap letter"))
                {
                    var parts = line.Replace("swap letter ", "").Split(" with letter ");
                    var x = input.IndexOf(parts[0][0]);
                    var y = input.IndexOf(parts[1][0]);
                    (input[y], input[x]) = (input[x], input[y]);
                }
                else if (line.StartsWith("rotate based on position of letter "))
                {
                    var x = input.IndexOf(line.Replace("rotate based on position of letter ", "")[0]);
                    x += (x >= 4 ? 2 : 1);
                    x %= input.Count;
                    var newInput = input.Skip(input.Count - x).ToList();
                    newInput.AddRange(input.Take(input.Count - x));
                    input = newInput;
                }
                else if (line.StartsWith("rotate "))
                {
                    var parts = line.Replace("rotate ", "").Replace(" steps", "").Replace(" step", "").Split(" ");
                    var amountToRotate = int.Parse(parts[1]);
                    if (parts[0] == "right")
                    {
                        var newInput = input.Skip(input.Count - amountToRotate).ToList();
                        newInput.AddRange(input.Take(input.Count - amountToRotate));
                        input = newInput;
                    }
                    else
                    {
                        var newInput = input.Skip(amountToRotate).ToList();
                        newInput.AddRange(input.Take(amountToRotate));
                        input = newInput;
                    }
                }
                else if (line.StartsWith("reverse positions "))
                {
                    var parts = line.Replace("reverse positions ", "").Split(" through ");
                    var x = int.Parse(parts[0]);
                    var y = int.Parse(parts[1]);
                    var newInput = x == 0 ? [] : input.Take(x).ToList();
                    newInput.AddRange(input.Skip(x).Take(y + 1 - x).Reverse());
                    if (y < input.Count - 1) newInput.AddRange(input.Skip(y + 1));
                    input = newInput;
                }
                else if (line.StartsWith("move position "))
                {
                    var parts = line.Replace("move position ", "").Split(" to position ");
                    var x = int.Parse(parts[0]);
                    var y = int.Parse(parts[1]);
                    var c = input[x];
                    input.RemoveAt(x);
                    input.Insert(y, c);
                }
            }

            var expectedResult = "bgfacdeh";
            var result = new string(input.ToArray());
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var input = "fbgdceah".ToList();

            foreach (var line in lines.Reverse())
            {
                if (line.StartsWith("swap position"))
                {
                    var parts = line.Replace("swap position ", "").Split(" with position ");
                    var x = int.Parse(parts[0]);
                    var y = int.Parse(parts[1]);
                    (input[y], input[x]) = (input[x], input[y]);
                }
                else if (line.StartsWith("swap letter"))
                {
                    var parts = line.Replace("swap letter ", "").Split(" with letter ");
                    var x = input.IndexOf(parts[0][0]);
                    var y = input.IndexOf(parts[1][0]);
                    (input[y], input[x]) = (input[x], input[y]);
                }
                else if (line.StartsWith("rotate based on position of letter "))
                {
                    var x = input.IndexOf(line.Replace("rotate based on position of letter ", "")[0]);
                    var shiftLeftAmount = x switch
                    {
                        0 => 9,
                        1 => 1,
                        2 => 6,
                        3 => 2,
                        4 => 7,
                        5 => 3,
                        6 => 8,
                        7 => 4,
                        _ => throw new NotImplementedException()
                    } % input.Count;
                    var newInput = input.Skip(shiftLeftAmount).ToList();
                    newInput.AddRange(input.Take(shiftLeftAmount));
                    input = newInput;
                }
                else if (line.StartsWith("rotate "))
                {
                    var parts = line.Replace("rotate ", "").Replace(" steps", "").Replace(" step", "").Split(" ");
                    var amountToRotate = int.Parse(parts[1]);
                    if (parts[0] == "left")
                    {
                        var newInput = input.Skip(input.Count - amountToRotate).ToList();
                        newInput.AddRange(input.Take(input.Count - amountToRotate));
                        input = newInput;
                    }
                    else
                    {
                        var newInput = input.Skip(amountToRotate).ToList();
                        newInput.AddRange(input.Take(amountToRotate));
                        input = newInput;
                    }
                }
                else if (line.StartsWith("reverse positions "))
                {
                    var parts = line.Replace("reverse positions ", "").Split(" through ");
                    var x = int.Parse(parts[0]);
                    var y = int.Parse(parts[1]);
                    var newInput = x == 0 ? [] : input.Take(x).ToList();
                    newInput.AddRange(input.Skip(x).Take(y + 1 - x).Reverse());
                    if (y < input.Count - 1) newInput.AddRange(input.Skip(y + 1));
                    input = newInput;
                }
                else if (line.StartsWith("move position "))
                {
                    var parts = line.Replace("move position ", "").Split(" to position ");
                    var y = int.Parse(parts[0]);
                    var x = int.Parse(parts[1]);
                    var c = input[x];
                    input.RemoveAt(x);
                    input.Insert(y, c);
                }
            }

            var expectedResult = "bdgheacf";
            var result = new string(input.ToArray());
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
