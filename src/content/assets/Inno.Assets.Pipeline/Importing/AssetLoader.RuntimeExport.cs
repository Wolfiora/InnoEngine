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
    /// Writes a source-free runtime catalog and its exact immutable artifact closure.
    /// </summary>
    /// <param name="destinationLibraryRoot">
    /// Empty destination that becomes the deployed content root.
    /// </param>
    /// <returns>
    /// Counts and source identities for the exported runtime snapshot.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when runtime artifacts are incomplete, a deployed reference is authoring-only,
    /// or a transitive authoring input has failed or become stale.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown when the destination is not empty or content cannot be copied.
    /// </exception>
    public AssetRuntimeContentInfo ExportRuntimeArtifacts(string destinationLibraryRoot)
        => ExportRuntimeArtifacts(destinationLibraryRoot, CancellationToken.None);

    /// <summary>
    /// Exports the validated runtime artifact closure while observing cooperative cancellation between files.
    /// </summary>
    /// <param name="destinationLibraryRoot">
    /// The empty directory that receives the runtime-only asset database.
    /// </param>
    /// <param name="cancellationToken">
    /// The token checked before every artifact file is copied.
    /// </param>
    /// <returns>
    /// Counts and source identities represented by the exported snapshot.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when artifacts are incomplete or transitive source inputs no longer certify the imported snapshot.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when cancellation is requested before the snapshot finishes.
    /// </exception>
    public AssetRuntimeContentInfo ExportRuntimeArtifacts(
        string destinationLibraryRoot,
        CancellationToken cancellationToken
    )
        => Execute(() => ExportRuntimeArtifactsLocked(destinationLibraryRoot, cancellationToken));

    internal AssetRuntimeContentInfo ExportRuntimeArtifacts(
        string destinationLibraryRoot,
        SerializationGeneration serialization,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(serialization);
        return Execute(() => ExportRuntimeArtifactsLocked(
            destinationLibraryRoot,
            cancellationToken,
            serialization));
    }

    private AssetRuntimeContentInfo ExportRuntimeArtifactsLocked(
        string destinationLibraryRoot,
        CancellationToken cancellationToken,
        SerializationGeneration? serialization = null
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationLibraryRoot);
        string destination = Path.GetFullPath(destinationLibraryRoot);
        if (Directory.Exists(destination) && Directory.EnumerateFileSystemEntries(destination).Any())
            throw new IOException("Runtime asset content destination must be empty.");
        AssetRecord[] imported = m_recordsByPath.Values
            .Where(static record =>
                !record.meta.isDirectory
                && !record.meta.isTombstone
                && !AssetSample.IsRuntimeExcluded(AssetPath.Parse(record.relativePath), isDirectory: false)
                && (record.meta.importStatus == (int)AssetImportStatus.Imported
                    || !string.IsNullOrEmpty(record.meta.artifactKey)
                    || (!string.IsNullOrEmpty(record.meta.importerId)
                        && record.meta.importStatus is (int)AssetImportStatus.Pending or (int)AssetImportStatus.Failed)))
            .OrderBy(static record => record.relativePath, StringComparer.Ordinal)
            .ToArray();
        AssetRecord? invalidScope = imported.FirstOrDefault(static record =>
            !Enum.IsDefined((AssetDeploymentScope)record.meta.deploymentScope));
        if (invalidScope is not null)
        {
            throw new InvalidOperationException(
                $"Asset '{invalidScope.relativePath}' has an invalid deployment scope.");
        }
        AssetRecord[] exported = imported
            .Where(static record => record.meta.deploymentScope == (int)AssetDeploymentScope.Runtime)
            .ToArray();
        var validated = new HashSet<Guid>();
        foreach (AssetRecord record in exported)
            ValidateExportInputsLocked(record, record.relativePath, validated, cancellationToken);
        string[] keys = exported
            .Select(static record => record.meta.artifactKey)
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal)
            .ToArray();
        foreach (AssetRecord record in exported)
        {
            AssetArtifactKey key = new(record.meta.artifactKey);
            if (!m_artifacts.TryGet(key, "asset-state", serialization, out _)
                || !m_artifacts.TryGet(key, "runtime", serialization, out _))
            {
                throw new InvalidOperationException(
                    $"Asset '{record.relativePath}' has no complete runtime artifact bundle '{key}'.");
            }
        }
        HashSet<Guid> exportedIds = exported
            .Select(static record => record.persistentId)
            .ToHashSet();
        foreach (AssetRecord record in exported)
        {
            AssetDependencyData unavailable = record.meta.runtimeDependencies
                .FirstOrDefault(dependency => !exportedIds.Contains(dependency.persistentId));
            if (unavailable.persistentId != Guid.Empty)
            {
                throw new InvalidOperationException(
                    $"Runtime asset '{record.relativePath}' depends on non-runtime asset " +
                    $"'{unavailable.lastKnownPath}'.");
            }
        }

        Directory.CreateDirectory(destination);
        AssetCatalogStore catalog = serialization is null
            ? new AssetCatalogStore(destination, m_serialization)
            : new AssetCatalogStore(destination, serialization);
        var destinationArtifacts = new AssetArtifactStore(destination, m_serialization);
        var projectedKeys = new Dictionary<string, AssetArtifactKey>(StringComparer.Ordinal);
        foreach (string key in keys)
            projectedKeys.Add(key, m_artifacts.ExportRuntime(new(key), destinationArtifacts, serialization, cancellationToken));
        var deployed = new List<AssetMeta>();
        foreach (AssetRecord record in exported)
        {
            byte[] bytes = serialization is null ? m_serialization.Serialize(record.meta) : serialization.Serialize(record.meta);
            AssetMeta meta = serialization is null ? m_serialization.Deserialize<AssetMeta>(bytes) : serialization.Deserialize<AssetMeta>(bytes);
            meta.artifactKey = projectedKeys[record.meta.artifactKey].value;
            meta.lastSuccessfulArtifactKey = meta.artifactKey;
            meta.importDependencies = [];
            deployed.Add(meta);
        }
        catalog.Commit(deployed.ToArray());
        long totalBytes = Directory
            .EnumerateFiles(Path.Combine(destination, "AssetDatabase"), "*", SearchOption.AllDirectories)
            .Sum(static path => new FileInfo(path).Length);
        totalBytes += Directory.EnumerateFiles(destinationArtifacts.root, "*", SearchOption.AllDirectories).Sum(static path => new FileInfo(path).Length);

        AssetSourceId[] sources = exported
            .Select(static record => AssetPath.Parse(record.relativePath).source)
            .Append(AssetSourceId.project)
            .Distinct()
            .OrderBy(static source => source.value, StringComparer.Ordinal)
            .ToArray();
        foreach (AssetSourceId source in sources)
            Directory.CreateDirectory(Path.Combine(destination, "Sources", source.value));
        return new AssetRuntimeContentInfo(sources, exported.Length, keys.Length, totalBytes);
    }

    private void ValidateExportInputsLocked(
        AssetRecord record,
        string dependencyChain,
        HashSet<Guid> validated,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        if (!validated.Add(record.persistentId))
            return;
        if (record.meta.isTombstone || record.meta.importStatus != (int)AssetImportStatus.Imported)
        {
            throw new InvalidOperationException(
                $"Runtime export requires successful current inputs: {dependencyChain}. " +
                $"Status: {(AssetImportStatus)record.meta.importStatus}. " +
                string.Join(" | ", record.meta.diagnostics));
        }

        // Validate authoring inputs as well as deployed references. Editor last-good artifacts
        // remain available, but must never certify a build of failed or stale source content.
        foreach (AssetImportDependencyData dependency in record.meta.importDependencies)
        {
            if ((AssetImportDependencyKind)dependency.kind != AssetImportDependencyKind.Artifact)
                continue;
            if (!Guid.TryParse(dependency.key, out Guid id)
                || !m_recordsById.TryGetValue(id, out AssetRecord? input))
            {
                throw new InvalidOperationException(
                    $"Runtime export has a missing artifact input: {dependencyChain} -> {dependency.key}.");
            }
            ValidateExportInputsLocked(input, $"{dependencyChain} -> {input.relativePath}", validated, cancellationToken);
        }
        // Do not reimport here: target compilation may already have captured this generation.
        // A source change during the build must reject the snapshot, not mix two generations.
        if (IsStale(record, out _))
        {
            throw new InvalidOperationException(
                $"Runtime export has stale source inputs: {dependencyChain}. Reimport current sources and rebuild.");
        }
    }

    private void ValidateRuntimeArtifactsLocked()
    {
        foreach (AssetRecord record in m_recordsByPath.Values)
        {
            if (!IsMounted(record.relativePath))
            {
                throw new InvalidDataException(
                    $"Runtime asset '{record.relativePath}' references an undeclared source mount.");
            }
            if (record.meta.isDirectory || record.meta.isTombstone)
                continue;
            if (record.meta.deploymentScope != (int)AssetDeploymentScope.Runtime)
            {
                throw new InvalidDataException(
                    $"Runtime catalog contains authoring-only asset '{record.relativePath}'.");
            }
            if (record.meta.importStatus != (int)AssetImportStatus.Imported)
            {
                throw new InvalidDataException(
                    $"Runtime asset '{record.relativePath}' was not successfully imported before export.");
            }
            AssetArtifactKey key = new(record.meta.artifactKey);
            if (!m_artifacts.TryGet(key, "asset-state", out _)
                || !m_artifacts.TryGet(key, "runtime", out _))
            {
                throw new InvalidDataException(
                    $"Runtime asset '{record.relativePath}' has an incomplete artifact bundle '{key}'.");
            }
        }
    }

    private void EnsureReadOnlyImportsSucceededLocked()
    {
        AssetRecord[] failed = m_recordsByPath.Values
            .Where(record =>
                GetMount(record.relativePath).isReadOnly
                && record.meta.importStatus is (int)AssetImportStatus.Failed
                    or (int)AssetImportStatus.Conflict)
            .OrderBy(static record => record.relativePath, StringComparer.Ordinal)
            .ToArray();
        if (failed.Length == 0)
        {
            return;
        }

        string details = string.Join(
            "; ",
            failed.Select(record =>
                $"{record.relativePath}: {string.Join(" | ", record.meta.diagnostics)}"));
        throw new InvalidDataException(
            $"Read-only Asset Source candidate contains failed imports: {details}");
    }

}
