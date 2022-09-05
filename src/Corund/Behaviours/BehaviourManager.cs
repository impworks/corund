using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours;

/// <summary>
/// A utility object that handles behaviours attached to an object at runtime.
/// </summary>
public class BehaviourManager: IEnumerable<BehaviourBase>
{
    #region Constructor

    public BehaviourManager(DynamicObject parent)
    {
        _parent = parent;
        _behaviours = new List<BehaviourBase>(4);
    }

    #endregion

    #region Fields

    private readonly DynamicObject _parent;
    private readonly List<BehaviourBase> _behaviours;

    #endregion

    #region Update

    /// <summary>
    /// Applies all behaviours to the object.
    /// </summary>
    public void Update()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.UpdateObjectState(_parent);

            // effect has completed
            var effect = behaviour as IEffect;
            if(effect?.Progress is float progress && progress.IsAlmost(1))
                GameEngine.Defer(() => Remove(behaviour));
        }
    }

    #endregion

    #region List manipulation

    /// <summary>
    /// Adds the behaviour to the list.
    /// </summary>
    public void Add(BehaviourBase behaviour)
    {
        behaviour.Bind(_parent);
        _behaviours.Add(behaviour);
    }

    /// <summary>
    /// Inserts a behaviour at the specified position.
    /// </summary>
    public void Insert(int position, BehaviourBase behaviour)
    {
        behaviour.Bind(_parent);
        _behaviours.Insert(position, behaviour);
    }

    /// <summary>
    /// Gets the first behaviour of specified type.
    /// Only exact matches are valid, no interfaces or base classes are supported.
    /// </summary>
    public T Get<T>() where T : BehaviourBase
    {
        var targetType = typeof(T);

        foreach (var behaviour in _behaviours)
            if (behaviour.GetType() == targetType)
                return (T)behaviour;

        return null;
    }

    /// <summary>
    /// Removes the behaviour from the list.
    /// </summary>
    public void Remove(BehaviourBase behaviour)
    {
        behaviour.Unbind(_parent);
        _behaviours.Remove(behaviour);
    }

    /// <summary>
    /// Removes the first behaviour of specified type.
    /// </summary>
    public void Remove<T>() where T : BehaviourBase
    {
        var targetType = typeof(T);
        for (var idx = 0; idx < _behaviours.Count; idx++)
        {
            if (_behaviours[idx].GetType() == targetType)
            {
                _behaviours.RemoveAt(idx);
                return;
            }
        }
    }

    /// <summary>
    /// Removes all behaviours which are derived from the specified type.
    /// </summary>
    public void RemoveAll<T>() where T : BehaviourBase
    {
        var targetType = typeof(T).GetTypeInfo();

        for (var idx = _behaviours.Count - 1; idx >= 0; idx--)
            if (_behaviours[idx].GetType().GetTypeInfo().IsAssignableFrom(targetType))
                _behaviours.RemoveAt(idx);
    }

    #endregion

    #region IEnumerable implementation

    public IEnumerator<BehaviourBase> GetEnumerator() => _behaviours.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}