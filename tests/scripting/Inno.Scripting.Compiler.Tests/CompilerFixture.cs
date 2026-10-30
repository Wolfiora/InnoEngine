using System;
using System.IO;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Identity;
using Inno.Core.IO;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scene;

namespace Inno.Scripting.Compiler.Tests;

internal sealed class CompilerFixture : IDisposable
{
    private readonly IdentityAllocator m_identities = new();
    private readonly IDisposable m_identityScope;
    private readonly IDisposable m_diagnosticScope;
    private readonly EngineHost m_host;
    private readonly AssetPipeline m_assets;
    private readonly ProjectSettingsStore m_settings;
    private readonly PluginEnvironment m_plugins;

    internal CompilerFixture(
        Action<string, SerializationRegistry>? configure = null,
        IScriptCompilationEnvironmentProvider? compilationEnvironment = null
    )
    {
        projectRoot = Path.Combine(Path.GetTempPath(), "InnoCompilerTests", Guid.NewGuid().ToString("N"));
        string assetsRoot = Path.Combine(projectRoot, "Assets");
        string pluginsRoot = Path.Combine(projectRoot, "Plugins");
        string libraryRoot = Path.Combine(projectRoot, "Library");
        Directory.CreateDirectory(assetsRoot);
        Directory.CreateDirectory(pluginsRoot);
        Directory.CreateDirectory(libraryRoot);
        m_identityScope = m_identities.EnterScope();
        m_host = new EngineHostBuilder().UseMetadataSources(
            new DotNetAssemblyCatalogSource(typeof(CompilerFixture).Assembly,
                typeof(GameBehavior).Assembly, typeof(ScriptCompiler).Assembly),
            new ReflectionTypeCatalogSource(), new ReflectionSerializationMetadataSource()).Build();
        m_diagnosticScope = m_host.diagnostics.EnterScope();
        configure?.Invoke(projectRoot, m_host.serialization);
        var sources = new PluginSourceService(m_host.serialization, pluginsRoot, libraryRoot);
        PluginScanResult scan = sources.Scan();
        AssetPipelineOptions options = AssetPipelineOptions.Create(assetsRoot, libraryRoot);
        m_assets = new AssetPipeline(m_host.modules, m_host.types, m_host.serialization,
            m_identities, m_host.diagnostics, m_host.logs, options with
            {
                enableFileSystemWatcher = false,
                sourceMounts = [.. options.sourceMounts!, .. PluginSourceService.GetActivatableMounts(scan)]
            });
        m_settings = new ProjectSettingsStore(
            new FileByteDocumentStore(Path.Combine(projectRoot, "Settings.Project.inno")),
            m_host.types, m_host.serialization, new ProjectId("tests.compiler"),
            AssetSerializationContext.Create(m_assets));
        m_plugins = new PluginEnvironment(m_assets, m_settings, m_host.serialization,
            pluginsRoot, libraryRoot, scan, m_host.modules.generations);
        string dotnetHost = Path.Combine(
            Path.GetFullPath("../../..", System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        compiler = new ScriptCompiler(
            new ScriptCompilerOptions
            {
                projectRootDirectory = projectRoot,
                compilationEnvironment = compilationEnvironment ?? new Inno.Build.SdkScriptCompilationEnvironment(dotnetHost, projectRoot)
            }, m_assets, m_plugins);
    }

    internal string projectRoot { get; }

    internal ScriptCompiler compiler { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        m_plugins.Dispose();
        m_assets.Dispose();
        m_settings.Dispose();
        m_diagnosticScope.Dispose();
        m_host.Dispose();
        m_identityScope.Dispose();
        Directory.Delete(projectRoot, recursive: true);
    }

    internal void Write(
        string relativePath,
        string source
    ) => File.WriteAllText(Path.Combine(projectRoot, "Assets", relativePath), source);

    internal void Rescan() => m_assets.Rescan();

    internal ScriptCompilationResult Compile()
    {
        Rescan();
        return compiler.CompileAuthoringGenerationAsync().GetAwaiter().GetResult();
    }
}
