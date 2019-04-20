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
    public class ColorTween<TObject, TPropBase> : PropertyTweenBase<TObject, TPropBase, Color>, IReversible<ColorTween<TObject, TPropBase>>
        where TObject : DynamicObject, TPropBase
    {
        public ColorTween(IPropertyDescriptor<TPropBase, Color> descriptor, Color targetValue, float duration, InterpolationMethod interpolation = null)
            : base(descriptor, targetValue, duration, interpolation)
        { }

        /// <summary>
        /// Creates interpolated value.
        /// </summary>
        protected override Color GetValue()
        {
            return new Color(
                GetFloat(_initialValue.R, _targetValue.R),
                GetFloat(_initialValue.G, _targetValue.G),
                GetFloat(_initialValue.B, _targetValue.B),
                GetFloat(_initialValue.A, _targetValue.A)
            );
        }

        /// <summary>
        /// Creates an effect that cancels out the current tween.
        /// </summary>
        public ColorTween<TObject, TPropBase> Reverse(float? duration = null)
        {
            return new ColorTween<TObject, TPropBase>(_descriptor, _initialValue, duration ?? _duration, _interpolation);
        }
    }
}
