using System;
using System.Collections.Generic;
using System.IO;
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
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Core;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;
using Inno.Editor.Shaders;
using Inno.Editor.Shaders.ImGui;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Shaders;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using EngineColor = Inno.Core.Mathematics.Color;
using EngineVector4 = Inno.Core.Mathematics.Vector4;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.Shaders.Native.Tests;

public sealed class MaterialEditorWorkflowTests : ShaderEditorFixture
{
    [Fact]
    public void MaterialGesturesAreIndependentAndOnlySavePublishesTheDraft()
    {
        (AssetPath path, ShaderPropertyID property) = CreateMaterial();
        MaterialDocuments documents = Assert.IsType<MaterialDocuments>(m_sink.materials);
        Guid id = documents.Open(path);
        byte[] original = File.ReadAllBytes(Path.Combine(m_root, "Assets", path.localPath));
        MaterialAsset canonical = m_assets.Load<MaterialAsset>(path);
        void Set(
            float value,
            string? mergeKey = null
        ) {
            MaterialAsset candidate = documents.Read(id);
            candidate.Set(property, MaterialValue.FromColor(new EngineColor(value, 1, 1, 1)));
            documents.Replace(id, candidate, mergeKey);
        }

        using (var gesture = new EditorEditGesture(m_runtime.interactions.history, "First Color"))
        {
            Set(0.1f, gesture.mergeKey);
            Set(0.2f, gesture.mergeKey);
            gesture.Commit();
        }

        using (var gesture = new EditorEditGesture(m_runtime.interactions.history, "Second Color"))
        {
            Set(0.3f, gesture.mergeKey);
            Set(0.4f, gesture.mergeKey);
            gesture.Commit();
        }

        Assert.False(canonical.TryGet(property, out _));
        Assert.Equal(original, File.ReadAllBytes(Path.Combine(m_root, "Assets", path.localPath)));
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.True(documents.Read(id).TryGet(property, out MaterialValue value));
        Assert.Equal(0.2f, value.vector.x);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.False(documents.Read(id).TryGet(property, out _));
        Assert.False(Assert.Single(m_runtime.interactions.documents.documents).isDirty);
        Assert.True(m_runtime.interactions.history.Redo().succeeded);
        EditorDocumentContext context = Assert.Single(m_runtime.interactions.documents.documents);
        Assert.True(m_runtime.interactions.documents.Save(context.documentID));
        Assert.False(canonical.TryGet(property, out _));
        Tick();
        Assert.True(canonical.TryGet(property, out value));
        Assert.Equal(0.2f, value.vector.x);
        Set(0.8f);
        Assert.True(m_runtime.interactions.documents.Revert(context.documentID));
        Assert.True(documents.Read(id).TryGet(property, out value));
        Assert.Equal(0.2f, value.vector.x);
    }

    [Fact]
    public void CancelledMaterialGestureRestoresDraftDirtyStateAndLeavesNoUndoEntry()
    {
        (AssetPath path, ShaderPropertyID property) = CreateMaterial();
        MaterialDocuments documents = Assert.IsType<MaterialDocuments>(m_sink.materials);
        Guid id = documents.Open(path);
        Assert.False(m_runtime.interactions.history.canUndo);
        using var gesture = new EditorEditGesture(m_runtime.interactions.history, "Cancelled Material Edit");
        MaterialAsset candidate = documents.Read(id);
        candidate.Set(property, MaterialValue.FromColor(new EngineColor(0.7f, 1, 1, 1)));
        documents.Replace(id, candidate, gesture.mergeKey);
        Assert.True(Assert.Single(m_runtime.interactions.documents.documents).isDirty);
        Assert.True(gesture.Cancel().succeeded);
        Assert.False(documents.Read(id).TryGet(property, out _));
        Assert.False(Assert.Single(m_runtime.interactions.documents.documents).isDirty);
        Assert.False(m_runtime.interactions.history.canUndo);
    }

    [Fact]
    public void MaterialSaveConflictKeepsDiskAndRecoveredDraftDistinct()
    {
        (AssetPath path, ShaderPropertyID property) = CreateMaterial();
        MaterialDocuments documents = Assert.IsType<MaterialDocuments>(m_sink.materials);
        Guid id = documents.Open(path);
        MaterialAsset candidate = documents.Read(id);
        candidate.Set(property, MaterialValue.FromColor(new EngineColor(0.3f, 1, 1, 1)));
        documents.Replace(id, candidate);
        AssetSourceStore sources = m_assets.CreateSourceStore();
        AssetSourceSnapshot before = sources.Read(path);
        MaterialAsset external = sources.Decode<MaterialAsset>(before.bytes);
        external.Set(property, MaterialValue.FromColor(new EngineColor(0.9f, 1, 1, 1)));
        byte[] externalBytes = sources.Encode(external);
        sources.Save(path, externalBytes, before.contentHash);
        EditorDocumentContext context = Assert.Single(m_runtime.interactions.documents.documents);
        Assert.False(m_runtime.interactions.documents.Save(context.documentID));
        Assert.Equal(externalBytes, sources.Read(path).bytes);
        m_runtime.Dispose();
        m_runtime = CreateRuntime();
        documents = Assert.IsType<MaterialDocuments>(m_sink.materials);
        documents.Open(path);
        Assert.True(documents.Read(id).TryGet(property, out MaterialValue recovered));
        Assert.Equal(0.3f, recovered.vector.x);
        Assert.Equal(externalBytes, sources.Read(path).bytes);
    }

