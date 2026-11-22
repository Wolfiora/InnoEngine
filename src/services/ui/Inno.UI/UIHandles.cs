namespace Inno.UI;

/// <summary>
/// Identifies one UI context owned by the active UI service generation.
/// </summary>
/// <param name="value">
/// The concrete value read or transformed by this operation.
/// </param>
public readonly record struct UIContextHandle(ulong value)
{
    /// <summary>
    /// Gets whether this handle identifies a live context candidate.
    /// </summary>
    public bool isValid => value != 0;
}

/// <summary>
/// Identifies one document owned by a UI context.
/// </summary>
/// <param name="value">
/// The concrete value read or transformed by this operation.
/// </param>
public readonly record struct UIDocumentHandle(ulong value)
{
    /// <summary>
    /// Gets whether this handle identifies a live document candidate.
    /// </summary>
    public bool isValid => value != 0;
}
