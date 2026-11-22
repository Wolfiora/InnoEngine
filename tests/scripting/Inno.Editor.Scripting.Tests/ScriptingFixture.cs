using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Core.IO;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scripting.Compiler;
using Inno.Scripting.Reload;

namespace Inno.Editor.Scripting.Tests;

internal sealed class ScriptingFixture : IDisposable
{
    private readonly IdentityAllocator m_identities = new();
    private readonly IDisposable m_diagnosticScope;
    private readonly IDisposable m_identityScope;
    private readonly ProjectSettingsStore m_settings;
    private readonly PluginEnvironment m_plugins;
    private RuntimeSession? m_editorSession;
    private bool m_disposed;

    internal ScriptingFixture(
        Action<string, SerializationRegistry>? configureProject = null,
        bool enableFileSystemWatcher = false,
        int fileWatcherFlushDelayMs = 100
    ) {
        this.projectRoot = Path.Combine(Path.GetTempPath(), "InnoScriptingPipelineTests", Guid.NewGuid().ToString("N"));
        string assetRoot = Path.Combine(projectRoot, "Assets");
        string pluginRoot = Path.Combine(projectRoot, "Plugins");
        string libraryRoot = Path.Combine(projectRoot, "Library");
        Directory.CreateDirectory(assetRoot);
        Directory.CreateDirectory(pluginRoot);
        Directory.CreateDirectory(libraryRoot);
        m_identityScope = m_identities.EnterScope();
        this.host = new EngineHostBuilder().UseMetadataSources(
            new DotNetAssemblyCatalogSource(typeof(ScriptingPipelineTests).Assembly),
            new ReflectionTypeCatalogSource(),
            new ReflectionSerializationMetadataSource()
        ).Build();
        m_diagnosticScope = host.diagnostics.EnterScope();
        configureProject?.Invoke(projectRoot, host.serialization);
        var pluginSources = new PluginSourceService(host.serialization, pluginRoot, libraryRoot);
        PluginScanResult scan = pluginSources.Scan();
        AssetPipelineOptions options = AssetPipelineOptions.Create(assetRoot, libraryRoot);
        this.assets = new AssetPipeline(
            host.modules,
            host.types,
            host.serialization,
            m_identities,
            host.diagnostics,
            host.logs,
            options with
            {
                enableFileSystemWatcher = enableFileSystemWatcher,
                fileWatcherFlushDelayMs = fileWatcherFlushDelayMs,
                sourceMounts = [..options.sourceMounts!, ..PluginSourceService.GetActivatableMounts(scan)]
            }
        );
        m_settings = new ProjectSettingsStore(
            new FileByteDocumentStore(Path.GetFullPath(Path.Combine(projectRoot, "Settings.Project.inno"))),
            host.types,
            host.serialization,
            new ProjectID("tests.scripting"),
            AssetSerializationContext.Create(assets)
        );
        m_plugins = new PluginEnvironment(assets, m_settings, host.serialization, pluginRoot, libraryRoot, scan, host.modules.generations);
        string dotnetHost = Path.Combine(
            Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"
        );
        this.compiler = new ScriptCompiler(
            new ScriptCompilerOptions
            {
                projectRootDirectory = projectRoot,
                compilationEnvironment = new SdkScriptCompilationEnvironment(dotnetHost, projectRoot)
            },
            assets,
            m_plugins
        );
    }

    public void Dispose()
    {
        if (m_disposed)
        {
            return;
        }
        m_disposed = true;
        m_editorSession?.Dispose();
        m_plugins.Dispose();
        assets.Dispose();
        m_settings.Dispose();
        m_diagnosticScope.Dispose();
        host.Dispose();
        m_identityScope.Dispose();
        if (Directory.Exists(projectRoot))
        {
            Directory.Delete(projectRoot, recursive: true);
        }
    }

    internal string projectRoot { get; }
    internal EngineHost host { get; }
    internal AssetPipeline assets { get; }
    internal ScriptCompiler compiler { get; }
    internal IReadOnlyList<PluginCandidate> activePlugins => m_plugins.activePlugins;

    internal RuntimeSession editorSession
        => m_editorSession ?? throw new InvalidOperationException("The Editor runtime session has not been created.");

    internal void Write(
        string relativePath,
        string source
    ) {
        string path = Path.Combine(projectRoot, "Assets", relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, source);
    }

