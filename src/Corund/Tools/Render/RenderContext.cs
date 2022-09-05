using System;
using Corund.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Tools.Render;

/// <summary>
/// Temporary context for rendering to a texture.
/// </summary>
public class RenderContext: IDisposable
{
    public RenderContext(RenderTarget2D rt, Color? clearColor = null)
    {
        GameEngine.Render.PushContext(rt, clearColor);
    }

    public void Dispose()
    {
        GameEngine.Render.PopContext();
    }
}