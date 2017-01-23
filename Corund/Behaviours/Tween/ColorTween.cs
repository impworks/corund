using System.Diagnostics;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Tween
{
    /// <summary>
    /// Animation for color properties.
    /// </summary>
    [DebuggerDisplay("ColorTween: [{_descriptor.Name}] {_initialValue} => {_targetValue} ({_duration} s)")]
    public class ColorTween<TObject> : PropertyTweenBase<TObject, Color>, IReversible<ColorTween<TObject>>
        where TObject : DynamicObject
    {
        public ColorTween(IPropertyDescriptor<TObject, Color> descriptor, Color targetValue, float duration, InterpolationMethod interpolation = null)
            : base(descriptor, targetValue, duration, interpolation)
        { }

        /// <summary>
        /// Creates interpolated value.
        /// </summary>
        protected override Color getValue()
        {
            return new Color(
                getFloat(_initialValue.R, _targetValue.R),
                getFloat(_initialValue.G, _targetValue.G),
                getFloat(_initialValue.B, _targetValue.B),
                getFloat(_initialValue.A, _targetValue.A)
            );
        }

        /// <summary>
        /// Creates an effect that cancels out the current tween.
        /// </summary>
        public ColorTween<TObject> Reverse(float? duration = null)
        {
            return new ColorTween<TObject>(_descriptor, _initialValue, duration ?? _duration, _interpolation);
        }
    }
}
