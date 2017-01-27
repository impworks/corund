using System.Collections.Generic;
using System.Reflection;
using Corund.Engine;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours
{
    /// <summary>
    /// A utility object that handles behaviours attached to an object at runtime.
    /// </summary>
    public class BehaviourManager: List<BehaviourBase>
    {
        #region Constructor

        public BehaviourManager(DynamicObject parent)
        {
            _parent = parent;
        }

        #endregion

        #region Fields

        private readonly DynamicObject _parent;

        #endregion

        #region Update

        /// <summary>
        /// Applies all behaviours to the object.
        /// </summary>
        public void Update()
        {
            foreach (var behaviour in this)
            {
                behaviour.UpdateObjectState(_parent);

                // effect has completed
                var effect = behaviour as IEffect;
                if(effect?.Progress == 1)
                    GameEngine.InvokeDeferred(() => Remove(behaviour));
            }
        }

        #endregion

        #region List manipulation

        /// <summary>
        /// Adds the behaviour to the list.
        /// </summary>
        public new void Add(BehaviourBase behaviour)
        {
            behaviour.Bind(_parent);
            base.Add(behaviour);
        }

        /// <summary>
        /// Inserts a behaviour at the specified position.
        /// </summary>
        public new void Insert(int position, BehaviourBase behaviour)
        {
            behaviour.Bind(_parent);
            base.Insert(position, behaviour);
        }

        /// <summary>
        /// Gets the first behaviour of specified type.
        /// Only exact matches are valid, no interfaces or base classes are supported.
        /// </summary>
        public T Get<T>() where T : BehaviourBase
        {
            var targetType = typeof(T);

            foreach (var behaviour in this)
                if (behaviour.GetType() == targetType)
                    return (T)behaviour;

            return null;
        }

        /// <summary>
        /// Removes the behaviour from the list.
        /// </summary>
        public new void Remove(BehaviourBase behaviour)
        {
            behaviour.Unbind(_parent);
            base.Remove(behaviour);
        }

        /// <summary>
        /// Removes the behaviour from the list by its position.
        /// </summary>
        public new void RemoveAt(int index)
        {
            if(index >= 0 && index < Count)
                this[index].Unbind(_parent);

            base.RemoveAt(index);
        }

        /// <summary>
        /// Removes the first behaviour of specified type.
        /// </summary>
        public void Remove<T>() where T : BehaviourBase
        {
            var targetType = typeof(T);

            for (var i = 0; i < Count; i++)
            {
                var type = this[i].GetType();
                if (type == targetType)
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Removes all behaviours which are derived from the specified type.
        /// </summary>
        public void RemoveAll<T>() where T : BehaviourBase
        {
            var type = typeof(T).GetTypeInfo();
            RemoveAll(x => x.GetType().GetTypeInfo().IsAssignableFrom(type));
        }

        #endregion
    }
}
