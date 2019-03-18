using System.Linq;

namespace Assets.Scripts.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsIn<T>(this T item, params T[] items)
            where T : new()
        {
            return items.Any(x => x.Equals(item));
        }
    }
}