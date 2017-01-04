namespace Corund.Behaviours
{
    /// <summary>
    /// An effect or action that has a known duration in seconds.
    /// </summary>
    public interface IEffect
    {
        /// <summary>
        /// Duration in seconds.
        /// </summary>
        float Duration { get; }

        /// <summary>
        /// Current state of the effect.
        /// Null = not activated.
        /// 0 = just started.
        /// 1 = completed.
        /// </summary>
        float? State { get; }
    }
}
