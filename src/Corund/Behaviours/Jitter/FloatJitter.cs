using System.Diagnostics;
using Corund.Tools.Helpers;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Jitter;

/// <summary>
/// Float jitter effect.
/// </summary>
[DebuggerDisplay("ColorJitter: [{_descriptor.Name}] {_range} (each {_delay} s, relative = {_isRelative})")]
public class FloatJitter<TObject, TPropBase> : PropertyJitterBase<TObject, TPropBase, float, float>
    where TObject: DynamicObject, TPropBase
{
    #region Constructor

    /// <summary>
    /// Creates a new FloatJitter effect.
    /// </summary>
    /// <param name="descriptor">Property to affect.</param>
    /// <param name="delay">Time between value changes in seconds.</param>
    /// <param name="range">Jitter magnitude.</param>
    /// <param name="isRelative">Flag indicating that magnitude is a fraction of the actual value, rather than an absolute.</param>
    public FloatJitter(IPropertyDescriptor<TPropBase, float> descriptor, float delay, float range, bool isRelative = false)
        : base(descriptor, delay, range, isRelative)
    {
    }

    #endregion

    #region Overrides

    protected override float Add(float a, float b) => a + b;
    protected override float Subtract(float a, float b) => a - b;

    protected override float Generate(float value)
    {
        var r = _range * (_isRelative ? value : 1);
        return RandomHelper.Float(-r, r);
    }

    #endregion
}