using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Gem.Engine.AI.Steering
{
    public static class EnumerableExtensions
    {
        public static Vector2 Sum<T>(this IEnumerable<T> enumerable, Func<T, Vector2> addition)
        {
            var result = Vector2.Zero;
            foreach (var obj in enumerable)
            {
                result += addition(obj);
            }
            return result;
        }
    }
}
