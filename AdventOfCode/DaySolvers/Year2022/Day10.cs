namespace AdventOfCode.Year2022
{
    internal class Day10 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var currentValue = 1;
            var cycleValues = new List<int> { 1 };
            foreach (var line in lines)
            {
                if (line == "noop")
                {
                    cycleValues.Add(currentValue);
                }
                else
                {
                    var addAmount = int.Parse(line.Replace("addx ", ""));
                    cycleValues.Add(currentValue);
                    currentValue += addAmount;
                    cycleValues.Add(currentValue);
                }
            }

            var signalStrengthSums = 0;
            for (var x = 20; x < cycleValues.Count; x += 40)
            {
                signalStrengthSums += x * cycleValues[x - 1];
            }
            return signalStrengthSums;
        }

        private int _currentValue = 1;
        private int _index = 0;
        public object HardSolution(IList<string> lines)
        {
            foreach (var line in lines)
            {
                if (line == "noop")
                {
                    TickAndDraw();
                }
                else
                {
                    TickAndDraw();
                    TickAndDraw();
                    _currentValue += int.Parse(line.Replace("addx ", ""));
                }
            }

            Console.WriteLine();
            return 0;
        }

        private void TickAndDraw()
        {
            if (_index % 40 == 0 && _index != 0)
            {
                Console.WriteLine();
            }
            if (Math.Abs(_currentValue - (_index % 40)) <= 1)
            {
                Console.Write('#');
            }
            else
            {
                Console.Write(' ');
            }
            _index++;
        }
    }
}
