using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Adapter.Modules.DotNet;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;

namespace Inno.Benchmark.Engine;

/// <summary>
/// Reloads the collectible extensibility test fixture modules through the public module host.
/// </summary>
public sealed class ReloadFixture : IDisposable
{
    public const string C_MODULE_DIRECTORY_VARIABLE = "INNO_BENCHMARK_MODULES";

    private AssemblyModuleHandle m_handle;
    private bool m_isV2;

    public ReloadFixture()
    {
        this.artifactRoot = BenchmarkPaths.CreateRoot("Reload");
        this.modules = new ModuleHost(
            new ModuleHostOptions { catalogSource = new DotNetAssemblyCatalogSource(typeof(ReloadFixture).Assembly) }
        );
        this.types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        m_handle = modules.Load(Request("V1", artifactRoot));
    }

    public string artifactRoot { get; }
    public ModuleHost modules { get; }
    public TypeCatalog types { get; }

    /// <summary>
    /// Publishes the other fixture version and returns the previous generation's unload probe.
    /// </summary>
    public IAssemblyUnloadProbe HotReload()
    {
        m_isV2 = !m_isV2;
        using AssemblyReloadSession reload = modules.BeginReload(m_handle, Request(m_isV2 ? "V2" : "V1", artifactRoot));
        reload.Activate();
        m_handle = reload.context.module;
        return reload.Complete();
    }

    public void Dispose()
    {
        types.Dispose();
        modules.Dispose();
        BenchmarkPaths.Delete(artifactRoot);
    }

    public static int WaitForRetirement(IAssemblyUnloadProbe probe)
    {
        int collections = 0;
        while (!probe.isCompleted && collections < 200)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            collections++;
        }
        if (!probe.isCompleted)
        {
            throw new InvalidOperationException("Previous generation was not retired: " + probe.description);
        }
        return collections;
    }

    public static DotNetModuleSource Request(
        string variant,
        string artifactRoot
    ) {
        string directory = Path.Combine(ModuleDirectory(), variant);
        return new DotNetModuleSource
        {
            artifactRootDirectory = artifactRoot,
            moduleName = "RetirementTests",
            mainAssemblyPath = Path.Combine(directory, "Inno.Extensibility.Modules.TestModule.dll"),
            preloadAssemblyPaths = [Path.Combine(directory, "Reloadable.PrivateDependency.dll")],
            domain = AssemblyDomain.InnoPlugin,
            scope = AssemblyScope.Runtime
        };
    }

    private static string ModuleDirectory()
        => Environment.GetEnvironmentVariable(C_MODULE_DIRECTORY_VARIABLE) is { Length: > 0 } configured
           ? configured
           : Path.Combine(AppContext.BaseDirectory, "Modules");
}
