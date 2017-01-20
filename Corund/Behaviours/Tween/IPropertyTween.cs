using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Tween
{
    public interface IPropertyTween: IEffect
    {
        /// <summary>
        /// Name of the tweened property.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Updates the property value.
        /// The property tween stores the property, easing, time and value ranges internally.
        /// </summary>
        void UpdateObjectState(DynamicObject obj);

        /// <summary>
        /// Skips the tween, optionally setting the property to target value.
        /// </summary>
        void StopTween(DynamicObject obj, bool skipToFinalValue);
    }
}
