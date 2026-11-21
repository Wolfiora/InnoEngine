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
using Inno.Core.Collections;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.IO;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.References;

using IOFile = System.IO.File;

namespace Inno.Assets.Pipeline;

public sealed partial class AssetLoader
{
    /// <summary>
    /// Gets direct or transitive runtime dependencies of an asset.
    /// </summary>
    /// <param name="asset">
    /// The asset to query.
    /// </param>
    /// <param name="recursive">
    /// Whether transitive dependencies should be included.
    /// </param>
    /// <returns>
    /// The persistent dependency descriptors.
    /// </returns>
    public IReadOnlyList<AssetDependency> GetDependencies(
        AssetObject asset,
        bool recursive = false
    ) {
        ArgumentNullException.ThrowIfNull(asset);
        return Execute(() => GetDependenciesLocked(asset.identity.persistentId, recursive));
    }

    /// <summary>
    /// Gets source import dependencies that invalidate an asset artifact.
    /// </summary>
    /// <param name="asset">
    /// The asset to query.
    /// </param>
    /// <param name="recursive">
    /// Whether transitive source dependencies should be included.
    /// </param>
    /// <returns>
    /// Canonical isolated source paths in stable order.
    /// </returns>
    public IReadOnlyList<AssetPath> GetImportDependencies(
        AssetObject asset,
        bool recursive = false
    ) {
        ArgumentNullException.ThrowIfNull(asset);
        return Execute(() =>
        {
            string path = NormalizeRelativePath(asset.assetPath.ToString());
            return m_importGraph.GetDependencies(path, recursive)
                .Select(AssetPath.Parse)
                .OrderBy(static value => value.source.value, StringComparer.Ordinal)
                .ThenBy(static value => value.localPath, StringComparer.Ordinal)
                .ToArray();
        });
    }

    /// <summary>
    /// Gets an engine-known reference diagnostic snapshot.
    /// </summary>
    /// <param name="asset">
    /// The asset to inspect.
    /// </param>
    /// <returns>
    /// The reference diagnostic snapshot.
    /// </returns>
    public AssetReferenceInfo GetReferenceInfo(AssetObject asset)
    {
        ArgumentNullException.ThrowIfNull(asset);
        return Execute(() => GetReferenceInfoLocked(asset));
    }

    private void AttachDependenciesLocked(AssetRecord record)
    {
        if (record.asset is null)
        {
            return;
        }
        var dependencies = new List<AssetObject>();
        foreach (AssetDependency dependency in GetDirectDependencies(record.meta))
        {
            AssetRecord? dependencyRecord = FindDependencyRecordLocked(dependency);
            AssetObject dependencyAsset = dependencyRecord?.asset
                                          ?? ResolveReferenceLocked(
                                              dependency.persistentId,
                                              dependency.type.stableId,
                                              dependency.lastKnownPath,
                                              ResolveDependencyExpectedType(dependency)
                                          );
            dependencies.Add(dependencyAsset);
        }
        m_dependencyRetention.Remove(record.asset);
        m_dependencyRetention.Add(record.asset, new AssetDependencySet(dependencies.ToArray()));
    }

