using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Adapter.Modules.DotNet;
using Inno.Benchmark.Runner;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;

namespace Inno.Benchmark.Reload;

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
        using AssemblyReloadSession reload = PrepareCandidate();
        reload.Activate();
        return Commit(reload);
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

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    internal IAssemblyUnloadProbe TimedActivation(out double elapsedMilliseconds)
    {
        using AssemblyReloadSession candidate = PrepareCandidate();
        long start = System.Diagnostics.Stopwatch.GetTimestamp();
        candidate.Activate();
        elapsedMilliseconds = System.Diagnostics.Stopwatch.GetElapsedTime(start).TotalMilliseconds;
        return Commit(candidate);
    }

    internal IAssemblyUnloadProbe UnloadCurrent() => modules.Unload(m_handle);

    private AssemblyReloadSession PrepareCandidate()
        => modules.BeginReload(m_handle, Request(m_isV2 ? "V1" : "V2", artifactRoot));

    private IAssemblyUnloadProbe Commit(AssemblyReloadSession reload)
    {
        AssemblyModuleHandle next = reload.context.module;
        IAssemblyUnloadProbe result = reload.Complete();
        m_handle = next;
        m_isV2 = !m_isV2;
        return result;
    }

    private static string ModuleDirectory()
        => Environment.GetEnvironmentVariable(C_MODULE_DIRECTORY_VARIABLE) is { Length: > 0 } configured
           ? configured
           : Path.Combine(AppContext.BaseDirectory, "Modules");
}
