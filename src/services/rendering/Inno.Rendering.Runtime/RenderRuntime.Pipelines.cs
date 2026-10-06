using Inno.References;
using Inno.Runtime.Contracts;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Input;
using Inno.Core.Mathematics;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Runtime;

sealed partial class RenderRuntime
{
    /// <summary>
    /// Validates and selects a project default while preserving its last-good generation.
    /// </summary>
    /// <param name="pipelineAsset">
    /// Candidate project default pipeline asset.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a usable generation exists for the candidate.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown during an open graphics frame, during retirement, or after generation cleanup faults the owner.
    /// </exception>
    public bool TryActivateDefaultPipeline(RenderPipelineAsset pipelineAsset)
    {
        EnsureActive();
        ArgumentNullException.ThrowIfNull(pipelineAsset);
        if (m_frameOpen)
            throw new InvalidOperationException("A default pipeline can only change at a frame boundary.");
        if (TryGetGeneration(pipelineAsset, out _))
        {
            m_graphicsSettings.defaultPipeline = pipelineAsset;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Begins an isolated rendering-extension reload transaction at a frame boundary.
    /// </summary>
    /// <returns>
    /// A transaction that preserves the active generation until its candidate is explicitly completed.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown during an open frame, during retirement, after a terminal generation failure, or while another
    /// rendering reload transaction is active.
    /// </exception>
    public IRenderRuntimeReloadTransaction BeginExtensionReload()
    {
        EnsureActive();
        if (m_frameOpen)
            throw new InvalidOperationException("Rendering extensions can only reload at a frame boundary.");
        if (m_reloadSession is not null)
            throw new InvalidOperationException("A rendering extension reload is already active.");

        m_reloadSession = RenderRuntimeReloadSession.Create(this);
        return m_reloadSession;
    }

    private bool TryGetGeneration(
        RenderPipelineAsset asset,
        out RenderPipelineGeneration? generation
    ) {
        generation = null;
        string fingerprint = RenderExtensionRegistry.GetConfigurationFingerprint(asset);
        RenderExtensionRegistry.Snapshot snapshot;
        try
        {
            snapshot = m_extensions.extensions;
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch (Exception exception)
        {
            m_extensions.EnsureHealthy();
            m_diagnostics.Publish(new Diagnostic(
                "RENDER_EXTENSION_REGISTRY_FAILED",
                $"Rendering extension discovery kept every last-good generation: {exception.Message}",
                DiagnosticSeverity.Error,
                asset.pipelineTypeId));
            return m_generations.TryGetValue(asset, out GenerationCacheEntry? failedEntry)
                && (generation = failedEntry.lastGood) is not null;
        }
        m_diagnostics.Resolve("RENDER_EXTENSION_REGISTRY_FAILED", asset.pipelineTypeId);

        if (!m_generations.TryGetValue(asset, out GenerationCacheEntry? entry))
        {
            entry = new GenerationCacheEntry(asset);
            m_generations.Add(asset, entry);
        }

        if (entry.lastGood is not null
            && entry.lastGood.typeCacheVersion != snapshot.typeCacheVersion)
        {
            m_extensions.Retire(entry.lastGood);
            entry.lastGood = null;
        }

        if (entry.attemptedTypeCacheVersion == snapshot.typeCacheVersion
            && string.Equals(entry.attemptedFingerprint, fingerprint, StringComparison.Ordinal))
        {
            generation = entry.lastGood;
            return generation is not null;
        }

        entry.attemptedTypeCacheVersion = snapshot.typeCacheVersion;
        entry.attemptedFingerprint = fingerprint;
        RenderPipelineGeneration? previous = entry.lastGood;
        try
        {
            if (!TryCreateGeneration(snapshot, asset, out RenderPipelineGeneration? next))
            {
                return false;
            }
            entry.lastGood = next!;
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch (Exception exception)
        {
            m_extensions.EnsureHealthy();
            m_diagnostics.Publish(new Diagnostic(
                "RENDER_EXTENSION_GENERATION_FAILED",
                $"Pipeline '{asset.pipelineTypeId}' kept its last-good generation: {exception.Message}",
                DiagnosticSeverity.Error,
                asset.pipelineTypeId));
            generation = entry.lastGood;
            return generation is not null;
        }

        m_diagnostics.Resolve("RENDER_EXTENSION_GENERATION_FAILED", asset.pipelineTypeId);
        m_extensions.Retire(previous);

        generation = entry.lastGood;
        return generation is not null;
    }

    private static bool TryCreateGeneration(
        RenderExtensionRegistry.Snapshot snapshot,
        RenderPipelineAsset asset,
        out RenderPipelineGeneration? generation
    )
        => snapshot.TryCreateGeneration(asset, out generation);

    private void EndReloadSession(RenderRuntimeReloadSession session)
    {
        if (ReferenceEquals(m_reloadSession, session))
            m_reloadSession = null;
    }

    private void PruneRetiredGenerations()
    {
        PruneRetiredAssets();
        RenderExtensionRegistry.Snapshot snapshot;
        try
        {
            snapshot = m_extensions.extensions;
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch (Exception exception)
        {
            m_extensions.EnsureHealthy();
            m_diagnostics.Publish(new Diagnostic(
                "RENDER_EXTENSION_REGISTRY_FAILED",
                $"Rendering extension discovery kept the current frame available: {exception.Message}",
                DiagnosticSeverity.Error));
            return;
        }
        m_diagnostics.Resolve("RENDER_EXTENSION_REGISTRY_FAILED");

        if (m_prunedTypeCacheVersion == snapshot.typeCacheVersion)
            return;
        m_scratch.generations.AddRange(m_generations);
        try
        {
            foreach ((RenderPipelineAsset asset, GenerationCacheEntry entry) in m_scratch.generations)
            {
                if (entry.lastGood is not null
                    && entry.lastGood.typeCacheVersion != snapshot.typeCacheVersion)
                {
                    m_extensions.Retire(entry.lastGood);
                    entry.lastGood = null;
                }

                if (entry.attemptedTypeCacheVersion != snapshot.typeCacheVersion)
                    m_generations.Remove(asset);
            }

        }
        finally
        {
            m_scratch.generations.Clear();
        }

        if (m_requestProviders is not null
            && m_requestProviders.typeCacheVersion != snapshot.typeCacheVersion)
        {
            m_requestProviders = null;
        }
        m_prunedTypeCacheVersion = snapshot.typeCacheVersion;
    }

    private void AddFeatures(
        RenderPipelineAsset asset,
        IReadOnlyDictionary<string, RenderPipelineFeature> features,
        RenderPipelineContext context
    ) {
        foreach (RenderFeatureConfiguration configuration in asset.features)
        {
            if (!configuration.enabled
                || !features.TryGetValue(configuration.featureTypeId, out RenderPipelineFeature? feature))
            {
                continue;
            }

            using RenderGraphMutationScope mutation = context.graph.BeginMutationScope();
            try
            {
                feature.AddRenderPasses(new RenderFeatureContext(context, configuration));
                mutation.Commit();
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception exception)
            {
                PublishFrameIssue(new Diagnostic(
                    "RENDER_FEATURE_FAILED",
                    $"Feature '{configuration.featureTypeId}' was rolled back: {exception.Message}",
                    DiagnosticSeverity.Error,
                    configuration.featureTypeId));
            }
        }
    }

    private sealed class GenerationCacheEntry(RenderPipelineAsset asset)
    {
        internal readonly Identity assetIdentity = asset.identity;
        internal readonly bool hasAssetOwner = asset.identity.runtimeIdentity.HasValue;
        internal long attemptedTypeCacheVersion { get; set; } = -1;
        internal string? attemptedFingerprint { get; set; }
        internal RenderPipelineGeneration? lastGood { get; set; }
    }

    private readonly record struct GenerationState(
        long attemptedTypeCacheVersion,
        string? attemptedFingerprint,
        RenderPipelineGeneration? lastGood
    );

    internal sealed class RenderRuntimeReloadSession : IRenderRuntimeReloadTransaction
    {
        private readonly RenderRuntime m_owner;
        private IReadOnlyDictionary<RenderPipelineAsset, GenerationState>? m_previous;
        private RenderExtensionRegistry.RequestProviderGeneration? m_previousRequestProviders;
        private RenderRequest[]? m_previousPendingRequests;
        private RenderRequest[]? m_previousCurrentRequests;
        private CompositionRequest[]? m_previousPendingCompositions;
        private CompositionRequest[]? m_previousCurrentCompositions;
        private readonly Dictionary<RenderPipelineAsset, GenerationState> m_candidates = [];
        private RenderExtensionRegistry.RequestProviderGeneration? m_candidateRequestProviders;
        private bool m_prepared;
        private bool m_activated;
        private bool m_finished;

        private RenderRuntimeReloadSession(
            RenderRuntime owner,
            IReadOnlyDictionary<RenderPipelineAsset, GenerationState> previous,
            RenderExtensionRegistry.RequestProviderGeneration? previousRequestProviders,
            RenderRequest[] previousPendingRequests,
            RenderRequest[] previousCurrentRequests,
            CompositionRequest[] previousPendingCompositions,
            CompositionRequest[] previousCurrentCompositions
        ) {
            m_owner = owner;
            m_previous = previous;
            m_previousRequestProviders = previousRequestProviders;
            m_previousPendingRequests = previousPendingRequests;
            m_previousCurrentRequests = previousCurrentRequests;
            m_previousPendingCompositions = previousPendingCompositions;
            m_previousCurrentCompositions = previousCurrentCompositions;
        }

        internal static RenderRuntimeReloadSession Create(RenderRuntime owner)
        {
            // Disposed sessions must not contribute obsolete assets to the next extension generation.
            owner.PruneRetiredAssets();
            var previous = new Dictionary<RenderPipelineAsset, GenerationState>();
            foreach ((RenderPipelineAsset asset, GenerationCacheEntry entry) in owner.m_generations)
            {
                previous.Add(asset, new GenerationState(
                    entry.attemptedTypeCacheVersion,
                    entry.attemptedFingerprint,
                    entry.lastGood));
            }
            lock (owner.m_requestLock)
            {
                return new RenderRuntimeReloadSession(
                    owner,
                    previous,
                    owner.m_requestProviders,
                    [.. owner.m_pendingRequests],
                    [.. owner.m_currentRequests],
                    [.. owner.m_pendingCompositions],
                    [.. owner.m_currentCompositions]);
            }
        }

        internal void PrepareCandidate()
        {
            EnsureNotFinished();
            if (m_prepared)
                return;

            RenderExtensionRegistry.Snapshot snapshot = m_owner.m_extensions.extensions;
            try
            {
                m_candidateRequestProviders = snapshot.providers;
                foreach (RenderPipelineAsset asset in m_previous!.Keys)
                {
                    string fingerprint = RenderExtensionRegistry.GetConfigurationFingerprint(asset);
                    _ = TryCreateGeneration(snapshot, asset, out RenderPipelineGeneration? candidate);
                    m_candidates.Add(asset, new GenerationState(
                        snapshot.typeCacheVersion,
                        fingerprint,
                        candidate));
                }
                m_prepared = true;
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception exception)
            {
                DisposeCandidates();
                m_owner.m_diagnostics.Publish(new Diagnostic(
                    "RENDER_EXTENSION_RELOAD_REJECTED",
                    "Rendering extension candidate was rejected and every last-good generation was retained: " +
                    exception.Message,
                    DiagnosticSeverity.Error));
                throw new InvalidOperationException(
                    "Rendering extension candidate activation failed.",
                    exception);
            }
        }

        internal void Activate()
        {
            EnsureNotFinished();
            if (!m_prepared)
                throw new InvalidOperationException("Rendering extension candidates have not been prepared.");
            if (m_activated)
                return;

            foreach (RenderPipelineAsset asset in m_candidates.Keys)
            {
                if (!m_owner.m_generations.ContainsKey(asset))
                    throw new InvalidOperationException("A tracked rendering generation changed during reload.");
            }
            foreach ((RenderPipelineAsset asset, GenerationState candidate) in m_candidates)
            {
                GenerationCacheEntry entry = m_owner.m_generations[asset];
                entry.attemptedTypeCacheVersion = candidate.attemptedTypeCacheVersion;
                entry.attemptedFingerprint = candidate.attemptedFingerprint;
                entry.lastGood = candidate.lastGood;
            }
            m_owner.m_requestProviders = m_candidateRequestProviders;
            lock (m_owner.m_requestLock)
            {
                m_owner.m_acceptingCurrentFrame = false;
                m_owner.m_pendingRequests.Clear();
                m_owner.m_currentRequests.Clear();
                m_owner.m_pendingCompositions.Clear();
                m_owner.m_currentCompositions.Clear();
            }
            m_activated = true;
        }

        internal void Complete()
        {
            EnsureNotFinished();
            if (!m_activated)
                throw new InvalidOperationException("Rendering extension candidates have not been activated.");
            try
            {
                var resources = new LifetimeScope();
                foreach (GenerationState previous in m_previous!.Values)
                {
                    if (previous.lastGood is not null)
                        resources.Own(previous.lastGood);
                }
                m_owner.m_extensions.Retire(resources);
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch
            {
                Finish();
                throw;
            }
            m_owner.m_diagnostics.Resolve("RENDER_EXTENSION_RELOAD_REJECTED");
            Finish();
        }

        internal void Rollback()
        {
            if (m_finished)
                return;

            if (m_activated)
            {
                foreach ((RenderPipelineAsset asset, GenerationState previous) in m_previous!)
                {
                    if (!m_owner.m_generations.TryGetValue(asset, out GenerationCacheEntry? entry))
                        continue;
                    entry.attemptedTypeCacheVersion = previous.attemptedTypeCacheVersion;
                    entry.attemptedFingerprint = previous.attemptedFingerprint;
                    entry.lastGood = previous.lastGood;
                }
                m_owner.m_requestProviders = m_previousRequestProviders;
                lock (m_owner.m_requestLock)
                {
                    m_owner.m_pendingRequests.Clear();
                    m_owner.m_pendingRequests.AddRange(m_previousPendingRequests!);
                    m_owner.m_currentRequests.Clear();
                    m_owner.m_currentRequests.AddRange(m_previousCurrentRequests!);
                    m_owner.m_pendingCompositions.Clear();
                    m_owner.m_pendingCompositions.AddRange(m_previousPendingCompositions!);
                    m_owner.m_currentCompositions.Clear();
                    m_owner.m_currentCompositions.AddRange(m_previousCurrentCompositions!);
                }
            }
            try
            {
                DisposeCandidates();
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch
            {
                Finish();
                throw;
            }
            Finish();
        }

        void IRenderRuntimeReloadTransaction.Prepare() => PrepareCandidate();

        void IRenderRuntimeReloadTransaction.Activate() => Activate();

        void IRenderRuntimeReloadTransaction.Complete() => Complete();

        void IRenderRuntimeReloadTransaction.Rollback() => Rollback();

        private void DisposeCandidates()
        {
            var resources = new LifetimeScope();
            foreach (GenerationState candidate in m_candidates.Values)
            {
                if (candidate.lastGood is not null)
                    resources.Own(candidate.lastGood);
            }
            m_owner.m_extensions.Retire(resources);
            m_candidates.Clear();
            m_candidateRequestProviders = null;
        }

        private void Finish()
        {
            m_candidates.Clear();
            m_candidateRequestProviders = null;
            m_previous = null;
            m_previousRequestProviders = null;
            m_previousPendingRequests = null;
            m_previousCurrentRequests = null;
            m_previousPendingCompositions = null;
            m_previousCurrentCompositions = null;
            m_finished = true;
            m_owner.EndReloadSession(this);
        }

        private void EnsureNotFinished()
        {
            if (m_finished)
                throw new InvalidOperationException("Rendering extension reload session is already finished.");
        }
    }

}
