using Microsoft.Xna.Framework;

namespace Corund.Tools.ResolutionAdapters;

/// <summary>
/// Resolution adapter that uses physical screen size.
/// </summary>
public class NativeResolutionAdapter: IResolutionAdapter
{
    public Vector2 GetLogicalScreenSize(Vector2 nativeSize) => nativeSize;
    public TransformInfo GetFrameTransformInfo(Vector2 frameViewSize) => TransformInfo.None;
}