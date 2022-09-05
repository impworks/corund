using Microsoft.Xna.Framework;

namespace Corund.Visuals.Primitives;

/// <summary>
/// Common interface for visual primitives that clip their contents.
/// </summary>
public interface IView
{
    /// <summary>
    /// Checks if the point, given in frame coordinates, is inside current view.
    /// </summary>
    bool IsPointInView(Vector2 point);
}