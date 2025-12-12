using Helpers.Extensions;
using System;
using System.Globalization;
using System.Text;

namespace AdventOfCode.DaySolvers.Year2025
{
    internal class Day06 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var operations = lines.Last().Split(" ").Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            var operationDict = new Dictionary<string, (Func<double, double, double> operation, double startingValue)>
            {
                { "+", ((double a, double b) => a + b, 0) },
                { "*", ((double a, double b) => a * b, 1) },
            };
            var numbers = lines.Take(lines.Count - 1).Select(l => l.Split(" ").Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.ToDouble()).ToArray()).ToArray();

            var totalResult = 0d;
            for (var column = 0; column < numbers[0].Length; column++)
            {
                var (operation, columnResult) = operationDict[operations[column]];
                for (var row = 0; row < numbers.Length; row++)
                {
                    if (numbers[row].Length > column)
                    {
                        columnResult = operation(columnResult, numbers[row][column]);
                    }
                }
                totalResult += columnResult;
            }
            var expectedResult = 3968933219902d;
            var result = totalResult;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var operationRowIndex = lines.Count - 1;
            var operationRow = lines[operationRowIndex];
            var operationDict = new Dictionary<char, (Func<double, double, double> operation, double startingValue)>
            {
                { '+', ((double a, double b) => a + b, 0) },
                { '*', ((double a, double b) => a * b, 1) },
            };

            var currentOperation = operationDict['+'].operation;
            var totalResult = 0d;
            double operationResult = 0d;
            for (var column = 0; column <= lines[0].Length; column++)
            {
                if (column >= lines[0].Length)
                {
                    totalResult += operationResult;
                    break;
                }

                var opChar = operationRow[column];
                if (opChar != ' ')
                {
                    (currentOperation, operationResult) = operationDict[opChar];
                }

                var sb = new StringBuilder();
                for (var row = 0; row < lines.Count - 1; row++)
                {
                    sb.Append(lines[row][column]);
                }

                var numString = sb.ToString();
                if (string.IsNullOrWhiteSpace(numString))
                {
                    totalResult += operationResult;
                    continue;
                }

                operationResult = currentOperation(operationResult, numString.ToDouble());
            }
            var expectedResult = 6019576291014d;
            var result = totalResult;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
