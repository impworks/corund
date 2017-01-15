using System;
using Corund.Engine;
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
            Size = new Vector2(width, height);
            Bounds = new Rectangle(0, 0, width, height);
            RenderTarget = new RenderTarget2D(GameEngine.Render.Device, width, height);

            BackgroundColor = Color.Black;
            Timeline = new TimelineManager();
            Camera = new Camera();
            ZOrderFunction = obj => _zOrder += 0.0001f;
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
        /// Special binding for camera-related position.
        /// </summary>
        public override Vector2 Position
        {
            get { return -Camera.Offset; }
            set { throw new InvalidOperationException("To scroll frame, use Camera instead."); }
        }

        /// <summary>
        /// Direct binding for camera scale.
        /// </summary>
        public override Vector2 ScaleVector
        {
            get { return Camera.ScaleVector; }
            set { throw new InvalidOperationException("To scale frame, use Camera instead."); }
        }

        /// <summary>
        /// Direct binding for camera angle.
        /// </summary>
        public override float Angle
        {
            get { return Camera.Angle; }
            set { throw new InvalidOperationException("To rotate frame, use Camera instead."); }
        }

        /// <summary>
        /// Current z-order function.
        /// </summary>
        public readonly Func<DynamicObject, float> ZOrderFunction;

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
        public abstract void FinalizeDraw();

        #endregion

        #region Update

        /// <summary>
        /// Updates all objects in the current frame.
        /// </summary>
        public override void Update()
        {
            var pm = GameEngine.Current.PauseMode | PauseMode;

            if((pm & PauseMode.Timeline) != 0)
                Timeline.Update();

            base.Update();
        }

        #endregion
    }
}
