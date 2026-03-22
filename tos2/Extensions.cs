using UnityEngine;

namespace MRK
{
    public static class Extensions
    {
        public static Rect Shrink(this Rect rect, float amount)
        {
            return new Rect(
                rect.x + amount,
                rect.y + amount,
                rect.width - 2 * amount,
                rect.height - 2 * amount
            );
        }

        public static Rect Add(this Rect rect, Rect other)
        {
            return new Rect(
                rect.x + other.x,
                rect.y + other.y,
                rect.width + other.width,
                rect.height + other.height
            );
        }
    }
}
