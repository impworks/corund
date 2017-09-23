using Corund.Engine.Config;
using Microsoft.Xna.Framework;

namespace Corund.Engine
{
    /// <summary>
    /// Various helpers for getting screen properties.
    /// </summary>
    public class ScreenManager
    {
        #region Constructor

        public ScreenManager(GameEngineOptions options)
        {
            Orientation = options.Orientation;

            var vp = options.GraphicsDeviceManager.GraphicsDevice.Viewport;
            Viewport = new Vector2(vp.Width, vp.Height);
            Size = options.ResolutionAdaptationMode == ResolutionAdaptationMode.Adjust
                ? Viewport
                : options.DesiredScreenSize;

            if (options.Orientation == DisplayOrientation.Portrait && Size.X > Size.Y)
                Size = new Vector2(Size.Y, Size.X);

            Rect = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Desired orientation of the device.
        /// </summary>
        public readonly DisplayOrientation Orientation;

        /// <summary>
        /// Gets the pixel size of the screen window available to the game.
        /// </summary>
        public readonly Vector2 Size;

        /// <summary>
        /// Gets the screen rectangle.
        /// </summary>
        public readonly Rectangle Rect;

        /// <summary>
        /// Center point of the screen.
        /// </summary>
        public Vector2 Center => Size/2;

        /// <summary>
        /// Gets the pixel size of the device's screen.
        /// </summary>
        public readonly Vector2 Viewport;

        #endregion
    }
}
