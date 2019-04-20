using Corund.Engine;
using Corund.Frames;
using Corund.Tools;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

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
            Tint = Color.White;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Visual object position relative to it's parent (scene, batch, etc).
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Scale coefficient.
        /// 1 = normal size.
        /// </summary>
        public float Scale
        {
            get => ScaleVector.X;
            set => ScaleVector = new Vector2(value);
        }

        /// <summary>
        /// Scale coefficient that can be different for X and Y axis.
        /// </summary>
        public Vector2 ScaleVector;

        /// <summary>
        /// Rotation angle in radians.
        /// </summary>
        public float Angle;

        /// <summary>
        /// Tint color.
        /// Default is White (no tint).
        /// </summary>
        public Color Tint;

        /// <summary>
        /// Opacity coefficient.
        /// Default is 1 (fully opaque).
        /// </summary>
        public float Opacity
        {
            get => Tint.A/255f;
            set => Tint.A = Tint.R = Tint.G = Tint.B = (byte) MathHelper.Clamp(value*255, 0, 255);
        }

        /// <summary>
        /// Base object, to which current object is relative.
        /// </summary>
        public ObjectBase Parent;

        /// <summary>
        /// Gets or sets the flag indicating the object is to be displayed.
        /// </summary>
        public bool IsVisible;

        /// <summary>
        /// Pause mode for current object.
        /// </summary>
        public PauseMode PauseMode;

        #endregion

        #region Interface

        /// <summary>
        /// The update logic method.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Renders the current object to current render target.
        /// </summary>
        public abstract void Draw();

        #endregion

        #region Parent-child

        /// <summary>
        /// Returns the absolute position of the object.
        /// </summary>
        public TransformInfo GetTransformInfo(bool toScreen)
        {
            var position = Position;
            var scale = ScaleVector;
            var angle = Angle;

            // traverse all visual tree parents to the frame
            var curr = Parent;
            while (curr != null)
            {
                if (curr is FrameBase)
                    break;

                angle += curr.Angle;
                scale *= curr.ScaleVector;
                position = position.Rotate(curr.Angle)*curr.ScaleVector + curr.Position;

                curr = curr.Parent;
            }

            // apply frame's camera transformation
            if (toScreen)
            {
                var camera = (curr as FrameBase).Camera;
                angle += camera.Angle;
                scale *= camera.ScaleVector;
                position = position.Rotate(camera.Angle)*camera.ScaleVector - camera.Offset;
            }

            // this does not include ResolutionAdaptationMode transforms
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
            GameEngine.Defer(() =>
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

            GameEngine.Defer(() =>
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
            GameEngine.Defer(() =>
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

            GameEngine.Defer(() =>
                {
                    list.Remove(this);
                    list.Insert(0, this);
                }
            );
        }

        /// <summary>
        /// RemoveSelf the object from visual list.
        /// </summary>
        public virtual void RemoveSelf(bool immediate = false)
        {
            var list = (Parent as ObjectGroup)?.Children;
            if (list == null)
                return;

            GameEngine.Defer(() => list.Remove(this));
        }

        #endregion
    }
}
