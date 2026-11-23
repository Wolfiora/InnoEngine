using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets;
using Inno.Core.Execution;

namespace Inno.Plugins.Authoring;

/// <summary>
/// Publishes the host-validated active local Plugin snapshot.
/// </summary>
internal sealed class PluginCatalog
{
    private readonly object m_sync = new();
    private Snapshot m_current = Snapshot.empty;
    private PluginScanResult m_discovery = new([], []);
    private long m_revision;

    /// <summary>
    /// Gets dependency-ordered active Plugins.
    /// </summary>
    public IReadOnlyList<PluginCandidate> activePlugins
    {
        get
        {
            lock (m_sync)
            {
                return m_current.ordered;
            }
        }
    }

    /// <summary>
    /// Gets the monotonic identity of the active validated Plugin generation.
    /// </summary>
    public long revision
    {
        get
        {
            lock (m_sync)
            {
                return m_revision;
            }
        }
    }

    /// <summary>
    /// Gets the latest validated installed Plugin discovery and isolated diagnostics.
    /// </summary>
    public PluginScanResult discovery
    {
        get
        {
            lock (m_sync)
            {
                return m_discovery;
            }
        }
    }

    /// <summary>
    /// Occurs after installed discovery or active Plugin state changes.
    /// </summary>
    public event Action? changed;

    /// <summary>
    /// Tries to resolve the active Plugin that owns an asset source.
    /// </summary>
    /// <param name="source">
    /// Asset source identity.
    /// </param>
    /// <param name="plugin">
    /// Receives the active Plugin candidate.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the source belongs to an active Plugin.
    /// </returns>
    public bool TryGet(
        AssetSourceID source,
        out PluginCandidate? plugin
    ) {
        lock (m_sync)
        {
            return m_current.bySource.TryGetValue(source, out plugin);
        }
    }

    /// <summary>
    /// Atomically publishes a fully validated Plugin scan.
    /// </summary>
    /// <param name="result">
    /// Validated Plugin source scan result.
    /// </param>
    public void Activate(PluginScanResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        PluginCandidate[] ordered = PluginSourceService.GetActivatableCandidates(result).ToArray();
        FrozenDictionary<AssetSourceID, PluginCandidate> bySource = ordered.ToFrozenDictionary(
            static candidate => candidate.sourceMount.id,
            static candidate => candidate
        );
        Action? handlers;
        lock (m_sync)
        {
            m_discovery = result;
            m_current = new Snapshot(Array.AsReadOnly(ordered), bySource);
            m_revision++;
            handlers = changed;
        }
        InvokeObservers(handlers);
    }

    /// <summary>
    /// Publishes discovery diagnostics without changing the active mount generation.
    /// </summary>
    /// <param name="result">
    /// Latest validated installed Plugin source scan.
    /// </param>
    public void PublishDiscovery(PluginScanResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        Action? handlers;
        lock (m_sync)
        {
            m_discovery = result;
            handlers = changed;
        }
        InvokeObservers(handlers);
    }

    /// <summary>
    /// Clears the active Plugin snapshot.
    /// </summary>
    public void Shutdown()
    {
        lock (m_sync)
        {
            m_current = Snapshot.empty;
            m_discovery = new PluginScanResult([], []);
            m_revision++;
            changed = null;
        }
    }

    private static void InvokeObservers(Action? handlers)
    {
        if (handlers is null)
        {
            return;
        }
        List<Exception>? failures = null;
        foreach (Delegate handler in handlers.GetInvocationList())
        {
            try
            {
                ((Action)handler)();
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                if (failures is not null)
                {
                    throw new AggregateException(
                        "Plugin catalog observers reported unfinished retirement.",
                        [.. failures, pendingRetirement]
                    );
                }
                throw;
            }
            catch (Exception exception)
            {
                failures ??= [];
                failures.Add(exception);
            }
        }
        if (failures is not null)
        {
            throw new AggregateException("One or more Plugin catalog observers failed after publication.", failures);
        }
    }

    private sealed record Snapshot(
        IReadOnlyList<PluginCandidate> ordered,
        FrozenDictionary<AssetSourceID, PluginCandidate> bySource
    ) {
        internal static Snapshot empty { get; } = new(
            [],
            new Dictionary<AssetSourceID, PluginCandidate>().ToFrozenDictionary()
        );
    }
}
