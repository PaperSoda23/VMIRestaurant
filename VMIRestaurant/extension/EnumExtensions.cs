using System;

namespace VMIRestaurant.extension
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}