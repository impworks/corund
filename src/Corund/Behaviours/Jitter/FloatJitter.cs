using System.Diagnostics;
using Corund.Tools.Helpers;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Jitter;

/// <summary>
/// Float jitter effect.
/// </summary>
[DebuggerDisplay("ColorJitter: [{_descriptor.Name}] {Range} ({Rate}/s, relative = {_isRelative})")]
public class FloatJitter<TObject, TPropBase> : PropertyJitter<TObject, TPropBase, float, float>
    where TObject: DynamicObject, TPropBase
{
    #region Constructor

    /// <summary>
    /// Creates a new FloatJitter effect.
    /// </summary>
    /// <param name="descriptor">Property to affect.</param>
    /// <param name="rate">Number of jits per second.</param>
    /// <param name="range">Jitter magnitude.</param>
    /// <param name="isRelative">Flag indicating that magnitude is a fraction of the actual value, rather than an absolute.</param>
    public FloatJitter(IPropertyDescriptor<TPropBase, float> descriptor, float rate, float range, bool isRelative = false)
        : base(descriptor, rate, range, isRelative)
    {
    }

    #endregion

    #region Overrides

    protected override float Add(float a, float b) => a + b;
    protected override float Subtract(float a, float b) => a - b;

    protected override float Generate(float value)
    {
        var r = Range * (_isRelative ? value : 1);
        return RandomHelper.Float(-r, r);
    }

    #endregion
}