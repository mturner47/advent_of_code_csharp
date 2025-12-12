namespace Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string? CoalesceNoBlanks(params string[] strings)
        {
            return strings.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
        }

        public static int ToInt(this string s)
        {
            var result = s.ToNullableInt();
            if (result.HasValue) return result.Value;
            throw new Exception($"Not an int: {s}");
        }

        public static uint ToUInt(this string s)
        {
            var result = s.ToNullableUInt();
            if (result.HasValue) return result.Value;
            throw new Exception($"Not a uint: {s}");
        }

        public static double ToDouble(this string s)
        {
            var result = s.ToNullableDouble();
            if (result.HasValue) return result.Value;
            throw new Exception($"Not a double: {s}");
        }

        public static int? ToNullableInt(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return int.TryParse(s.Trim(), out var i) ? i : null;
        }

        public static uint? ToNullableUInt(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return uint.TryParse(s.Trim(), out var i) ? i : null;
        }

        public static double? ToNullableDouble(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return double.TryParse(s.Trim(), out var i) ? i : null;
        }

        public static DateTime? ToNullableDateTime(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return DateTime.TryParse(s.Trim(), out var i) ? i : null;
        }

        public static decimal? ToNullableDecimal(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return decimal.TryParse(s.Trim(), out var i) ? i : null;
        }

        public static Guid? ToNullableGuid(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return Guid.TryParse(s.Trim(), out var i) ? i : null;
        }

        public static (double x, double y) To2dPoint(this string s)
        {
            var parts = s.Split(",");
            return (parts[0].ToDouble(), parts[1].ToDouble());
        }
    }
}
