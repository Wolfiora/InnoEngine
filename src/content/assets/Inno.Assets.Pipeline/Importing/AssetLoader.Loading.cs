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
    /// Loads a canonical asset by isolated source path.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="requestedAssetType">
    /// The required assignable asset type.
    /// </param>
    /// <returns>
    /// The canonical asset, or <see langword="null"/> when unavailable or incompatible.
    /// </returns>
    public AssetObject? Load(
        AssetPath path,
        Type requestedAssetType
    ) {
        ArgumentNullException.ThrowIfNull(requestedAssetType);
        return Execute(() => LoadPathLocked(NormalizeAssetPath(path), requestedAssetType));
    }

    /// <summary>
    /// Tries to load a canonical asset by isolated source path.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="requestedAssetType">
    /// The required assignable asset type.
    /// </param>
    /// <param name="asset">
    /// The canonical asset when successful.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a compatible asset was loaded.
    /// </returns>
    public bool TryLoad(
        AssetPath path,
        Type requestedAssetType,
        out AssetObject? asset
    ) {
        asset = Load(path, requestedAssetType);
        return asset is not null;
    }

    /// <summary>
    /// Loads a canonical asset by persistent identity.
    /// </summary>
    /// <param name="persistentId">
    /// The persistent asset identity.
    /// </param>
    /// <param name="requestedAssetType">
    /// The required assignable asset type.
    /// </param>
    /// <returns>
    /// The canonical asset, or <see langword="null"/> when unavailable or incompatible.
    /// </returns>
    public AssetObject? Load(
        Guid persistentId,
        Type requestedAssetType
    ) {
        ArgumentNullException.ThrowIfNull(requestedAssetType);
        return Execute(() => LoadIdLocked(persistentId, requestedAssetType));
    }

    /// <summary>
    /// Tries to load a canonical asset by persistent identity.
    /// </summary>
    /// <param name="persistentId">
    /// The persistent asset identity.
    /// </param>
    /// <param name="requestedAssetType">
    /// The required assignable asset type.
    /// </param>
    /// <param name="asset">
    /// The canonical asset when successful.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a compatible asset was loaded.
    /// </returns>
    public bool TryLoad(
        Guid persistentId,
        Type requestedAssetType,
        out AssetObject? asset
    ) {
        asset = Load(persistentId, requestedAssetType);
        return asset is not null;
    }

    /// <summary>
    /// Asynchronously loads a canonical asset by isolated source path.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="requestedAssetType">
    /// The required assignable asset type.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation for the current caller's wait.
    /// </param>
    /// <returns>
    /// The canonical asset, or <see langword="null"/> when unavailable or incompatible.
    /// </returns>
    public ValueTask<AssetObject?> LoadAsync(
        AssetPath path,
        Type requestedAssetType,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(requestedAssetType);
        string normalized = NormalizeAssetPath(path);
        Task<AssetObject?> operation;
        lock (m_asyncSync)
        {
            ObjectDisposedException.ThrowIf(m_disposeRequested, this);
            if (!m_inFlightPathLoads.TryGetValue(normalized, out operation!))
            {
                operation = Task.Run(() => Load(AssetPath.Parse(normalized), typeof(AssetObject)));
                m_inFlightPathLoads.Add(normalized, operation);
                _ = operation.ContinueWith(
                    _ => RemovePathOperation(normalized, operation),
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
            }
        }
        return AwaitSharedLoad(operation, requestedAssetType, cancellationToken);
    }

    /// <summary>
    /// Asynchronously loads a canonical asset by persistent identity.
    /// </summary>
    /// <param name="persistentId">
    /// The persistent asset identity.
    /// </param>
    /// <param name="requestedAssetType">
    /// The required assignable asset type.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation for the current caller's wait.
    /// </param>
    /// <returns>
    /// The canonical asset, or <see langword="null"/> when unavailable or incompatible.
    /// </returns>
    public ValueTask<AssetObject?> LoadAsync(
        Guid persistentId,
        Type requestedAssetType,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(requestedAssetType);
        Task<AssetObject?> operation;
        lock (m_asyncSync)
        {
            ObjectDisposedException.ThrowIf(m_disposeRequested, this);
            if (!m_inFlightIdLoads.TryGetValue(persistentId, out operation!))
            {
                operation = Task.Run(() => Load(persistentId, typeof(AssetObject)));
                m_inFlightIdLoads.Add(persistentId, operation);
                _ = operation.ContinueWith(
                    _ => RemoveIdOperation(persistentId, operation),
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
            }
        }
        return AwaitSharedLoad(operation, requestedAssetType, cancellationToken);
    }

    /// <summary>
    /// Resolves a serialized reference or creates a persistent missing placeholder.
    /// </summary>
    /// <param name="persistentId">
    /// The referenced persistent identity.
    /// </param>
    /// <param name="stableTypeId">
    /// The referenced stable asset type identity.
    /// </param>
    /// <param name="lastKnownPath">
    /// The last known source-relative path.
    /// </param>
    /// <param name="expectedType">
    /// The declared destination type.
    /// </param>
    /// <returns>
    /// A compatible canonical asset or missing placeholder.
    /// </returns>
    public AssetObject ResolveReference(
        Guid persistentId,
        Guid stableTypeId,
        string lastKnownPath,
        Type expectedType
    ) {
        ArgumentNullException.ThrowIfNull(expectedType);
        if (persistentId == Guid.Empty)
            throw new InvalidOperationException("A serialized asset reference has an empty persistent identity.");
        return Execute(() => ResolveReferenceLocked(persistentId, stableTypeId, lastKnownPath, expectedType));
    }

    AssetObject IAssetReferenceResolver.Resolve(
        Guid persistentId,
        Guid stableTypeId,
        string lastKnownPath,
        Type expectedType,
        string propertyPath
    ) {
        try
        {
            return ResolveReference(persistentId, stableTypeId, lastKnownPath, expectedType);
        }
        catch (AssetImportExtensionUnavailableException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                $"Asset reference '{persistentId:D}' at '{propertyPath}' cannot be resolved as " +
                $"'{expectedType.FullName}'.",
                exception);
        }
    }

    private AssetObject? LoadPathLocked(
        string relativePath,
        Type requestedAssetType
    ) {
        AssetRecord? record = FindRecordLocked(relativePath);
        bool sourceExists = IOFile.Exists(GetSourcePath(relativePath));
        bool stale = false;
        bool catalogChanged = false;
        if (record is not null && sourceExists)
            stale = IsStale(record, out catalogChanged);
        if (catalogChanged && !stale)
            CommitCatalogLocked();
        if (record is null || stale)
        {
            if (!ImportLocked(relativePath))
            {
                record = FindRecordLocked(relativePath);
                return record is null ? null : LoadRecordLocked(record, requestedAssetType);
            }
            record = FindRecordLocked(relativePath);
        }
        return record is null ? null : LoadRecordLocked(record, requestedAssetType);
    }

    private AssetObject? LoadIdLocked(
        Guid persistentId,
        Type requestedAssetType
    ) {
        if (persistentId == Guid.Empty)
            return null;
        if (!m_recordsById.TryGetValue(persistentId, out AssetRecord? record))
        {
            LoadCatalogLocked();
            if (!m_recordsById.TryGetValue(persistentId, out record))
                return null;
        }
        if (record.meta.isTombstone)
        {
            return record.asset is null
                ? null
                : LoadRecordLocked(record, requestedAssetType);
        }

        // A source-side identity can be indexed before its body is imported. Route identity loads
        // through the same freshness gate as path loads so pending records never hydrate an empty
        // or stale artifact merely because their sidecar was discovered first. The sidecar check
        // keeps a newly created asset at the same path from satisfying an older identity lookup.
        bool ownsCurrentSource = IOFile.Exists(GetSourcePath(record.relativePath))
            && TryReadSourceMeta(GetMetaPath(record.relativePath), out AssetSourceMeta sourceMeta)
            && sourceMeta.persistentId == persistentId;
        return ownsCurrentSource
            ? LoadPathLocked(record.relativePath, requestedAssetType)
            : LoadRecordLocked(record, requestedAssetType);
    }

    private AssetObject? LoadRecordLocked(
        AssetRecord record,
        Type requestedAssetType
    ) {
        if (string.IsNullOrEmpty(record.meta.artifactKey) && record.meta.assetStateBytes.Length == 0)
        {
            if (m_activeImports.Count != 0 && m_unavailableImports.TryGetValue(record.relativePath, out var unavailable))
                throw new AssetImportExtensionUnavailableException(unavailable.kind, unavailable.id);
            return null;
        }
        Type? actualType = ResolveRecordType(record);
        if (actualType is null || !requestedAssetType.IsAssignableFrom(actualType))
            return null;
        if (record.asset is not null)
            return requestedAssetType.IsInstanceOfType(record.asset) ? record.asset : null;

        var transaction = new AssetLoadTransaction();
        try
        {
            PrepareShellsLocked(record, transaction);
            foreach (AssetRecord created in transaction.createdRecords)
                HydrateRecordLocked(created);
            foreach (AssetRecord created in transaction.createdRecords)
                AttachDependenciesLocked(created);
            return record.asset;
        }
        catch
        {
            RollbackLoadLocked(transaction);
            throw;
        }
    }

    private void PrepareShellsLocked(
        AssetRecord record,
        AssetLoadTransaction transaction
    ) {
        if (record.asset is not null)
            return;
        Type type = ResolveRecordType(record)
            ?? throw new InvalidOperationException(
                $"Asset '{record.relativePath}' has unknown stable type '{record.stableTypeId}'.");
        AssetObject shell;
        try
        {
            shell = (AssetObject)(Activator.CreateInstance(type, nonPublic: true)
                ?? throw new InvalidOperationException("Activator returned null."));
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                $"Asset type '{type.FullName}' requires a parameterless constructor.", exception);
        }
        m_identities.InitializePersistentIdentity(shell, record.persistentId);
        if (m_identitiesActive)
            m_identities.Register(shell, record.persistentId);
        record.asset = shell;
        transaction.createdRecords.Add(record);

        foreach (AssetDependency dependency in GetDirectDependencies(record.meta))
        {
            AssetRecord? dependencyRecord = FindDependencyRecordLocked(dependency);
            // A valid last-good dependency may belong to a module generation that has not
            // activated yet. Do not let that temporary type gap abort the entire object graph;
            // AttachDependenciesLocked installs the identity-preserving placeholder and a later
            // registry generation rehydrates the reference from the retained catalog record.
            if (dependencyRecord is not null && ResolveRecordType(dependencyRecord) is not null &&
                (dependencyRecord.meta.assetStateBytes.Length != 0 || !string.IsNullOrEmpty(dependencyRecord.meta.artifactKey)))
                PrepareShellsLocked(dependencyRecord, transaction);
        }
    }

    private void HydrateRecordLocked(AssetRecord record)
    {
        AssetObject asset = record.asset!;
        RestoreAssetState(asset, record.meta.assetStateBytes);
        byte[] payload = m_artifacts.Read(
            new AssetArtifactKey(record.meta.artifactKey),
            "runtime");
        if (payload.Length == 0 && record.payload.Length > 0)
            payload = record.payload;
        record.payload = payload;
        bool isMissing = record.meta.importStatus == (int)AssetImportStatus.Missing;
        m_runtimeOwner.Initialize(
            asset,
            AssetPath.Parse(record.relativePath),
            record.meta.sourceHash,
            payload,
            isMissing,
            1);
    }

    private AssetObject ResolveReferenceLocked(
        Guid persistentId,
        Guid stableTypeId,
        string lastKnownPath,
        Type expectedType
    ) {
        AssetObject? loaded = LoadIdLocked(persistentId, expectedType);
        if (loaded is not null)
            return loaded;
        // A cataloged source whose importer/type belongs to a later module generation is
        // unavailable, not missing. Startup and atomic reload may legitimately observe this
        // state before the corresponding generation is activated; publishing a missing-reference
        // diagnostic here would turn a healthy last-good asset into a false warning.
        if (!IsSourceBackedTypeUnavailableLocked(persistentId))
            m_diagnostics.PublishMissingReference(persistentId, lastKnownPath, expectedType);
        if (m_missingAssets.TryGetValue(persistentId, out WeakReference<AssetObject>? weak) &&
            weak.TryGetTarget(out AssetObject? existing) && expectedType.IsInstanceOfType(existing))
        {
            return existing;
        }
        Type type = ResolveDependencyExpectedType(
            new AssetDependency(persistentId, new TypeRef(stableTypeId), lastKnownPath));
        if (type == typeof(MissingAsset) &&
            !expectedType.IsAbstract &&
            !expectedType.IsInterface &&
            typeof(AssetObject).IsAssignableFrom(expectedType))
        {
            type = expectedType;
        }
        if (!expectedType.IsAssignableFrom(type))
        {
            throw new InvalidOperationException(
                $"Missing asset type '{type.FullName}' cannot be assigned to '{expectedType.FullName}'.");
        }
        AssetObject missing = (AssetObject)(Activator.CreateInstance(type, nonPublic: true)
            ?? throw new InvalidOperationException($"Missing asset type '{type.FullName}' cannot be created."));
        m_identities.InitializePersistentIdentity(missing, persistentId);
        m_runtimeOwner.Initialize(
            missing,
            AssetPath.Parse(lastKnownPath),
            string.Empty,
            ReadOnlyMemory<byte>.Empty,
            true,
            0);
        m_missingAssets[persistentId] = new WeakReference<AssetObject>(missing);
        return missing;
    }

    private void RollbackLoadLocked(AssetLoadTransaction transaction)
    {
        foreach (AssetRecord record in transaction.createdRecords.AsEnumerable().Reverse())
        {
            if (record.asset is not null)
            {
                m_runtimeOwner.Release(record.asset);
                m_identities.Unregister(record.asset);
            }
            record.asset = null;
        }
    }

    private T Execute<T>(Func<T> action)
    {
        if (ReferenceEquals(t_activeLoader, this))
            return action();
        using IDisposable generation = m_types.AcquireOperation("access Asset loader");
        lock (m_asyncSync)
        {
            ObjectDisposedException.ThrowIf(m_disposed || m_disposeRequested, this);
            m_admittedOperations++;
        }
        try
        {
            m_operationGate.Wait();
            AssetLoader? previous = t_activeLoader;
            t_activeLoader = this;
            try
            {
                return action();
            }
            finally
            {
                t_activeLoader = previous;
                m_operationGate.Release();
            }
        }
        finally
        {
            lock (m_asyncSync)
                m_admittedOperations--;
        }
    }

    private void Execute(Action action)
        => Execute(() =>
        {
            action();
            return 0;
        });

    private static async ValueTask<AssetObject?> AwaitSharedLoad(
        Task<AssetObject?> operation,
        Type requestedAssetType,
        CancellationToken cancellationToken
    ) {
        AssetObject? asset = await operation.WaitAsync(cancellationToken).ConfigureAwait(false);
        return asset is not null && requestedAssetType.IsInstanceOfType(asset) ? asset : null;
    }

    private void RemovePathOperation(
        string relativePath,
        Task<AssetObject?> operation
    ) {
        lock (m_asyncSync)
        {
            if (m_inFlightPathLoads.TryGetValue(relativePath, out Task<AssetObject?>? current) &&
                ReferenceEquals(current, operation))
            {
                m_inFlightPathLoads.Remove(relativePath);
            }
        }
    }

    private void RemoveIdOperation(
        Guid persistentId,
        Task<AssetObject?> operation
    ) {
        lock (m_asyncSync)
        {
            if (m_inFlightIdLoads.TryGetValue(persistentId, out Task<AssetObject?>? current) &&
                ReferenceEquals(current, operation))
            {
                m_inFlightIdLoads.Remove(persistentId);
            }
        }
    }

    private sealed class AssetLoadTransaction
    {
        internal List<AssetRecord> createdRecords { get; } = [];
    }

}
