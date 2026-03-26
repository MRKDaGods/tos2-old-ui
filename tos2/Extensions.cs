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

        public static RectOffset CenterVertically(this Rect rect, float otherHeight)
        {
            return new RectOffset(
                0,
                0,
                Mathf.RoundToInt(rect.y + (rect.height - otherHeight) / 2f),
                0
            );
        }

        public static RectOffset CenterHorizontally(this Rect rect, float otherWidth)
        {
            return new RectOffset(
                Mathf.RoundToInt(rect.x + (rect.width - otherWidth) / 2f),
                0,
                0,
                0
            );
        }

        public static RectOffset Center(this Rect rect, Vector2 otherSize)
        {
            return new RectOffset(
                Mathf.RoundToInt(rect.x + (rect.width - otherSize.x) / 2f),
                0,
                Mathf.RoundToInt(rect.y + (rect.height - otherSize.y) / 2f),
                0
            );
        }

        public static Rect ToAbsolute(this Rect rect, Vector2 parentPos)
        {
            return new Rect(rect.x + parentPos.x, rect.y + parentPos.y, rect.width, rect.height);
        }
    }
}
