using Corund.Geometry;

namespace Corund.Visuals.Primitives
{
    /// <summary>
    /// An object with geometry that can test collision with other objects.
    /// </summary>
    public abstract class InteractiveObject: DynamicObject, IGeometryObject
    {
        #region Fields

        /// <summary>
        /// Geometry of the current object.
        /// </summary>
        public abstract IGeometry Geometry { get; }

        #endregion
    }
}
