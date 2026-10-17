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
    /// Ends initial authoring extension discovery and strictly retries dependent imports.
    /// </summary>
    /// <remarks>
    /// Call on the initialization thread after successful extension activation. Failed compilation must not call this method.
    /// </remarks>
    [ScriptingApiIgnore]
    public void CompleteExtensionDiscovery()
    {
        EnsureOwnerThread();
        using IDisposable operationScope = AcquireOperation();
        GetLoader().CompleteExtensionDiscovery();
        GetFileSystem().Refresh();
    }

    /// <summary>
    /// Tries to resolve an asset type without loading the asset.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="assetType">
    /// The resolved concrete asset type.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the type can be resolved.
    /// </returns>
    public bool TryGetAssetType(
        AssetPath path,
        out Type? assetType
    ) => GetLoader().TryGetAssetType(path, out assetType);

    /// <summary>
    /// Tries to resolve a persistent identity without loading the asset.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="persistentId">
    /// The resolved persistent identity.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when catalog metadata exists.
    /// </returns>
    public bool TryGetPersistentId(
        AssetPath path,
        out Guid persistentId
    ) => GetLoader().TryGetPersistentId(path, out persistentId);

    /// <summary>
    /// Tries to get a catalog snapshot by source-relative path.
    /// </summary>
    /// <param name="path">
    /// The isolated source path.
    /// </param>
    /// <param name="info">
    /// The catalog snapshot when available.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the path is cataloged.
    /// </returns>
    public bool TryGetInfo(
        AssetPath path,
        out AssetInfo? info
    ) => GetLoader().TryGetInfo(path, out info);

    /// <summary>
    /// Tries to get a catalog snapshot by persistent identity.
    /// </summary>
    /// <param name="persistentId">
    /// The persistent asset identity.
    /// </param>
    /// <param name="info">
    /// The catalog snapshot when available.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the identity is cataloged.
    /// </returns>
    public bool TryGetInfo(
        Guid persistentId,
        out AssetInfo? info
    ) => GetLoader().TryGetInfo(persistentId, out info);

    private sealed class AssetCatalogParticipant(AssetPipeline owner) : IAssemblyCatalogParticipant
    {
        /// <summary>
        /// Prepares an isolated source generation or joins the source candidate already owned by Plugin publication.
        /// </summary>
        /// <param name="catalog">
        /// The assembly generation whose types and serializers will activate before this participant.
        /// </param>
        /// <returns>
        /// A transaction that never rescans or compensates by mutating the previous live loader.
        /// </returns>
        public IAssemblyCatalogTransaction Prepare(AssemblyCatalogSnapshot catalog)
        {
            ArgumentNullException.ThrowIfNull(catalog);
            if (!owner.isInitialized || owner.m_catalogParticipantRegistration is null)
                return new AssetCatalogTransaction(null, false, AssetImportHealthSnapshot.empty);
            owner.EnsureOwnerThread();
            AssetImportHealthSnapshot existingFailures = owner.m_loader!.CaptureWritableImportHealth();
            bool ownsCandidate = owner.m_sourceMountCandidate is null;
            AssetSourceMountTransaction candidate = owner.m_sourceMountCandidate
                ?? owner.PrepareSourceMounts(owner.sourceMounts);
            return new AssetCatalogTransaction(candidate, ownsCandidate, existingFailures);
        }
    }

    private sealed class AssetCatalogTransaction(
        AssetSourceMountTransaction? candidate,
        bool ownsCandidate,
        AssetImportHealthSnapshot existingFailures
    ) : IAssemblyCatalogTransaction
    {
        private bool m_activated;
        private bool m_finished;

        /// <summary>
        /// Gets no additional context because reference recovery is owned by the shared source transaction.
        /// </summary>
        public object? context => null;

        /// <summary>
        /// Validates candidate import results before publishing an independently owned source generation.
        /// </summary>
        public void Activate()
        {
            EnsureNotFinished();
            if (candidate is not null)
            {
                candidate.candidateLoader.ActivateExtensionDiscovery();
                candidate.candidateLoader.RefreshRegistries();
                IReadOnlyList<AssetImportFailure> failures =
                    candidate.candidateLoader.FindIntroducedImportFailures(existingFailures);
                if (failures.Count != 0)
                {
                    string details = string.Join("; ", failures.Select(static failure =>
                        $"{failure.assetPath}: {failure.diagnostics}"));
                    throw new InvalidDataException(
                        "The candidate assembly catalog introduced or changed writable Asset import failures: " + details);
                }
                if (ownsCandidate)
                    candidate.Activate();
            }
            m_activated = true;
        }

        /// <summary>
        /// Commits only a source generation owned by this transaction; joined Plugin candidates keep their original owner.
        /// </summary>
        public void Complete()
        {
            EnsureNotFinished();
            if (!m_activated)
                throw new InvalidOperationException("Asset catalog transaction has not been activated.");
            if (ownsCandidate)
                candidate!.Complete();
            m_finished = true;
        }

        /// <summary>
        /// Discards the isolated candidate and restores its previous identities without rescanning previous assets.
        /// </summary>
        public void Rollback()
        {
            if (m_finished)
                return;
            if (ownsCandidate)
                candidate!.Rollback();
            m_finished = true;
        }

        private void EnsureNotFinished()
        {
            if (m_finished)
                throw new InvalidOperationException("Asset catalog transaction is already finished.");
        }
    }

}
