using Corund.Tools;
using System;

namespace Corund.Engine.Prompts;

/// <summary>
/// Options to display an input prompt.
/// </summary>
public class TextInputPromptOptions
{
    /// <summary>
    /// Big header to display at the top of the input.
    /// </summary>
    public string Header { get; init; }

    /// <summary>
    /// Smaller explanation text.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Default value to display in the input box.
    /// Defaults to none.
    /// </summary>
    public string DefaultValue { get; init; }

    /// <summary>
    /// Action to execute when the user confirmed the prompt.
    /// Value is passed as an argument.
    /// </summary>
    public Action<string> OnConfirm { get; init; }

    /// <summary>
    /// Action to execute when the user has cancelled the prompt.
    /// </summary>
    public Action OnCancel { get; init; }

    /// <summary>
    /// Pause mode to apply to the background game while the prompt is active.
    /// Defaults to none.
    /// </summary>
    public PauseMode PauseMode { get; init; } = PauseMode.None;
}