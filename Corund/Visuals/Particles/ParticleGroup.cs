using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Tools.Jitter;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals.Particles
{
    /// <summary>
    /// A group of particle objects.
    /// </summary>
    public abstract class ParticleGroup: ObjectGroup
    {
        #region Constructor

        public ParticleGroup(int rate)
        {
            ParticleBlendState = BlendState.AlphaBlend;
            GenerationRate = rate;

            IsActive = true;
        }

        #endregion

        #region Fields

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
        public BlendState ParticleBlendState;

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
        public JitteryValue ParticleLifeDuration;

        /// <summary>
        /// Jittery duration of a particle's fade out effect.
        /// </summary>
        public JitteryValue ParticleFadeDuration;

        /// <summary>
        /// Maximum number of particles before system self-destructs.
        /// </summary>
        public int? ParticleLimit;

        /// <summary>
        /// Number of particles to create per second.
        /// </summary>
        public int GenerationRate
        {
            get => _generationRate;
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

                    var newParticle = CreateParticle();
                    ConfigureParticle(newParticle);
                    Add(newParticle);

                    TotalParticleCount++;

                    if (TotalParticleCount >= ParticleLimit)
                    {
                        OnLimitReached();
                        break;
                    }
                }
            }

            Children.RemoveAll(obj =>
            {
                var pt = obj as ParticleObject;
                return pt.ElapsedTime > pt.LifeDuration + pt.FadeDuration;
            });
        }

        /// <summary>
        /// Draws the particle system.
        /// </summary>
        protected override void DrawInternal()
        {
            GameEngine.Render.TryBeginBatch(ParticleBlendState);

            base.DrawInternal();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Creates a new particle.
        /// </summary>
        protected abstract ParticleObject CreateParticle();

        /// <summary>
        /// Applies settings to a newly created particle.
        /// </summary>
        protected virtual void ConfigureParticle(ParticleObject obj)
        {
            obj.Position = ParticleOrigin.GetValue();
            obj.Momentum = VectorHelper.FromLength(ParticleSpeed.GetValue(), ParticleAngle.GetValue());
            obj.LifeDuration = ParticleLifeDuration.GetValue();
            obj.FadeDuration = ParticleFadeDuration.GetValue();
        }

        /// <summary>
        /// Deactivates the particle system when the limit has been reached.
        /// </summary>
        protected virtual void OnLimitReached()
        {
            IsActive = false;
        }

        #endregion
    }
}
