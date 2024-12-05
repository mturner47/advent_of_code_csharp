namespace AdventOfCode.Year2020
{
    internal class Day05 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var seats = lines.Select(Parse).ToList();
            var expectedResult = 878;
            var result = seats.Max(s => s.seatID);
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var seats = lines.Select(Parse).ToList().OrderBy(s => s.seatID).ToList();
            var mySeatID = 0;
            for (var i = 0; i < seats.Count - 1; i++)
            {
                if (seats[i].seatID + 2 == seats[i + 1].seatID)
                {
                    var seatID = seats[i].seatID + 1;
                    var row = seatID / 8;
                    if (row != 0 && row != 127)
                    {
                        mySeatID = seatID;
                        break;
                    }
                }
            }
            var expectedResult = 504;
            var result = mySeatID;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        private static (int row, int col, int seatID) Parse(string line)
        {
            var rowChars = line[..7].Replace("F", "0").Replace("B", "1");
            var colChars = line[7..].Replace("L", "0").Replace("R", "1");
            var row = Convert.ToInt32(rowChars, 2);
            var col = Convert.ToInt32(colChars, 2);
            return (row, col, row * 8 + col);
        }
    }
}
