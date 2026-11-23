using System;
using System.IO;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Editor.Assets;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Extensibility.Types;
using Inno.Rendering.Assets;
using Inno.Scripting.Api;

namespace Inno.Editor.Rendering;

/// <summary>
/// Edits Pipeline sources and typed extension settings without changing canonical rendering assets before Save.
/// </summary>
[EditorModule("rendering.pipeline-documents", order: 170)]
public sealed class PipelineDocuments : EditorModule
{
    internal const string C_HISTORY = "inno.pipeline/draft";
    private readonly TypeCatalog m_types;

    internal PipelineDocuments(
        AssetPipeline assets,
        SerializationRegistry serialization,
        TypeCatalog types,
        EditorInteractions interactions
    ) {
        this.assets = assets;
        this.serialization = serialization;
        m_types = types;
        this.drafts = new(assets, serialization, interactions, "inno.pipeline", C_HISTORY, ".irenderpipeline", "Pipeline");
    }

    /// <summary>
    /// Gets the authoritative asset pipeline used by source inspectors.
    /// </summary>
    [ScriptingApiIgnore]
    public AssetPipeline assets { get; }

    /// <summary>
    /// Gets the current owner serialization registry.
    /// </summary>
    [ScriptingApiIgnore]
    public SerializationRegistry serialization { get; }

    /// <summary>
    /// Gets the source draft owner shared by Pipeline inspectors.
    /// </summary>
    [ScriptingApiIgnore]
    public AssetDraftDocuments<RenderPipelineAsset> drafts { get; }

    /// <summary>
    /// Opens a native Pipeline source in the shared document service, including failed imports.
    /// </summary>
    /// <param name="path">
    /// Mounted source path.
    /// </param>
    /// <returns>
    /// The persistent source identity used by draft operations.
    /// </returns>
    public Guid Open(AssetPath path) => drafts.Open(path);

    /// <summary>
    /// Reads a detached current-generation Pipeline value; its referenced canonical assets must not be mutated.
    /// </summary>
    /// <param name="assetID">
    /// Open Pipeline identity.
    /// </param>
    /// <returns>
    /// The unsaved native Pipeline value.
    /// </returns>
    public RenderPipelineAsset Read(Guid assetID) => drafts.Read(assetID);

    /// <summary>
    /// Changes a draft through shared History without publishing it to Scene/Game.
    /// </summary>
    /// <param name="assetID">
    /// Open Pipeline identity.
    /// </param>
    /// <param name="candidate">
    /// Detached edited value.
    /// </param>
    /// <param name="mergeKey">
    /// The current gesture key, or null for a discrete edit.
    /// </param>
    public void Replace(
        Guid assetID,
        RenderPipelineAsset candidate,
        string? mergeKey = null
    ) => drafts.Replace(assetID, candidate, mergeKey);

    /// <summary>
    /// Captures typed Pipeline settings with this source owner's complete reference and dependency context.
    /// </summary>
    /// <typeparam name="TSettings">
    /// Current plugin settings contract.
    /// </typeparam>
    /// <param name="assetID">
    /// Open Pipeline identity.
    /// </param>
    /// <param name="settings">
    /// Detached edited settings.
    /// </param>
    /// <param name="mergeKey">
    /// The current gesture key, or null for a discrete edit.
    /// </param>
    public void ReplaceSettings<TSettings>(
        Guid assetID,
        TSettings settings,
        string? mergeKey = null
    ) where TSettings : class, ISerializable
    {
        RenderPipelineAsset pipeline = Read(assetID);
        pipeline.pipelineState = new SerializedRenderExtensionState(assets.CaptureProperties(settings));
        Replace(assetID, pipeline, mergeKey);
    }

    /// <summary>
    /// Restores detached settings using the current type and serialization generation.
    /// </summary>
    /// <param name="state">
    /// Neutral settings retaining their stable type identity and properties.
    /// </param>
    /// <returns>
    /// The detached settings, or null when the extension is temporarily unavailable.
    /// </returns>
    [ScriptingApiIgnore]
    public ISerializable? RestoreSettings(SerializedRenderExtensionState state)
    {
        if (state.stableTypeId == Guid.Empty)
        {
            return null;
        }
        Type type = m_types.Resolve(new TypeRef(state.stableTypeId));
        if (Activator.CreateInstance(type) is not ISerializable settings)
        {
            throw new InvalidOperationException("Pipeline settings must be a constructible native serializable type.");
        }
        assets.RestoreProperties(state.stableTypeId, state.propertyData, settings);
        return settings;
    }

    /// <summary>
    /// Initializes this feature when its owning runtime becomes active.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnStart(EditorContext context) => drafts.Start();

    /// <summary>
    /// Advances this feature using the current runtime state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnUpdate(EditorContext context) => drafts.Update();

    /// <summary>
    /// Stops this feature before its owning runtime releases the active generation.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnStop(EditorContext context) => drafts.Dispose();

    internal static bool Recoverable(Exception error)
        => error is IOException or UnauthorizedAccessException or InvalidOperationException or ArgumentException or FormatException
           && RetirementPendingException.Find(error) is null;
}
