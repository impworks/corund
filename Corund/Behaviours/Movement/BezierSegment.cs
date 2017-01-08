using System;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Movement
{
    /// <summary>
    /// A smooth segment approximated by spline between 3 points.
    /// </summary>
    public class BezierSegment: IPathSegment
    {
        #region Constructor

        public BezierSegment(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;

            Length = GetCurveLength();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Starting point.
        /// </summary>
        public readonly Vector2 Point1;

        /// <summary>
        /// Middle point.
        /// </summary>
        public readonly Vector2 Point2;

        /// <summary>
        /// Ending point.
        /// </summary>
        public readonly Vector2 Point3;

        /// <summary>
        /// Length of the curve (in pixels).
        /// </summary>
        public float Length { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the position on the spline for state (0..1).
        /// </summary>
        public Vector2 GetPosition(float state)
        {
            var p1 = GetPoint(Point1, Point2, state);
            var p2 = GetPoint(Point2, Point3, state);
            return GetPoint(p1, p2, state);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Gets the length of the curve.
        /// http://segfaultlabs.com/docs/quadratic-bezier-curve-length
        /// </summary>
        private float GetCurveLength()
        {
            var aX = Point1.X - 2 * Point2.X + Point3.X;
            var aY = Point1.Y - 2 * Point2.Y + Point3.Y;
            var bX = 2 * (Point2.X - Point1.X);
            var bY = 2 * (Point2.Y - Point1.Y);

            var A = 4 * (aX * aX + aY * aY);
            var B = 4 * (aX * bX + aY * bY);
            var C = bX * bX + bY * bY;

            var Sabc = (float)(2 * Math.Sqrt(A + B + C));
            var A_2 = (float)Math.Sqrt(A);
            var A_32 = 2 * A * A_2;
            var C_2 = (float)(2 * Math.Sqrt(C));
            var BA = B / A_2;

            if ((BA + C_2).IsAlmostNull())
                return (Point2 - Point1).Length() + (Point3 - Point2).Length();

            return (A_32*Sabc + A_2*B*(Sabc - C_2) + (4*C*A - B*B)*(float) Math.Log((2*A_2 + BA + Sabc)/(BA + C_2)))/(4*A_32);
        }

        /// <summary>
        /// Finds a point on the line piece.
        /// </summary>
        /// <param name="p1">Line start.</param>
        /// <param name="p2">Line end.</param>
        /// <param name="percent">The point's position on the line piece.</param>
        private Vector2 GetPoint(Vector2 p1, Vector2 p2, float percent)
        {
            return p1 + (p2 - p1)*percent;
        }

        #endregion
    }
}
