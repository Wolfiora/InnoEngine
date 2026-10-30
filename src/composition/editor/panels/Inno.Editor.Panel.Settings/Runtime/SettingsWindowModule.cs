using System;
using Inno.Build;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Settings;

namespace Inno.Editor.Panel.Settings;

[EditorModule("settings-window", order: 20)]
internal sealed class SettingsWindowModule(
    EditorSettings editorSettings,
    ProjectSettingsEditor projectSettings,
    BuildPipeline buildPipeline,
    BuildSettingsStore buildSettings,
    EditorInteractions interactions,
    EditorReloadCoordinator reloads
) : EditorModule, IEditorReloadGuard
{
    private const string C_PRESENTATION_FAILURE = "Settings presentation failed. Reload extensions or restart the Editor.";

    private SettingsEditSession? m_session;
    private IDisposable? m_reloadGuard;
    private long m_failedCatalogRevision = long.MinValue;

    internal bool isVisible { get; private set; }

    internal SettingsEditSession? session => m_session;
    internal long sessionRevision { get; private set; }
    internal bool canOpen => m_failedCatalogRevision != editorSettings.catalogRevision;
    internal string? unavailableReason => canOpen ? null : C_PRESENTATION_FAILURE;

    internal void Open()
    {
        if (!canOpen)
            throw new InvalidOperationException(C_PRESENTATION_FAILURE);
        if (!isVisible)
        {
            m_session = new SettingsEditSession(
                editorSettings,
                projectSettings,
                buildPipeline,
                buildSettings,
                interactions.history);
            sessionRevision++;
        }
        isVisible = true;
    }

    internal void Close()
    {
        m_session?.Dispose();
        isVisible = false;
        m_session = null;
        sessionRevision++;
    }

    internal void Refresh()
    {
        m_session?.Dispose();
        m_session = new SettingsEditSession(
            editorSettings,
            projectSettings,
            buildPipeline,
            buildSettings,
            interactions.history);
        sessionRevision++;
    }

    internal void FailPresentation()
    {
        m_failedCatalogRevision = editorSettings.catalogRevision;
        Close();
    }

    /// <summary>
    /// Registers the admission boundary for generation-bound Settings editing.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnStart(EditorContext context) => m_reloadGuard = reloads.RegisterGuard(this);

    /// <inheritdoc />
    protected override void OnStop(EditorContext context)
    {
        Close();
        m_reloadGuard?.Dispose();
        m_reloadGuard = null;
    }

    bool IEditorReloadGuard.canRequestReload => !isVisible;
    bool IEditorReloadGuard.canActivateReload => !isVisible;
    string? IEditorReloadGuard.blockingReason
        => isVisible ? "Close Settings before reloading scripts to retire its current-generation editing state." : null;
}
