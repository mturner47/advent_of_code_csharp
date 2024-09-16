using System.Diagnostics.CodeAnalysis;

namespace Helpers.Helpers
{
    public class LRUCache<TKey, TValue> where TKey : notnull
    {
        private readonly int _capacity;
        private readonly LinkedList<(TKey key, TValue value)> _lru = new();
        private readonly Dictionary<TKey, LinkedListNode<(TKey key, TValue value)>> _cache = new();

        public LRUCache(int capacity)
        {
            _capacity = capacity;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                _lru.Remove(node);
                _lru.AddLast(node);
                value = node.Value.value;
                return true;
            }
            value = default;
            return false;
        }

        public TValue Put(TKey key, TValue value)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                node.Value = (key, value);
                _lru.Remove(node);
                _lru.AddLast(node);
            }
            else
            {
                _lru.AddLast((key, value));
                _cache.Add(key, _lru.Last!);
            }

            if (_cache.Count > _capacity)
            {
                _cache.Remove(_lru.First!.Value.key);
                _lru.RemoveFirst();
            }

            return value;
        }
    }
}
