using System;
using Corund.Engine;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Fade;

/// <summary>
/// Creates an explosion object when the base object is destroyed.
/// </summary>
public class ExplosionBehaviour: IBehaviour, IFadeOutEffect
{
    #region Constructor

    /// <summary>
    /// Creates a new ExplosionBehaviour instance.
    /// </summary>
    /// <param name="explosionFactory">A callback that creates the explosion object.</param>
    /// <param name="timeout">Time for explosion object to live.</param>
    public ExplosionBehaviour(Func<ObjectBase> explosionFactory, float? timeout = null)
    {
        _factory = explosionFactory;
        _timeout = timeout;
    }

    #endregion
        
    #region Fields

    /// <summary>
    /// Explosion creator factory.
    /// </summary>
    private readonly Func<ObjectBase> _factory;

    /// <summary>
    /// Explosion timeout.
    /// </summary>
    private readonly float? _timeout;

    #endregion

    #region Properties

    /// <summary>
    /// Effect duration (it is instantaneous).
    /// </summary>
    public float Duration => 0;

    /// <summary>
    /// Effect progress.
    /// </summary>
    public float? Progress { get; private set; }

    #endregion

    #region Methods

    public void UpdateObjectState(DynamicObject obj)
    {
        // nothing here
    }

    /// <summary>
    /// Creates the explosion in frame space.
    /// </summary>
    public void ActivateFadeOut(DynamicObject obj)
    {
        Progress = 1;

        var explosion = _factory();
        explosion.Position = obj.GetTransformInfo(toScreen: false).Position;
        GameEngine.Current.Frame.Add(explosion);

        if (_timeout != null)
            GameEngine.Current.Timeline.Add(_timeout.Value, () => explosion.RemoveSelf());
    }

    #endregion
}