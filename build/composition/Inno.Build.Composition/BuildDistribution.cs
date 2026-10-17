using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Inno.Assets.Pipeline;
using Inno.Build.Managed;
using Inno.Build.SupportPacks;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Build.Toolchains;

namespace Inno.Build.Composition;

/// <summary>
/// Freezes platform factories, managed publishers, and matching Support Pack sources as one distribution.
/// </summary>
public sealed class BuildDistribution
{
    private readonly GameBuildContribution[] m_games;
    private readonly BuildPlatformContribution[] m_platforms;
    private readonly IReadOnlyList<BuildPlatformContribution> m_platformSnapshot;
    private readonly HashSet<BuildTargetId> m_targetIds;
    private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, ProductNativeBuildPlan>> m_nativeProducts;
    private readonly IReadOnlyDictionary<string, INativeToolchainProvider> m_nativeToolchains;

    /// <summary>
    /// Captures a complete distribution without creating authoring services or starting any tool.
    /// </summary>
    /// <param name="games">
    /// Complete target contributions with explicitly bound factories, sources and SDK providers.
    /// </param>
    /// <param name="managedCompilers">
    /// The deployment implementations owned by this composition.
    /// </param>
    /// <param name="nativeOnlyContributions">
    /// Additional implemented SDK capabilities that do not register game publication targets.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// A required collection is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Contributions are empty, null, duplicated, or unassigned.
    /// </exception>
    public BuildDistribution(
        IReadOnlyList<GameBuildContribution> games,
        IReadOnlyList<IManagedDeploymentCompiler> managedCompilers,
        IReadOnlyList<NativeToolchainContribution>? nativeOnlyContributions = null
    ) {
        ArgumentNullException.ThrowIfNull(games);
        ArgumentNullException.ThrowIfNull(managedCompilers);
        if (games.Count == 0 || games.Any(static contribution => contribution is null))
            throw new ArgumentException("A distribution requires complete platform contributions.", nameof(games));
        m_games = games.ToArray();
        m_platforms = m_games.Select(static game => game.platform).ToArray();
        m_platformSnapshot = Array.AsReadOnly(m_platforms);
        m_targetIds = m_platforms.Select(static contribution => contribution.descriptor.id).ToHashSet();
        if (m_targetIds.Count != m_platforms.Length)
            throw new ArgumentException("Platform target identities must be unique.", nameof(games));
        var native = m_platforms.ToDictionary(static platform => platform.descriptor.id.value,
            static platform => platform.toolchainProvider, StringComparer.Ordinal);
        var products = m_platforms.ToDictionary(static platform => platform.descriptor.id.value,
            static platform => platform.nativeProducts, StringComparer.Ordinal);
        foreach (NativeToolchainContribution contribution in nativeOnlyContributions ?? [])
        {
            if (contribution is null || !native.TryAdd(contribution.descriptor.id.value, contribution.provider))
                throw new ArgumentException("Native SDK contributions must be complete and have unique target identities.", nameof(nativeOnlyContributions));
            products.Add(contribution.descriptor.id.value, contribution.nativeProducts);
        }
        m_nativeProducts = new System.Collections.ObjectModel.ReadOnlyDictionary<string,
            IReadOnlyDictionary<string, ProductNativeBuildPlan>>(products);
        m_nativeToolchains = new System.Collections.ObjectModel.ReadOnlyDictionary<string, INativeToolchainProvider>(native);
        availableTargets = Array.AsReadOnly(m_targetIds.OrderBy(static id => id.value, StringComparer.Ordinal).ToArray());
        managedDeployments = new ManagedDeploymentCatalog(managedCompilers);
        supportPacks = new PlayerSupportPackPublisher(m_platforms.Select(static contribution => contribution.supportPackSource).ToArray());
    }

    /// <summary>
    /// Gets the immutable platform identities shared by all hosts.
    /// </summary>
    public IReadOnlyList<BuildTargetId> availableTargets { get; }

