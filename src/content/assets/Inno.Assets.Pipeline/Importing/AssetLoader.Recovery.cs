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
    private bool IsSourceBackedTypeUnavailableLocked(Guid persistentId)
    {
        if (!m_recordsById.TryGetValue(persistentId, out AssetRecord? record) ||
            record.meta.isTombstone ||
            record.persistentId == Guid.Empty ||
            string.IsNullOrWhiteSpace(record.meta.importerId) ||
            !IsMounted(record.relativePath) ||
            !IOFile.Exists(GetSourcePath(record.relativePath)))
        {
            return false;
        }
        return ResolveRecordType(record) is null || m_unavailableImports.ContainsKey(record.relativePath);
    }

    private void RefreshLoadedAssetReferencesLocked()
    {
        AssetRecord[] loaded = m_recordsByPath.Values
            .Where(static record => record.asset is not null)
            .ToArray();
        for (int i = 0; i < loaded.Length; i++)
        {
            AssetRecord record = loaded[i];
            AssetObject asset = record.asset!;
            if (record.meta.assetStateBytes.Length == 0)
                continue;
            byte[] rollback = CaptureAssetState(asset);
            try
            {
                RestoreAssetState(asset, record.meta.assetStateBytes);
            }
            catch
            {
                RestoreAssetState(asset, rollback);
                throw;
            }
        }
        for (int i = 0; i < loaded.Length; i++)
            AttachDependenciesLocked(loaded[i]);
    }

    internal IReadOnlyList<SerializedMissingState> CaptureRecoveryStates()
        => Execute(() =>
        {
            var states = new Dictionary<Guid, SerializedMissingState>();
            foreach (AssetRecord record in m_recordsById.Values)
            {
                // Catalog tombstones preserve source identity, but they are not live reference
                // roots. Only a materialized canonical asset belongs to the active generation;
                // retained missing placeholders are captured separately below.
                if (record.asset is not null)
                    Capture(record.persistentId, record.relativePath, record.stableTypeId, record.meta.assetStateBytes);
            }
            foreach ((Guid id, SerializedMissingState state) in m_preservedMissingStates)
                states.TryAdd(id, state);
            foreach ((Guid id, WeakReference<AssetObject> reference) in m_missingAssets)
            {
                if (!states.ContainsKey(id) && reference.TryGetTarget(out AssetObject? missing))
                {
                    m_recordsById.TryGetValue(id, out AssetRecord? record);
                    Capture(id, missing.assetPath.ToString(), record?.stableTypeId ?? Guid.Empty, record?.meta.assetStateBytes ?? []);
                }
            }
            return (IReadOnlyList<SerializedMissingState>)Array.AsReadOnly(states.Values.ToArray());

            void Capture(
                Guid id,
                string path,
                Guid stableTypeId,
                byte[] payload
            ) {
                states.Add(id, new SerializedMissingState(new ReferenceKey(id, "$asset"),
                    new ReferenceDescriptor(AssetReferenceProtocol.id, id, stableTypeId,
                        lastKnownPath: path), payload));
            }
        });

    internal void PreserveMissingRecoveryStates(IReadOnlyList<ReferenceRecoveryChange> changes)
        => Execute(() =>
        {
            m_preservedMissingStates.Clear();
            foreach (ReferenceRecoveryChange change in changes)
            {
                if (change.resolution.state != ReferenceResolutionState.Missing)
                    continue;
                SerializedMissingState state = change.missingState;
                m_preservedMissingStates.Add(state.descriptor.targetPersistentId, state);
            }
        });

    internal bool IsSourceBackedTypeUnavailable(Guid persistentId) => Execute(() => IsSourceBackedTypeUnavailableLocked(persistentId));

    internal void SetIdentitiesActive(bool active)
        => Execute(() =>
        {
            if (m_identitiesActive == active)
                return;
            var attempted = new List<AssetObject>();
            try
            {
                foreach (AssetRecord record in m_recordsById.Values)
                {
                    if (record.asset is null)
                        continue;
                    attempted.Add(record.asset);
                    if (active)
                        m_identities.Register(record.asset, record.persistentId);
                    else
                        m_identities.Unregister(record.asset);
                }
                m_identitiesActive = active;
                m_diagnostics.SetActive(active);
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception failure)
            {
                List<Exception> failures = [failure];
                foreach (AssetObject asset in attempted.AsEnumerable().Reverse())
                {
                    try
                    {
                        if (active)
                            m_identities.Unregister(asset);
                        else
                            m_identities.Register(asset, asset.identity.persistentId);
                    }
                    catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
                    {
                        throw;
                    }
                    catch (Exception rollback)
                    {
                        failures.Add(rollback);
                    }
                }
                if (failures.Count > 1)
                    throw new AggregateException("Asset identity publication and compensation failed.", failures);
                throw;
            }
        });

    private sealed class MissingAsset : AssetObject;

}
