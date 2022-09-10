using System;
using Corund.Visuals.Particles;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Particles
{
    /// <summary>
    /// Particle modifier that alters the particle's scale during its age.
    /// </summary>
    public class ParticleScaleBehaviour : IParticleBehaviour
    {
        /// <summary>
        /// Creates a behaviour that updates the object's scale.
        /// </summary>
        /// <param name="scale">Resulting scale.</param>
        /// <param name="startThreshold">Scale start threshold (0..1). Particle scale is 1 before it's age reached this value.</param>
        /// <param name="endThreshold">Scale end threshold (0..1). Particle scale is  transparent after it's age reached this value.</param>
        public ParticleScaleBehaviour(float scale, float startThreshold = 0f, float endThreshold = 1f)
            : this(new Vector2(scale, scale), startThreshold, endThreshold)
        {
        }

        /// <summary>
        /// Creates a behaviour that updates the object's scale.
        /// </summary>
        /// <param name="scale">Resulting scale.</param>
        /// <param name="startThreshold">Scale start threshold (0..1). Particle scale is 1 before it's age reached this value.</param>
        /// <param name="endThreshold">Scale end threshold (0..1). Particle scale is  transparent after it's age reached this value.</param>
        public ParticleScaleBehaviour(Vector2 scale, float startThreshold = 0f, float endThreshold = 1f)
        {
            if (startThreshold < 0 || startThreshold > 1)
                throw new ArgumentOutOfRangeException(nameof(startThreshold));

            if (endThreshold < 0 || endThreshold > 1)
                throw new ArgumentOutOfRangeException(nameof(endThreshold));

            if (endThreshold <= startThreshold)
                throw new ArgumentOutOfRangeException(nameof(startThreshold), "The startThreshold value must be lower than endThreshold.");

            _scale = scale;
            _startThreshold = startThreshold;
            _endThreshold = endThreshold;
        }

        private readonly Vector2 _scale;
        private readonly float _startThreshold;
        private readonly float _endThreshold;

        public void UpdateParticleState(ParticleObject obj, ParticleSystem system)
        {
            obj.ScaleVector = GetScale(obj);
        }

        private Vector2 GetScale(ParticleObject obj)
        {
            var age = obj.Age;
            if(age < _startThreshold)
                return Vector2.One;
            if (age >= _endThreshold)
                return _scale;

            var k = (obj.Age - _startThreshold) * (_endThreshold - _startThreshold);
            return Vector2.One + (_scale - Vector2.One) * k;
        }
    }
}
