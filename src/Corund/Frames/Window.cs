﻿using Corund.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Frames;

/// <summary>
/// Base class for pop-up windows.
/// </summary>
public class Window: FrameBase
{
    #region Constructor

    public Window(int width, int height)
        : this(new Vector2(width, height))
    {
            
    }

    public Window(Vector2 size)
        : base(size, size)
    {
        _shadowTexture = new Texture2D(GameEngine.Render.Device, 1, 1);
        ShadowColor = new Color(Color.Black, 0.4f);
        Scale = 1f;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Texture to render below the window.
    /// </summary>
    private Texture2D _shadowTexture;

    /// <summary>
    /// Color of the underlying texture.
    /// </summary>
    private Color _shadowColor;

    #endregion

    #region Properties

    /// <summary>
    /// Checks whether the user can tap through window's semitransparent background.
    /// </summary>
    protected bool AllowBackgroundTouches;

    /// <summary>
    /// Checks whether window should be closed when the user taps in the background.
    /// </summary>
    protected bool CloseOnBackgroundTouch;

    /// <summary>
    /// Color to draw over underlying frames.
    /// </summary>
    public Color ShadowColor
    {
        get => _shadowColor;
        set
        {
            _shadowColor = value;
            _shadowTexture.SetData(new[] { value });
        }
    }

    #endregion

    #region Updating

    public override void Update()
    {
        base.Update();

        var touches = GameEngine.Touch.GlobalTouches;
        if (CloseOnBackgroundTouch && !IsFadingOut)
        {
            var frameRect = GetViewRectPolygon();
            foreach (var touch in touches)
            {
                if (!GameEngine.Touch.CanHandle(touch, this))
                    continue;

                if (!frameRect.ContainsPoint(touch.Position))
                    continue;

                GameEngine.Touch.Handle(touch, this);
                RemoveSelf();
            }
        }

        if (!AllowBackgroundTouches)
        {
            foreach(var touch in touches)
                GameEngine.Touch.Handle(touch, this);
        }
    }

    #endregion

    #region Drawing

    /// <summary>
    /// Renders the window and its background overlay to the screen.
    /// </summary>
    public override void FinalizeDraw(float zOrder)
    {
        var tx = ResolutionAdaptationTransform;
        var batch = GameEngine.Render.SpriteBatch;
        batch.Draw(
            _shadowTexture,
            new Rectangle(0, 0, (int)GameEngine.Screen.NativeSize.X, (int)GameEngine.Screen.NativeSize.Y),
            Color.White
        );

        batch.Draw(
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