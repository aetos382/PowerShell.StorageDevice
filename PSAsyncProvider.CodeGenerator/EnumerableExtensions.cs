using System.Collections.Generic;

namespace PSAsyncProvider.CodeGenerator
{
    internal static class EnumerableExtensions
    {
        public static bool TryGetValue<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source,
            TKey key,
            IEqualityComparer<TKey>? keyComparer,
            out TValue value)
        {
            keyComparer ??= EqualityComparer<TKey>.Default;

            foreach (var entry in source)
            {
                if (keyComparer.Equals(entry.Key, key))
                {
                    value = entry.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}
