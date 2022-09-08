using System.Diagnostics;
using Corund.Tools;
using Microsoft.Xna.Framework;

namespace Corund.Geometry;

/// <summary>
/// A single rectangle that can be tested for collision.
/// </summary>
[DebuggerDisplay("GeometryRect (Position: {Position}, Size: {Size})")]
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
    public Vector2 Position;

    /// <summary>
    /// Dimensions of the rectangle.
    /// </summary>
    public Vector2 Size;

    #endregion

    #region IGeometry implementation

    /// <summary>
    /// Checks if current geometry contains specified point after applying transformations.
    /// </summary>
    public bool ContainsPoint(Vector2 point, TransformInfo? selfTransform)
    {
        return CreateRectPolygon(selfTransform).ContainsPoint(point);
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
        return poly.Overlaps(otherPoly);
    }

    /// <summary>
    /// Checks if the current geometry is inside bounds.
    /// </summary>
    public bool IsInsideBounds(Rectangle bounds, TransformInfo? selfTransform)
    {
        return CreateRectPolygon(selfTransform).IsInsideBounds(bounds);
    }

    /// <summary>
    /// Checks if the current geometry is inside bounds.
    /// </summary>
    public bool IsOutsideBounds(Rectangle bounds, TransformInfo? selfTransform)
    {
        return CreateRectPolygon(selfTransform).IsOutsideBounds(bounds);
    }

    /// <summary>
    /// Checks if the current geometry crosses the bounding rectangle on any of the sides.
    /// </summary>
    public bool CrossesBounds(Rectangle bounds, RectSide side, TransformInfo? selfTransform)
    {
        return CreateRectPolygon(selfTransform).CrossesBounds(bounds, side);
    }

    /// <summary>
    /// Returns a bounding box for the geometry.
    /// </summary>
    public Rectangle GetBoundingBox(TransformInfo? selfTransform)
    {
        var poly = CreateRectPolygon(selfTransform);
        var bounds = new BoundingBoxBuilder();
        bounds.AddPoint(poly.LeftUpper);
        bounds.AddPoint(poly.LeftLower);
        bounds.AddPoint(poly.RightUpper);
        bounds.AddPoint(poly.RightLower);
        return bounds.GetRectangle();
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
        var ll = new Vector2(Position.X, Position.Y + Size.Y);

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