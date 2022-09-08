using System.Diagnostics;
using Corund.Tools.Helpers;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Jitter;

/// <summary>
/// Color jitter effect.
/// </summary>
[DebuggerDisplay("ColorJitter: [{_descriptor.Name}] {Range} ({Rate}/s, relative = {_isRelative})")]
public class ColorJitter<TObject, TPropBase> : PropertyJitter<TObject, TPropBase, Color, Vector4>
    where TObject: DynamicObject, TPropBase
{
    #region Constructor

    /// <summary>
    /// Creates a new ColorJitter effect.
    /// </summary>
    /// <param name="descriptor">Property to affect.</param>
    /// <param name="rate">Number of jits per second.</param>
    /// <param name="range">Jitter magnitude for color components.</param>
    /// <param name="isRelative">Flag indicating that magnitude is a fraction of the actual value, rather than an absolute.</param>
    public ColorJitter(IPropertyDescriptor<TPropBase, Color> descriptor, float rate, Vector4 range, bool isRelative = false)
        : base(descriptor, rate, range, isRelative)
    {
    }

    #endregion

    #region Overrides

    protected override Color Add(Color a, Vector4 b) => new Color(a.ToVector4() + b);
    protected override Color Subtract(Color a, Vector4 b) => new Color(a.ToVector4() - b);

    protected override Vector4 Generate(Color value)
    {
        var r = _isRelative ? Range * value.ToVector4() : Range;
        return new Vector4(
            RandomHelper.Float(-r.X, r.X),
            RandomHelper.Float(-r.Y, r.Y),
            RandomHelper.Float(-r.Z, r.Z),
            RandomHelper.Float(-r.W, r.W)
        );
    }

    #endregion
}