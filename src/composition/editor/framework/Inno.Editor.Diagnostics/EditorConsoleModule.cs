using Inno.Core.Diagnostics;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.PlayMode;

namespace Inno.Editor.Diagnostics;

[EditorModule("editor-console", order: int.MinValue)]
internal sealed class EditorConsoleModule : EditorModule, IEditorConsole
{
    private readonly EditorConsole m_console;

    internal EditorConsoleModule(
        LogRouter logs,
        DiagnosticHub diagnostics,
        IEditorPlayMode playMode
    ) {
        m_console = new EditorConsole(logs, diagnostics, playMode);
    }

    /// <inheritdoc />
    public int capacity { get => m_console.capacity; set => m_console.capacity = value; }

    /// <inheritdoc />
    public bool clearOnPlay { get => m_console.clearOnPlay; set => m_console.clearOnPlay = value; }

    /// <inheritdoc />
    public EditorConsoleSnapshot Capture() => m_console.Capture();

    /// <inheritdoc />
    public void Clear() => m_console.Clear();

    /// <inheritdoc />
    protected override void OnStart(EditorContext context) => m_console.Start();

    /// <inheritdoc />
    protected override void OnStop(EditorContext context) => m_console.Dispose();

    /// <inheritdoc />
    protected override void OnDispose() => m_console.Dispose();
}
