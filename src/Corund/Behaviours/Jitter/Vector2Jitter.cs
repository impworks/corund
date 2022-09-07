using System.Diagnostics;
using Corund.Tools.Helpers;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Jitter;

/// <summary>
/// Float jitter effect.
/// </summary>
[DebuggerDisplay("Vector2Jitter: [{_descriptor.Name}] {_xRange}, {_yRange} (each {_delay} s, relative = {_isRelative})")]
public class Vector2Jitter<TObject, TPropBase> : PropertyJitter<TObject, TPropBase, Vector2, Vector2>
    where TObject : DynamicObject, TPropBase
{
    #region Constructor

    /// <summary>
    /// Creates a new Vector2Jitter effect.
    /// </summary>
    /// <param name="descriptor">Property to affect.</param>
    /// <param name="delay">Time between value changes in seconds.</param>
    /// <param name="range">Jitter magnitude.</param>
    /// <param name="isRelative">Flag indicating that magnitude is a fraction of the actual value, rather than an absolute.</param>
    public Vector2Jitter(IPropertyDescriptor<TPropBase, Vector2> descriptor, float delay, Vector2 range, bool isRelative = false)
        : base(descriptor, delay, range, isRelative)
    {
    }

    #endregion

    #region Overrides

    protected override Vector2 Add(Vector2 a, Vector2 b) => a + b;
    protected override Vector2 Subtract(Vector2 a, Vector2 b) => a - b;

    protected override Vector2 Generate(Vector2 value)
    {
        var r = _isRelative ? _range * value : _range;
        return new Vector2(
            RandomHelper.Float(-r.X, r.X),
            RandomHelper.Float(-r.Y, r.Y)
        );
    }

    #endregion
}