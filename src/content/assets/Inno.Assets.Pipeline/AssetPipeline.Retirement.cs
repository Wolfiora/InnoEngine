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

sealed partial class AssetPipeline
{
    /// <summary>
    /// Releases watchers, catalog participants, canonical objects, and rebuildable staging state.
    /// </summary>
    public void Dispose()
    {
        lock (m_lifecycleLock)
            ShutdownLocked();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Collects assets that have no external managed references.
    /// </summary>
    /// <returns>
    /// The number of released canonical assets.
    /// </returns>
    public int UnloadUnusedAssets() => GetLoader().UnloadUnusedAssets();

    private void PruneRetiredObservers()
    {
        RemoveRetiredObservers(Changed, observer => Changed -= observer);
        RemoveRetiredObservers(AssetReloaded, observer => AssetReloaded -= observer);
    }

    private void RemoveRetiredObservers<T>(
        Action<T>? handlers,
        Action<Action<T>> remove
    ) {
        if (handlers is null)
            return;
        foreach (Delegate observer in handlers.GetInvocationList())
        {
            if (!IsRetiredCollectibleObserver(observer))
                continue;
            remove((Action<T>)observer);
        }
    }

    private bool IsRetiredCollectibleObserver(Delegate observer)
    {
        Type? declaringType = observer.Method.DeclaringType;
        Type? targetType = observer.Target?.GetType();
        return IsRetiredCollectibleType(declaringType) ||
               IsRetiredCollectibleType(targetType);
    }

    private bool IsRetiredCollectibleType(Type? type)
    {
        if (type is null ||
            !type.Assembly.IsCollectible)
        {
            return false;
        }
        return !m_types.TryGetTypeRef(type, out _);
    }

    private void ShutdownLocked()
    {
        m_generations.EnsureRetirementSafe();
        m_sampleImport?.Rollback();
        m_sourceMountCandidate?.Rollback();
        if (m_shutdown is null)
        {
            m_shutdownBarrier = new RetirementBarrier("Asset Pipeline shutdown");
            m_shutdown = new LifetimeScope();
            if (m_loader is not null)
            {
                m_loader.AssetReloaded -= OnAssetReloaded;
                m_shutdown.Own(m_loader);
            }
            if (m_fileSystem is not null)
                m_shutdown.Own(m_fileSystem);
            if (m_catalogParticipantRegistration is not null)
                m_shutdown.Own(m_catalogParticipantRegistration);
            if (m_failedPreparation is not null)
                m_shutdown.Own(m_failedPreparation);
        }
        try
        {
            Retire(m_shutdown, m_shutdownBarrier!);
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch
        {
            FinishShutdown();
            throw;
        }
        FinishShutdown();
    }

    private void FinishShutdown()
    {
        m_catalogParticipantRegistration = null;
        m_failedPreparation = null;
        m_diagnostics.ResolveSourceDatabase();
        m_fileSystem = null;
        m_loader = null;
        assetRoot = string.Empty;
        libraryRoot = string.Empty;
        artifactRoot = string.Empty;
        sourceMounts = [];
        m_ownerThreadId = 0;
        m_revision = 0;
        m_reconcileSampleImport = false;
        m_cacheOptions = default;
        m_options = default;
        m_lastArtifactCollectionTimestamp = 0;
        isInitialized = false;
        Changed = null;
        AssetReloaded = null;
        SourceMountsChanged = null;
    }

}
