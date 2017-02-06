using System;
using Corund.Engine;
using Corund.Frames;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Movement
{
    /// <summary>
    /// Positions object relative to camera with a coefficient.
    /// </summary>
    public class ParallaxBehaviour: BehaviourBase
    {
        #region Constructor

        /// <summary>
        /// Creates a new ParallaxBehaviour.
        /// Can only be applied to direct children of the frame (no nesting).
        /// </summary>
        /// <param name="coefficient">
        /// Parallax coefficient.
        /// 0 = object is static, never moves.
        /// 1 = object moves as usual.
        /// 2 = object moves twice as fast.
        /// </param>
        public ParallaxBehaviour(float coefficient = 1)
        {
            if(coefficient < 0)
                throw new ArgumentException("Parallax coefficient cannot be less than zero.", nameof(coefficient));

            _coefficient = coefficient;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Parallax coefficient.
        /// </summary>
        private readonly float _coefficient;

        /// <summary>
        /// Object position at the moment of behaviour binding.
        /// </summary>
        private Vector2 _originalPosition;

        #endregion

        #region Methods

        /// <summary>
        /// Checks that object is the direct child of the frame.
        /// </summary>
        public override void Bind(DynamicObject obj)
        {
            var isDirectChild = obj.Parent is FrameBase;
            if (!isDirectChild)
                throw new ArgumentException("Parallax behaviour can only be applied to a direct child of the frame!");

            _originalPosition = obj.Position;
        }

        /// <summary>
        /// Updates the object's position relative to the camera.
        /// </summary>
        public override void UpdateObjectState(DynamicObject obj)
        {
            var cam = GameEngine.Current.Frame.Camera;
            obj.Position = _originalPosition + (1 - _coefficient)*cam.Position;
        }

        #endregion
    }
}
