using System;
using Inno.Adapter;
using Inno.Core.Logging;
using Inno.Rendering;
using Inno.Runtime;
using Inno.Shell;

namespace Inno.Player.Runtime;

/// <summary>
/// Supplies resolved host services and deployment locations to the common Player lifecycle.
/// </summary>
public sealed class PlayerLaunchOptions
{
    /// <summary>
    /// Gets the host-owned factories used to create isolated runtime adapters.
    /// </summary>
    public required IAdapterCatalog adapters { get; init; }

    /// <summary>
    /// Gets the directory containing the prepared runtime manifest and content pack.
    /// </summary>
    public required string contentDirectory { get; init; }

    /// <summary>
    /// Gets the parent directory under which the manifest's application data path is resolved.
    /// </summary>
    public required string persistentDataRoot { get; init; }

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
    public AdapterSelection adapterSelection { get; init; } = AdapterSelection.defaultValue;

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
    /// Gets an optional positive frame count for a bounded verification run.
    /// </summary>
    public int? smokeFrameLimit { get; init; }
}
