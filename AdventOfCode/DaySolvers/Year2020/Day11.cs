using Helpers.Helpers;
using System.Text;

namespace AdventOfCode.Year2020
{
    internal class Day11 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var seats = lines.ToList();
            var maxX = seats[0].Length;
            var maxY = seats.Count;
            var seatString = string.Join("", seats);

            var iteration = 0;
            while (true)
            {
                iteration++;
                var newSeats = new List<string>();
                for (var y = 0; y < maxY; y++)
                {
                    var row = seats[y];
                    var sb = new StringBuilder();
                    for (var x = 0; x < maxX; x++)
                    {
                        var adjacentSeatCount = 0;
                        var currentSeat = row[x];
                        var newSeat = currentSeat;
                        foreach (var (ax, ay) in DirectionExtensions.GetAllMovements((x, y), true))
                        {
                            if (ax < 0 || ax >= maxX || ay < 0 || ay >= maxY) continue;
                            if (seats[ay][ax] == '#') adjacentSeatCount++;
                        }
                        if (currentSeat == '#' && adjacentSeatCount >= 4) newSeat = 'L';
                        else if (currentSeat == 'L' && adjacentSeatCount == 0) newSeat = '#';
                        sb.Append(newSeat);
                    }
                    newSeats.Add(sb.ToString());
                }
                seats = newSeats;
                var newSeatString = string.Join("", newSeats);
                if (newSeatString == seatString) break;
                seatString = newSeatString;
            }

            var expectedResult = 2468;
            var result = seats.Sum(r => r.Count(c => c == '#'));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var seats = lines.ToList();
            var maxX = seats[0].Length;
            var maxY = seats.Count;
            var seatString = string.Join("", seats);

            var iteration = 0;
            var directions = DirectionExtensions.GetAllMovements(true);
            while (true)
            {
                iteration++;
                var newSeats = new List<string>();
                for (var y = 0; y < maxY; y++)
                {
                    var row = seats[y];
                    var sb = new StringBuilder();
                    for (var x = 0; x < maxX; x++)
                    {
                        var adjacentSeatCount = 0;
                        var currentSeat = row[x];
                        var newSeat = currentSeat;
                        if (currentSeat == '.')
                        {
                            sb.Append('.');
                            continue;
                        }

                        foreach (var (dx, dy) in directions)
                        {
                            var ax = x;
                            var ay = y;
                            while (true)
                            {
                                ax += dx;
                                ay += dy;
                                if (ax < 0 || ax >= maxX || ay < 0 || ay >= maxY) break;
                                var adjSpot = seats[ay][ax];
                                if (adjSpot == '.') continue;
                                if (adjSpot == '#') adjacentSeatCount++;
                                break;
                            }
                        }
                        if (currentSeat == '#' && adjacentSeatCount >= 5) newSeat = 'L';
                        else if (currentSeat == 'L' && adjacentSeatCount == 0) newSeat = '#';
                        sb.Append(newSeat);
                    }
                    newSeats.Add(sb.ToString());
                }
                seats = newSeats;
                var newSeatString = string.Join("", newSeats);
                if (newSeatString == seatString) break;
                seatString = newSeatString;
            }

            var expectedResult = 2214;
            var result = seats.Sum(r => r.Count(c => c == '#'));
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
