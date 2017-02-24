using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Corund.Engine.Config
{
    /// <summary>
    /// The options to initialize the game engine.
    /// </summary>
    public class GameEngineOptions
    {
        #region Constructor

        public GameEngineOptions(Game game, GraphicsDeviceManager manager)
        {
            Game = game;
            GraphicsDeviceManager = manager;
            Content = game.Content;

            Orientation = DisplayOrientation.Portrait;
            ResolutionAdaptationMode = ResolutionAdaptationMode.Adjust;
            DesiredScreenSize = new Vector2(480, 800);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Reference to the Game instance.
        /// </summary>
        public Game Game;

        /// <summary>
        /// Reference to graphic device manager.
        /// </summary>
        public GraphicsDeviceManager GraphicsDeviceManager;

        /// <summary>
        /// Reference to content manager.
        /// </summary>
        public ContentManager Content;

        /// <summary>
        /// Supported display orientation(s).
        /// </summary>
        public DisplayOrientation Orientation;

        /// <summary>
        /// The preferred way of adjusting the game to actual screen size.
        /// </summary>
        public ResolutionAdaptationMode ResolutionAdaptationMode;

        /// <summary>
        /// Screen size for which the game has been tailored.
        /// Not applicable in ResolutionAdaptationMode.Adjust.
        /// </summary>
        public Vector2 DesiredScreenSize;

        /// <summary>
        /// Use anti-aliased rendering or not?
        /// </summary>
        public bool EnableAntiAliasing;

        /// <summary>
        /// Platform-specific content provider.
        /// </summary>
        public IContentProvider ContentProvider;

        #endregion
    }
}
