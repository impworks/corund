using System.Collections;
using System.Collections.Generic;
using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

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
        /// Gets or sets a particular item in the list.
        /// </summary>
        /// <param name="id">Item's ID.</param>
        public virtual ObjectBase this[int id]
        {
            get => Children[id];
            set
            {
                Detach(Children[id]);
                Attach(value);
                Children[id] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add an object to the visual list.
        /// </summary>
        /// <param name="obj">Object to insert.</param>
        /// <param name="toTop">Whether to put object on top or on bottom.</param>
        public virtual T Add<T>(T obj, bool toTop = true)
            where T: ObjectBase
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
        public void AddRange(params ObjectBase[] children)
        {
            AddRange(true, children);
        }

        /// <summary>
        /// Adds many objects to the visual list.
        /// </summary>
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
        public virtual void Remove(ObjectBase obj)
        {
            Detach(obj);
            Children.Remove(obj);
        }

        /// <summary>
        /// Remove an object at given position from the list.
        /// </summary>
        public virtual void RemoveAt(int idx)
        {
            Detach(Children[idx]);
            Children.RemoveAt(idx);
        }

        /// <summary>
        /// Remove all the children from the list.
        /// </summary>
        public virtual void Clear()
        {
            for (var idx = 0; idx < Children.Count; idx++)
                Detach(Children[idx]);

            Children.Clear();
        }

        /// <summary>
        /// Inserts the object at the specified position.
        /// </summary>
        public virtual T InsertAt<T>(int idx, T obj)
            where T: ObjectBase
        {
            if (obj == null || Children.Contains(obj))
                return obj;

            Attach(obj);
            Children.Insert(idx, obj);

            return obj;
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

        #region Private helpers

        /// <summary>
        /// Attaches the object to this group.
        /// </summary>
        protected void Attach(ObjectBase obj)
        {
            (obj.Parent as ObjectGroup)?.Children.Remove(obj);
            obj.Parent = this;
        }

        /// <summary>
        /// Detaches the object from this group.
        /// </summary>
        protected void Detach(ObjectBase obj)
        {
            obj.Parent = null;
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
