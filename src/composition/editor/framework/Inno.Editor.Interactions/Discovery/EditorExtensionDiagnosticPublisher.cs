using System;
using System.Collections.Generic;
using System.Linq;

using Inno.Core.Diagnostics;
using Inno.Core.Identity;

namespace Inno.Editor.Interactions;

internal sealed class EditorExtensionDiagnosticPublisher : IdentityObject, IDisposable
{
    private const string C_DEPENDENCY_GROUP = "Extension Dependencies";
    private const string C_PANEL_ACTIVATION_GROUP = "Panel Activation";

    private readonly Dictionary<string, string> m_panelFailures = new(StringComparer.Ordinal);
    private string m_publishedState = string.Empty;
    private bool m_hasRejections;

    internal void ReportPanelFailure(
        string panelId,
        Exception exception
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(panelId);
        ArgumentNullException.ThrowIfNull(exception);
        m_panelFailures[panelId] = exception.ToString();
    }

    internal void PublishRejections(IReadOnlyList<Diagnostic> rejections)
    {
        if (rejections.Count == 0 && !m_hasRejections)
            return;
        Diagnostics.Set(identity.persistentId, C_DEPENDENCY_GROUP, rejections);
        m_hasRejections = rejections.Count != 0;
    }

    internal void ResolvePanel(string panelId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(panelId);
        m_panelFailures.Remove(panelId);
    }

    internal void RetainPanels(IReadOnlySet<string> panelIds)
    {
        ArgumentNullException.ThrowIfNull(panelIds);
        string[] removed = m_panelFailures.Keys
            .Where(id => !panelIds.Contains(id))
            .ToArray();
        for (int i = 0; i < removed.Length; i++)
            m_panelFailures.Remove(removed[i]);
    }

    internal void Commit()
    {
        if (m_panelFailures.Count == 0)
        {
            if (!string.IsNullOrEmpty(m_publishedState))
                Diagnostics.Clear(identity.persistentId, C_PANEL_ACTIVATION_GROUP);
            m_publishedState = string.Empty;
            return;
        }

        KeyValuePair<string, string>[] failures = m_panelFailures
            .OrderBy(static pair => pair.Key, StringComparer.Ordinal)
            .ToArray();
        string state = string.Join('\n', failures.Select(static pair => $"{pair.Key}:{pair.Value}"));
        if (string.Equals(m_publishedState, state, StringComparison.Ordinal))
            return;
        Diagnostics.Set(
            identity.persistentId,
            C_PANEL_ACTIVATION_GROUP,
            failures.Select(static pair => new Diagnostic(
                "EDITOR-PANEL",
                $"Panel '{pair.Key}' failed to attach: {pair.Value}",
                DiagnosticSeverity.Error,
                semanticId: pair.Key)));
        m_publishedState = state;
    }

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public void Dispose()
    {
        PublishRejections([]);
        m_panelFailures.Clear();
        if (!string.IsNullOrEmpty(m_publishedState))
            Diagnostics.Clear(identity.persistentId, C_PANEL_ACTIVATION_GROUP);
        m_publishedState = string.Empty;
    }
}
