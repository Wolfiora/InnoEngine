using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using BGCS.Runtime;
using Xunit;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Diagnostics;
using Inno.Core.Events;
using Inno.Core.Graphs;
using Inno.Core.Input;
using Inno.Core.Serialization;
using Inno.Editor.Assets;
using Inno.Editor.Core;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;
using Inno.Editor.Rendering;
using Inno.Editor.Shaders.ImGui;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

using EngineColor = Inno.Core.Mathematics.Color;
using ImGuiApi = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.Shaders.Native.Tests;

public sealed class ShaderDocumentDraftTests : ShaderEditorFixture
{
    [Fact]
    public unsafe void DiagnosticCheckUsesTheStandardTransientModalWithoutChangingFileSelectionOrHistory()
    {
        AssetFileEntry entry = Create("Diagnostics.ishader");
        SelectAndDraw(entry);
        byte[] before = File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath));
        Assert.True(m_runtime.interactions.For("panel/rendering.shader-editor", entry).Execute("shader/check"));
        EditorModalExtension modal = m_runtime.modals.Single(value => value.id == "shader.check");
        Assert.True(modal.TryGetPresentation(out EditorModalExtension.Presentation presentation));
        Assert.True(presentation.isVisible);
        Assert.Equal(Vector2.Zero, presentation.initialSize);
        Assert.False(presentation.canResize);
        for (int frame = 0; frame < 120 && presentation.isVisible; frame++)
        {
            using IDisposable uiScope = m_ui.Enter();
            ImGuiApi.NewFrame();
            ImGuiApi.SetNextWindowPos(new Vector2(0, 0));
            ImGuiApi.SetNextWindowSize(new Vector2(1200, 800));
            using NativeStringLease nativeTestText0 = Utf8Codec.EncodeNullTerminated("Shader Check Workflow", allowsNull: false);
            _ = ImGuiApi.Raw.Begin((sbyte*)nativeTestText0.pointer, null, 0);
            try
            {
                Assert.True(modal.TryPrepare(m_runtime.context, presentation, new Vector2(1100, 700), out _));
                Assert.True(modal.Draw(m_runtime.context));
            }
            finally
            {
                ImGuiApi.End();
                ImGuiApi.Render();
            }

            m_runtime.Update(new EditorFrame(0.01f, frame * 0.01f, true));
            Thread.Sleep(5);
            Assert.True(modal.TryGetPresentation(out presentation));
        }

        Assert.False(presentation.isVisible);
        Assert.Equal(before, File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath)));
        Assert.Single(m_runtime.interactions.documents.documents);
    }

    [Fact]
    public void SourceSettingsUndoRedoRestoresValidAndInvalidImportStates()
    {
        AssetPath path = CreateFunction();
        var edits = new AssetImportSettingsEdits(m_assets, m_serialization, m_types, m_runtime.interactions);
        AssetImportSettingsSnapshot before = m_assets.GetImportSettings(path);
        ShaderSourceImportSettings settings = Assert.IsType<ShaderSourceImportSettings>(before.value);
        settings.exports = ["MissingFunction"];
        Assert.False(edits.Apply(path, settings, before.fingerprint));
        Assert.Equal(
            "MissingFunction",
            Assert.Single(Assert.IsType<ShaderSourceImportSettings>(m_assets.GetImportSettings(path).value).exports)
        );
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal("Evaluate", Assert.Single(Assert.IsType<ShaderSourceImportSettings>(m_assets.GetImportSettings(path).value).exports));
        Assert.True(m_assets.TryGetInfo(path, out AssetInfo? info));
        Assert.Equal(AssetImportStatus.Imported, info!.status);
        Assert.True(m_runtime.interactions.history.Redo().succeeded);
        Assert.Equal(
            "MissingFunction",
            Assert.Single(Assert.IsType<ShaderSourceImportSettings>(m_assets.GetImportSettings(path).value).exports)
        );
    }

    [Fact]
    public void SourceSettingsUndoRefusesToOverwriteAnExternalSidecarEdit()
    {
        AssetPath path = CreateFunction();
        var edits = new AssetImportSettingsEdits(m_assets, m_serialization, m_types, m_runtime.interactions);
        AssetImportSettingsSnapshot before = m_assets.GetImportSettings(path);
        ShaderSourceImportSettings settings = Assert.IsType<ShaderSourceImportSettings>(before.value);
        settings.exports = ["MissingFunction"];
        _ = edits.Apply(path, settings, before.fingerprint);
        AssetImportSettingsSnapshot current = m_assets.GetImportSettings(path);
        ShaderSourceImportSettings external = Assert.IsType<ShaderSourceImportSettings>(current.value);
        external.exports = ["ExternalFunction"];
        _ = m_assets.SaveImportSettings(path, external, current.fingerprint);
        Assert.False(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal(
            "ExternalFunction",
            Assert.Single(Assert.IsType<ShaderSourceImportSettings>(m_assets.GetImportSettings(path).value).exports)
        );
    }

    [Fact]
    public void FileSelectionOpensOnlyTheShaderCanvasAndNonShaderSelectionKeepsItsDocument()
    {
        AssetFileEntry entry = Create("Surface.ishader");
        SelectAndDraw(entry);
        EditorDocumentContext document = Assert.Single(m_runtime.interactions.documents.documents);
        Assert.Equal(AssetID(entry), document.assetID);
        Assert.NotEqual(entry.identity.persistentID, document.assetID);
        Assert.DoesNotContain(m_runtime.panels, panel => panel.id == "editor.documents" && panel.isOpen);
        m_runtime.interactions.SetSelection(null);
        Draw();
        Assert.Same(document, Assert.Single(m_runtime.interactions.documents.documents));
        SelectAndDraw(entry);
        Assert.Single(m_runtime.interactions.documents.documents);
    }

    [Fact]
    public void SeparateMovesAndUndoRedoRemainDraftsUntilExplicitSave()
    {
        AssetFileEntry entry = Create("Moves.ishader");
        SelectAndDraw(entry);
        GraphDocumentController graph = Controller(entry);
        GraphNodeID node = graph.document.nodes[0].id;
        GraphPosition original = graph.document.nodes[0].position;
        graph.MoveNodes(new Dictionary<GraphNodeID, GraphPosition> { [node] = new(111, 222) });
        Tick();
        Assert.Equal(original, ReadPosition(entry, node));
        Assert.True(Assert.Single(m_runtime.interactions.documents.documents).isDirty);
        graph.MoveNodes(new Dictionary<GraphNodeID, GraphPosition> { [node] = new(333, 444) });
        Tick();
        Assert.Equal(original, ReadPosition(entry, node));
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Tick();
        Assert.Equal(new GraphPosition(111, 222), graph.document.FindNode(node)!.position);
        Assert.Equal(original, ReadPosition(entry, node));
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Tick();
        Assert.Equal(original, ReadPosition(entry, node));
        Assert.True(m_runtime.interactions.history.Redo().succeeded);
        Tick();
        Assert.Equal(new GraphPosition(111, 222), graph.document.FindNode(node)!.position);
        Assert.Equal(original, ReadPosition(entry, node));
        Assert.True(m_runtime.interactions.For("panel/rendering.shader-editor", entry).Execute("shader/save"));
        Assert.Equal(new GraphPosition(111, 222), ReadPosition(entry, node));
        Assert.False(Assert.Single(m_runtime.interactions.documents.documents).isDirty);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Tick();
        Assert.Equal(original, graph.document.FindNode(node)!.position);
        Assert.Equal(new GraphPosition(111, 222), ReadPosition(entry, node));
    }

    [Fact]
    public void SwitchingSelectionKeepsAnInvalidDraftAndExplicitCloseSavePersistsIt()
    {
        AssetFileEntry first = Create("Incomplete.ishader");
        AssetFileEntry second = Create("Other.ishader");
        SelectAndDraw(first);
        GraphNodeID missing = Controller(first).AddNode("tests.uninstalled.node", new GraphPosition(32, 64));
        Tick();
        SelectAndDraw(second);
        Assert.Null(m_source.Read(first.assetPath).document.FindNode(missing));
        Assert.NotNull(Controller(first).document.FindNode(missing));
        EditorDocumentContext firstDocument = m_runtime.interactions.documents.documents.Single(value => value.assetID == AssetID(first));
        Assert.True(m_runtime.interactions.documents.Close(firstDocument.documentID, EditorDocumentCloseMode.Save));
        SelectAndDraw(first);
        Assert.NotNull(Controller(first).document.FindNode(missing));
    }

    [Fact]
    public unsafe void SaveDuringANativeDragCompletesOneDomainEditBeforeWriting()
    {
        AssetFileEntry entry = Create("SaveDrag.ishader");
        SelectAndDraw(entry);
        GraphDocumentController controller = Controller(entry);
        var graph = new GraphDocument();
        var node = new GraphNodeRecord(new GraphNodeID("probe"), "tests.shader-ui-probe") { position = new(0f, 0f) };
        graph.AddNode(node);
        controller.ReplaceDocument(graph, "Prepare Save Probe");
        Assert.True(m_runtime.interactions.For("panel/rendering.shader-editor", entry).Execute("shader/focus"));
        Draw();
        Draw();
        GraphPosition original = controller.document.FindNode(node.id)!.position;
        BeginDragProbe(new Vector2(52f, 37f));
        Assert.Equal(original, controller.document.FindNode(node.id)!.position);
        EditorDocumentContext document = Assert.Single(m_runtime.interactions.documents.documents);
        Assert.True(m_runtime.interactions.documents.Save(document.documentID));
        GraphPosition saved = controller.document.FindNode(node.id)!.position;
        Assert.NotEqual(original, saved);
        Assert.Equal(saved, ReadPosition(entry, node.id));
        ImGuiApi.Raw.AddMouseButtonEvent(ImGuiApi.Raw.GetIO(), 0, 0);
        Draw();
        Assert.Equal(saved, controller.document.FindNode(node.id)!.position);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal(original, controller.document.FindNode(node.id)!.position);
        Assert.Equal(saved, ReadPosition(entry, node.id));
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void ReopenedShaderDocumentsRetireTheirGraphOwners()
    {
        AssetFileEntry entry = Create("Lifecycle.ishader");
        byte[] source = File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath));
        for (int iteration = 0; iteration < 2; iteration++)
        {
            SelectAndDraw(entry);
            GraphDocumentController controller = Controller(entry);
            controller.AddNode("missing.lifecycle", new GraphPosition(iteration, iteration));
            Tick();
            EditorDocumentContext document = Assert.Single(m_runtime.interactions.documents.documents);
            Assert.True(m_runtime.interactions.documents.Close(document.documentID, EditorDocumentCloseMode.Discard));
            Assert.Empty(m_runtime.interactions.documents.documents);
            Assert.False(controller.isAvailable);
            Assert.False(m_sink.graphs!.TryOpenDocument(AssetID(entry), m_runtime.interactions.history, out _));
        }
        Assert.Equal(source, File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath)));
    }

    [Fact]
    public void ExternalSourceConflictKeepsBothDiskAndUnsavedRecovery()
    {
        AssetFileEntry entry = Create("Conflict.ishader");
        SelectAndDraw(entry);
        GraphNodeID pending = Controller(entry).AddNode("tests.unsaved", new GraphPosition(40, 50));
        GraphDocument external = m_source.Read(entry.assetPath).document;
        external.AddNode(new GraphNodeRecord(new GraphNodeID("external"), "tests.external"));
        byte[] externalBytes = GraphDocumentCodec.Encode(external, m_serialization);
        File.WriteAllBytes(Path.Combine(m_root, "Assets", "Conflict.ishader"), externalBytes);
        Tick();
        Assert.False(m_runtime.interactions.documents.Save(Assert.Single(m_runtime.interactions.documents.documents).documentID));
        Assert.Equal(externalBytes, File.ReadAllBytes(Path.Combine(m_root, "Assets", "Conflict.ishader")));
        Assert.NotNull(Controller(entry).document.FindNode(pending));
        Assert.True(Assert.Single(m_runtime.interactions.documents.documents).isDirty);
        Assert.True(File.Exists(Path.Combine(m_assets.libraryRoot, "Editor", "ShaderRecovery", AssetID(entry).ToString("N") + ".inno")));
    }

    [Fact]
    public void SavingAppliesTheGraphWithoutDependingOnFileWatcherAndUndoStaysDraftOnly()
    {
        AssetFileEntry entry = Create("Apply.ishader");
        SelectAndDraw(entry);
        ShaderAsset asset = m_assets.Load<ShaderAsset>(AssetID(entry));
        long before = asset.contentVersion;
        GraphDocumentController controller = Controller(entry);
        GraphDocument graph = controller.document.Clone();
        ShaderDefinition definition = ShaderGraphDocument.ReadDefinition(
            graph,
            m_serialization,
            AssetSerializationContext.Create(m_assets)
        );
        definition.name = "Saved Change";
        graph.SetMetadata(
            ShaderGraphDocument.C_DEFINITION_KEY,
            ShaderGraphDocument.Encode(
                m_serialization.Serialize(definition, AssetSerializationContext.Create(m_assets)),
                m_serialization,
                AssetSerializationContext.Create(m_assets)
            )
        );
        controller.ReplaceDocument(graph, "Rename Shader");
        Tick();
        Assert.Equal(before, asset.contentVersion);
        EditorInteraction interaction = m_runtime.interactions.For("panel/rendering.shader-editor", entry);
        interaction.Focus();
        HotKeyGesture save = HotKeyGesture.Primary(KeyCode.S, new EditorKeyboardPolicy(KeyModifier.Control, "Super"));
        m_runtime.HandleKeyPressed(new KeyPressedEvent(0, save.key, save.modifiers));
        Tick();
        Assert.True(m_assets.TryLoad(AssetID(entry), out ShaderAsset? saved));
        Assert.Equal("Saved Change", saved!.definition!.name);
        Assert.True(saved.contentVersion > before);
        long savedVersion = saved.contentVersion;
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Tick();
        Assert.Equal("Saved Change", saved.definition!.name);
        Assert.Equal(savedVersion, saved.contentVersion);
        Assert.True(interaction.Execute("shader/save"));
        Tick();
        Assert.True(m_assets.TryLoad(AssetID(entry), out ShaderAsset? undone));
        Assert.NotEqual("Saved Change", undone!.definition!.name);
    }

    [Fact]
    public void CancelKeepsDraftAndDiscardRestoresDiskWithoutApplyingIt()
    {
        AssetFileEntry entry = Create("Close.ishader");
        SelectAndDraw(entry);
        GraphNodeID node = Controller(entry).AddNode("tests.unsaved", new GraphPosition(0, 0));
        Tick();
        EditorDocumentContext document = Assert.Single(m_runtime.interactions.documents.documents);
        Assert.False(m_runtime.interactions.documents.Close(document.documentID, EditorDocumentCloseMode.Cancel));
        Assert.NotNull(Controller(entry).document.FindNode(node));
        Assert.True(m_runtime.interactions.documents.Close(document.documentID, EditorDocumentCloseMode.Discard));
        SelectAndDraw(entry);
        Assert.Null(Controller(entry).document.FindNode(node));
        Assert.Null(m_source.Read(entry.assetPath).document.FindNode(node));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RestartRecoversOnlyTheLatestDraftAndNeverAppliesIt(bool undo)
    {
        AssetFileEntry entry = Create("Recovery.ishader");
        SelectAndDraw(entry);
        byte[] saved = File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath));
        GraphNodeID node = Controller(entry).AddNode("tests.unsaved", new GraphPosition(0, 0));
        Tick();
        if (undo)
        {
            Assert.True(m_runtime.interactions.history.Undo().succeeded);
        }
        // Shutdown must capture even the final undo that has not had an update frame yet.
        m_runtime.Dispose();
        Assert.Equal(saved, File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath)));
        m_runtime = CreateRuntime();
        SelectAndDraw(entry);
        Assert.Equal(!undo, Controller(entry).document.FindNode(node)is not null);
        Assert.Equal(saved, File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath)));
    }

    [Fact]
    public void DraftCompilationNeverReplacesTheCanonicalArtifactAndLayoutChangesDoNotRecompile()
    {
        AssetFileEntry entry = Create("Preview.ishader");
        ShaderAsset shader = m_assets.Load<ShaderAsset>(entry.assetPath);
        byte[] disk = File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath));
        RenderTextureFormat[] formats = Enum.GetValues<RenderTextureFormat>();
        var capabilities = new GraphicsCapabilities(
            GraphicsApi.metal,
            GraphicsCapability.None,
            new GraphicsLimits(256, 8, 8192, 16),
            formats,
            formats,
            formats,
            formats,
            false,
            false
        );
        var compilation = new EditorShaderCompilation(m_artifacts, capabilities);
        RenderShaderArtifact? canonical = null;
        for (int i = 0; i < 100 && canonical is null; i++)
        {
            _ = m_artifacts.GetShaderArtifact(shader, RenderShaderVariant.empty, capabilities, out canonical);
            Thread.Sleep(1);
        }

        Assert.NotNull(canonical);
        GraphDocument draft = m_source.Read(entry.assetPath).document;
        SerializationContext context = AssetSerializationContext.Create(m_assets);
        ShaderDefinition definition = ShaderGraphDocument.ReadDefinition(draft, m_serialization, context);
        ShaderPropertyDefinition property = definition.properties[0];
        property.defaultValue = MaterialValue.FromColor(new EngineColor(0.23f, 1, 1, 1));
        definition.properties[0] = property;
        draft.SetMetadata(
            ShaderGraphDocument.C_DEFINITION_KEY,
            ShaderGraphDocument.Encode(m_serialization.Serialize(definition, context), m_serialization, context)
        );
        Guid previewID = Guid.NewGuid();
        EditorShaderDraftCompilationSnapshot Poll(ulong revision)
        {
            EditorShaderDraftCompilationSnapshot result = compilation.RequestDraft(previewID, draft, revision, RenderShaderVariant.empty);
            for (int i = 0; i < 100 && result.state == EditorShaderCompilationState.Compiling; i++)
            {
                Thread.Sleep(1);
                result = compilation.RequestDraft(previewID, draft, revision, RenderShaderVariant.empty);
            }

            return result;
        }

        EditorShaderDraftCompilationSnapshot preview = Poll(1);
        Assert.Equal(EditorShaderCompilationState.Succeeded, preview.state);
        Assert.NotNull(preview.artifact);
        Assert.NotEqual(canonical!.contentHash, preview.artifact!.contentHash);
        draft.nodes[0].position = new(123, 456);
        Assert.Same(preview.artifact, Poll(2).artifact);
        GraphNodeRecord output = draft.nodes.First(value => value.definitionID == ShaderGraphDocument.C_OUTPUT_DEFINITION_ID);
        draft.RemoveNode(output.id);
        EditorShaderDraftCompilationSnapshot failed = Poll(3);
        Assert.Equal(EditorShaderCompilationState.Failed, failed.state);
        Assert.True(failed.usingLastGood);
        Assert.Same(preview.artifact, failed.artifact);
        _ = m_artifacts.GetShaderArtifact(shader, RenderShaderVariant.empty, capabilities, out RenderShaderArtifact? unchanged);
        Assert.Same(canonical, unchanged);
        Assert.Equal(disk, File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath)));
        compilation.ReleaseDraft(previewID);
    }

    private GraphPosition ReadPosition(
        AssetFileEntry entry,
        GraphNodeID id
    ) => m_source.Read(entry.assetPath).document.FindNode(id)!.position;
}
