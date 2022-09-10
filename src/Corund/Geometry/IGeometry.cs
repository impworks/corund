using Corund.Tools;
using Microsoft.Xna.Framework;

namespace Corund.Geometry;

/// <summary>
/// Common interface for all geometry types.
/// </summary>
public interface IGeometry
{
    /// <summary>
    /// Checks if current geometry overlaps another geometry after applying transformations.
    /// </summary>
    bool Overlaps(IGeometry other, TransformInfo? selfTransform, TransformInfo? otherTransform);

    /// <summary>
    /// Checks if current geometry contains specified point after applying transformations.
    /// </summary>
    bool ContainsPoint(Vector2 point, TransformInfo? selfTransform);

    /// <summary>
    /// Checks if current geometry is completely inside bounds.
    /// </summary>
    bool IsInsideBounds(Rectangle bounds, TransformInfo? selfTransform);

    /// <summary>
    /// Checks if current geometry is completely outside bounds.
    /// </summary>
    bool IsOutsideBounds(Rectangle bounds, TransformInfo? selfTransform);

    /// <summary>
    /// Checks if current geometry enters or leaves the rectangle from the specified side.
    /// </summary>
    bool CrossesBounds(Rectangle bounds, RectSide side, TransformInfo? selfTransform, bool? leaves, Vector2? momentum);

    /// <summary>
    /// Returns the bounding box in parent coordinates, axes-aligned.
    /// </summary>
    Rectangle GetBoundingBox(TransformInfo? selfTransform);
}