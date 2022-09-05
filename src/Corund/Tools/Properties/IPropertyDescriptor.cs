using System;

namespace Corund.Tools.Properties;

/// <summary>
/// Property descriptor interface (to support covariance in object type).
/// </summary>
public interface IPropertyDescriptor<in TObject, TProperty>
{
    /// <summary>
    /// A function that returns the property's current value.
    /// </summary>
    Func<TObject, TProperty> Getter { get; }

    /// <summary>
    /// A function that updates the property's current value.
    /// </summary>
    Action<TObject, TProperty> Setter { get; }

    /// <summary>
    /// Property name.
    /// </summary>
    string Name { get; }
}