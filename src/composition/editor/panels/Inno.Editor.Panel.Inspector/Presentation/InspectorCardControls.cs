using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Numerics;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Scene;
using Inno.Scene.Components;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Inspector;
internal sealed class InspectorCardControls
{
    private const string C_COMPONENT_PAYLOAD = "INNO_INSPECTOR_COMPONENT";
    private const string C_SYSTEM_PAYLOAD = "INNO_INSPECTOR_SYSTEM";
    private readonly Logger m_log;
    internal InspectorCardControls(LogRouter logs)
    {
        ArgumentNullException.ThrowIfNull(logs);
        m_log = logs.CreateLogger<InspectorCardControls>();
    }

    internal float GetWidth(bool canRemove) => EditorUI.current.widgets.GetCompactClickableTextSize().X * (canRemove ? 2f : 1f);
    internal void DrawComponent(
        SceneEdits edits,
        GameComponent component,
        bool canRemove,
        Action requestRemove
    ) {
        ArgumentNullException.ThrowIfNull(edits);
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(requestRemove);
        Guid componentId = component.identity.persistentId;
        if (EditorUI.current.widgets.ClickableText($"reset_Component_{componentId:N}", ImGuiIcon.ArrowRotateLeft, "Reset Component"))
        {
            TryEdit(() => edits.ResetComponent(component), "Component reset");
        }

        if (!canRemove)
            return;
        Ui.SameLine(0f, 0f);
        if (EditorUI.current.widgets.ClickableText($"remove_Component_{componentId:N}", ImGuiIcon.Xmark, "Remove Component"))
        {
            requestRemove();
        }
    }

    internal void DrawSystem(
        SceneEdits edits,
        GameScene owner,
        GameSystem system,
        Action requestRemove
    ) {
        ArgumentNullException.ThrowIfNull(edits);
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(system);
        ArgumentNullException.ThrowIfNull(requestRemove);
        Guid systemId = system.identity.persistentId;
        if (EditorUI.current.widgets.ClickableText($"reset_System_{systemId:N}", ImGuiIcon.ArrowRotateLeft, "Reset System"))
        {
            TryEdit(() => edits.ResetSystem(system), "System reset");
        }

        Ui.SameLine(0f, 0f);
        if (EditorUI.current.widgets.ClickableText($"remove_System_{systemId:N}", ImGuiIcon.Xmark, "Remove System"))
        {
            requestRemove();
        }
    }

    internal void DrawComponentDragSource(
        GameComponent component,
        string title,
        bool dimmed
    ) {
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(title);
        if (component.identity.runtimeIdentity is not RuntimeIdentity identity)
            return;
        _ = EditorUI.current.widgets.DragDropSource(C_COMPONENT_PAYLOAD, identity, () => DrawDragPreview(title, dimmed), allowHoldToOpenOthers: false);
    }

    internal void DrawSystemDragSource(
        GameSystem system,
        string title,
        bool dimmed
    ) {
        ArgumentNullException.ThrowIfNull(system);
        ArgumentNullException.ThrowIfNull(title);
        if (system.identity.runtimeIdentity is not RuntimeIdentity identity)
            return;
        _ = EditorUI.current.widgets.DragDropSource(C_SYSTEM_PAYLOAD, identity, () => DrawDragPreview(title, dimmed), allowHoldToOpenOthers: false);
    }

    internal void DrawComponentDropTarget(
        EditorInteractions interactions,
        SceneEdits edits,
        GameComponent target,
        int targetIndex,
        Vector2 cardMinimum,
        Vector2 cardMaximum
    ) {
        ArgumentNullException.ThrowIfNull(interactions);
        ArgumentNullException.ThrowIfNull(edits);
        ArgumentNullException.ThrowIfNull(target);
        DrawDropTarget(C_COMPONENT_PAYLOAD, $"component_{target.identity.persistentId:N}", interactions, cardMinimum, cardMaximum, source => source is GameComponent component
            && ReferenceEquals(
            component.gameObject,
            target.gameObject
        )
            && !ReferenceEquals(
            component,
            target
        ), (
            source,
            insertAfter
        ) =>
        {
            var component = (GameComponent)source;
            int sourceIndex = component.gameObject.GetComponentIndex(component);
            int insertionBoundary = targetIndex + (insertAfter ? 1 : 0);
            int destinationIndex = insertionBoundary > sourceIndex ? insertionBoundary - 1 : insertionBoundary;
            edits.SetComponentIndex(component, destinationIndex);
        }, "Component reorder");
    }

