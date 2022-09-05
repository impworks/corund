using System.Collections.Generic;

namespace Corund.Visuals.Primitives;

/// <summary>
/// Interface for object groups that keep track of objects.
/// </summary>
public interface IObjectGroup: IEnumerable<ObjectBase>
{
    /// <summary>
    /// Removes the object from this group.
    /// The group may need to update its geometry.
    /// </summary>
    void Remove(ObjectBase obj);

    /// <summary>
    /// Removes all objects from the group.
    /// </summary>
    void Clear();

    /// <summary>
    /// Number of objects in the group.
    /// </summary>
    int Count { get; }
}