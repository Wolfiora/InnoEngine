using System;
using Inno.Editor.Core;
using Inno.Editor.Settings;
using Inno.Platform;

namespace Inno.Editor.Workbench;

[EditorModule("editor-frame-pacing", order: 20)]
internal sealed class EditorFramePacingModule(
    EditorSettings settings,
    FramePacingOptions pacing
) : EditorModule
{
    private IDisposable? m_settingsSubscription;

    /// <summary>
    /// Initializes this feature when its owning runtime becomes active.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnStart(EditorContext context)
    {
        Apply(settings);
        m_settingsSubscription = settings.ListenChanges(Apply);
    }

    /// <summary>
    /// Stops this feature before its owning runtime releases the active generation.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnStop(EditorContext context) => m_settingsSubscription?.Dispose();

    private void Apply(EditorSettings current)
    {
        pacing.verticalSync = current.Get("Editor/Rendering/Vertical Sync").GetAsBoolean("enabled", false);
        pacing.maximumFrameRate = Math.Clamp(current.Get("Editor/Rendering/Maximum Frame Rate").GetAsInt32("value", 0), 0, 1000);
    }
}