    /// <summary>
    /// Gets complete platform registrations in their explicit composition order.
    /// </summary>
    public IReadOnlyList<BuildPlatformContribution> platforms => m_platformSnapshot;

    /// <summary>
    /// Gets the immutable deployment implementation catalog.
    /// </summary>
    public ManagedDeploymentCatalog managedDeployments { get; }

    /// <summary>
    /// Gets the publisher containing the same platform input providers.
    /// </summary>
    public PlayerSupportPackPublisher supportPacks { get; }

    /// <summary>
    /// Resolves the SDK integration bound to an explicitly registered publication target.
    /// </summary>
    /// <param name="targetId">
    /// The requested native target identity, independently selected from the execution host.
    /// </param>
    /// <returns>
    /// The borrowed provider; resolving it freezes tools before component work begins.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// No native toolchain contribution is registered for the target.
    /// </exception>
    public INativeToolchainProvider ResolveNativeToolchain(string targetId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        if (m_nativeToolchains.TryGetValue(targetId, out INativeToolchainProvider? provider))
            return provider;
        throw new NotSupportedException($"No native toolchain is registered for target '{targetId}'.");
    }

    /// <summary>
    /// Checks whether this distribution contributes an SDK for an explicitly requested binding target.
    /// A standalone binding definition may describe a target without registering a product publisher.
    /// </summary>
    /// <param name="targetId">
    /// The exact target identity; no host-derived target is substituted.
    /// </param>
    /// <param name="provider">
    /// The borrowed SDK provider when registered, or null when the distribution has no such contribution.
    /// </param>
    /// <returns>
    /// True when the target has an explicit SDK contribution; false does not imply SDK-free configuration.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The target identity is empty.
    /// </exception>
    public bool TryResolveNativeToolchain(
        string targetId,
        [NotNullWhen(true)] out INativeToolchainProvider? provider
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        return m_nativeToolchains.TryGetValue(targetId, out provider);
    }

    /// <summary>
    /// Resolves a platform-contributed product closure without selecting backends inside the invoking host.
    /// </summary>
    /// <param name="targetId">
    /// The explicit registered product target.
    /// </param>
    /// <param name="productId">
    /// The product whose ordinary native build is requested.
    /// </param>
    /// <returns>
    /// The borrowed immutable plan declared by the target's composition.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The target or its ordinary native product build is not registered.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A target or product identity is empty.
    /// </exception>
    public ProductNativeBuildPlan ResolveNativeProduct(
        string targetId,
        string productId
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        ArgumentException.ThrowIfNullOrWhiteSpace(productId);
        if (m_nativeProducts.TryGetValue(targetId, out IReadOnlyDictionary<string, ProductNativeBuildPlan>? products)
            && products.TryGetValue(productId, out ProductNativeBuildPlan? plan))
            return plan;
        throw new NotSupportedException($"No ordinary native build is registered for product '{productId}' on target '{targetId}'.");
    }

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
    public IReadOnlyList<GameBuildTargetBinding> CreateBindings(
        AssetPipeline assets,
        SerializationRegistry serialization,
        TypeCatalog types
    ) {
        ArgumentNullException.ThrowIfNull(assets);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(types);
        var targets = new GameBuildTargetBinding[m_games.Length];
        var identities = new HashSet<BuildTargetId>();
        for (int index = 0; index < targets.Length; index++)
        {
            BuildPlatformContribution platform = m_platforms[index];
            IGameBuildTarget target = platform.factory()
                ?? throw new InvalidOperationException("A build target factory returned no target.");
            if (target.id != platform.descriptor.id || target.runtimeIdentifier != platform.descriptor.runtimeIdentifier
                || !identities.Add(target.id))
                throw new InvalidOperationException($"Build target '{target.id}' does not match its bound platform description.");
            managedDeployments.Resolve(target.defaultManagedDeployment, target.runtimeIdentifier);
            IGameContentCompiler compiler = m_games[index].compilerFactory(assets, serialization, types)
                ?? throw new InvalidOperationException("A content compiler factory returned no compiler.");
            targets[index] = new GameBuildTargetBinding(target, compiler);
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
