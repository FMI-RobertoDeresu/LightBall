using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<IList<T>> Split<T>(this IList<T> items, Func<T, bool> splitCondition, bool excludeSeparators = true)
        {
            var locations = items.Zip(Enumerable.Range(0, items.Count), (elem, index) => (elem, index))
                .Where(x => splitCondition(x.elem))
                .Select(x => x.index)
                .ToList();

            if (locations.All(x => x != items.Count - 1))
                locations.Add(items.Count - 1);

            var start = 0;
            foreach (var location in locations)
            {
                var stop = location + (excludeSeparators ? -1 : 0);
                if (start <= stop)
                    yield return items.Skip(start).Take(stop - start + 1).ToList();
                start = location + 1;
            }
        }
    }
}
