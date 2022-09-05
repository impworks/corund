using Microsoft.Xna.Framework;

namespace Corund.Tools.ResolutionAdapters;

/// <summary>
/// Common interface for adapters handling different resolution sizes.
/// </summary>
public interface IResolutionAdapter
{
    /// <summary>
    /// Returns the screen size for any in-game calculations.
    /// </summary>
    Vector2 GetLogicalScreenSize(Vector2 nativeSize);

    /// <summary>
    /// Returns the transformation that must be applied to the frame when rendering to actual screen.
    /// </summary>
    TransformInfo GetFrameTransformInfo(Vector2 frameViewSize);
}