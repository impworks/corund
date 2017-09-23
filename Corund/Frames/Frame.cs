using Corund.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Frames
{
    /// <summary>
    /// Base class for all "ordinary" frames.
    /// </summary>
    public class Frame: FrameBase
    {
        #region Constructor

        public Frame()
            : base(GameEngine.Screen.Size.X, GameEngine.Screen.Size.Y)
        {
            // nothing to do here?
        }

        public Frame(float width, float height, int? viewWidth = null, int? viewHeight = null)
            : base(width, height, viewWidth, viewHeight)
        {
            // nothing to do here?
        }

        #endregion

        #region Draw

        /// <summary>
        /// Renders the frame to the screen.
        /// </summary>
        public override void FinalizeDraw(float zOrder)
        {
            var tx = ResolutionAdaptationTransform;
            GameEngine.Render.TryBeginBatch(BlendState.AlphaBlend);
            GameEngine.Render.SpriteBatch.Draw(
                RenderTarget,
                tx.Position + Position * tx.ScaleVector,
                null,
                Tint,
                Angle,
                HotSpot,
                ScaleVector * tx.ScaleVector,
                SpriteEffects.None,
                zOrder
            );
        }

        #endregion
    }
}
