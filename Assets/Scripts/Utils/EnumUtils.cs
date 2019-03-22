using System;

namespace Assets.Scripts.Utils
{
    public static class EnumUtils
    {
        public static T Parse<T>(string value)
        {
            var enumValue = (T) Enum.Parse(typeof(T), value);
            return enumValue;
        }
    }
}