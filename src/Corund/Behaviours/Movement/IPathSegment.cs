using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Movement;

/// <summary>
/// Common interface for all path segments of any kind.
/// </summary>
public interface IPathSegment
{
    /// <summary>
    /// Length of the segment (in pixels).
    /// </summary>
    float Length { get; }

    /// <summary>
    /// Calculate the position for given state (0..1).
    /// </summary>
    Vector2 GetPosition(float state);
}