    private AssetDependency[] ResolveDeclaredDependenciesLocked(AssetImportContext context)
    {
        Dictionary<Guid, AssetDependency> result = context.runtimeDependencies.ToDictionary(static value => value.persistentId);
        foreach (Guid persistentID in result.Keys.ToArray())
        {
            AssetDependency dependency = result[persistentID];
            string dependencyPath = dependency.lastKnownPath;
            if (m_recordsByID.TryGetValue(dependency.persistentId, out AssetRecord? record))
            {
                dependencyPath = record.relativePath;
                result[persistentID] = new AssetDependency(dependency.persistentId, dependency.type, dependencyPath);
            }
            if (!string.IsNullOrWhiteSpace(dependencyPath))
            {
                ValidateSourceReferenceLocked(context.assetPath.ToString(), NormalizeRelativePath(dependencyPath));
            }
        }
        foreach (string path in context.runtimeDependencyPaths.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            string normalized = NormalizeRelativePath(path);
            ValidateSourceReferenceLocked(context.assetPath.ToString(), normalized);
            if (m_activeImports.Contains(normalized))
            {
                Guid pendingID = m_pendingImportIds[normalized];
                AssetImporter? pendingImporter = m_importers.FindByPath(normalized);
                TypeRef typeRef = pendingImporter is not null
                                  && m_types.TryGetTypeRef(pendingImporter.targetAssetType, out TypeRef pendingTypeRef)
                                  ? pendingTypeRef
                                  : default;
                result[pendingID] = new AssetDependency(pendingID, typeRef, normalized);
                continue;
            }
            AssetRecord? dependencyRecord = FindRecordLocked(normalized);
            bool dependencyStale = dependencyRecord is null
                                   || IsStale(dependencyRecord, out _);
            if (dependencyStale && !ImportLocked(normalized))
            {
                throw new InvalidOperationException(
                    $"Runtime dependency '{normalized}' referenced by '{context.assetPath}' cannot be imported."
                );
            }
            dependencyRecord = FindRecordLocked(normalized)
                               ?? throw new InvalidOperationException($"Runtime dependency '{normalized}' has no metadata.");
            result[dependencyRecord.persistentID] = new AssetDependency(
                dependencyRecord.persistentID,
                new TypeRef(dependencyRecord.stableTypeID),
                dependencyRecord.relativePath
            );
        }
        return result.Values.OrderBy(static value => value.persistentId).ToArray();
    }

    private void ValidateImportDependenciesLocked(AssetMeta candidate)
    {
        string node = candidate.relativePath;
        IReadOnlyList<string> previous = m_importGraph.GetDependencies(node);
        string[] dependencies = candidate.importDependencies
            .Where(static value => (AssetImportDependencyKind)value.kind == AssetImportDependencyKind.Source)
            .Select(static value => value.key)
            .ToArray();
        m_importGraph.ReplaceDependencies(node, dependencies);
        if (m_importGraph.TryFindCycle(out IReadOnlyList<string> cycle))
        {
            m_importGraph.ReplaceDependencies(node, previous);
            throw new InvalidOperationException(
                $"Asset import dependency cycle detected: {string.Join(" -> ", cycle)}."
            );
        }
    }

    private void ValidateImportDependencySnapshotsLocked(AssetMeta candidate)
    {
        for (int i = 0; i < candidate.importDependencies.Length; i++)
        {
            AssetImportDependencyData dependency = candidate.importDependencies[i];
            string fingerprint = ComputeImportDependencyFingerprintLocked(ref dependency, out bool metadataChanged);
            if (!string.Equals(dependency.fingerprint, fingerprint, StringComparison.Ordinal))
            {
                throw new IOException(
                    $"Import dependency '{dependency.key}' changed while "
                    + $"'{candidate.relativePath}' was importing."
                );
            }
            if (metadataChanged)
            {
                candidate.importDependencies[i] = dependency;
            }
        }
    }

    private IReadOnlyList<AssetDependency> GetDependenciesLocked(
        Guid persistentID,
        bool recursive
    ) {
        if (!m_recordsByID.TryGetValue(persistentID, out AssetRecord? record))
        {
            return Array.Empty<AssetDependency>();
        }
        IEnumerable<Guid> ids = recursive
                                ? m_runtimeGraph.GetDependencies(persistentID, recursive: true)
                                : m_runtimeGraph.GetDependencies(persistentID);
        return ids
            .Select(id => (
                m_recordsByID.TryGetValue(id, out AssetRecord? dependency)
                ? new AssetDependency(id, new TypeRef(dependency.stableTypeID), dependency.relativePath)
                : FindDescriptor(record.meta, id)
            ))
            .Where(static descriptor => descriptor.persistentId != Guid.Empty)
            .ToArray();
    }

    private AssetReferenceInfo GetReferenceInfoLocked(AssetObject asset)
    {
        Guid id = asset.identity.persistentId;
        var locations = new List<AssetReferenceLocation>();
        foreach (Guid dependentID in m_runtimeGraph.GetDependents(id))
        {
            if (!m_recordsByID.TryGetValue(dependentID, out AssetRecord? dependent))
            {
                continue;
            }
            locations.Add(
                new AssetReferenceLocation(
                    AssetReferenceKind.AssetDependency,
                    dependentID,
                    dependent.relativePath,
                    "runtimeDependencies"
                )
            );
        }
        m_recordsByID.TryGetValue(id, out AssetRecord? record);
        return new AssetReferenceInfo(
            id,
            asset.assetPath,
            asset.contentVersion,
            record?.asset is not null,
            record?.lastSweepReachability,
            locations.ToArray()
        );
    }

