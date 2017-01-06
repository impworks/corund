using System;
using System.Linq.Expressions;
using Corund.Engine;
using Corund.Tools;
using Corund.Tools.Helpers;
using Corund.Tools.Interpolation;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Tweening
{
    /// <summary>
    /// Base class for property animation behaviours.
    /// </summary>
    public abstract class PropertyAnimationBase<TObject, TProperty>: BehaviourBase, IPropertyAnimation
        where TObject: DynamicObject
    {
        #region Constructor

        protected PropertyAnimationBase(Expression<Func<TObject, TProperty>> property, TProperty targetValue, float duration, InterpolationMethod interpolation = null)
        {
            _descriptor = PropertyHelper.GetDescriptor(property);

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
        private readonly PropertyDescriptor<TObject, TProperty> _descriptor;

        /// <summary>
        /// Currently elapsed time.
        /// </summary>
        private float _elapsedTime;

        #endregion

        #region Properties

        /// <summary>
        /// Name of the property handled by current animator.
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
            if (_elapsedTime == _duration)
            {
                GameEngine.InvokeDeferred(() => obj.Behaviours.Remove(this));
                return;
            }

            _elapsedTime += GameEngine.Delta;
            if (_elapsedTime > _duration)
                _elapsedTime = _duration;

            _descriptor.Setter((TObject) obj, getValue());
        }

        /// <summary>
        /// Skips the animation, setting the property to target value.
        /// </summary>
        public void StopAnimation(DynamicObject obj, bool skipToFinalValue)
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
