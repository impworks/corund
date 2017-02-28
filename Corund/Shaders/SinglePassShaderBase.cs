using System;
using Corund.Engine;
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
            // PASS 1: inner -> RT
            {
                GameEngine.Render.PushContext(_renderTarget, Color.Transparent);

                innerDraw();

                GameEngine.Render.PopContext();
            }

            // PASS 2: RT -> base, overlay
            {
                _effect.Parameters["WorldViewProjection"].SetValue(GameEngine.Render.WorldViewProjection);

                ConfigureShader(obj);

                GameEngine.Render.SpriteBatch.Begin(0, BlendState.AlphaBlend, GameEngine.Render.GetSamplerState(false), null, null, _effect);
                GameEngine.Render.SpriteBatch.Draw(_renderTarget, RenderTargetRect, Color.White);
                GameEngine.Render.SpriteBatch.End();
            }
        }

        /// <summary>
        /// Sets required arguments to the shader.
        /// </summary>
        protected abstract void ConfigureShader(DynamicObject obj);

        #endregion
    }
}
