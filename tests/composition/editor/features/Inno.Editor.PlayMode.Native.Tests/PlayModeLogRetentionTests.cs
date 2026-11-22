using System;
using System.Linq;
using Xunit;
using Inno.Core.Diagnostics;
using Inno.Core.Events;
using Inno.Core.Logging;
using Inno.Editor.Diagnostics;
using Inno.Extensibility.Modules;

namespace Inno.Editor.PlayMode.Native.Tests;

public sealed class PlayModeLogRetentionTests : IDisposable
{
    private readonly FakePlayMode m_playMode = new();
    private readonly DiagnosticHub m_diagnosticHub = new();
    private readonly LogRouter m_logRouter = new();
    private readonly IDisposable m_diagnosticScope;
    private readonly IDisposable m_logScope;
    private readonly EditorConsole m_console;

    public PlayModeLogRetentionTests()
    {
        m_logScope = m_logRouter.EnterScope();
        m_diagnosticScope = m_diagnosticHub.EnterScope();
        m_logRouter.SetMinimumLevel(LogLevel.Debug);
        m_console = new EditorConsole(m_logRouter, m_diagnosticHub, m_playMode);
        m_console.Start();
    }

    public void Dispose()
    {
        m_console.Dispose();
        m_diagnosticScope.Dispose();
        m_logScope.Dispose();
        m_logRouter.Dispose();
    }

    [Fact]
    public void ClearOnPlayRemovesOldLogsButRetainsCurrentDiagnostics()
    {
        Assert.True(m_console.clearOnPlay);
        Dispatch(LogLevel.Info, LogSessionID.none, "before-play");
        m_diagnosticHub.Set(
            new DiagnosticSource("test.compiler", "Test Compiler"),
            [Diagnostic.Error("TEST001", "compiler-diagnostic")]
        );
        m_logRouter.Flush();

        _ = m_playMode.Begin();

        EditorConsoleOccurrence[] entries = m_console.Capture().occurrences.ToArray();

        Assert.DoesNotContain(entries, static entry => entry.message == "before-play");
        Assert.Contains(entries, static entry => entry.message == "compiler-diagnostic");
    }

    [Fact]
    public void ManualClearRemovesHistoryButKeepsCurrentIssuesUntilResolved()
    {
        Dispatch(LogLevel.Error, LogSessionID.none, "old failure");
        using DiagnosticReporter reporter = m_diagnosticHub.CreateReporter(
            new DiagnosticSource("tests.current", "Current issue")
        );
        reporter.Publish(Diagnostic.Error("CURRENT", "still failing"));
        m_logRouter.Flush();

        m_console.Clear();

        Assert.Equal("still failing", Assert.Single(m_console.Capture().occurrences).message);
        reporter.Resolve("CURRENT");
        Assert.Empty(m_console.Capture().occurrences);
    }

    [Fact]
    public void CompletedPlaySessionRetainsEveryRuntimeSeverity()
    {
        LogSessionID sessionID = m_playMode.Begin();
        Dispatch(LogLevel.Debug, sessionID, "runtime-debug");
        Dispatch(LogLevel.Info, sessionID, "runtime-info");
        Dispatch(LogLevel.Warn, sessionID, "runtime-warning");
        Dispatch(LogLevel.Error, sessionID, "runtime-error");
        Dispatch(LogLevel.Fatal, sessionID, "runtime-fatal");
        m_playMode.SetState(EditorPlayModeState.Playing);
        m_playMode.SetState(EditorPlayModeState.Stopping);
        m_playMode.End();
        m_logRouter.Flush();

        EditorConsoleOccurrence[] entries = m_console.Capture().occurrences.ToArray();

        Assert.Contains(entries, static entry => entry.message == "runtime-debug");
        Assert.Contains(entries, static entry => entry.message == "runtime-info");
        Assert.Contains(entries, static entry => entry.message == "runtime-warning");
        Assert.Contains(entries, static entry => entry.message == "runtime-error");
        Assert.Contains(entries, static entry => entry.message == "runtime-fatal");
    }

    [Fact]
    public void FailedEntryRetainsPreparationDiagnosticsBecauseSimulationNeverStarted()
    {
        LogSessionID sessionID = m_playMode.Begin();
        Dispatch(LogLevel.Info, sessionID, "entry-diagnostic");
        m_playMode.SetState(EditorPlayModeState.Failed);
        m_logRouter.Flush();

        Assert.Contains(m_console.Capture().occurrences, static entry => entry.message == "entry-diagnostic");
    }

