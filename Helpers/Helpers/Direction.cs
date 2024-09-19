namespace Helpers.Helpers
{
    public enum Direction
    {
        East,
        West,
        North,
        South,
    }

    public enum DiagonalDirection
    {
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest,
    }

    public static class DirectionExtensions
    {
        public static List<Direction> EnumerateDirections()
        {
            return
            [
                Direction.North,
                Direction.South,
                Direction.West,
                Direction.East,
            ];
        }

        public static List<DiagonalDirection> EnumerateDiagonalDirecitons()
        {
            return
            [
                DiagonalDirection.NorthWest,
                DiagonalDirection.NorthEast,
                DiagonalDirection.SouthWest,
                DiagonalDirection.SouthEast,
            ];
        }

        public static (int x, int y) GetMovement(this Direction direction)
        {
            return direction switch
            {
                Direction.East => (1, 0),
                Direction.West => (-1, 0),
                Direction.North => (0, -1),
                Direction.South => (0, 1),
                _ => throw new NotImplementedException(),
            };
        }

        public static (int x, int y) GetMovement(this DiagonalDirection d)
        {
            return d switch
            {
                DiagonalDirection.NorthWest => (-1, -1),
                DiagonalDirection.NorthEast => (1, -1),
                DiagonalDirection.SouthWest => (-1, 1),
                DiagonalDirection.SouthEast => (1, 1),
                _ => throw new NotImplementedException(),
            };
        }

        public static List<(int x, int y)> GetAllMovements(bool includeDiagonals = false)
        {
            List<(int x, int y)> movements = [(1, 0), (-1, 0), (0, -1), (0, 1)];
            if (includeDiagonals) movements = movements.Concat([(-1, -1), (-1, 1), (1, -1), (1, 1)]).ToList();
            return movements;
        }

        public static List<(int x, int y)> GetAllMovements((int x, int y) startingLocation, bool includeDiagonals = false)
        {
            var (x, y) = startingLocation;
            return GetAllMovements(includeDiagonals).Select(m => (x + m.x, y + m.y)).ToList();
        }

        public static Direction ParseChar(char c)
        {
            return c switch
            {
                'N' => Direction.North,
                'U' => Direction.North,
                'W' => Direction.West,
                'L' => Direction.West,
                'E' => Direction.East,
                'R' => Direction.East,
                'S' => Direction.South,
                'D' => Direction.South,
                _ => throw new NotImplementedException(),
            };
        }

        public static Direction GetOpposite(this Direction d)
        {
            return d switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.West => Direction.East,
                Direction.East => Direction.West,
                _ => throw new NotImplementedException(),
            };
        }

        public static Direction GetCW(this Direction d)
        {
            return d switch
            {
                Direction.North => Direction.East,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                Direction.East => Direction.South,
                _ => throw new NotImplementedException(),
            };
        }

        public static Direction GetCCW(this Direction d)
        {
            return d switch
            {
                Direction.North => Direction.West,
                Direction.South => Direction.East,
                Direction.West => Direction.South,
                Direction.East => Direction.North,
                _ => throw new NotImplementedException(),
            };
        }

        public static (int x, int y) GetMovement(this Direction direction, (int x, int y) currentPosition)
        {
            var (x, y) = direction.GetMovement();
            return (currentPosition.x + x, currentPosition.y + y);
        }

        public static (long x, long y) GetMovement(this Direction direction, (long x, long y) currentPosition, long distanceMoved)
        {
            var (x, y) = direction.GetMovement();
            return (currentPosition.x + x * distanceMoved, currentPosition.y + y * distanceMoved);
        }
    }
}
