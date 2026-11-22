namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Carries frame-local list navigation intents over visible indices without owning objects or selection state.
/// </summary>
public readonly struct ListNavigationResult
{
    internal ListNavigationResult(
        int focusedIndex,
        bool focusChanged,
        bool selectionRequested,
        bool extendRange,
        bool additive,
        bool toggle,
        bool activate,
        bool cancel
    ) {
        this.focusedIndex = focusedIndex;
        this.focusChanged = focusChanged;
        this.selectionRequested = selectionRequested;
        this.extendRange = extendRange;
        this.additive = additive;
        this.toggle = toggle;
        this.activate = activate;
        this.cancel = cancel;
    }

    /// <summary>
    /// Gets the resulting visible focus index, or minus one for no focus.
    /// </summary>
    public int focusedIndex { get; }

    /// <summary>
    /// Gets whether the visible focus index changed.
    /// </summary>
    public bool focusChanged { get; }

    /// <summary>
    /// Gets whether the caller should apply a domain selection operation.
    /// </summary>
    public bool selectionRequested { get; }

    /// <summary>
    /// Gets whether the selection should extend from its existing anchor.
    /// </summary>
    public bool extendRange { get; }

    /// <summary>
    /// Gets whether the product's primary modifier requests preserving other selected entries.
    /// </summary>
    public bool additive { get; }

    /// <summary>
    /// Gets whether Space with the primary modifier requests toggling the focused entry.
    /// </summary>
    public bool toggle { get; }

    /// <summary>
    /// Gets whether Enter requests activation of the focused entry.
    /// </summary>
    public bool activate { get; }

    /// <summary>
    /// Gets whether Escape requests canceling the current list workflow.
    /// </summary>
    public bool cancel { get; }
}
