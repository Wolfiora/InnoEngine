using Inno.Build.Managed;
using Inno.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Plugins.Authoring;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Modules;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scripting.Compiler;

namespace Inno.Build;

internal sealed class GameBuildPipeline
{
    private readonly AssetPipeline m_assets;
    private readonly PluginEnvironment m_plugins;
    private readonly ProjectSettingsStore m_settings;
    private readonly SerializationRegistry m_serialization;
    private readonly ScriptCompiler m_compiler;
    private readonly IReadOnlyDictionary<BuildTargetId, GameBuildTargetBinding> m_targets;
    private readonly PlayerSupportPackCatalog m_supportPacks;
    private readonly ManagedDeploymentCatalog m_managedDeployments;

    internal GameBuildPipeline(
        AssetPipeline assets,
        PluginEnvironment plugins,
        ProjectSettingsStore settings,
        SerializationRegistry serialization,
        ScriptCompiler compiler,
        IReadOnlyDictionary<BuildTargetId, GameBuildTargetBinding> targets,
        PlayerSupportPackCatalog supportPacks,
        ManagedDeploymentCatalog managedDeployments
    ) {
        m_assets = assets;
        m_plugins = plugins;
        m_settings = settings;
        m_serialization = serialization;
        m_compiler = compiler;
        m_targets = targets;
        m_supportPacks = supportPacks;
        m_managedDeployments = managedDeployments;
    }

