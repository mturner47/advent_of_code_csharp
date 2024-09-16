using System.Data;

namespace Helpers.Extensions
{
    public static class DataExtensions
    {
        public static bool IsEmpty(this DataTable dt)
        {
            return dt.Rows.Count < 1;
        }

        public static bool IsEmpty(this DataSet ds)
        {
            return ds.Tables.Count < 1 || ds.Tables[0].IsEmpty();
        }

        public static IEnumerable<T> ToModel<T>(this DataTable dt, Func<DataRow, T> convertFunction)
        {
            return dt.IsEmpty() ? new List<T>() : dt.AsEnumerable().Select(convertFunction);
        }

        public static IEnumerable<T> ToModel<T>(this DataSet ds, Func<DataRow, T> convertFunction)
        {
            return ds.IsEmpty() ? (IEnumerable<T>)new List<T>() : ds.Tables[0].ToModel(convertFunction);
        }

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this DataTable dt, Func<DataRow, TKey> keyFunction, Func<DataRow, TValue> valueFunction) where TKey : notnull
        {
            return dt.IsEmpty() ? new Dictionary<TKey, TValue>() : dt.AsEnumerable().ToDictionary(keyFunction, valueFunction);
        }

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this DataSet ds, Func<DataRow, TKey> keyFunction, Func<DataRow, TValue> valueFunction) where TKey : notnull
        {
            return ds.IsEmpty() ? new Dictionary<TKey, TValue>() : ds.Tables[0].ToDictionary(keyFunction, valueFunction);
        }
    }
}
