using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Editor.Interactions;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Scene;
using Inno.Scene;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Inspector;
/// <summary>
/// Draws the project-defined tag picker used by GameObject target headers.
/// </summary>
internal sealed class GameObjectTagSelector
{
    private readonly SceneProjectSettingsModule m_settings;
    private readonly SceneEdits m_edits;
    /// <summary>
    /// Creates a tag selector backed by the effective Project Settings snapshot.
    /// </summary>
    /// <param name="settings">
    /// The project Scene-classification settings module.
    /// </param>
    /// <param name="edits">
    /// The Scene editing service used to record GameObject tag changes.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name = "settings"/> or <paramref name="edits"/> is <see langword="null"/>.
    /// </exception>
    internal GameObjectTagSelector(
        SceneProjectSettingsModule settings,
        SceneEdits edits
    ) {
        m_settings = settings ?? throw new ArgumentNullException(nameof(settings));
        m_edits = edits ?? throw new ArgumentNullException(nameof(edits));
    }

    /// <summary>
    /// Draws a compact tag selector for one live GameObject.
    /// </summary>
    /// <param name="targets">
    /// The GameObject whose tag should be displayed and edited.
    /// </param>
    /// <param name="interactions">
    /// The authoritative shared History owner.
    /// </param>
    /// <param name="width">
    /// The width reserved for the selector.
    /// </param>
    internal void Draw(
        IReadOnlyList<GameObject> targets,
        EditorInteractions interactions,
        float width
    ) {
        ArgumentNullException.ThrowIfNull(targets);
        if (targets.Count == 0)
            return;
        GameObject target = targets[0];
        bool mixed = targets.Any(value => value.tag != target.tag);
        IReadOnlyList<string> tags = m_settings.tagCatalog.GetTags();
        string preview = m_settings.tagCatalog.IsDefined(target.tag) ? target.tag : $"{target.tag} (Undefined)";
        if (mixed)
            preview = "Mixed values";
        EditorUI.current.widgets.LabelChip("Tag", InspectorTargetStyle.tagLabel);
        Ui.SameLine(0f, 0f);
        float selectorWidth = MathF.Max(1f, width);
        if (!EditorUI.current.widgets.BeginMenuSelector($"game_object_tag_{target.identity.persistentId:N}", preview, selectorWidth, selectorWidth))
        {
            return;
        }

        try
        {
            for (int i = 0; i < tags.Count; i++)
            {
                string tag = tags[i];
                if (Ui.Selectable(tag, string.Equals(target.tag, tag, StringComparison.Ordinal)))
                {
                    using EditorHistoryTransaction transaction = interactions.history.BeginTransaction("Set Selection Tag");
                    foreach (GameObject selected in targets)
                        m_edits.SetGameObjectTag(selected, tag);
                    transaction.Commit();
                }
            }
        }
        finally
        {
            EditorUI.current.widgets.EndMenuSelector();
        }
    }

    /// <summary>
    /// Determines whether the current Project Settings catalog defines a tag.
    /// </summary>
    /// <param name="tag">
    /// The tag value to resolve.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the tag is currently defined.
    /// </returns>
    internal bool IsTagDefined(string tag) => m_settings.tagCatalog.IsDefined(tag);
}
