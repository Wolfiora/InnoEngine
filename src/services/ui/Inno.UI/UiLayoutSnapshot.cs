using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.UI;

/// <summary>
/// Holds an immutable batch of element measurements from one document and layout revision.
/// </summary>
/// <remarks>
/// This value is historical data, not a document lease. After mutation, capture a new snapshot before
/// using geometry for an action. Closing the context or document does not make the captured values live.
/// </remarks>
public sealed class UiLayoutSnapshot
{
    /// <summary>
    /// Freezes the measurements produced by one owner-thread layout capture.
    /// </summary>
    /// <param name="context">
    /// The context that produced the measurement.
    /// </param>
    /// <param name="document">
    /// The owning document, validated by the backend during capture.
    /// </param>
    /// <param name="revision">
    /// The nonzero context-local layout revision; it is not a persistent format version.
    /// </param>
    /// <param name="elements">
    /// Ordered measurements corresponding to the request, copied into this snapshot.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A handle is unassigned or the revision is zero.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The measurements are null.
    /// </exception>
    public UiLayoutSnapshot(
        UiContextHandle context,
        UiDocumentHandle document,
        ulong revision,
        IEnumerable<UiElementLayout> elements
    ) {
        if (!context.isValid || !document.isValid || revision == 0)
            throw new ArgumentException("Layout snapshots require assigned owners and a nonzero revision.");
        ArgumentNullException.ThrowIfNull(elements);
        this.context = context;
        this.document = document;
        this.revision = revision;
        this.elements = Array.AsReadOnly(elements.ToArray());
    }

    /// <summary>
    /// Gets the context identity that produced these measurements.
    /// </summary>
    public UiContextHandle context { get; }
    /// <summary>
    /// Gets the document identity that produced these measurements.
    /// </summary>
    public UiDocumentHandle document { get; }
    /// <summary>
    /// Gets the context-local revision shared by all measurements in this batch.
    /// </summary>
    public ulong revision { get; }
    /// <summary>
    /// Gets the immutable measurements in request order, including missing and hidden entries.
    /// </summary>
    public IReadOnlyList<UiElementLayout> elements { get; }
}
