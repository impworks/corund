using System.Diagnostics;

namespace Corund.Tools;

/// <summary>
/// A range of float values.
/// </summary>
[DebuggerDisplay("Range ({Min}..{Max})")]
public struct FloatRange
{
    public FloatRange(float min, float max)
    {
        Min = min;
        Max = max;
    }

    public readonly float Min;
    public readonly float Max;
}