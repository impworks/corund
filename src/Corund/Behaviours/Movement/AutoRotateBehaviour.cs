using Corund.Visuals.Particles;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Movement
{
    /// <summary>
    /// Behaviour for binding angle to direction.
    /// </summary>
    public class AutoRotateBehaviour: IBehaviour, IParticleBehaviour
    {
        public void UpdateObjectState(DynamicObject obj)
        {
            obj.Angle = -obj.Direction;
        }

        public void UpdateParticleState(ParticleObject obj, ParticleSystem system)
        {
            obj.Angle = -obj.Direction;
        }
    }
}
