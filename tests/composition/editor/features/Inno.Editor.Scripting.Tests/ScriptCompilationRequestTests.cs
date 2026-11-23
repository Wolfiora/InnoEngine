using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Assets.Pipeline;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.IO;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Editor.Annotations;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Plugins;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scene.Components;
using Inno.Scripting.Compiler;
using Inno.Scripting.Reload;

namespace Inno.Editor.Scripting.Tests;

public sealed class ScriptCompilationRequestTests : ScriptingPipelineFixture
{
    [Fact]
    public void AutomaticPublicationDoesNotQueueItselfButExternalMountChangesStillDo()
    {
        m_fixture.WriteVersionedBehavior(1);
        using ScriptReloadHost reload = m_fixture.CreateReloadHost(autoCompile: true);
        reload.Start();
        Assert.True(m_fixture.CompilePending(reload).success);
        Assert.True(reload.ApplyPendingReload());
        Assert.False(
            reload.isCompilationPending,
            "Publishing this script generation must not queue another generation from its own catalog notification."
        );
        CompleteUnloadVerification(reload);
        m_fixture.assets.ReplaceSourceMounts(m_fixture.assets.sourceMounts);
        Assert.True(reload.isCompilationPending, "An external source-mount publication must still invalidate scripting inputs.");
    }

    [Fact]
    public void EditorAutomaticCompilationClosesModalAndTicketWaitsForRetirement()
    {
        m_fixture.WriteVersionedBehavior(1);
        m_fixture.Rescan();
        using EditorInteractionRuntime runtime = m_fixture.CreateEditorRuntime();
        runtime.Start();
        IEditorScriptCompilation scripting = Assert.IsAssignableFrom<IEditorScriptCompilation>(ScriptingCompilationProbe.compilation);
        PumpEditorUntil(m_fixture, runtime, () => scripting.state == EditorScriptCompilationState.Ready, () => scripting.progress.status);
        AssertCompilationModalHidden(runtime);
        ScriptCompilationResult initial = Assert.IsType<ScriptCompilationResult>(scripting.lastCompilation);
        for (int frame = 0; frame < 150; frame++)
        {
            runtime.Update(new EditorFrame(1f / 60f, frame / 60f, true));
            Assert.Equal(EditorScriptCompilationState.Ready, scripting.state);
            Assert.Same(initial, scripting.lastCompilation);
            Thread.Sleep(5);
        }

        List<Type> retained = RetainActiveScriptType(m_fixture);
        try
        {
            m_fixture.WriteVersionedBehavior(2);
            m_fixture.Rescan();
            IScriptCompilationTicket ticket = scripting.RequestCompilation();
            PumpEditorUntil(
                m_fixture,
                runtime,
                () => m_fixture.host.modules.generations.state == GenerationState.AwaitingCollection,
                () => scripting.progress.status
            );
            Assert.Equal(ScriptCompilationTicketState.Compiling, ticket.state);
            Assert.False(ticket.isCompleted);
            retained.Clear();
            // Publication readiness may be advanced by another host boundary before the next editor update.
            m_fixture.host.modules.generations.Wait();
            PumpEditorUntil(
                m_fixture,
                runtime,
                () => scripting.state == EditorScriptCompilationState.Ready && ticket.isCompleted,
                () => scripting.progress.status,
                focused: false
            );
            Assert.Equal(ScriptCompilationTicketState.Succeeded, ticket.state);
            AssertCompilationModalHidden(runtime);
            Assert.Equal(2, ReadVersion(m_fixture.ResolveActiveType("VersionedBehavior")));
        }
        finally
        {
            retained.Clear();
        }
    }

    [Fact]
    public void EditorQueuesLatestAutomaticChangeWithoutReplacingAcceptedTicketWhileReloadIsBlocked()
    {
        using var fixture = new ScriptingFixture(enableFileSystemWatcher: true, fileWatcherFlushDelayMs: 0);
        fixture.WriteVersionedBehavior(1);
        fixture.Rescan();
        var reloads = new EditorReloadCoordinator();
        using EditorInteractionRuntime runtime = fixture.CreateEditorRuntime(reloads);
        runtime.Start();
        IEditorScriptCompilation scripting = Assert.IsAssignableFrom<IEditorScriptCompilation>(ScriptingCompilationProbe.compilation);
        PumpEditorUntil(
            fixture,
            runtime,
            () => scripting.state == EditorScriptCompilationState.Ready,
            () => $"{scripting.state}: {scripting.progress.status}; ticket={scripting.currentTicket?.state}",
            timeoutMilliseconds: 120_000
        );
        ScriptCompilationResult initial = Assert.IsType<ScriptCompilationResult>(scripting.lastCompilation);
        IScriptCompilationTicket? accepted = scripting.currentTicket;
        var guard = new DeferredReloadGuard();
        using IDisposable registration = reloads.RegisterGuard(guard);
        using (fixture.host.generations.AcquireRead("fixture active Play session"))
        {
            Assert.Equal(ScriptCompilationTicketState.Failed, scripting.RequestCompilation().state);
            Assert.Same(accepted, scripting.currentTicket);
            fixture.WriteVersionedBehavior(2);
            fixture.Rescan();
            fixture.WriteVersionedBehavior(3);
            fixture.Rescan();
            for (int frame = 0; frame < 30; frame++)
            {
                fixture.assets.Update();
                using (fixture.editorSession.EnterExecutionScope())
                {
                    runtime.Update(new EditorFrame(1f / 60f, frame / 60f, true));
                }
                Thread.Sleep(5);
            }

            Assert.Same(initial, scripting.lastCompilation);
            Assert.Same(accepted, scripting.currentTicket);
            Assert.Contains("fixture active Play", scripting.progress.status);
            AssertCompilationModalHidden(runtime);
        }

        guard.isBlocked = false;
        PumpEditorUntil(
            fixture,
            runtime,
            () => scripting.state == EditorScriptCompilationState.Ready && !ReferenceEquals(initial, scripting.lastCompilation),
            () => $"{scripting.state}: {scripting.progress.status}; ticket={scripting.currentTicket?.state}",
            timeoutMilliseconds: 120_000
        );
        Assert.Equal(GenerationState.Ready, fixture.host.generations.state);
        Assert.Equal(3, ReadLatestVersion(fixture));
    }

