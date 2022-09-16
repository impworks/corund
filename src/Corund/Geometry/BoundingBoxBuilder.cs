using Microsoft.Xna.Framework;

namespace Corund.Geometry;

/// <summary>
/// A helper for building a bounding box for a set of points.
/// </summary>
public class BoundingBoxBuilder
{
    #region Fields

    private bool _hasValues;
    private float _top;
    private float _left;
    private float _right;
    private float _bottom;

    #endregion

    #region Methods

    /// <summary>
    /// Adds a point to the area.
    /// </summary>
    public void AddPoint(Vector2 vec)
    {
        if (!_hasValues)
        {
            _left = _right = vec.X;
            _bottom = _top = vec.Y;
            _hasValues = true;
            return;
        }

        if (vec.X < _left)
            _left = vec.X;

        if (vec.X > _right)
            _right = vec.X;

        if (vec.Y < _top)
            _top = vec.Y;

        if (vec.Y > _bottom)
            _bottom = vec.Y;
    }

    /// <summary>
    /// Gets the bounding box rectangle.
    /// </summary>
    public Rectangle GetRectangle()
    {
        return new Rectangle(
            (int) _left,
            (int) _top,
            (int) (_right - _left),
            (int) (_bottom - _top)
        );
    }

    /// <summary>
    /// Gets the bounding box rectangle as a GeometryRect.
    /// </summary>
    public GeometryRect GetGeometryRect()
    {
        return new GeometryRect(
            _left,
            _top,
            _right - _left,
            _bottom - _top
        );
    }

    #endregion
}