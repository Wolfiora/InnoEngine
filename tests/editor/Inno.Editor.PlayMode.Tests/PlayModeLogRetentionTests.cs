using System;
using System.Linq;
using System.IO;
using Inno.Adapter.Modules.DotNet;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Extensibility.Types;

using Inno.Extensibility.Modules;
using Inno.Core.Diagnostics;
using Inno.Core.Logging;
using Inno.Editor.Diagnostics;

using Xunit;

namespace Inno.Editor.PlayMode.Tests;

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
    public void DiagnosticServiceActivatesWithoutLoggingPanelSettingsAndRetiresItsSubscription()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoConsoleOwner", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            using var logs = new LogRouter();
            var diagnostics = new DiagnosticHub();
            var playMode = new FakePlayMode();
            using var modules = new ModuleHost(new ModuleHostOptions
            {
                catalogSource = new DotNetAssemblyCatalogSource(typeof(PlayModeLogRetentionTests).Assembly)
            });
            using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
            for (int cycle = 0; cycle < 100; cycle++)
            {
                using var runtime = new EditorInteractionRuntime(
                    new EditorContext(root, new EditorKeyboardPolicy(Inno.Core.Input.KeyModifier.Control, "Super")),
                    types, logs, [diagnostics, playMode]);
                runtime.Start();
                Assert.True(runtime.interactions.TryGetModule<ConsoleConsumerModule>(out var consumer));
                Assert.NotNull(consumer);
                var source = new DiagnosticSource("tests.console-owner", "Owner test");
                diagnostics.Set(source, [Diagnostic.Error("OWNER", "Before retirement")]);
                Assert.Equal("Before retirement", Assert.Single(consumer.console.Capture().occurrences).message);
                Assert.Equal(1, playMode.listenerCount);
                runtime.Dispose();
                Assert.Equal(0, playMode.listenerCount);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void ClearOnPlayRemovesOldLogsButRetainsCurrentDiagnostics()
    {
        Assert.True(m_console.clearOnPlay);
        Dispatch(LogLevel.Info, LogSessionId.none, "before-play");
        m_diagnosticHub.Set(
            new DiagnosticSource("test.compiler", "Test Compiler"),
            [Diagnostic.Error("TEST001", "compiler-diagnostic")]);
        m_logRouter.Flush();

        _ = m_playMode.Begin();

        EditorConsoleOccurrence[] entries = m_console.Capture().occurrences.ToArray();

        Assert.DoesNotContain(entries, static entry => entry.message == "before-play");
        Assert.Contains(entries, static entry => entry.message == "compiler-diagnostic");
    }

    [Fact]
    public void DiagnosticCorrelationSurvivesProjectionAndDoesNotCollapseDifferentTargets()
    {
        Guid objectId = Guid.NewGuid();
        var source = new DiagnosticSource("tests.activation", "Extension activation");
        m_diagnosticHub.Set(source,
        [
            new Diagnostic("ACTIVATION", "Missing dependency", DiagnosticSeverity.Error, "extension.a", objectId),
            new Diagnostic("ACTIVATION", "Missing dependency", DiagnosticSeverity.Error, "extension.b", objectId),
            new Diagnostic("ACTIVATION", "Missing dependency", DiagnosticSeverity.Error, "extension.a", Guid.NewGuid())
        ]);
        EditorConsoleSnapshot captured = m_console.Capture();
        Assert.Same(captured, m_console.Capture());
        Assert.Equal(3, captured.groups.Count);
        Assert.Equal("extension.a", captured.occurrences[0].semanticId);
        Assert.Equal(objectId, captured.occurrences[0].objectId);
        m_diagnosticHub.Clear(source);
        EditorConsoleSnapshot cleared = m_console.Capture();
        Assert.NotSame(captured, cleared);
        Assert.Empty(cleared.occurrences);
        Assert.Same(cleared, m_console.Capture());
        Assert.Equal(3, captured.occurrences.Count);
        Dispatch(LogLevel.Info, LogSessionId.none, "ordinary log");
        m_logRouter.Flush();
        EditorConsoleOccurrence log = Assert.Single(m_console.Capture().occurrences);
        Assert.Null(log.semanticId);
        Assert.Null(log.objectId);
    }

    [Fact]
    public void ManualClearRemovesHistoryButKeepsCurrentIssuesUntilResolved()
    {
        Dispatch(LogLevel.Error, LogSessionId.none, "old failure");
        using DiagnosticReporter reporter = m_diagnosticHub.CreateReporter(
            new DiagnosticSource("tests.current", "Current issue"));
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
        LogSessionId sessionId = m_playMode.Begin();
        Dispatch(LogLevel.Debug, sessionId, "runtime-debug");
        Dispatch(LogLevel.Info, sessionId, "runtime-info");
        Dispatch(LogLevel.Warn, sessionId, "runtime-warning");
        Dispatch(LogLevel.Error, sessionId, "runtime-error");
        Dispatch(LogLevel.Fatal, sessionId, "runtime-fatal");
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
        LogSessionId sessionId = m_playMode.Begin();
        Dispatch(LogLevel.Info, sessionId, "entry-diagnostic");
        m_playMode.SetState(EditorPlayModeState.Failed);
        m_logRouter.Flush();

        Assert.Contains(
            m_console.Capture().occurrences,
            static entry => entry.message == "entry-diagnostic");
    }

    [Fact]
    public void DisabledClearOnPlayRetainsExistingLogs()
    {
        m_console.clearOnPlay = false;
        Dispatch(LogLevel.Info, LogSessionId.none, "before-play");
        m_logRouter.Flush();

        _ = m_playMode.Begin();

        Assert.Contains(
            m_console.Capture().occurrences,
            static entry => entry.message == "before-play");
    }

    [Fact]
    public void CollapseGroupsEquivalentNonConsecutiveOccurrencesGlobally()
    {
        Dispatch(LogLevel.Info, LogSessionId.none, "repeated", file: "A.cs", line: 10);
        Dispatch(LogLevel.Warn, LogSessionId.none, "middle", file: "B.cs", line: 20);
        Dispatch(LogLevel.Info, LogSessionId.none, "repeated", file: "A.cs", line: 10);
        Dispatch(LogLevel.Info, LogSessionId.none, "repeated", file: "C.cs", line: 30);
        m_logRouter.Flush();

        EditorConsoleSnapshot snapshot = m_console.Capture();
        EditorConsoleGroup repeated = Assert.Single(snapshot.groups.Where(
            static group => group.latest.message == "repeated" && group.latest.file == "A.cs"));

        Assert.Equal(2, repeated.count);
        Assert.Equal(3, snapshot.groups.Count);
        Assert.True(repeated.occurrences[0].sequence < repeated.occurrences[1].sequence);
    }

    [Fact]
    public void CollapseDoesNotMergeEquivalentEntriesAcrossRuntimeSessions()
    {
        LogSessionId first = LogSessionId.Create();
        LogSessionId second = LogSessionId.Create();
        Dispatch(LogLevel.Info, first, "session-entry", file: "A.cs", line: 10);
        Dispatch(LogLevel.Info, second, "session-entry", file: "A.cs", line: 10);
        m_logRouter.Flush();

        EditorConsoleSnapshot snapshot = m_console.Capture();

        Assert.Equal(
            2,
            snapshot.groups.Count(static group => group.latest.message == "session-entry"));
    }

    [Fact]
    public void ConsoleTimelineAppendsTheNewestEntryAtTheBottom()
    {
        Dispatch(LogLevel.Info, LogSessionId.none, "first");
        Dispatch(LogLevel.Error, LogSessionId.none, "latest");
        m_logRouter.Flush();

        EditorConsoleSnapshot snapshot = m_console.Capture();

        Assert.Equal("first", snapshot.occurrences[0].message);
        Assert.Equal("latest", snapshot.occurrences[^1].message);
        Assert.Equal("first", snapshot.groups[0].latest.message);
        Assert.Equal("latest", snapshot.groups[^1].latest.message);
    }

    private void Dispatch(
        LogLevel level,
        LogSessionId sessionId,
        string message,
        string file = nameof(PlayModeLogRetentionTests),
        int line = 0)
        => m_logRouter.Dispatch(new LogEntry(
            level,
            AssemblyDomain.InnoScripting,
            AssemblyScope.Runtime,
            "PlayModeLogRetentionTests",
            message,
            file,
            line,
            $"at {file}:{line}",
            sessionId));

    private sealed class FakePlayMode : IEditorPlayMode
    {
        public EditorPlayModeState state { get; private set; }

        public bool isPlaying => state == EditorPlayModeState.Playing;

        public string? lastFailure => null;

        public LogSessionId activeSessionId { get; private set; }

        private readonly Inno.Core.Events.EventDispatcher m_stateDispatcher = new();
        private readonly Inno.Core.Events.EventHub m_stateEvents;

        internal FakePlayMode() => m_stateEvents = m_stateDispatcher.CreateHub();
        internal int listenerCount { get; private set; }
        public IDisposable ListenStateChanges(Action<EditorPlayModeState> listener)
        {
            IDisposable subscription = m_stateEvents.Listen<StateNotification>(notification => listener(notification.state));
            listenerCount++;
            return new StateSubscription(this, subscription);
        }

        private sealed class StateSubscription(
            FakePlayMode owner,
            IDisposable subscription
        ) : IDisposable
        {
            private IDisposable? m_subscription = subscription;
            public void Dispose()
            {
                if (m_subscription is null)
                    return;
                m_subscription.Dispose();
                m_subscription = null;
                owner.listenerCount--;
            }
        }
        private sealed class StateNotification(EditorPlayModeState state) : Inno.Core.Events.Event
        {
            internal EditorPlayModeState state { get; } = state;
        }

        public bool EnterPlayMode() => false;

        public bool ExitPlayMode() => false;

        internal LogSessionId Begin()
        {
            activeSessionId = LogSessionId.Create();
            SetState(EditorPlayModeState.Compiling);
            return activeSessionId;
        }

        internal void End()
        {
            SetState(EditorPlayModeState.Editing);
            activeSessionId = LogSessionId.none;
        }

        internal void SetState(EditorPlayModeState value)
        {
            state = value;
            m_stateDispatcher.Emit(new StateNotification(value));
        }
    }
}

[EditorModule("tests.console-consumer")]
public sealed class ConsoleConsumerModule : EditorModule
{
    public ConsoleConsumerModule(IEditorConsole console) => this.console = console;
    public IEditorConsole console { get; }
}
