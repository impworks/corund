using System;
using Corund.Visuals.Particles;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Particles
{
    /// <summary>
    /// Particle modifier that fades a particle out until it disappears.
    /// </summary>
    public class ParticleFadeBehaviour : IParticleBehaviour
    {
        /// <summary>
        /// Creates a new FadeModifier instance.
        /// </summary>
        /// <param name="startThreshold">Fade start threshold (0..1). Particle is fully opaque before it's age reached this value.</param>
        /// <param name="endThreshold">Fade end threshold (0..1). Particle is fully transparent after it's age reached this value.</param>
        public ParticleFadeBehaviour(float startThreshold = 0f, float endThreshold = 1f)
        {
            if (startThreshold < 0 || startThreshold > 1)
                throw new ArgumentOutOfRangeException(nameof(startThreshold));

            if (endThreshold < 0 || endThreshold > 1)
                throw new ArgumentOutOfRangeException(nameof(endThreshold));

            if (endThreshold <= startThreshold)
                throw new ArgumentOutOfRangeException(nameof(startThreshold), "The startThreshold value must be lower than endThreshold.");

            _startThreshold = startThreshold;
            _endThreshold = endThreshold;
        }

        private readonly float _startThreshold;
        private readonly float _endThreshold;

        public void UpdateParticleState(ParticleObject obj, ParticleSystem system)
        {
            var k = (obj.Age - _startThreshold) * (_endThreshold - _startThreshold);
            obj.Opacity = 1 - MathHelper.Clamp(k, 0, 1);
        }
    }
}
