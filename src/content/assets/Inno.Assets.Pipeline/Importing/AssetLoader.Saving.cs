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
    /// Saves an asset back to its current source path.
    /// </summary>
    /// <param name="asset">
    /// The asset to export.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an importer exported the asset.
    /// </returns>
    public bool Save(AssetObject asset)
    {
        ArgumentNullException.ThrowIfNull(asset);
        if (string.IsNullOrWhiteSpace(asset.assetPath.ToString()))
            throw new InvalidOperationException("An unsaved asset requires an explicit source-relative path.");
        return Save(asset.assetPath, asset);
    }

    /// <summary>
    /// Saves an asset to its initial or existing isolated source path while preserving an existing destination identity.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="asset">
    /// The asset to export. Detached replacement values do not replace an existing source's persistent identity.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an importer exported the asset.
    /// </returns>
    public bool Save(
        AssetPath path,
        AssetObject asset
    ) {
        ArgumentNullException.ThrowIfNull(asset);
        string normalized = NormalizeAssetPath(path);
        return Execute(() => SaveLocked(normalized, asset));
    }

    private bool SaveLocked(
        string relativePath,
        AssetObject asset
    ) {
        if (GetMount(relativePath).isReadOnly)
            throw new InvalidOperationException($"Asset source '{relativePath}' is read-only.");
        if (!string.IsNullOrWhiteSpace(asset.assetPath.ToString()) &&
            !string.Equals(NormalizeRelativePath(asset.assetPath.ToString()), relativePath, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Asset '{asset.assetPath.ToString()}' cannot be saved to unrelated path '{relativePath}' without creating a new asset.");
        }
        AssetImporter? importer = m_importers.FindByPath(relativePath);
        if (importer is null || !importer.targetAssetType.IsInstanceOfType(asset))
            return false;
        ReadOnlyMemory<byte>? exported = importer
            .ExportInternalAsync(
                new AssetExportContext(m_types, m_serialization, this),
                asset,
                CancellationToken.None)
            .AsTask()
            .GetAwaiter()
            .GetResult();
        if (exported is null)
            return false;
        byte[] sourceBytes = exported.Value.ToArray();
        AssetRecord? destinationRecord = FindRecordLocked(relativePath);
        bool destinationOwnsIdentity = destinationRecord is not null
            && destinationRecord.persistentId != Guid.Empty;
        Guid persistentId = destinationOwnsIdentity
            ? destinationRecord!.persistentId
            : asset.identity.persistentId;
        if (persistentId == Guid.Empty)
            persistentId = Guid.NewGuid();
        if (asset.identity.runtimeId is not null && asset.identity.persistentId != persistentId)
        {
            throw new InvalidOperationException(
                $"Registered asset '{asset.identity.persistentId:D}' cannot replace destination identity " +
                $"'{persistentId:D}' at '{relativePath}'.");
        }
        ImportBuild build = BuildImportLocked(relativePath, sourceBytes, importer, persistentId);
        AssetRecord? provisionalRecord = null;
        bool registeredHere = false;
        if (string.IsNullOrWhiteSpace(asset.assetPath.ToString()) && !destinationOwnsIdentity)
        {
            m_identities.InitializePersistentIdentity(asset, persistentId);
            registeredHere = m_identitiesActive && m_identities.Register(asset, persistentId);
            provisionalRecord = new AssetRecord
            {
                relativePath = relativePath,
                persistentId = persistentId,
                stableTypeId = build.meta.stableAssetTypeId,
                meta = build.meta,
                payload = build.payload,
                asset = asset
            };
            AddOrReplaceRecordLocked(provisionalRecord);
        }
        try
        {
            try
            {
                CommitBuildLocked(build, writeSource: true, sourceBytes);
            }
            finally
            {
                m_runtimeOwner.Release(build.asset);
            }
        }
        catch
        {
            if (provisionalRecord is not null)
                RemoveRecordLocked(provisionalRecord, removeGeneratedFiles: false);
            if (registeredHere)
                m_identities.Unregister(asset);
            throw;
        }
        AssetRecord committed = m_recordsByPath[relativePath];
        if (committed.asset is null)
        {
            if (asset.identity.persistentId != persistentId)
                m_identities.InitializePersistentIdentity(asset, persistentId);
            committed.asset = asset;
            if (m_identitiesActive && asset.identity.runtimeId is null)
                m_identities.Register(asset, persistentId);
            m_runtimeOwner.Initialize(
                asset,
                AssetPath.Parse(relativePath),
                build.meta.sourceHash,
                build.payload,
                false,
                1);
            AttachDependenciesLocked(committed);
        }
        return true;
    }

    private void RestoreCanonical(
        AssetObject canonical,
        byte[] state,
        byte[] payload,
        string sourcePath,
        string sourceHash,
        bool isMissing,
        long version
    ) {
        RestoreAssetState(canonical, state);
        m_runtimeOwner.Initialize(
            canonical,
            AssetPath.Parse(sourcePath),
            sourceHash,
            payload,
            isMissing,
            version);
    }

    private byte[] CaptureAssetState(AssetObject asset)
        => m_serialization.Encode(
            writer => writer.WriteProperties(asset),
            m_serializationContext);

    private void RestoreAssetState(
        AssetObject asset,
        ReadOnlySpan<byte> state
    ) {
        m_serialization.Decode(state, reader =>
        {
            reader.RestoreProperties(asset);
            return true;
        }, m_serializationContext);
    }

}
