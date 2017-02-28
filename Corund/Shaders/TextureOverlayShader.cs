using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders
{
    public class TextureOverlayShader: SinglePassShaderBase
    {
        #region Constructor

        public TextureOverlayShader(Texture2D texture)
            : this(texture, Vector2.Zero, Vector2.One, 1)
        {

        }

        public TextureOverlayShader(Texture2D texture, Vector2 offset, Vector2 scale, float opacity)
        {
            Texture = texture;
            Offset = offset;
            Scale = scale;
            Opacity = opacity;

            _effect = GameEngine.EmbeddedContent.Load<Effect>("texture-overlay");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Overlay texture.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// Distance between object's hotspot and texture's origin.
        /// </summary>
        public Vector2 Offset;

        /// <summary>
        /// Texture scale.
        /// </summary>
        public Vector2 Scale;

        /// <summary>
        /// Texture overlay opacity (0..1).
        /// 0 = original texture.
        /// 1 = complete overlay.
        /// </summary>
        public float Opacity;

        #endregion

        #region Methods

        /// <summary>
        /// Sets shader configuration for rendering.
        /// </summary>
        protected override void ConfigureShader(DynamicObject obj)
        {
            var objPos = obj.GetTransformInfo(true).Position;
            var origin = (objPos - Offset) / GameEngine.Screen.Size;

            _effect.Parameters["OverlayTexture"].SetValue(Texture);
            _effect.Parameters["OverlayScale"].SetValue(Scale);
            _effect.Parameters["OverlayOrigin"].SetValue(origin);
            _effect.Parameters["OverlayOpacity"].SetValue(Opacity);
        }

        #endregion
    }
}
