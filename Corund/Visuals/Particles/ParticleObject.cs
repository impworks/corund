using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals.Particles
{
    /// <summary>
    /// Base class for "thin" particles.
    /// </summary>
    public class ParticleObject : MovingObject
    {
        #region Constructor

        public ParticleObject(Texture2D texture, Vector2 hotSpot)
        {
            _texture = texture;
            _hotSpot = hotSpot;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Current texture.
        /// </summary>
        private readonly Texture2D _texture;

        /// <summary>
        /// Offset from texture's top left coordinate that is the origin point for drawing.
        /// </summary>
        private readonly Vector2 _hotSpot;

        #endregion

        #region Properties

        /// <summary>
        /// Particle's lifespan in seconds before it fades out.
        /// </summary>
        public float LifeDuration;

        /// <summary>
        /// Particle's fade duration, after which it is removed completely.
        /// </summary>
        public float FadeDuration;

        /// <summary>
        /// Time elapsed since the particle's creation.
        /// </summary>
        public float ElapsedTime;

        #endregion

        #region Drawing

        public override void Update()
        {
            base.Update();

            ElapsedTime += GameEngine.Delta;

            var fadeElapsed = ElapsedTime - LifeDuration;
            if (fadeElapsed >= 0)
            {
                Opacity = FadeDuration == 0
                    ? 0 
                    : 1 - (fadeElapsed/FadeDuration);
            }
        }

        /// <summary>
        /// Draws the particle to the screen.
        /// </summary>
        public override void Draw()
        {
            // sic! SpriteBatch.Begin is called once in ParticleGroup.DrawInternal

            var transform = GetTransformInfo(true);
            GameEngine.Render.SpriteBatch.Draw(
                _texture,
                transform.Position,
                null,
                Tint,
                transform.Angle,
                _hotSpot,
                transform.ScaleVector,
                SpriteEffects.None,
                GameEngine.Current.ZOrderFunction(this)
            );
        }

        #endregion
    }
}
