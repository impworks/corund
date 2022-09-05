using Corund.Engine;
using Corund.Engine.Prompts;
using Corund.Tools;
using Microsoft.Xna.Framework.Input;

namespace Corund.Platform.iOS.Tools;

/// <summary>
/// Platform-specific prompt manager for Android.
/// </summary>
public class iOSPromptManager : IPromptManager
{
    #region Fields

    /// <summary>
    /// Pause mode to be used during prompts.
    /// </summary>
    private PauseMode _pauseMode = PauseMode.None;

    #endregion

    #region IPromptManager implementation

    public void Update()
    {
        GameEngine.Current.PauseMode = _pauseMode;
    }

    public void TextInput(TextInputPromptOptions opts)
    {
        _pauseMode = opts.PauseMode;

        var task = KeyboardInput.Show(
            opts.Header,
            opts.Description,
            opts.DefaultValue ?? ""
        );

        task.ContinueWith(
            t =>
            {
                var result = t.Result;
                if (string.IsNullOrEmpty(result))
                    opts.OnCancel?.Invoke();
                else
                    opts.OnConfirm?.Invoke(result);

                _pauseMode = PauseMode.None;
            }
        );
    }

    public void MessageBox(MessageBoxPromptOptions opts)
    {
        _pauseMode = opts.PauseMode;

        var task = Microsoft.Xna.Framework.Input.MessageBox.Show(
            opts.Header,
            opts.Description,
            opts.Options
        );

        task.ContinueWith(
            t =>
            {
                var result = t.Result;
                if (result == null)
                    opts.OnCancel?.Invoke();
                else
                    opts.OnConfirm?.Invoke(result.Value);

                _pauseMode = PauseMode.None;
            }
        );
    }

    #endregion
}