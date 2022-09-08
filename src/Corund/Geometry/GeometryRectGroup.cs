using System.Collections.Generic;
using System.Diagnostics;
using Corund.Tools;
using Microsoft.Xna.Framework;

namespace Corund.Geometry;

/// <summary>
/// A set of rectangles comprising a single shape.
/// </summary>
[DebuggerDisplay("GeometryRectGroup ({Rectangles.Count} rects)")]
public class GeometryRectGroup: IGeometry
{
    #region Constructor

    public GeometryRectGroup(IReadOnlyList<GeometryRect> rects)
    {
        Rectangles = rects;
    }

    public GeometryRectGroup(params GeometryRect[] rects)
    {
        Rectangles = rects;
    }

    #endregion

    #region Fields

    /// <summary>
    /// List of rectangles in the group.
    /// </summary>
    public IReadOnlyList<GeometryRect> Rectangles;

    #endregion

    #region IGeometry implementation

    /// <summary>
    /// Checks if any of the rectangles in the group contain the point.
    /// </summary>
    public bool ContainsPoint(Vector2 point, TransformInfo? selfTransform)
    {
        for(var idx = 0; idx < Rectangles.Count; idx++)
            if (Rectangles[idx].ContainsPoint(point, selfTransform))
                return true;

        return false;
    }

    /// <summary>
    /// Checks if the group overlaps another geometry.
    /// </summary>
    public bool Overlaps(IGeometry other, TransformInfo? selfTransform, TransformInfo? otherTransform)
    {
        if (other is GeometryRect otherRect)
            return Overlaps(otherRect, selfTransform, otherTransform);

        if (other is GeometryRectGroup otherGroup)
            return Overlaps(otherGroup, selfTransform, otherTransform);

        return other.Overlaps(this, otherTransform, selfTransform);
    }

    /// <summary>
    /// Checks if the group overlaps another rectangle.
    /// </summary>
    public bool Overlaps(GeometryRect other, TransformInfo? selfTransform, TransformInfo? otherTransform)
    {
        var otherPoly = other.CreateRectPolygon(otherTransform);

        for (var idx = 0; idx < Rectangles.Count; idx++)
        {
            var poly = Rectangles[idx].CreateRectPolygon(selfTransform);
            if (poly.Overlaps(otherPoly))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the group overlaps another rectangle.
    /// </summary>
    public bool Overlaps(GeometryRectGroup other, TransformInfo? selfTransform, TransformInfo? otherTransform)
    {
        // pre-transform all rects
        var polys = new RectPolygon[Rectangles.Count];

        for (var idx = 0; idx < Rectangles.Count; idx++)
            polys[idx] = Rectangles[idx].CreateRectPolygon(selfTransform);

        // check any-x-any overlap
        for (var idx = 0; idx < other.Rectangles.Count; idx++)
        {
            var otherPoly = other.Rectangles[idx].CreateRectPolygon(otherTransform);

            for (var idx2 = 0; idx2 < polys.Length; idx2++)
                if (polys[idx2].Overlaps(otherPoly))
                    return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the current geometry is inside bounds.
    /// </summary>
    public bool IsInsideBounds(Rectangle bounds, TransformInfo? selfTransform)
    {
        for (var idx = 0; idx < Rectangles.Count; idx++)
        {
            var poly = Rectangles[idx].CreateRectPolygon(selfTransform);
            if (!poly.IsInsideBounds(bounds))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the current geometry is inside bounds.
    /// </summary>
    public bool IsOutsideBounds(Rectangle bounds, TransformInfo? selfTransform)
    {
        for (var idx = 0; idx < Rectangles.Count; idx++)
        {
            var poly = Rectangles[idx].CreateRectPolygon(selfTransform);
            if (!poly.IsOutsideBounds(bounds))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the current geometry crosses the bounding rectangle on any of the sides.
    /// </summary>
    public bool CrossesBounds(Rectangle bounds, RectSide side, TransformInfo? selfTransform)
    {
        for (var idx = 0; idx < Rectangles.Count; idx++)
        {
            var poly = Rectangles[idx].CreateRectPolygon(selfTransform);
            if (poly.CrossesBounds(bounds, side))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the bounding box for the group of rectangles.
    /// </summary>
    public Rectangle GetBoundingBox(TransformInfo? selfTransform)
    {
        var bounds = new BoundingBoxBuilder();
        for (var idx = 0; idx < Rectangles.Count; idx++)
        {
            var poly = Rectangles[idx].CreateRectPolygon(selfTransform);
            bounds.AddPoint(poly.LeftUpper);
            bounds.AddPoint(poly.LeftLower);
            bounds.AddPoint(poly.RightUpper);
            bounds.AddPoint(poly.RightLower);
        }

        return bounds.GetRectangle();
    }

    #endregion
}