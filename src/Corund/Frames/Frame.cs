using Corund.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Frames;

/// <summary>
/// Base class for all "ordinary" frames.
/// </summary>
public class Frame: FrameBase
{
    #region Constructor

    public Frame()
        : base(GameEngine.Screen.Size)
    {
        // nothing to do here?
    }

    public Frame(Vector2 size, Vector2? viewSize = null)
        : base(size, viewSize)
    {
            
    }

    public Frame(float width, float height, Vector2? viewSize = null)
        : base(new Vector2(width, height), viewSize)
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