using System;
using Corund.Engine;
using Corund.Tools.Interpolation;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Tweening
{
    /// <summary>
    /// Base class for property animation behaviours.
    /// </summary>
    public abstract class PropertyAnimationBase<T>: BehaviourBase, IPropertyAnimation
    {
        #region Constructor

        protected PropertyAnimationBase(T initial, T target, float duration, Action<T> setter, InterpolationMethod interpolation = null)
        {
            _initialValue = initial;
            _targetValue = target;
            _duration = duration;
            _setter = setter;
            _interpolation = interpolation ?? Interpolate.Linear;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Value of the property at the start of the animation.
        /// </summary>
        protected readonly T _initialValue;

        /// <summary>
        /// Value of the property when the animation is complete.
        /// </summary>
        protected readonly T _targetValue;

        /// <summary>
        /// Estimated time of the effect.
        /// </summary>
        private readonly float _duration;

        /// <summary>
        /// Interpolation method to use.
        /// </summary>
        private readonly InterpolationMethod _interpolation;

        /// <summary>
        /// Property setter.
        /// </summary>
        private readonly Action<T> _setter;

        /// <summary>
        /// Currently elapsed time.
        /// </summary>
        private float _elapsedTime;

        #endregion

        #region Properties

        /// <summary>
        /// Duration of the effect.
        /// </summary>
        public float Duration => _duration;

        /// <summary>
        /// Current progress of the effect.
        /// </summary>
        public float? Progress => _elapsedTime/_duration;

        #endregion

        #region Methods

        /// <summary>
        /// Advances the property animation.
        /// </summary>
        public override void UpdateObjectState(DynamicObject obj)
        {
            if (_elapsedTime == _duration)
            {
                GameEngine.InvokeDeferred(() => obj.Behaviours.Remove(this));
                return;
            }

            _elapsedTime += GameEngine.Delta;
            if (_elapsedTime > _duration)
                _elapsedTime = _duration;

            _setter(getValue());
        }

        /// <summary>
        /// Skips the animation, setting the property to target value.
        /// </summary>
        public void SkipAnimation(DynamicObject obj)
        {
            _elapsedTime = _duration;
            _setter(_targetValue);
        }

        /// <summary>
        /// Interpolates a float part of the actual value.
        /// </summary>
        protected float getFloat(float initial, float target)
        {
            return _interpolation(initial, target, _elapsedTime/_duration);
        }

        /// <summary>
        /// Gets the intermediate value between initial and target.
        /// </summary>
        protected abstract T getValue();

        #endregion
    }
}
