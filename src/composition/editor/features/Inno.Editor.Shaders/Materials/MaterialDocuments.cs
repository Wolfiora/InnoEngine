using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Serialization;
using Inno.Core.Graphs;
using Inno.Editor.Assets;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Rendering;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Draft = Inno.Editor.Assets.AssetDraftDocuments<Inno.Rendering.Assets.MaterialAsset>.Draft;
using Inno.Rendering.Assets.Authoring;

namespace Inno.Editor.Shaders;
/// <summary>
/// Owns Material authoring sessions; native source editing uses the shared asset draft lifecycle.
/// </summary>
[EditorModule("rendering.material-documents", order: 170)]
public sealed class MaterialDocuments : EditorModule
{
    internal const string C_HISTORY = "inno.material/draft";
    private readonly AssetDraftDocuments<MaterialAsset> m_documents;
    private readonly SerializationRegistry m_serialization;
    private readonly ConditionalWeakTable<ShaderAsset, PresentationCache> m_presentations = new();
    /// <summary>
    /// Gets the authoritative asset catalog used by source-aware inspectors.
    /// </summary>
    [Inno.Scripting.Api.ScriptingApiIgnore]
    public AssetPipeline assets { get; }

    internal readonly EditorInteractions interactions;
    /// <summary>
    /// Gets the generation-scoped preview service shared by material presentations.
    /// </summary>
    [Inno.Scripting.Api.ScriptingApiIgnore]
    public IEditorPreviewService previews { get; }

    internal MaterialDocuments(
        AssetPipeline assets,
        SerializationRegistry serialization,
        EditorInteractions interactions,
        IEditorPreviewService previews
    ) {
        this.assets = assets;
        this.interactions = interactions;
        this.previews = previews;
        m_serialization = serialization;
        m_documents = new(assets, serialization, interactions, "inno.material", C_HISTORY, ".imaterial", "Material");
    }

    /// <summary>
    /// Opens native source, including a source with failed import, without publishing its draft.
    /// </summary>
    /// <param name="path">
    /// Mounted Material source path.
    /// </param>
    /// <returns>
    /// The persistent identity used for subsequent draft operations.
    /// </returns>
    public Guid Open(AssetPath path) => m_documents.Open(path);
    /// <summary>
    /// Reads a detached current-generation Material; referenced assets remain canonical read-only inputs.
    /// </summary>
    /// <param name="assetId">
    /// Open Material source identity.
    /// </param>
    /// <returns>
    /// The current unsaved Material value.
    /// </returns>
    public MaterialAsset Read(Guid assetId) => m_documents.Read(assetId);
    /// <summary>
    /// Changes a draft through shared History without modifying its source or Scene/Game.
    /// </summary>
    /// <param name="assetId">
    /// Open Material source identity.
    /// </param>
    /// <param name="candidate">
    /// Detached edited Material.
    /// </param>
    /// <param name="mergeKey">
    /// The current gesture key, or null for a discrete edit.
    /// </param>
    public void Replace(
        Guid assetId,
        MaterialAsset candidate,
        string? mergeKey = null
    ) => m_documents.Replace(assetId, candidate, mergeKey);
    /// <summary>
    /// Changes compatible selected drafts within one shared gesture.
    /// </summary>
    /// <param name="candidates">
    /// Detached Material values keyed by persistent source identity.
    /// </param>
    /// <param name="mergeKey">
    /// The current gesture key, or null for a discrete edit.
    /// </param>
    public void ReplaceMany(
        IReadOnlyDictionary<Guid, MaterialAsset> candidates,
        string? mergeKey = null
    ) => m_documents.ReplaceMany(candidates, mergeKey);
    /// <summary>
    /// Opens the current source draft without publishing it.
    /// </summary>
    /// <param name="path">
    /// The isolated Material source path.
    /// </param>
    /// <returns>
    /// The generation-scoped draft; callers must not retain it across a reload.
    /// </returns>
    [Inno.Scripting.Api.ScriptingApiIgnore]
    public Draft OpenDraft(AssetPath path) => m_documents.GetDraft(Open(path));
    /// <summary>
    /// Resolves the source draft for a canonical Material identity.
    /// </summary>
    /// <param name="material">
    /// The current canonical Material whose source is edited.
    /// </param>
    /// <returns>
    /// The generation-scoped editable source draft.
    /// </returns>
    [Inno.Scripting.Api.ScriptingApiIgnore]
    public Draft Open(MaterialAsset material)
    {
        if (!assets.TryGetInfo(material.identity.persistentId, out AssetInfo? info) || info is null)
            throw new IOException("The Material source identity is unavailable.");
        return OpenDraft(info.assetPath);
    }

