using Inno.Extensibility.Modules;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using System;
using Inno.Build.Managed;
using Inno.Build.Managed.DotNet;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.Platform.MacOS;
using Inno.Build.Platform.Browser;
using Inno.Build.Platform.Windows;
using Inno.Build.SupportPacks;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.Settings;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scripting.Compiler;

namespace Inno.Build.Cli;

internal sealed class BuildWorkspace : IDisposable
{
    private readonly EngineHost m_engine;
    private readonly ProjectSettingsStore m_settings;
    private readonly AssetPipeline m_assets;
    private readonly PluginEnvironment m_plugins;
    private readonly ScriptCompiler m_compiler;
    private readonly string m_projectDirectory;

    private BuildWorkspace(
        EngineHost engine,
        ProjectSettingsStore settings,
        AssetPipeline assets,
        PluginEnvironment plugins,
        ScriptCompiler compiler,
        string projectDirectory,
        BuildPipeline pipeline
    ) {
        m_engine = engine;
        m_settings = settings;
        m_assets = assets;
        m_plugins = plugins;
        m_compiler = compiler;
        m_projectDirectory = projectDirectory;
        this.pipeline = pipeline;
    }

    internal BuildPipeline pipeline { get; }
    internal ProjectId projectId => m_settings.projectId;

