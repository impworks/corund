using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

namespace Corund.Geometry
{
    /// <summary>
    /// A rectangle in polygon representation, can be rotated.
    /// </summary>
    public struct RectPolygon
    {
        #region Constructors

        private RectPolygon(Vector2 lu, Vector2 ru, Vector2 rl, Vector2 ll, float angle)
        {
            LeftUpper = lu;
            RightUpper = ru;
            RightLower = rl;
            LeftLower = ll;
            Angle = angle;
        }

        /// <summary>
        /// Creates a new RectPolygon by specifying the center point and dimensions.
        /// Rectangle is axis-aligned.
        /// </summary>
        public static RectPolygon FromCenter(Vector2 center, Vector2 size)
        {
            var half = size / 2;
            return new RectPolygon(
                center + new Vector2(-half.X, -half.Y),
                center + new Vector2(+half.X, -half.Y),
                center + new Vector2(+half.X, +half.Y),
                center + new Vector2(-half.X, +half.Y),
                0
            );
        }

        /// <summary>
        /// Creates a new RectPolygon by specifying the center point, dimensions, and rotation angle.
        /// Rectangle is rotated around its center point.
        /// </summary>
        public static RectPolygon FromCenter(Vector2 center, Vector2 size, float angle)
        {
            var half = size / 2;
            return new RectPolygon(
                center + new Vector2(-half.X, -half.Y).Rotate(angle),
                center + new Vector2(+half.X, -half.Y).Rotate(angle),
                center + new Vector2(+half.X, +half.Y).Rotate(angle),
                center + new Vector2(-half.X, +half.Y).Rotate(angle),
                angle
            );
        }

        /// <summary>
        /// Creates a new RectPolygon by specifying opposite points on a diagonal.
        /// Rectangle is axis-aligned.
        /// </summary>
        public static RectPolygon FromCorners(Vector2 leftUpper, Vector2 rightLower)
        {
            return new RectPolygon(
                leftUpper,
                new Vector2(rightLower.X, leftUpper.Y),
                rightLower,
                new Vector2(leftUpper.X, rightLower.Y),
                0
            );
        }

        /// <summary>
        /// Creates a new RectPolygon by specifying opposite points on a diagonal (in original rotation), and the rotation angle.
        /// Rectangle is rotated around the (0, 0) point.
        /// </summary>
        public static RectPolygon FromCorners(Vector2 leftUpper, Vector2 rightLower, float angle)
        {
            return new RectPolygon(
                leftUpper.Rotate(angle),
                new Vector2(rightLower.X, leftUpper.Y).Rotate(angle),
                rightLower.Rotate(angle),
                new Vector2(leftUpper.X, rightLower.Y).Rotate(angle),
                0
            );
        }

        #endregion

        #region Fields

        public readonly Vector2 LeftUpper;
        public readonly Vector2 RightUpper;
        public readonly Vector2 RightLower;
        public readonly Vector2 LeftLower;
        public readonly float Angle;

        #endregion

        /// <summary>
        /// Gets the center point of the rectangle.
        /// </summary>
        public Vector2 Center => LeftUpper + (RightLower - LeftUpper)/2;

        /// <summary>
        /// Gets the radius of the circumscribed circle.
        /// </summary>
        public float Radius => ((RightLower - LeftUpper)/2).Length();

        /// <summary>
        /// Gets the size of the rectangle (non axis-aligned).
        /// </summary>
        public Vector2 Size => new Vector2(RightUpper.X - LeftUpper.X, RightLower.Y - RightUpper.Y);

        /// <summary>
        /// Gets an array of rectangle points in clockwise order.
        /// </summary>
        public Vector2[] Points => new[] {LeftUpper, RightUpper, RightLower, LeftLower};
    }
}
