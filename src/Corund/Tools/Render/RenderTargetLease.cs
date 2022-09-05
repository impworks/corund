using System;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Tools.Render;

/// <summary>
/// A handle for using a render target acquired from a pool.
/// </summary>
public class RenderTargetLease: IDisposable
{
    #region Constructor

    public RenderTargetLease(RenderTarget2D rt, Action disposeAction)
    {
        RenderTarget = rt ?? throw new ArgumentNullException(nameof(rt));
        _disposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
    }

    #endregion

    #region Fields

    private readonly Action _disposeAction;

    /// <summary>
    /// Temporarily acquired RenderTarget.
    /// </summary>
    public readonly RenderTarget2D RenderTarget;

    #endregion

    #region MyRegion

    public void Dispose()
    {
        _disposeAction();
    }

    #endregion
}