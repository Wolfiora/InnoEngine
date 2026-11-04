using System;

namespace Inno.UI;

/// <summary>
/// Identifies the focused element without borrowing a backend object.
/// </summary>
/// <remarks>
/// An empty element ID denotes an anonymous focused element. The value is a snapshot, not a lifetime lease.
/// </remarks>
public readonly record struct UiFocusTarget
{
    /// <summary>
    /// Creates a document-scoped focus snapshot.
    /// </summary>
    /// <param name="document">
    /// The live owning document.
    /// </param>
    /// <param name="elementId">
    /// The element's authored ID, or empty for an anonymous element.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The document handle is unassigned.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The element ID is null.
    /// </exception>
    public UiFocusTarget(
        UiDocumentHandle document,
        string elementId
    ) {
        if (!document.isValid)
            throw new ArgumentException("Focus requires an assigned document.", nameof(document));
        ArgumentNullException.ThrowIfNull(elementId);
        this.document = document;
        this.elementId = elementId;
    }

    /// <summary>
    /// Gets the owning document identity; it must be revalidated before future operations.
    /// </summary>
    public UiDocumentHandle document { get; }

    /// <summary>
    /// Gets the authored element ID, or empty for an anonymous element.
    /// </summary>
    public string elementId { get; }
}