    /// <summary>
    /// Reads parameter presentation from the current authoring Shader graph.
    /// </summary>
    /// <param name="shader">
    /// The canonical Shader used by the Material.
    /// </param>
    /// <param name="propertyId">
    /// The stable binding identity.
    /// </param>
    /// <param name="error">
    /// Receives the source diagnostic, or an empty string when presentation is available.
    /// </param>
    /// <returns>
    /// Detached presentation values; an unavailable graph uses the ordinary presentation defaults.
    /// </returns>
    [Inno.Scripting.Api.ScriptingApiIgnore]
    public ShaderParameterPresentation Presentation(
        ShaderAsset shader,
        ShaderPropertyId propertyId,
        out string error
    ) {
        PresentationCache cache = m_presentations.GetValue(shader, static _ => new());
        if (cache.contentVersion != shader.contentVersion)
        {
            cache.contentVersion = shader.contentVersion;
            cache.graph = null;
            cache.error = "";
            try
            {
                cache.graph = ShaderGraphArtifact.ReadDocument(ShaderGraphArtifact.Read(shader, assets), m_serialization);
            }
            catch (Exception failure) when (Recoverable(failure))
            {
                cache.error = failure.Message;
            }
        }

        error = cache.error;
        return cache.graph is null ? new() : ShaderParameterPresentation.Read(cache.graph, propertyId, m_serialization, AssetSerializationContext.Create(assets));
    }

    internal void ValidateHistory(
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) => m_documents.ValidateHistory(change, direction);
    internal void ApplyHistory(
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) => m_documents.ApplyHistory(change, direction);
    internal bool TryMergeHistory(
        EditorHistoryChange older,
        EditorHistoryChange newer,
        out EditorHistoryChange? merged
    ) => m_documents.TryMergeHistory(older, newer, out merged);
    internal static bool Recoverable(Exception error) => error is IOException or UnauthorizedAccessException or InvalidOperationException or ArgumentException or FormatException
        && Inno.Core.Execution.RetirementPendingException.Find(error) is null;
    /// <summary>
    /// Initializes this feature when its owning runtime becomes active.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnStart(EditorContext context) => m_documents.Start();
    /// <summary>
    /// Advances this feature using the current runtime state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnUpdate(EditorContext context) => m_documents.Update();
    /// <summary>
    /// Stops this feature before its owning runtime releases the active generation.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnStop(EditorContext context)
    {
        m_presentations.Clear();
        m_documents.Dispose();
    }

    private sealed class PresentationCache
    {
        internal long contentVersion = -1;
        internal GraphDocument? graph;
        internal string error = "";
    }
}

[EditorHistoryHandler(MaterialDocuments.C_HISTORY)]
internal sealed class MaterialDraftHistory(MaterialDocuments documents) : EditorHistoryHandler
{
    /// <summary>
    /// Evaluates whether the requested change can be applied to the current generation.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="change">
    /// The neutral change payload to query or apply.
    /// </param>
    /// <param name="direction">
    /// The history direction that determines which state is applied.
    /// </param>
    /// <returns>
    /// The validated editor history availability that represents the completed operation.
    /// </returns>
    protected override EditorHistoryAvailability Query(
        EditorHistoryContext context,
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) {
        try
        {
            documents.ValidateHistory(change, direction);
            return EditorHistoryAvailability.Available();
        }
        catch (Exception error) when (MaterialDocuments.Recoverable(error))
        {
            return EditorHistoryAvailability.Unavailable(error.Message);
        }
    }

    /// <summary>
    /// Applies a validated change atomically at the caller-controlled commit point.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="change">
    /// The neutral change payload to query or apply.
    /// </param>
    /// <param name="direction">
    /// The history direction that determines which state is applied.
    /// </param>
    /// <returns>
    /// The validated editor history result that represents the completed operation.
    /// </returns>
    protected override EditorHistoryResult Apply(
        EditorHistoryContext context,
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) {
        try
        {
            documents.ApplyHistory(change, direction);
            return EditorHistoryResult.Success();
        }
        catch (Exception error) when (MaterialDocuments.Recoverable(error))
        {
            return EditorHistoryResult.Failure(error.Message);
        }
    }

    /// <inheritdoc/>
    protected override bool TryMerge(
        EditorHistoryChange older,
        EditorHistoryChange newer,
        out EditorHistoryChange? merged
    ) => documents.TryMergeHistory(older, newer, out merged);
}
