using System.Diagnostics;
using Corund.Tools;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

namespace Corund.Geometry;

/// <summary>
/// A rectangle in polygon representation, can be rotated.
/// </summary>
[DebuggerDisplay("RectPolygon ({Points})")]
public struct RectPolygon
{
    #region Constructors

    /// <summary>
    /// Creates an axis-aligned polygon from 2 points.
    /// </summary>
    public RectPolygon(Vector2 leftUpper, Vector2 rightLower)
    {
        LeftUpper = leftUpper;
        RightUpper = new Vector2(rightLower.X, leftUpper.Y);
        RightLower = rightLower;
        LeftLower = new Vector2(leftUpper.X, rightLower.Y);
        Angle = 0;
    }

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

    #region Properties

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

    #endregion

    #region Public methods

    /// <summary>
    /// Checks if the point is inside the rectangle.
    /// </summary>
    public bool ContainsPoint(Vector2 point)
    {
        // rotate everything around (0, 0) to axis-align the rect
        var leftUp = LeftUpper.Rotate(-Angle);
        var rightLow = RightLower.Rotate(-Angle);
        var pt = point.Rotate(-Angle);

        return pt.X >= leftUp.X
               && pt.Y >= leftUp.Y
               && pt.X <= rightLow.X
               && pt.Y <= rightLow.Y;
    }

    /// <summary>
    /// Checks if two rects overlap.
    /// </summary>
    public bool Overlaps(RectPolygon other)
    {
        if (AreRectsTooFar(this, other))
            return false;

        if (Angle.IsAlmostZero() && other.Angle.IsAlmostZero())
            return TestAlignedCollision(this, other);

        return TestOrientedCollision(this, other);
    }

    /// <summary>
    /// Checks if the polygon is completely inside the rectangle.
    /// </summary>
    public bool IsInsideBounds(Rectangle bounds)
    {
        return bounds.Contains(LeftUpper)
               && bounds.Contains(RightUpper)
               && bounds.Contains(RightLower)
               && bounds.Contains(LeftLower);
    }

    /// <summary>
    /// Checks if the polygon is completely outside the rectangle.
    /// </summary>
    public bool IsOutsideBounds(Rectangle bounds)
    {
        return !bounds.Contains(LeftUpper)
               && !bounds.Contains(RightUpper)
               && !bounds.Contains(RightLower)
               && !bounds.Contains(LeftLower);
    }

    /// <summary>
    /// Checks if the polygon crosses the specified side of the rectangle bounds.
    /// </summary>
    public bool CrossesBounds(Rectangle bounds, RectSide side)
    {
        return (side.HasFlag(RectSide.Left) && RCrossesX(this, bounds.Left) && RInsideY(this, bounds))
               || (side.HasFlag(RectSide.Right) && RCrossesX(this, bounds.Right) && RInsideY(this, bounds))
               || (side.HasFlag(RectSide.Top) && RCrossesY(this, bounds.Top) && RInsideX(this, bounds))
               || (side.HasFlag(RectSide.Bottom) && RCrossesY(this, bounds.Bottom) && RInsideX(this, bounds));

        // Checks that the points lie at the different sides of a horizontal line
        static bool RCrossesY(RectPolygon r, float y)
        {
            var ll = r.LeftLower.Y > y;
            var lu = r.LeftUpper.Y > y;
            var rl = r.RightLower.Y > y;
            var ru = r.RightUpper.Y > y;
            return ll != lu || ll != rl || ll != ru;
        }

        // Checks that the points lie at the different sides of a vertical line
        static bool RCrossesX(RectPolygon r, float x)
        {
            var ll = r.LeftLower.X > x;
            var lu = r.LeftUpper.X > x;
            var rl = r.RightLower.X > x;
            var ru = r.RightUpper.X > x;
            return ll != lu || ll != rl || ll != ru;
        }

        // Checks that all points lie between the top and bottom sides of the bounds
        static bool RInsideY(RectPolygon r, Rectangle b)
        {
            return InsideY(r.LeftUpper, b)
                   && InsideY(r.LeftLower, b)
                   && InsideY(r.RightUpper, b)
                   && InsideY(r.RightLower, b);
        }

        // Checks that all points lie between the left and right sides of the bounds
        static bool RInsideX(RectPolygon r, Rectangle b)
        {
            return InsideX(r.LeftUpper, b)
                   && InsideX(r.LeftLower, b)
                   && InsideX(r.RightUpper, b)
                   && InsideX(r.RightLower, b);
        }

        static bool InsideY(Vector2 p, Rectangle b) => p.Y >= b.Top && p.Y <= b.Bottom;
        static bool InsideX(Vector2 p, Rectangle b) => p.X >= b.Left && p.X <= b.Right;
    }

    #endregion

    #region Private helpers

    /// <summary>
    /// Performs a fast radius-based check if two polygons can possibly collide.
    /// </summary>
    private static bool AreRectsTooFar(RectPolygon rect1, RectPolygon rect2)
    {
        var radius = rect1.Radius + rect2.Radius;
        var dist = (rect1.Center - rect2.Center).Length();
        return dist > radius;
    }

    /// <summary>
    /// Performs a fast check for axis-aligned boxes.
    /// </summary>
    private static bool TestAlignedCollision(RectPolygon rect1, RectPolygon rect2)
    {
        var size1 = rect1.Size;
        var size2 = rect2.Size;
        var r1 = rect1.LeftUpper;
        var r2 = rect2.LeftUpper;

        return r1.X <= r2.X + size2.X
               && r1.X + size1.X >= r2.X
               && r1.Y <= r2.Y + size2.Y
               && r1.Y + size1.Y >= r2.Y;
    }

    /// <summary>
    /// Performs full collision detection by axis separation.
    /// </summary>
    private bool TestOrientedCollision(RectPolygon rect1, RectPolygon rect2)
    {
        var points1 = rect1.Points;
        var points2 = rect2.Points;

        return HasProjectionOverlapOnAxis(rect1.RightUpper - rect1.LeftUpper, points1, points2)
               && HasProjectionOverlapOnAxis(rect1.RightUpper - rect1.RightLower, points1, points2)
               && HasProjectionOverlapOnAxis(rect2.LeftUpper - rect2.LeftLower, points1, points2)
               && HasProjectionOverlapOnAxis(rect2.LeftUpper - rect2.RightUpper, points1, points2);
    }

    /// <summary>
    /// Checks if rect projections intersect on specified axis.
    /// </summary>
    private bool HasProjectionOverlapOnAxis(Vector2 axis, Vector2[] points1, Vector2[] points2)
    {
        var proj1 = FindProjectionRanges(axis, points1);
        var proj2 = FindProjectionRanges(axis, points2);
        return proj1.Min <= proj2.Max && proj2.Max >= proj1.Min;
    }

    /// <summary>
    /// Returns the range (min and max) values of all rectangle points projected onto the axis.
    /// </summary>
    private FloatRange FindProjectionRanges(Vector2 axis, Vector2[] points)
    {
        float? min = null, max = null;

        for (var idx = 0; idx < points.Length; idx++)
        {
            // a little trick here: we need to get a relative point the axis to compare with other points
            // the number is only meaningful when comparing with other points on the same axis
            // simplified formula from here:
            // https://www.gamedev.net/resources/_/technical/game-programming/2d-rotated-rectangle-collision-r2604

            var point = points[idx];
            var prod = point.X * axis.X + point.Y * axis.Y;

            if (min == null || min > prod)
                min = prod;
            if (max == null || max < prod)
                max = prod;
        }

        return new FloatRange(min.Value, max.Value);
    }

    #endregion
}