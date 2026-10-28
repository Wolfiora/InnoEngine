using Inno.Editor.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Rendering.Assets;
using EditorImGui = Inno.Editor.ImGui.ImGui;
using Inno.Rendering.Assets.Authoring;

namespace Inno.Editor.Panel.ShaderEditor;
internal sealed partial class ShaderEditorCanvas
{
    private void SourceSettings(Guid sourceId)
    {
        if (!owner.assets.TryGetInfo(sourceId, out AssetInfo? info) || info is null)
            return;
        InspectorRow("source.actions", "Source", () =>
        {
            if (EditorImGui.SmallButton("Show in File Browser")
                && owner.interactions.TryGetModule<Inno.Editor.Assets.ImGui.AssetBrowserModule>(out var browser)
                && browser is not null)
                RevealShaderFunction.Reveal(owner, browser, sourceId);
            EditorImGui.SameLine(0f, -1f);
            if (EditorImGui.SmallButton("Import Settings…"))
            {
                LoadSourceSettings(info.assetPath, sourceId);
                EditorImGui.OpenPopup("##source-settings");
            }
        });
        EditorImGui.SetNextWindowSize(new(580, 470), (ImGuiCond.Appearing));
        if (!EditorImGui.BeginPopup("##source-settings"))
            return;
        try
        {
            EditorImGui.Text(info.assetPath.ToString());
            EditorImGui.Separator();
            if (draft.settingsSource != sourceId)
                LoadSourceSettings(info.assetPath, sourceId);
            bool readOnly = !owner.assets.TryGetFileSystemEntry(info.assetPath, out AssetFileEntry entry) || entry.isReadOnly;
            EditorImGui.BeginDisabled(readOnly);
            try
            {
                var settings = owner.serialization.Deserialize<ShaderSourceImportSettings>(draft.sourceSettings, owner.context);
                bool changed = false;
                InspectorRow("source.language", "Language", () =>
                {
                    if (!EditorUI.current.widgets.BeginBoundedCombo("##language", settings.languageId))
                        return;
                    try
                    {
                        foreach (string language in owner.frontends.languageIds)
                            if (EditorImGui.Selectable(language, settings.languageId == language))
                            {
                                settings.languageId = language;
                                changed = true;
                            }
                    }
                    finally
                    {
                        EditorUI.current.widgets.EndBoundedCombo();
                    }
                });
                string implementation = settings.implementationId;
                bool implementationChanged = false;
                InspectorRow("source.adapter", "Adapter", () => implementationChanged = EditorImGui.InputText("##adapter", ref implementation, 256));
                if (implementationChanged)
                {
                    settings.implementationId = implementation;
                    changed = true;
                }

                string catalog = settings.catalogPath;
                bool catalogChanged = false;
                InspectorRow("source.catalog", "Catalog Path", () => catalogChanged = EditorImGui.InputText("##catalog", ref catalog, 256));
                if (catalogChanged)
                {
                    settings.catalogPath = catalog;
                    changed = true;
                }

                int catalogOrder = settings.catalogOrder;
                bool orderChanged = false;
                InspectorRow("source.catalog-order", "Catalog Order", () => orderChanged = EditorImGui.InputInt("##catalog-order", ref catalogOrder, 1, 100, 0));
                if (orderChanged)
                {
                    settings.catalogOrder = catalogOrder;
                    changed = true;
                }

                EditorUI.current.widgets.WrappedText("Catalog paths are declared by the source library and group its exported functions in Shader creation tools. They do not affect compilation or runtime assets.");
                EditorImGui.SeparatorText("Exported Functions");
                for (int i = 0; i < settings.exports.Length; i++)
                {
                    EditorImGui.PushId("export." + i);
                    try
                    {
                        string function = settings.exports[i];
                        bool functionChanged = false, remove = false;
                        InspectorRow("source.export", "Function " + (i + 1), () =>
                        {
                            functionChanged = EditorImGui.InputText("##name", ref function, 256);
                            EditorImGui.SameLine(0f, -1f);
                            remove = EditorImGui.SmallButton("Remove");
                        });
                        if (functionChanged)
                        {
                            settings.exports[i] = function;
                            changed = true;
                        }

                        if (remove)
                        {
                            settings.exports = settings.exports.Where((
                                _,
                                index
                            ) => index != i).ToArray();
                            changed = true;
                            break;
                        }
                    }
                    finally
                    {
                        EditorImGui.PopId();
                    }
                }

                if (CenteredAddButton("Add Export"))
                {
                    settings.exports = [..settings.exports, "Function"];
                    changed = true;
                }

                EditorUI.current.widgets.WrappedText("Every listed name is exported as an independent graph function. All other functions remain private helpers. Ports are parsed from each exported declaration; alternative implementations must expose matching interfaces.");
                EditorImGui.SeparatorText("Alternative Implementations");
                for (int i = 0; i < settings.implementations.Length; i++)
                {
                    EditorImGui.PushId(i);
                    try
                    {
                        ShaderFunctionAsset? current = settings.implementations[i];
                        bool remove = false;
                        InspectorRow("source.implementation", "Implementation " + (i + 1), () =>
                        {
                            if (EditorUI.current.widgets.BeginBoundedCombo("##implementation", current?.assetPath.ToString() ?? "Missing source"))
                            {
                                try
                                {
                                    foreach (AssetFileEntry source in owner.assets.GetFileSystemEntries(includeDirectories: false))
                                        if (source.extension == ".ishadersource"
                                            && source.assetPath != info.assetPath
                                            && EditorImGui.Selectable(
                                            source.assetPath.ToString(),
                                            current?.assetPath == source.assetPath
                                        )
                                            && owner.assets.TryLoad(
                                            source.assetPath,
                                            out ShaderFunctionAsset? replacement
                                        )
                                            && replacement is not null)
                                        {
                                            settings.implementations[i] = replacement;
                                            changed = true;
                                        }
                                }
                                finally
                                {
                                    EditorUI.current.widgets.EndBoundedCombo();
                                }
                            }

                            EditorImGui.SameLine(0f, -1f);
                            remove = EditorImGui.SmallButton("Remove");
                        });
                        if (remove)
                        {
                            settings.implementations = settings.implementations.Where((
                                _,
                                index
                            ) => index != i).ToArray();
                            changed = true;
                            break;
                        }
                    }
                    finally
                    {
                        EditorImGui.PopId();
                    }
                }

                if (CenteredAddButton("Add Implementation"))
                {
                    settings.implementations = [..settings.implementations, null!];
                    changed = true;
                }

                if (changed)
                    draft.sourceSettings = owner.serialization.Serialize(settings, owner.context);
                EditorImGui.Separator();
                if (EditorImGui.Button("Apply Import Settings"))
                {
                    bool imported = owner.importSettings.Apply(info.assetPath, settings, draft.sourceSettingsFingerprint);
                    LoadSourceSettings(info.assetPath, sourceId);
                    draft.sourceSettingsStatus = imported ? "Applied · ports refreshed" : "Settings saved · import failed; see diagnostics";
                    draft.portRevision = ulong.MaxValue;
                }
            }
            finally
            {
                EditorImGui.EndDisabled();
            }

            EditorImGui.SameLine(0f, -1f);
            if (EditorImGui.Button("Reload"))
                LoadSourceSettings(info.assetPath, sourceId);
            if (readOnly)
                EditorUI.current.widgets.ColoredText(Inno.Editor.ImGui.EditorPalette.textDisabled, "Installed source settings are read-only.");
            if (draft.sourceSettingsStatus.Length != 0)
                EditorUI.current.widgets.WrappedText(draft.sourceSettingsStatus);
        }
        catch (Exception failure) when ((failure is IOException or InvalidOperationException or ArgumentException or FormatException)
            && Inno.Core.Execution.RetirementPendingException.Find(failure) is null)
        {
            draft.sourceSettingsStatus = failure.Message;
            EditorUI.current.widgets.WrappedText(draft.sourceSettingsStatus);
        }
        finally
        {
            EditorImGui.EndPopup();
        }
    }

    private void LoadSourceSettings(
        AssetPath path,
        Guid sourceId
    ) {
        AssetImportSettingsSnapshot snapshot = owner.assets.GetImportSettings(path);
        if (snapshot.value is not ShaderSourceImportSettings settings)
            throw new InvalidOperationException("The source importer settings are unavailable.");
        draft.settingsSource = sourceId;
        draft.sourceSettingsFingerprint = snapshot.fingerprint;
        draft.sourceSettings = owner.serialization.Serialize(settings, owner.context);
        draft.sourceSettingsStatus = "";
    }
}
