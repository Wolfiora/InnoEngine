using System.Linq;
using Inno.Assets.Pipeline;
using Inno.Core.Settings;

namespace Inno.Plugins.Authoring;

/// <summary>
/// Describes one validated installed Plugin source candidate.
/// </summary>
public sealed class PluginCandidate
{
    private readonly PluginManifest m_manifest;

    internal PluginCandidate(
        string sourcePath,
        PluginSourceKind sourceKind,
        string contentHash,
        PluginManifest manifest,
        AssetSourceMount sourceMount,
        bool containsCode
    ) {
        this.sourcePath = sourcePath;
        this.sourceKind = sourceKind;
        this.contentHash = contentHash;
        m_manifest = CloneManifest(manifest);
        this.sourceMount = sourceMount;
        this.containsCode = containsCode;
    }

    /// <summary>
    /// Gets the installed <c>.iplugin</c> package path.
    /// </summary>
    public string sourcePath { get; }

    /// <summary>
    /// Gets the physical source container kind.
    /// </summary>
    public PluginSourceKind sourceKind { get; }

    /// <summary>
    /// Gets the deterministic complete source-content hash.
    /// </summary>
    public string contentHash { get; }

    /// <summary>
    /// Gets a detached copy of the validated native manifest, including its nested contribution payloads.
    /// </summary>
    public PluginManifest manifest => CloneManifest(m_manifest);

    /// <summary>
    /// Gets the isolated read-only asset source mount.
    /// </summary>
    public AssetSourceMount sourceMount { get; }

    /// <summary>
    /// Gets whether source code exists in this Plugin.
    /// </summary>
    public bool containsCode { get; }

    private static PluginManifest CloneManifest(PluginManifest value)
        => new()
    {
        pluginId = value.pluginId,
        displayName = value.displayName,
        dependencies = value.dependencies.ToArray(),
        overrides = value.overrides.ToArray(),
        contentRoots = value.contentRoots.ToArray(),
        assemblyDefinitions = value.assemblyDefinitions.ToArray(),
        settingContributions = value.settingContributions.Select(static record => new ProjectSettingRecord(
            record.id,
            record.stableTypeId,
            record.propertyData
        )).ToArray()
    };
}
