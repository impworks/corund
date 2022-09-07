using Corund.Visuals.Particles;

namespace Corund.Behaviours
{
    /// <summary>
    /// Interface for behaviours that can be applied to particle systems.
    /// Since only one behaviour instance is created per the entire particle system (not per each particle), this behaviour cannot contain internal state.
    /// It also cannot be used together with IBindableBehaviour since no binding is made.
    /// </summary>
    public interface IParticleBehaviour
    {
        /// <summary>
        /// Updates the state of a single particle.
        /// </summary>
        void UpdateParticleState(ParticleObject obj, ParticleSystem system);
    }
}
