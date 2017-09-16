using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders
{
    /// <summary>
    /// Warps the object around a specified point.
    /// </summary>
    public class SwirlShader : SinglePassShaderBase
    {
        #region Constructor

        public SwirlShader(Vector2 center, float size, float coefficient = 6)
        {
            Center = center;
            Size = size;
            WarpCoefficient = coefficient;

            _effect = GameEngine.EmbeddedContent.Load<Effect>("swirl");
        }

        #endregion

        #region Fields

        /// <summary>
        /// Warp coefficient.
        /// 0 = no warp.
        /// 1 = little warp.
        /// 6 = normal warp.
        /// </summary>
        public float WarpCoefficient;

        /// <summary>
        /// Coordinates of the warp center.
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// Size of the vortex.
        /// </summary>
        public float Size;

        #endregion

        #region Methods

        protected override void ConfigureShader(DynamicObject obj)
        {
            var screen = GameEngine.Screen.Size;
            var max = MathHelper.Max(screen.X, screen.Y);
            var size = Size/max;
            var center = (Center/screen) - new Vector2(0.5f);

            _effect.Parameters["WarpCoefficient"].SetValue(WarpCoefficient);
            _effect.Parameters["Center"].SetValue(center);
            _effect.Parameters["RadiusOuter"].SetValue(size);
            _effect.Parameters["RadiusInner"].SetValue(size / 7f);
            _effect.Parameters["ScreenRatio"].SetValue(screen / max);
        }

        #endregion
    }
}
