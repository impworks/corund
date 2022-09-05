using System.Diagnostics;
using Corund.Tools.Helpers;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Jitter;

/// <summary>
/// Color jitter effect.
/// </summary>
[DebuggerDisplay("ColorJitter: [{_descriptor.Name}] {_range} (each {_delay} s, relative = {_isRelative})")]
public class ColorJitter<TObject, TPropBase> : PropertyJitterBase<TObject, TPropBase, Color>
    where TObject: DynamicObject, TPropBase
{
    #region Constructor

    /// <summary>
    /// Creates a new FloatJitter effect.
    /// </summary>
    /// <param name="descriptor">Property to affect.</param>
    /// <param name="delay">Time between value changes in seconds.</param>
    /// <param name="range">Jitter magnitude for color components.</param>
    /// <param name="isRelative">Flag indicating that magnitude is a fraction of the actual value, rather than an absolute.</param>
    public ColorJitter(IPropertyDescriptor<TPropBase, Color> descriptor, float delay, Vector4 range, bool isRelative = false)
        : base(descriptor, delay)
    {
        _range = range;
        _isRelative = isRelative;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Jitter range for all color components.
    /// </summary>
    private readonly Vector4 _range;

    /// <summary>
    /// Flag indicating that the range is a fraction of the actual value, rather than an absolute.
    /// </summary>
    private readonly bool _isRelative;

    /// <summary>
    /// Previously applied jitter.
    /// </summary>
    private Vector4 _lastJitter;

    #endregion

    #region Methods

    /// <summary>
    /// Cancels out previous jitter.
    /// </summary>
    protected override Color CancelPrevious(Color value)
    {
        return new Color(value.ToVector4() - _lastJitter);
    }

    /// <summary>
    /// Applies new jitter.
    /// </summary>
    protected override Color ApplyNew(Color value)
    {
        var vec = value.ToVector4();

        var range = _range;
        if (_isRelative)
            range *= vec;

        _lastJitter = new Vector4(
            RandomHelper.Float(-range.X, range.X),
            RandomHelper.Float(-range.Y, range.Y),
            RandomHelper.Float(-range.Z, range.Z),
            RandomHelper.Float(-range.W, range.W)
        );
        return new Color(vec + _lastJitter);
    }

    #endregion
}