namespace Helpers.Extensions
{
    public static class CollectionExtensions
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(action);

            foreach (var item in source)
            {
                action(item);
            }
        }

        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> selector)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(selector);

            return source.All(x => !selector(x));
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(selector);

            return source.GroupBy(selector).Select(x => x.First());
        }

        public static List<TSource> AsOrToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) return [];
            return source as List<TSource> ?? source.ToList();
        }

        public static IEnumerable<TSource> WithNewFirstElement<TSource>(this IEnumerable<TSource> source, TSource newFirst)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(newFirst);

            yield return newFirst;
            foreach (var item in source)
            {
                yield return item;
            }
        }

        public static TValue? GetNullableValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : struct
        {
            if (dict == null) return null;
            return dict.TryGetValue(key, out var value) ? value : null;
        }

        public static List<string> Transpose(this List<string> strings)
        {
            var result = new List<string>();
            for (var x = 0; x < strings[0].Length; x++)
            {
                result.Add(new string(strings.Select(s => s[x]).ToArray()));
            }
            return result;
        }

        public static List<List<T>> Transpose<T>(this IList<IList<T>> items)
        {
            var result = new List<List<T>>();

            for (var i = 0; i < items.Count; i++)
            {
                result.Add(items.Select(j => j[i]).ToList());
            }
            return result;
        }

        public static List<string> RotateCCW(this List<string> strings)
        {
            var result = new List<string>();
            for (var x = 0; x < strings[0].Length; x++)
            {
                result.Add(new string(strings.Select(s => s[s.Length - x -1]).ToArray()));
            }
            return result;
        }

        public static List<string> RotateCW(this List<string> strings)
        {
            var result = new List<string>();
            for (var x = 0; x < strings[0].Length; x++)
            {
                result.Add(new string(strings.Select(s => s[x]).Reverse().ToArray()));
            }
            return result;
        }

        public static List<List<T>> RotateCCW<T>(this IList<IList<T>> items)
        {
            var result = new List<List<T>>();

            for (var i = 0; i < items.Count; i++)
            {
                result.Add(items.Select(j => j[j.Count - i - 1]).ToList());
            }
            return result;
        }

        public static List<List<T>> RotateCW<T>(this IList<IList<T>> items)
        {
            var result = new List<List<T>>();

            for (var i = 0; i < items.Count; i++)
            {
                result.Add(items.Select(j => j[i]).Reverse().ToList());
            }
            return result;
        }
    }
}
