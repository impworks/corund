using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Corund.Visuals.Particles;

namespace Corund.Behaviours.Particles
{
    /// <summary>
    /// Helper class for managing behaviours for a particle system.
    /// </summary>
    public class ParticleBehaviourManager: IEnumerable<IParticleBehaviour>
    {
        public ParticleBehaviourManager(ParticleSystem sys)
        {
            _sys = sys;
            _behaviours = new List<IParticleBehaviour>();
        }

        private readonly ParticleSystem _sys;
        private readonly List<IParticleBehaviour> _behaviours;

        /// <summary>
        /// Adds a new behaviour.
        /// </summary>
        public void Add(IParticleBehaviour behaviour)
        {
            _behaviours.Add(behaviour);
        }

        /// <summary>
        /// Adds several behaviours.
        /// </summary>
        public void Add(params IParticleBehaviour[] behaviours)
        {
            _behaviours.AddRange(behaviours);
        }

        /// <summary>
        /// Removes a behaviour from the list.
        /// </summary>
        public void Remove(IParticleBehaviour behaviour)
        {
            _behaviours.Remove(behaviour);
        }

        /// <summary>
        /// Applies all behaviours to all particles.
        /// </summary>
        public void Update()
        {
            if(_behaviours.Any())
                foreach(var obj in _sys)
                    foreach(var b in _behaviours)
                        b.UpdateParticleState(obj, _sys);
        }

        #region IEnumerable implementation

        public IEnumerator<IParticleBehaviour> GetEnumerator() => _behaviours.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
