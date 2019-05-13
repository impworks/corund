using System;
using Corund.Engine;
using Corund.Tools.Render;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders
{
    /// <summary>
    /// Draws an object to the screen using the shader.
    /// </summary>
    public abstract class SinglePassShaderBase: ShaderBase
    {
        #region Methods

        public override void DrawWrapper(DynamicObject obj, Action innerDraw)
        {
            using (var rt = GameEngine.Render.LeaseRenderTarget())
            {
                // PASS 1: inner -> RT
                {
                    using (new RenderContext(rt.RenderTarget, Color.Transparent))
                        innerDraw();
                }

                // PASS 2: RT -> base, overlay
                {
                    _effect.Parameters["WorldViewProjection"].SetValue(GameEngine.Render.WorldViewProjection);

                    ConfigureShader(obj);

                    GameEngine.Render.SpriteBatch.Begin(
                        0,
                        BlendState.AlphaBlend,
                        GameEngine.Render.GetSamplerState(false),
                        null,
                        null,
                        _effect
                    );
                    GameEngine.Render.SpriteBatch.Draw(rt.RenderTarget, RenderTargetRect, Color.White);
                    GameEngine.Render.SpriteBatch.End();
                }
            }
        }

        /// <summary>
        /// Sets required arguments to the shader.
        /// </summary>
        protected abstract void ConfigureShader(DynamicObject obj);

        #endregion
    }
}
