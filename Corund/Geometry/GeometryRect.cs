using Corund.Tools;
using Microsoft.Xna.Framework;

namespace Corund.Geometry
{
    /// <summary>
    /// A single 
    /// </summary>
    public class GeometryRect: IGeometry
    {
        #region Constructor

        public GeometryRect(float x, float y, float width, float height)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(width, height);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Position of the top left corner of the rectangle.
        /// </summary>
        public readonly Vector2 Position;

        /// <summary>
        /// Dimensions of the rectangle.
        /// </summary>
        public readonly Vector2 Size;

        #endregion

        #region IGeometry implementation

        /// <summary>
        /// Checks if current geometry contains specified point after applying transformations.
        /// </summary>
        public bool ContainsPoint(Vector2 point, TransformInfo? selfTransform)
        {
            var rect = CreateRectPolygon(selfTransform);
            return CollisionDetector.IsPointInsideRect(rect, point);
        }

        /// <summary>
        /// Checks if current rectangle overlaps another unspecified geometry.
        /// Used for double dispatching.
        /// </summary>
        public bool Overlaps(IGeometry other, TransformInfo? selfTransform, TransformInfo? otherTransform)
        {
            return (other as GeometryRect)?.Overlaps(this, otherTransform, selfTransform)
                ?? other.Overlaps(this, otherTransform, selfTransform);
        }

        /// <summary>
        /// Checks if current rectangle overlaps another rectangle.
        /// </summary>
        public bool Overlaps(GeometryRect other, TransformInfo? selfTransform, TransformInfo? otherTransform)
        {
            var poly = CreateRectPolygon(selfTransform);
            var otherPoly = other.CreateRectPolygon(otherTransform);
            return CollisionDetector.AreRectsOverlapping(poly, otherPoly);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Converts the rectangle points to scene coordinates.
        /// </summary>
        public RectPolygon CreateRectPolygon(TransformInfo? transform)
        {
            var lu = Position;
            var ru = new Vector2(Position.X + Size.X, Position.Y);
            var rl = Position + Size;
            var ll = new Vector2(Position.X, Position.X + Size.Y);

            if (!transform.HasValue)
                return new RectPolygon(lu, ru, rl, ll, 0);

            return new RectPolygon(
                transform.Value.Translate(lu),
                transform.Value.Translate(ru),
                transform.Value.Translate(rl),
                transform.Value.Translate(ll),
                transform.Value.Angle
            );
        }

        #endregion
    }
}