    internal void DrawSystemDropTarget(
        EditorInteractions interactions,
        SceneEdits edits,
        GameScene scene,
        GameSystem target,
        int targetIndex,
        Vector2 cardMinimum,
        Vector2 cardMaximum
    ) {
        ArgumentNullException.ThrowIfNull(interactions);
        ArgumentNullException.ThrowIfNull(edits);
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(target);
        DrawDropTarget(C_SYSTEM_PAYLOAD, $"system_{target.identity.persistentId:N}", interactions, cardMinimum, cardMaximum, source => source is GameSystem system
            && !ReferenceEquals(
            system,
            target
        )
            && IsOwnedBy(
            scene,
            system
        ), (
            source,
            insertAfter
        ) =>
        {
            var system = (GameSystem)source;
            int sourceIndex = scene.GetSystemIndex(system);
            int insertionBoundary = targetIndex + (insertAfter ? 1 : 0);
            int destinationIndex = insertionBoundary > sourceIndex ? insertionBoundary - 1 : insertionBoundary;
            edits.SetSystemIndex(scene, system, destinationIndex);
        }, "System reorder");
    }

    private void DrawDropTarget(
        string payloadType,
        string id,
        EditorInteractions interactions,
        Vector2 cardMinimum,
        Vector2 cardMaximum,
        Func<IdentityObject, bool> accepts,
        Action<IdentityObject, bool> move,
        string operation
    ) {
        if (!Inno.Editor.ImGui.ImGui.HasDragPayload(payloadType) || cardMaximum.X <= cardMinimum.X || cardMaximum.Y <= cardMinimum.Y)
        {
            return;
        }

        uint targetId = Ui.GetControlId($"##inspector_card_drop_{id}");
        bool delivered = EditorUI.current.widgets.DragDropTarget(
            payloadType,
            cardMinimum,
            cardMaximum,
            targetId,
            out RuntimeIdentity identity,
            out bool isPreviewing,
            drawDefaultHighlight: false
        );
        if (!isPreviewing && !delivered)
            return;
        if (!interactions.TryResolveIdentity(identity, out IdentityObject? source) || source is null || !accepts(source))
        {
            return;
        }

        bool insertAfter = Ui.GetMousePos().Y >= (cardMinimum.Y + cardMaximum.Y) * 0.5f;
        float markerY = insertAfter ? cardMaximum.Y : cardMinimum.Y;
        if (isPreviewing)
            EditorUI.current.widgets.InsertionLine(cardMinimum.X, cardMaximum.X, markerY);
        if (delivered)
            TryEdit(() => move(source, insertAfter), operation);
    }

    private static void DrawDragPreview(
        string title,
        bool dimmed
    ) {
        Vector2 origin = Ui.GetCursorScreenPosition();
        Vector2 padding = EditorUI.current.widgets.style.cardHeaderPadding;
        Vector2 gripSize = Ui.MeasureText(ImGuiIcon.GripVertical);
        Vector2 titleSize = Ui.MeasureText(title);
        Vector2 size = new(
            padding.X * 2f + gripSize.X + EditorUI.current.widgets.style.headerControlSpacing + titleSize.X,
            MathF.Max(gripSize.Y, titleSize.Y) + padding.Y * 2f
        );
        Ui.DrawFilledRectangle(origin, origin + size, Ui.ToPackedColor(EditorPalette.cardHeader), EditorUI.current.widgets.style.frameRounding);
        uint color = Ui.ToPackedColor(dimmed ? EditorPalette.cardDisabledText : EditorPalette.text);
        Vector2 textOrigin = origin + padding;
        Ui.DrawText(textOrigin, color, ImGuiIcon.GripVertical);
        Ui.DrawText(new Vector2(textOrigin.X + gripSize.X + EditorUI.current.widgets.style.headerControlSpacing, textOrigin.Y), color, title);
        Ui.Dummy(size);
    }

    private void TryEdit(
        Action edit,
        string operation
    ) {
        try
        {
            edit();
        }
        catch (InvalidOperationException exception)
        {
            m_log.Write(LogLevel.Warn, "{0} was rejected: {1}", [operation, exception.Message]);
        }
    }

    private static bool IsOwnedBy(
        GameScene scene,
        GameSystem system
    ) {
        try
        {
            _ = scene.GetSystemIndex(system);
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
}
