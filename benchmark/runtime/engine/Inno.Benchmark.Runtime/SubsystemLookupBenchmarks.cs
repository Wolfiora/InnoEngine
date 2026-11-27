using System.Linq;
using BenchmarkDotNet.Attributes;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Runtime;
using Inno.Runtime.Contracts;

namespace Inno.Benchmark.Runtime;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class SubsystemLookupBenchmarks
{
    private EngineHost m_host = null!;
    private RuntimeSession m_session = null!;

    [Params(1, 16, 64)]
    public int factoryCount;

    [GlobalSetup]
    public void Setup()
    {
        m_host = new EngineHostBuilder().UseMetadataSources(
            new DotNetAssemblyCatalogSource(typeof(SubsystemLookupBenchmarks).Assembly),
            new ReflectionTypeCatalogSource(),
            new ReflectionSerializationMetadataSource()
        ).Build();
        m_session = m_host.CreateSession(new RuntimeSessionOptions
        {
            applicationID = "bench.lookup",
            kind = RuntimeSessionKind.Play,
            jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
            createSubsystems = _ => Enumerable.Range(0, factoryCount).Select(i => new Factory(i)).ToArray()
        });
    }

    [Benchmark]
    public IRuntimeSubsystem ResolveInterface() => m_session.subsystems.GetRequiredSubsystem<IMarker>();

    [GlobalCleanup]
    public void Cleanup()
    {
        m_session.Dispose();
        m_host.Dispose();
    }

    private interface IMarker : IRuntimeSubsystem;

    private sealed class Marker : RuntimeSubsystem, IMarker;

    private sealed class Filler : RuntimeSubsystem;

    private sealed class Factory(int index) : IRuntimeSubsystemFactory
    {
        public RuntimeSubsystemDescriptor descriptor { get; } = new(new RuntimeSubsystemID($"bench.lookup.{index}"));

        public IRuntimeSubsystem Create(RuntimeSubsystemContext context) => index == 0 ? new Marker() : new Filler();
    }
}
