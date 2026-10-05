using System;
using Inno.Build.Managed;
using Inno.Extensibility.Reload;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Plugins.Authoring;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Scripting.Compiler;

namespace Inno.Build;

/// <summary>
/// Orchestrates deterministic Plugin and game builds over isolated staging and atomic commits.
/// </summary>
public sealed class BuildPipeline
{
    private readonly IReadOnlyList<BuildTargetId> m_availableGameTargets;
    private readonly BuildTargetId m_defaultGameTarget;
    private readonly IReadOnlyDictionary<BuildTargetId, IGameBuildTarget> m_gameTargets;
    private readonly GameBuildPipeline m_game;
    private readonly PluginPackageBuilder m_plugins;
    private readonly PlayerSupportPackCatalog m_supportPacks;
    private readonly IPlayerSupportPackProvisioner? m_supportPackProvisioner;
    private readonly GenerationCoordinator m_generations;
    private readonly ManagedDeploymentCatalog m_managedDeployments;

    /// <summary>
    /// Creates a build pipeline from installed Player Support Packs and platform packagers.
    /// </summary>
    /// <param name="supportPackRoot">
    /// The directory containing one child directory per <see cref="BuildTargetId"/>.
    /// </param>
    /// <param name="assets">
    /// The active authoring asset pipeline captured by builds.
    /// </param>
    /// <param name="generations">
    /// The host-wide admission gate that remains pinned until the build snapshot is released.
    /// </param>
    /// <param name="plugins">
    /// The active Plugin environment captured by builds.
    /// </param>
    /// <param name="settings">
    /// The current project settings store captured by builds.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry used to pin and encode the build snapshot.
    /// </param>
    /// <param name="compiler">
    /// The project compiler used to produce a fresh runtime-only assembly generation for each game build.
    /// </param>
    /// <param name="gameTargets">
    /// The complete set of replaceable platform package implementations available to this host.
    /// </param>
    /// <param name="managedDeployments">
    /// Explicit managed publishers selected independently of the platform packagers.
    /// </param>
    /// <param name="supportPackProvisioner">
    /// Optional build-time provider that checks current source and SDK inputs during explicit pack preparation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the Support Pack root is empty, no target is provided, a target identity is duplicated,
    /// or multiple targets claim current-host preference.
    /// </exception>
    public BuildPipeline(
        AssetPipeline assets,
        PluginEnvironment plugins,
        ProjectSettingsStore settings,
        SerializationRegistry serialization,
        GenerationCoordinator generations,
        ScriptCompiler compiler,
        string supportPackRoot,
        IEnumerable<IGameBuildTarget> gameTargets,
        ManagedDeploymentCatalog managedDeployments,
        IPlayerSupportPackProvisioner? supportPackProvisioner = null
    ) {
        ArgumentNullException.ThrowIfNull(assets);
        ArgumentNullException.ThrowIfNull(plugins);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(serialization);
        m_generations = generations ?? throw new ArgumentNullException(nameof(generations));
        ArgumentNullException.ThrowIfNull(compiler);
        ArgumentException.ThrowIfNullOrWhiteSpace(supportPackRoot);
        ArgumentNullException.ThrowIfNull(gameTargets);
        ArgumentNullException.ThrowIfNull(managedDeployments);
        m_managedDeployments = managedDeployments;
        IGameBuildTarget[] targets = gameTargets.ToArray();
        if (targets.Any(static value => value is null))
            throw new ArgumentException("Game target collection cannot contain null values.", nameof(gameTargets));
        if (targets.Length == 0)
            throw new ArgumentException("At least one game build target is required.", nameof(gameTargets));
        if (targets.Any(static value => string.IsNullOrWhiteSpace(value.id.value)))
            throw new ArgumentException("Every game build target requires a valid identity.", nameof(gameTargets));
        if (targets.Any(static value => string.IsNullOrWhiteSpace(value.displayName)))
            throw new ArgumentException("Every game build target requires a display name.", nameof(gameTargets));
        IGrouping<BuildTargetId, IGameBuildTarget>? duplicate = targets
            .GroupBy(static value => value.id)
            .FirstOrDefault(static group => group.Count() > 1);
        if (duplicate is not null)
            throw new ArgumentException($"Game target '{duplicate.Key}' is registered more than once.", nameof(gameTargets));
        IGameBuildTarget[] preferred = targets
            .Where(static target => target.isPreferredOnCurrentHost)
            .OrderBy(static target => target.id.value, StringComparer.Ordinal)
            .ToArray();
        if (preferred.Length > 1)
        {
            throw new ArgumentException(
                "More than one game build target is preferred on the current host.",
                nameof(gameTargets));
        }
        m_availableGameTargets = Array.AsReadOnly(targets
            .Select(static target => target.id)
            .OrderBy(static id => id.value, StringComparer.Ordinal)
            .ToArray());
        m_defaultGameTarget = preferred.Length == 1
            ? preferred[0].id
            : m_availableGameTargets[0];
        m_gameTargets = targets.ToDictionary(static value => value.id);
        m_supportPacks = new PlayerSupportPackCatalog(supportPackRoot);
        m_supportPackProvisioner = supportPackProvisioner;
        m_game = new GameBuildPipeline(
            assets,
            plugins,
            settings,
            serialization,
            compiler,
            m_gameTargets,
            m_supportPacks,
            managedDeployments);
        m_plugins = new PluginPackageBuilder(
            assets,
            plugins,
            settings,
            serialization);
    }