    [Fact]
    public void DisabledClearOnPlayRetainsExistingLogs()
    {
        m_console.clearOnPlay = false;
        Dispatch(LogLevel.Info, LogSessionID.none, "before-play");
        m_logRouter.Flush();

        _ = m_playMode.Begin();

        Assert.Contains(m_console.Capture().occurrences, static entry => entry.message == "before-play");
    }

    [Fact]
    public void CollapseGroupsEquivalentNonConsecutiveOccurrencesGlobally()
    {
        Dispatch(LogLevel.Info, LogSessionID.none, "repeated", file: "A.cs", line: 10);
        Dispatch(LogLevel.Warn, LogSessionID.none, "middle", file: "B.cs", line: 20);
        Dispatch(LogLevel.Info, LogSessionID.none, "repeated", file: "A.cs", line: 10);
        Dispatch(LogLevel.Info, LogSessionID.none, "repeated", file: "C.cs", line: 30);
        m_logRouter.Flush();

        EditorConsoleSnapshot snapshot = m_console.Capture();
        EditorConsoleGroup repeated = Assert.Single(
            snapshot.groups.Where(static group => group.latest.message == "repeated" && group.latest.file == "A.cs")
        );

        Assert.Equal(2, repeated.count);
        Assert.Equal(3, snapshot.groups.Count);
        Assert.True(repeated.occurrences[0].sequence < repeated.occurrences[1].sequence);
    }

    [Fact]
    public void CollapseDoesNotMergeEquivalentEntriesAcrossRuntimeSessions()
    {
        LogSessionID first = LogSessionID.Create();
        LogSessionID second = LogSessionID.Create();
        Dispatch(LogLevel.Info, first, "session-entry", file: "A.cs", line: 10);
        Dispatch(LogLevel.Info, second, "session-entry", file: "A.cs", line: 10);
        m_logRouter.Flush();

        EditorConsoleSnapshot snapshot = m_console.Capture();

        Assert.Equal(2, snapshot.groups.Count(static group => group.latest.message == "session-entry"));
    }

    [Fact]
    public void ConsoleTimelineAppendsTheNewestEntryAtTheBottom()
    {
        Dispatch(LogLevel.Info, LogSessionID.none, "first");
        Dispatch(LogLevel.Error, LogSessionID.none, "latest");
        m_logRouter.Flush();

        EditorConsoleSnapshot snapshot = m_console.Capture();

        Assert.Equal("first", snapshot.occurrences[0].message);
        Assert.Equal("latest", snapshot.occurrences[^1].message);
        Assert.Equal("first", snapshot.groups[0].latest.message);
        Assert.Equal("latest", snapshot.groups[^1].latest.message);
    }

    private void Dispatch(
        LogLevel level,
        LogSessionID sessionID,
        string message,
        string file = nameof(PlayModeLogRetentionTests),
        int line = 0
    ) => m_logRouter.Dispatch(
        new LogEntry(
            level,
            AssemblyDomain.InnoScripting,
            AssemblyScope.Runtime,
            "PlayModeLogRetentionTests",
            message,
            file,
            line,
            $"at {file}:{line}",
            sessionID
        )
    );

    private sealed class FakePlayMode : IEditorPlayMode
    {
        private readonly EventDispatcher m_stateDispatcher = new();
        private readonly EventHub m_stateEvents;

        internal FakePlayMode()
        {
            m_stateEvents = m_stateDispatcher.CreateHub();
        }

        public EditorPlayModeState state { get; private set; }

        public bool isPlaying => state == EditorPlayModeState.Playing;

        public string? lastFailure => null;

        public LogSessionID activeSessionID { get; private set; }

        public IDisposable ListenStateChanges(Action<EditorPlayModeState> listener)
            => m_stateEvents.Listen<StateNotification>(notification => listener(notification.state));

        public bool EnterPlayMode() => false;

        public bool ExitPlayMode() => false;

        internal LogSessionID Begin()
        {
            activeSessionID = LogSessionID.Create();
            SetState(EditorPlayModeState.Compiling);
            return activeSessionID;
        }

        internal void End()
        {
            SetState(EditorPlayModeState.Editing);
            activeSessionID = LogSessionID.none;
        }

        internal void SetState(EditorPlayModeState value)
        {
            state = value;
            m_stateDispatcher.Emit(new StateNotification(value));
        }

        private sealed class StateNotification(EditorPlayModeState state) : Event
        {
            internal EditorPlayModeState state { get; } = state;
        }
    }
}
