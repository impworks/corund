using System;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Helpers
{
    /// <summary>
    /// A collection of helper methods for vectors.
    /// </summary>
    public static class VectorHelper
    {
        #region Constants

        /// <summary>
        /// Known directions, ordered from 0 to 2PI (counter-clockwise) and looped.
        /// </summary>
        private static Direction[] OrderedDirections =
        {
            Direction.Right,
            Direction.RightUp,
            Direction.Up,
            Direction.LeftUp,
            Direction.Left,
            Direction.LeftDown,
            Direction.Down,
            Direction.RightDown,
            Direction.Right
        };

        #endregion

        #region Methods

        /// <summary>
        /// Rotates the vector around its origin point by given amount of degrees.
        /// </summary>
        public static Vector2 Rotate(this Vector2 vector, float angle)
        {
            if (angle == 0)
                return vector;

            var x = vector.X;
            var y = vector.Y;
            var distance = (float)Math.Sqrt(x*x + y*y);
            var newAngle = (float)Math.Atan2(y, x) + angle;

            return FromLength(distance, newAngle);
        }

        /// <summary>
        /// Creates a vector by angle and direction.
        /// </summary>
        public static Vector2 FromLength(float length, float angle)
        {
            return new Vector2(
                (float)(Math.Cos(angle) * length),
                (float)(Math.Sin(angle) * length)
            );
        }

        /// <summary>
        /// Calculates the direction between two points.
        /// </summary>
        public static float AngleTo(this Vector2 p1, Vector2 p2)
        {
            var vec = p2 - p1;
            return (float) Math.Atan2(vec.Y, vec.X);
        }

        /// <summary>
        /// Finds a known direction (using specified precision).
        /// </summary>
        public static Direction? GetDirection(this Vector2 vec, float precision = 0.5f)
        {
            const float fraction = 45f;

            var angle = Math.Atan2(vec.Y, vec.X) * 180 / MathHelper.Pi;
            var threshold = fraction*(1-precision);

            for (var idx = 0; idx < OrderedDirections.Length; idx++)
            {
                var exact = idx*fraction;

                if (angle <= exact + threshold && angle >= exact - threshold)
                    return OrderedDirections[idx];

                if (angle < exact - threshold)
                    return null;
            }

            return null;
        }

        #endregion
    }
}
