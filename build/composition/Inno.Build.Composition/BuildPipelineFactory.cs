using System;
using Inno.Assets.Pipeline;
using Inno.Build.SupportPacks;
using Inno.Core.Settings;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scripting.Compiler;

namespace Inno.Build.Composition;

/// <summary>
/// Creates common build pipelines from a single distribution and explicitly borrowed authoring owners.
/// </summary>
public static class BuildPipelineFactory
{
    /// <summary>
    /// Validates and binds the distribution to one authoring host without starting a build.
    /// </summary>
    /// <param name="context">
    /// The SDK selection and host location used for optional source provisioning.
    /// </param>
    /// <param name="distribution">
    /// The frozen platform and deployment registrations shared by all entry points.
    /// </param>
    /// <param name="engine">
    /// The borrowed engine owning type, serialization, and generation transactions.
    /// </param>
    /// <param name="assets">
    /// The borrowed active authoring assets.
    /// </param>
    /// <param name="plugins">
    /// The borrowed active plugin generation.
    /// </param>
    /// <param name="settings">
    /// The borrowed project settings owner.
    /// </param>
    /// <param name="compiler">
    /// The borrowed compiler that freezes project code for each publication.
    /// </param>
    /// <param name="supportPackRoot">
    /// The host's installed Support Pack location.
    /// </param>
    /// <returns>
    /// A pipeline retaining the supplied owners without taking responsibility for their disposal.
    /// </returns>
    public static BuildPipeline Create(
        BuildCompositionContext context,
        BuildDistribution distribution,
        EngineHost engine,
        AssetPipeline assets,
        PluginEnvironment plugins,
        ProjectSettingsStore settings,
        ScriptCompiler compiler,
        string supportPackRoot
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(distribution);
        ArgumentNullException.ThrowIfNull(engine);
        return new BuildPipeline(
            assets, plugins, settings, engine.serialization, engine.generations, compiler, supportPackRoot,
            distribution.CreateTargets(assets, engine.serialization, engine.types),
            distribution.managedDeployments,
            SourcePlayerSupportPackProvisioner.TryCreateForHost(
                context.applicationDirectory, distribution.supportPacks, context.dotnetHost));
    }
}
