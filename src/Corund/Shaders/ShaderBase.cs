using System;
using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders
{
    /// <summary>
    /// Base class for shader effects.
    /// </summary>
    public abstract class ShaderBase: IShader
    {
        #region Fields

        /// <summary>
        /// Reference to compiled effect.
        /// </summary>
        protected Effect _effect;

        #endregion

        #region Properties

        /// <summary>
        /// Shorthand render target rectangle getter.
        /// </summary>
        protected Rectangle RenderTargetRect
        {
            get
            {
                var pp = GameEngine.Render.Device.Viewport;
                return new Rectangle(0, 0, pp.Width, pp.Height);
            }
        }

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

        #endregion
    }
}
