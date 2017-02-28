using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders
{
    public class DesaturateShader: SinglePassShaderBase
    {
        #region Constructor

        public DesaturateShader(float coefficient = 1)
        {
            Coefficient = coefficient;

            _effect = GameEngine.EmbeddedContent.Load<Effect>("desaturate");
        }

        #endregion

        private float _coefficient;

        #region Fields

        /// <summary>
        /// Desaturation coefficient.
        /// 0 = original.
        /// 1 = fully black & white.
        /// </summary>
        public float Coefficient
        {
            get { return _coefficient;}
            set { _coefficient = MathHelper.Clamp(value, 0, 1); }
        }

        #endregion

        #region Methods

        protected override void ConfigureShader(DynamicObject obj)
        {
            _effect.Parameters["Coefficient"].SetValue(Coefficient);
        }

        #endregion
    }
}
