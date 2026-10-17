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

/// <summary>
/// Coordinates importing, persistent cataloging, canonical loading, reloading and collection
/// for one source and artifact root pair.
/// </summary>
public sealed partial class AssetLoader : IDisposable, IAssetReferenceResolver, IAssetArtifactLookup, IAssetPropertyStateResolver
{
    internal const string C_META_POSTFIX = ".imeta";
    private const string C_REJECTED_SOURCE_REFERENCE = "REJECTED_SOURCE_REFERENCE:";

    [ThreadStatic]
    private static AssetLoader? t_activeLoader;

    private readonly AssetRuntimeOwner m_runtimeOwner;
    private readonly ArtifactRetention m_artifactRetention = new();
    private readonly SemaphoreSlim m_operationGate = new(1, 1);
    private readonly object m_asyncSync = new();
    private readonly Dictionary<string, Task<AssetObject?>> m_inFlightPathLoads =
        new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<Guid, Task<AssetObject?>> m_inFlightIdLoads = [];
    private readonly AssetImporterRegistry m_importers;
    private readonly AssetBuildProcessorRegistry m_buildProcessors;
    private readonly Dictionary<string, AssetRecord> m_recordsByPath = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<Guid, AssetRecord> m_recordsById = [];
    private readonly Dictionary<Guid, WeakReference<AssetObject>> m_missingAssets = [];
    private readonly Dictionary<Guid, SerializedMissingState> m_preservedMissingStates = [];
    private readonly DependencyGraph<Guid> m_runtimeGraph = new();
    private readonly DependencyGraph<string> m_importGraph = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConditionalWeakTable<AssetObject, AssetDependencySet> m_dependencyRetention = new();
    private readonly HashSet<string> m_activeImports = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Guid> m_pendingImportIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, (long generation, AssetSourceFileStamp source, AssetSourceFileStamp metadata,
        string kind, string id)> m_unavailableImports = new(StringComparer.OrdinalIgnoreCase);
    private readonly AssetArtifactStore m_artifacts;
    private readonly AssetCatalogStore m_catalog;
    private readonly AssetDiagnosticPublisher m_diagnostics;
    private readonly DiagnosticHub m_diagnosticHub;
    private readonly IdentityAllocator m_identities;
    private readonly LogRouter m_logs;
    private readonly Logger m_log;
    private readonly SerializationRegistry m_serialization;
    private readonly SerializationContext m_serializationContext;
    private readonly AssetSourcePolicy m_sourcePolicy;
    private readonly TypeCatalog m_types;
    private readonly IReadOnlyDictionary<AssetSourceId, AssetSourceMount> m_mounts;
    private readonly bool m_runtimeArtifactsOnly;

    private CancellationToken m_importCancellation;
    private bool m_disposed;
    private bool m_disposeRequested;
    private int m_admittedOperations;
    private bool m_retirementActive;
    private bool m_identitiesActive = true;
    private bool m_deferUnavailableExtensions;
    private bool m_preserveInitialExtensionDiscovery;
    private LifetimeScope? m_retirement;
    private AssetSourceMetadataStage? m_sourceMetadataStage;
    private long m_importerRegistryVersion = -1;
    private long m_buildProcessorRegistryVersion = -1;

    /// <summary>
    /// Restores serialized properties to the existing asset object.
    /// </summary>
    /// <param name="stableTypeId">
    /// The stable type id consumed by restore properties; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="propertyData">
    /// The property data consumed by restore properties; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    /// <typeparam name="TValue">
    /// Serialized asset object type receiving restored properties.
    /// </typeparam>
    public void RestoreProperties<TValue>(
        Guid stableTypeId,
        byte[] propertyData,
        TValue target
    ) where TValue : class, ISerializable
    {
        ObjectDisposedException.ThrowIf(m_disposed || m_disposeRequested, this);
        ArgumentNullException.ThrowIfNull(target);
        if (m_types.GetTypeRef(target.GetType()).stableId != stableTypeId)
            throw new InvalidOperationException("The asset property payload has an incompatible stable type identity.");
        m_serialization.Decode(propertyData, reader =>
        {
            reader.RestoreProperties(target);
            return true;
        }, m_serializationContext);
    }

