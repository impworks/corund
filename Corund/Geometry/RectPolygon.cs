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

        public RectPolygon(Vector2 center, Vector2 size)
        {
            var half = size/2;
            LeftUpper  = center + new Vector2(-half.X, -half.Y);
            RightUpper = center + new Vector2(+half.X, -half.Y);
            RightLower = center + new Vector2(+half.X, +half.Y);
            LeftLower  = center + new Vector2(-half.X, +half.Y);

            Angle = 0;
        }

        public RectPolygon(Vector2 center, Vector2 size, float angle)
        {
            var half = size / 2;
            LeftUpper  = center + new Vector2(-half.X, -half.Y).Rotate(angle);
            RightUpper = center + new Vector2(+half.X, -half.Y).Rotate(angle);
            RightLower = center + new Vector2(+half.X, +half.Y).Rotate(angle);
            LeftLower  = center + new Vector2(-half.X, +half.Y).Rotate(angle);

            Angle = 0;
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
