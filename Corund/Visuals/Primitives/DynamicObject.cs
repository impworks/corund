using System;
using Corund.Behaviours;
using Corund.Behaviours.Fade;
using Corund.Engine;
using Corund.Tools;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

namespace Corund.Visuals.Primitives
{
    /// <summary>
    /// An object that can move, rotate, scale and change transparency.
    /// </summary>
    public abstract class DynamicObject : ObjectBase
    {
        #region Constructors

        protected DynamicObject()
        {
            Behaviours = new BehaviourManager(this);
        }

        #endregion

        #region Fields

        /// <summary>
        /// The behaviour manager for current object.
        /// </summary>
        public readonly BehaviourManager Behaviours;

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
            get { return Momentum.Length(); }
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
            get { return (float)Math.Atan2(Momentum.Y, Momentum.X); }
            set { Momentum = VectorHelper.FromLength(Momentum.Length(), value); }
        }

        /// <summary>
        /// Pause mode for current object.
        /// </summary>
        public PauseMode PauseMode;

        /// <summary>
        /// Has the object been disposed already?
        /// </summary>
        public bool IsFadingOut { get; protected set; }

        #endregion

        #region ObjectBase overrides

        /// <summary>
        /// Applies value derivatives to values.
        /// </summary>
        public override void Update()
        {
            var pm = GameEngine.Current.PauseMode | PauseMode;

            if ((pm & PauseMode.Behaviours) == 0)
                Behaviours.Update();

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

        /// <summary>
        /// Renders the object to the screen.
        /// </summary>
        protected override void DrawInternal()
        {
            // does nothing per se
        }

        #endregion

        /// <summary>
        /// Remove the object by applying fadeout effects.
        /// </summary>
        public void FadeOut()
        {
            if (IsFadingOut)
                return;

            IsFadingOut = true;

            var timeout = 0f;
            foreach (var curr in Behaviours)
            {
                var fadeOut = curr as IFadeOutEffect;
                if (fadeOut != null)
                {
                    // no break intended: activate ALL the effects! \o/
                    fadeOut.ActivateFadeOut();

                    if (fadeOut.Duration > timeout)
                        timeout = fadeOut.Duration;
                }
            }

            if (timeout.IsAlmostNull())
                Remove();
            else
                GameEngine.Current.Timeline.Add(timeout, Remove);
        }
    }
}
