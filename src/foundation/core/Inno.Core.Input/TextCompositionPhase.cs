namespace Inno.Core.Input;

/// <summary>
/// Distinguishes the lifetime transitions of one input-method composition.
/// </summary>
public enum TextCompositionPhase
{
    /// <summary>
    /// A new composition owns provisional text and a new operation identity.
    /// </summary>
    Started,
    /// <summary>
    /// Provisional text or its selection changed without committing content.
    /// </summary>
    Updated,
    /// <summary>
    /// The composition ended with the supplied committed text.
    /// </summary>
    Committed,
    /// <summary>
    /// The composition ended without committing its provisional text.
    /// </summary>
    Cancelled
}
