using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Interaction;

/// <summary>
/// Information about a detected swipe.
/// </summary>
public struct SwipeInfo
{
    public SwipeInfo(Vector2 origin, Vector2 vector, float duration)
    {
        Origin = origin;
        Vector = vector;
        Duration = duration;
    }

    /// <summary>
    /// The point where the swipe has originated (in frame coordinates).
    /// </summary>
    public readonly Vector2 Origin;

    /// <summary>
    /// The swipe vector (in frame coordinates).
    /// </summary>
    public readonly Vector2 Vector;

    /// <summary>
    /// The duration of the swipe (in seconds).
    /// </summary>
    public readonly float Duration;
}