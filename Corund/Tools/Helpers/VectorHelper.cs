﻿using System;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Helpers
{
    /// <summary>
    /// A collection of helper methods for vectors.
    /// </summary>
    public static class VectorHelper
    {
        #region Methods

        /// <summary>
        /// Rotates the vector around its origin point by given amount of degrees.
        /// </summary>
        public static Vector2 Rotate(this Vector2 vector, float angle)
        {
            if (angle.IsAlmostZero())
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
        /// Limits the vector size within two vectors.
        /// </summary>
        public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
        {
            return new Vector2(
                MathHelper.Clamp(value.X, min.X, max.X),
                MathHelper.Clamp(value.Y, min.Y, max.Y)
            );
        }

        #endregion
    }
}
