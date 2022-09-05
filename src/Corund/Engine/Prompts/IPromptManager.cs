namespace Corund.Engine.Prompts;

public interface IPromptManager
{
    /// <summary>
    /// Updates the prompt manager state.
    /// </summary>
    void Update();

    /// <summary>
    /// Displays a text input prompt.
    /// </summary>
    void TextInput(TextInputPromptOptions opts);

    /// <summary>
    /// Displays a message box with one or more options to pick.
    /// </summary>
    void MessageBox(MessageBoxPromptOptions opts);
}