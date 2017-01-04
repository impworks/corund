namespace Corund.Tools.Interpolation
{
    /// <summary>
    /// The interface for all tweening methods.
    /// </summary>
    /// <param name="min">Value range start.</param>
    /// <param name="max">Value range end.</param>
    /// <param name="tweenState">Tweening state between 0 (not applied) and 1 (completed).</param>
    /// <returns>Interpolated value.</returns>
    public delegate float InterpolationMethod(float min, float max, float tweenState);
}
