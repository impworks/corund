using System;
using System.Collections.Generic;
using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Visuals
{
	/// <summary>
	/// The quantity indicator. Can be used to display number of lives, powerups, etc.
	/// </summary>
	public class CounterObject : ObjectGroup
	{
		#region Constructor

		public CounterObject(Func<InteractiveObject> generator, Func<int> binding)
		{
			_Generator = generator;
			_Binding = binding;
			_Objects = new List<InteractiveObject>();

			Orientation = CounterOrientation.Right;
		}

		#endregion

		#region Fields

		/// <summary>
		/// The space between objects.
		/// </summary>
		public int Spacing
		{
			get { return _Spacing; }
			set
			{
				_Spacing = value;
				reset();
			}
		}
		private int _Spacing;

		/// <summary>
		/// The function that creates indicator objects.
		/// </summary>
		private readonly Func<InteractiveObject> _Generator;

		/// <summary>
		/// The function that returns the value to display.
		/// </summary>
		private readonly Func<int> _Binding;

		/// <summary>
		/// The direction in which the counter grows.
		/// </summary>
		public CounterOrientation Orientation
		{
			get { return _Orientation; }
			set
			{
				_Orientation = value;
				reset();
			}
		}

		private CounterOrientation _Orientation;

		/// <summary>
		/// The currently existing objects.
		/// The Counter's own Objects property contains all objects to be rendered, including the ones under FadeOut effect.
		/// The _Objects field contains only active objects.
		/// </summary>
		private readonly List<InteractiveObject> _Objects;

		/// <summary>
		/// The position of the new object to add.
		/// </summary>
		private Vector2 _LastPosition;

		#endregion

		#region Methods

		public override void Update()
		{
			var value = _Binding();
			if (value > _Objects.Count)
			{
				while (value > _Objects.Count)
				{
					var newObj = _Generator();
					newObj.Position = _LastPosition;
					updateLastPosition(newObj.GetBoundingBox(), true);

					_Objects.Add(newObj);
					Add(newObj);
				}
			}
			else if (value < _Objects.Count)
			{
				// remove extra objects
				for(var idx = value; idx < _Objects.Count; idx++)
				{
					var obj = _Objects[idx];
					updateLastPosition(obj.GetBoundingBox(), false);
					obj.RemoveSelf();
					_Objects.RemoveAt(idx);
				}
			}

			base.Update();
		}

		/// <summary>
		/// Update the cached position at which to insert the new object.
		/// </summary>
		/// <param name="box">The object's bounding box.</param>
		/// <param name="isAdding">Are we adding or removing the object?</param>
		private void updateLastPosition(Rectangle box, bool isAdding)
		{
			var direction = isAdding ? 1 : -1;
			switch(Orientation)
			{
				case CounterOrientation.Right:	_LastPosition.X += (box.Width + Spacing)*direction; break;
				case CounterOrientation.Left:	_LastPosition.X -= (box.Width + Spacing)*direction; break;
				case CounterOrientation.Down:	_LastPosition.Y += (box.Height + Spacing)*direction; break;
				case CounterOrientation.Up:		_LastPosition.Y -= (box.Height + Spacing)*direction; break;
			}
		}

		/// <summary>
		/// Reposition all objects according to new settings.
		/// </summary>
		private void reset()
		{
			_LastPosition = new Vector2();
			foreach (var curr in _Objects)
			{
				curr.Position = _LastPosition;
				var box = curr.GetBoundingBox();
				switch (Orientation)
				{
					case CounterOrientation.Right:	_LastPosition.X += box.Width + Spacing; break;
					case CounterOrientation.Left:	_LastPosition.X -= box.Width + Spacing; break;
					case CounterOrientation.Down:	_LastPosition.Y += box.Height + Spacing; break;
					case CounterOrientation.Up:		_LastPosition.Y -= box.Height + Spacing; break;
				}
			}
		}

		#endregion
	}

	/// <summary>
	/// The direction in which the counter grows.
	/// </summary>
	public enum CounterOrientation
	{
		Up,
		Down,
		Left,
		Right
	}
}
