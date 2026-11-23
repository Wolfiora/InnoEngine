using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Identity;
using Inno.Editor.Assets;
using Inno.Editor.ImGui;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;
using Inno.Editor.Shaders;
using Inno.Rendering;
using Inno.Rendering.Assets;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using ImGuiApi = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Shaders.ImGui;

[InspectionDrawer(typeof(EditorSelectionSnapshot), priority: 100, conditional: true)]
internal sealed class MaterialSelectionDrawer(
    IInspectionIconProvider<AssetFileEntry> icons,
    EditorInteractions interactions
) : InspectionDrawer<EditorSelectionSnapshot>
{
    /// <summary>
    /// Gets the icon glyph used to represent this item in the editor.
    /// </summary>
    public override string icon => ImGuiIcon.C_FILE;

    /// <summary>
    /// Retrieves the current icon from authoritative state.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    /// <returns>
    /// The validated text representation owned by the caller.
    /// </returns>
    protected override string GetIcon(
        InspectionDrawContext context,
        EditorSelectionSnapshot target
    ) {
        Guid id = GetAssetIDs(target).FirstOrDefault();
        return id != Guid.Empty
               && context.interactions.TryGetModule<MaterialDocuments>(out MaterialDocuments? documents)
               && documents is not null
               && documents.assets.TryGetInfo(id, out AssetInfo? info)
               && info is not null
               && documents.assets.TryGetFileSystemEntry(
                   info.assetPath,
                   out AssetFileEntry entry
               ) ? icons.GetIcon(entry) : icon;
    }

    /// <summary>
    /// Binds a caller-visible label to the current inspection target.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    /// <returns>
    /// The validated (string name, actionstring? setter) that represents the completed operation.
    /// </returns>
    protected override (string name, Action<string>? setter) BindName(
        InspectionDrawContext context,
        EditorSelectionSnapshot target
    ) => (GetAssetIDs(target).Count + " Assets", null);

    /// <summary>
    /// Renders the header presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    protected override void DrawHeader(
        InspectionDrawContext context,
        EditorSelectionSnapshot target
    ) {
        if (!TryOpenDrafts(context, target, out MaterialDocuments? documents, out List<AssetDraftDocuments<MaterialAsset>.Draft> drafts))
        {
            return;
        }
        bool readOnly = drafts.Any(value => value.readOnly);
        ImGuiApi.BeginDisabled(readOnly);
        try
        {
            if (ImGuiApi.Button("Save Selected"))
            {
                EditorUISession.current.widgets.CommitEdits();
                foreach (AssetDraftDocuments<MaterialAsset>.Draft draft in drafts)
                {
                    _ = context.interactions.documents.Save(draft.documentID);
                }
            }

            ImGuiApi.SameLine(0f, -1f);
            if (ImGuiApi.Button("Revert Selected"))
            {
                EditorUISession.current.widgets.CommitEdits();
                using EditorHistoryTransaction transaction = context.interactions.history.BeginTransaction("Revert Materials");
                foreach (AssetDraftDocuments<MaterialAsset>.Draft draft in drafts)
                {
                    _ = context.interactions.documents.Revert(draft.documentID);
                }
                transaction.Commit();
            }
        }
        finally
        {
            ImGuiApi.EndDisabled();
        }
    }

    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    protected override void Draw(
        InspectionDrawContext context,
        EditorSelectionSnapshot target
    ) {
        if (!TryOpenDrafts(context, target, out MaterialDocuments? documents, out List<AssetDraftDocuments<MaterialAsset>.Draft> drafts))
        {
            EditorUI.current.widgets.Hint("Select Materials with compatible Shader interfaces to edit their common parameters.");
            return;
        }

        MaterialDocuments owner = documents!;
        Dictionary<Guid, MaterialAsset> materials = drafts.ToDictionary(value => value.id, value => owner.Read(value.id));
        bool readOnly = drafts.Any(value => value.readOnly);
        ImGuiApi.PushID("materials." + string.Join(".", GetAssetIDs(target)));
        ImGuiApi.BeginDisabled(readOnly);
        try
        {
            EditorUI.current.widgets.Hint("Save runs per document. Unsaved edits do not change Scene/Game.");
            if (readOnly)
            {
                EditorUI.current.widgets.Hint("Selection includes installed read-only content. Copy it to the project before editing.");
            }
            foreach (AssetDraftDocuments<MaterialAsset>.Draft draft in drafts.Where(value => value.error.Length != 0))
            {
                EditorUI.current.widgets.Hint(draft.path.localPath + ": " + draft.error);
            }
            var edits = new SelectionEdits(owner, materials);
            MaterialAsset first = materials.Values.First();
            bool selectionOpen = EditorUI.current.widgets.SectionHeader(
                "Material Selection",
                "Changes are recorded together. Changing Shader retains each Material's independent overrides."
            );
            if (selectionOpen)
            {
                if (materials.Values.Any(value => value.shader?.identity.persistentID != first.shader?.identity.persistentID))
                {
                    EditorUI.current.widgets.Hint("Shader · Mixed values");
                }
                context.properties.DrawValue(
                    context.editorContext,
                    target,
                    "materials.shader",
                    "Shader",
                    typeof(ShaderAsset),
                    () => first.shader,
                    value =>
                    {
                        foreach (MaterialAsset material in materials.Values)
                        {
                            material.shader = (ShaderAsset? )value;
                        }
                    },
                    edits,
                    readOnly
                );
            }

            if (first.shader is null || first.shader.isMissing || first.shader.definition is not { } firstDefinition)
            {
                if (selectionOpen)
                {
                    EditorUI.current.widgets.Hint("A selected Shader is unavailable.");
                }
                return;
            }

            ShaderTechniqueID[] commonTechniques = firstDefinition.techniques
                .Select(value => value.id)
                .Where(
                    id => materials.Values.All(
                        material => (
                            material.shader is { isMissing: false, definition: { } definition }
                            && definition.techniques.Any(technique => technique.id == id)
                        )
                    )
                )
                .ToArray();
            bool mixedTechnique = materials.Values.Any(value => value.techniqueId != first.techniqueId);
            if (selectionOpen && (commonTechniques.Length > 1 || materials.Values.Any(value => value.techniqueId.isValid)))
            {
                string current = mixedTechnique ? "Mixed Techniques" : first.techniqueId.isValid ? first.techniqueId.value : "Automatic";
                if (EditorUI.current.widgets.BeginBoundedCombo("##materials.technique", current))
                {
                    try
                    {
                        if (ImGuiApi.Selectable("Automatic", !mixedTechnique && !first.techniqueId.isValid))
                        {
                            SetTechnique(default);
                        }
                        foreach (ShaderTechniqueID technique in commonTechniques)
                        {
                            if (ImGuiApi.Selectable(technique.value, !mixedTechnique && first.techniqueId == technique))
                            {
                                SetTechnique(technique);
                            }
                        }
                    }
                    finally
                    {
                        EditorUI.current.widgets.EndBoundedCombo();
                    }
                }
            }

            foreach (ShaderKeywordDefinition keyword in selectionOpen ? firstDefinition.keywords : [])
            {
                if (!materials.Values.All(
                    material => (
                        material.shader is { isMissing: false, definition: { } definition }
                        && definition.keywords.Any(value => value.id == keyword.id && value.options.SequenceEqual(keyword.options))
                    )
                )) {
                    continue;
                }
                string Option(MaterialAsset material) => keyword.options.FirstOrDefault(material.keywords.Contains) ?? "None";
                string selected = Option(first);
                bool mixed = materials.Values.Any(material => Option(material) != selected);
                if (!EditorUI.current.widgets.BeginBoundedCombo(keyword.id, mixed ? "Mixed values" : selected))
                {
                    continue;
                }
                try
                {
                    if (ImGuiApi.Selectable("None", !mixed && selected == "None"))
                    {
                        SetOption(null);
                    }
                    foreach (string option in keyword.options)
                    {
                        if (ImGuiApi.Selectable(option, !mixed && selected == option))
                        {
                            SetOption(option);
                        }
                    }
                }
                finally
                {
                    EditorUI.current.widgets.EndBoundedCombo();
                }

                void SetOption(string? next)
                {
                    foreach (MaterialAsset material in materials.Values)
                    {
                        foreach (string option in keyword.options)
                        {
                            material.SetKeyword(option, option == next);
                        }
                    }
                    owner.ReplaceMany(materials);
                }
            }

            bool parametersOpen = EditorUI.current.widgets.SectionHeader(
                "Common Parameters",
                "Only compatible Material-owned inputs appear here. Unrelated overrides are retained per Material."
            );
            bool groupOpen = parametersOpen;
            string currentGroup = string.Empty;
            foreach (ShaderPropertyDefinition property in firstDefinition.properties)
            {
                if (property.bindingOwner != ShaderPropertyBindingOwner.Material
                    || property.bindingKind is not (ShaderPropertyBindingKind.Uniform or ShaderPropertyBindingKind.SampledTexture)
                ) {
                    continue;
                }
                List<MaterialValue> values = [];
                ShaderParameterPresentation presentation = owner.Presentation(first.shader, property.id, out string presentationError);
                if (presentationError.Length != 0)
                {
                    EditorUI.current.widgets.Hint("Parameter presentation unavailable: " + presentationError);
                }
                if (!presentation.visible)
                {
                    continue;
                }
                bool compatible = true;
                foreach (MaterialAsset material in materials.Values)
                {
                    ShaderPropertyDefinition[] declarations = material.shader is { isMissing: false } shader
                                                              ? shader.definition?.properties ?? []
                                                              : [];
                    int index = Array.FindIndex(
                        declarations,
                        value => (
                            value.id == property.id
                            && value.type == property.type
                            && value.bindingKind == property.bindingKind
                            && value.bindingOwner == property.bindingOwner
                        )
                    );
                    if (index < 0)
                    {
                        compatible = false;
                        break;
                    }

                    ShaderParameterPresentation other = owner.Presentation(material.shader!, property.id, out _);
                    if (!other.visible)
                    {
                        compatible = false;
                        break;
                    }

                    if (other.hasRange)
                    {
                        if (presentation.hasRange)
                        {
                            presentation.minimum = Math.Max(presentation.minimum, other.minimum);
                            presentation.maximum = Math.Min(presentation.maximum, other.maximum);
                            if (presentation.minimum > presentation.maximum)
                            {
                                compatible = false;
                                break;
                            }
                        }
                        else
                        {
                            presentation.hasRange = true;
                            presentation.minimum = other.minimum;
                            presentation.maximum = other.maximum;
                        }
                    }

                    MaterialValue value = material.TryGet(property.id, out MaterialValue overridden)
                                          ? overridden
                                          : declarations[index].defaultValue;
                    if (!ShaderPropertyInspector.Compatible(property.type, value.kind))
                    {
                        compatible = false;
                        break;
                    }

                    values.Add(value);
                }

                if (!compatible)
                {
                    continue;
                }
                if (presentation.group != currentGroup)
                {
                    currentGroup = presentation.group;
                    groupOpen = EditorUI.current.widgets.SectionHeader(currentGroup.Length == 0 ? "Common Parameters" : currentGroup);
                }

                if (!groupOpen)
                {
                    continue;
                }
                bool mixed = values.Skip(1).Any(value => !value.Equals(values[0]));
                ImGuiApi.PushID(property.id.value);
                try
                {
                    if (mixed)
                    {
                        EditorUI.current.widgets.Hint(property.displayName + " · Mixed values (editing applies to all selected Materials)");
                    }
                    ShaderPropertyInspector.Draw(context, target, "materials." + property.id.value, property, values[0], value =>
                    {
                        int selectedIndex = 0;
                        foreach (MaterialAsset material in materials.Values)
                        {
                            material.Set(
                                property.id,
                                ShaderPropertyInspector.ApplyEdit(property.type, values[0], value, values[selectedIndex++])
                            );
                        }
                    }, edits, readOnly, presentation);
                    if (mixed && ImGuiApi.SmallButton("Use First Value for Selection"))
                    {
                        foreach (MaterialAsset material in materials.Values)
                        {
                            material.Set(property.id, values[0]);
                        }
                        owner.ReplaceMany(materials);
                    }

                    if (ImGuiApi.SmallButton("Reset Selected Overrides"))
                    {
                        foreach (MaterialAsset material in materials.Values)
                        {
                            material.ReplaceProperties(material.properties.Where(value => value.id != property.id).ToArray());
                        }
                        owner.ReplaceMany(materials);
                    }
                }
                finally
                {
                    ImGuiApi.PopID();
                }
            }

            if (parametersOpen
                && materials.Values.Any(value => value.properties.Count != 0)
                && ImGuiApi.Button("Reset All Selected Overrides")
            ) {
                foreach (MaterialAsset material in materials.Values)
                {
                    material.ReplaceProperties([]);
                }
                owner.ReplaceMany(materials);
            }

            void SetTechnique(ShaderTechniqueID technique)
            {
                foreach (MaterialAsset material in materials.Values)
                {
                    material.techniqueId = technique;
                }
                owner.ReplaceMany(materials);
            }
        }
        finally
        {
            ImGuiApi.EndDisabled();
            ImGuiApi.PopID();
        }
    }

    /// <inheritdoc/>
    protected override bool CanInspect(EditorSelectionSnapshot target)
        => target.identities.Count > 1 && GetAssetIDs(target).Count == target.identities.Count;

    private IReadOnlyList<Guid> GetAssetIDs(EditorSelectionSnapshot target)
    {
        if (!interactions.TryGetModule<MaterialDocuments>(out MaterialDocuments? materials) || materials is null)
        {
            return [];
        }
        var ids = new List<Guid>(target.identities.Count);
        var entries = new List<AssetFileEntry>(target.identities.Count);
        target.CopyTargets(interactions, entries);
        if (entries.Count != target.identities.Count)
        {
            return [];
        }
        entries.Reverse();
        foreach (AssetFileEntry entry in entries)
        {
            if (entry.isDirectory
                || !entry.extension.Equals(".imaterial", StringComparison.OrdinalIgnoreCase)
                || !materials.assets.TryGetInfo(entry.assetPath, out AssetInfo? info)
                || info is null
            ) {
                return [];
            }
            ids.Add(info.persistentID);
        }

        return ids;
    }

    private bool TryOpenDrafts(
        InspectionDrawContext context,
        EditorSelectionSnapshot target,
        out MaterialDocuments? documents,
        out List<AssetDraftDocuments<MaterialAsset>.Draft> drafts
    ) {
        drafts = [];
        if (!context.interactions.TryGetModule(out documents) || documents is null)
        {
            return false;
        }
        foreach (Guid id in GetAssetIDs(target))
        {
            if (!documents.assets.TryGetInfo(id, out AssetInfo? info)
                || info is null
                || !documents.assets.TryGetFileSystemEntry(info.assetPath, out AssetFileEntry entry)
                || !entry.extension.Equals(".imaterial", StringComparison.OrdinalIgnoreCase)
            ) {
                return false;
            }
            AssetDraftDocuments<MaterialAsset>.Draft draft = documents.OpenDraft(info.assetPath);
            drafts.Add(draft);
        }

        return drafts.Count != 0;
    }

    private sealed class SelectionEdits(
        MaterialDocuments documents,
        IReadOnlyDictionary<Guid, MaterialAsset> materials
    ) : IInspectionPropertyEditService
    {
        /// <inheritdoc/>
        public bool ChangeProperty(
            object owner,
            string propertyName,
            Action mutation,
            string historyName,
            string? mergeKey,
            string? componentName
        ) {
            mutation();
            documents.ReplaceMany(materials, mergeKey);
            return true;
        }
    }
}
