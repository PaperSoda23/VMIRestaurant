﻿using System;

namespace VMIRestaurant.extensions
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}