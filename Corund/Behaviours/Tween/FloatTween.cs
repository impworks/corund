using System.Diagnostics;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Tween
{
    /// <summary>
    /// Animation for float values.
    /// </summary>
    [DebuggerDisplay("FloatTween: [{_descriptor.Name}] {_initialValue} => {_targetValue} ({_duration} s)")]
    public class FloatTween<TObject>: PropertyTweenBase<TObject, float>, IReversible<FloatTween<TObject>>
        where TObject: DynamicObject
    {
        public FloatTween(IPropertyDescriptor<TObject, float> descriptor, float targetValue, float duration, InterpolationMethod interpolation = null)
            : base(descriptor, targetValue, duration, interpolation)
        { }

        /// <summary>
        /// Creates interpolated value.
        /// </summary>
        protected override float getValue()
        {
            return getFloat(_initialValue, _targetValue);
        }

        /// <summary>
        /// Creates an effect that cancels out the current tween.
        /// </summary>
        public FloatTween<TObject> Reverse(float? duration = null)
        {
            return new FloatTween<TObject>(_descriptor, _initialValue, duration ?? _duration, _interpolation);
        }
    }
}
