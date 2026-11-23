using System;
using System.IO;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Editor.Assets;
using Inno.Editor.Inspection;
using Inno.Editor.Rendering;
using Inno.Rendering.Assets;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using ImGuiApi = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Inspector;

internal static class PipelineInspector
{
    internal static void DrawHeader(
        InspectionDrawContext context,
        PipelineDocuments documents,
        Guid id
    ) {
        AssetDraftDocuments<RenderPipelineAsset>.Draft draft = documents.drafts.GetDraft(id);
        ImGuiApi.BeginDisabled(draft.readOnly);
        try
        {
            if (ImGuiApi.Button("Save"))
            {
                _ = context.interactions.documents.Save(draft.documentID);
            }
            ImGuiApi.SameLine(0f, -1f);
            if (ImGuiApi.Button("Revert"))
            {
                _ = context.interactions.documents.Revert(draft.documentID);
            }
        }
        finally
        {
            ImGuiApi.EndDisabled();
        }
    }

    internal static void Draw(
        InspectionDrawContext context,
        PipelineDocuments documents,
        Guid id
    ) {
        AssetDraftDocuments<RenderPipelineAsset>.Draft draft = documents.drafts.GetDraft(id);
        RenderPipelineAsset pipeline = documents.Read(id);
        var edits = new DraftEdits(documents, id, pipeline);
        ImGuiApi.PushID(id.ToString("N"));
        try
        {
            if (EditorUI.current.widgets.SectionHeader(
                "Render Pipeline",
                "This source is the configuration authority. Unsaved settings do not change Scene, Game or Player content."
            )) {
                EditorUI.current.widgets.Hint(
                    draft.isDirty ? "Unsaved changes · rendering unchanged" : "Saved · import and activation are separate"
                );
                if (draft.error.Length != 0)
                {
                    EditorUI.current.widgets.Hint(draft.error);
                }
                if (draft.readOnly)
                {
                    EditorUI.current.widgets.Hint("Installed Pipeline · copy into the project to edit");
                }
                EditorUI.current.widgets.Hint("Pipeline: " + pipeline.pipelineTypeId);
                DrawSettings("pipeline.settings", pipeline.pipelineState, value => pipeline.pipelineState = value);
            }

            bool featuresOpen = EditorUI.current.widgets.SectionHeader(
                "Features",
                "Ordered extension settings belong to this Pipeline. Each enabled feature participates in its render graph."
            );
            RenderFeatureConfiguration[] features = pipeline.features;
            for (int index = 0; featuresOpen && index < features.Length; index++)
            {
                int slot = index;
                ImGuiApi.PushID(slot);
                try
                {
                    RenderFeatureConfiguration feature = features[slot];
                    context.properties.DrawValue(
                        context.editorContext,
                        draft,
                        "pipeline.features."
                        + slot
                        + ".enabled",
                        feature.featureTypeId,
                        typeof(bool),
                        () => features[slot].enabled,
                        value =>
                    {
                        features[slot].enabled = (bool)value!;
                        pipeline.features = features;
                    },
                        edits,
                        draft.readOnly
                    );
                    DrawSettings("pipeline.features." + slot, feature.state, value =>
                    {
                        features[slot].state = value;
                        pipeline.features = features;
                    });
                    ImGuiApi.BeginDisabled(draft.readOnly);
                    try
                    {
                        if (slot > 0 && ImGuiApi.SmallButton("Move Up"))
                        {
                            (features[slot - 1], features[slot]) = (features[slot], features[slot - 1]);
                            pipeline.features = features;
                            documents.Replace(id, pipeline);
                            break;
                        }

                        if (slot + 1 < features.Length)
                        {
                            if (slot > 0)
                            {
                                ImGuiApi.SameLine(0f, -1f);
                            }
                            if (ImGuiApi.SmallButton("Move Down"))
                            {
                                (features[slot + 1], features[slot]) = (features[slot], features[slot + 1]);
                                pipeline.features = features;
                                documents.Replace(id, pipeline);
                                break;
                            }
                        }
                    }
                    finally
                    {
                        ImGuiApi.EndDisabled();
                    }
                }
                finally
                {
                    ImGuiApi.PopID();
                }
            }

            if (featuresOpen && features.Length == 0)
            {
                EditorUI.current.widgets.Hint("No additional features");
            }
        }
        finally
        {
            ImGuiApi.PopID();
        }

        void DrawSettings(
            string path,
            SerializedRenderExtensionState state,
            Action<SerializedRenderExtensionState> assign
        ) {
            try
            {
                ISerializable? settings = documents.RestoreSettings(state);
                if (settings is null)
                {
                    EditorUI.current.widgets.Hint("No typed settings");
                    return;
                }

                var settingsEdits = new SettingsEdits(
                    edits,
                    () => assign(new SerializedRenderExtensionState(documents.assets.CaptureProperties(settings)))
                );
                foreach (SerializedProperty property in documents.serialization.GetProperties(settings))
                {
                    context.properties.DrawDraftProperty(
                        context.editorContext,
                        draft,
                        settings,
                        path,
                        property,
                        settingsEdits,
                        draft.readOnly
                    );
                }
            }
            catch (Exception error) when ((error is IOException
                                           or UnauthorizedAccessException
                                           or InvalidOperationException
                                           or ArgumentException
                                           or FormatException)
                                          && RetirementPendingException.Find(error) is null)
            {
                EditorUI.current.widgets.Hint(
                    "Settings unavailable · " + state.stableTypeId + " · " + error.Message + " · Stored properties and dependencies are retained."
                );
            }
        }
    }

    private sealed class DraftEdits(
        PipelineDocuments documents,
        Guid id,
        RenderPipelineAsset pipeline
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
            documents.Replace(id, pipeline, mergeKey);
            return true;
        }
    }

    private sealed class SettingsEdits(
        DraftEdits edits,
        Action capture
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
        ) => edits.ChangeProperty(owner, propertyName, () =>
        {
            mutation();
            capture();
        }, historyName, mergeKey, componentName);
    }
}
