using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Movement;

/// <summary>
/// Segment of a straight line.
/// </summary>
public class LineSegment: IPathSegment
{
    #region Constructor

    public LineSegment(Vector2 point1, Vector2 point2)
    {
        Point1 = point1;
        Point2 = point2;
        Length = (Point2 - Point1).Length();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Starting point.
    /// </summary>
    public readonly Vector2 Point1;

    /// <summary>
    /// Ending point.
    /// </summary>
    public readonly Vector2 Point2;

    /// <summary>
    /// Gets the distance of the segment in pixels.
    /// </summary>
    public float Length { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Returns the position in the segment for state (0..1).
    /// </summary>
    public Vector2 GetPosition(float state)
    {
        var dist = (Point2 - Point1)*state;
        return Point1 + dist;
    }

    #endregion
}