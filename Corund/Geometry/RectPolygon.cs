using Microsoft.Xna.Framework;

namespace Corund.Geometry
{
    /// <summary>
    /// A rectangle in polygon representation, can be rotated.
    /// </summary>
    public struct RectPolygon
    {
        #region Constructors

        /// <summary>
        /// Creates the polygon from 4 points of a rectangle.
        /// </summary>
        public RectPolygon(Vector2 leftUpper, Vector2 rightUpper, Vector2 rightLower, Vector2 leftLower, float angle)
        {
            LeftUpper = leftUpper;
            RightUpper = rightUpper;
            RightLower = rightLower;
            LeftLower = leftLower;
            Angle = angle;
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
