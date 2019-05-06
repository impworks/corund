using System.Collections.Generic;
using Corund.Engine;
using Corund.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Tools.Helpers
{
    /// <summary>
    /// Helper methods for working with geometry.
    /// </summary>
    public static class GeometryHelper
    {
        #region Collision detection

        /// <summary>
        /// Checks if two objects overlap via their geometries.
        /// </summary>
        public static bool Overlaps(this IGeometryObject obj, IGeometryObject other)
        {
            if (obj.Geometry == null || other.Geometry == null)
                return false;

            // shortcut for objects within the same group:
            // skip coordinate translation
            if (other.Parent == obj.Parent)
                return obj.Geometry.Overlaps(other.Geometry, null, null);

            var transform = obj.GetTransformInfo(false);
            var otherTransform = other.GetTransformInfo(false);

            return obj.Geometry.Overlaps(other.Geometry, transform, otherTransform);
        }

        #endregion

        #region Bounds

        /// <summary>
        /// Checks if the object is completely inside bounds.
        /// </summary>
        public static bool IsInside(this IGeometryObject obj, Rectangle bounds)
        {
            return obj.Geometry?.IsInsideBounds(bounds, obj.GetTransformInfo(false)) ?? false;
        }

        /// <summary>
        /// Checks if the object is completely outside bounds.
        /// </summary>
        public static bool IsOutside(this IGeometryObject obj, Rectangle bounds)
        {
            return obj.Geometry?.IsOutsideBounds(bounds, obj.GetTransformInfo(false)) ?? false;
        }

        /// <summary>
        /// Checks if the current object is completely inside the frame.
        /// </summary>
        public static bool IsInsideFrame(this IGeometryObject obj)
        {
            return obj.IsInside(GameEngine.Current.Frame.Bounds);
        }

        /// <summary>
        /// Checks if the current object is completely outside the frame.
        /// </summary>
        public static bool IsOutsideFrame(this IGeometryObject obj)
        {
            return obj.IsOutside(GameEngine.Current.Frame.Bounds);
        }

        #endregion

        #region Touch

        /// <summary>
        /// Attempts to get a touch location for current object.
        /// </summary>
        /// <param name="obj">Object with a geometry definition.</param>
        /// <param name="tapThrough">Flag indicating that touch should be available for underlying objects as well.</param>
        public static TouchLocation? TryGetTouch(this IGeometryObject obj, bool tapThrough = false)
        {
            return GameEngine.Touch.TryGetTouch(obj, tapThrough);
        }

        /// <summary>
        /// Attempts to get all touch locations for current object.
        /// </summary>
        /// <param name="obj">Object with a geometry definition.</param>
        /// <param name="tapThrough">Flag indicating that touch should be available for underlying objects as well.</param>
        public static IList<TouchLocation> TryGetTouches(this IGeometryObject obj, bool tapThrough = false)
        {
            if (obj.Geometry == null)
                return null;

            List<TouchLocation> result = null;
            var transform = obj.GetTransformInfo(false);
            foreach (var touch in GameEngine.Touch.LocalTouches)
            {
                if (obj.Geometry.ContainsPoint(touch.Position, transform))
                {
                    if (!tapThrough)
                        GameEngine.Touch.Handle(touch, obj);

                    if (result == null)
                        result = new List<TouchLocation>();

                    result.Add(touch);
                }
            }

            return result;
        }

        #endregion
    }
}