    internal async ValueTask<BuildResult> BuildAsync(
        GameBuildRequest request,
        IProgress<BuildProgress>? progress,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(request);
        request.Validate();
        if (!m_targets.TryGetValue(request.profile.target, out GameBuildTargetBinding? binding))
            throw new InvalidOperationException($"No game packager is registered for '{request.profile.target}'.");
        IGameBuildTarget target = binding.packager;
        IManagedDeploymentCompiler managedCompiler = m_managedDeployments.Resolve(
            request.profile.managedDeployment ?? target.defaultManagedDeployment, target.runtimeIdentifier);
        if (!m_assets.isInitialized)
            throw new InvalidOperationException("Game build requires an active authoring asset database.");
        string outputRoot = Path.GetFullPath(request.outputDirectory);
        ValidateOutputRoot(outputRoot);
        cancellationToken.ThrowIfCancellationRequested();
        progress?.Report(new BuildProgress("support-pack", 0.01d, "Checking the prepared Player Support Pack."));
        string supportPack = m_supportPacks.Resolve(request.profile.target, target);
        m_assets.WaitForIdle();
        AssetPath startupPath = AssetPath.Parse(request.profile.startupScene);
        if (!m_assets.TryGetAssetType(startupPath, out Type? sceneType) || sceneType != typeof(SceneAsset))
            throw new InvalidOperationException($"Startup scene '{startupPath}' is not an imported Scene asset.");
        if (AssetSample.IsRuntimeExcluded(startupPath, isDirectory: false))
        {
            throw new InvalidOperationException(
                $"Startup scene '{startupPath}' belongs to an authoring-only '~' sample directory " +
                "and cannot be included in a Player build.");
        }

        long assetRevision = m_assets.revision;
        long pluginRevision = m_plugins.revision;
        long settingsRevision = m_settings.revision;
        PluginCandidate[] plugins = m_plugins.activePlugins.ToArray();
        byte[] settings = m_settings.CaptureDocument();
        using SerializationGeneration serialization = m_serialization.CaptureGeneration();
        progress?.Report(new BuildProgress("snapshot", 0.05d, "Captured the combined authoring generation."));

        Directory.CreateDirectory(outputRoot);
        string staging = Path.Combine(outputRoot, ".inno-build-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(staging);
        using var stagingCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        Task<AssetRuntimeContentInfo>? assetExportTask = null;
        Task? targetContentTask = null;
        try
        {
            CancellationToken stagingToken = stagingCancellation.Token;
            stagingToken.ThrowIfCancellationRequested();
            string rawContent = Path.Combine(staging, "RawContent");
            string targetContent = Path.Combine(staging, "TargetContent");
            progress?.Report(new BuildProgress("content", 0.12d, "Exporting the runtime artifact closure."));
            assetExportTask = m_assets.ExportRuntimeArtifactsAsync(
                rawContent,
                stagingToken);
            targetContentTask = binding.compiler.CompileAsync(
                    new GameBuildContentContext(request.profile, targetContent, serialization),
                    stagingToken)
                .AsTask();

            ScriptCompilationResult compilation = await m_compiler.CompileRuntimeDeploymentAsync(
                    Path.Combine(supportPack, "References"),
                    new ScriptBuildProgress(progress),
                    stagingToken)
                .ConfigureAwait(false);
            if (!compilation.success)
            {
                stagingCancellation.Cancel();
                BuildDiagnostic[] diagnostics = compilation.diagnostics.Select(ToBuildDiagnostic).ToArray();
                return BuildResult.Failure(request.profile.target, diagnostics);
            }

            string[] assemblies = compilation.runtimeAssemblyPaths
                .Select(Path.GetFullPath)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(static value => value, StringComparer.OrdinalIgnoreCase)
                .ToArray();
            if (assemblies.Length == 0 || assemblies.Any(static path => !File.Exists(path)))
            {
                stagingCancellation.Cancel();
                return BuildResult.Failure(
                    request.profile.target,
                    [new BuildDiagnostic(
                        BuildDiagnosticSeverity.Error,
                        "INNOBUILD1001",
                        "Runtime script compilation did not produce a complete assembly generation.")]);
            }
            GameRuntimeModule[] runtimeModules = CreateRuntimeModules(compilation.moduleDeployments);
            byte[] runtimeManifest = RuntimeManifestEnvelope.Encode(
                CreateManifest(request.profile, plugins, runtimeModules),
                serialization);
            EnsureGenerationUnchanged(assetRevision, pluginRevision, settingsRevision);
            progress?.Report(new BuildProgress("prepare", 0.28d, "Prepared the runtime script and manifest generation."));

            AssetRuntimeContentInfo assetInfo = await assetExportTask.ConfigureAwait(false);
            await targetContentTask.ConfigureAwait(false);
            await BuildFileSystem.MergeDirectoryAsync(targetContent, rawContent, stagingToken)
                .ConfigureAwait(false);
            EnsureGenerationUnchanged(assetRevision, pluginRevision, settingsRevision);

            await File.WriteAllBytesAsync(
                    Path.Combine(rawContent, SettingsFileNames.project),
                    settings,
                    stagingToken)
                .ConfigureAwait(false);
            string managed = Path.Combine(staging, "CodeInputs");
            Directory.CreateDirectory(managed);
            for (int index = 0; index < assemblies.Length; index++)
            {
                stagingToken.ThrowIfCancellationRequested();
                string destination = Path.Combine(managed, Path.GetFileName(assemblies[index]));
                if (File.Exists(destination))
                    throw new InvalidOperationException($"Runtime assembly '{Path.GetFileName(destination)}' is duplicated.");
                await BuildFileSystem.CopyFileAsync(assemblies[index], destination, stagingToken)
                    .ConfigureAwait(false);
            }

            string snapshotFingerprint = await BuildSnapshotFingerprint.ComputeAsync(
                    assetRevision,
                    assemblies,
                    plugins,
                    settings,
                    stagingToken)
                .ConfigureAwait(false);
            string packagedContent = Path.Combine(staging, "PackagedContent");
            Directory.CreateDirectory(packagedContent);
            await File.WriteAllBytesAsync(
                    Path.Combine(packagedContent, "runtime.manifest"),
                    runtimeManifest,
                    stagingToken)
                .ConfigureAwait(false);

            progress?.Report(new BuildProgress("pack", 0.55d, "Writing the deterministic content pack."));
            (_, string contentHash) = await ContentPackWriter.WriteAsync(
                    rawContent,
                    packagedContent,
                    serialization,
                    stagingToken)
                .ConfigureAwait(false);
            var catalog = new RuntimeContentCatalog
            {
                contentHash = contentHash,
                packFileName = $"content-{contentHash}.pack",
                snapshotFingerprint = snapshotFingerprint,
                assetCount = assetInfo.assetCount,
                artifactBundleCount = assetInfo.artifactBundleCount,
                runtimeAssemblyCount = assemblies.Length
            };
            catalog.Validate();
            await File.WriteAllBytesAsync(
                    Path.Combine(packagedContent, "catalog.inno"),
                    serialization.Serialize(catalog),
                    stagingToken)
                .ConfigureAwait(false);
            EnsureGenerationUnchanged(assetRevision, pluginRevision, settingsRevision);

            string playerLink = Path.Combine(staging, "PlayerLink");
            await BuildFileSystem.CopyDirectoryAsync(Path.Combine(supportPack, "PlayerLink"),
                playerLink, stagingToken).ConfigureAwait(false);
            await PlayerCodeCompositionWriter.WriteAsync(playerLink,
                GameCodeDeployment.FromManifest(runtimeModules), stagingToken).ConfigureAwait(false);
            progress?.Report(new BuildProgress("managed", 0.62d, "Publishing the frozen game code deployment."));
            string managedStaging = Path.Combine(staging, "ManagedDeployment");
            ManagedDeploymentResult managedDeployment = await managedCompiler.CompileAsync(
                new ManagedDeploymentRequest(Path.Combine(playerLink, "Player.csproj"),
                    target.runtimeIdentifier, managed, managedStaging,
                    Path.Combine(outputRoot, ".build-logs", Path.GetFileName(staging))), stagingToken).ConfigureAwait(false);
            ValidateManagedPublication(managedCompiler.id, managedStaging, managedDeployment);
            EnsureGenerationUnchanged(assetRevision, pluginRevision, settingsRevision);

            string platformStaging = Path.Combine(staging, "Platform");
            Directory.CreateDirectory(platformStaging);
            progress?.Report(new BuildProgress("package", 0.78d, $"Composing {request.profile.target} Player output."));
            string composed = await target.PackageAsync(
                    new GameBuildPackageContext(request.profile, supportPack, packagedContent, managedDeployment, platformStaging),
                    stagingToken)
                .ConfigureAwait(false);
            string normalizedComposed = Path.GetFullPath(composed);
            if (!IsWithin(platformStaging, normalizedComposed))
                throw new InvalidOperationException("A game target returned output outside its staging directory.");
            if (!Directory.Exists(normalizedComposed))
                throw new DirectoryNotFoundException("The game target did not create its declared output directory.");
            stagingToken.ThrowIfCancellationRequested();
            string final = Path.Combine(outputRoot, Path.GetFileName(normalizedComposed));
            BuildFileSystem.InstallDirectoryAtomically(normalizedComposed, final);
            progress?.Report(new BuildProgress("commit", 1d, "Game build completed."));
            return BuildResult.Success(
                final,
                request.profile.target,
                contentHash,
                assetInfo.assetCount,
                assetInfo.artifactBundleCount,
                assemblies.Length,
                embeddedPluginCount: 0);
        }
        finally
        {
            stagingCancellation.Cancel();
            await ObserveCompletionAsync(assetExportTask).ConfigureAwait(false);
            await ObserveCompletionAsync(targetContentTask).ConfigureAwait(false);
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
        }
    }

    private static async ValueTask ObserveCompletionAsync(Task? task)
    {
        if (task is null)
            return;
        try
        {
            await task.ConfigureAwait(false);
        }
        catch
        {
            // The primary build path reports the stage failure; cleanup only observes task completion.
        }
    }

    private static void ValidateManagedPublication(
        ManagedDeploymentId deployment,
        string staging,
        ManagedDeploymentResult result
    ) {
        ArgumentNullException.ThrowIfNull(result);
        StringComparison comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        if (result.deployment != deployment || !string.Equals(Path.GetFullPath(staging), result.outputDirectory, comparison)
            || !Directory.Exists(result.outputDirectory))
            throw new InvalidDataException("The managed publisher returned foreign or absent staging output.");
        string[] actualFiles = Directory.EnumerateFiles(result.outputDirectory, "*", SearchOption.AllDirectories)
            .Select(file => Path.GetRelativePath(result.outputDirectory, file)).Order(StringComparer.Ordinal).ToArray();
        if (!actualFiles.SequenceEqual(result.files.Order(StringComparer.Ordinal), StringComparer.Ordinal))
            throw new InvalidDataException("The managed publication evidence differs from its staged output.");
    }

    private static BuildDiagnostic ToBuildDiagnostic(ScriptDiagnostic diagnostic)
    {
        BuildDiagnosticSeverity severity = diagnostic.severity switch
        {
            DiagnosticSeverity.Error => BuildDiagnosticSeverity.Error,
            DiagnosticSeverity.Warning => BuildDiagnosticSeverity.Warning,
            _ => BuildDiagnosticSeverity.Information
        };
        string location = diagnostic.filePath is null
            ? string.Empty
            : $" ({diagnostic.filePath}:{diagnostic.line}:{diagnostic.column})";
        return new BuildDiagnostic(severity, diagnostic.id, diagnostic.message + location);
    }

    private static GameRuntimeManifest CreateManifest(
        BuildProfile profile,
        IReadOnlyList<PluginCandidate> plugins,
        GameRuntimeModule[] modules
    ) {
        var manifest = new GameRuntimeManifest
        {
            applicationId = profile.applicationId,
            persistentDataPath = profile.persistentDataPath,
            productName = profile.productName,
            startupScene = AssetPath.Parse(profile.startupScene).ToString(),
            windowWidth = profile.windowWidth,
            windowHeight = profile.windowHeight,
            modules = modules,
            plugins = plugins.Select(static plugin => new GameRuntimePlugin
            {
                id = plugin.manifest.pluginId,
                dependencies = plugin.manifest.dependencies.ToArray(),
                overrides = plugin.manifest.overrides.ToArray(),
                settings = plugin.manifest.settingContributions.ToArray()
            }).ToArray()
        };
        manifest.Validate();
        return manifest;
    }

    private static GameRuntimeModule[] CreateRuntimeModules(IReadOnlyList<ScriptModuleDeployment> requests)
    {
        var selected = requests
            .Select(request => new
            {
                request,
                assemblies = new[] { request.mainAssemblyPath }
                    .Concat(request.preloadAssemblyPaths)
                    .Where(path => request.assemblyScopes.TryGetValue(
                            Path.GetFileNameWithoutExtension(path),
                            out AssemblyScope scope)
                        ? scope == AssemblyScope.Runtime
                        : request.scope == AssemblyScope.Runtime)
                    .ToArray()
            })
            .Where(static candidate => candidate.assemblies.Length > 0)
            .ToArray();
        var includedNames = selected
            .Select(static candidate => candidate.request.moduleName)
            .ToHashSet(StringComparer.Ordinal);
        GameRuntimeModule[] candidates = selected.Select(candidate => new GameRuntimeModule
        {
            name = candidate.request.moduleName,
            domain = candidate.request.domain,
            assemblies = candidate.assemblies.Select(static path => new GameRuntimeAssembly
            {
                name = AssemblyName.GetAssemblyName(path).Name!,
                contentFingerprint = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(path)))
            }).ToArray(),
            dependencies = candidate.request.upstreamModuleNames
                .Where(includedNames.Contains)
                .ToArray()
        }).ToArray();
        var byName = candidates.ToDictionary(static module => module.name, StringComparer.Ordinal);
        var visiting = new HashSet<string>(StringComparer.Ordinal);
        var visited = new HashSet<string>(StringComparer.Ordinal);
        var ordered = new List<GameRuntimeModule>(candidates.Length);
        foreach (GameRuntimeModule module in candidates.OrderBy(static module => module.name, StringComparer.Ordinal))
            Visit(module);
        return ordered.ToArray();

