using Corund.Tools.ResolutionAdapters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Corund.Engine.Config;

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
        ResolutionAdapter = new NativeResolutionAdapter();
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
    /// Use anti-aliased rendering or not?
    /// </summary>
    public bool EnableAntiAliasing;

    /// <summary>
    /// Platform-specific resource wrapper (optional).
    /// </summary>
    public IPlatformAdapter PlatformAdapter;

    /// <summary>
    /// Resolution adapter (defaults to Native).
    /// </summary>
    public IResolutionAdapter ResolutionAdapter;

    /// <summary>
    /// Translate mouse coordinates to touch?
    /// </summary>
    public bool EnableMouse;

    #endregion
}