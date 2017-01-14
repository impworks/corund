using System;
using System.Collections.Generic;
using Corund.Tools;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

namespace Corund.Geometry
{
    /// <summary>
    /// A collection of methods to detect collisions between geometry objects.
    /// </summary>
    public static class GeometryHelper
    {
        #region Point inside RectPolygon

        /// <summary>
        /// Checks if the point is inside the rectangle.
        /// </summary>
        public static bool IsPointInsideRect(RectPolygon rect, Vector2 point)
        {
            // rotate everything around (0, 0) to axis-align the rect
            var leftUp = rect.LeftUpper.Rotate(rect.Angle); 
            var rightLow = rect.RightLower.Rotate(rect.Angle);
            var pt = point.Rotate(rect.Angle);

            return pt.X >= leftUp.X
                   && pt.Y >= leftUp.Y
                   && pt.X <= rightLow.X
                   && pt.Y <= rightLow.Y;
        }

        #endregion

        #region RectPolygon collision

        /// <summary>
        /// Checks if two rects overlap.
        /// </summary>
        public static bool AreRectsOverlapping(RectPolygon rect1, RectPolygon rect2)
        {
            if (AreRectsTooFar(rect1, rect2))
                return false;

            if(rect1.Angle == 0 && rect2.Angle == 0)
                return TestAlignedCollision(rect1, rect2);

            return TestOrientedCollision(rect1, rect2);
        }

        /// <summary>
        /// Performs a fast radius-based check if two polygons can possibly collide.
        /// </summary>
        private static bool AreRectsTooFar(RectPolygon rect1, RectPolygon rect2)
        {
            var radius = rect1.Radius + rect2.Radius;
            var dist = (rect1.Center - rect2.Center).Length();
            return dist > radius;
        }

        /// <summary>
        /// Performs a fast check for axis-aligned boxes.
        /// </summary>
        private static bool TestAlignedCollision(RectPolygon rect1, RectPolygon rect2)
        {
            var size1 = rect1.Size;
            var size2 = rect2.Size;
            var r1 = rect1.LeftUpper;
            var r2 = rect2.LeftUpper;

            return r1.X <= r2.X + size2.X
                && r1.X + size1.X >= r2.X
                && r1.Y <= r2.Y + size2.Y
                && r1.Y + size1.Y >= r2.Y;
        }

        /// <summary>
        /// Performs full collision detection by axis separation.
        /// </summary>
        private static bool TestOrientedCollision(RectPolygon rect1, RectPolygon rect2)
        {
            var points1 = rect1.Points;
            var points2 = rect2.Points;

            return HasProjectionOverlapOnAxis(rect1.RightUpper - rect1.LeftUpper, points1, points2)
                   && HasProjectionOverlapOnAxis(rect1.RightUpper - rect1.RightLower, points1, points2)
                   && HasProjectionOverlapOnAxis(rect2.LeftUpper - rect2.LeftLower, points1, points2)
                   && HasProjectionOverlapOnAxis(rect2.LeftUpper - rect2.RightUpper, points1, points2);
        }

        #endregion

        #region Geometry combination

        /// <summary>
        /// Combines geometries into one.
        /// </summary>
        public static GeometryRectGroup Combine(params IGeometry[] items)
        {
            var result = new List<GeometryRect>(items.Length);

            foreach (var item in items)
            {
                var rect = item as GeometryRect;
                if (rect != null)
                {
                    result.Add(rect);
                    continue;
                }

                var group = item as GeometryRectGroup;
                if (group != null)
                {
                    result.AddRange(group.Rectangles);
                    continue;
                }

                throw new ArgumentException($"Unknown item type: '{item.GetType()}'");
            }

            return new GeometryRectGroup(result.ToArray());
        }

        #endregion

        #region Bound checking

        /// <summary>
        /// Checks if the polygon is completely inside the rectangle.
        /// </summary>
        public static bool IsRectInsideBounds(RectPolygon rect, Rectangle bounds)
        {
            return bounds.Contains(rect.LeftUpper)
                   && bounds.Contains(rect.RightUpper)
                   && bounds.Contains(rect.RightLower)
                   && bounds.Contains(rect.LeftLower);
        }

        /// <summary>
        /// Checks if the polygon is completely outside the rectangle.
        /// </summary>
        public static bool IsRectOutsideBounds(RectPolygon rect, Rectangle bounds)
        {
            return !bounds.Contains(rect.LeftUpper)
                   && !bounds.Contains(rect.RightUpper)
                   && !bounds.Contains(rect.RightLower)
                   && !bounds.Contains(rect.LeftLower);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Checks if rect projections intersect on specified axis.
        /// </summary>
        private static bool HasProjectionOverlapOnAxis(Vector2 axis, Vector2[] points1, Vector2[] points2)
        {
            var proj1 = FindProjectionRanges(axis, points1);
            var proj2 = FindProjectionRanges(axis, points2);
            return proj1.Min <= proj2.Max && proj2.Max >= proj1.Min;
        }

        /// <summary>
        /// Returns the range (min and max) values of all rectangle points projected onto the axis.
        /// </summary>
        private static Range FindProjectionRanges(Vector2 axis, Vector2[] points)
        {
            float? min = null, max = null;

            for (var idx = 0; idx < points.Length; idx++)
            {
                // a little trick here: we need to get a relative point the axis to compare with other points
                // the number is only meaningful when comparing with other points on the same axis
                // simplified formula from here:
                // https://www.gamedev.net/resources/_/technical/game-programming/2d-rotated-rectangle-collision-r2604

                var point = points[idx];
                var prod = point.X * axis.X + point.Y * axis.Y;

                if (min == null || min > prod)
                    min = prod;
                if (max == null || max < prod)
                    max = prod;
            }

            return new Range(min.Value, max.Value);
        }

        #endregion
    }
}