    [Fact]
    public void NewCompilationTicketSupersedesTheExactPreviousRequest()
    {
        using EditorInteractionRuntime runtime = m_fixture.CreateEditorRuntime();
        runtime.Start();
        _ = runtime.panelCount;
        IEditorScriptCompilation scripting = Assert.IsAssignableFrom<IEditorScriptCompilation>(ScriptingCompilationProbe.compilation);
        IScriptCompilationTicket first = scripting.RequestCompilation();
        IScriptCompilationTicket second = scripting.RequestCompilation();
        Assert.Equal(ScriptCompilationTicketState.Superseded, first.state);
        Assert.True(first.isCompleted);
        Assert.True(second.requestID > first.requestID);
        Assert.Same(second, scripting.currentTicket);
        Assert.Equal(ScriptCompilationTicketState.Queued, second.state);
    }

    [Fact]
    [Trait("ProcessIsolation", "Required")]
    public void SupersededCompilationCannotPublishIntoTheNextProgressOperation()
    {
        m_fixture.Write("Progress.cs", "public sealed class ProgressPayload { }");
        using ScriptReloadHost reload = m_fixture.CreateReloadHost();
        reload.Start();
        reload.RecompileScripting();
        ScriptReloadProgress first = reload.progress;
        Assert.True(reload.TryCompilePending(out Task<ScriptCompilationResult>? running));
        reload.RecompileScripting();
        ScriptReloadProgress queued = reload.progress;
        Assert.True(queued.operationID > first.operationID);
        Assert.False(reload.CancelCompilation(first.operationID));
        _ = CompleteCompilationOnOwner(running!);
        Assert.Same(queued, reload.progress);
        Assert.Equal(0f, queued.fraction);
        Assert.Contains("queued", queued.stage);
        Assert.False(queued.canCancel);
        Assert.True(reload.TryCompilePending(out Task<ScriptCompilationResult>? replacement));
        Assert.True(CompleteCompilationOnOwner(replacement!).success);
        ScriptReloadProgress compiled = reload.progress;
        Assert.Equal(queued.operationID, compiled.operationID);
        Assert.Equal(0.8f, compiled.fraction);
        Assert.Contains("waiting for generation activation", compiled.stage);
        Assert.False(compiled.canCancel);
        Assert.Equal(0f, queued.fraction);
    }

