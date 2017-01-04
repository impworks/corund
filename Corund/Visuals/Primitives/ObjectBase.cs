using Corund.Engine;
using Corund.Frames;
using Corund.Tools;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals.Primitives
{
    /// <summary>
    /// Base class for all renderable objects.
    /// </summary>
    public abstract class ObjectBase
    {
        #region Constructors

        protected ObjectBase()
        {
            IsVisible = true;
            ScaleVector = new Vector2(1);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Visual object position relative to it's parent (scene, batch, etc).
        /// </summary>
        public virtual Vector2 Position { get; set; }

        /// <summary>
        /// Scale coefficient.
        /// 1 = normal size.
        /// </summary>
        public float Scale
        {
            get { return ScaleVector.X; }
            set { ScaleVector = new Vector2(value); }
        }

        /// <summary>
        /// Scale coefficient that can be different for X and Y axis.
        /// </summary>
        public virtual Vector2 ScaleVector { get; set; }

        /// <summary>
        /// Rotation angle in radians.
        /// </summary>
        public virtual float Angle { get; set; }

        /// <summary>
        /// Base object, to which current object is relative.
        /// </summary>
        public ObjectBase Parent;

        /// <summary>
        /// Gets or sets the flag indicating the object is to be displayed.
        /// </summary>
        public bool IsVisible;

        #endregion

        #region Interface

        /// <summary>
        /// The update logic method.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// The update screen method.
        /// </summary>
        public abstract void Draw(SpriteBatch batch);

        #endregion

        #region Parent-child

        /// <summary>
        /// Returns the absolute position of the object.
        /// </summary>
        public TransformInfo GetAbsolutePosition()
        {
            var position = Position;
            var scale = ScaleVector;
            var angle = Angle;

            var curr = Parent;
            while (curr != null)
            {
                var frame = curr as FrameBase;
                if (frame != null)
                {
                    angle -= frame.Camera.Angle;
                    scale *= frame.Camera.ScaleVector;
                    position = position.Rotate(-frame.Camera.Angle)*frame.Camera.ScaleVector - frame.Camera.Position;
                }
                else
                {
                    angle += curr.Angle;
                    scale *= curr.ScaleVector;
                    position = position.Rotate(curr.Angle)*curr.ScaleVector + curr.Position;
                }

                curr = curr.Parent;
            }

            return new TransformInfo(position, angle, scale);
        }

        #endregion

        #region Visual layering

        /// <summary>
        /// Bring the object 1 layer down in the drawing order.
        /// </summary>
        public void BringDown()
        {
            var list = (Parent as ObjectGroup)?.Children;
            var index = list?.IndexOf(this);

            if (index == null || index == list.Count - 1)
                return;

            var position = index.Value;
            GameEngine.InvokeDeferred(() =>
                {
                    var tmp = list[position];
                    list[position] = list[position + 1];
                    list[position + 1] = tmp;
                }
            );
        }

        /// <summary>
        /// Bring the object to the bottom of current object batch.
        /// </summary>
        public void BringToBack()
        {
            var list = (Parent as ObjectGroup)?.Children;
            var index = list?.IndexOf(this);

            if (index == null || index == list.Count - 1)
                return;

            GameEngine.InvokeDeferred(() =>
                {
                    list.Remove(this);
                    list.Add(this);
                }
            );
        }

        /// <summary>
        /// Bring the object 1 layer up in the drawing order.
        /// </summary>
        public void BringUp()
        {
            var list = (Parent as ObjectGroup)?.Children;
            var index = list?.IndexOf(this);

            if (index == null || index == 0)
                return;

            var position = index.Value;
            GameEngine.InvokeDeferred(() =>
                {
                    var tmp = list[position];
                    list[position] = list[position - 1];
                    list[position - 1] = tmp;
                }
            );
        }

        /// <summary>
        /// Bring the object to the top of current object batch.
        /// </summary>
        public void BringToFront()
        {
            var list = (Parent as ObjectGroup)?.Children;
            var index = list?.IndexOf(this);

            if (index == null || index == 0)
                return;

            GameEngine.InvokeDeferred(() =>
                {
                    list.Remove(this);
                    list.Insert(0, this);
                }
            );
        }

        /// <summary>
        /// Remove the object from visual list.
        /// </summary>
        public virtual void Remove()
        {
            var list = (Parent as ObjectGroup)?.Children;
            if (list == null)
                return;

            GameEngine.InvokeDeferred(() => list.Remove(this));
        }

        #endregion
    }
}
