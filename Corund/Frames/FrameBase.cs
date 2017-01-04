﻿using System;
using Corund.Engine;
using Corund.Managers;
using Corund.Tools;
using Corund.Visuals;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Frames
{
    /// <summary>
    /// Base class for all game frames and pop-up windows.
    /// </summary>
    public abstract class FrameBase: ObjectGroup
    {
        #region Constructors

        public FrameBase()
            : this(GameEngine.Screen.Rect.Width, GameEngine.Screen.Rect.Height)
        { }

        public FrameBase(int width, int height)
        {
            Width = width;
            Height = height;
        }
         
        #endregion

        #region Fields

        /// <summary>
        /// Current z-order value for the scene.
        /// </summary>
        private float _zOrder;

        #endregion

        #region Properties

        /// <summary>
        /// The width of current frame in pixels.
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// The height of current frame in pixels.
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Color to fill the frame's background.
        /// </summary>
        public Color BackgroundColor { get; protected set; }

        /// <summary>
        /// Sprite batch for current scene.
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }

        /// <summary>
        /// Render target where all scene contents is drawn into.
        /// Is later composed with other scenes on the graphic device itself.
        /// </summary>
        public RenderTarget2D RenderTarget { get; private set; }

        /// <summary>
        /// The list of timed events of current frame.
        /// </summary>
        public TimelineManager Timeline { get; private set; }

        /// <summary>
        /// The frame's camera.
        /// </summary>
        public Camera Camera { get; private set; }

        /// <summary>
        /// Current z-order function.
        /// </summary>
        public Func<DynamicObject, float> ZOrderFunction { get; set; }

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the current frame.
        /// </summary>
        public override void Draw(SpriteBatch batch)
        {
            _zOrder = 1;

            base.Draw(batch);
        }

        /// <summary>
        /// Sets graphic mode properties for drawing the frame to its RenderTarget.
        /// </summary>
        public virtual void BeginDraw()
        {
            GameEngine.GraphicsDevice.SetRenderTarget(RenderTarget);
            GameEngine.GraphicsDevice.Clear(BackgroundColor);

            // todo
        }

        /// <summary>
        /// Finished drawing the frame to its RenderTarget.
        /// </summary>
        public void EndDraw()
        {
            SpriteBatch.End();
        }

        /// <summary>
        /// Draws the frame's RenderTarget to the actual screen.
        /// </summary>
        public abstract void FinalizeDraw(SpriteBatch batch, float angle, Vector2 position, Vector2 screenSize);

        #endregion

        #region Update

        public override void Update()
        {
            var pm = GameEngine.Frames.PauseMode | PauseMode;

            if((pm & PauseMode.Timeline) != 0)
                Timeline.Update();

            base.Update();
        }

        #endregion
    }
}