    internal void Move(
        string sourceRelativePath,
        string destinationRelativePath
    ) {
        string source = Path.Combine(projectRoot, "Assets", sourceRelativePath);
        string destination = Path.Combine(projectRoot, "Assets", destinationRelativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        File.Move(source, destination);
        string sourceMetadata = source + ".imeta";
        if (File.Exists(sourceMetadata))
        {
            File.Move(sourceMetadata, destination + ".imeta");
        }
    }

    internal void WriteVersionedBehavior(int version)
        => Write("VersionedBehavior.cs", $$"""
            using InnoEngine.Scene;

            public sealed class VersionedBehavior : GameBehavior
            {
                public int version => {{version}};
            }
            """);

    internal void Rescan() => assets.Rescan();

    internal ScriptCompilationResult Compile(
        IProgress<ScriptCompilationProgress>? progress = null,
        CancellationToken cancellationToken = default
    ) {
        Rescan();
        return compiler.CompileAuthoringGenerationAsync(progress, cancellationToken).GetAwaiter().GetResult();
    }

    internal ScriptCompilationResult CompileRuntimeDeployment(
        string? targetRuntimeDirectory = null,
        IProgress<ScriptCompilationProgress>? progress = null,
        CancellationToken cancellationToken = default
    ) {
        Rescan();
        return compiler
            .CompileRuntimeDeploymentAsync(targetRuntimeDirectory ?? AppContext.BaseDirectory, progress, cancellationToken)
            .GetAwaiter()
            .GetResult();
    }

    internal string CreateDeploymentRuntime()
    {
        string directory = Path.Combine(projectRoot, "TargetRuntime");
        Directory.CreateDirectory(directory);
        foreach (string source in Directory.EnumerateFiles(AppContext.BaseDirectory, "Inno.*.dll", SearchOption.TopDirectoryOnly))
        {
            File.Copy(source, Path.Combine(directory, Path.GetFileName(source)), overwrite: true);
        }

        return directory;
    }

    internal ScriptReloadHost CreateReloadHost(
        EditorReloadCoordinator? reloads = null,
        bool autoCompile = false
    ) => new(
        new ScriptReloadOptions
        {
            autoCompile = autoCompile,
            debounceMilliseconds = 0,
            compilationWarningTimeout = Timeout.InfiniteTimeSpan
        },
        compiler,
        assets,
        m_plugins,
        host.modules,
        m_settings,
        reloads ?? new EditorReloadCoordinator(),
        CreateScriptModuleSource
    );

    internal ScriptCompilationResult CompilePending(ScriptReloadHost reload)
    {
        Rescan();
        reload.RecompileScripting();
        Assert.True(reload.TryCompilePending(out Task<ScriptCompilationResult>? compilation));
        return Assert.IsAssignableFrom<Task<ScriptCompilationResult>>(compilation).GetAwaiter().GetResult();
    }

    internal ScriptCompilationResult CompilePendingPluginReload(ScriptReloadHost reload)
    {
        reload.ReloadPlugins();
        Assert.True(reload.TryCompilePending(out Task<ScriptCompilationResult>? compilation));
        return Assert.IsAssignableFrom<Task<ScriptCompilationResult>>(compilation).GetAwaiter().GetResult();
    }

    internal bool RefreshPlugins() => m_plugins.Refresh();

    internal Type ResolveActiveType(string name)
    {
        TypeCacheSnapshot snapshot = host.types.current;
        return snapshot.types
            .Select(typeRef => typeRef.Resolve(snapshot))
            .Single(type => string.Equals(type.Name, name, StringComparison.Ordinal));
    }

    internal EditorInteractionRuntime CreateEditorRuntime(EditorReloadCoordinator? reloads = null)
    {
        ScriptingCompilationProbe.Reset();
        SceneReloadProbe.Reset();
        reloads ??= new EditorReloadCoordinator();
        RuntimeSession editorSession = CreateEditorSession();
        return new EditorInteractionRuntime(
            new EditorContext(projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super")),
            host.types,
            host.logs,
            [
                host.types,
                host.serialization,
                m_identities,
                editorSession.identities,
                host.modules,
                assets,
                m_plugins,
                m_settings,
                compiler,
                new Func<ScriptModuleDeployment, IModuleSource>(CreateScriptModuleSource),
                editorSession,
                reloads
            ]
        );
    }

    internal RuntimeSession CreateEditorSession()
    {
        m_editorSession ??= host.CreateSession(
            new RuntimeSessionOptions
            {
                kind = RuntimeSessionKind.Edit,
                applicationID = "tests.editor",
                createLogSink = _ => new FileLogSink(Path.Combine(Path.Combine(projectRoot, "Persistent", "tests.editor"), "Logs")),
                referenceResolvers = [assets]
            }
        );
        return m_editorSession;
    }

    private IModuleSource CreateScriptModuleSource(ScriptModuleDeployment deployment)
        => new DotNetModuleSource
    {
        artifactRootDirectory = Path.Combine(projectRoot, "Library", "Assemblies"),
        moduleName = deployment.moduleName,
        mainAssemblyPath = deployment.mainAssemblyPath,
        domain = deployment.domain,
        scope = deployment.scope,
        preloadAssemblyPaths = deployment.preloadAssemblyPaths,
        upstreamModuleNames = deployment.upstreamModuleNames,
        assemblyScopes = deployment.assemblyScopes,
        collectible = true
    };
}
