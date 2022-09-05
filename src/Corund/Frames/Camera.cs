using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Frames;

/// <summary>
/// The camera settings for the frame.
/// </summary>
public class Camera: DynamicObject
{
    #region Constructor

    public Camera()
    {
        LockX = true;
        LockY = true;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Scrolling offset (from the top left corner).
    /// </summary>
    public virtual Vector2 Offset => Position;

    /// <summary>
    /// Gets or sets the locking mode for X axis.
    /// If locked, the camera will not allow scrolling outside of the frame's boundaries.
    /// Default is true.
    /// </summary>
    public bool LockX;

    /// <summary>
    /// Gets or sets the locking mode for Y axis.
    /// If locked, the camera will not allow scrolling outside of the frame's boundaries.
    /// Default is true.
    /// </summary>
    public bool LockY;

    #endregion

    #region Methods

    public override void Update()
    {
        base.Update();

        var frameSize = GameEngine.Current.Frame.Size;
        var winSize = GameEngine.Current.Frame.ViewSize;

        // not using MathHelper.Clamp
        // because the frame can be smaller than the window

        if (LockX)
        {
            if (Position.X + winSize.X > frameSize.X)
                Position.X = frameSize.X - winSize.X;
            
            if (Position.X < 0)
                Position.X = 0;
        }
            
        if (LockY)
        {
            if (Position.Y + winSize.Y > frameSize.Y)
                Position.Y = frameSize.Y - winSize.Y;
            
            if (Position.Y < 0)
                Position.Y = 0;
        }
    }

    protected override void DrawInternal()
    {
        // does nothing
    }

    #endregion
}