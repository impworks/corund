using System;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Helpers
{
    /// <summary>
    /// Direction-related utilities.
    /// </summary>
    public static class KnownDirectionHelper
    {
        #region Constants

        /// <summary>
        /// Known directions, ordered from 0 to 2PI (counter-clockwise) and looped.
        /// </summary>
        public static KnownDirection[] OrderedDirections =
        {
            KnownDirection.Right,
            KnownDirection.RightDown,
            KnownDirection.Down,
            KnownDirection.LeftDown,
            KnownDirection.Left,
            KnownDirection.LeftUp,
            KnownDirection.Up,
            KnownDirection.RightUp,
            KnownDirection.Right
        };

        /// <summary>
        /// Distance between neighbouring directions in degrees.
        /// </summary>
        private const float DirectionDistance = 45f;

        #endregion

        #region Methods

        /// <summary>
        /// Finds a known direction (using specified precision).
        /// </summary>
        public static KnownDirection? GetDirection(this Vector2 vec, float precision = 0.5f)
        {
            var angle = Math.Atan2(vec.Y, vec.X) * 180 / MathHelper.Pi;
            if (angle < 0)
                angle += 360;

            var threshold = DirectionDistance * (1 - precision);

            for (var idx = 0; idx < OrderedDirections.Length; idx++)
            {
                var exact = idx * DirectionDistance;

                if (angle <= exact + threshold && angle >= exact - threshold)
                    return OrderedDirections[idx];

                if (angle < exact - threshold)
                    return null;
            }

            return null;
        }

        /// <summary>
        /// Returns the angle corresponding to this direction (in radians).
        /// </summary>
        public static float GetAngle(this KnownDirection dir)
        {
            var idx = Array.IndexOf(OrderedDirections, dir);
            var degrees = idx*DirectionDistance;
            return degrees/180*MathHelper.Pi;
        }

        /// <summary>
        /// Turns a direction to a vector of given length.
        /// </summary>
        public static Vector2 GetVector(this KnownDirection dir, float length = 1)
        {
            return VectorHelper.FromLength(length, dir.GetAngle());
        }

        #endregion
    }
}
