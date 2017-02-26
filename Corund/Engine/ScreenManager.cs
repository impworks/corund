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

            var viewport = options.GraphicsDeviceManager.GraphicsDevice.Viewport;
            Size = options.ResolutionAdaptationMode == ResolutionAdaptationMode.Adjust
                ? new Vector2(viewport.Width, viewport.Height)
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
        /// Gets the screen size in pixels.
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

        #endregion
    }
}
