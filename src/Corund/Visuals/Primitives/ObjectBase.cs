using Corund.Engine;
using Corund.Frames;
using Corund.Tools;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

namespace Corund.Visuals.Primitives;

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
    public ObjectBase Parent { get; set; }

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
    public TransformInfo GetTransformInfo(bool toScreen = false)
    {
        var position = Position;
        var scale = ScaleVector;
        var angle = Angle;

        // traverse all visual tree parents to the frame
        var curr = Parent;
        while (curr != null && curr is not FrameBase)
        {
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

    /// <summary>
    /// Returns the tint color to use for an object, considering parent colors.
    /// </summary>
    public Color GetMixedTintColor()
    {
        var coeff = 1.0f / byte.MaxValue;
        var alpha = Tint.A * coeff;
        var curr = Parent;
        while (curr != null && curr is not FrameBase)
        {
            alpha *= curr.Tint.A * coeff;
            curr = curr.Parent;
        }

        var vec = Tint.ToVector3() * alpha;
        return new Color(vec.X, vec.Y, vec.Z, alpha);
    }

    /// <summary>
    /// Attaches another object to this one as a child.
    /// </summary>
    protected T Attach<T>(T obj)
        where T: ObjectBase
    {
        if (obj != null)
        {
            if (obj.Parent is IObjectGroup group)
                group.Remove(obj);

            obj.Parent = this;
        }

        return obj;
    }

    /// <summary>
    /// Removes a child from the current object.
    /// </summary>
    public virtual void Remove(ObjectBase obj)
    {
        if(obj.Parent == this)
            obj.Parent = null;
    }

    /// <summary>
    /// Removes the object from its parent.
    /// </summary>
    public virtual void RemoveSelf(bool immediate = false)
    {
        GameEngine.Defer(() => Parent?.Remove(this));
    }

    #endregion
}