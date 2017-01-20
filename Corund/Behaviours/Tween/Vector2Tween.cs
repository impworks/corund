using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Tween
{
    /// <summary>
    /// Animation for vector2 values.
    /// </summary>
    public class Vector2Tween<TObject> : PropertyTweenBase<TObject, Vector2>
       where TObject : DynamicObject
    {
        public Vector2Tween(IPropertyDescriptor<TObject, Vector2> descriptor, Vector2 targetValue, float duration, InterpolationMethod interpolation = null)
            : base(descriptor, targetValue, duration, interpolation)
        { }

        protected override Vector2 getValue()
        {
            return new Vector2(
                getFloat(_initialValue.X, _targetValue.X),
                getFloat(_initialValue.Y, _targetValue.Y)
            );  
        }
    }
}