    private void UpdateGraphsLocked(AssetRecord record)
    {
        m_runtimeGraph.ReplaceDependencies(
            record.persistentID,
            record.meta.runtimeDependencies.Select(static value => value.persistentId)
        );
        m_importGraph.ReplaceDependencies(
            record.relativePath,
            record.meta.importDependencies
                .Where(static value => (AssetImportDependencyKind)value.kind == AssetImportDependencyKind.Source)
                .Select(static value => value.key)
        );
    }

    private AssetRecord? FindDependencyRecordLocked(AssetDependency dependency)
    {
        if (m_recordsByID.TryGetValue(dependency.persistentId, out AssetRecord? record))
        {
            return record;
        }
        return null;
    }

    private AssetImportDependencyData CreateImportDependencyDataLocked(
        string ownerPath,
        AssetImportDependency dependency
    ) {
        if (dependency.kind == AssetImportDependencyKind.Source)
        {
            ValidateSourceReferenceLocked(ownerPath, NormalizeRelativePath(dependency.key));
        }
        var result = new AssetImportDependencyData
        {
            kind = (int)dependency.kind,
            key = dependency.key,
            fingerprint = dependency.fingerprint
        };
        result.fingerprint = ComputeImportDependencyFingerprintLocked(ref result, out _);
        return result;
    }

    private void ValidateSourceReferenceLocked(
        string ownerPath,
        string dependencyPath
    ) {
        AssetPath owner = AssetPath.Parse(ownerPath);
        AssetPath dependency = AssetPath.Parse(dependencyPath);
        if (owner.source == dependency.source || owner.source == AssetSourceId.project)
        {
            return;
        }
        if (dependency.source == AssetSourceId.project)
        {
            throw new InvalidOperationException(
                $"Plugin source '{owner.source}' cannot reference project asset '{dependency.localPath}'."
            );
        }
        if (!m_mounts.TryGetValue(owner.source, out AssetSourceMount? ownerMount))
        {
            throw new InvalidOperationException($"Asset source '{owner.source}' is not mounted.");
        }
        if (!m_mounts.ContainsKey(dependency.source))
        {
            throw new InvalidOperationException($"Asset source dependency '{dependency.source}' is not mounted.");
        }
        if (!ownerMount.dependencySourceIds.Contains(dependency.source))
        {
            throw new InvalidOperationException(
                $"Plugin source '{owner.source}' did not declare dependency '{dependency.source}'."
            );
        }
    }

    private string ComputeImportDependencyFingerprintLocked(
        ref AssetImportDependencyData dependency,
        out bool metadataChanged
    ) {
        metadataChanged = false;
        switch ((AssetImportDependencyKind)dependency.kind)
        {
            case AssetImportDependencyKind.Source:
            {
                string sourcePath = GetSourcePath(NormalizeRelativePath(dependency.key));
                if (!AssetSourceFileStamp.TryCapture(sourcePath, out AssetSourceFileStamp sourceStamp))
                {
                    return "MISSING";
                }
                bool stampChanged = !SourceStampMatches(dependency, sourceStamp);
                if (GetMount(dependency.key).isReadOnly && !stampChanged)
                {
                    return dependency.fingerprint;
                }
                string hash = ReadStableSourceHash(sourcePath, out sourceStamp);
                if (stampChanged)
                {
                    ApplySourceStamp(ref dependency, sourceStamp);
                    metadataChanged = true;
                }
                return hash;
            }
            case AssetImportDependencyKind.Artifact:
                return Guid.TryParse(dependency.key, out Guid id)
                       && m_recordsByID.TryGetValue(id, out AssetRecord? record)
                       ? record.meta.artifactKey
                       : "MISSING";
            case AssetImportDependencyKind.Custom:
                return dependency.fingerprint;
            default:
                return "UNKNOWN";
        }
    }

    private static AssetDependency[] GetDirectDependencies(AssetMeta meta)
        => meta.runtimeDependencies.Select(static value => new AssetDependency(
            value.persistentId,
            new TypeRef(value.stableTypeId),
            value.lastKnownPath
        )).ToArray();

    private sealed class AssetDependencySet(AssetObject[] assets)
    {
        internal AssetObject[] assets { get; } = assets;
    }
}
