using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using lifebook.core.cqrses.Domains;
using lifebook.core.processmanager.Attributes;

namespace lifebook.core.processmanager.Aggregates
{
    internal class ProcessManager
    {
        private static Dictionary<string, MethodInfo> eventNameToMethods = new Dictionary<string, MethodInfo>();
        private static object _lock = new object();

        public void Handle(List<AggregateEvent> events)
        {
            BuildEventNameToMethodInfoDictionary();
            
        }

        private void BuildEventNameToMethodInfoDictionary()
        {
            if (eventNameToMethods.Count == 0)
            {
                lock (_lock)
                {
                    if (eventNameToMethods.Count == 0)
                    {
                        var mi = GetType().GetMethods(System.Reflection.BindingFlags.NonPublic)
                                        .Where(m => m.GetCustomAttributes(typeof(UponProcessEvent), false).Count() > 0);

                        foreach (var m in mi)
                        {
                            eventNameToMethods = eventNameToMethods.MergeDictionaries(m.GetCustomAttributes(typeof(UponProcessEvent), false)
                             .Select(a => (UponProcessEvent)a)
                             .Select(a => a.EventName)
                             .ToDictionary(value => value, key => m));
                        }
                    }
                }
            }
        }
    }

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