    [Fact]
    [Trait("ProcessIsolation", "Required")]
    public void CompilationFailureDoesNotPublishSuccessfulWorkflowCompletion()
    {
        m_fixture.Write("InvalidProgress.cs", "public sealed class Broken {");
        using ScriptReloadHost reload = m_fixture.CreateReloadHost();
        reload.Start();
        ScriptCompilationResult result = m_fixture.CompilePending(reload);
        Assert.False(result.success);
        Assert.Contains("failed", reload.progress.stage);
        Assert.False(reload.progress.canCancel);
        Assert.False(reload.progress.cancellationRequested);
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    [Trait("ProcessIsolation", "Required")]
    public void NeutralSnapshotSurvivesRetirementWithoutKeepingTheScriptGenerationAlive()
    {
        m_fixture.Write("SnapshotPayload.cs", """
            using InnoEngine.Serialization;
            public sealed class SnapshotPayload : ISerializable
            {
                [SerializableProperty] public int count = 42;
            }
            """);
        using ScriptReloadHost reload = m_fixture.CreateReloadHost();
        reload.Start();
        ScriptCompilationResult first = m_fixture.CompilePending(reload);
        Assert.True(first.success, FormatDiagnostics(first));
        Assert.True(reload.ApplyPendingReload());
        (SerializationSnapshot snapshot, WeakReference retired) = CaptureScriptSnapshot(m_fixture);
        byte[] original = snapshot.ToArray();

        m_fixture.Write("SnapshotPayload.cs", "public sealed class ReplacementPayload { }");
        ScriptCompilationResult replacement = m_fixture.CompilePending(reload);
        Assert.True(replacement.success, FormatDiagnostics(replacement));
        Assert.True(reload.ApplyPendingReload());
        CompleteUnloadVerification(reload);
        Assert.False(retired.IsAlive, "The neutral snapshot retained the old script type across retirement.");
        Assert.Equal(original, snapshot.ToArray());
        Assert.NotEmpty(original);
        GC.KeepAlive(snapshot);
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    [Trait("ProcessIsolation", "Required")]
    public void TenActionMenuGenerationsRetireTypesAndPreserveLastGoodAfterFailure()
    {
        m_fixture.WriteVersionedBehavior(1);
        WriteAction(1);
        var reloads = new EditorReloadCoordinator();
        using ScriptReloadHost reload = m_fixture.CreateReloadHost(reloads);
        reload.Start();
        ScriptCompilationResult first = m_fixture.CompilePending(reload);
        Assert.True(first.success, FormatDiagnostics(first));
        Assert.True(reload.ApplyPendingReload());
        using EditorInteractionRuntime runtime = m_fixture.CreateEditorRuntime(reloads);
        runtime.Start();
        EditorMenuModel retainedMenu = runtime.interactions.For("tests/menu-lifecycle").BuildMenu();
        Assert.Equal("Generation 1", Assert.Single(retainedMenu.items).label);
        WeakReference previous = CaptureActiveType(m_fixture, "ReloadedAction", 1);

        for (int version = 2; version <= 11; version++)
        {
            m_fixture.Write("Editor/LifecycleAction.editor.cs", "public sealed class ReloadedAction {");
            ScriptCompilationResult failed = m_fixture.CompilePending(reload);
            Assert.False(failed.success);
            Assert.False(reload.ApplyPendingReload());
            AssertMenu(runtime, version - 1);

            WriteAction(version);
            ScriptCompilationResult replacement = m_fixture.CompilePending(reload);
            Assert.True(replacement.success, FormatDiagnostics(replacement));
            Assert.True(reload.ApplyPendingReload());
            CompleteUnloadVerification(reload);
            Assert.False(previous.IsAlive, "The retired Action type survived the public generation barrier.");
            AssertMenu(runtime, version);
            previous = CaptureActiveType(m_fixture, "ReloadedAction", version);
        }

        Assert.Equal("Generation 1", Assert.Single(retainedMenu.items).label);
        GC.KeepAlive(retainedMenu);

        void WriteAction(int version)
            => m_fixture.Write("Editor/LifecycleAction.editor.cs", $$"""
                using InnoEditor.Interactions;

                [EditorAction("tests.lifecycle.action", "tests/menu-lifecycle")]
                [EditorMenu("tests/menu-lifecycle", "Generation")]
                public sealed class ReloadedAction : EditorAction
                {
                    public int version => {{version}};
                    protected override EditorActionState Query(EditorActionContext context)
                        => new(true, true, false, "Generation {{version}}", null);
                    protected override void Execute(EditorActionContext context) { }
                }
                """);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int ReadLatestVersion(ScriptingFixture fixture) => ReadVersion(fixture.ResolveActiveType("VersionedBehavior"));

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static List<Type> RetainActiveScriptType(ScriptingFixture fixture) => [fixture.ResolveActiveType("VersionedBehavior")];

    private static void AssertCompilationModalHidden(EditorInteractionRuntime runtime)
    {
        EditorModalExtension modal = runtime.modals.Single(value => value.id == "scripting.compilation");
        Assert.True(modal.TryGetPresentation(out EditorModalExtension.Presentation presentation));
        Assert.False(presentation.isVisible);
    }

    // Reload and fixture retirement must remain on their creating owner thread.
    private static ScriptCompilationResult CompleteCompilationOnOwner(Task<ScriptCompilationResult> compilation)
        => compilation.GetAwaiter().GetResult();

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (SerializationSnapshot, WeakReference) CaptureScriptSnapshot(ScriptingFixture fixture)
    {
        Type type = fixture.ResolveActiveType("SnapshotPayload");
        ISerializable payload = (ISerializable)Activator.CreateInstance(type)!;
        return (fixture.host.serialization.CaptureSnapshot(new CapturedScriptPayload(payload)), new WeakReference(type));
    }

    private static void AssertMenu(
        EditorInteractionRuntime runtime,
        int version
    ) {
        EditorMenuItem item = Assert.Single(runtime.interactions.For("tests/menu-lifecycle").BuildMenu().items);
        Assert.Equal("Generation " + version, item.label);
        Assert.Equal("tests.lifecycle.action", item.actionID);
        Assert.True(item.status.isEnabled);
    }

    private sealed class DeferredReloadGuard : IEditorReloadGuard
    {
        public bool canRequestReload => !isBlocked;
        public bool canActivateReload => !isBlocked;
        public string? blockingReason => isBlocked ? "Waiting for fixture active Play to retire." : null;

        internal bool isBlocked { get; set; } = true;
    }
}
