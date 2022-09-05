using Corund.Sprites;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Visuals.UI;

/// <summary>
/// Button that acts like a checkbox.
/// </summary>
public class CheckButton: Button
{
    #region Constructor

    /// <summary>
    /// Creates a button with a single state and default text.
    /// </summary>
    public CheckButton(string text, SpriteBase active, SpriteBase pressed = null, SpriteBase disabled = null)
        : base(CreateText(text), active, pressed, disabled)
    {
    }

    public CheckButton(ObjectBase contents, SpriteBase active, SpriteBase pressed = null, SpriteBase disabled = null)
        : base(contents, active, pressed, disabled)
    {
    }

    #endregion

    #region Fields

    private bool _isChecked;

    /// <summary>
    /// Flag indicating that the button is currently checked.
    /// </summary>
    public bool IsChecked
    {
        get => _isChecked && !IsDisabled;
        set
        {
            if (_isChecked == value || IsDisabled)
                return;

            SetSprite(value && HasSprite(PRESSED_STATE) ? PRESSED_STATE : ACTIVE_STATE, false);
        }
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Updates the button state.
    /// </summary>
    protected override void UpdateClickedState(TouchLocation? touch)
    {
        if (touch?.State == TouchLocationState.Released)
            IsChecked = !IsChecked;
    }

    #endregion
}