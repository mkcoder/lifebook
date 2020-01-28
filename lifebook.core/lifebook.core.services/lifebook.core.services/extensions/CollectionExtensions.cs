using System;
using System.Collections.Generic;
using System.Linq;

namespace lifebook.core.services.extensions
{
    public static class CollectionExtensions
    {
        public static Dictionary<TKey, TValue> MergeDictionaries<TKey, TValue>(this Dictionary<TKey, TValue> t, Dictionary<TKey, TValue> o)
        {
            return t.SelectMany(a => o)
                    .ToLookup(pair => pair.Key, pair => pair.Value)
                    .ToDictionary(group => group.Key, group => group.First());
        }
    }
}