    /// <summary>
    /// Creates an asset loader for one source and Library root pair.
    /// </summary>
    /// <param name="types">
    /// The immutable type-generation owner used to discover importers and resolve asset types.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry used for catalog, metadata, and asset state persistence.
    /// </param>
    /// <param name="identities">
    /// The identity allocator that owns every canonical asset loaded by this instance.
    /// </param>
    /// <param name="logs">
    /// The explicitly owned router receiving importer and catalog diagnostics.
    /// </param>
    /// <param name="diagnostics">
    /// The diagnostic hub that owns import, build, catalog, and reference reports.
    /// </param>
    /// <param name="assetRoot">
    /// The absolute source root.
    /// </param>
    /// <param name="libraryRoot">
    /// The absolute rebuildable Library root.
    /// </param>
    /// <param name="sourcePolicy">
    /// The source filtering policy, or <see langword="null"/> for defaults.
    /// </param>
    public AssetLoader(
        TypeCatalog types,
        SerializationRegistry serialization,
        IdentityAllocator identities,
        DiagnosticHub diagnostics,
        LogRouter logs,
        string assetRoot,
        string libraryRoot,
        AssetSourcePolicy? sourcePolicy = null
    )
        : this(
            types,
            serialization,
            identities,
            diagnostics,
            logs,
            [new AssetSourceMount(AssetSourceId.project, assetRoot, isReadOnly: false)],
            libraryRoot,
            sourcePolicy)
    {
    }

    /// <summary>
    /// Creates an asset loader over one project source and zero or more isolated sources.
    /// </summary>
    /// <param name="types">
    /// The immutable type-generation owner used to discover importers and resolve asset types.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry used for catalog, metadata, and asset state persistence.
    /// </param>
    /// <param name="identities">
    /// The identity allocator that owns every canonical asset loaded by this instance.
    /// </param>
    /// <param name="logs">
    /// The explicitly owned router receiving importer and catalog diagnostics.
    /// </param>
    /// <param name="diagnostics">
    /// The diagnostic hub that owns import, build, catalog, and reference reports.
    /// </param>
    /// <param name="mounts">
    /// Complete isolated source mount snapshot.
    /// </param>
    /// <param name="libraryRoot">
    /// Absolute rebuildable Library root.
    /// </param>
    /// <param name="sourcePolicy">
    /// Source filtering policy, or <see langword="null"/> for defaults.
    /// </param>
    /// <param name="runtimeArtifactsOnly">
    /// Whether to trust a deployed read-only catalog and skip every source reconciliation operation.
    /// </param>
    public AssetLoader(
        TypeCatalog types,
        SerializationRegistry serialization,
        IdentityAllocator identities,
        DiagnosticHub diagnostics,
        LogRouter logs,
        IReadOnlyList<AssetSourceMount> mounts,
        string libraryRoot,
        AssetSourcePolicy? sourcePolicy = null,
        bool runtimeArtifactsOnly = false
    )
        : this(
            types,
            serialization,
            identities,
            diagnostics,
            logs,
            mounts,
            libraryRoot,
            libraryRoot,
            sourcePolicy,
            runtimeArtifactsOnly)
    {
    }

