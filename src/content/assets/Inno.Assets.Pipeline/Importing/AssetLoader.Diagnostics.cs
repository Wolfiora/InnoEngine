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
    /// Captures the current failure identity of every writable source without exposing catalog internals.
    /// </summary>
    /// <returns>
    /// An immutable health snapshot suitable for validating a later candidate generation.
    /// </returns>
    public AssetImportHealthSnapshot CaptureWritableImportHealth()
        => new(Execute(() => m_recordsByPath.Values
            .Where(record =>
                IsMounted(record.relativePath)
                && !GetMount(record.relativePath).isReadOnly
                && record.meta.importStatus is (int)AssetImportStatus.Failed
                    or (int)AssetImportStatus.Conflict)
            .Select(static record => new AssetImportFailureFingerprint(
                record.relativePath,
                record.meta.importStatus,
                record.meta.sourceHash,
                record.meta.importerId,
                string.Join("\n", record.meta.diagnostics)))
            .ToHashSet()));

    /// <summary>
    /// Finds writable-source failures that are new or changed relative to an earlier health snapshot.
    /// </summary>
    /// <param name="baseline">
    /// The health snapshot captured before the candidate generation was activated.
    /// </param>
    /// <returns>
    /// A deterministic path-ordered collection containing only introduced or changed failures.
    /// </returns>
    public IReadOnlyList<AssetImportFailure> FindIntroducedImportFailures(AssetImportHealthSnapshot baseline)
    {
        ArgumentNullException.ThrowIfNull(baseline);
        return Execute(() => m_recordsByPath.Values
            .Where(record =>
                IsMounted(record.relativePath)
                && !GetMount(record.relativePath).isReadOnly
                && record.meta.importStatus is (int)AssetImportStatus.Failed
                    or (int)AssetImportStatus.Conflict)
            .Select(static record => new AssetImportFailureFingerprint(
                record.relativePath,
                record.meta.importStatus,
                record.meta.sourceHash,
                record.meta.importerId,
                string.Join("\n", record.meta.diagnostics)))
            .Where(failure => !baseline.failures.Contains(failure))
            .OrderBy(static failure => failure.relativePath, StringComparer.Ordinal)
            .Select(static failure => new AssetImportFailure(
                failure.relativePath,
                failure.diagnostics))
            .ToArray());
    }

    private void RecordImportFailureLocked(
        string relativePath,
        byte[] sourceBytes,
        AssetImporter importer,
        Guid persistentId,
        Exception exception,
        AssetSourceFileStamp sourceStamp = default,
        AssetImportContext? attemptedContext = null
    ) {
        AssetRecord record = FindRecordLocked(relativePath) ?? new AssetRecord
        {
            relativePath = relativePath,
            persistentId = persistentId
        };
        string sourceHash = sourceBytes.Length == 0 ? string.Empty : ComputeSha256Hex(sourceBytes);
        string settingsHash;
        string? settingsFailure = null;
        try
        {
            settingsHash = ComputeSha256Hex(ReadImportSettingsBytesLocked(relativePath, importer));
        }
        catch (Exception secondaryFailure) when (
            secondaryFailure is not OutOfMemoryException &&
            RetirementPendingException.Find(secondaryFailure) is null)
        {
            settingsHash = string.Empty;
            settingsFailure = $"{secondaryFailure.GetType().Name}: {secondaryFailure.Message}";
        }
        AssetImportDependencyData[] dependencies = attemptedContext?.importDependencies
            .Select(dependency => TryCaptureFailedImportDependency(relativePath, dependency))
            .Where(static dependency => dependency.HasValue)
            .Select(static dependency => dependency!.Value)
            .ToArray() ?? [];
        string diagnostic = $"{exception.GetType().Name}: {exception.Message}";
        if (settingsFailure is not null)
            diagnostic += $" Import settings could not be inspected: {settingsFailure}";
        bool repeatedFailure = record.meta.importStatus == (int)AssetImportStatus.Failed
            && string.Equals(record.meta.sourceHash, sourceHash, StringComparison.Ordinal)
            && string.Equals(record.meta.importerSettingsHash, settingsHash, StringComparison.Ordinal)
            && record.importerGeneration == m_importers.GetGeneration(importer.importerId)
            && string.Equals(record.meta.importerImplementationFingerprint,
                GetImporterImplementationFingerprint(importer), StringComparison.Ordinal)
            && record.meta.diagnostics.Length == 1
            && string.Equals(record.meta.diagnostics[0], diagnostic, StringComparison.Ordinal)
            && record.meta.importDependencies.SequenceEqual(dependencies);
        record.relativePath = relativePath;
        record.persistentId = persistentId;
        record.meta.relativePath = relativePath;
        record.meta.persistentId = persistentId;
        record.meta.sourceHash = sourceHash;
        record.meta.importerId = importer.importerId;
        record.meta.importerImplementationFingerprint = GetImporterImplementationFingerprint(importer);
        record.meta.importerSettingsHash = settingsHash;
        record.meta.importDependencies = dependencies;
        record.importerGeneration = m_importers.GetGeneration(importer.importerId);
        ApplySourceStamp(record.meta, sourceStamp);
        record.meta.deploymentScope = (int)importer.deploymentScope;
        if (record.meta.stableAssetTypeId == Guid.Empty &&
            m_types.TryGetTypeRef(importer.targetAssetType, out TypeRef importerTypeRef))
        {
            record.meta.stableAssetTypeId = importerTypeRef.stableId;
            record.stableTypeId = importerTypeRef.stableId;
        }
        AssetImportExtensionUnavailableException? unavailable = exception as AssetImportExtensionUnavailableException;
        bool extensionUnavailable = unavailable is not null;
        bool pending = extensionUnavailable && m_deferUnavailableExtensions;
        record.meta.importStatus = (int)(pending ? AssetImportStatus.Pending : AssetImportStatus.Failed);
        record.meta.diagnostics = [pending
            ? $"Waiting for authoring extension publication (or recovery after compilation failure). {exception.Message}" +
              (settingsFailure is null ? string.Empty : $" Import settings could not be inspected: {settingsFailure}")
            : diagnostic];
        AddOrReplaceRecordLocked(record);
        // Failure belongs to the writable catalog, not the immutable source being rejected.
        if (!GetMount(relativePath).isReadOnly &&
            (ReadMetadata(GetMetaPath(relativePath)) is null || TryReadSourceMeta(GetMetaPath(relativePath), out _)))
            WriteSourceMeta(record.meta);
        CommitCatalogLocked();
        if (extensionUnavailable)
        {
            AssetSourceFileStamp.TryCapture(GetSourcePath(relativePath), out AssetSourceFileStamp pendingSourceStamp);
            AssetSourceFileStamp.TryCapture(GetMetaPath(relativePath), out AssetSourceFileStamp metadataStamp);
            m_unavailableImports[relativePath] = (m_types.current.version, pendingSourceStamp, metadataStamp,
                unavailable!.extensionKind, unavailable.extensionId);
        }
        else
            m_unavailableImports.Remove(relativePath);
        if (pending || repeatedFailure)
            return;
        m_log.Write(
            LogLevel.Error,
            "Asset import for '{0}' failed: {1}",
            [relativePath, exception]);
    }

    private AssetImportDependencyData? TryCaptureFailedImportDependency(
        string ownerPath,
        AssetImportDependency dependency
    ) {
        try
        {
            return CreateImportDependencyDataLocked(ownerPath, dependency);
        }
        catch (Exception exception) when (dependency.kind == AssetImportDependencyKind.Source
                                          && exception is InvalidOperationException or ArgumentException)
        {
            return new AssetImportDependencyData
            {
                kind = (int)dependency.kind,
                key = dependency.key,
                fingerprint = C_REJECTED_SOURCE_REFERENCE + exception.Message
            };
        }
    }

    private void TrackUnsupportedSourceLocked(string relativePath)
    {
        AssetRecord record = m_recordsByPath.GetValueOrDefault(relativePath) ?? new AssetRecord();
        // The source catalog is last-good state. An importer can be absent briefly while the
        // module/type generation that owns it is compiling or activating. Preserve a previously
        // cataloged source verbatim so a scan cannot erase its persistent identity, stable type,
        // dependency graph, or immutable artifact closure. A source that has never been imported
        // still follows the Unsupported path below.
        if (record.persistentId != Guid.Empty &&
            !record.meta.isTombstone &&
            !string.IsNullOrWhiteSpace(record.meta.importerId))
        {
            record.relativePath = relativePath;
            record.meta.relativePath = relativePath;
            AddOrReplaceRecordLocked(record);
            UpdateGraphsLocked(record);
            return;
        }
        if (record.persistentId != Guid.Empty)
            m_recordsById.Remove(record.persistentId);
        record.relativePath = relativePath;
        record.persistentId = Guid.Empty;
        record.stableTypeId = Guid.Empty;
        record.meta.relativePath = relativePath;
        record.meta.persistentId = Guid.Empty;
        record.meta.importerId = string.Empty;
        record.meta.stableAssetTypeId = Guid.Empty;
        record.meta.importStatus = (int)AssetImportStatus.Unsupported;
        record.meta.diagnostics = [$"No importer supports '{Path.GetExtension(relativePath)}'."];
        record.meta.artifactKey = string.Empty;
        m_recordsByPath[relativePath] = record;
    }

    private void RecordAmbiguousRenameDiagnosticLocked(
        string relativePath,
        int matchCount
    ) {
        AssetRecord? record = FindRecordLocked(relativePath);
        if (record is null)
            return;
        string diagnostic =
            $"Warning: source '{relativePath}' matched {matchCount} removed assets; " +
            "a new persistent identity was assigned instead of guessing a rename.";
        record.meta.diagnostics = record.meta.diagnostics
            .Append(diagnostic)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        CommitCatalogLocked();
    }

}
