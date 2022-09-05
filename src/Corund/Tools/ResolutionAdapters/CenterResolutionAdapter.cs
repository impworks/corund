using Corund.Engine;
using Microsoft.Xna.Framework;

namespace Corund.Tools.ResolutionAdapters;

/// <summary>
/// Resolution adapter that centers the logical screen inside the physical one, without resize.
/// </summary>
public class CenterResolutionAdapter: IResolutionAdapter
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="desiredSize">Desired screen size/</param>
    public CenterResolutionAdapter(Vector2 desiredSize)
    {
        _desiredSize = desiredSize;
    }

    /// <summary>
    /// Preferable screen size provided during configuration.
    /// </summary>
    protected readonly Vector2 _desiredSize;

    #region IResolutionAdapter implementation
    
    public virtual Vector2 GetLogicalScreenSize(Vector2 nativeSize) => _desiredSize;
    public virtual TransformInfo GetFrameTransformInfo(Vector2 frameViewSize)
    {
        var offset = (GameEngine.Screen.NativeSize - frameViewSize) / 2;
        return new TransformInfo(offset, 0, Vector2.One);
    }

    #endregion
}