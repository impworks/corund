using System.Diagnostics;
using Corund.Tools.Helpers;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Jitter;

/// <summary>
/// Float jitter effect.
/// </summary>
[DebuggerDisplay("ColorJitter: [{_descriptor.Name}] {_range} (each {_delay} s, relative = {_isRelative})")]
public class FloatJitter<TObject, TPropBase> : PropertyJitterBase<TObject, TPropBase, float>
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
        : base(descriptor, delay)
    {
        _range = range;
        _isRelative = isRelative;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Jitter range.
    /// </summary>
    private readonly float _range;

    /// <summary>
    /// Flag indicating that the range is a fraction of the actual value, rather than an absolute.
    /// </summary>
    private readonly bool _isRelative;

    /// <summary>
    /// Previously applied jitter.
    /// </summary>
    private float _lastJitter;

    #endregion

    #region Methods

    /// <summary>
    /// Cancels out previous jitter.
    /// </summary>
    protected override float CancelPrevious(float value)
    {
        return value - _lastJitter;
    }

    /// <summary>
    /// Applies new jitter.
    /// </summary>
    protected override float ApplyNew(float value)
    {
        var r = _range;
        if (_isRelative)
            r *= value;

        _lastJitter = RandomHelper.Float(-r, r);
        return value + _lastJitter;
    }

    #endregion
}