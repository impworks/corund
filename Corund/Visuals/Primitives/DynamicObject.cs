using Corund.Behaviours;
using Corund.Behaviours.Fade;
using Corund.Engine;
using Corund.Shaders;
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

        /// <summary>
        /// Current shader effect applied to the object.
        /// </summary>
        public ShaderBase Shader;

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

            Shader?.Update();
        }

        /// <summary>
        /// Handles effects.
        /// </summary>
        public override void Draw()
        {
            if (!IsVisible)
                return;

            if(Shader == null)
                DrawInternal();
            else
                Shader.DrawWrapper(this, DrawInternal);
        }

        /// <summary>
        /// Draws the object to the render target.
        /// </summary>
        protected abstract void DrawInternal();

        #endregion

        #region Methods

        /// <summary>
        /// RemoveSelf the object by applying fadeout effects.
        /// </summary>
        protected void FadeOut()
        {
            if (IsFadingOut)
                return;

            IsFadingOut = true;

            var timeout = 0f;
            foreach (var curr in Behaviours)
            {
                if (curr is IFadeOutEffect fadeOut)
                {
                    // no break intended: activate ALL the effects! \o/
                    fadeOut.ActivateFadeOut(this);

                    if (fadeOut.Duration > timeout)
                        timeout = fadeOut.Duration;
                }
            }

            if (timeout.IsAlmostZero())
                RemoveSelf(true);
            else
                GameEngine.Current.Timeline.Add(timeout, () => RemoveSelf(true));
        }

        /// <summary>
        /// Fires fade out effects and removes the object afterwards.
        /// </summary>
        public override void RemoveSelf(bool immediate = false)
        {
            if(immediate)
                base.RemoveSelf(immediate);
            else
                FadeOut();
        }

        #endregion
    }
}