        void Visit(GameRuntimeModule module)
        {
            if (visited.Contains(module.name))
                return;
            if (!visiting.Add(module.name))
                throw new InvalidOperationException($"Runtime module dependency cycle contains '{module.name}'.");
            foreach (string dependencyName in module.dependencies.Order(StringComparer.Ordinal))
            {
                if (!byName.TryGetValue(dependencyName, out GameRuntimeModule? dependency))
                {
                    throw new InvalidOperationException(
                        $"Runtime module '{module.name}' depends on missing module '{dependencyName}'.");
                }
                Visit(dependency);
            }
            visiting.Remove(module.name);
            visited.Add(module.name);
            ordered.Add(module);
        }
    }

    private void EnsureGenerationUnchanged(
        long expectedAssetRevision,
        long expectedPluginRevision,
        long expectedSettingsRevision
    ) {
        if (m_assets.revision != expectedAssetRevision
            || m_plugins.revision != expectedPluginRevision
            || m_settings.revision != expectedSettingsRevision)
        {
            throw new InvalidOperationException(
                "The active Asset, Plugin, or Project Settings generation changed during the build. " +
                "No output was committed; start a new build from the latest generation.");
        }
    }

    private void ValidateOutputRoot(string outputRoot)
    {
        var protectedRoots = m_assets.sourceMounts
            .Select(static mount => mount.rootPath)
            .Append(m_assets.libraryRoot)
            .ToList();
        string? projectRoot = Directory.GetParent(m_assets.assetRoot)?.FullName;
        if (projectRoot is not null)
            protectedRoots.Add(Path.Combine(projectRoot, "Plugins"));
        string? conflict = protectedRoots.FirstOrDefault(root => IsWithin(root, outputRoot));
        if (conflict is not null)
        {
            throw new InvalidOperationException(
                $"Build output cannot be written inside managed project content '{Path.GetFullPath(conflict)}'.");
        }
    }

    private static bool IsWithin(
        string root,
        string candidate
    ) {
        string normalizedRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
        StringComparison comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        return string.Equals(candidate, normalizedRoot, comparison)
               || candidate.StartsWith(normalizedRoot + Path.DirectorySeparatorChar, comparison);
    }

    private sealed class ScriptBuildProgress(IProgress<BuildProgress>? progress)
        : IProgress<ScriptCompilationProgress>
    {
        /// <summary>
        /// Publishes one progress update to the receiving workflow.
        /// </summary>
        /// <param name="value">
        /// The concrete value read or transformed by this operation.
        /// </param>
        public void Report(ScriptCompilationProgress value)
            => progress?.Report(new BuildProgress(
                "scripting",
                0.05d + value.fraction * 0.2d,
                value.stage));
    }
}
