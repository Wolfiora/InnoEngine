using UiDrawing = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using Inno.Core.Mathematics;
using Inno.Editor.ImGui;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Scene.Components;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Inspector;
[InspectionDrawer(typeof(Transform))]
internal sealed class TransformInspectionDrawer(
    SceneEdits edits,
    EditorInteractions interactions
) : InspectionDrawer<Transform>
{
    private bool m_world;
    /// <summary>
    /// Gets the icon glyph used to represent this item in the editor.
    /// </summary>
    public override string icon => ImGuiIcon.ArrowsUpDownLeftRight;

    /// <summary>
    /// Binds a caller-visible label to the current inspection target.
    /// </summary>
    /// <param name="context">
    /// Current Inspector drawing context.
    /// </param>
    /// <param name="target">
    /// The transform being presented.
    /// </param>
    /// <returns>
    /// A fixed Transform label with no rename action.
    /// </returns>
    protected override (string name, Action<string>? setter) BindName(
        InspectionDrawContext context,
        Transform target
    ) => ("Transform", null);
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    protected override void Draw(
        InspectionDrawContext context,
        Transform target
    ) {
        if (!EditorUI.current.widgets.SectionHeader(
            m_world ? "World Space" : "Local Space",
            m_world ? "Position, rotation, and scale include the parent hierarchy. Edits are stored as local values and support Undo/Redo." : "Position, rotation, and scale are relative to the parent. Rotation is expressed in degrees.",
            () => _ = EditorUI.current.widgets.CompactCheckbox("##transform_space", ref m_world, tooltip: m_world ? "Switch to Local Space." : "Switch to World Space.")
        ))
        {
            return;
        }

        bool invertible = true;
        if (m_world && target.parent is not null)
        {
            try
            {
                _ = target.parent.worldToLocalMatrix;
            }
            catch (InvalidOperationException)
            {
                invertible = false;
            }
        }

        if (!invertible)
            EditorUI.current.widgets.HelpBox(
                "World editing requires an invertible parent transform. Edit in Local Space or restore the parent's zero scale.",
                ImGuiIcon.TriangleExclamation,
                EditorPalette.warning
            );
        Ui.BeginDisabled(!invertible);
        try
        {
            Vector3 position = m_world ? target.worldPosition : target.localPosition;
            Quaternion rotation = m_world ? target.worldRotation : target.localRotation;
            Vector3 euler = rotation.ToEulerAnglesZYXDegrees();
            Vector3 scale = m_world ? target.worldScale : target.localScale;
            DrawVector(EditPath(target, "localPosition"), "Position", "Translation in world units.", position, value => Change(target, "localPosition", () =>
            {
                if (m_world)
                    target.worldPosition = value;
                else
                    target.localPosition = value;
            }));
            DrawVector(EditPath(target, "localRotation"), "Rotation", "Euler rotation in degrees.", euler, value => Change(target, "localRotation", () =>
            {
                Quaternion next = Quaternion.FromEulerAnglesZYXDegrees(value);
                if (m_world)
                    target.worldRotation = next;
                else
                    target.localRotation = next;
            }));
            DrawVector(EditPath(target, "localScale"), "Scale", "Scale along each axis. One preserves the original size.", scale, value => Change(target, "localScale", () =>
            {
                if (m_world)
                    target.worldScale = value;
                else
                    target.localScale = value;
            }));
        }
        finally
        {
            Ui.EndDisabled();
        }
    }

    private void Change(
        Transform target,
        string property,
        Action mutation
    ) {
        string path = EditPath(target, property);
        EditorUI.current.widgets.BeginPropertyEdit(interactions.history, path, "Change Transform " + property);
        try
        {
            _ = edits.ChangeProperty(target, property, mutation, "Change Transform " + property, EditorUI.current.widgets.editMergeKey);
        }
        catch
        {
            EditorUI.current.widgets.CancelEdits();
            throw;
        }
    }

    private string EditPath(
        Transform target,
        string property
    ) => $"transform:{target.identity.persistentId:N}:{property}:{m_world}";
    private static void DrawVector(
        string path,
        string label,
        string tooltip,
        Vector3 value,
        Action<Vector3> apply
    ) {
        EditorUI.current.widgets.PropertyRow("transform." + label, label, () =>
        {
            float width = MathF.Max(1f, (Ui.GetContentRegionAvailable().X - UiDrawing.GetLayout().itemSpacing.X * 2f) / 3f);
            bool changed = EditorUI.current.widgets.AxisDragFloat(label, "X", ref value.x, width);
            Ui.SameLine(0f, -1f);
            changed |= EditorUI.current.widgets.AxisDragFloat(label, "Y", ref value.y, width);
            Ui.SameLine(0f, -1f);
            changed |= EditorUI.current.widgets.AxisDragFloat(label, "Z", ref value.z, width);
            if (changed)
                apply(value);
        }, tooltip: tooltip);
        EditorUI.current.widgets.CompletePropertyEdit(path);
    }
}
