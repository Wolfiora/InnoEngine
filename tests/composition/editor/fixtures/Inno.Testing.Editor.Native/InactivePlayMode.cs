using System;
using Inno.Core.Events;
using Inno.Core.Logging;
using Inno.Editor.PlayMode;

namespace Inno.Testing.Editor.Native;

/// <summary>
/// Reports a play mode that stays in editing for compositions that never enter play.
/// </summary>
public sealed class InactivePlayMode : IEditorPlayMode
{
    private readonly EventDispatcher m_events = new();

    public EditorPlayModeState state => EditorPlayModeState.Editing;
    public bool isPlaying => false;
    public string? lastFailure => null;
    public LogSessionID activeSessionID => LogSessionID.none;

    public IDisposable ListenStateChanges(Action<EditorPlayModeState> listener) => m_events.CreateHub();

    public bool EnterPlayMode() => false;

    public bool ExitPlayMode() => false;
}
