namespace Inno.Core.Input;

/// <summary>
/// Describes one immutable input fact in acquisition order, without mutable event-dispatch state.
/// </summary>
public abstract record InputChange
{
    /// <summary>
    /// Initializes a fact defined by this assembly with its physical modifiers.
    /// </summary>
    /// <param name="modifiers">
    /// Modifier state when the fact was acquired.
    /// </param>
    private protected InputChange(KeyModifier modifiers) => this.modifiers = modifiers;

    /// <summary>
    /// Gets the physical modifiers observed at this change, not at the end of the frame.
    /// </summary>
    public KeyModifier modifiers { get; }
}
