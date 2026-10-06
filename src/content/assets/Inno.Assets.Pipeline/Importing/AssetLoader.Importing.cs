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
    /// Imports one isolated source file into metadata and a runtime artifact.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an importer handled the source.
    /// </returns>
    public bool Import(AssetPath path) => Execute(() => ImportLocked(NormalizeAssetPath(path), force: true));

    /// <summary>
    /// Builds and validates the requested artifact asynchronously before publishing it.
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
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(inputs);
        Guid targetId = definition.identity.persistentId;
        string displayName = string.IsNullOrWhiteSpace(definition.assetPath.ToString())
            ? definition.GetType().Name
            : definition.assetPath.ToString();
        AssetArtifactKey key = Execute(() =>
        {
            try
            {
                AssetBuildProcessor processor = m_buildProcessors.Find(definition.GetType())
                    ?? throw new InvalidOperationException(
                        $"No asset build processor accepts '{definition.GetType().FullName}'.");
                var output = new AssetArtifactWriter();
                processor.BuildInternalAsync(definition, inputs, output, cancellationToken)
                    .AsTask()
                    .GetAwaiter()
                    .GetResult();
                if (output.outputs.Count == 0)
                    throw new InvalidOperationException("An asset build processor produced no outputs.");
                string fingerprint = CreateBuildFingerprint(processor, definition, inputs);
                AssetArtifactKey result = m_artifacts.Commit(fingerprint, output.outputs, output.authoringOutputs);
                m_diagnostics.PublishBuild(targetId, displayName, output.diagnostics);
                return result;
            }
            catch (Exception exception)
            {
                m_diagnostics.PublishBuildFailure(targetId, displayName, exception);
                m_log.Write(
                    LogLevel.Error,
                    "Asset build for '{0}' failed: {1}",
                    [displayName, exception]);
                throw;
            }
        });
        return ValueTask.FromResult(key);
    }

    private bool ImportLocked(
        string relativePath,
        bool force = false
    ) {
        if (m_activeImports.Contains(relativePath))
            return true;
        if (!force && IsExtensionImportUnchanged(relativePath))
            return false;
        if (IsSourceIgnored(relativePath, isDirectory: false))
            return false;
        string sourcePath = GetSourcePath(relativePath);
        if (!IOFile.Exists(sourcePath))
            return false;
        AssetImporter? importer = m_importers.FindByPath(relativePath);
        if (importer is null)
            return false;

        AssetRecord? existingRecord = FindRecordLocked(relativePath);
        AssetSourceMount sourceMount = GetMount(relativePath);
        if (sourceMount.isReadOnly && !IOFile.Exists(GetMetaPath(relativePath)))
        {
            throw new InvalidDataException(
                $"Read-only source '{relativePath}' requires a valid '{C_META_POSTFIX}' sidecar.");
        }
        Guid persistentId = existingRecord?.persistentId
            ?? m_pendingImportIds.GetValueOrDefault(relativePath);
        if (persistentId == Guid.Empty &&
            TryReadSourceMeta(GetMetaPath(relativePath), out AssetSourceMeta sourceMeta))
        {
            persistentId = sourceMeta.persistentId;
            AssetRecord? sameId = FindRecordByIdWithoutLoading(persistentId);
            if (sameId is not null &&
                !string.Equals(sameId.relativePath, relativePath, StringComparison.OrdinalIgnoreCase))
            {
                if (sameId.meta.isTombstone)
                {
                    RebindTombstoneLocked(sameId, relativePath);
                    existingRecord = sameId;
                }
                else if (sourceMount.isReadOnly)
                {
                    throw new InvalidDataException(
                        $"Read-only source '{relativePath}' duplicates persistent ID '{persistentId}'.");
                }
                else
                {
                    persistentId = Guid.NewGuid();
                }
            }
        }
        if (persistentId == Guid.Empty)
            persistentId = Guid.NewGuid();
        m_pendingImportIds[relativePath] = persistentId;
        m_activeImports.Add(relativePath);
        try
        {
            byte[] sourceBytes = ReadStableSourceBytes(sourcePath, out AssetSourceFileStamp sourceStamp);
            AssetImportContext? attemptedContext = null;
            try
            {
                ImportBuild build = BuildImportLocked(
                    relativePath,
                    sourceBytes,
                    importer,
                    persistentId,
                    sourceStamp,
                    context => attemptedContext = context);
                try
                {
                    CommitBuildLocked(build, writeSource: false, sourceBytes);
                    if (m_unavailableImports.Remove(relativePath, out var recovered))
                    {
                        foreach (string dependent in m_unavailableImports
                            .Where(pair => pair.Value.kind == recovered.kind && pair.Value.id == recovered.id)
                            .Select(static pair => pair.Key).ToArray())
                            m_unavailableImports.Remove(dependent);
                    }
                }
                finally
                {
                    m_runtimeOwner.Release(build.asset);
                }
                return true;
            }
            catch (Exception exception)
            {
                RecordImportFailureLocked(
                    relativePath,
                    sourceBytes,
                    importer,
                    persistentId,
                    exception,
                    sourceStamp,
                    attemptedContext);
                return false;
            }
        }
        catch (IOException exception)
        {
            RecordImportFailureLocked(relativePath, [], importer, persistentId, exception);
            return false;
        }
        catch (UnauthorizedAccessException exception)
        {
            RecordImportFailureLocked(relativePath, [], importer, persistentId, exception);
            return false;
        }
        finally
        {
            m_activeImports.Remove(relativePath);
            m_pendingImportIds.Remove(relativePath);
        }
    }

    private bool IsExtensionImportUnchanged(string relativePath)
    {
        if (!m_unavailableImports.TryGetValue(relativePath, out var previous) ||
            previous.generation != m_types.current.version ||
            !AssetSourceFileStamp.TryCapture(GetSourcePath(relativePath), out AssetSourceFileStamp sourceStamp) ||
            sourceStamp != previous.source)
            return false;
        AssetSourceFileStamp.TryCapture(GetMetaPath(relativePath), out AssetSourceFileStamp metadataStamp);
        if (metadataStamp != previous.metadata)
            return false;
        AssetRecord? record = FindRecordLocked(relativePath);
        if (record is null)
            return false;
        foreach (AssetImportDependencyData value in record.meta.importDependencies)
        {
            AssetImportDependencyData dependency = value;
            if (ComputeImportDependencyFingerprintLocked(ref dependency, out _) != value.fingerprint)
                return false;
        }
        return true;
    }

    private ImportBuild BuildImportLocked(
        string relativePath,
        byte[] sourceBytes,
        AssetImporter importer,
        Guid persistentId,
        AssetSourceFileStamp sourceStamp = default,
        Action<AssetImportContext>? onContextCreated = null
    ) {
        string sourceHash = ComputeSha256Hex(sourceBytes);
        var context = new AssetImportContext(
            relativePath,
            GetSourcePath(relativePath),
            sourceBytes,
            sourceHash,
            persistentId,
            m_types,
            m_serialization,
            this,
            (
                dependencyPath,
                dependencyType
            ) =>
            {
                string normalizedDependency = NormalizeRelativePath(dependencyPath);
                ValidateSourceReferenceLocked(relativePath, normalizedDependency);
                return LoadPathLocked(normalizedDependency, dependencyType);
            },
            sourceDependencyPath =>
            {
                string normalizedDependency = NormalizeRelativePath(sourceDependencyPath);
                ValidateSourceReferenceLocked(relativePath, normalizedDependency);
                string physicalPath = GetSourcePath(normalizedDependency);
                if (!IOFile.Exists(physicalPath))
                {
                    throw new FileNotFoundException(
                        $"Import source dependency '{normalizedDependency}' does not exist.",
                        physicalPath);
                }
                return ReadStableSourceBytes(physicalPath, out _);
            }, this);
        onContextCreated?.Invoke(context);
        byte[] settingsBytes = ReadImportSettingsBytesLocked(relativePath, importer);
        context.importSettings = RestoreImportSettingsLocked(importer, settingsBytes, context);
        AssetImportProduct product = importer
            .ImportInternalAsync(context, m_importCancellation)
            .AsTask()
            .GetAwaiter()
            .GetResult();
        AssetDeploymentScope deploymentScope = product.deploymentScope ?? importer.deploymentScope;
        if (!Enum.IsDefined(deploymentScope))
        {
            throw new InvalidOperationException(
                $"Importer '{importer.GetType().FullName}' declares an unsupported deployment scope.");
        }
        bool hasRuntimeOutput = product.outputs.ContainsKey("runtime");
        if (deploymentScope == AssetDeploymentScope.Runtime && !hasRuntimeOutput)
        {
            throw new InvalidOperationException(
                $"Runtime importer '{importer.GetType().FullName}' must write a 'runtime' artifact output.");
        }
        if (deploymentScope == AssetDeploymentScope.AuthoringOnly && hasRuntimeOutput)
        {
            throw new InvalidOperationException(
                $"Authoring-only importer '{importer.GetType().FullName}' cannot write a 'runtime' artifact output.");
        }
        if (!importer.targetAssetType.IsInstanceOfType(product.asset))
        {
            throw new InvalidOperationException(
                $"Importer '{importer.GetType().FullName}' returned '{product.asset.GetType().FullName}' " +
                $"instead of '{importer.targetAssetType.FullName}'.");
        }
        if (!m_types.TryGetTypeRef(product.asset.GetType(), out TypeRef assetTypeRef))
        {
            throw new InvalidOperationException(
                $"Imported asset type '{product.asset.GetType().FullName}' requires a StableTypeId.");
        }

        AssetDependency[] runtimeDependencies = ResolveDeclaredDependenciesLocked(context);
        byte[] state = CaptureAssetState(product.asset);
        m_runtimeOwner.Initialize(
            product.asset,
            AssetPath.Parse(relativePath),
            sourceHash,
            product.runtimePayload,
            false,
            1);
        var outputs = new Dictionary<string, ReadOnlyMemory<byte>>(product.outputs, StringComparer.Ordinal);
        if (!outputs.TryAdd("asset-state", state))
            throw new InvalidOperationException("The artifact output name 'asset-state' is reserved.");
        string implementationFingerprint = GetImporterImplementationFingerprint(importer);
        var meta = new AssetMeta
        {
            persistentId = persistentId,
            relativePath = relativePath,
            sourceHash = sourceHash,
            importerId = importer.importerId,
            deploymentScope = (int)deploymentScope,
            stableAssetTypeId = assetTypeRef.stableId,
            assetStateBytes = state,
            runtimeDependencies = runtimeDependencies.Select(ToData).ToArray(),
            importDependencies = context.importDependencies
                .Select(dependency => CreateImportDependencyDataLocked(context.assetPath.ToString(), dependency))
                .ToArray(),
            importStatus = (int)AssetImportStatus.Imported,
            importerImplementationFingerprint = implementationFingerprint,
            importerSettingsHash = ComputeSha256Hex(settingsBytes),
            diagnostics = product.diagnostics.ToArray()
        };
        ApplySourceStamp(meta, sourceStamp);
        ValidateImportDependenciesLocked(meta);
        return new ImportBuild(
            meta,
            product.asset,
            product.runtimePayload.ToArray(),
            outputs,
            product.authoringOutputs,
            runtimeDependencies);
    }

    private void CommitBuildLocked(
        ImportBuild build,
        bool writeSource,
        byte[] sourceBytes
    ) {
        if (writeSource && m_sourceMetadataStage is not null)
            throw new InvalidOperationException("An isolated Asset candidate cannot edit authoring source content.");
        AssetRecord? existing = FindRecordLocked(build.meta.relativePath);
        AssetObject? canonical = existing?.asset;
        byte[]? previousState = null;
        byte[]? previousPayload = null;
        string previousPath = string.Empty;
        string previousHash = string.Empty;
        long previousVersion = 0;
        bool previousMissing = false;
        if (canonical is not null)
        {
            if (canonical.GetType() != build.asset.GetType())
            {
                AssetObject replaced = canonical;
                m_runtimeOwner.Initialize(
                    replaced,
                    AssetPath.Parse(build.meta.relativePath),
                    build.meta.sourceHash,
                    replaced.runtimePayload,
                    true,
                    replaced.contentVersion + 1);
                m_runtimeOwner.Release(replaced);
                m_identities.Unregister(replaced);
                existing!.asset = null;
                canonical = null;
                PublishReloaded(replaced);
            }
        }
        if (canonical is not null)
        {
            previousState = CaptureAssetState(canonical);
            previousPayload = canonical.runtimePayload.ToArray();
            previousPath = canonical.assetPath.ToString();
            previousHash = m_runtimeOwner.GetSourceHash(canonical);
            previousVersion = canonical.contentVersion;
            previousMissing = canonical.isMissing;
            try
            {
                RestoreAssetState(canonical, build.meta.assetStateBytes);
                m_runtimeOwner.Initialize(
                    canonical,
                    AssetPath.Parse(build.meta.relativePath),
                    build.meta.sourceHash,
                    build.payload,
                    false,
                    previousVersion + 1);
            }
            catch
            {
                RestoreCanonical(
                    canonical,
                    previousState,
                    previousPayload,
                    previousPath,
                    previousHash,
                    previousMissing,
                    previousVersion);
                throw;
            }
        }

        string sourcePath = GetSourcePath(build.meta.relativePath);
        string metaPath = GetMetaPath(build.meta.relativePath);
        FileSnapshot sourceSnapshot = CaptureFile(sourcePath);
        FileSnapshot metaSnapshot = CaptureFile(metaPath);
        try
        {
            if (writeSource)
                WriteAtomic(sourcePath, sourceBytes);
            if (!AssetSourceFileStamp.TryCapture(sourcePath, out AssetSourceFileStamp sourceStamp))
                throw new IOException($"Source '{build.meta.relativePath}' changed while its import was committing.");
            if (!writeSource && !SourceStampMatches(build.meta, sourceStamp))
            {
                throw new IOException(
                    $"Source '{build.meta.relativePath}' changed while its importer was running.");
            }
            ApplySourceStamp(build.meta, sourceStamp);
            if (!string.Equals(build.meta.importerSettingsHash,
                    ComputeSha256Hex(ReadImportSettingsBytesLocked(build.meta.relativePath,
                        m_importers.FindById(build.meta.importerId)!)), StringComparison.Ordinal))
                throw new IOException($"Import settings for '{build.meta.relativePath}' changed during import.");
            ValidateImportDependencySnapshotsLocked(build.meta);
            AssetArtifactKey artifactKey = m_artifacts.Commit(
                CreateImportFingerprint(build.meta),
                build.outputs,
                build.authoringOutputs);
            build.meta.artifactKey = artifactKey.value;
            build.meta.lastSuccessfulArtifactKey = artifactKey.value;
            WriteSourceMeta(build.meta);
        }
        catch
        {
            if (writeSource)
                RestoreFile(sourcePath, sourceSnapshot);
            RestoreFile(metaPath, metaSnapshot);
            if (canonical is not null && previousState is not null && previousPayload is not null)
            {
                RestoreCanonical(
                    canonical,
                    previousState,
                    previousPayload,
                    previousPath,
                    previousHash,
                    previousMissing,
                    previousVersion);
            }
            throw;
        }

        AssetRecord record = existing ?? new AssetRecord();
        record.relativePath = build.meta.relativePath;
        record.persistentId = build.meta.persistentId;
        record.stableTypeId = build.meta.stableAssetTypeId;
        record.meta = build.meta;
        record.payload = build.payload;
        record.importerGeneration = m_importers.GetGeneration(build.meta.importerId);
        if (canonical is not null)
            record.asset = canonical;
        AddOrReplaceRecordLocked(record);
        UpdateGraphsLocked(record);
        CommitCatalogLocked();
        if (canonical is not null)
        {
            AttachDependenciesLocked(record);
            PublishReloaded(canonical);
        }
    }

    private readonly record struct ImportBuild(
        AssetMeta meta,
        AssetObject asset,
        byte[] payload,
        IReadOnlyDictionary<string, ReadOnlyMemory<byte>> outputs,
        IReadOnlySet<string> authoringOutputs,
        AssetDependency[] dependencies
    );

}
