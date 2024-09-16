namespace AdventOfCode.DaySolvers.Year2017
{
    public static class Shared2017
    {
        public static string KnotHash(string input)
        {
            var size = 256;
            var iterations = 64;
            var lengths = input.Select(c => (int)c).ToList();
            lengths.AddRange([17, 31, 73, 47, 23]);
            var values = Enumerable.Range(0, size).ToList();

            var currentPosition = 0;
            var skipSize = 0;
            for (var iteration = 0; iteration < iterations; iteration++)
            {
                for (var i = 0; i < lengths.Count; i++)
                {
                    var length = lengths[i];
                    var start = currentPosition;
                    var end = (currentPosition + length - 1) % size;
                    for (var j = 1; j <= length / 2; j++)
                    {
                        (values[start], values[end]) = (values[end], values[start]);
                        start++;
                        end--;
                        if (end < 0) end = size - 1;
                        if (start == size) start = 0;
                    }
                    currentPosition = (currentPosition + length + skipSize) % size;
                    skipSize++;
                }
            }

            var finalValue = "";
            for (var i = 0; i < 16; i++)
            {
                var chunk = values.Skip(i * 16).Take(16).ToList();
                var xorValue = chunk[0] ^ chunk[1];
                for (var j = 2; j < chunk.Count; j++)
                {
                    xorValue ^= chunk[j];
                }
                finalValue += xorValue.ToString("X2");
            }
            return finalValue;
        }
    }
}
