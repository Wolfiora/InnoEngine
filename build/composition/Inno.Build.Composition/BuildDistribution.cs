using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets.Pipeline;
using Inno.Build.Managed;
using Inno.Build.SupportPacks;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Build.Composition;

/// <summary>
/// Freezes platform factories, managed publishers, and matching Support Pack sources as one distribution.
/// </summary>
public sealed class BuildDistribution
{
    private readonly BuildTargetFactory[] m_targetFactories;
    private readonly HashSet<BuildTargetId> m_targetIds;

    /// <summary>
    /// Captures a complete distribution without creating authoring services or starting any tool.
    /// </summary>
    /// <param name="targetFactories">
    /// One factory per platform source; identities are verified when authoring targets are composed.
    /// </param>
    /// <param name="managedCompilers">
    /// The deployment implementations owned by this composition.
    /// </param>
    /// <param name="supportPackSources">
    /// The complete platform input providers owned by this composition.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// A required collection is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Registrations are empty, null, duplicated, unassigned, or have inconsistent counts.
    /// </exception>
    public BuildDistribution(
        IReadOnlyList<BuildTargetFactory> targetFactories,
        IReadOnlyList<IManagedDeploymentCompiler> managedCompilers,
        IReadOnlyList<IPlayerSupportPackSource> supportPackSources
    ) {
        ArgumentNullException.ThrowIfNull(targetFactories);
        ArgumentNullException.ThrowIfNull(managedCompilers);
        ArgumentNullException.ThrowIfNull(supportPackSources);
        if (targetFactories.Count == 0 || targetFactories.Count != supportPackSources.Count
            || targetFactories.Any(static factory => factory is null)
            || supportPackSources.Any(static source => source is null || string.IsNullOrWhiteSpace(source.target.value)))
            throw new ArgumentException("A distribution requires one non-null factory and source per platform.");
        m_targetFactories = targetFactories.ToArray();
        m_targetIds = supportPackSources.Select(static source => source.target).ToHashSet();
        if (m_targetIds.Count != supportPackSources.Count)
            throw new ArgumentException("Support Pack target identities must be unique.", nameof(supportPackSources));
        availableTargets = Array.AsReadOnly(m_targetIds.OrderBy(static id => id.value, StringComparer.Ordinal).ToArray());
        managedDeployments = new ManagedDeploymentCatalog(managedCompilers);
        supportPacks = new PlayerSupportPackPublisher(supportPackSources.ToArray());
    }

    /// <summary>
    /// Gets the immutable platform identities shared by all hosts.
    /// </summary>
    public IReadOnlyList<BuildTargetId> availableTargets { get; }

    /// <summary>
    /// Gets the immutable deployment implementation catalog.
    /// </summary>
    public ManagedDeploymentCatalog managedDeployments { get; }

    /// <summary>
    /// Gets the publisher containing the same platform input providers.
    /// </summary>
    public PlayerSupportPackPublisher supportPacks { get; }

    /// <summary>
    /// Creates platform targets and validates their identities and default deployment capabilities.
    /// </summary>
    /// <param name="assets">
    /// The active borrowed authoring asset owner.
    /// </param>
    /// <param name="serialization">
    /// The matching borrowed serialization registry.
    /// </param>
    /// <param name="types">
    /// The matching borrowed extension catalog.
    /// </param>
    /// <returns>
    /// An immutable target collection ready for pipeline construction.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// A factory returns no target, targets disagree with Support Pack identities,
    /// or a default managed deployment cannot publish its platform.
    /// </exception>
    public IReadOnlyList<IGameBuildTarget> CreateTargets(
        AssetPipeline assets,
        SerializationRegistry serialization,
        TypeCatalog types
    ) {
        ArgumentNullException.ThrowIfNull(assets);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(types);
        var targets = new IGameBuildTarget[m_targetFactories.Length];
        var identities = new HashSet<BuildTargetId>();
        for (int index = 0; index < targets.Length; index++)
        {
            IGameBuildTarget target = m_targetFactories[index](assets, serialization, types)
                ?? throw new InvalidOperationException("A build target factory returned no target.");
            if (!m_targetIds.Contains(target.id) || !identities.Add(target.id))
                throw new InvalidOperationException($"Build target '{target.id}' does not match the distribution's unique Support Pack sources.");
            managedDeployments.Resolve(target.defaultManagedDeployment, target.runtimeIdentifier);
            targets[index] = target;
        }
        return Array.AsReadOnly(targets);
    }

    /// <summary>
    /// Rejects unsupported target and deployment pairs before any staging or publication begins.
    /// </summary>
    /// <param name="target">
    /// A target composed by this distribution.
    /// </param>
    /// <param name="deployment">
    /// The explicit deployment selected for publication.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// The platform is absent or the deployment lacks its runtime capability.
    /// </exception>
    public void ValidateDeployment(
        IGameBuildTarget target,
        ManagedDeploymentId deployment
    ) {
        ArgumentNullException.ThrowIfNull(target);
        if (!m_targetIds.Contains(target.id))
            throw new InvalidOperationException($"Build target '{target.id}' is not registered in this distribution.");
        managedDeployments.Resolve(deployment, target.runtimeIdentifier);
    }
}
