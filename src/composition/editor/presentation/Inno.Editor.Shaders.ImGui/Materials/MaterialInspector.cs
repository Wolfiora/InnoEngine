using System;
using System.Linq;
using System.Numerics;
using Inno.Assets.Pipeline;
using Inno.Editor.Assets;
using Inno.Editor.Inspection;
using Inno.Editor.Rendering;
using Inno.Rendering;
using Inno.Rendering.Assets;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Shaders.ImGui;

internal static class MaterialInspector
{
    internal static void DrawHeader(
        InspectionDrawContext context,
        MaterialDocuments documents,
        AssetDraftDocuments<MaterialAsset>.Draft draft
    ) => EditorUI.current.widgets.Disabled(draft.readOnly, () =>
    {
        if (Gui.Button("Save"))
        {
            _ = context.interactions.documents.Save(draft.documentID);
        }
        Gui.SameLine(0f, -1f);
        if (Gui.Button("Revert"))
        {
            _ = context.interactions.documents.Revert(draft.documentID);
        }
    });

    internal static void Draw(
        InspectionDrawContext context,
        MaterialDocuments documents,
        AssetDraftDocuments<MaterialAsset>.Draft draft
    ) {
        MaterialAsset material = documents.Read(draft.id);
        var edits = new DraftEdits(documents, draft, material);
        Gui.PushID(draft.id.ToString("N"));
        try
        {
            if (EditorUI.current.widgets.SectionHeader("Preview", "Shows an isolated preview of the current Material draft."))
            {
                if (material.shader is null || material.shader.isMissing)
                {
                    EditorUI.current.widgets.Hint("Choose an available Shader to preview this Material.");
                }
                else if (context.interactions.TryGetModule<ShaderPreviews>(out ShaderPreviews? shaderPreviews)
                         && shaderPreviews is not null
                ) {
                    shaderPreviews.DrawMaterial(draft.id, material, MathF.Max(1f, MathF.Min(256f, Gui.GetContentRegionAvailable().X)));
                }
                else
                {
                    EditorUI.current.widgets.Hint("Material preview is unavailable.");
                }
            }

            bool materialOpen = EditorUI.current.widgets.SectionHeader(
                "Material",
                "Edits stay in this draft. Save publishes through normal asset import; Revert reloads the source."
            );
            if (materialOpen)
            {
                EditorUI.current.widgets.Hint(
                    draft.isDirty ? "Unsaved changes · Scene/Game unchanged" : "Saved · compilation and import are separate"
                );
                if (draft.error.Length != 0)
                {
                    EditorUI.current.widgets.Hint(draft.error);
                }
                if (draft.readOnly)
                {
                    EditorUI.current.widgets.Hint("Installed material · copy to the project to edit");
                }
                Draw("shader", "Shader", typeof(ShaderAsset), () => material.shader, value => material.shader = (ShaderAsset? )value);
            }

            ShaderDefinition? definition = material.shader?.definition;
            if (material.shader is null || material.shader.isMissing || definition is null)
            {
                if (materialOpen)
                {
                    EditorUI.current.widgets.Hint(
                        "Choose an available Shader. Existing overrides are retained until its interface can be resolved."
                    );
                }
                return;
            }

            if (materialOpen && Gui.SmallButton("Open Shader Editor"))
            {
                context.interactions.SetSelection(material.shader);
                context.interactions.OpenPanel("rendering.shader-editor");
            }

            if (materialOpen)
            {
                Gui.SameLine(0f, -1f);
                if (Gui.SmallButton("Locate Shader")
                    && documents.assets.TryGetFileSystemEntry(material.shader.assetPath, out AssetFileEntry shaderEntry)
                ) {
                    context.interactions.SetSelection(shaderEntry);
                }
            }

            if (materialOpen && (definition.techniques.Length > 1 || material.techniqueId.isValid))
            {
                if (EditorUI.current.widgets.BeginBoundedCombo(
                    "##technique",
                    material.techniqueId.isValid ? material.techniqueId.value : "Automatic"
                )) {
                    try
                    {
                        if (Gui.Selectable("Automatic", !material.techniqueId.isValid) && !draft.readOnly)
                        {
                            material.techniqueId = default;
                            documents.Replace(draft.id, material);
                        }

                        foreach (ShaderTechniqueDefinition technique in definition.techniques)
                        {
                            if (Gui.Selectable(technique.id.value, material.techniqueId == technique.id) && !draft.readOnly)
                            {
                                material.techniqueId = technique.id;
                                documents.Replace(draft.id, material);
                            }
                        }
                    }
                    finally
                    {
                        EditorUI.current.widgets.EndBoundedCombo();
                    }
                }
            }

            bool parametersOpen = EditorUI.current.widgets.SectionHeader(
                "Parameters",
                "Inherited values follow Shader defaults. Changing a field creates a Material override; Reset restores inheritance."
            );
            bool groupOpen = parametersOpen;
            string currentGroup = string.Empty;
            int visibleParameters = 0;
            foreach (ShaderPropertyDefinition property in definition.properties.Where(IsEditable))
            {
                ShaderParameterPresentation presentation = documents.Presentation(
                    material.shader,
                    property.id,
                    out string presentationError
                );
                if (presentationError.Length != 0)
                {
                    EditorUI.current.widgets.Hint("Parameter presentation unavailable: " + presentationError);
                }
                if (!presentation.visible)
                {
                    continue;
                }
                visibleParameters++;
                if (presentation.group != currentGroup)
                {
                    currentGroup = presentation.group;
                    groupOpen = EditorUI.current.widgets.SectionHeader(currentGroup.Length == 0 ? "Parameters" : currentGroup);
                }

                if (!groupOpen)
                {
                    continue;
                }
                bool overridden = material.TryGet(property.id, out MaterialValue value);
                if (overridden && !ShaderPropertyInspector.Compatible(property.type, value.kind))
                {
                    continue;
                }
                if (!overridden)
                {
                    value = property.defaultValue;
                }
                Gui.PushID(property.id.value);
                try
                {
                    ShaderPropertyInspector.Draw(
                        context,
                        draft,
                        "material." + property.id.value,
                        property,
                        value,
                        next => material.Set(property.id, next),
                        edits,
                        draft.readOnly,
                        presentation
                    );
                    EditorUI.current.widgets.Hint(overridden ? "Material override" : "Inherited from Shader");
                    if (overridden && !draft.readOnly && Gui.SmallButton("Reset override"))
                    {
                        material.ReplaceProperties(material.properties.Where(entry => entry.id != property.id).ToArray());
                        documents.Replace(draft.id, material);
                    }

                    if (value.kind == MaterialValueKind.Texture)
                    {
                        if (value.texture is { isMissing: false } texture
                            && documents.previews.TryGetTexture(texture, out EditorPreviewHandle preview)
                        ) {
                            float width = MathF.Max(1f, MathF.Min(128f, Gui.GetContentRegionAvailable().X));
                            float height = width * preview.pixelHeight / preview.pixelWidth;
                            if (height > 128f)
                            {
                                width *= 128f / height;
                                height = 128f;
                            }

                            documents.previews.Draw(preview, new Vector2(width, height));
                        }
                    }
                }
                finally
                {
                    Gui.PopID();
                }
            }

            if (parametersOpen && visibleParameters == 0 && definition.keywords.Length == 0)
            {
                EditorUI.current.widgets.Hint("No editable parameters.");
            }
            if (parametersOpen && material.properties.Count != 0 && !draft.readOnly && Gui.Button("Reset all overrides"))
            {
                material.ReplaceProperties([]);
                documents.Replace(draft.id, material);
            }

            foreach (ShaderKeywordDefinition keyword in parametersOpen ? definition.keywords : [])
            {
                string selected = keyword.options.FirstOrDefault(option => material.keywords.Contains(option)) ?? "None";
                if (!EditorUI.current.widgets.BeginBoundedCombo(keyword.id, selected))
                {
                    continue;
                }
                try
                {
                    if (Gui.Selectable("None", selected == "None"))
                    {
                        SetOption(null);
                    }
                    foreach (string option in keyword.options)
                    {
                        if (Gui.Selectable(option, selected == option))
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
                    if (draft.readOnly)
                    {
                        return;
                    }
                    foreach (string option in keyword.options)
                    {
                        material.SetKeyword(option, option == next);
                    }
                    documents.Replace(draft.id, material);
                }
            }

            MaterialPropertyEntry[] orphaned = material.properties
                .Where(
                    entry => !definition.properties.Any(
                        property => (
                            property.id == entry.id
                            && IsEditable(property)
                            && ShaderPropertyInspector.Compatible(property.type, entry.value.kind)
                        )
                    )
                )
                .ToArray();
            if (orphaned.Length != 0)
            {
                if (EditorUI.current.widgets.SectionHeader(
                    "Unresolved Overrides",
                    "These values were retained after an interface change. Remove them explicitly or restore a compatible Shader before publishing."
                )) {
                    foreach (MaterialPropertyEntry entry in orphaned)
                    {
                        Gui.Text(entry.id.value + " · " + entry.value.kind);
                        if (!draft.readOnly && Gui.SmallButton("Remove##" + entry.id.value))
                        {
                            material.ReplaceProperties(material.properties.Where(value => value.id != entry.id).ToArray());
                            documents.Replace(draft.id, material);
                        }
                    }
                }
            }

            ShaderPropertyDefinition[] passBindings = definition.properties.Where(property => !IsEditable(property)).ToArray();
            string[] unknownKeywords = material.keywords
                .Where(value => !definition.keywords.Any(keyword => keyword.options.Contains(value)))
                .ToArray();
            if (unknownKeywords.Length != 0)
            {
                if (EditorUI.current.widgets.SectionHeader(
                    "Unresolved Keywords",
                    "These overrides no longer belong to the Shader interface. They remain until explicitly removed."
                )) {
                    foreach (string keyword in unknownKeywords)
                    {
                        Gui.Text(keyword);
                        if (!draft.readOnly && Gui.SmallButton("Remove##keyword." + keyword))
                        {
                            material.SetKeyword(keyword, false);
                            documents.Replace(draft.id, material);
                        }
                    }
                }
            }

            if (passBindings.Length != 0)
            {
                if (EditorUI.current.widgets.SectionHeader(
                    "Pipeline Bindings",
                    "These resources are supplied by the Render Pass, not by this Material."
                )) {
                    foreach (ShaderPropertyDefinition property in passBindings)
                    {
                        EditorUI.current.widgets.Hint(property.displayName + " · " + property.type + " · " + property.bindingOwner);
                    }
                }
            }
        }
        finally
        {
            Gui.PopID();
        }

        void Draw(
            string path,
            string label,
            Type type,
            Func<object?> getter,
            Action<object?> setter
        ) => context.properties.DrawValue(
            context.editorContext,
            draft,
            "material." + path,
            label,
            type,
            getter,
            setter,
            edits,
            draft.readOnly
        );
    }

    private static bool IsEditable(ShaderPropertyDefinition property)
        => property.bindingOwner == ShaderPropertyBindingOwner.Material
           && property.bindingKind is ShaderPropertyBindingKind.Uniform or ShaderPropertyBindingKind.SampledTexture;

    private sealed class DraftEdits(
        MaterialDocuments documents,
        AssetDraftDocuments<MaterialAsset>.Draft draft,
        MaterialAsset material
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
            documents.Replace(draft.id, material, mergeKey);
            return true;
        }
    }
}
