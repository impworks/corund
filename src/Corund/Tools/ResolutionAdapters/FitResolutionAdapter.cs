using Corund.Engine;
using Microsoft.Xna.Framework;
using System;

namespace Corund.Tools.ResolutionAdapters;

/// <summary>
/// Resolution adapter that centers the logical screen inside the physical one, without resize.
/// </summary>
public class FitResolutionAdapter : CenterResolutionAdapter
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="desiredSize">Desired screen size/</param>
    public FitResolutionAdapter(Vector2 desiredSize)
        : base(desiredSize)
    {
    }

    public override TransformInfo GetFrameTransformInfo(Vector2 frameViewSize)
    {
        var vp = GameEngine.Screen.NativeSize;
        var scale = 1.0f / Math.Max(frameViewSize.X / vp.X, frameViewSize.Y / vp.Y);
        var newSize = frameViewSize * scale;
        var offset = (vp - newSize) / 2;
        return new TransformInfo(offset, 0, new Vector2(scale));
    }
}