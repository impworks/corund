using System;
using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders
{
    /// <summary>
    /// A shader that draws an object filled with specified color.
    /// </summary>
    public class ColorOverlayShader: ShaderBase
    {
        #region Constructor

        public ColorOverlayShader(Color color, float opacity = 1f)
        {
            Color = color;
            Opacity = opacity;

            _effect = GameEngine.EmbeddedContent.Load<Effect>("color-overlay");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Overlay color.
        /// </summary>
        public Color Color;

        /// <summary>
        /// Overlay opacity.
        /// </summary>
        public float Opacity;

        #endregion

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

                _effect.Parameters["OverlayColor"].SetValue(Color.ToVector3());
                _effect.Parameters["OverlayOpacity"].SetValue(Opacity);

                GameEngine.Render.SpriteBatch.Begin(0, BlendState.AlphaBlend, GameEngine.Render.GetSamplerState(false), null, null, _effect);
                GameEngine.Render.SpriteBatch.Draw(_renderTarget, RenderTargetRect, Color.White);
                GameEngine.Render.SpriteBatch.End();
            }
        }

        #endregion
    }
}
