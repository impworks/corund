using Corund.Sprites;
using Corund.Visuals;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Properties
{
    /// <summary>
    /// The list of known property descriptors.
    /// </summary>
    public static class AnimatedProperty
    {
        #region DynamicObject

        /// <summary>
        /// Descriptor for DynamicObject.Position.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, Vector2> Position = new PropertyDescriptor<DynamicObject, Vector2>(
            x => x.Position,
            (x, v) => x.Position = v,
            nameof(DynamicObject.Position)
        );

        /// <summary>
        /// Descriptor for DynamicObject.ScaleVector.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, Vector2> ScaleVector = new PropertyDescriptor<DynamicObject, Vector2>(
            x => x.ScaleVector,
            (x, v) => x.ScaleVector = v,
            nameof(DynamicObject.ScaleVector)
        );

        /// <summary>
        /// Descriptor for DynamicObject.Scale.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, float> Scale = new PropertyDescriptor<DynamicObject, float>(
            x => x.Scale,
            (x, v) => x.Scale = v,
            nameof(DynamicObject.Scale)
        );

        /// <summary>
        /// Descriptor for DynamicObject.Angle.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, float> Angle = new PropertyDescriptor<DynamicObject, float>(
            x => x.Angle,
            (x, v) => x.Angle = v,
            nameof(DynamicObject.Angle)
        );

        /// <summary>
        /// Descriptor for DynamicObject.Opacity.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, float> Opacity = new PropertyDescriptor<DynamicObject, float>(
            x => x.Opacity,
            (x, v) => x.Opacity = v,
            nameof(DynamicObject.Opacity)
        );

        /// <summary>
        /// Descriptor for DynamicObject.Tint.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, Color> Tint = new PropertyDescriptor<DynamicObject, Color>(
            x => x.Tint,
            (x, v) => x.Tint = v,
            nameof(DynamicObject.Tint)
        );

        /// <summary>
        /// Descriptor for DynamicObject.Rotation.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, float> Rotation = new PropertyDescriptor<DynamicObject, float>(
            x => x.Rotation,
            (x, v) => x.Rotation = v,
            nameof(DynamicObject.Rotation)
        );

        /// <summary>
        /// Descriptor for DynamicObject.Momentum.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, Vector2> Momentum = new PropertyDescriptor<DynamicObject, Vector2>(
            x => x.Momentum,
            (x, v) => x.Momentum = v,
            nameof(DynamicObject.Momentum)
        );

        /// <summary>
        /// Descriptor for DynamicObject.Acceleration.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, float> Acceleration = new PropertyDescriptor<DynamicObject, float>(
            x => x.Acceleration,
            (x, v) => x.Acceleration = v,
            nameof(DynamicObject.Acceleration)
        );

        /// <summary>
        /// Descriptor for DynamicObject.Speed.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, float> Speed = new PropertyDescriptor<DynamicObject, float>(
            x => x.Speed,
            (x, v) => x.Speed = v,
            nameof(DynamicObject.Speed)
        );

        /// <summary>
        /// Descriptor for DynamicObject.Direction.
        /// </summary>
        public static IPropertyDescriptor<DynamicObject, float> Direction = new PropertyDescriptor<DynamicObject, float>(
            x => x.Direction,
            (x, v) => x.Direction = v,
            nameof(DynamicObject.Direction)
        );

        #endregion

        #region SpriteObject

        /// <summary>
        /// Descriptor for SpriteObject.CurrentSprite.HotSpot.
        /// </summary>
        public static IPropertyDescriptor<SpriteObject, Vector2> SpriteHotSpot = new PropertyDescriptor<SpriteObject, Vector2>(
            x => x.CurrentSprite.HotSpot,
            (x, v) => x.CurrentSprite.HotSpot = v,
            nameof(SpriteObject.CurrentSprite) + "." + nameof(SpriteBase.HotSpot)
        );

        /// <summary>
        /// Descriptor for SpriteObject.CurrentSprite.FrameDelay (for animated sprites).
        /// </summary>
        public static IPropertyDescriptor<SpriteObject, float> AnimatedSpriteFrameDelay = new PropertyDescriptor<SpriteObject, float>(
            x => ((AnimatedSprite)x.CurrentSprite).FrameDelay,
            (x, v) => ((AnimatedSprite)x.CurrentSprite).FrameDelay = v,
            nameof(SpriteObject.CurrentSprite) + "." + nameof(AnimatedSprite.FrameDelay)
        );

        /// <summary>
        /// Descriptor for SpriteObject.CurrentSprite.EffectiveSize (for tiled sprites).
        /// </summary>
        public static IPropertyDescriptor<SpriteObject, Vector2> TiledSpriteEffectiveSize = new PropertyDescriptor<SpriteObject, Vector2>(
            x => ((ITiledSprite)x.CurrentSprite).EffectiveSize,
            (x, v) => ((ITiledSprite)x.CurrentSprite).EffectiveSize = v,
            nameof(SpriteObject.CurrentSprite) + "." + nameof(ITiledSprite.EffectiveSize)
        );

        /// <summary>
        /// Descriptor for SpriteObject.CurrentSprite.TextureOffset (for tiled sprites).
        /// </summary>
        public static IPropertyDescriptor<SpriteObject, Vector2> TiledSpriteTextureOffset = new PropertyDescriptor<SpriteObject, Vector2>(
            x => ((ITiledSprite)x.CurrentSprite).TextureOffset,
            (x, v) => ((ITiledSprite)x.CurrentSprite).TextureOffset = v,
            nameof(SpriteObject.CurrentSprite) + "." + nameof(ITiledSprite.TextureOffset)
        );

        #endregion
    }
}
