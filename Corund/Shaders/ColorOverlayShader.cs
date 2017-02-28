using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders
{
    /// <summary>
    /// A shader that draws an object filled with specified color.
    /// </summary>
    public class ColorOverlayShader: SinglePassShaderBase
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

        /// <summary>
        /// Sets shader configuration for rendering.
        /// </summary>
        protected override void ConfigureShader(DynamicObject obj)
        {
            _effect.Parameters["OverlayColor"].SetValue(Color.ToVector3());
            _effect.Parameters["OverlayOpacity"].SetValue(Opacity);
        }

        #endregion
    }
}
