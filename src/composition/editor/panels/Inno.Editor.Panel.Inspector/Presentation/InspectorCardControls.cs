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

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

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
        Guid componentID = component.identity.persistentID;
        if (EditorUI.current.widgets.ClickableText($"reset_Component_{componentID:N}", ImGuiIcon.C_ARROW_ROTATE_LEFT, "Reset Component"))
        {
            TryEdit(() => edits.ResetComponent(component), "Component reset");
        }

        if (!canRemove)
        {
            return;
        }
        Gui.SameLine(0f, 0f);
        if (EditorUI.current.widgets.ClickableText($"remove_Component_{componentID:N}", ImGuiIcon.C_XMARK, "Remove Component"))
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
        Guid systemID = system.identity.persistentID;
        if (EditorUI.current.widgets.ClickableText($"reset_System_{systemID:N}", ImGuiIcon.C_ARROW_ROTATE_LEFT, "Reset System"))
        {
            TryEdit(() => edits.ResetSystem(system), "System reset");
        }

        Gui.SameLine(0f, 0f);
        if (EditorUI.current.widgets.ClickableText($"remove_System_{systemID:N}", ImGuiIcon.C_XMARK, "Remove System"))
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
        {
            return;
        }
        _ = EditorUI.current.widgets.DragDropSource(
            C_COMPONENT_PAYLOAD,
            identity,
            () => DrawDragPreview(title, dimmed),
            allowHoldToOpenOthers: false
        );
    }

    internal void DrawSystemDragSource(
        GameSystem system,
        string title,
        bool dimmed
    ) {
        ArgumentNullException.ThrowIfNull(system);
        ArgumentNullException.ThrowIfNull(title);
        if (system.identity.runtimeIdentity is not RuntimeIdentity identity)
        {
            return;
        }
        _ = EditorUI.current.widgets.DragDropSource(
            C_SYSTEM_PAYLOAD,
            identity,
            () => DrawDragPreview(title, dimmed),
            allowHoldToOpenOthers: false
        );
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
        DrawDropTarget(
            C_COMPONENT_PAYLOAD,
            $"component_{target.identity.persistentID:N}",
            interactions,
            cardMinimum,
            cardMaximum,
            source => (
                source is GameComponent component
                && ReferenceEquals(component.gameObject, target.gameObject)
                && !ReferenceEquals(component, target)
            ),
            (
                source,
                insertAfter
            ) =>
        {
            GameComponent component = (GameComponent)source;
            int sourceIndex = component.gameObject.GetComponentIndex(component);
            int insertionBoundary = targetIndex + (insertAfter ? 1 : 0);
            int destinationIndex = insertionBoundary > sourceIndex ? insertionBoundary - 1 : insertionBoundary;
            edits.SetComponentIndex(component, destinationIndex);
        },
            "Component reorder"
        );
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
        DrawDropTarget(
            C_SYSTEM_PAYLOAD,
            $"system_{target.identity.persistentID:N}",
            interactions,
            cardMinimum,
            cardMaximum,
            source => (
                source is GameSystem system
                && !ReferenceEquals(system, target)
                && IsOwnedBy(scene, system)
            ),
            (
                source,
                insertAfter
            ) =>
        {
            GameSystem system = (GameSystem)source;
            int sourceIndex = scene.GetSystemIndex(system);
            int insertionBoundary = targetIndex + (insertAfter ? 1 : 0);
            int destinationIndex = insertionBoundary > sourceIndex ? insertionBoundary - 1 : insertionBoundary;
            edits.SetSystemIndex(scene, system, destinationIndex);
        },
            "System reorder"
        );
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
        if (!Gui.HasDragPayload(payloadType) || cardMaximum.X <= cardMinimum.X || cardMaximum.Y <= cardMinimum.Y)
        {
            return;
        }

        uint targetID = Gui.GetControlID($"##inspector_card_drop_{id}");
        bool delivered = EditorUI.current.widgets.DragDropTarget(
            payloadType,
            cardMinimum,
            cardMaximum,
            targetID,
            out RuntimeIdentity identity,
            out bool isPreviewing,
            drawDefaultHighlight: false
        );
        if (!isPreviewing && !delivered)
        {
            return;
        }
        if (!interactions.TryResolveIdentity(identity, out IdentityObject? source) || source is null || !accepts(source))
        {
            return;
        }

        bool insertAfter = Gui.GetMousePos().Y >= (cardMinimum.Y + cardMaximum.Y) * 0.5f;
        float markerY = insertAfter ? cardMaximum.Y : cardMinimum.Y;
        if (isPreviewing)
        {
            EditorUI.current.widgets.InsertionLine(cardMinimum.X, cardMaximum.X, markerY);
        }
        if (delivered)
        {
            TryEdit(() => move(source, insertAfter), operation);
        }
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

    private static void DrawDragPreview(
        string title,
        bool dimmed
    ) {
        Vector2 origin = Gui.GetCursorScreenPosition();
        Vector2 padding = EditorUI.current.widgets.style.cardHeaderPadding;
        Vector2 gripSize = Gui.MeasureText(ImGuiIcon.C_GRIP_VERTICAL);
        Vector2 titleSize = Gui.MeasureText(title);
        var size = new Vector2(
            padding.X * 2f + gripSize.X + EditorUI.current.widgets.style.headerControlSpacing + titleSize.X,
            MathF.Max(gripSize.Y, titleSize.Y) + padding.Y * 2f
        );
        Gui.DrawFilledRectangle(
            origin,
            origin + size,
            Gui.ToPackedColor(EditorPalette.cardHeader),
            EditorUI.current.widgets.style.frameRounding
        );
        uint color = Gui.ToPackedColor(dimmed ? EditorPalette.cardDisabledText : EditorPalette.text);
        Vector2 textOrigin = origin + padding;
        Gui.DrawText(textOrigin, color, ImGuiIcon.C_GRIP_VERTICAL);
        Gui.DrawText(
            new Vector2(textOrigin.X + gripSize.X + EditorUI.current.widgets.style.headerControlSpacing, textOrigin.Y),
            color,
            title
        );
        Gui.Dummy(size);
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
