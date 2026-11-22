using System;
using System.Globalization;
using Inno.Core.Identity;
using Inno.Editor.Interactions;

using EditorUI = Inno.Editor.ImGui.EditorUISession;

namespace Inno.Editor.ImGui;

/// <summary>
/// Bridges managed editor drag sessions to the native ImGui payload API.
/// </summary>
public static class EditorDragDropRenderer
{
    private const string C_IDENTITY_PAYLOAD_PREFIX = "INNO_ID_";

    /// <summary>
    /// Publishes managed drag data for the most recently submitted ImGui item.
    /// </summary>
    /// <param name="interaction">
    /// The interaction area and target that produced the drag source.
    /// </param>
    /// <param name="data">
    /// The managed drag data published by the source.
    /// </param>
    /// <param name="drawPreview">
    /// An optional callback that draws the native drag preview.
    /// </param>
    /// <returns>
    /// <see langword="true"/> while the item is an active drag source.
    /// </returns>
    public static bool Source(
        EditorInteraction interaction,
        EditorDragData data,
        Action? drawPreview = null
    ) {
        ArgumentNullException.ThrowIfNull(data);
        return EditorUI.current.widgets.DragDropSource(
            GetPayloadType(data.sourceIdentity.domainID),
            () => interaction.BeginDrag(data).runtimeID,
            drawPreview
        );
    }

    /// <summary>
    /// Evaluates and, on delivery, accepts a managed drag on the most recently submitted ImGui item.
    /// </summary>
    /// <param name="interaction">
    /// The interaction area and managed drop target.
    /// </param>
    /// <param name="placement">
    /// The requested position relative to the target.
    /// </param>
    /// <returns>
    /// The native preview state together with the managed compatibility and delivery results.
    /// </returns>
    public static EditorDropWidgetResult Target(
        EditorInteraction interaction,
        EditorDropPlacement placement = EditorDropPlacement.None
    ) {
        if (!interaction.TryGetActiveDragIdentity(out RuntimeIdentity activeIdentity))
        {
            return EditorDropWidgetResult.none;
        }
        bool delivered = EditorUI.current.widgets.DragDropTarget(
            GetPayloadType(activeIdentity.domainID),
            out int runtimeID,
            out bool isPreviewing,
            drawDefaultHighlight: false
        );
        if (!isPreviewing && !delivered)
        {
            return EditorDropWidgetResult.none;
        }
        if (runtimeID <= 0)
        {
            return EditorDropWidgetResult.none;
        }
        var identity = new RuntimeIdentity(activeIdentity.domainID, runtimeID);
        EditorDropStatus status = interaction.QueryDrop(identity, placement);
        EditorDropResult result = delivered && status.canDrop ? interaction.Drop(identity, placement) : EditorDropResult.rejected;
        return new EditorDropWidgetResult(isPreviewing, status, result);
    }

    private static string GetPayloadType(IdentityDomainID domainID)
        => C_IDENTITY_PAYLOAD_PREFIX + domainID.value.ToString("X8", CultureInfo.InvariantCulture);
}
