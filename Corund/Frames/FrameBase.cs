using System;
using System.Collections.Generic;
using Corund.Engine;
using Corund.Tools;
using Corund.Visuals;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Frames
{
    /// <summary>
    /// Base class for all game frames and pop-up windows.
    /// </summary>
    public abstract class FrameBase: ObjectGroup
    {
        #region Constructors

        public FrameBase(float width, float height, int? viewWidth = null, int? viewHeight = null)
        {
            Size = new Vector2(width, height);
            Bounds = new Rectangle(0, 0, (int)width, (int)height);
            ViewSize = GetViewSize(viewWidth, viewHeight);
            
            RenderTarget = new RenderTarget2D(GameEngine.Render.Device, (int)ViewSize.X, (int)ViewSize.Y);
            HotSpot = ViewSize/2;
            Position = GameEngine.Screen.Size/2;

            BackgroundColor = Color.Black;
            Touches = new List<TouchLocation>();
            Timeline = new TimelineManager();
            Camera = new Camera();
            ZOrderFunction = obj => _zOrder -= 0.0001f;
        }
         
        #endregion

        #region Fields

        /// <summary>
        /// Current z-order value for the scene.
        /// </summary>
        private float _zOrder;

        /// <summary>
        /// Current blend state.
        /// </summary>
        private BlendState _blendState;

        #endregion

        #region Properties

        /// <summary>
        /// Size of the frame.
        /// </summary>
        public readonly Vector2 Size;

        /// <summary>
        /// Scene bounds rectangle.
        /// </summary>
        public readonly Rectangle Bounds;

        /// <summary>
        /// Size of the window through which the frame is displayed.
        /// </summary>
        public readonly Vector2 ViewSize;

        /// <summary>
        /// Origin point around which the frame is scaled and rotated in post-rendering.
        /// </summary>
        public Vector2 HotSpot;

        /// <summary>
        /// Color to fill the frame's background.
        /// </summary>
        public Color BackgroundColor { get; protected set; }

        /// <summary>
        /// Render target where all scene contents is drawn into.
        /// Is later composed with other scenes on the graphic device itself.
        /// </summary>
        public readonly RenderTarget2D RenderTarget;

        /// <summary>
        /// The list of timed events of current frame.
        /// </summary>
        public readonly TimelineManager Timeline;

        /// <summary>
        /// The frame's camera.
        /// </summary>
        public Camera Camera { get; protected set; }

        /// <summary>
        /// Current z-order function.
        /// </summary>
        public readonly Func<DynamicObject, float> ZOrderFunction;

        /// <summary>
        /// Touch locations translated to current frame.
        /// </summary>
        public readonly List<TouchLocation> Touches;

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the current frame.
        /// </summary>
        public override void Draw()
        {
            GameEngine.Render.PushContext(RenderTarget, BackgroundColor);

            _zOrder = 1;
            base.Draw();

            GameEngine.Render.PopContext();
        }

        /// <summary>
        /// Draws the frame's RenderTarget to the actual screen.
        /// </summary>
        public abstract void FinalizeDraw(float zOrder);

        #endregion

        #region Update

        /// <summary>
        /// Updates all objects in the current frame.
        /// </summary>
        public override void Update()
        {
            var pm = GameEngine.Current.PauseMode | PauseMode;

            if((pm & PauseMode.Timeline) == 0)
                Timeline.Update();

            Touches.Clear();
            foreach (var globalTouch in GameEngine.Touch.Touches)
            {
                var localTouch = GameEngine.Touch.TranslateToFrame(globalTouch, this);
                if(localTouch != null)
                    Touches.Add(localTouch.Value);
            }

            base.Update();
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Calculates proper view size from optional sizes.
        /// </summary>
        private Vector2 GetViewSize(int? viewWidth, int? viewHeight)
        {
            var screen = GameEngine.Screen.Size;
            if(viewWidth < 1 || viewWidth > screen.X)
                throw new ArgumentException($"View width must be within the range of 1..{screen.X}.");

            if (viewHeight < 1 || viewHeight > screen.Y)
                throw new ArgumentException($"View height must be within the range of 1..{screen.Y}.");

            return new Vector2(viewWidth ?? screen.X, viewHeight ?? screen.Y);
        }

        #endregion
    }
}
