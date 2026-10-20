using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Inno.Extensibility.Modules;
using Xunit;

namespace Inno.Runtime.Tests;

public sealed class AssemblyClassificationTests
{
    [Fact]
    public async Task IndependentModuleHostsCanPublishTheSameStaticAssemblyConcurrently()
    {
        // A separate linked instance keeps the test host's own classification unchanged.
        Assembly assembly = Assembly.Load(File.ReadAllBytes(typeof(AssemblyClassificationTests).Assembly.Location));
        var source = new StaticModuleSource("Shared", AssemblyDomain.InnoScripting,
            AssemblyScope.Runtime, [assembly], []);
        const int workerCount = 8;
        using var ready = new Barrier(workerCount);
        Task[] workers = Enumerable.Range(0, workerCount).Select(workerIndex => Task.Factory.StartNew(
            () =>
            {
                using var modules = new ModuleHost(new ModuleHostOptions
                {
                    catalogSource = new StaticAssemblyCatalogSource([], [])
                });
                for (int iteration = 0; iteration < 128; iteration++)
                {
                    Assert.True(ready.SignalAndWait(TimeSpan.FromSeconds(10)));
                    AssemblyModuleHandle handle = modules.Load(source);
                    Assert.True(assembly.TryGetInnoAssemblyClassification(out AssemblyDomain domain, out AssemblyScope scope));
                    Assert.Equal(AssemblyDomain.InnoScripting, domain);
                    Assert.Equal(AssemblyScope.Runtime, scope);
                    _ = modules.Unload(handle);
                    modules.generations.Wait();
                }
            },
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default)).ToArray();
        await Task.WhenAll(workers).WaitAsync(TimeSpan.FromSeconds(30));
    }
}
