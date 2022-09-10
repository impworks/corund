using System;
using System.Collections.Generic;
using Corund.Engine;
using Corund.Tools.Render;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders;

/// <summary>
/// A shader that combines multiple shaders together.
/// </summary>
public class MultiShader : IShader
{
    #region Constructor

    public MultiShader(params IShader[] shaders)
    {
        Shaders = new List<IShader>(shaders);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Currently available shaders.
    /// </summary>
    public List<IShader> Shaders;

    #endregion

    #region Methods

    /// <summary>
    /// Does nothing.
    /// </summary>
    public void Update()
    {
        foreach(var shader in Shaders)
            shader.Update();
    }

    /// <summary>
    /// Draws the shaders.
    /// </summary>
    public void DrawWrapper(DynamicObject obj, Action innerDraw)
    {
        if (Shaders.Count == 0)
        {
            innerDraw();
            return;
        }

        var currDraw = innerDraw;

        using var rt1 = GameEngine.Render.LeaseRenderTarget();
        using var rt2 = GameEngine.Render.LeaseRenderTarget();

        for(var idx = 0; idx < Shaders.Count; idx++)
        {
            var currRt = (idx % 2 == 0 ? rt1 : rt2).RenderTarget;
            var shader = Shaders[idx];

            using(new RenderContext(currRt, Color.Transparent))
                shader.DrawWrapper(obj, currDraw);

            currDraw = () => Draw(obj, currRt);
        }

        Draw(obj, (Shaders.Count % 2 == 0 ? rt2 : rt1).RenderTarget);
    }

    /// <summary>
    /// Draws the render target to an underlying surface.
    /// </summary>
    private void Draw(DynamicObject obj, Texture2D tex)
    {
        var zOrder = GameEngine.Current.ZOrderFunction(obj);
        GameEngine.Render.TryBeginBatch(BlendState.AlphaBlend);
        GameEngine.Render.SpriteBatch.Draw(
            tex,
            Vector2.Zero,
            null,
            Color.White,
            0,
            Vector2.Zero,
            1,
            SpriteEffects.None,
            zOrder
        );
    }

    #endregion
}