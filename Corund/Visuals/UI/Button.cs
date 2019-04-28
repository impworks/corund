using System;
using Corund.Geometry;
using Corund.Sprites;
using Corund.Tools.Helpers;
using Corund.Tools.UI;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Visuals.UI
{
    /// <summary>
    /// Clickable button.
    /// </summary>
    public class Button : SpriteObject
    {
        #region Constants

        protected const string DISABLED_STATE = "disabled";
        protected const string PRESSED_STATE = "pressed";
        protected const string ACTIVE_STATE = "active";

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a button with a single state and default text.
        /// </summary>
        public Button(string text, SpriteBase active, SpriteBase pressed = null, SpriteBase disabled = null)
            : this(CreateText(text), active, pressed, disabled)
        {
        }

        /// <summary>
        /// Creates a button with all three states and arbitrary contents.
        /// </summary>
        public Button(ObjectBase contents, SpriteBase active, SpriteBase pressed = null, SpriteBase disabled = null)
        {
            if (active == null)
                throw new ArgumentNullException(nameof(active));

            if (contents == null)
                throw new ArgumentNullException(nameof(contents));

            Geometry = active.Geometry;

            DefineSprite(active, ACTIVE_STATE);

            if (pressed != null)
                DefineSprite(pressed, PRESSED_STATE);
            if (disabled != null)
                DefineSprite(disabled, DISABLED_STATE);

            Attach(contents);
            _contents = contents;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Contents of the button (like text).
        /// </summary>
        protected ObjectBase _contents;

        /// <summary>
        /// Saved touch.
        /// </summary>
        protected TouchLocation? _touch;

        /// <summary>
        /// Flag indicating that the button is disabled.
        /// It shows a different animation and does not fire events.
        /// </summary>
        public bool Disabled;

        /// <summary>
        /// Geometry for tap checking.
        /// Always refers to the default state.
        /// </summary>
        public override IGeometry Geometry { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Flag indicating that the user has fully pressed-and-released the button.
        /// Fires once per click.
        /// </summary>
        public bool IsClicked()
        {
            return _touch?.State == TouchLocationState.Released;
        }

        #endregion

        #region Overrides

        public override void Update()
        {
            base.Update();

            if (Disabled)
            {
                if (HasSprite(DISABLED_STATE))
                    SetSprite(DISABLED_STATE, false);

                _touch = null;
                return;
            }

            _touch = this.TryGetTouch();
            var state = _touch == null || !HasSprite(PRESSED_STATE) ? ACTIVE_STATE : PRESSED_STATE;
            SetSprite(state, false);

            _contents.Update();
        }

        public override void Draw()
        {
            base.Draw();

            _contents.Draw();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Creates a centered text object from the string.
        /// </summary>
        private static TextObject CreateText(string text)
        {
            return new TextObject(text)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        #endregion
    }
}
