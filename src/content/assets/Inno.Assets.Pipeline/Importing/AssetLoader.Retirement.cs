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
    /// Stops new loads and releases canonical assets before their registries and diagnostics.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// A load, serialized operation or unload hook is still active. Retain the loader and retry after quiescence.
    /// </exception>
    /// <exception cref="AggregateException">
    /// Completed releases failed; all quiescent resources have still been attempted.
    /// </exception>
    public void Dispose()
    {
        lock (m_asyncSync)
        {
            if (m_disposed)
                return;
            m_disposeRequested = true;
            bool pending = m_inFlightPathLoads.Values
                .Concat(m_inFlightIdLoads.Values)
                .Any(static task => !task.IsCompleted);
            if (pending || m_admittedOperations > 0 || m_retirementActive)
                throw new RetirementPendingException("Asset operations must complete before their loader retires.");
            m_retirementActive = true;
        }
        AssetLoader? previous = t_activeLoader;
        t_activeLoader = this;
        try
        {
            DisposeLocked();
        }
        finally
        {
            t_activeLoader = previous;
            if (m_disposed)
                m_operationGate.Dispose();
            lock (m_asyncSync)
                m_retirementActive = false;
        }
    }

    private void RetireUnmountedRecordLocked(AssetRecord record)
        => RetireRecordLocked(
            record,
            $"Asset source mount for '{record.relativePath}' is not active.");

    private void RetireRecordLocked(
        AssetRecord record,
        string diagnostic
    ) {
        string recordPath = record.relativePath;
        m_recordsByPath.Remove(recordPath);
        m_importGraph.RemoveNode(recordPath);
        if (record.persistentId == Guid.Empty)
        {
            return;
        }

        record.meta.isTombstone = true;
        record.meta.importStatus = (int)AssetImportStatus.Missing;
        record.meta.diagnostics = [diagnostic];
        record.meta.artifactKey = string.Empty;
        // Tombstones retain neutral intent; only live payload and object retention retire.
        record.payload = [];
        m_runtimeGraph.ReplaceDependencies(record.persistentId, []);
        if (record.asset is null)
        {
            return;
        }

        m_dependencyRetention.Remove(record.asset);
        m_runtimeOwner.Initialize(
            record.asset,
            AssetPath.Parse(recordPath),
            record.meta.sourceHash,
            ReadOnlyMemory<byte>.Empty,
            true,
            record.asset.contentVersion + 1);
        PublishReloaded(record.asset);
    }

    private bool ReleaseRetiredCanonicalAssetsLocked()
    {
        bool releasedAny = false;
        foreach (AssetRecord record in m_recordsByPath.Values.ToArray())
        {
            AssetObject? asset = record.asset;
            if (asset is null)
                continue;
            bool isCurrent = m_types.TryGetTypeRef(asset.GetType(), out _);
            TypeRef persistedType = new(record.stableTypeId);
            if (isCurrent &&
                (record.stableTypeId == Guid.Empty ||
                 !m_types.TryResolve(persistedType, out Type? resolvedType) ||
                 resolvedType == asset.GetType()))
            {
                continue;
            }

            m_dependencyRetention.Remove(asset);
            m_runtimeOwner.Release(asset);
            releasedAny = true;
            try
            {
                _ = m_identities.Unregister(asset);
            }
            catch (Exception exception)
            {
                m_log.Write(
                    LogLevel.Error,
                    "Retired asset '{0}' was released, but an identity observer failed: {1}",
                    [record.relativePath, exception]);
            }
            finally
            {
                record.asset = null;
                PublishReloaded(asset);
            }
        }
        return releasedAny;
    }

    private void DisposeLocked()
    {
        if (m_disposed)
            return;
        if (m_retirement is null)
        {
            m_retirement = new LifetimeScope();
            m_retirement.Own(m_buildProcessors);
            m_retirement.Own(m_importers);
            m_retirement.Own(m_diagnostics);
            foreach (AssetRecord record in m_recordsById.Values)
            {
                if (record.asset is not null)
                    m_retirement.Own(new AssetRecordRetirement(this, record));
            }
        }
        try
        {
            m_retirement.Dispose();
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch
        {
            CompleteRetirement();
            throw;
        }
        CompleteRetirement();
    }

    private void CompleteRetirement()
    {
        m_disposed = true;
        m_sourceMetadataStage = null;
        m_recordsByPath.Clear();
        m_recordsById.Clear();
        m_runtimeGraph.Clear();
        m_importGraph.Clear();
        m_missingAssets.Clear();
        m_preservedMissingStates.Clear();
        lock (m_asyncSync)
        {
            m_inFlightPathLoads.Clear();
            m_inFlightIdLoads.Clear();
        }
    }

    private sealed class AssetRecordRetirement(
        AssetLoader owner,
        AssetRecord record
    ) : IDisposable
    {
        private bool m_released;
        private readonly List<Exception> m_failures = [];

        /// <summary>
        /// Keeps the canonical registration until its unload hook has actually quiesced.
        /// </summary>
        public void Dispose()
        {
            if (record.asset is null)
                return;
            if (!m_released)
            {
                try
                {
                    owner.m_runtimeOwner.Release(record.asset);
                }
                catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
                {
                    throw;
                }
                catch (Exception failure)
                {
                    m_failures.Add(failure);
                }
                m_released = true;
            }
            try
            {
                owner.m_identities.Unregister(record.asset);
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception failure)
            {
                m_failures.Add(failure);
            }
            record.asset = null;
            if (m_failures.Count > 0)
                throw new AggregateException("Canonical Asset retirement failed after releasing its registration.", m_failures);
        }
    }

}
