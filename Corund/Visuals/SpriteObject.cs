using System;
using System.Collections.Generic;
using Corund.Engine;
using Corund.Geometry;
using Corund.Sprites;
using Corund.Tools;
using Corund.Visuals.Primitives;

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

            var transform = GetTransformInfo();
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
            SpriteBase sprite;
            if(!_sprites.TryGetValue(name, out sprite))
                throw new ArgumentException($"Sprite '{name}' is not defined.", nameof(name));

            CurrentSprite = sprite;

            if(reset)
                CurrentSprite.Reset();
        }

        #endregion
    }
}