    internal AssetPath ImportSample(
        AssetPath source,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        AssetSampleImportTransaction import = m_assets.PrepareSampleImport(source);
        try
        {
            while (!import.Advance())
            {
                cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(1);
            }
            import.BeginValidation(async (
                sources,
                cancellationToken
            ) =>
            {
                ScriptCompilationResult result = await m_compiler.CompileAuthoringGenerationAsync(
                    cancellationToken: cancellationToken, sourceSnapshot: sources).ConfigureAwait(false);
                if (!result.success)
                    throw new InvalidOperationException("Sample script preflight failed:" + Environment.NewLine
                        + string.Join(Environment.NewLine,
                            result.diagnostics.Select(static diagnostic => diagnostic.message)));
            });
            while (!import.isValidationComplete)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(1);
            }
            cancellationToken.ThrowIfCancellationRequested();
            import.Commit();
            return import.target;
        }
        finally
        {
            new RetirementBarrier("CLI sample import").Wait(import.Dispose);
        }
    }

    internal string ExportProjectScripts(string outputDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);
        ScriptCompilationResult result = m_compiler.CompileAuthoringGenerationAsync()
            .GetAwaiter().GetResult();
        if (!result.success)
            throw new InvalidOperationException("Project script compilation failed:" + Environment.NewLine
                + string.Join(Environment.NewLine,
                    result.diagnostics.Select(static diagnostic => diagnostic.message)));
        string[] assemblies = result.moduleDeployments
            .Where(static request => request.domain == Inno.Extensibility.Modules.AssemblyDomain.InnoScripting)
            .SelectMany(static request => new[] { request.mainAssemblyPath }
                .Concat(request.preloadAssemblyPaths))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        string destination = Path.GetFullPath(outputDirectory);
        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
        string backup = destination + ".backup-" + Guid.NewGuid().ToString("N");
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        Directory.CreateDirectory(staging);
        try
        {
            foreach (string assembly in assemblies)
            {
                File.Copy(assembly, Path.Combine(staging, Path.GetFileName(assembly)));
                string symbols = Path.ChangeExtension(assembly, ".pdb");
                if (File.Exists(symbols))
                    File.Copy(symbols, Path.Combine(staging, Path.GetFileName(symbols)));
            }
            if (Directory.Exists(destination))
                Directory.Move(destination, backup);
            try
            {
                Directory.Move(staging, destination);
            }
            catch
            {
                if (Directory.Exists(backup))
                    Directory.Move(backup, destination);
                throw;
            }
        }
        finally
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
            if (Directory.Exists(destination) && Directory.Exists(backup))
                Directory.Delete(backup, recursive: true);
        }
        return destination;
    }

    internal static BuildWorkspace Open(
        string projectDirectory,
        string supportPackRoot
    ) {
        string projectRoot = Path.GetFullPath(projectDirectory);
        string assetsRoot = Path.Combine(projectRoot, "Assets");
        string pluginsRoot = Path.Combine(projectRoot, "Plugins");
        string libraryRoot = Path.Combine(projectRoot, "Library");
        if (!Directory.Exists(assetsRoot))
            throw new DirectoryNotFoundException($"Project Assets directory '{assetsRoot}' does not exist.");
        Directory.CreateDirectory(pluginsRoot);
        Directory.CreateDirectory(libraryRoot);

        EngineHost? engine = null;
        ProjectSettingsStore? settings = null;
        AssetPipeline? assets = null;
        PluginEnvironment? plugins = null;
        try
        {
            engine = new EngineHostBuilder()
                .UseMetadataSources(new DotNetAssemblyCatalogSource(typeof(BuildWorkspace).Assembly),
                    new ReflectionTypeCatalogSource(), new ReflectionSerializationMetadataSource())
                .UseMetadataCache(Path.Combine(libraryRoot, "Build", "Metadata"))
                .Build();
            var sources = new PluginSourceService(engine.serialization, pluginsRoot, libraryRoot);
            PluginScanResult scan = sources.Scan();
            AssetPipelineOptions options = AssetPipelineOptions.Create(assetsRoot, libraryRoot);
            AssetSourceMount projectMount = options.sourceMounts!.Single(static mount =>
                mount.id == AssetSourceId.project);
            assets = new AssetPipeline(
                engine.modules,
                engine.types,
                engine.serialization,
                new IdentityAllocator(),
                engine.diagnostics,
                engine.logs,
                options with
                {
                    enableFileSystemWatcher = false,
                    deferUnavailableExtensions = true,
                    sourceMounts =
                    [
                        projectMount,
                        .. PluginSourceService.GetActivatableMounts(scan)
                    ]
                });
            settings = new ProjectSettingsStore(
                Path.Combine(projectRoot, SettingsFileNames.project),
                engine.types,
                engine.serialization,
                ProjectId.FromName(new DirectoryInfo(projectRoot).Name),
                AssetSerializationContext.Create(assets));
            plugins = new PluginEnvironment(
                assets,
                settings,
                engine.serialization,
                pluginsRoot,
                libraryRoot,
                scan,
                engine.modules.generations);
            if (plugins.discovery.diagnostics.Count != 0
                || plugins.activePlugins.Count != scan.candidates.Count)
            {
                throw new InvalidOperationException("Installed Plugin activation failed:" + Environment.NewLine
                    + string.Join(Environment.NewLine, plugins.discovery.diagnostics.Select(static diagnostic =>
                        $"{diagnostic.sourcePath}: {diagnostic.message}")));
            }
            var compiler = new ScriptCompiler(
                new ScriptCompilerOptions
                {
                    projectRootDirectory = projectRoot
                },
                assets,
                plugins);
            ActivateAuthoring(engine, plugins, compiler);
            engine.generations.Wait();
            assets.CompleteExtensionDiscovery();
            assets.Rescan();
            BuildPipeline pipeline = BuildComposition.CreatePipeline(engine, assets, plugins, settings, compiler, supportPackRoot);
            return new BuildWorkspace(engine, settings, assets, plugins, compiler, projectRoot, pipeline);
        }
        catch
        {
            plugins?.Dispose();
            assets?.Dispose();
            settings?.Dispose();
            engine?.Dispose();
            throw;
        }
    }

    private static IModuleSource CreateScriptModuleSource(ScriptModuleDeployment deployment)
        => new DotNetModuleSource
        {
            moduleName = deployment.moduleName,
            mainAssemblyPath = deployment.mainAssemblyPath,
            domain = deployment.domain,
            scope = deployment.scope,
            preloadAssemblyPaths = deployment.preloadAssemblyPaths,
            upstreamModuleNames = deployment.upstreamModuleNames,
            assemblyScopes = deployment.assemblyScopes,
            collectible = true
        };

    private static void ActivateAuthoring(
        EngineHost engine,
        PluginEnvironment plugins,
        ScriptCompiler compiler
    ) {
        // Asset importers and graph extensions belong to the authoring generation, including in a
        // headless build. Compiling only Player scripts would export unresolved/last-good asset state.
        ScriptCompilationResult result = compiler.CompileAuthoringGenerationAsync().GetAwaiter().GetResult();
        if (!result.success)
            throw new InvalidOperationException("Build authoring compilation failed:" + Environment.NewLine
                + string.Join(Environment.NewLine, result.diagnostics.Select(static diagnostic => diagnostic.message)));
        using Inno.Extensibility.Modules.AssemblyReloadSession reload = engine.modules.BeginReload(
            result.moduleDeployments.Select(CreateScriptModuleSource).ToArray());
        _ = engine.generations.Execute("Build authoring generation", reload, [plugins.CreateReloadChange()]);
    }

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public void Dispose()
    {
        m_plugins.Dispose();
        m_assets.Dispose();
        m_settings.Dispose();
        m_engine.Dispose();
    }

    internal BuildProfile LoadGameProfile(string? path)
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            string profilePath = Path.GetFullPath(path, m_projectDirectory);
            BuildProfile profile = new BuildProfileStore(profilePath, m_engine.serialization).Load();
            profile.applicationId = projectId.value;
            return profile;
        }

        BuildTargetId defaultTarget = OperatingSystem.IsWindows()
            ? BuildTargetId.windowsX64
            : BuildTargetId.macOSArm64;
        BuildSettings defaults = BuildSettings.CreateDefault(
            Path.GetFileName(Path.TrimEndingDirectorySeparator(m_projectDirectory)),
            FindDefaultStartupScene(),
            defaultTarget);
        var settings = new BuildSettingsStore(
            Path.Combine(m_projectDirectory, SettingsFileNames.build),
            m_engine.serialization,
            defaults);
        return settings.Load().CreateGameProfile(projectId);
    }

    private string FindDefaultStartupScene()
    {
        foreach (AssetFileEntry entry in m_assets.GetFileSystemEntries(includeDirectories: false)
                     .Where(static entry => entry.source == AssetSourceId.project)
                     .Where(static entry => !AssetSample.IsRuntimeExcluded(entry.assetPath, isDirectory: false))
                     .OrderBy(static entry => entry.assetPath.localPath, StringComparer.Ordinal))
        {
            if (m_assets.TryGetAssetType(entry.assetPath, out Type? type) && type == typeof(SceneAsset))
                return entry.assetPath.ToString();
        }
        return string.Empty;
    }
}

