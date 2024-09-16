namespace AdventOfCode.Year2016
{
    internal class Day02 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var currentButton = '5';
            var pushedButtons = "";

            foreach (var line in lines)
            {
                foreach (var c in line)
                {
                    switch ((c, currentButton))
                    {
                        case ('U', '4'):
                        case ('L', '2'):
                            currentButton = '1';
                            break;
                        case ('U', '5'):
                        case ('L', '3'):
                        case ('R', '1'):
                            currentButton = '2';
                            break;
                        case ('U', '6'):
                        case ('R', '2'):
                            currentButton = '3';
                            break;
                        case ('U', '7'):
                        case ('D', '1'):
                        case ('L', '5'):
                            currentButton = '4';
                            break;
                        case ('U', '8'):
                        case ('L', '6'):
                        case ('R', '4'):
                        case ('D', '2'):
                            currentButton = '5';
                            break;
                        case ('U', '9'):
                        case ('R', '5'):
                        case ('D', '3'):
                            currentButton = '6';
                            break;
                        case ('D', '4'):
                        case ('L', '8'):
                            currentButton = '7';
                            break;
                        case ('D', '5'):
                        case ('L', '9'):
                        case ('R', '7'):
                            currentButton = '8';
                            break;
                        case ('D', '6'):
                        case ('R', '8'):
                            currentButton = '9';
                            break;
                        default:
                            break;
                    }
                }
                pushedButtons += currentButton;
            }

            var expectedResult = 73597;
            var result = int.Parse(pushedButtons);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var currentButton = '5';
            var pushedButtons = "";

            foreach (var line in lines)
            {
                foreach (var c in line)
                {
                    switch ((c, currentButton))
                    {
                        case ('U', '3'):
                            currentButton = '1';
                            break;
                        case ('U', '6'):
                        case ('L', '3'):
                            currentButton = '2';
                            break;
                        case ('U', '7'):
                        case ('R', '2'):
                        case ('D', '1'):
                        case ('L', '4'):
                            currentButton = '3';
                            break;
                        case ('U', '8'):
                        case ('R', '3'):
                            currentButton = '4';
                            break;
                        case ('L', '6'):
                            currentButton = '5';
                            break;
                        case ('U', 'A'):
                        case ('L', '7'):
                        case ('R', '5'):
                        case ('D', '2'):
                            currentButton = '6';
                            break;
                        case ('U', 'B'):
                        case ('L', '8'):
                        case ('R', '6'):
                        case ('D', '3'):
                            currentButton = '7';
                            break;
                        case ('U', 'C'):
                        case ('L', '9'):
                        case ('R', '7'):
                        case ('D', '4'):
                            currentButton = '8';
                            break;
                        case ('R', '8'):
                            currentButton = '9';
                            break;
                        case ('D', '6'):
                        case ('L', 'B'):
                            currentButton = 'A';
                            break;
                        case ('U', 'D'):
                        case ('R', 'A'):
                        case ('D', '7'):
                        case ('L', 'C'):
                            currentButton = 'B';
                            break;
                        case ('D', '8'):
                        case ('R', 'B'):
                            currentButton = 'C';
                            break;
                        case ('D', 'B'):
                            currentButton = 'D';
                            break;
                        default:
                            break;
                    }
                }
                pushedButtons += currentButton;
            }

            var expectedResult = "A47DA";
            var result = pushedButtons;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
