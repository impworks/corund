using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals.Particles
{
    /// <summary>
    /// Base class for "thin" particles.
    /// </summary>
    public abstract class ParticleBase : DynamicObject
    {
        #region Constructor

        protected ParticleBase(Texture2D texture, Vector2 hotSpot)
        {
            Texture = texture;
            HotSpot = hotSpot;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Current texture.
        /// </summary>
        public readonly Texture2D Texture;

        /// <summary>
        /// Offset from texture's top left coordinate that is the origin point for drawing.
        /// </summary>
        public readonly Vector2 HotSpot;

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the particle to the screen.
        /// </summary>
        protected override void DrawInternal()
        {
            base.DrawInternal();

            // sic! SpriteBatch.Begin is called once in ParticleGroup.DrawInternal

            var transform = GetTransformInfo(true);
            GameEngine.Render.SpriteBatch.Draw(
                Texture,
                transform.Position,
                null,
                Tint,
                transform.Angle,
                HotSpot,
                transform.ScaleVector,
                SpriteEffects.None,
                GameEngine.Current.ZOrderFunction(this)
            );
        }

        #endregion
    }
}
