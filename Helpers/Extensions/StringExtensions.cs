namespace Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string? CoalesceNoBlanks(params string[] strings)
        {
            return strings.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
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

        public static decimal? ToNullableDouble(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return decimal.TryParse(s.Trim(), out var i) ? i : null;
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
    }
}
