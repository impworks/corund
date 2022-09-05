namespace Corund.Behaviours;

/// <summary>
/// The interface for effects that can be reversed.
/// </summary>
public interface IReversible<T>
{
    /// <summary>
    /// Creates an effect that cancels out the current effect.
    /// </summary>
    /// <param name="duration">New duration, or null to use current.</param>
    T Reverse(float? duration = null);
}