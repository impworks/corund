using Corund.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Frames
{
    /// <summary>
    /// Base class for pop-up windows.
    /// </summary>
    public class Window: FrameBase
    {
        #region Constructor

        public Window(int width, int height)
            : base(width, height, width, height)
        {
            _shadowTexture = new Texture2D(GameEngine.Render.Device, 1, 1);
            ShadowColor = new Color(Color.Black, 0.4f);
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
        /// Color to draw over underlying frames.
        /// </summary>
        public Color ShadowColor
        {
            get { return _shadowColor; }
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

            if (!AllowBackgroundTouches)
            {
                var touches = GameEngine.Touch.Touches;
                foreach(var touch in touches)
                    GameEngine.Touch.HandleTouch(touch, this);
            }
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Renders the window and its background overlay to the screen.
        /// </summary>
        public override void FinalizeDraw(float zOrder)
        {
            var batch = GameEngine.Render.SpriteBatch;
            batch.Draw(
                _shadowTexture,
                GameEngine.Screen.Rect,
                Color.White
            );

            batch.Draw(
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
