using System;
using System.Collections.Generic;
using Inno.Core.Identity;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;
/// <summary>
/// Applies ordered selection gestures through the authoritative interaction owner.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    /// <summary>
    /// Applies ordinary, additive or visible-range selection using the active keyboard modifiers.
    /// </summary>
    /// <param name="interactions">
    /// The session's authoritative interaction service.
    /// </param>
    /// <param name="target">
    /// The freshly resolved clicked target.
    /// </param>
    /// <param name="visibleTargets">
    /// Current view order; collapsed or filtered items must be omitted.
    /// </param>
    public void SelectTarget(
        EditorInteractions interactions,
        IdentityObject target,
        IReadOnlyList<IdentityObject> visibleTargets
    ) {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(interactions);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(visibleTargets);
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        bool additive = io->KeyCtrl != 0 || io->KeySuper != 0;
        if (io->KeyShift != 0)
            interactions.SelectRange(visibleTargets, target, additive);
        else if (additive)
            interactions.ToggleSelection(target);
        else
            interactions.SetSelection(target);
    }
}
