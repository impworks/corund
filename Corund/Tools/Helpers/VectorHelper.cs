using System;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Helpers
{
    /// <summary>
    /// A collection of helper methods for vectors.
    /// </summary>
    public static class VectorHelper
    {
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
    }
}
