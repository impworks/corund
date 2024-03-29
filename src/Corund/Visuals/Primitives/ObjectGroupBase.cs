﻿using System;
using System.Collections;
using System.Collections.Generic;
using Corund.Engine;

namespace Corund.Visuals.Primitives;

/// <summary>
/// Base class for various object containers.
/// </summary>
public abstract class ObjectGroupBase : ObjectGroupBase<ObjectBase>
{
}

/// <summary>
/// Strogly typed base class for various object containers.
/// </summary>
public abstract class ObjectGroupBase<TElement> : DynamicObject, IObjectGroup, IEnumerable<TElement>
    where TElement : ObjectBase
{
    #region Constructors

    protected ObjectGroupBase()
    {
        Children = new List<TElement>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// List of objects managed by the group.
    /// </summary>
    public readonly List<TElement> Children;

    /// <summary>
    /// The shortcut to the number of objects in the list.
    /// </summary>
    public int Count => Children.Count;

    /// <summary>
    /// Gets or sets a particular item in the list.
    /// </summary>
    /// <param name="id">Item's ID.</param>
    public virtual TElement this[int id]
    {
        get => Children[id];
        set
        {
            Children[id].Parent = null;
            Attach(value);
            Children[id] = value;
        }
    }

    #endregion

    #region Removal

    /// <summary>
    /// Remove an object object from the list.
    /// </summary>
    public override void Remove(ObjectBase obj)
    {
        base.Remove(obj);
        Children.Remove((TElement) obj);
    }

    /// <summary>
    /// Remove an object at given position from the list.
    /// </summary>
    public virtual void RemoveAt(int idx)
    {
        Children[idx].Parent = null;
        Children.RemoveAt(idx);
    }

    /// <summary>
    /// Remove all the children from the list.
    /// </summary>
    public virtual void Clear()
    {
        for (var idx = 0; idx < Children.Count; idx++)
            Children[idx].Parent = null;

        Children.Clear();
    }

    #endregion

    #region ObjectBase overrides

    /// <summary>
    /// Update all sub-items inside the batch.
    /// </summary>
    public override void Update()
    {
        base.Update();

        var previousPause = GameEngine.Current.PauseMode;
        GameEngine.Current.PauseMode |= PauseMode;

        foreach (var curr in Children)
            curr.Update();

        GameEngine.Current.PauseMode = previousPause;
    }

    /// <summary>
    /// Redraw all the items inside the batch.
    /// </summary>
    protected override void DrawInternal()
    {
        // draw in reverse: bottom to top
        for (var idx = Children.Count - 1; idx >= 0; idx--)
            Children[idx].Draw();
    }

    #endregion

    #region IEnumerable implementation

    public IEnumerator<TElement> GetEnumerator()
    {
        return Children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}