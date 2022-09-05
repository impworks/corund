using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Jitter;

public interface IPropertyJitter
{
    /// <summary>
    /// Name of the tweened property.
    /// </summary>
    string PropertyName { get; }

    /// <summary>
    /// Updates the property value.
    /// The property tween stores the property and jitter settings internally.
    /// </summary>
    void UpdateObjectState(DynamicObject obj);
}