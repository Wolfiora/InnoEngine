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
    private bool IsStale(
        AssetRecord record,
        out bool catalogChanged
    ) {
        catalogChanged = false;
        string sourcePath = GetSourcePath(record.relativePath);
        if (!IOFile.Exists(sourcePath))
            return true;
        AssetImporter? importer = m_importers.FindById(record.meta.importerId);
        if (importer is null ||
            !string.Equals(
                record.meta.importerImplementationFingerprint,
                GetImporterImplementationFingerprint(importer),
                StringComparison.Ordinal))
        {
            return true;
        }
        if (record.importerGeneration != m_importers.GetGeneration(record.meta.importerId))
            return true;
        if (record.meta.importStatus != (int)AssetImportStatus.Imported &&
            record.meta.importStatus != (int)AssetImportStatus.Failed)
            return true;
        if (record.meta.importStatus == (int)AssetImportStatus.Failed &&
            record.meta.diagnostics.Any(static diagnostic =>
                diagnostic.StartsWith("IOException:", StringComparison.Ordinal) ||
                diagnostic.StartsWith("UnauthorizedAccessException:", StringComparison.Ordinal)))
        {
            // File locks and access checks may clear without any source or importer change.
            return true;
        }
        if (AreImportSettingsStaleLocked(record, importer))
            return true;
        if (!AssetSourceFileStamp.TryCapture(sourcePath, out AssetSourceFileStamp sourceStamp))
            return true;
        if (!SourceStampMatches(record.meta, sourceStamp))
        {
            byte[] sourceBytes = ReadStableSourceBytes(sourcePath, out sourceStamp);
            if (!string.Equals(
                    record.meta.sourceHash,
                    ComputeSha256Hex(sourceBytes),
                    StringComparison.Ordinal))
            {
                return true;
            }
            ApplySourceStamp(record.meta, sourceStamp);
            catalogChanged = true;
        }
        for (int i = 0; i < record.meta.importDependencies.Length; i++)
        {
            AssetImportDependencyData dependency = record.meta.importDependencies[i];
            if (record.meta.importStatus == (int)AssetImportStatus.Failed
                && dependency.fingerprint.StartsWith(C_REJECTED_SOURCE_REFERENCE, StringComparison.Ordinal))
            {
                try
                {
                    ValidateSourceReferenceLocked(record.relativePath,
                        NormalizeRelativePath(dependency.key));
                    return true;
                }
                catch (Exception exception) when (exception is InvalidOperationException or ArgumentException)
                {
                    continue;
                }
            }
            string fingerprint = ComputeImportDependencyFingerprintLocked(
                ref dependency,
                out bool dependencyChanged);
            if (dependencyChanged)
            {
                record.meta.importDependencies[i] = dependency;
                catalogChanged = true;
            }
            if (!string.Equals(
                    dependency.fingerprint,
                    fingerprint,
                    StringComparison.Ordinal))
            {
                return true;
            }
        }
        return false;
    }

    private string NormalizeRelativePath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentException("An asset-relative path is required.", nameof(relativePath));
        AssetPath path = AssetPath.Parse(relativePath);
        _ = GetMount(path);
        return path.ToString();
    }

    private string NormalizeAssetPath(AssetPath path)
    {
        if (!path.isValid || string.IsNullOrWhiteSpace(path.localPath))
            throw new ArgumentException("An isolated asset path is required.", nameof(path));
        _ = GetMount(path);
        return path.ToString();
    }

    private void EnsureDirectoryMetadataLocked()
    {
        foreach (AssetSourceMount mount in m_mounts.Values)
        {
            foreach (string directoryPath in Directory.GetDirectories(
                         mount.rootPath,
                         "*",
                         SearchOption.AllDirectories))
            {
                string localPath = Path.GetRelativePath(mount.rootPath, directoryPath).Replace('\\', '/');
                string relativePath = new AssetPath(mount.id, localPath).ToString();
                if (IsSourceIgnored(new AssetPath(mount.id, localPath), isDirectory: true))
                    continue;
                string metaPath = GetMetaPath(relativePath);
                AssetSourceMeta sourceMeta;
                if (!TryReadSourceMeta(metaPath, out sourceMeta!))
                {
                    if (mount.isReadOnly)
                    {
                        throw new InvalidDataException(
                            $"Read-only source directory '{relativePath}' requires a valid '{C_META_POSTFIX}' sidecar.");
                    }
                    sourceMeta = new AssetSourceMeta
                    {
                        persistentId = Guid.NewGuid(),
                        sourceKind = (int)AssetSourceKind.Directory
                    };
                    WriteAtomic(metaPath, m_serialization.Serialize(sourceMeta));
                }

                AssetRecord? record = FindRecordByIdWithoutLoading(sourceMeta.persistentId);
                if (record is not null &&
                    !string.Equals(record.relativePath, relativePath, StringComparison.OrdinalIgnoreCase) &&
                    Directory.Exists(GetSourcePath(record.relativePath)))
                {
                    if (mount.isReadOnly)
                    {
                        throw new InvalidDataException(
                            $"Read-only source directory '{relativePath}' duplicates persistent ID '{sourceMeta.persistentId}'.");
                    }
                    sourceMeta.persistentId = Guid.NewGuid();
                    WriteAtomic(metaPath, m_serialization.Serialize(sourceMeta));
                    record = null;
                }
                record ??= new AssetRecord();
                record.relativePath = relativePath;
                record.persistentId = sourceMeta.persistentId;
                record.meta.relativePath = relativePath;
                record.meta.persistentId = sourceMeta.persistentId;
                record.meta.isDirectory = true;
                record.meta.isTombstone = false;
                record.meta.importStatus = (int)AssetImportStatus.Imported;
                record.meta.diagnostics = [];
                AddOrReplaceRecordLocked(record);
            }
        }
    }

    private void WriteSourceMeta(AssetMeta meta)
    {
        string metaPath = GetMetaPath(meta.relativePath);
        byte[]? existingBytes = ReadMetadata(metaPath);
        AssetSourceMeta? existing = existingBytes is null
            ? null : m_serialization.Deserialize<AssetSourceMeta>(existingBytes);
        var sourceMeta = new AssetSourceMeta
        {
            persistentId = meta.persistentId,
            sourceKind = meta.isDirectory
                ? (int)AssetSourceKind.Directory
                : (int)AssetSourceKind.File,
            importerId = meta.importerId,
            importerSettingsBytes = existing?.importerSettingsBytes ?? []
        };
        byte[] data = m_serialization.Serialize(sourceMeta);
        if (GetMount(meta.relativePath).isReadOnly)
        {
            if (!IOFile.Exists(metaPath) || !IOFile.ReadAllBytes(metaPath).AsSpan().SequenceEqual(data))
            {
                throw new InvalidDataException(
                    $"Read-only source metadata for '{meta.relativePath}' is missing or inconsistent.");
            }
            return;
        }
        WriteAtomic(metaPath, data);
    }

    private bool TryReadSourceMeta(
        string metaPath,
        out AssetSourceMeta sourceMeta
    ) {
        sourceMeta = null!;
        byte[]? bytes = ReadMetadata(metaPath);
        if (bytes is null)
            return false;
        try
        {
            sourceMeta = m_serialization.Deserialize<AssetSourceMeta>(bytes);
            return sourceMeta.persistentId != Guid.Empty;
        }
        catch
        {
            return false;
        }
    }

    private string CreateImportFingerprint(AssetMeta meta)
    {
        var parts = new List<string>
        {
            "Inno.AssetImport",
            meta.sourceHash,
            meta.importerId,
            meta.importerImplementationFingerprint,
            meta.importerSettingsHash
        };
        foreach (AssetDependencyData dependency in meta.runtimeDependencies
                     .OrderBy(static value => value.persistentId))
        {
            parts.Add(dependency.persistentId.ToString("D"));
            if (m_recordsById.TryGetValue(dependency.persistentId, out AssetRecord? record))
                parts.Add(record.meta.artifactKey);
        }
        foreach (AssetImportDependencyData dependency in meta.importDependencies
                     .OrderBy(static value => value.kind)
                     .ThenBy(static value => value.key, StringComparer.Ordinal))
        {
            parts.Add(dependency.kind.ToString());
            parts.Add(dependency.key);
            parts.Add(dependency.fingerprint);
        }
        return string.Join("\n", parts);
    }

    private static string GetImporterImplementationFingerprint(AssetImporter importer)
        => $"{importer.GetType().Assembly.ManifestModule.ModuleVersionId:D}:" +
           $"{importer.GetType().FullName}:{importer.deploymentScope}";

    private string CreateBuildFingerprint(
        AssetBuildProcessor processor,
        AssetObject definition,
        IReadOnlyList<AssetInfo> inputs
    ) {
        var parts = new List<string>
        {
            "Inno.AssetBuild",
            processor.processorId,
            processor.GetType().Assembly.ManifestModule.ModuleVersionId.ToString("D"),
            definition.identity.persistentId.ToString("D"),
            definition.GetType().Assembly.ManifestModule.ModuleVersionId.ToString("D"),
            definition.GetType().FullName ?? definition.GetType().Name,
            m_runtimeOwner.GetSourceHash(definition),
            ComputeSha256Hex(CaptureAssetState(definition)),
            ComputeSha256Hex(definition.runtimePayload.ToArray())
        };
        foreach (AssetInfo input in inputs.OrderBy(static value => value.persistentId))
        {
            parts.Add(input.persistentId.ToString("D"));
            parts.Add(input.artifactKey.value);
        }
        return string.Join("\n", parts);
    }

    private string GetSourcePath(string relativePath)
    {
        AssetPath path = AssetPath.Parse(relativePath);
        return GetMount(path).Resolve(path.localPath);
    }

    private AssetSourceMount GetMount(string canonicalPath) => GetMount(AssetPath.Parse(canonicalPath));

    private AssetSourceMount GetMount(AssetPath path)
        => m_mounts.TryGetValue(path.source, out AssetSourceMount? mount)
            ? mount
            : throw new ArgumentException($"Asset source mount '{path.source}' is not active.", nameof(path));

    private string GetMetaPath(string relativePath) => GetSourcePath(relativePath) + C_META_POSTFIX;

    private static byte[] ReadStableSourceBytes(
        string sourcePath,
        out AssetSourceFileStamp sourceStamp
    ) {
        const int C_MAX_ATTEMPTS = 3;
        for (int attempt = 0; attempt < C_MAX_ATTEMPTS; attempt++)
        {
            if (!AssetSourceFileStamp.TryCapture(sourcePath, out AssetSourceFileStamp before))
                throw new FileNotFoundException("Asset source is unavailable.", sourcePath);
            byte[] bytes = IOFile.ReadAllBytes(sourcePath);
            if (AssetSourceFileStamp.TryCapture(sourcePath, out AssetSourceFileStamp after) &&
                before == after &&
                bytes.LongLength == after.length)
            {
                sourceStamp = after;
                return bytes;
            }
        }

        throw new IOException($"Asset source '{sourcePath}' did not remain stable while it was read.");
    }

    private static bool SourceStampMatches(
        AssetMeta meta,
        AssetSourceFileStamp sourceStamp
    )
        => sourceStamp.isValid &&
           meta.sourceLength == sourceStamp.length &&
           meta.sourceLastWriteUtcTicks == sourceStamp.lastWriteUtcTicks &&
           meta.sourceCreationTimeUtcTicks == sourceStamp.creationTimeUtcTicks;

    private static bool SourceStampMatches(
        AssetImportDependencyData dependency,
        AssetSourceFileStamp sourceStamp
    )
        => dependency.sourceStampValid &&
           sourceStamp.isValid &&
           dependency.sourceLength == sourceStamp.length &&
           dependency.sourceLastWriteUtcTicks == sourceStamp.lastWriteUtcTicks &&
           dependency.sourceCreationTimeUtcTicks == sourceStamp.creationTimeUtcTicks;

    private static void ApplySourceStamp(
        AssetMeta meta,
        AssetSourceFileStamp sourceStamp
    ) {
        if (!sourceStamp.isValid)
        {
            meta.sourceLength = -1;
            meta.sourceLastWriteUtcTicks = 0;
            meta.sourceCreationTimeUtcTicks = 0;
            return;
        }

        meta.sourceLength = sourceStamp.length;
        meta.sourceLastWriteUtcTicks = sourceStamp.lastWriteUtcTicks;
        meta.sourceCreationTimeUtcTicks = sourceStamp.creationTimeUtcTicks;
    }

    private static void ApplySourceStamp(
        ref AssetImportDependencyData dependency,
        AssetSourceFileStamp sourceStamp
    ) {
        dependency.sourceStampValid = sourceStamp.isValid;
        dependency.sourceLength = sourceStamp.length;
        dependency.sourceLastWriteUtcTicks = sourceStamp.lastWriteUtcTicks;
        dependency.sourceCreationTimeUtcTicks = sourceStamp.creationTimeUtcTicks;
    }

    private static string ComputeSha256Hex(byte[] bytes) => Convert.ToHexString(SHA256.HashData(bytes));

    private bool IsInternalGeneratedPath(string relativePath) => AssetSourcePolicy.IsGeneratedPath(relativePath);

    private byte[]? ReadMetadata(string path)
        => m_sourceMetadataStage is not null
            ? m_sourceMetadataStage.Read(path)
            : IOFile.Exists(path) ? IOFile.ReadAllBytes(path) : null;

    private bool IsStagedMetadata(string path)
        => m_sourceMetadataStage is not null && path.EndsWith(C_META_POSTFIX, StringComparison.Ordinal);

    private void WriteAtomic(
        string targetPath,
        byte[] bytes
    ) {
        if (IsStagedMetadata(targetPath))
            m_sourceMetadataStage!.Write(targetPath, bytes);
        else
            AtomicFile.WriteAllBytes(targetPath, bytes);
    }

    private FileSnapshot CaptureFile(string path)
    {
        byte[]? bytes = IsStagedMetadata(path) ? ReadMetadata(path) : IOFile.Exists(path) ? IOFile.ReadAllBytes(path) : null;
        return bytes is null ? new FileSnapshot(false, []) : new FileSnapshot(true, bytes);
    }

    private void RestoreFile(
        string path,
        FileSnapshot snapshot
    ) {
        if (snapshot.existed)
        {
            WriteAtomic(path, snapshot.bytes);
            return;
        }

        DeleteIfExists(path);
    }

    private void MoveGeneratedFile(
        string sourcePath,
        string targetPath
    ) {
        if (IsStagedMetadata(sourcePath))
        {
            byte[]? bytes = ReadMetadata(sourcePath);
            if (bytes is null)
                return;
            if (ReadMetadata(targetPath) is not null)
                throw new IOException($"Generated metadata target '{targetPath}' already exists.");
            m_sourceMetadataStage!.Write(targetPath, bytes);
            m_sourceMetadataStage.Write(sourcePath, null);
            return;
        }
        if (!IOFile.Exists(sourcePath))
            return;
        Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
        if (IOFile.Exists(targetPath))
            throw new IOException($"Generated metadata target '{targetPath}' already exists.");
        IOFile.Move(sourcePath, targetPath);
    }

    private void DeleteIfExists(string path)
    {
        if (IsStagedMetadata(path))
        {
            m_sourceMetadataStage!.Write(path, null);
            return;
        }
        if (IOFile.Exists(path))
            IOFile.Delete(path);
    }

    private static AssetDependencyData ToData(AssetDependency dependency) => new()
    {
        persistentId = dependency.persistentId,
        stableTypeId = dependency.type.stableId,
        lastKnownPath = dependency.lastKnownPath
    };

    private readonly record struct FileSnapshot(
        bool existed,
        byte[] bytes
    );

}
