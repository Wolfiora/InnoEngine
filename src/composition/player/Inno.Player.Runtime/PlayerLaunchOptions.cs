using System;
using Inno.Adapter;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Runtime;
using Inno.Shell;
using Inno.Storage;

namespace Inno.Player.Runtime;

/// <summary>
/// Supplies resolved host services and content to the common Player lifecycle.
/// </summary>
public sealed class PlayerLaunchOptions
{
    /// <summary>
    /// Gets the code catalog selected by the platform composition; ownership transfers to the application.
    /// </summary>
    public required IAssemblyCatalogSource modules { get; init; }

    /// <summary>
    /// Gets the metadata implementation corresponding to the selected code deployment.
    /// </summary>
    public required ITypeCatalogSource types { get; init; }

    /// <summary>
    /// Gets generated declaration access and collection construction for the linked code closure.
    /// </summary>
    public required ISerializationMetadataSource serializationMetadata { get; init; }

    /// <summary>
    /// Gets the host-owned factories used to create isolated runtime adapters.
    /// </summary>
    public required IAdapterCatalog adapters { get; init; }

    /// <summary>
    /// Gets the borrowed source of deployment metadata and verified immutable content.
    /// </summary>
    public required IPlayerContentSource contentSource { get; init; }

    /// <summary>
    /// Gets the host factory transferring application storage ownership to the game session.
    /// </summary>
    public required Func<GameRuntimeManifest, IApplicationStorage> createStorage { get; init; }

    /// <summary>
    /// Gets the optional host factory transferring a session log sink to the game session.
    /// </summary>
    public Func<GameRuntimeManifest, LogSessionId, ILogSink>? createLogSink { get; init; }

    /// <summary>
    /// Gets the strategy that activates the frozen deployment's managed modules.
    /// </summary>
    public required IPlayerModuleActivator moduleActivator { get; init; }

    /// <summary>
    /// Gets the owner-thread driver that schedules the common shell frames.
    /// </summary>
    public required IShellFrameDriver frameDriver { get; init; }

    /// <summary>
    /// Gets the coherent runtime backend selection.
    /// </summary>
    public required AdapterSelection adapterSelection { get; init; }

    /// <summary>
    /// Gets the execution policy available to the game's job scheduler.
    /// </summary>
    public RuntimeJobExecutionMode jobExecutionMode { get; init; } = RuntimeJobExecutionMode.WorkerPool;

    /// <summary>
    /// Gets whether graphics commands must execute on the frame owner's thread.
    /// </summary>
    public bool renderOnCallingThread { get; init; }

    /// <summary>
    /// Gets the host's logging delivery policy.
    /// </summary>
    public LogDeliveryMode logDeliveryMode { get; init; } = LogDeliveryMode.Background;

    /// <summary>
    /// Gets whether the host console supports changing terminal colors.
    /// </summary>
    public bool consoleColors { get; init; }

    /// <summary>
    /// Gets an optional rendering API preference; null selects the adapter default.
    /// </summary>
    public GraphicsApi? graphicsApi { get; init; }

    /// <summary>
    /// Gets whether the primary window is initially shown; false preserves rendering without taking focus.
    /// </summary>
    public bool windowVisible { get; init; } = true;

    /// <summary>
    /// Gets an optional positive frame count for a bounded verification run.
    /// </summary>
    public int? smokeFrameLimit { get; init; }
}
