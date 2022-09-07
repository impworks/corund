using Corund.Behaviours.Particles;
using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Tools.Jitter;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals.Particles;

/// <summary>
/// A group of particle objects.
/// </summary>
public abstract class ParticleSystem: ObjectGroup<ParticleObject>
{
    #region Constructor

    public ParticleSystem(int rate)
    {
        ParticleBehaviours = new ParticleBehaviourManager(this);
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

    #endregion

    #region Properties

    /// <summary>
    /// Flag indicating that the group actively generates new particles.
    /// </summary>
    public bool IsActive;

    /// <summary>
    /// Number of particles generated during the entire lifespan of the group.
    /// </summary>
    public int TotalParticleCount { get; private set; }

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
    /// Maximum number of particles before system self-destructs.
    /// </summary>
    public int? ParticleLimit;

    /// <summary>
    /// Number of particles to create per second.
    /// </summary>
    public int GenerationRate
    {
        get => (int)(1.0f/_particleDelay);
        set => _particleDelay = 1.0f/value;
    }

    /// <summary>
    /// List of modifiers to be applied to each particle.
    /// </summary>
    public readonly ParticleBehaviourManager ParticleBehaviours;

    #endregion

    #region Methods

    public override void Update()
    {
        base.Update();

        Children.RemoveAll(x => x.Age >= 1);
        ParticleBehaviours.Update();

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
    }

    /// <summary>
    /// Reinitializes the particle system.
    /// </summary>
    public void Reset()
    {
        IsActive = true;
        TotalParticleCount = 0;
        Clear();
    }
    
    #endregion

    #region Overridables

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
        obj.ElapsedTime = 0;
        obj.LifeDuration = ParticleLifeDuration.GetValue();
    }

    /// <summary>
    /// Deactivates the particle system when the limit has been reached.
    /// </summary>
    protected virtual void OnLimitReached()
    {
        IsActive = false;
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
}