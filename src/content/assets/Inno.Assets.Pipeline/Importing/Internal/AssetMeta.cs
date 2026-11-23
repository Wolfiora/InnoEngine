using System;
using Inno.Core.Serialization;

namespace Inno.Assets.Pipeline;

internal sealed class AssetMeta : ISerializable
{
    [SerializableProperty]
    internal Guid persistentId { get; set; }

    [SerializableProperty]
    internal string relativePath { get; set; } = string.Empty;

    [SerializableProperty]
    internal string sourceHash { get; set; } = string.Empty;

    [SerializableProperty]
    internal long sourceLength { get; set; } = -1;

    [SerializableProperty]
    internal long sourceLastWriteUtcTicks { get; set; }

    [SerializableProperty]
    internal long sourceCreationTimeUtcTicks { get; set; }

    [SerializableProperty]
    internal string importerId { get; set; } = string.Empty;

    [SerializableProperty]
    internal int deploymentScope { get; set; } = (int)AssetDeploymentScope.Runtime;

    [SerializableProperty]
    internal Guid stableAssetTypeId { get; set; }

    [SerializableProperty]
    internal byte[] assetStateBytes { get; set; } = [];

    [SerializableProperty]
    internal AssetDependencyData[] runtimeDependencies { get; set; } = [];

    [SerializableProperty]
    internal AssetImportDependencyData[] importDependencies { get; set; } = [];

    [SerializableProperty]
    internal int importStatus { get; set; } = (int)AssetImportStatus.Pending;

    [SerializableProperty]
    internal string importerImplementationFingerprint { get; set; } = string.Empty;

    [SerializableProperty]
    internal string importerSettingsHash { get; set; } = string.Empty;

    [SerializableProperty]
    internal string artifactKey { get; set; } = string.Empty;

    [SerializableProperty]
    internal string lastSuccessfulArtifactKey { get; set; } = string.Empty;

    [SerializableProperty]
    internal string[] diagnostics { get; set; } = [];

    [SerializableProperty]
    internal bool isDirectory { get; set; }

    [SerializableProperty]
    internal bool isTombstone { get; set; }
}
