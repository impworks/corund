using System;
using System.Collections.Generic;
using Corund.Engine.Config;
using Corund.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Engine
{
    /// <summary>
    /// The main class that governs all inner workings of Corund game engine.
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

            var mgr = Options.GraphicsDeviceManager;
            mgr.SupportedOrientations = opts.Orientation;
            mgr.IsFullScreen = true;
            mgr.SynchronizeWithVerticalRetrace = true;
            mgr.ApplyChanges();

            Frames = new FrameManager();
            Debug = new DebugManager();
            Screen = new ScreenManager(opts);
            Render = new RenderManager(opts.GraphicsDeviceManager.GraphicsDevice);

            _deferredActions = new List<Action>();
        }

        #endregion

        #region Fields

        /// <summary>
        /// Gets the content manager.
        /// </summary>
        public static ContentManager Content { get; private set; }

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
        public static void InvokeDeferred(Action action)
        {
            _deferredActions.Add(action);
        }

        /// <summary>
        /// Execute all the deferred actions.
        /// </summary>
        private static void ExecuteDeferredActions()
        {
            var actions = _deferredActions;
            _deferredActions = new List<Action>();

            foreach (var action in actions)
                action();
        }

        #endregion
    }
}
