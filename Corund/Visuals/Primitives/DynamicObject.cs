using Corund.Behaviours;
using Corund.Behaviours.Fade;
using Corund.Engine;
using Corund.Tools;
using Corund.Tools.Helpers;

namespace Corund.Visuals.Primitives
{
    /// <summary>
    /// An object that can be augmented by behaviours.
    /// </summary>
    public abstract class DynamicObject : MovingObject
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
            base.Update();

            var pm = GameEngine.Current.PauseMode | PauseMode;

            if ((pm & PauseMode.Behaviours) == 0)
                Behaviours.Update();
        }

        /// <summary>
        /// Handles effects.
        /// </summary>
        public override void Draw()
        {
            if (!IsVisible)
                return;

            DrawInternal();
        }

        /// <summary>
        /// Draws the object to the render target.
        /// </summary>
        protected abstract void DrawInternal();

        #endregion

        #region Methods



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
                    fadeOut.ActivateFadeOut(this);

                    if (fadeOut.Duration > timeout)
                        timeout = fadeOut.Duration;
                }
            }

            if (timeout.IsAlmostNull())
                Remove();
            else
                GameEngine.Current.Timeline.Add(timeout, () => Remove());
        }

        #endregion
    }
}
