using Corund.Visuals.Primitives;

namespace Corund.Behaviours;

/// <summary>
/// An attachable class that modifies the properties of a dynamic object.
/// </summary>
public interface IBehaviour
{
    /// <summary>
    /// Apply the behaviour to the object.
    /// </summary>
    void UpdateObjectState(DynamicObject obj);
}