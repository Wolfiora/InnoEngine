using System;
using Inno.Editor.Core;
using Inno.Editor.Diagnostics;
using Inno.Editor.Settings;

namespace Inno.Editor.Panel.Logging;
[EditorModule("diagnostics-logging", order: int.MinValue)]
internal sealed class LoggingModule : EditorModule
{
    private IDisposable? m_settingsSubscription;
    private readonly IEditorConsole m_console;
    private readonly EditorSettings m_settings;
    internal LoggingModule(
        IEditorConsole console,
        EditorSettings settings
    ) {
        m_settings = settings ?? throw new ArgumentNullException(nameof(settings));
        m_console = console ?? throw new ArgumentNullException(nameof(console));
    }

    /// <summary>
    /// Initializes this feature after its owning runtime has activated all required services.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void OnStart(EditorContext context)
    {
        ApplySettings(m_settings);
        m_settingsSubscription = m_settings.ListenChanges(ApplySettings);
    }

    /// <summary>
    /// Stops this feature before its owning runtime releases generation-scoped services.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void OnStop(EditorContext context)
    {
        m_settingsSubscription?.Dispose();
        m_settingsSubscription = null;
    }

    /// <summary>
    /// Releases resources retained by this feature after it has stopped.
    /// </summary>
    protected override void OnDispose()
    {
        m_settingsSubscription?.Dispose();
        m_settingsSubscription = null;
    }

    private void ApplySettings(EditorSettings settings) => m_console.clearOnPlay = ClearConsoleOnPlaySetting.Read(settings);
}
