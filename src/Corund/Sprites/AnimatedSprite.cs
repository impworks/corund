using System;
using Corund.Engine;
using Corund.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Sprites;

/// <summary>
/// A sprite that uses different locations on a texture as animation frames.
/// </summary>
public class AnimatedSprite: SpriteBase
{
    #region Constructor

    public AnimatedSprite(string assetName, int frameCount, float framesPerSecond, bool loop = false)
        : this(GameEngine.Content.Load<Texture2D>(assetName), frameCount, framesPerSecond, loop)
    {
    }

    public AnimatedSprite(Texture2D texture, int frameCount, float framesPerSecond, bool loop = false)
        : base(texture)
    {
        FrameCount = frameCount;

        var texWidth = texture.Width;
        if (texWidth % frameCount != 0)
            throw new ArgumentOutOfRangeException(nameof(frameCount), "The texture does not contain a whole number of frames!");

        Size = new Vector2(texWidth / frameCount, Texture.Height);
        FrameDelay = 1 / framesPerSecond;
        Loop = loop;
    }

    #endregion

    #region Fields

    /// <summary>
    /// ID of the current frame.
    /// </summary>
    private int _currentFrame;

    /// <summary>
    /// Rectangular bounds of the current frame.
    /// </summary>
    private Rectangle _currentBounds;

    /// <summary>
    /// Time elapsed since last frame change.
    /// </summary>
    private float _elapsedFrameTime;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the number of frames in the animation.
    /// </summary>
    public readonly int FrameCount;

    /// <summary>
    /// Gets or sets duration between frame changes (in seconds).
    /// </summary>
    public float FrameDelay;

    /// <summary>
    /// Gets duration of the animation with current speed.
    /// </summary>
    public float Duration => FrameCount*FrameDelay;

    /// <summary>
    /// Gets or sets pause flag.
    /// </summary>
    public bool IsPaused;

    /// <summary>
    /// Gets or sets currently displayed frame.
    /// </summary>
    public int CurrentFrame
    {
        get => _currentFrame;
        set
        {
            var frameSize = (int)Size.X;

            _currentFrame = value;
            _currentBounds = new Rectangle(value * frameSize, 0, frameSize, Texture.Height);
        }
    }

    /// <summary>
    /// Flag indicating that the animation has completed and is paused at the last frame.
    /// </summary>
    public bool IsCompleted => _currentFrame == FrameCount - 1;

    /// <summary>
    /// Flag indicating that the animation must revert to first frame at the end.
    /// </summary>
    public readonly bool Loop;

    #endregion

    #region Methods

    /// <summary>
    /// Rewinds the animation to first frame.
    /// </summary>
    public override void Reset()
    {
        CurrentFrame = 0;
        _elapsedFrameTime = 0;
    }

    /// <summary>
    /// Update current frame state.
    /// </summary>
    public override void Update()
    {
        if (IsPaused)
            return;

        _elapsedFrameTime += GameEngine.Delta;
        if (_elapsedFrameTime < FrameDelay)
            return;

        if (CurrentFrame < FrameCount - 1)
        {
            CurrentFrame++;
            _elapsedFrameTime -= FrameDelay;
        }
        else if(Loop)
        {
            Reset();
        }
    }

    /// <summary>
    /// Draws the sprite's current frame to the render target.
    /// </summary>
    public override void Draw(TransformInfo transform, BlendState blend, Color tint, float zOrder)
    {
        GameEngine.Render.TryBeginBatch(blend);
        GameEngine.Render.SpriteBatch.Draw(
            Texture,
            transform.Position,
            _currentBounds,
            tint,
            transform.Angle,
            HotSpot,
            transform.ScaleVector,
            SpriteEffects.None,
            zOrder
        );
    }

    /// <summary>
    /// Get a portion of the current frame's texture as a sequence of colors.
    /// </summary>
    public override Color[] GetTextureRegion(Rectangle rect)
    {
        rect.X += _currentBounds.Left;
        var result = new Color[rect.Width * rect.Height];
        Texture.GetData(0, rect, result, 0, result.Length);
        return result;
    }

    #endregion
}