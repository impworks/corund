using System;
using System.Collections.Generic;
using Corund.Engine;
using Corund.Engine.Config;
using Corund.Geometry;
using Corund.Tools;
using Corund.Tools.Helpers;
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

        public FrameBase(Vector2 size, Vector2? viewSize = null)
        {
            if(size.X < 1 || size.Y < 1)
                throw new ArgumentException("Frame size must not be less than 1.");

            Size = size;
            Bounds = new Rectangle(0, 0, (int)size.X, (int)size.Y);

            var screen = GameEngine.Screen.Size;
            ViewSize = viewSize ?? screen;
            if(ViewSize.X < 1 || ViewSize.X > screen.X || ViewSize.Y < 1 || ViewSize.Y > screen.Y)
                throw new ArgumentException("View size must be at least 1x1 pixels in size and not exceed the screen size!");

            RenderTarget = new RenderTarget2D(
                GameEngine.Render.Device,
                (int)ViewSize.X,
                (int)ViewSize.Y,
                false,
                SurfaceFormat.Color,
                (DepthFormat) 2,
                1,
                RenderTargetUsage.PreserveContents
            );
            HotSpot = ViewSize/2;
            Position = GameEngine.Screen.Size/2;

            BackgroundColor = Color.Black;
            Touches = new List<TouchLocation>();
            Timeline = new TimelineManager();
            Camera = new Camera();
            ZOrderFunction = obj => _zOrder -= 0.0001f;
            ResolutionAdaptationTransform = GetResolutionAdaptationTransform();
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

        /// <summary>
        /// Flag indicating that initialization has been performed once.
        /// </summary>
        private bool _isInitialized;

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
        public readonly Func<ObjectBase, float> ZOrderFunction;

        /// <summary>
        /// Touch locations translated to current frame.
        /// </summary>
        public readonly List<TouchLocation> Touches;

        /// <summary>
        /// The transform that must be applied during scene rendering to adapt the scene to current resolution.
        /// </summary>
        public readonly TransformInfo ResolutionAdaptationTransform;

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
            if (!_isInitialized)
            {
                Initialize();
                _isInitialized = true;
            }

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

            Camera.Update();

            base.Update();
        }

        /// <summary>
        /// Creates frame objects and settings when the frame is in current context.
        /// </summary>
        protected virtual void Initialize()
        {
            // do nothing yet.
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Removes the frame from the list.
        /// </summary>
        public override void RemoveSelf(bool immediate = false)
        {
            if(immediate)
                GameEngine.InvokeDeferred(() => GameEngine.Frames.Remove(this));
            else
                FadeOut();
        }

        /// <summary>
        /// Returns the current view bounds in screen coordinates.
        /// </summary>
        protected RectPolygon GetViewRectPolygon()
        {
            var info = new TransformInfo(Position, Angle, ScaleVector);

            var lu = Vector2.Zero - HotSpot;
            var ru = new Vector2(Size.X, 0) - HotSpot;
            var rl = Size - HotSpot;
            var ll = new Vector2(0, Size.Y) - HotSpot;

            return new RectPolygon(
                info.Translate(lu),
                info.Translate(ru),
                info.Translate(rl),
                info.Translate(ll),
                Angle
            );
        }

        /// <summary>
        /// Returns the appropriate transform.
        /// </summary>
        protected TransformInfo GetResolutionAdaptationTransform()
        {
            var mode = GameEngine.Options.ResolutionAdaptationMode;
            var vp = GameEngine.Screen.Viewport;

            if (mode == ResolutionAdaptationMode.Adjust)
                return TransformInfo.None;

            if (mode == ResolutionAdaptationMode.Center)
            {
                var offset = (vp - ViewSize) / 2;
                return new TransformInfo(offset, 0, Vector2.One);
            }

            if (mode == ResolutionAdaptationMode.CenterAndResize)
            {
                var scale = 1.0f / Math.Max(ViewSize.X / vp.X, ViewSize.Y / vp.Y);
                var newSize = ViewSize * scale;
                var offset = (vp - newSize) / 2;
                return new TransformInfo(offset, 0, new Vector2(scale));
            }

            throw new ArgumentException($"Unknown resolution adaptation mode: {mode}");
        }

        #endregion
    }
}
