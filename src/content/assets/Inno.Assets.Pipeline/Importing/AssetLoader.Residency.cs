using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Diagnostics;
using Inno.Core.Identity;
using Inno.References;
using Inno.Core.Execution;
using Inno.Core.IO;
using Inno.Core.Logging;
using Inno.Extensibility.Types;
using Inno.Core.Serialization;
using Inno.Core.Collections;

using IOFile = System.IO.File;

namespace Inno.Assets.Pipeline;

sealed partial class AssetLoader
{
    /// <summary>
    /// Collects unreachable content-addressed artifacts.
    /// </summary>
    /// <param name="gracePeriod">
    /// The minimum age of an unreachable bundle.
    /// </param>
    /// <param name="maximumSizeBytes">
    /// The cache size limit, or zero for no limit.
    /// </param>
    /// <returns>
    /// The number of removed artifact bundles.
    /// </returns>
    public int CollectArtifacts(
        TimeSpan gracePeriod,
        long maximumSizeBytes
    ) {
        return Execute(() =>
        {
            HashSet<string> reachable = [];
            foreach (AssetArtifactKey retained in m_artifactRetention.GetRetainedKeys())
                AddArtifactKey(reachable, retained.value);
            foreach (AssetRecord record in m_recordsByPath.Values)
            {
                AddArtifactKey(reachable, record.meta.artifactKey);
                AddArtifactKey(reachable, record.meta.lastSuccessfulArtifactKey);
            }
            return m_artifacts.Collect(reachable, gracePeriod, maximumSizeBytes);
        });
    }

    /// <summary>
    /// Tries to resolve a named output from the current artifact bundle.
    /// </summary>
    /// <param name="persistentId">
    /// The stable persistent identity used for lookup.
    /// </param>
    /// <param name="outputName">
    /// The stable artifact output name used for lookup.
    /// </param>
    /// <param name="artifact">
    /// The resolved immutable artifact payload returned to the caller.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetArtifact(
        Guid persistentId,
        string outputName,
        out AssetArtifactInfo? artifact
    ) {
        AssetArtifactInfo? result = Execute(() =>
        {
            if (!m_recordsById.TryGetValue(persistentId, out AssetRecord? record))
                return null;
            return m_artifacts.TryGet(
                new AssetArtifactKey(record.meta.artifactKey),
                outputName,
                out AssetArtifactInfo? found)
                ? found
                : null;
        });
        artifact = result;
        return result is not null;
    }

    /// <summary>
    /// Acquires an immutable output while excluding concurrent artifact collection.
    /// </summary>
    /// <param name="persistentId">
    /// The source asset's persistent identity.
    /// </param>
    /// <param name="outputName">
    /// The required output protocol.
    /// </param>
    /// <returns>
    /// A lease retaining this exact artifact key, even after reimport or source deletion.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// No current output can be resolved.
    /// </exception>
    public ArtifactLease AcquireArtifact(
        Guid persistentId,
        string outputName
    )
        => Execute(() =>
        {
            if (!TryGetArtifact(persistentId, outputName, out AssetArtifactInfo? artifact) || artifact is null)
                throw new InvalidOperationException($"Asset '{persistentId:D}' has no output '{outputName}'.");
            return m_artifactRetention.Retain(artifact, m_artifacts.CreateReadFactory(artifact));
        });

    /// <summary>
    /// Collects canonical assets that have no external managed references.
    /// </summary>
    /// <returns>
    /// The number of released canonical assets.
    /// </returns>
    public int UnloadUnusedAssets() => Execute(UnloadUnusedAssetsLocked);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private int UnloadUnusedAssetsLocked()
    {
        SweepCandidate[] candidates = DetachSweepCandidatesLocked();
        if (candidates.Length == 0)
            return 0;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
        GC.WaitForPendingFinalizers();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);

        int released = 0;
        foreach (SweepCandidate candidate in candidates)
        {
            if (candidate.reference.TryGetTarget(out AssetObject? survivor))
            {
                candidate.record.asset = survivor;
                candidate.record.lastSweepReachability = true;
                continue;
            }
            candidate.record.lastSweepReachability = false;
            released++;
        }
        return released;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private SweepCandidate[] DetachSweepCandidatesLocked()
    {
        var result = new List<SweepCandidate>();
        foreach (AssetRecord record in m_recordsByPath.Values)
        {
            if (record.asset is null)
                continue;
            result.Add(new SweepCandidate(record, new WeakReference<AssetObject>(record.asset)));
            record.asset = null;
        }
        return result.ToArray();
    }

    private static void AddArtifactKey(
        HashSet<string> reachable,
        string value
    ) {
        if (!string.IsNullOrWhiteSpace(value))
            reachable.Add(value.ToUpperInvariant());
    }

    private readonly record struct SweepCandidate(
        AssetRecord record,
        WeakReference<AssetObject> reference
    );

}
