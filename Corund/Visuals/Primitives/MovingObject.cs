using System;
using Corund.Engine;
using Corund.Tools;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

namespace Corund.Visuals.Primitives
{
    /// <summary>
    /// An object that can move and rotate.
    /// </summary>
    public abstract class MovingObject: ObjectBase
    {
        #region Properties

        /// <summary>
        /// Angle derivative.
        /// Sprite is rotated by this value each second.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Sprite momentum.
        /// Sprite is moved by this value each second.
        /// </summary>
        public Vector2 Momentum;

        /// <summary>
        /// Gets or sets object's speed.
        /// </summary>
        public float Speed
        {
            get => Momentum.Length();
            set
            {
                if (Momentum.X.IsAlmostNull() && Momentum.Y.IsAlmostNull())
                {
                    Momentum = new Vector2(value, 0);
                }
                else
                {
                    Momentum.Normalize();
                    Momentum *= value;
                }
            }
        }

        /// <summary>
        /// Speed derivative.
        /// </summary>
        public float Acceleration;

        /// <summary>
        /// Gets or sets the object's direction.
        /// </summary>
        public float Direction
        {
            get => (float)Math.Atan2(Momentum.Y, Momentum.X);
            set => Momentum = VectorHelper.FromLength(Momentum.Length(), value);
        }

        #endregion

        #region Methods

        public override void Update()
        {
            var pm = GameEngine.Current.PauseMode | PauseMode;
            var delta = GameEngine.Delta;

            if ((pm & PauseMode.Momentum) == 0)
            {
                if (!Rotation.IsAlmostNull())
                    Angle += delta * Rotation;

                if (!Acceleration.IsAlmostNull())
                    Speed += delta * Acceleration;

                if (!Momentum.X.IsAlmostNull() || !Momentum.Y.IsAlmostNull())
                    Position += delta * Momentum;
            }
        }

        #endregion
    }
}
