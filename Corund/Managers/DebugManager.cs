using System.Diagnostics;
using Corund.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Managers
{
    /// <summary>
    /// Methods to help debug the game.
    /// </summary>
    public class DebugManager
    {
        #region Constructor

        public DebugManager()
        {
            _boxTexture = new Texture2D(GameEngine.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _boxTexture.SetData(new[] { Color.Red });
        }

        #endregion

        #region Fields and properties

        /// <summary>
        /// Texture used to draw bounding boxes.
        /// </summary>
        private readonly Texture2D _boxTexture;

        /// <summary>
        /// Current FPS counter.
        /// </summary>
        private int _fps;

        /// <summary>
        /// Time elapsed since last FPS update.
        /// </summary>
        private float _fpsElapsedTime;

        /// <summary>
        /// Flag indicating that FPS must be calculated and written to the debug console.
        /// </summary>
        public bool DisplayFPS;

        #endregion

        #region Update

        /// <summary>
        /// Dumps the FPS counter into debug console.
        /// </summary>
        public void Update()
        {
            if (DisplayFPS)
            {
                _fps++;

                if (_fpsElapsedTime >= 1)
                {
                    _fps = 0;
                    _fpsElapsedTime = 0;
                    Debug.WriteLine($"FPS: {_fps}");
                }
            }
        }

        #endregion

        #region Geometry visualization

        // todo...

        #endregion
    }
}
