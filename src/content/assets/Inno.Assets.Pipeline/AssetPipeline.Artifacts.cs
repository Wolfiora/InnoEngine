using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Extensibility.Modules;
using Inno.Core.Identity;
using Inno.Core.Diagnostics;
using Inno.Core.Logging;
using Inno.Extensibility.Types;
using Inno.Scripting.Api;
using Inno.Core.Serialization;
using Inno.Core.Execution;
using Inno.Extensibility.Reload;

namespace Inno.Assets.Pipeline;

sealed partial class AssetPipeline
{
    /// <summary>
    /// Tries to resolve a named artifact output.
    /// </summary>
    /// <param name="persistentId">
    /// The artifact owner identity.
    /// </param>
    /// <param name="outputName">
    /// The named output.
    /// </param>
    /// <param name="artifact">
    /// The output information when available.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the output exists.
    /// </returns>
    public bool TryGetArtifact(
        Guid persistentId,
        string outputName,
        out AssetArtifactInfo? artifact
    )
        => GetLoader().TryGetArtifact(persistentId, outputName, out artifact);

    /// <summary>
    /// Acquires one verified authoring artifact generation for an explicit lifetime.
    /// </summary>
    /// <param name="persistentId">
    /// Persistent identity of the artifact owner.
    /// </param>
    /// <param name="outputName">
    /// Stable artifact output name.
    /// </param>
    /// <returns>
    /// A lease over the currently committed immutable artifact generation.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the requested artifact is unavailable.
    /// </exception>
    public ArtifactLease AcquireArtifact(
        Guid persistentId,
        string outputName
    ) => GetLoader().AcquireArtifact(persistentId, outputName);

    /// <summary>
    /// Runs an aggregate asset build using the processor registered for a definition.
    /// </summary>
    /// <param name="definition">
    /// The build definition asset.
    /// </param>
    /// <param name="inputs">
    /// The immutable input catalog snapshots.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation for the candidate build.
    /// </param>
    /// <returns>
    /// The content-addressed output bundle key.
    /// </returns>
    public ValueTask<AssetArtifactKey> BuildAsync(
        AssetObject definition,
        IReadOnlyList<AssetInfo> inputs,
        CancellationToken cancellationToken = default
    ) {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        return GetLoader().BuildAsync(definition, inputs, cancellationToken);
    }

    /// <summary>
    /// Exports the current source-free runtime catalog and its exact artifact closure.
    /// </summary>
    /// <param name="destinationContentRoot">
    /// Empty destination for the deployed asset database.
    /// </param>
    /// <returns>
    /// Counts and source identities for the deployed runtime snapshot.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when runtime artifacts are incomplete, a deployed reference is authoring-only,
    /// or a transitive authoring input has failed or changed since import. Last-good inputs do not certify an export.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown when the destination is not empty or cannot be written.
    /// </exception>
    [ScriptingApiIgnore]
    public AssetRuntimeContentInfo ExportRuntimeArtifacts(string destinationContentRoot)
    {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        WaitForIdle();
        using SerializationGeneration serialization = m_serialization.CaptureGeneration();
        return GetLoader().ExportRuntimeArtifacts(
            destinationContentRoot,
            serialization,
            CancellationToken.None);
    }

    /// <summary>
    /// Exports a runtime-only artifact snapshot on a worker without loading artifact files into memory.
    /// </summary>
    /// <remarks>
    /// Keeps generation admission and serialization pinned until the worker completes, fails or cancels.
    /// A pending or faulted generation cannot start a new export.
    /// </remarks>
    /// <param name="destinationContentRoot">
    /// The empty destination that receives the runtime asset database.
    /// </param>
    /// <param name="cancellationToken">
    /// The token checked while individual artifact files are copied.
    /// </param>
    /// <returns>
    /// A task that completes with counts and source identities for the captured runtime snapshot.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when runtime artifacts are incomplete, transitive authoring inputs have failed or become stale,
    /// or the asset service is not available.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when cancellation is requested before export completes.
    /// </exception>
    [ScriptingApiIgnore]
    public async Task<AssetRuntimeContentInfo> ExportRuntimeArtifactsAsync(
        string destinationContentRoot,
        CancellationToken cancellationToken = default
    ) {
        EnsureOwnerThread();
        using IDisposable operationScope = m_generations.AcquireRead("export runtime artifacts");
        WaitForIdle();
        AssetLoader loader = GetLoader();
        using SerializationGeneration serialization = m_serialization.CaptureGeneration();
        return await Task.Run(
            () => loader.ExportRuntimeArtifacts(destinationContentRoot, serialization, cancellationToken),
            CancellationToken.None).ConfigureAwait(false);
    }

    private void CollectArtifactsIfDue(
        AssetLoader loader,
        bool force
    ) {
        if (m_options.mode == AssetPipelineMode.RuntimeArtifacts)
            return;
        long now = Environment.TickCount64;
        if (!force && now - m_lastArtifactCollectionTimestamp < 60_000)
            return;
        m_lastArtifactCollectionTimestamp = now;
        _ = loader.CollectArtifacts(
            m_cacheOptions.garbageCollectionGracePeriod,
            m_cacheOptions.maximumSizeBytes);
    }

}
