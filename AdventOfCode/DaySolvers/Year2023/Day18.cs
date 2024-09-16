using Helpers.Helpers;

namespace AdventOfCode.Year2023
{
    internal class Day18 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var instructions = lines.Select(Instruction.ParsePart1).ToList();
            return Calculate(instructions);
        }

        public object HardSolution(IList<string> lines)
        {
            var instructions = lines.Select(Instruction.ParsePart2).ToList();
            return Calculate(instructions);
        }

        private static long Calculate(List<Instruction> instructions)
        {
            var currentPoint = (x: 0L, y: 0L);
            var points = new List<(long x, long y)>();

            var perimeter = 0L;
            foreach (var instruction in instructions)
            {
                var (x, y) = currentPoint;
                currentPoint = instruction.Direction.GetMovement(currentPoint, instruction.Distance);
                points.Add(currentPoint);
                perimeter += instruction.Distance;
            }

            var area = CalculateShoelaceArea(points);
            var interiorArea = area - perimeter / 2 + 1; // Pick's Theorem
            return interiorArea + perimeter;
        }

        private static long CalculateShoelaceArea(List<(long x, long y)> points)
        {
            long sum = 0;
            for (var i = 0; i < points.Count; i++)
            {
                var (x1, y1) = points[i];
                var (x2, y2) = points[(i + 1)%points.Count];
                sum += (x1 * y2 - y1 * x2);
            }
            return Math.Abs(sum) / 2;
        }

        private class Instruction
        {
            public Direction Direction { get; set; }
            public long Distance { get; set; }

            public static Instruction ParsePart1(string line)
            {
                var parts = line.Split(' ');
                var direction = parts[0] switch
                {
                    "R" => Direction.East,
                    "L" => Direction.West,
                    "U" => Direction.North,
                    "D" => Direction.South,
                    _ => throw new NotImplementedException()
                };
                var distance = int.Parse(parts[1]);
                return new Instruction
                {
                    Direction = direction,
                    Distance = distance,
                };
            }

            public static Instruction ParsePart2(string line)
            {
                var parts = line.Split(" ");
                var colorCode = parts[2].Replace("(#", "").Replace(")", "");
                var hex = colorCode[0..5];
                var distance = ConvertHexString(hex);
                var direction = colorCode[5] switch
                {
                    '0' => Direction.East,
                    '1' => Direction.South,
                    '2' => Direction.West,
                    '3' => Direction.North,
                    _ => throw new NotImplementedException()
                };

                return new Instruction
                {
                    Direction = direction,
                    Distance = distance,
                };
            }

            private static long ConvertHexString(string hex)
            {
                var reversedString = hex.Reverse().ToList();
                double sum = 0;
                for (var i = 0; i < reversedString.Count; i++)
                {
                    var c = reversedString[i];
                    int? val = c switch
                    {
                        'a' => 10,
                        'b' => 11,
                        'c' => 12,
                        'd' => 13,
                        'e' => 14,
                        'f' => 15,
                        _ => null,
                    }; 
                    val ??= (c - '0');
                    sum += val.Value * Math.Pow(16, i);
                }
                return (long)sum;
            }
        }
    }
}
