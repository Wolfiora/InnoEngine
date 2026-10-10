using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Extensibility.Modules;
using Inno.Core.Identity;
using Inno.Core.Diagnostics;
using Inno.Core.Logging;
using Inno.Extensibility.Types;
using Inno.Scripting.Api;
using Inno.Core.Serialization;
using Inno.Core.Execution;
using Inno.Extensibility.Reload;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Provides the single application-level entry point for importing, loading, saving and
/// collecting assets.
/// </summary>
public sealed partial class AssetPipeline : AssetResidencyProvider,
    IDisposable,
    IAssetLookup,
    IAssetReferenceResolver,
    IAssetPropertyStateResolver,
    IAssetArtifactLookup,
    IAssetResidency,
    IAssetSourceSnapshot
{
    private readonly Lock m_lifecycleLock = new();
    private readonly AssetCatalogParticipant m_catalogParticipant;
    private readonly AssetPipelineDiagnosticPublisher m_diagnostics;
    private readonly Logger m_log;
    private readonly SerializationRegistry m_serialization;
    private readonly TypeCatalog m_types;
    private readonly IdentityAllocator m_identities;
    private readonly GenerationCoordinator m_generations;

    private AssetLoader? m_loader;
    private AssetFileSystem? m_fileSystem;
    private IDisposable? m_catalogParticipantRegistration;
    private int m_ownerThreadId;
    private long m_revision;
    private AssetCacheOptions m_cacheOptions;
    private long m_lastArtifactCollectionTimestamp;
    private AssetPipelineOptions m_options;
    private AssetSourceMountTransaction? m_sourceMountCandidate;
    private LifetimeScope? m_failedPreparation;
    private LifetimeScope? m_shutdown;
    private RetirementBarrier? m_shutdownBarrier;

    /// <summary>
    /// Gets whether asset services are initialized.
    /// </summary>
    public bool isInitialized { get; private set; }

    /// <summary>
    /// Gets the absolute source asset root.
    /// </summary>
    public string assetRoot { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the absolute root containing rebuildable asset database data.
    /// </summary>
    public string libraryRoot { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the absolute generated artifact root.
    /// </summary>
    public string artifactRoot { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the monotonic identity of the current committed asset and source-mount state.
    /// </summary>
    [ScriptingApiIgnore]
    public long revision => Interlocked.Read(ref m_revision);

    /// <summary>
    /// Gets the active isolated source mount snapshot.
    /// </summary>
    [ScriptingApiIgnore]
    public IReadOnlyList<AssetSourceMount> sourceMounts { get; private set; } = [];

    /// <summary>
    /// Gets the authoring identity domain shared by canonical assets and addressable source entries.
    /// </summary>
    [ScriptingApiIgnore]
    public IdentityAllocator identities => m_identities;

    /// <summary>
    /// Creates a detached source editor sharing this owner's mounts, native converters and references.
    /// </summary>
    /// <returns>
    /// A source store that must not outlive this asset pipeline.
    /// </returns>
    [ScriptingApiIgnore]
    public AssetSourceStore CreateSourceStore() => new(this, new AssetSerializationServices(m_types, m_serialization, this, null));

    /// <summary>
    /// Captures native settings and nested asset dependencies through this explicit authoring owner.
    /// </summary>
    /// <typeparam name="TValue">
    /// Current settings type.
    /// </typeparam>
    /// <param name="value">
    /// Typed settings, never retained by the returned snapshot.
    /// </param>
    /// <returns>
    /// Complete stable properties and dependencies without writing or importing an asset.
    /// </returns>
    public AssetPropertySnapshot CaptureProperties<TValue>(TValue value) where TValue : class, ISerializable
    {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        return new AssetSerializationServices(m_types, m_serialization, this, null).CaptureProperties(value);
    }

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
        ArgumentNullException.ThrowIfNull(target);
        using IDisposable operationScope = AcquireOperation();
        if (m_types.GetTypeRef(target.GetType()).stableId != stableTypeId)
            throw new InvalidOperationException("The asset property payload has an incompatible stable type identity.");
        m_serialization.Decode(propertyData, reader =>
        {
            reader.RestoreProperties(target);
            return true;
        }, AssetSerializationContext.Create(this));
    }

    /// <summary>
    /// Occurs after an asset database transaction has committed.
    /// </summary>
    public event Action<AssetChangeSet>? Changed;

    /// <summary>
    /// Occurs after a canonical loaded asset has been updated in place.
    /// </summary>
    public event Action<AssetObject>? AssetReloaded;

    /// <summary>
    /// Occurs after a complete isolated source-mount generation is atomically replaced.
    /// </summary>
    [ScriptingApiIgnore]
    public event Action? SourceMountsChanged;

    /// <summary>
    /// Creates one isolated authoring or deployed-runtime asset pipeline.
    /// </summary>
    /// <param name="modules">
    /// The module host whose candidate generations coordinate importer refreshes.
    /// </param>
    /// <param name="types">
    /// The type catalog used to discover and resolve asset extensions.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry used for metadata and canonical asset state.
    /// </param>
    /// <param name="identities">
    /// The identity allocator that owns every canonical asset in this pipeline.
    /// </param>
    /// <param name="logs">
    /// The explicitly owned router receiving asset pipeline diagnostics.
    /// </param>
    /// <param name="diagnostics">
    /// The diagnostic hub that owns source database and asset processing reports.
    /// </param>
    /// <param name="options">
    /// The asset source, artifact and watcher configuration.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when a required service is null.
    /// </exception>
    [ScriptingApiIgnore]
    public AssetPipeline(
        ModuleHost modules,
        TypeCatalog types,
        SerializationRegistry serialization,
        IdentityAllocator identities,
        DiagnosticHub diagnostics,
        LogRouter logs,
        AssetPipelineOptions options
    ) {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(types);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(identities);
        ArgumentNullException.ThrowIfNull(diagnostics);
        ArgumentNullException.ThrowIfNull(logs);
        if (string.IsNullOrWhiteSpace(options.assetRoot))
            throw new ArgumentException("Asset root is required.", nameof(options));
        if (string.IsNullOrWhiteSpace(options.libraryRoot))
            throw new ArgumentException("Library root is required.", nameof(options));

        m_types = types;
        m_generations = modules.generations;
        m_identities = identities;
        m_serialization = serialization;
        m_diagnostics = new AssetPipelineDiagnosticPublisher(diagnostics);
        m_log = logs.CreateLogger<AssetPipeline>();
        m_catalogParticipant = new AssetCatalogParticipant(this);

        lock (m_lifecycleLock)
        {
            AssetSourceMount[] mounts = options.sourceMounts?.ToArray()
                ?? [new AssetSourceMount(AssetSourceId.project, options.assetRoot, isReadOnly: false)];
            AssetSourceMount projectMount = mounts.SingleOrDefault(static mount => mount.id == AssetSourceId.project)
                ?? throw new ArgumentException("A project asset source mount is required.", nameof(options));
            if (projectMount.isReadOnly && options.mode != AssetPipelineMode.RuntimeArtifacts)
                throw new ArgumentException("The project asset source mount must be writable.", nameof(options));
            assetRoot = projectMount.rootPath;
            libraryRoot = Path.GetFullPath(options.libraryRoot);
            DeleteCandidateCatalogRoots(libraryRoot);
            artifactRoot = Path.Combine(libraryRoot, "Artifacts");
            sourceMounts = Array.AsReadOnly(mounts.ToArray());
            AssetLoader loader = new(
                types,
                serialization,
                identities,
                diagnostics,
                logs,
                mounts,
                libraryRoot,
                options.sourcePolicy,
                runtimeArtifactsOnly: options.mode == AssetPipelineMode.RuntimeArtifacts);
            if (options.deferUnavailableExtensions)
                loader.DeferUnavailableExtensions();
            AssetFileSystem fileSystem = new(
                mounts,
                autoStart: false,
                options.fileWatcherFlushDelayMs,
                options.sourcePolicy,
                requireWritableProject: options.mode == AssetPipelineMode.Authoring,
                identities,
                persistentIdentityResolver: path =>
                    loader.TryGetPersistentId(path, out Guid persistentId)
                        ? persistentId
                        : null,
                activateIdentities: true);
            loader.AssetReloaded += OnAssetReloaded;
            m_loader = loader;
            m_fileSystem = fileSystem;
            m_ownerThreadId = Environment.CurrentManagedThreadId;
            m_revision = 0;
            m_cacheOptions = options.cacheOptions;
            m_options = options with { sourceMounts = mounts };
            m_lastArtifactCollectionTimestamp = 0;
            isInitialized = true;
            try
            {
                loader.Rescan();
                CollectArtifactsIfDue(loader, force: true);
                fileSystem.Refresh();
                if (options.enableFileSystemWatcher && options.mode == AssetPipelineMode.Authoring)
                    fileSystem.Start();
                m_catalogParticipantRegistration = modules.RegisterCatalogParticipant(
                    m_catalogParticipant);
            }
            catch
            {
                ShutdownLocked();
                throw;
            }
        }
    }

    /// <summary>
    /// Waits until queued source watcher changes have been processed.
    /// </summary>
    public void WaitForIdle()
    {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        AssetFileSystem fileSystem = GetFileSystem();
        IReadOnlyList<AssetChangedEvent> changes = fileSystem.WaitForIdle(out bool requiresFullRescan);
        if (changes.Count > 0 || requiresFullRescan)
            ApplySourceChanges(changes, requiresFullRescan);
        GetLoader().WaitForIdle();
        CollectArtifactsIfDue(GetLoader(), force: true);
    }

    private IDisposable AcquireOperation()
    {
        EnsureAccess();
        _ = m_types.current;
        return m_generations.AcquireOperation("operate Asset Pipeline");
    }

    private void EnsureAccess()
    {
        m_generations.EnsureRetirementSafe();
        if (m_generations.state == GenerationState.Faulted)
            m_generations.EnsureReady("access Asset Pipeline");
        if (m_shutdown is not null || m_failedPreparation is not null)
            throw new InvalidOperationException("The Asset Pipeline is retiring and cannot accept operations.");
    }

    private void EnsureOwnerThread()
    {
        EnsureInitializationThread();
        if (m_sampleImport is not null)
            throw new InvalidOperationException("Asset mutations are deferred until the pending sample import finishes.");
    }

    private void EnsureInitializationThread()
    {
        if (!isInitialized)
            throw new InvalidOperationException("AssetPipeline is not initialized.");
        if (Environment.CurrentManagedThreadId != m_ownerThreadId)
        {
            throw new InvalidOperationException(
                "Asset database mutations must run on the thread that initialized AssetPipeline.");
        }
    }
}