    internal AssetLoader(
        TypeCatalog types,
        SerializationRegistry serialization,
        IdentityAllocator identities,
        DiagnosticHub diagnostics,
        LogRouter logs,
        IReadOnlyList<AssetSourceMount> mounts,
        string libraryRoot,
        string catalogLibraryRoot,
        AssetSourcePolicy? sourcePolicy,
        bool runtimeArtifactsOnly = false
    ) {
        m_runtimeOwner = new(this);
        ArgumentNullException.ThrowIfNull(types);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(identities);
        ArgumentNullException.ThrowIfNull(diagnostics);
        ArgumentNullException.ThrowIfNull(logs);
        ArgumentNullException.ThrowIfNull(mounts);
        if (mounts.Count == 0)
            throw new ArgumentException("At least one asset source mount is required.", nameof(mounts));
        if (string.IsNullOrWhiteSpace(libraryRoot))
            throw new ArgumentException("Library root is required.", nameof(libraryRoot));
        if (string.IsNullOrWhiteSpace(catalogLibraryRoot))
            throw new ArgumentException("Catalog Library root is required.", nameof(catalogLibraryRoot));
        Dictionary<AssetSourceId, AssetSourceMount> byId = mounts.ToDictionary(static mount => mount.id);
        if (byId.Count != mounts.Count)
            throw new ArgumentException("Asset source mount IDs must be unique.", nameof(mounts));
        if (!byId.TryGetValue(AssetSourceId.project, out AssetSourceMount? project)
            || project.isReadOnly && !runtimeArtifactsOnly)
        {
            throw new ArgumentException(
                runtimeArtifactsOnly
                    ? "A project asset source mount is required."
                    : "A writable project asset source mount is required.",
                nameof(mounts));
        }

        m_types = types;
        m_serialization = serialization;
        m_serializationContext = AssetSerializationContext.Create(this);
        m_identities = identities;
        m_diagnosticHub = diagnostics;
        m_diagnostics = new AssetDiagnosticPublisher(diagnostics);
        m_logs = logs;
        m_log = logs.CreateLogger<AssetLoader>();
        m_importers = new AssetImporterRegistry(types);
        m_buildProcessors = new AssetBuildProcessorRegistry(types);
        m_mounts = byId;
        m_runtimeArtifactsOnly = runtimeArtifactsOnly;
        assetRoot = project.rootPath;
        this.libraryRoot = Path.GetFullPath(libraryRoot);
        foreach (AssetSourceMount mount in mounts)
        {
            if (mount.isReadOnly && !Directory.Exists(mount.rootPath))
            {
                throw new DirectoryNotFoundException(
                    $"Read-only asset source root '{mount.rootPath}' does not exist.");
            }

            Directory.CreateDirectory(mount.rootPath);
        }
        Directory.CreateDirectory(this.libraryRoot);
        m_sourcePolicy = sourcePolicy ?? AssetSourcePolicy.defaultPolicy;
        m_artifacts = new AssetArtifactStore(this.libraryRoot, serialization);
        m_catalog = new AssetCatalogStore(catalogLibraryRoot, serialization);
    }

    /// <summary>
    /// Gets the absolute source root.
    /// </summary>
    public string assetRoot { get; }

    /// <summary>
    /// Gets the absolute rebuildable Library root.
    /// </summary>
    public string libraryRoot { get; }

    /// <summary>
    /// Gets the derived content-addressed artifact root.
    /// </summary>
    public string artifactRoot => m_artifacts.root;

    /// <summary>
    /// Occurs after a loaded canonical asset is updated in place.
    /// </summary>
    public event Action<AssetObject>? AssetReloaded;

    internal void DeferUnavailableExtensions() => Execute(() =>
    {
        m_deferUnavailableExtensions = true;
    });

    internal void CompleteExtensionDiscovery()
        => Execute(() =>
        {
            if (!m_deferUnavailableExtensions)
                return;
            m_deferUnavailableExtensions = false;
            m_unavailableImports.Clear();
            RescanLocked();
        });

    internal void ActivateExtensionDiscovery()
    {
        // Assembly discovery may publish built-in catalogs before the initial script compilation.
        // Only the composition host can close that initial discovery boundary.
        if (!m_preserveInitialExtensionDiscovery)
            CompleteExtensionDiscovery();
    }

    /// <summary>
    /// Waits for pending import and build work.
    /// </summary>
    public void WaitForIdle() => Execute(static () => { });

    /// <summary>
    /// Refreshes extension registries and reimports affected sources when their snapshot changed.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the importer registry changed.
    /// </returns>
    public bool RefreshRegistries()
    {
        return Execute(() =>
        {
            long version = m_importers.snapshotVersion;
            long buildVersion = m_buildProcessors.snapshotVersion;
            if (version == m_importerRegistryVersion &&
                buildVersion == m_buildProcessorRegistryVersion)
                return false;
            RescanLocked();
            return true;
        });
    }

    private void PublishReloaded(AssetObject asset)
    {
        Action<AssetObject>? handlers = AssetReloaded;
        if (handlers is null)
            return;
        foreach (Delegate handler in handlers.GetInvocationList())
        {
            try
            {
                ((Action<AssetObject>)handler)(asset);
            }
            catch
            {
                // Observer failures cannot roll back an already committed asset transaction.
            }
        }
    }

    private static AssetDependency FindDescriptor(
        AssetMeta meta,
        Guid persistentId
    ) {
        AssetDependencyData data = meta.runtimeDependencies.FirstOrDefault(value => value.persistentId == persistentId);
        return data.persistentId == Guid.Empty
            ? default
            : new AssetDependency(data.persistentId, new TypeRef(data.stableTypeId), data.lastKnownPath);
    }
}
