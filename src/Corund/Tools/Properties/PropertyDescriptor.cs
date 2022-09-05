using System;
using System.Diagnostics;

namespace Corund.Tools.Properties;

/// <summary>
/// A descriptor for property that can get and set values.
/// </summary>
[DebuggerDisplay("Property: {Name}")]
public class PropertyDescriptor<TObject, TProperty>: IPropertyDescriptor<TObject, TProperty>
{
    #region Constructor

    public PropertyDescriptor(Func<TObject, TProperty> getter, Action<TObject, TProperty> setter, string name)
    {
        Getter = getter;
        Setter = setter;
        Name = name;
    }

    #endregion

    #region Properties

    /// <summary>
    /// A function that returns the property's current value.
    /// </summary>
    public Func<TObject, TProperty> Getter { get; }

    /// <summary>
    /// A function that updates the property's current value.
    /// </summary>
    public Action<TObject, TProperty> Setter { get; }

    /// <summary>
    /// Property name.
    /// </summary>
    public string Name { get; }

    #endregion
}