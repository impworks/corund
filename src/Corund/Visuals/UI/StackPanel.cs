using System;
using Corund.Geometry;
using Corund.Tools;
using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Visuals.UI
{
    /// <summary>
    /// A container that aligns items by a coordinate axis.
    /// </summary>
    public class StackPanel: ObjectGroup, IGeometryObject
    {
        #region Constructor

        public StackPanel(KnownDirection orientation = KnownDirection.Down)
        {
            Validate(orientation);

            _orientation = orientation;
            _lastPosition = Vector2.Zero;

            _geometry = new GeometryRect(0, 0, 0, 0);
        }

        #endregion

        #region Fields

        private readonly GeometryRect _geometry;

        private float _padding;
        private KnownDirection _orientation;
        private Vector2 _lastPosition;

        #endregion

        #region Properties

        /// <summary>
        /// Distance between neighbour elements.
        /// </summary>
        public float Padding
        {
            get => _padding;
            set
            {
                if (_padding.IsAlmost(value))
                    return;

                _padding = value;
                RefreshLayout();
            }
        }

        /// <summary>
        /// Direction of the stack's growth.
        /// </summary>
        public KnownDirection Orientation
        {
            get => _orientation;
            set
            {
                if (value == _orientation)
                    return;

                _orientation = value;
                RefreshLayout();
            }
        }

        /// <summary>
        /// Geometry for this object.
        /// </summary>
        public IGeometry Geometry => _geometry;

        #endregion

        #region Methods

        /// <summary>
        /// Recalculates the positions of all objects.
        /// </summary>
        public void RefreshLayout()
        {
            _lastPosition = Vector2.Zero;

            for (var idx = 0; idx < Children.Count; idx++)
                PlaceObject(Children[idx]);
        }

        /// <summary>
        /// Puts the object to current position.
        /// </summary>
        protected void PlaceObject(ObjectBase obj)
        {
            var iobj = obj as IGeometryObject;
            if (iobj?.Geometry == null)
                return;

            obj.Position = _lastPosition;

            var box = iobj.Geometry.GetBoundingBox(iobj.GetTransformInfo(false));
            if (Orientation == KnownDirection.Down)
                _lastPosition.Y += (box.Height + _padding);
            else if (Orientation == KnownDirection.Right)
                _lastPosition.X += (box.Width + _padding);
            else if (Orientation == KnownDirection.Up)
                _lastPosition.Y -= (box.Height + _padding);
            else if (Orientation == KnownDirection.Left)
                _lastPosition.X -= (box.Width + _padding);

            _geometry.Size = _lastPosition;
        }

        #endregion

        #region ObjectGroup overrides

        public override T Add<T>(T obj, bool toTop = true)
        {
            base.Add(obj, toTop);

            if (toTop)
                PlaceObject(obj);
            else
                RefreshLayout();

            return obj;
        }

        public override T InsertAt<T>(int idx, T elem)
        {
            base.InsertAt(idx, elem);
            RefreshLayout();
            return elem;
        }

        public override void Remove(ObjectBase obj)
        {
            var idx = Children.IndexOf(obj);
            base.Remove(obj);
            if (idx != -1)
                RefreshLayout();
        }

        public override void RemoveAt(int idx)
        {
            base.RemoveAt(idx);
            RefreshLayout();
        }

        public override void Clear()
        {
            base.Clear();
            _lastPosition = Vector2.Zero;
        }

        public override ObjectBase this[int id]
        {
            get => base[id];
            set
            {
                if (ReferenceEquals(base[id], value))
                    return;

                base[id] = value;
                RefreshLayout();
            }
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Validates the stacks' growth direction.
        /// </summary>
        private void Validate(KnownDirection orientation)
        {
            var ok = orientation == KnownDirection.Down
                     || orientation == KnownDirection.Right
                     || orientation == KnownDirection.Up
                     || orientation == KnownDirection.Left;

            if(!ok)
                throw new ArgumentOutOfRangeException(nameof(orientation), "Stack orientation may only be one of the following: Up, Down, Left, Right.");
        }

        #endregion
    }
}
