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
            IsRotated = options.Orientation == DisplayOrientation.Portrait;

            var viewport = options.GraphicsDeviceManager.GraphicsDevice.Viewport;
            Size = options.ResolutionAdaptationMode == ResolutionAdaptationMode.Adjust
                ? new Vector2(viewport.Width, viewport.Height)
                : options.DesiredScreenSize;

            if (IsRotated && options.ResolutionAdaptationMode == ResolutionAdaptationMode.Adjust)
                Size = new Vector2(Size.Y, Size.X);

            Rect = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the flag indicating that current screen resolution is rotated relative to "default orientation".
        /// </summary>
        public readonly bool IsRotated;

        /// <summary>
        /// Gets the screen size in pixels.
        /// </summary>
        public readonly Vector2 Size;

        /// <summary>
        /// Gets the screen rectangle.
        /// </summary>
        public readonly Rectangle Rect;

        #endregion
    }
}
