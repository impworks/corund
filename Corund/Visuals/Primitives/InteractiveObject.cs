using System.Collections.Generic;
using Corund.Engine;
using Corund.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Visuals.Primitives
{
    /// <summary>
    /// An object with geometry that can test collision with other objects.
    /// </summary>
    public abstract class InteractiveObject: DynamicObject
    {
        #region Fields

        /// <summary>
        /// Geometry of the current object.
        /// </summary>
        public abstract IGeometry Geometry { get; }

        #endregion

        #region Collision detection

        /// <summary>
        /// Checks if two objects overlap via their geometries.
        /// </summary>
        public bool Overlaps(InteractiveObject other)
        {
            if (Geometry == null || other.Geometry == null)
                return false;

            // shortcut for objects within the same group:
            // skip coordinate translation
            if (other.Parent == Parent)
                return Geometry.Overlaps(other.Geometry, null, null);

            var transform = GetTransformInfo(false);
            var otherTransform = other.GetTransformInfo(false);

            return Geometry.Overlaps(other.Geometry, transform, otherTransform);
        }

        #endregion

        #region Bounds

        /// <summary>
        /// Checks if the object is completely inside bounds.
        /// </summary>
        public bool IsInside(Rectangle bounds)
        {
            return Geometry?.IsInsideBounds(bounds, GetTransformInfo(false)) ?? false;
        }

        /// <summary>
        /// Checks if the object is completely outside bounds.
        /// </summary>
        public bool IsOutside(Rectangle bounds)
        {
            return Geometry?.IsOutsideBounds(bounds, GetTransformInfo(false)) ?? false;
        }

        /// <summary>
        /// Checks if the current object is completely inside the frame.
        /// </summary>
        public bool IsInsideFrame()
        {
            return IsInside(GameEngine.Current.Frame.Bounds);
        }

        /// <summary>
        /// Checks if the current object is completely outside the frame.
        /// </summary>
        public bool IsOutsideFrame()
        {
            return IsOutside(GameEngine.Current.Frame.Bounds);
        }

        #endregion

        #region Touch

        /// <summary>
        /// Attempts to get a touch location for current object.
        /// </summary>
        /// <param name="tapThrough">Flag indicating that touch should be available for underlying objects as well.</param>
        public TouchLocation? TryGetTouch(bool tapThrough = false)
        {
            if (Geometry == null)
                return null;

            var transform = GetTransformInfo(false);
            foreach (var touch in GameEngine.Current.Touches)
            {
                if (!GameEngine.Touch.CanHandleTouch(touch, this))
                    continue;

                if (Geometry.ContainsPoint(touch.Position, transform))
                {
                    if(!tapThrough)
                        GameEngine.Touch.HandleTouch(touch, this);

                    return touch;
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to get all touch locations for current object.
        /// </summary>
        /// <param name="tapThrough">Flag indicating that touch should be available for underlying objects as well.</param>
        public IList<TouchLocation> TryGetTouches(bool tapThrough = false)
        {
            List<TouchLocation> result = null;

            if (Geometry == null)
                return result;

            var transform = GetTransformInfo(false);
            foreach (var touch in GameEngine.Current.Touches)
            {
                if (!GameEngine.Touch.CanHandleTouch(touch, this))
                    continue;

                if (Geometry.ContainsPoint(touch.Position, transform))
                {
                    if (!tapThrough)
                        GameEngine.Touch.HandleTouch(touch, this);

                    if(result == null)
                        result = new List<TouchLocation>();

                    result.Add(touch);
                }
            }

            return result;
        }

        #endregion
    }
}
