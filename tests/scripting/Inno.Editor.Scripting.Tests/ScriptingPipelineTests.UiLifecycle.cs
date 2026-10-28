using System;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Scripting.Compiler;
using Inno.Scripting.Reload;
using Xunit;

namespace Inno.Editor.Scripting.Tests;

public sealed partial class ScriptingPipelineTests
{
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
        {
            m_fixture.Write("Editor/LifecycleAction.editor.cs", $$"""
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
    }

    private static void AssertMenu(
        EditorInteractionRuntime runtime,
        int version
    ) {
        EditorMenuItem item = Assert.Single(runtime.interactions.For("tests/menu-lifecycle").BuildMenu().items);
        Assert.Equal("Generation " + version, item.label);
        Assert.Equal("tests.lifecycle.action", item.actionId);
        Assert.True(item.status.isEnabled);
    }
}
