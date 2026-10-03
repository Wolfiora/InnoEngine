using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Xunit;

namespace Inno.Extensibility.Modules.Tests;

public sealed class HostLoadContextTests
{
    [Fact]
    public void IsolatedHostDiscoversItsOwnTypesAndReleasesItsContext()
    {
        string cache = Path.Combine(Path.GetTempPath(), "InnoIsolatedHostTests", Guid.NewGuid().ToString("N"));
        try
        {
            WeakReference context = DiscoverInIsolatedContext(cache);
            // Loader allocator finalization can require additional full collection cycles.
            for (int attempt = 0; attempt < 8 && context.IsAlive; attempt++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
                GC.WaitForPendingFinalizers();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
            }
            Assert.False(context.IsAlive);
        }
        finally
        {
            if (Directory.Exists(cache))
                Directory.Delete(cache, recursive: true);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference DiscoverInIsolatedContext(string cache)
    {
        var context = new IsolatedHostContext();
        var monitor = new WeakReference(context);
        try
        {
            Assembly modules = context.LoadFromAssemblyPath(typeof(ModuleHost).Assembly.Location);
            Assembly types = context.LoadFromAssemblyPath(typeof(TypeCatalog).Assembly.Location);
            Type optionsType = modules.GetType(typeof(ModuleHostOptions).FullName!, throwOnError: true)!;
            object options = Activator.CreateInstance(optionsType)!;
            optionsType.GetProperty(nameof(ModuleHostOptions.cacheDirectory))!.SetValue(options, cache);
            Type hostType = modules.GetType(typeof(ModuleHost).FullName!, throwOnError: true)!;
            using var host = (IDisposable)Activator.CreateInstance(hostType, options)!;
            Type catalogType = types.GetType(typeof(TypeCatalog).FullName!, throwOnError: true)!;
            using var catalog = (IDisposable)Activator.CreateInstance(catalogType, host)!;
            object snapshot = catalogType.GetProperty(nameof(TypeCatalog.current))!.GetValue(catalog)!;
            MethodInfo query = snapshot.GetType().GetMethod(nameof(TypeCacheSnapshot.TryGetTypeRef))!;
            Assert.True((bool)query.Invoke(snapshot, [hostType, null])!);
            Assert.False((bool)query.Invoke(snapshot, [typeof(ModuleHost), null])!);
        }
        finally
        {
            context.Unload();
        }
        return monitor;
    }

    private sealed class IsolatedHostContext() : AssemblyLoadContext("Inno isolated host", isCollectible: true)
    {
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            if (!(assemblyName.Name ?? string.Empty).StartsWith("Inno.", StringComparison.Ordinal))
                return null;
            string path = Path.Combine(AppContext.BaseDirectory, assemblyName.Name + ".dll");
            return File.Exists(path) ? LoadFromAssemblyPath(path) : null;
        }
    }
}
