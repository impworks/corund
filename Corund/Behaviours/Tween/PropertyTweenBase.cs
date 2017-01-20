using System;
using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Tween
{
    /// <summary>
    /// Base class for property tween behaviours.
    /// </summary>
    public abstract class PropertyTweenBase<TObject, TProperty>: BehaviourBase, IPropertyTween
        where TObject: DynamicObject
    {
        #region Constructor

        protected PropertyTweenBase(IPropertyDescriptor<TObject, TProperty> descriptor, TProperty targetValue, float duration, InterpolationMethod interpolation = null)
        {
            if(duration.IsAlmostNull())
                throw new ArgumentException("Effect duration cannot be null.", nameof(duration));

            _descriptor = descriptor;
            _targetValue = targetValue;
            _duration = duration;
            _interpolation = interpolation ?? Interpolate.Linear;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Value of the property at the start of the animation.
        /// </summary>
        protected TProperty _initialValue;

        /// <summary>
        /// Value of the property when the animation is complete.
        /// </summary>
        protected readonly TProperty _targetValue;

        /// <summary>
        /// Estimated time of the effect.
        /// </summary>
        private readonly float _duration;

        /// <summary>
        /// Interpolation method to use.
        /// </summary>
        private readonly InterpolationMethod _interpolation;

        /// <summary>
        /// Property getter.
        /// </summary>
        private readonly IPropertyDescriptor<TObject, TProperty> _descriptor;

        /// <summary>
        /// Currently elapsed time.
        /// </summary>
        private float _elapsedTime;

        #endregion

        #region Properties

        /// <summary>
        /// Name of the property handled by current tween.
        /// </summary>
        public string PropertyName => _descriptor.Name;

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
        /// Sets the initial object value.
        /// </summary>
        public override void Bind(DynamicObject obj)
        {
            _initialValue = _descriptor.Getter((TObject) obj);
        }

        /// <summary>
        /// Advances the property animation.
        /// </summary>
        public override void UpdateObjectState(DynamicObject obj)
        {
            _elapsedTime += GameEngine.Delta;
            if (_elapsedTime > _duration)
                _elapsedTime = _duration;

            _descriptor.Setter((TObject) obj, getValue());
        }

        /// <summary>
        /// Skips the animation, setting the property to target value.
        /// </summary>
        public void StopTween(DynamicObject obj, bool skipToFinalValue)
        {
            _elapsedTime = _duration;

            if(skipToFinalValue)
                _descriptor.Setter((TObject)obj, _targetValue);
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
        protected abstract TProperty getValue();

        #endregion
    }
}
