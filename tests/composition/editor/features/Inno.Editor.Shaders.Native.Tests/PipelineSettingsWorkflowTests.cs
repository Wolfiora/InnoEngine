using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BGCS.Runtime;
using Xunit;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Diagnostics;
using Inno.Core.Events;
using Inno.Core.Graphs;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Core;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;
using Inno.Editor.Rendering;
using Inno.Editor.Shaders.ImGui;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Shaders;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using EngineColor = Inno.Core.Mathematics.Color;
using EngineVector4 = Inno.Core.Mathematics.Vector4;
using ImGuiApi = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.Shaders.Native.Tests;

public sealed class PipelineSettingsWorkflowTests : ShaderEditorFixture
{
    [Fact]
    public void PipelineSettingsDraftsPreserveReferencesAndPublishOnlyAfterSave()
    {
        (AssetPath materialPath, _) = CreateMaterial();
        MaterialAsset material = m_assets.Load<MaterialAsset>(materialPath);
        var originalSettings = new WorkflowPipelineSettings
        {
            material = material,
            exposure = 1
        };
        var initial = new RenderPipelineAsset
        {
            pipelineTypeId = "tests.workflow.pipeline",
            pipelineState = new(m_assets.CaptureProperties(originalSettings))
        };
        AssetPath path = AssetPath.Project("Workflow.irenderpipeline");
        AssetSourceStore sources = m_assets.CreateSourceStore();
        sources.Save(path, sources.Encode(initial), null);
        Assert.True(m_assets.Import(path));
        byte[] before = sources.Read(path).bytes;
        RenderPipelineAsset canonical = m_assets.Load<RenderPipelineAsset>(path);
        PipelineDocuments documents = Assert.IsType<PipelineDocuments>(m_sink.pipelines);
        Guid id = documents.Open(path);
        documents.ReplaceSettings(id, new WorkflowPipelineSettings { material = material, exposure = 3 });
        Assert.Equal(before, sources.Read(path).bytes);
        Assert.Equal(1, Read(canonical).exposure);
        Assert.Equal(3, Read(documents.Read(id)).exposure);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal(1, Read(documents.Read(id)).exposure);
        Assert.True(m_runtime.interactions.history.Redo().succeeded);
        RenderPipelineAsset draft = documents.Read(id);
        Assert.Equal(material.identity.persistentID, Assert.Single(draft.pipelineState.dependencies).persistentID);
        EditorDocumentContext document = Assert.Single(m_runtime.interactions.documents.documents);
        Assert.True(m_runtime.interactions.documents.Save(document.documentID));
        Assert.Equal(1, Read(canonical).exposure);
        Tick();
        Assert.Equal(3, Read(canonical).exposure);
        Assert.Same(material, Read(canonical).material);
        Assert.True(m_assets.TryGetInfo(path, out AssetInfo? info));
        Assert.Contains(m_assets.GetDependencies(canonical), value => value.persistentID == material.identity.persistentID);
        documents.ReplaceSettings(id, new WorkflowPipelineSettings { material = null, exposure = 8 });
        Assert.True(m_runtime.interactions.documents.Revert(document.documentID));
        Assert.Same(material, Read(documents.Read(id)).material);
        Assert.Equal(3, Read(documents.Read(id)).exposure);
        WorkflowPipelineSettings Read(RenderPipelineAsset pipeline)
        {
            var result = new WorkflowPipelineSettings();
            m_assets.RestoreProperties(pipeline.pipelineState.stableTypeId, pipeline.pipelineState.propertyData, result);
            return result;
        }
    }

    [Fact]
    public void PipelineMissingSettingsKeepOpaqueBytesThroughHistoryAndRecovery()
    {
        var unknown = new SerializedRenderExtensionState(Guid.NewGuid(), [19, 42, 7]);
        var source = new RenderPipelineAsset
        {
            pipelineTypeId = "tests.missing-pipeline",
            pipelineState = unknown
        };
        AssetPath path = AssetPath.Project("Missing.irenderpipeline");
        AssetSourceStore sources = m_assets.CreateSourceStore();
        sources.Save(path, sources.Encode(source), null);
        Assert.True(m_assets.Import(path));
        PipelineDocuments documents = Assert.IsType<PipelineDocuments>(m_sink.pipelines);
        Guid id = documents.Open(path);
        RenderPipelineAsset draft = documents.Read(id);
        draft.features = [new RenderFeatureConfiguration("tests.optional", unknown, false)];
        documents.Replace(id, draft);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Empty(documents.Read(id).features);
        Assert.True(m_runtime.interactions.history.Redo().succeeded);
        m_runtime.Dispose();
        m_runtime = CreateRuntime();
        documents = Assert.IsType<PipelineDocuments>(m_sink.pipelines);
        documents.Open(path);
        RenderPipelineAsset recovered = documents.Read(id);
        Assert.Equal(unknown.stableTypeId, recovered.pipelineState.stableTypeId);
        Assert.Equal(unknown.propertyData, recovered.pipelineState.propertyData);
        Assert.Equal(unknown.propertyData, Assert.Single(recovered.features).state.propertyData);
        Assert.Empty(sources.Decode<RenderPipelineAsset>(sources.Read(path).bytes).features);
    }

    [StableTypeID("8c096b6f-92c3-48ed-917e-955858d6a65c")]
    public sealed unsafe class WorkflowPipelineSettings : ISerializable
    {
        [SerializableProperty]
        public MaterialAsset? material { get; set; }

        [SerializableProperty]
        public float exposure { get; set; }
    }
}
