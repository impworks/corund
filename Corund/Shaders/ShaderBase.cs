using System;
using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders
{
    /// <summary>
    /// Base class for shader effects.
    /// </summary>
    public abstract class ShaderBase
    {
        #region Constructor

        public ShaderBase()
        {
            _renderTarget = CreateRenderTarget();
        }

        #endregion

        #region Fields

        /// <summary>
        /// Intermediate render target to draw on.
        /// </summary>
        protected RenderTarget2D _renderTarget;

        #endregion

        #region Methods

        /// <summary>
        /// Updates the shader's inner state, if necessary.
        /// </summary>
        public virtual void Update()
        {
            // nothing here
        }

        /// <summary>
        /// Sets up context for drawing onto the intermediate texture.
        /// </summary>
        public abstract void DrawWrapper(DynamicObject obj, Action innerDraw);

        /// <summary>
        /// Creates the render target for this shader.
        /// </summary>
        protected virtual RenderTarget2D CreateRenderTarget()
        {
            var pp = GameEngine.Render.Device.PresentationParameters;
            return new RenderTarget2D(
                GameEngine.Render.Device,
                pp.BackBufferWidth,
                pp.BackBufferHeight,
                false,
                SurfaceFormat.Color,
                (DepthFormat) 2,
                1,
                RenderTargetUsage.PreserveContents
            );
        }

        #endregion
    }
}
