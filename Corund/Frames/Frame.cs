﻿using Corund.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Frames
{
    /// <summary>
    /// Base class for all "ordinary" frames.
    /// </summary>
    public class Frame: FrameBase
    {
        #region Constructor

        public Frame(int width, int height, int? viewWidth = null, int? viewHeight = null)
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
            GameEngine.Render.TryBeginBatch(BlendState.AlphaBlend);
            GameEngine.Render.SpriteBatch.Draw(
                RenderTarget,
                Position,
                null,
                Tint,
                Angle,
                HotSpot,
                ScaleVector,
                SpriteEffects.None,
                zOrder
            );
        }

        #endregion
    }
}