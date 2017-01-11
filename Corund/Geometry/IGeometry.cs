using Corund.Tools;
using Microsoft.Xna.Framework;

namespace Corund.Geometry
{
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
    }
}
