using Inno.References;
using Inno.Runtime.Contracts;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Input;
using Inno.Core.Mathematics;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Runtime;

sealed partial class RenderRuntime
{
    private void PublishModelDiagnostic(string message)
    {
        PublishFrameIssue(new Diagnostic("RENDER_OUTPUT_MODEL_UNAVAILABLE", message,
            DiagnosticSeverity.Warning));
    }

    private void PublishFrameIssue(Diagnostic diagnostic)
    {
        var key = new FrameIssueKey(diagnostic.code, diagnostic.semanticId, diagnostic.objectId);
        m_currentFrameIssues[key] = diagnostic;
        m_diagnostics.Publish(diagnostic);
    }

    private void ReconcileFrameIssues()
    {
        foreach (FrameIssueKey key in m_previousFrameIssues.Keys)
        {
            if (!m_currentFrameIssues.ContainsKey(key))
                m_diagnostics.Resolve(key.code, key.semanticId, key.objectId);
        }
        m_previousFrameIssues.Clear();
        foreach ((FrameIssueKey key, Diagnostic diagnostic) in m_currentFrameIssues)
            m_previousFrameIssues.Add(key, diagnostic);
        m_currentFrameIssues.Clear();
    }

    private void PublishGraphDiagnostics(
        IReadOnlyList<RenderGraphDiagnostic> diagnostics,
        string source
    ) {
        foreach (RenderGraphDiagnostic diagnostic in diagnostics)
        {
            PublishFrameIssue(new Diagnostic(
                diagnostic.code,
                diagnostic.message,
                diagnostic.severity == DiagnosticSeverity.Error
                    ? DiagnosticSeverity.Error
                    : diagnostic.severity == DiagnosticSeverity.Warning
                        ? DiagnosticSeverity.Warning
                        : DiagnosticSeverity.Info,
                diagnostic.passName ?? diagnostic.resourceName ?? source));
        }
    }

    private void PublishContributorFailure(
        IRenderFrameGraphContributor contributor,
        string stage,
        Exception exception
    )
        => PublishFrameIssue(new Diagnostic(
            "RENDER_FRAME_CONTRIBUTOR_FAILED",
            $"Frame contributor '{contributor.GetType().Name}' failed during {stage}: {exception.Message}",
            DiagnosticSeverity.Error,
            contributor.GetType().FullName));

    private readonly record struct FrameIssueKey(
        string code,
        string? semanticId,
        Guid? objectId
    );

}
