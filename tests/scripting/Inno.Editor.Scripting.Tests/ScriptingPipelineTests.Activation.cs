using System;
using System.Collections.Generic;
using System.Linq;

using Inno.Core.Diagnostics;
using Inno.Editor.Interactions;
using Inno.Editor.Scripting;
using Inno.Extensibility.Reload;
using Inno.Scripting.Compiler;
using Xunit;

namespace Inno.Editor.Scripting.Tests;

public sealed partial class ScriptingPipelineTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void EditorActivationFailureKeepsExactCandidateSourceAndRecoversWithoutRetainingFailedTypes(bool pluginException)
    {
        var diagnostics = new ActivationDiagnosticSink();
        m_fixture.host.diagnostics.RegisterSink(diagnostics);
        try
        {
            m_fixture.WriteVersionedBehavior(1);
            m_fixture.Rescan();
            using EditorInteractionRuntime runtime = m_fixture.CreateEditorRuntime();
            runtime.Start();
            IEditorScriptCompilation scripting = Assert.IsAssignableFrom<IEditorScriptCompilation>(ScriptingCompilationProbe.compilation);
            PumpEditorUntil(m_fixture, runtime, () => scripting.state == EditorScriptCompilationState.Ready,
                () => scripting.progress.status, timeoutMilliseconds: 120000);
            m_fixture.Write("BrokenPanel.editor.cs", BrokenPanelSource(fails: true, pluginException));
            m_fixture.Rescan();
            IScriptCompilationTicket failed = scripting.RequestCompilation();
            PumpEditorUntil(m_fixture, runtime, () => failed.isCompleted && scripting.state == EditorScriptCompilationState.Failed,
                () => scripting.progress.status, timeoutMilliseconds: 120000);
            Diagnostic activation = Assert.Single(diagnostics.current,
                static diagnostic => diagnostic.code == "INNO-EXTENSION-ACTIVATION");
            Assert.Contains("injected panel construction failure", activation.message, StringComparison.Ordinal);
            Assert.NotNull(activation.semanticId);
            Assert.True(activation.location.HasValue);
            Assert.EndsWith("BrokenPanel.editor.cs", activation.location.Value.sourcePath, StringComparison.Ordinal);
            Assert.True(activation.location.Value.line > 0);
            Assert.Equal(1, ReadLatestVersion(m_fixture));
            Assert.Equal(GenerationState.Ready, m_fixture.host.generations.state);
            ScriptCompilationResult candidate = Assert.IsType<ScriptCompilationResult>(failed.result);
            ScriptTypeSource declaration = Assert.Single(candidate.typeSources,
                static source => source.typeName == "BrokenPanel");
            Assert.Null(declaration.typeId);
            Assert.Equal(activation.location.Value.sourcePath, declaration.sourcePath);
            Assert.DoesNotContain(diagnostics.current, static diagnostic =>
                diagnostic.code == "INNO-CANDIDATE-REJECTED"
                && diagnostic.message.Contains("VersionedBehavior", StringComparison.Ordinal));

            m_fixture.Write("BrokenPanel.editor.cs", BrokenPanelSource(fails: false, pluginException));
            m_fixture.Rescan();
            IScriptCompilationTicket recovered = scripting.RequestCompilation();
            PumpEditorUntil(m_fixture, runtime, () => recovered.isCompleted && scripting.state == EditorScriptCompilationState.Ready,
                () => scripting.progress.status, timeoutMilliseconds: 120000);
            Assert.Equal(ScriptCompilationTicketState.Succeeded, recovered.state);
            Assert.Equal(GenerationState.Ready, m_fixture.host.generations.state);
            Assert.DoesNotContain(diagnostics.current, static diagnostic =>
                diagnostic.code is "INNO-EXTENSION-ACTIVATION" or "INNO-CANDIDATE-REJECTED" or "INNO-RELOAD");
            Assert.Equal(declaration.sourcePath, Assert.Single(candidate.typeSources,
                static source => source.typeName == "BrokenPanel").sourcePath);
        }
        finally
        {
            m_fixture.host.diagnostics.UnregisterSink(diagnostics);
        }
    }

    private static string BrokenPanelSource(
        bool fails,
        bool pluginException
    ) => $$"""
        using System;
        using InnoEditor.Core;
        public sealed class CandidateException : Exception
        {
            public CandidateException() : base("injected panel construction failure") { }
        }
        [EditorPanel("tests.source-failure", "Source failure")]
        public sealed class BrokenPanel : EditorPanel
        {
            public BrokenPanel() { {{(fails ? pluginException ? "throw new CandidateException();" : "throw new InvalidOperationException(\"injected panel construction failure\");" : string.Empty)}} }
            protected override void OnDraw(EditorContext context) { }
        }
        """;

    private sealed class ActivationDiagnosticSink : IDiagnosticSink
    {
        private readonly Dictionary<string, DiagnosticReport> m_reports = new(StringComparer.Ordinal);

        internal IEnumerable<Diagnostic> current => m_reports.Values.SelectMany(static report => report.diagnostics);

        public void Replace(DiagnosticReport report) => m_reports[report.source.id] = report;

        public void Clear(DiagnosticSource source) => m_reports.Remove(source.id);
    }
}
