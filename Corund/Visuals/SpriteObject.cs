using System;
using System.Collections.Generic;
using Corund.Engine;
using Corund.Geometry;
using Corund.Sprites;
using Corund.Tools;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals
{
    /// <summary>
    /// Base class for all game sprites.
    /// </summary>
    public class SpriteObject: InteractiveObject
    {
        #region Constants

        /// <summary>
        /// Sprite name to use by default.
        /// </summary>
        public const string DEFAULT_SPRITE_NAME = "default";

        #endregion

        #region Constructor

        public SpriteObject()
        {
            _sprites = new Dictionary<string, SpriteBase>();
        }

        public SpriteObject(Sprite sprite)
            : this()
        {
            DefineSprite(sprite);
        }

        public SpriteObject(string asset, Vector2? hotSpot = null)
            : this()
        {
            var tex = GameEngine.Content.Load<Texture2D>(asset);
            var hs = hotSpot ?? new Vector2(tex.Width / 2, tex.Height / 2);

            var sprite = new Sprite(tex)
            {
                HotSpot = hs,
                Geometry = new GeometryRect(-hs.X, -hs.Y, tex.Width, tex.Height)
            };

            DefineSprite(sprite);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Lookup for sprites available in current object.
        /// </summary>
        private readonly Dictionary<string, SpriteBase> _sprites;

        #endregion

        #region Properties

        /// <summary>
        /// Sprite currently displayed by the object.
        /// </summary>
        public SpriteBase CurrentSprite { get; private set; }

        /// <summary>
        /// Geometry bound to current sprite.
        /// </summary>
        public override IGeometry Geometry => CurrentSprite?.Geometry;

        #endregion

        #region ObjectBase overrides

        /// <summary>
        /// Updates the current sprite, if it is animated.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if((PauseMode & PauseMode.SpriteAnimation) != 0)
                CurrentSprite?.Update();
        }

        /// <summary>
        /// Renders the current sprite to screen.
        /// </summary>
        protected override void DrawInternal()
        {
            if (CurrentSprite == null)
                return;

            var transform = GetTransformInfo(true);
            var zOrder = GameEngine.Current.ZOrderFunction(this);
            CurrentSprite.Draw(transform, Tint, zOrder);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new sprite to list of available sprites.
        /// </summary>
        protected void DefineSprite<T>(T sprite, string name = null)
            where T : SpriteBase
        {
            if (string.IsNullOrEmpty(name))
                name = DEFAULT_SPRITE_NAME;

            _sprites[name] = sprite;
            if (name == DEFAULT_SPRITE_NAME)
                CurrentSprite = sprite;
        }

        /// <summary>
        /// Checks if the object contains a sprite.
        /// </summary>
        public bool HasSprite(string name)
        {
            return _sprites.ContainsKey(name);
        }

        /// <summary>
        /// Sets the object's current sprite to one with given name.
        /// </summary>
        public void SetSprite(string name, bool reset = true)
        {
            if(!_sprites.TryGetValue(name, out var sprite))
                throw new ArgumentException($"Sprite '{name}' is not defined.", nameof(name));

            CurrentSprite = sprite;

            if(reset)
                CurrentSprite.Reset();
        }

        #endregion
    }
}
