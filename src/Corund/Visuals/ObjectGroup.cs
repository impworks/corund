using System;
using System.Collections.Generic;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Visuals;

/// <summary>
/// A meta-object that can contain any number of child objects.
/// </summary>
public class ObjectGroup : ObjectGroup<ObjectBase>
{
    #region Constructors

    public ObjectGroup()
    {
    }

    public ObjectGroup(Vector2 position)
        : base(position)
    {
    }

    public ObjectGroup(float x, float y)
        : base(x, y)
    {
    }

    #endregion

    #region Methods

    /// <summary>
    /// Add an object to the visual list.
    /// </summary>
    /// <param name="obj">Object to insert.</param>
    /// <param name="toTop">Whether to put object on top or on bottom.</param>
    public virtual T Add<T>(T obj, bool toTop = true) where T : ObjectBase
    {
        return (T)base.Add(obj, toTop);
    }

    /// <summary>
    /// Inserts the object at the specified position.
    /// </summary>
    public virtual T Insert<T>(T obj, int idx) where T : ObjectBase
    {
        return (T) base.Insert(obj, idx);
    }

    #endregion
}

/// <summary>
/// A strongly-typed meta-object that can contain any number of child objects.
/// </summary>
public class ObjectGroup<TElement>: ObjectGroupBase<TElement>
    where TElement: ObjectBase
{
    #region Constructors

    public ObjectGroup()
    {
    }

    public ObjectGroup(Vector2 position)
        : this()
    {
        Position = position;
    }

    public ObjectGroup(float x, float y)
        : this()
    {
        Position = new Vector2(x, y);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Add an object to the visual list.
    /// </summary>
    /// <param name="obj">Object to insert.</param>
    /// <param name="toTop">Whether to put object on top or on bottom.</param>
    public virtual TElement Add(TElement obj, bool toTop = true)
    {
        if (obj == null || Children.Contains(obj))
            return obj;

        Attach(obj);

        if (toTop)
            Children.Insert(0, obj);
        else
            Children.Add(obj);

        return obj;
    }

    /// <summary>
    /// Adds many objects to the visual list.
    /// </summary>
    public void AddRange(params TElement[] children)
    {
        AddRange(true, children);
    }

    /// <summary>
    /// Adds many objects to the visual list.
    /// </summary>
    public void AddRange(IEnumerable<TElement> children)
    {
        AddRange(true, children);
    }

    /// <summary>
    /// Adds many objects to the visual list.
    /// </summary>
    /// <param name="toTop">Whether to put objects on top or on bottom.</param>
    /// <param name="children">Objects to insert.</param>
    public void AddRange(bool toTop, IEnumerable<TElement> children)
    {
        foreach(var child in children)
            Add(child, toTop);
    }

    /// <summary>
    /// Inserts the object at the specified position.
    /// </summary>
    public virtual TElement Insert(TElement obj, int idx)
    {
        if (obj == null || Children.Contains(obj))
            return obj;

        Attach(obj);
        Children.Insert(idx, obj);

        return obj;
    }

    /// <summary>
    /// Replaces a child of the group with another value, keeping order.
    /// </summary>
    public virtual void Replace(TElement from, TElement to)
    {
        var idx = Children.IndexOf(from);
        if (idx == -1)
            throw new ArgumentException("Object to be replaced not found");

        from.Parent = null;
        to.Parent = this;
        Children[idx] = to;
    }

    #endregion
}