using System;
using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Tools.Jitter;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals.Particles
{
    /// <summary>
    /// A group of particle objects.
    /// </summary>
    public class ParticleGroup: ObjectGroup
    {
        #region Constructor

        public ParticleGroup(int rate, Func<DynamicObject> factory)
        {
            GenerationRate = rate;
            _particleFactory = factory;

            BlendState = BlendState.AlphaBlend;
        }

        #endregion

        #region Fields

        /// <summary>
        /// A function that creates particles.
        /// </summary>
        private readonly Func<DynamicObject> _particleFactory;

        /// <summary>
        /// Time elapsed since last particle has been created.
        /// </summary>
        private float _particleElapsedTime;

        /// <summary>
        /// Delay between two particles.
        /// </summary>
        private float _particleDelay;

        /// <summary>
        /// Number of particles per second to create.
        /// </summary>
        private int _generationRate;

        #endregion

        #region Properties

        /// <summary>
        /// Flag indicating that the group actively generates new particles.
        /// </summary>
        public bool IsActive;

        /// <summary>
        /// Number of particles generated during the entire lifespan of the group.
        /// </summary>
        public int TotalParticleCount;

        /// <summary>
        /// Blend state for the particle system.
        /// </summary>
        public BlendState BlendState;

        /// <summary>
        /// Jittery location at which particles are created.
        /// </summary>
        public JitteryVector2 ParticleOrigin;

        /// <summary>
        /// Jittery angle of particle propulsion.
        /// </summary>
        public JitteryValue ParticleAngle;

        /// <summary>
        /// Jittery speed of particle propulsion.
        /// </summary>
        public JitteryValue ParticleSpeed;

        /// <summary>
        /// Jittery lifespan of each particle in seconds.
        /// </summary>
        public JitteryValue ParticleTimeToLive;

        /// <summary>
        /// Number of particles to create per second.
        /// </summary>
        public int GenerationRate
        {
            get { return _generationRate; }
            set
            {
                _generationRate = value;
                _particleDelay = 1.0f/value;
            }
        }

        #endregion

        #region Methods

        public override void Update()
        {
            base.Update();

            if (IsActive)
            {
                _particleElapsedTime += GameEngine.Delta;
                while (_particleElapsedTime > _particleDelay)
                {
                    _particleElapsedTime -= _particleDelay;

                    var newParticle = _particleFactory();
                    ConfigureParticle(newParticle);
                    Add(newParticle);

                    TotalParticleCount++;
                }
            }
        }

        /// <summary>
        /// Draws the particle system.
        /// </summary>
        protected override void DrawInternal()
        {
            GameEngine.Render.TryBeginBatch(BlendState);

            base.DrawInternal();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Applies settings to a newly created particle.
        /// </summary>
        public virtual void ConfigureParticle(DynamicObject obj)
        {
            obj.Position = ParticleOrigin.GetValue();
            obj.Momentum = VectorHelper.FromLength(ParticleSpeed.GetValue(), ParticleAngle.GetValue());

            GameEngine.Current.Timeline.Add(ParticleTimeToLive.GetValue(), obj.FadeOut);
        }

        #endregion
    }
}
