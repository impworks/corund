using System.Diagnostics;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Tween
{
    /// <summary>
    /// Animation for vector2 values.
    /// </summary>
    [DebuggerDisplay("Vector2Tween: [{_descriptor.Name}] {_initialValue} => {_targetValue} ({_duration} s)")]
    public class Vector2Tween<TObject, TPropBase> : PropertyTweenBase<TObject, TPropBase, Vector2>, IReversible<Vector2Tween<TObject, TPropBase>>
       where TObject : DynamicObject, TPropBase
    {
        public Vector2Tween(IPropertyDescriptor<TPropBase, Vector2> descriptor, Vector2 targetValue, float duration, InterpolationMethod interpolation = null)
            : base(descriptor, targetValue, duration, interpolation)
        { }

        /// <summary>
        /// Creates interpolated value.
        /// </summary>
        protected override Vector2 GetValue()
        {
            return new Vector2(
                GetFloat(_initialValue.X, _targetValue.X),
                GetFloat(_initialValue.Y, _targetValue.Y)
            );  
        }

        /// <summary>
        /// Creates an effect that cancels out the current tween.
        /// </summary>
        public Vector2Tween<TObject, TPropBase> Reverse(float? duration = null)
        {
            return new Vector2Tween<TObject, TPropBase>(_descriptor, _initialValue, duration ?? _duration, _interpolation);
        }
    }
}
