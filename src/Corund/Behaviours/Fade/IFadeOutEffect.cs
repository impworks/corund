using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Fade;

/// <summary>
/// Marker interface for fade out effects.
/// </summary>
public interface IFadeOutEffect: IEffect
{
    /// <summary>
    /// Start the fade out effect.
    /// </summary>
    void ActivateFadeOut(DynamicObject obj);
}