using System.Collections;
using System.Collections.Generic;
using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals
{
    /// <summary>
    /// A meta-object that can contain any number of child objects.
    /// </summary>
    public class ObjectGroup: DynamicObject, IEnumerable<ObjectBase>
    {
        #region Constructors

        public ObjectGroup()
        {
            Children = new List<ObjectBase>();
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

        #region Properties

        /// <summary>
        /// List of objects managed by the group.
        /// </summary>
        public readonly List<ObjectBase> Children;

        /// <summary>
        /// The shortcut to the number of objects in the list.
        /// </summary>
        public int Count => Children.Count;

        /// <summary>
        /// The shortcut to a particular item in the list.
        /// </summary>
        /// <param name="id">Item's ID.</param>
        public virtual ObjectBase this[int id] => Children[id];

        #endregion

        #region Methods

        /// <summary>
        /// Add an object to the visual list.
        /// </summary>
        /// <param name="child">Object to insert.</param>
        /// <param name="toTop">Whether to put object on top or on bottom.</param>
        public virtual void Add(ObjectBase child, bool toTop = true)
        {
            if (child == null || Children.Contains(child))
                return;

            (child.Parent as ObjectGroup)?.Children.Remove(child);

            if (toTop)
                Children.Insert(0, child);
            else
                Children.Add(child);

            child.Parent = this;
        }

        /// <summary>
        /// Adds many objects to the visual list.
        /// </summary>
        /// <param name="children">Objects to insert.</param>
        public void AddRange(params ObjectBase[] children)
        {
            AddRange(true, children);
        }

        /// <summary>
        /// Adds many objects to the visual list.
        /// </summary>
        /// <param name="children">Objects to insert.</param>
        public void AddRange(IEnumerable<ObjectBase> children)
        {
            AddRange(true, children);
        }

        /// <summary>
        /// Adds many objects to the visual list.
        /// </summary>
        /// <param name="toTop">Whether to put objects on top or on bottom.</param>
        /// <param name="children">Objects to insert.</param>
        public void AddRange(bool toTop, IEnumerable<ObjectBase> children)
        {
            foreach(var child in children)
                Add(child, toTop);
        }

        /// <summary>
        /// Remove an object object from the list.
        /// </summary>
        public void Remove(ObjectBase obj)
        {
            Children.Remove(obj);
        }

        /// <summary>
        /// Remove an object at given position from the list.
        /// </summary>
        public virtual void RemoveAt(int idx)
        {
            Children.RemoveAt(idx);
        }

        /// <summary>
        /// Remove all the children from the list.
        /// </summary>
        public void Clear()
        {
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

            var previousPause = GameEngine.Frames.PauseMode;
            GameEngine.Frames.PauseMode |= PauseMode;

            foreach (var curr in Children)
                curr.Update();

            GameEngine.Frames.PauseMode = previousPause;
        }

        /// <summary>
        /// Redraw all the items inside the batch.
        /// </summary>
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);

            if (!IsVisible)
                return;

            // draw in reverse: bottom to top
            for (var idx = Children.Count - 1; idx >= 0; idx--)
                Children[idx].Draw(batch);
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<ObjectBase> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
