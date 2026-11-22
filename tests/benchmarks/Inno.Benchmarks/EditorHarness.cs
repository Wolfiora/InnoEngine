using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Core.Input;
using Inno.Editor.Core;
using Inno.Editor.Graph;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;

namespace Inno.Benchmarks;

/// <summary>
/// Starts the backend-neutral editor interaction runtime without native UI.
/// </summary>
public sealed class EditorHarness : IDisposable
{
    private readonly EditorReloadCoordinator m_reloads = new();
    private readonly GraphModuleSink m_sink = new();

    public EditorHarness()
    {
        this.services = new BenchmarkServices();
        this.root = BenchmarkPaths.CreateRoot("Editor");
        this.context = new EditorContext(root, new EditorKeyboardPolicy(KeyModifier.Control, "Super"));
        this.runtime = new EditorInteractionRuntime(
            context,
            services.types,
            services.logs,
            [services.types, services.serialization, m_sink, m_reloads]
        );
        runtime.Start();
        this.graphs = m_sink.module ?? throw new InvalidOperationException("Graph editor module did not start.");
    }

    public BenchmarkServices services { get; }
    public string root { get; }
    public EditorContext context { get; }
    public EditorInteractionRuntime runtime { get; }
    public GraphEditorModule graphs { get; }

    public void Dispose()
    {
        runtime.Dispose();
        services.Dispose();
        BenchmarkPaths.Delete(root);
    }

    public sealed class GraphModuleSink
    {
        internal GraphEditorModule? module { get; set; }
    }

    [EditorModule("bench.graph.module-probe")]
    private sealed class GraphModuleProbe : EditorModule
    {
        private readonly GraphEditorModule m_graphs;
        private readonly GraphModuleSink m_sink;

        private GraphModuleProbe(
            GraphEditorModule graphs,
            GraphModuleSink sink
        ) {
            m_graphs = graphs;
            m_sink = sink;
        }

        protected override void OnStart(EditorContext context)
        {
            _ = context;
            m_sink.module = m_graphs;
        }
    }
}
