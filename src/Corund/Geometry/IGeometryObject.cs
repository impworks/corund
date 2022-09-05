using Corund.Tools;
using Corund.Visuals.Primitives;

namespace Corund.Geometry;

/// <summary>
/// Interface for objects that provide a geometry definition.
/// </summary>
public interface IGeometryObject
{
    /// <summary>
    /// Geometry definition.
    /// </summary>
    IGeometry Geometry { get; }

    /// <summary>
    /// Returns the transform for this object.
    /// </summary>
    TransformInfo GetTransformInfo(bool toScreen);

    /// <summary>
    /// Pointer to parent.
    /// </summary>
    ObjectBase Parent { get; }
}