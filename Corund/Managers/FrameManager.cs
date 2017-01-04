using System.Collections.Generic;
using Corund.Behaviours.Fade;
using Corund.Engine;
using Corund.Frames;
using Corund.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Managers
{
    /// <summary>
    /// The manager class for all frames currently running in the game.
    /// </summary>
    public class FrameManager
    {
        #region Constructor

        public FrameManager()
        {
            _frames = new List<FrameBase>();

            _mainSpriteBatch = new SpriteBatch(GameEngine.GraphicsDevice);
        }

        #endregion

        #region Fields and properties

        /// <summary>
        /// The list of currently executed frames.
        /// </summary>
        private readonly List<FrameBase> _frames;

        /// <summary>
        /// The sprite batch for composing frames on the screen.
        /// </summary>
        private readonly SpriteBatch _mainSpriteBatch;

        /// <summary>
        /// The frame that is being currently executed.
        /// </summary>
        public FrameBase Current { get; private set; }

        /// <summary>
        /// The current pause mode, descended from top frame to bottom.
        /// </summary>
        public PauseMode PauseMode;

        #endregion

        #region Frame stack handling

        /// <summary>
        /// Adds a frame to the top of current stack.
        /// </summary>
        public void Add(FrameBase frame)
        {
            _frames.Add(frame);
        }

        /// <summary>
        /// Removes the frame from current stack.
        /// </summary>
        public void Remove(FrameBase frame)
        {
            _frames.Remove(frame);
        }

        /// <summary>
        /// Fades the new frame in, replacing all current frames.
        /// </summary>
        public void TransitionTo(FrameBase frame)
        {
            if (_frames.Count == 0)
            {
                Add(frame);
                return;
            }

            var fadeInTimeout = 0f;
            foreach (var behaviour in frame.Behaviours)
            {
                var duration = (behaviour as IFadeInEffect)?.Duration;
                if (duration > fadeInTimeout)
                    fadeInTimeout = duration.Value;
            }

            GameEngine.InvokeDeferred(() => Add(frame));

            foreach (var oldFrame in _frames)
                oldFrame.Timeline.Add(fadeInTimeout, oldFrame.FadeOut);
        }

        #endregion

        #region Update & draw

        /// <summary>
        /// Updates all frames in the list.
        /// </summary>
        public void Update()
        {
            PauseMode = PauseMode.None;

            // update frames from top to bottom
            for (var idx = _frames.Count - 1; idx >= 0; idx--)
            {
                Current = _frames[idx];
                Current.Update();
            }
        }

        /// <summary>
        /// Draws all frames to the device's screen.
        /// </summary>
        public void Draw()
        {
            // pass 1: Render all frames to their respective render targets (bottom to top)
            for (var idx = 0; idx < _frames.Count; idx++)
            {
                Current = _frames[idx];
                Current.BeginDraw();
                Current.Draw(Current.SpriteBatch);
                Current.EndDraw();
            }

            // pass 2: draw rendertargets to screen
            GameEngine.GraphicsDevice.SetRenderTarget(null);
            GameEngine.GraphicsDevice.Clear(Color.Black);

            // todo: finalize
        }

        #endregion
    }
}