    /// <summary>
    /// Gets an immutable view of every registered build target in stable identity order.
    /// </summary>
    public IReadOnlyList<BuildTargetId> availableGameTargets => m_availableGameTargets;

    /// <summary>
    /// Gets the single adapter-selected target preferred for new build settings on this host.
    /// </summary>
    public BuildTargetId defaultGameTarget => m_defaultGameTarget;

    /// <summary>
    /// Gets the managed deployment choices supported by a registered publication platform.
    /// </summary>
    /// <param name="target">
    /// The registered publication target whose managed runtime requirements are queried.
    /// </param>
    /// <returns>
    /// Immutable deployment identities in stable order; an unknown target throws explicitly.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The publication target is not registered in this build composition.
    /// </exception>
    public IReadOnlyList<ManagedDeploymentId> GetManagedDeployments(BuildTargetId target)
        => m_gameTargets.TryGetValue(target, out IGameBuildTarget? gameTarget)
            ? m_managedDeployments.GetSupportedDeployments(gameTarget.runtimeIdentifier)
            : throw new ArgumentException($"Game target '{target}' is not registered.", nameof(target));

    /// <summary>
    /// Tries to get the adapter-owned display name for a registered game target.
    /// </summary>
    /// <param name="target">
    /// The stable target identity to query.
    /// </param>
    /// <param name="displayName">
    /// Receives the registered user-facing name, or an empty value when no target is registered.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the target is registered; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetGameTargetDisplayName(
        BuildTargetId target,
        out string displayName
    ) {
        if (m_gameTargets.TryGetValue(target, out IGameBuildTarget? gameTarget))
        {
            displayName = gameTarget.displayName;
            return true;
        }
        displayName = string.Empty;
        return false;
    }

    /// <summary>
    /// Asynchronously prepares and verifies a target Support Pack before a game build starts.
    /// </summary>
    /// <param name="target">
    /// The registered platform and architecture target to prepare.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation before the Pack installation commits.
    /// </param>
    /// <returns>
    /// The verified target Pack directory. No authoring asset database operations run during preparation.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// No game target is registered for the requested identity.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The Pack is absent and this host has no provisioner.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// An installed or newly published Pack fails deployment validation.
    /// </exception>
    public async ValueTask<string> EnsurePlayerSupportPackAsync(
        BuildTargetId target,
        CancellationToken cancellationToken = default
    ) {
        if (!m_gameTargets.TryGetValue(target, out IGameBuildTarget? packager))
            throw new NotSupportedException($"Game target '{target}' is not registered.");
        return await m_supportPacks.ResolveOrProvisionAsync(
            target, packager, m_supportPackProvisioner, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Builds and installs one complete source-free game deployment with rollback protection.
    /// </summary>
    /// <remarks>
    /// Prepare current packs with EnsurePlayerSupportPackAsync before invoking this method on the
    /// authoring owner's thread. The synchronous snapshot phase never waits for asynchronous provisioning.
    /// Coordinate readers and writers of the output directory during its two installation moves.
    /// Backup cleanup failures preserve the installed candidate and report its remaining backup.
    /// </remarks>
    /// <param name="request">
    /// The exact profile, destination, and activated runtime compilation generation.
    /// </param>
    /// <param name="progress">
    /// Optional observer for monotonic stage progress.
    /// </param>
    /// <param name="cancellationToken">
    /// The token that cancels work before directory installation begins.
    /// </param>
    /// <returns>
    /// The durable output identity and deployment metrics.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// Thrown when cancellation is requested before commit.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The target Support Pack is missing; prepare it before starting the owner-thread build snapshot.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The target Support Pack is present but invalid.
    /// </exception>
    /// <exception cref="IOException">
    /// Output installation fails, or cleanup of the previous output's backup fails after commit.
    /// </exception>
    /// <exception cref="AggregateException">
    /// Output installation and restoration both fail; the previous tree remains at the reported backup path.
    /// </exception>
    public async ValueTask<BuildResult> BuildGameAsync(
        GameBuildRequest request,
        IProgress<BuildProgress>? progress = null,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(request);
        request.Validate();
        var snapshot = new GameBuildRequest
        {
            profile = request.profile.Copy(),
            outputDirectory = Path.GetFullPath(request.outputDirectory)
        };
        using IDisposable admission = m_generations.AcquireRead("build a Player");
        return await m_game.BuildAsync(snapshot, progress, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Builds and atomically commits one deterministic project Plugin package.
    /// </summary>
    /// <param name="request">
    /// The package identity, destination, and dependency embedding policy.
    /// </param>
    /// <param name="progress">
    /// Optional observer for monotonic stage progress.
    /// </param>
    /// <param name="cancellationToken">
    /// The token that cancels work before atomic commit.
    /// </param>
    /// <returns>
    /// The durable package identity and source-content metrics.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// Thrown when cancellation is requested before commit.
    /// </exception>
    public async ValueTask<BuildResult> BuildPluginAsync(
        PluginBuildRequest request,
        IProgress<BuildProgress>? progress = null,
        CancellationToken cancellationToken = default
    ) {
        using IDisposable admission = m_generations.AcquireRead("export a Plugin");
        return await m_plugins.BuildAsync(request, progress, cancellationToken).ConfigureAwait(false);
    }
}
