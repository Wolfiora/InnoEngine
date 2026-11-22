using System;
using Inno.Core.Events;

namespace Inno.UI;

/// <summary>
/// Describes one queued document event after a context update.
/// </summary>
public sealed class UIEvent : Event
{
    /// <summary>
    /// Creates a document event with its source document and target element.
    /// </summary>
    /// <param name="type">
    /// The kind of document interaction.
    /// </param>
    /// <param name="document">
    /// The document that produced the event.
    /// </param>
    /// <param name="targetID">
    /// The target element identifier, which can be empty.
    /// </param>
    public UIEvent(
        UIEventType type,
        UIDocumentHandle document,
        string targetID
    ) {
        ArgumentNullException.ThrowIfNull(targetID);
        this.type = type;
        this.document = document;
        this.targetID = targetID;
    }

    /// <summary>
    /// Gets the kind of document interaction.
    /// </summary>
    public UIEventType type { get; }

    /// <summary>
    /// Gets the source document.
    /// </summary>
    public UIDocumentHandle document { get; }

    /// <summary>
    /// Gets the target element identifier.
    /// </summary>
    public string targetID { get; }
}

/// <summary>
/// Identifies one document event made visible to game code.
/// </summary>
public enum UIEventType
{
    /// <summary>
    /// The target was clicked.
    /// </summary>
    Click,
    /// <summary>
    /// The target value changed.
    /// </summary>
    Change,
    /// <summary>
    /// A form was submitted.
    /// </summary>
    Submit,
    /// <summary>
    /// The target gained focus.
    /// </summary>
    Focus,
    /// <summary>
    /// The target lost focus.
    /// </summary>
    Blur,
    /// <summary>
    /// A pointer entered the target.
    /// </summary>
    MouseEnter,
    /// <summary>
    /// A pointer left the target.
    /// </summary>
    MouseLeave
}
