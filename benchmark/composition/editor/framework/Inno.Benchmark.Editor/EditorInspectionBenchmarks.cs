using System;
using BenchmarkDotNet.Attributes;
using Inno.Benchmark.Services;
using Inno.Editor.Core;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;

namespace Inno.Benchmark.Editor;

/// <summary>
/// Measures inspection drawer registry construction and conditional drawer resolution.
/// </summary>
/// <remarks>
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
public class EditorInspectionBenchmarks
{
    private readonly BenchInspectionTarget[] m_targets =
    [
        new BenchInspectionTarget(1),
        new BenchInspectionTarget(2),
        new BenchInspectionTarget(1),
        new BenchInspectionTarget(2)
    ];

    private EditorHarness m_harness = null!;
    private PropertyDrawerRegistry m_properties = null!;
    private InspectorAttributeDrawerRegistry m_attributes = null!;
    private SerializedPropertyRenderer m_renderer = null!;
    private InspectionDrawerRegistry m_inspectors = null!;

    [GlobalSetup]
    public void Setup()
    {
        m_harness = new EditorHarness();
        BenchmarkServices services = m_harness.services;
        m_properties = new PropertyDrawerRegistry(m_harness.runtime.interactions, services.types, services.serialization, []);
        m_attributes = new InspectorAttributeDrawerRegistry(m_harness.runtime.interactions, services.types, services.serialization, []);
        m_renderer = new SerializedPropertyRenderer(
            m_properties,
            m_attributes,
            m_harness.runtime.interactions,
            new NoPropertyEdits(),
            services.logs
        );
        m_inspectors = CreateInspectors();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        m_inspectors.Dispose();
        m_attributes.Dispose();
        m_properties.Dispose();
        m_harness.Dispose();
    }

    [Benchmark(Description = "Inspection: create drawer registry (cold)")]
    public int CreateRegistry()
    {
        using InspectionDrawerRegistry registry = CreateInspectors();
        return registry.GetHashCode();
    }

    [Benchmark(Description = "Inspection: resolve conditional drawer x1000 (warm)")]
    public int Resolve()
    {
        int resolved = 0;
        for (int index = 0; index < 1000; index++)
        {
            resolved += m_inspectors.TryResolve(m_harness.context, m_targets[index & 3], m_renderer, out _, out _) ? 1 : 0;
        }
        return resolved;
    }

    private InspectionDrawerRegistry CreateInspectors()
        => new(
            m_harness.runtime.interactions,
            static type => (IInspectionDrawer)Activator.CreateInstance(type, nonPublic: true)!,
            m_harness.services.types,
            m_harness.services.serialization
        );

    private sealed class NoPropertyEdits : IInspectionPropertyEditService
    {
        public bool ChangeProperty(
            object owner,
            string propertyName,
            Action mutation,
            string historyName,
            string? mergeKey,
            string? componentName
        ) => throw new InvalidOperationException("Resolution must not edit a property.");
    }
}

public sealed record BenchInspectionTarget(int kind);
