using Corund.Engine;
using Corund.Geometry;
using Microsoft.Xna.Framework;

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

        #region Methods

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

            var transform = GetTransformInfo();
            var otherTransform = other.GetTransformInfo();

            return Geometry.Overlaps(other.Geometry, transform, otherTransform);
        }

        /// <summary>
        /// Checks if the object is inside bounds.
        /// </summary>
        public bool IsInside(Rectangle bounds)
        {
            return Geometry?.IsInsideBounds(bounds, GetTransformInfo()) ?? false;
        }

        /// <summary>
        /// Checks if the object is inside bounds.
        /// </summary>
        public bool IsOutside(Rectangle bounds)
        {
            return Geometry?.IsOutsideBounds(bounds, GetTransformInfo()) ?? false;
        }

        #endregion
    }
}
