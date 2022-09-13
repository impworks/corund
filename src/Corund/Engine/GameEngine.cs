using System;
using System.Collections.Generic;
using Corund.Engine.Config;
using Corund.Engine.Prompts;
using Corund.Frames;
using Corund.Sound;
using Corund.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Corund.Engine;

/// <summary>
/// The main class that governs all inner workings of the Corund game engine.
/// </summary>
public static partial class GameEngine
{
    #region Initialization

    /// <summary>
    /// Initialize the game engine for XNA application.
    /// </summary>
    public static void Init(GameEngineOptions opts)
    {
        Options = opts;
        Content = opts.Content;

        Screen = new ScreenManager(opts);
        Render = new RenderManager(opts);
        Frames = new FrameManager();
        Sound = new SoundManager(opts);
        Touch = new TouchManager();
        Debug = new DebugManager(Render.Device);
        Accelerometer = opts.PlatformAdapter?.GetAccelerometerManager();
        Prompt = opts.PlatformAdapter?.GetPromptManager();

        var ecp = opts.PlatformAdapter?.GetEmbeddedContentProvider();
        if (ecp != null)
            EmbeddedContent = new EmbeddedContentManager(opts.Game.Services, ecp);

        _deferredActions = new List<Action>();
    }

    #endregion

    #region Fields

    /// <summary>
    /// Gets the game's own content manager.
    /// </summary>
    public static ContentManager Content { get; private set; }

    /// <summary>
    /// Gets the embedded content manager.
    /// </summary>
    public static EmbeddedContentManager EmbeddedContent { get; private set; }

    /// <summary>
    /// Gets the current timer delta value.
    /// </summary>
    public static float Delta { get; private set; }

    /// <summary>
    /// The options for rendering.
    /// </summary>
    public static GameEngineOptions Options { get; private set; }

    /// <summary>
    /// The frames manager.
    /// </summary>
    public static FrameManager Frames { get; private set; }

    /// <summary>
    /// The debug manager.
    /// </summary>
    public static DebugManager Debug { get; private set; }

    /// <summary>
    /// The screen manager.
    /// </summary>
    public static ScreenManager Screen { get; private set; }

    /// <summary>
    /// The render manager.
    /// </summary>
    public static RenderManager Render { get; private set; }

    /// <summary>
    /// The sound manager.
    /// </summary>
    public static SoundManager Sound { get; private set; }

    /// <summary>
    /// The touch manager.
    /// </summary>
    public static TouchManager Touch { get; private set; }

    /// <summary>
    /// The accelerometer manager (optional, depends on the platform).
    /// </summary>
    public static IAccelerometerManager Accelerometer { get; private set; }

    /// <summary>
    /// The prompt manager (optional, depends on the platform).
    /// </summary>
    public static IPromptManager Prompt { get; private set; }

    /// <summary>
    /// List of actions to execute after all update loops have completed.
    /// </summary>
    private static List<Action> _deferredActions;

    #endregion

    #region Main Methods: Update and Draw

    /// <summary>
    /// The main Update method for all game components.
    /// </summary>
    public static void Update(GameTime time)
    {
        Delta = (float)time.ElapsedGameTime.TotalSeconds;
        Current.PauseMode = PauseMode.None;

        Touch.Update();
        Prompt?.Update();
        Frames.Update();
        Debug.Update();

        ExecuteDeferredActions();
    }

    /// <summary>
    /// The main Draw method for all game components.
    /// </summary>
    public static void Draw(GameTime time)
    {
        Delta = (float)time.ElapsedGameTime.TotalSeconds;

        Frames.Draw();
    }

    #endregion

    #region Deferred and timed action list

    /// <summary>
    /// Register a callback that is invoked after all objects are updated.
    /// It's used to manipulate object's position in the list to avoid modifying the collection being traversed.
    /// </summary>
    public static void Defer(Action action)
    {
        _deferredActions.Add(action);
    }

    /// <summary>
    /// Execute all the deferred actions.
    /// </summary>
    private static void ExecuteDeferredActions()
    {
        if (_deferredActions.Count == 0)
            return;

        var actions = _deferredActions;
        _deferredActions = new List<Action>();

        foreach (var action in actions)
            action();
    }

    #endregion
}