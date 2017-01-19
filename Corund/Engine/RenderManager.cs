using System.Collections.Generic;
using Corund.Engine.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Engine
{
    /// <summary>
    /// A wrapper around the GraphicDevice.
    /// </summary>
    public class RenderManager
    {
        #region Constructor

        public RenderManager(GameEngineOptions opts)
        {
            DeviceManager = opts.GraphicsDeviceManager;
            Device = DeviceManager.GraphicsDevice;
            SpriteBatch = new SpriteBatch(Device);

            DeviceManager.SupportedOrientations = opts.Orientation;
            DeviceManager.IsFullScreen = true;
            DeviceManager.SynchronizeWithVerticalRetrace = true;
            DeviceManager.ApplyChanges();

            _renderStack = new Stack<RenderTarget2D>(4);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Reference to graphics device.
        /// </summary>
        public readonly GraphicsDevice Device;

        /// <summary>
        /// Reference to graphic device manager.
        /// </summary>
        public readonly GraphicsDeviceManager DeviceManager;

        /// <summary>
        /// The game's spritebatch.
        /// </summary>
        public readonly SpriteBatch SpriteBatch;

        /// <summary>
        /// Stack of current render targets.
        /// </summary>
        private readonly Stack<RenderTarget2D> _renderStack;

        /// <summary>
        /// Currently set blending state.
        /// </summary>
        private BlendState _blendState;

        /// <summary>
        /// Current tiling mode.
        /// </summary>
        private bool? _tileMode;

        /// <summary>
        /// Flag indicating that the current sprite batch is started
        /// </summary>
        private bool _isStarted;

        #endregion

        #region Methods

        /// <summary>
        /// Begins a new batch or uses current if the settings are intact.
        /// </summary>
        public void TryBeginBatch(BlendState blendState, bool tileMode = false)
        {
            var isModified = _blendState != blendState || _tileMode != tileMode;
            if(_isStarted && !isModified)
                return;

            if(_isStarted)
                SpriteBatch.End();

            var samplerState = GetSamplerState(tileMode);
            SpriteBatch.Begin(
                SpriteSortMode.BackToFront,
                blendState,
                samplerState 
            );

            _blendState = blendState;
            _tileMode = tileMode;
            _isStarted = true;
        }

        /// <summary>
        /// Ends the current batch.
        /// </summary>
        public void EndBatch()
        {
            if (!_isStarted)
                return;

            SpriteBatch.End();
            _isStarted = false;
        }

        /// <summary>
        /// Sets a new rendering context.
        /// </summary>
        public void PushContext(RenderTarget2D target, Color? clearColor = null)
        {
            EndBatch();

            _renderStack.Push(target);
            Device.SetRenderTarget(target);

            if(clearColor != null)
                Device.Clear(clearColor.Value);
        }

        /// <summary>
        /// Pops current context, setting the previous one as current.
        /// </summary>
        public RenderTarget2D PopContext()
        {
            EndBatch();

            var target = _renderStack.Pop();

            if(_renderStack.Count > 0)
                Device.SetRenderTarget(_renderStack.Peek());

            return target;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the appropriate sampler state depending on sprite mode and engine settings.
        /// </summary>
        public static SamplerState GetSamplerState(bool tileMode)
        {
            var useSmoothing = GameEngine.Options.EnableAntiAliasing;
            if (useSmoothing)
                return tileMode
                    ? SamplerState.LinearWrap
                    : SamplerState.LinearClamp;

            return tileMode
                ? SamplerState.PointWrap
                : SamplerState.PointClamp;
        }

        #endregion
    }
}