    [Fact]
    public void CleanMaterialFollowsExternalSourceButDirtyMaterialKeepsItsDraft()
    {
        (AssetPath path, ShaderPropertyID property) = CreateMaterial();
        MaterialDocuments documents = Assert.IsType<MaterialDocuments>(m_sink.materials);
        Guid id = documents.Open(path);
        void External(float value)
        {
            AssetSourceStore sources = m_assets.CreateSourceStore();
            AssetSourceSnapshot captured = sources.Read(path);
            MaterialAsset material = sources.Decode<MaterialAsset>(captured.bytes);
            material.Set(property, MaterialValue.FromColor(new EngineColor(value, 1, 1, 1)));
            sources.Save(path, sources.Encode(material), captured.contentHash);
            Assert.True(m_assets.Import(path));
            Tick();
        }

        External(0.2f);
        Assert.True(documents.Read(id).TryGet(property, out MaterialValue inherited));
        Assert.Equal(0.2f, inherited.vector.x);
        Assert.False(Assert.Single(m_runtime.interactions.documents.documents).isDirty);
        MaterialAsset candidate = documents.Read(id);
        candidate.Set(property, MaterialValue.FromColor(new EngineColor(0.5f, 1, 1, 1)));
        documents.Replace(id, candidate);
        External(0.8f);
        Assert.True(documents.Read(id).TryGet(property, out MaterialValue draft));
        Assert.Equal(0.5f, draft.vector.x);
        Assert.True(Assert.Single(m_runtime.interactions.documents.documents).isDirty);
        Assert.False(m_runtime.interactions.documents.Save(Assert.Single(m_runtime.interactions.documents.documents).documentID));
    }

    [Fact]
    public void MultipleMaterialSamplesCommitAsOneIndependentUndoWithoutPublishing()
    {
        (AssetPath firstPath, ShaderPropertyID property) = CreateMaterial();
        AssetPath secondPath = AssetPath.Project("SecondSurface.imaterial");
        AssetSourceStore sources = m_assets.CreateSourceStore();
        sources.Save(secondPath, sources.Read(firstPath).bytes, null);
        Assert.True(m_assets.Import(secondPath));
        MaterialDocuments documents = Assert.IsType<MaterialDocuments>(m_sink.materials);
        Guid first = documents.Open(firstPath);
        Guid second = documents.Open(secondPath);
        byte[] original = sources.Read(firstPath).bytes;
        void Sample(
            float value,
            string? mergeKey
        ) {
            var candidates = new Dictionary<Guid, MaterialAsset>();
            foreach (Guid id in new[]
            {
                first,
                second
            }

            ) {
                MaterialAsset candidate = documents.Read(id);
                candidate.Set(property, MaterialValue.FromColor(new EngineColor(value, 1, 1, 1)));
                candidates.Add(id, candidate);
            }

            documents.ReplaceMany(candidates, mergeKey);
        }

        using (var gesture = new EditorEditGesture(m_runtime.interactions.history, "First Selection Color"))
        {
            Sample(0.2f, gesture.mergeKey);
            Sample(0.4f, gesture.mergeKey);
            gesture.Commit();
        }

        using (var gesture = new EditorEditGesture(m_runtime.interactions.history, "Second Selection Color"))
        {
            Sample(0.6f, gesture.mergeKey);
            gesture.Commit();
        }
        Assert.True(m_runtime.interactions.history.canUndo);

        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        foreach (Guid id in new[]
        {
            first,
            second
        }

        ) {
            Assert.True(documents.Read(id).TryGet(property, out MaterialValue value));
            Assert.Equal(0.4f, value.vector.x);
        }

        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        foreach (Guid id in new[]
        {
            first,
            second
        }

        ) {
            Assert.False(documents.Read(id).TryGet(property, out _));
        }
        Assert.All(m_runtime.interactions.documents.documents, document => Assert.False(document.isDirty));
        Assert.Equal(original, sources.Read(firstPath).bytes);
        Assert.Equal(original, sources.Read(secondPath).bytes);
        Assert.True(m_runtime.interactions.history.Redo().succeeded);
        foreach (Guid id in new[]
        {
            first,
            second
        }

        ) {
            Assert.True(documents.Read(id).TryGet(property, out _));
        }
    }

    [Fact]
    public void CommonMaterialEditingPreservesUntouchedMixedComponentsAndTextureReferences()
    {
        MaterialValue first = MaterialValue.FromVector(new EngineVector4(1, 2, 3, 4));
        MaterialValue edited = MaterialValue.FromVector(new EngineVector4(8, 2, 3, 4));
        MaterialValue other = MaterialValue.FromVector(new EngineVector4(5, 6, 7, 9));
        MaterialValue result = ShaderPropertyInspector.ApplyEdit(ShaderPropertyType.Vector4, first, edited, other);
        Assert.Equal(new EngineVector4(8, 6, 7, 9), result.vector);
        var firstTexture = new TextureAsset(1, 1, TextureColorSpace.Linear, "first");
        var secondTexture = new TextureAsset(1, 1, TextureColorSpace.Linear, "second");
        first = MaterialValue.FromTexture(firstTexture);
        edited = first;
        edited.sampler = new(
            RenderSamplerFilter.Point,
            RenderSamplerAddressMode.Clamp,
            RenderSamplerAddressMode.Clamp,
            RenderSamplerAddressMode.Clamp
        );
        other = MaterialValue.FromTexture(secondTexture);
        result = ShaderPropertyInspector.ApplyEdit(ShaderPropertyType.Texture2D, first, edited, other);
        Assert.Same(secondTexture, result.texture);
        Assert.Equal(RenderSamplerFilter.Point, result.sampler.filter);
    }
}
