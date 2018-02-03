using System;
using System.Collections.Generic;

namespace Speercs.Server.Utilities {
    public static class CollectionUtils {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) {
            if (!dict.ContainsKey(key))
                return dict[key] = value;
            return dict[key];
        }

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
            Func<TKey, TValue> valueFactory) {
            if (!dict.ContainsKey(key))
                return dict[key] = valueFactory(key);
            return dict[key];
        }
    }
}