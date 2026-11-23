using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Execution;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Editor.Assets;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Rendering;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;
using Inno.Scripting.Api;

using MaterialDraft = Inno.Editor.Assets.AssetDraftDocuments<Inno.Rendering.Assets.MaterialAsset>.Draft;

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
    internal readonly EditorInteractions interactions;

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
    /// Gets the authoritative asset catalog used by source-aware inspectors.
    /// </summary>
    [ScriptingApiIgnore]
    public AssetPipeline assets { get; }

    /// <summary>
    /// Gets the generation-scoped preview service shared by material presentations.
    /// </summary>
    [ScriptingApiIgnore]
    public IEditorPreviewService previews { get; }

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
    /// Resolves the source draft for a canonical Material identity.
    /// </summary>
    /// <param name="material">
    /// The current canonical Material whose source is edited.
    /// </param>
    /// <returns>
    /// The generation-scoped editable source draft.
    /// </returns>
    [ScriptingApiIgnore]
    public MaterialDraft Open(MaterialAsset material)
    {
        if (!assets.TryGetInfo(material.identity.persistentID, out AssetInfo? info) || info is null)
        {
            throw new IOException("The Material source identity is unavailable.");
        }
        return OpenDraft(info.assetPath);
    }

    /// <summary>
    /// Reads a detached current-generation Material; referenced assets remain canonical read-only inputs.
    /// </summary>
    /// <param name="assetID">
    /// Open Material source identity.
    /// </param>
    /// <returns>
    /// The current unsaved Material value.
    /// </returns>
    public MaterialAsset Read(Guid assetID) => m_documents.Read(assetID);

    /// <summary>
    /// Changes a draft through shared History without modifying its source or Scene/Game.
    /// </summary>
    /// <param name="assetID">
    /// Open Material source identity.
    /// </param>
    /// <param name="candidate">
    /// Detached edited Material.
    /// </param>
    /// <param name="mergeKey">
    /// The current gesture key, or null for a discrete edit.
    /// </param>
    public void Replace(
        Guid assetID,
        MaterialAsset candidate,
        string? mergeKey = null
    ) => m_documents.Replace(assetID, candidate, mergeKey);

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
    [ScriptingApiIgnore]
    public MaterialDraft OpenDraft(AssetPath path) => m_documents.GetDraft(Open(path));

    /// <summary>
    /// Reads parameter presentation from the current authoring Shader graph.
    /// </summary>
    /// <param name="shader">
    /// The canonical Shader used by the Material.
    /// </param>
    /// <param name="propertyID">
    /// The stable binding identity.
    /// </param>
    /// <param name="error">
    /// Receives the source diagnostic, or an empty string when presentation is available.
    /// </param>
    /// <returns>
    /// Detached presentation values; an unavailable graph uses the ordinary presentation defaults.
    /// </returns>
    [ScriptingApiIgnore]
    public ShaderParameterPresentation Presentation(
        ShaderAsset shader,
        ShaderPropertyID propertyID,
        out string error
    ) {
        PresentationCache cache = m_presentations.GetValue(shader, static _ => new PresentationCache());
        if (cache.contentVersion != shader.contentVersion)
        {
            cache.contentVersion = shader.contentVersion;
            cache.graph = null;
            cache.error = string.Empty;
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
        return cache.graph is null
               ? new ShaderParameterPresentation()
               : ShaderParameterPresentation.Read(cache.graph, propertyID, m_serialization, AssetSerializationContext.Create(assets));
    }

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

    internal static bool Recoverable(Exception error)
        => error is IOException or UnauthorizedAccessException or InvalidOperationException or ArgumentException or FormatException
           && RetirementPendingException.Find(error) is null;

    private sealed class PresentationCache
    {
        internal long contentVersion = -1;
        internal GraphDocument? graph;
        internal string error = string.Empty;
    }
}
