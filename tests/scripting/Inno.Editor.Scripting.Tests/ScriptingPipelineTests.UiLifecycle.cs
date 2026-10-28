using System;
using System.Runtime.CompilerServices;
using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;
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

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (SerializationSnapshot, WeakReference) CaptureScriptSnapshot(ScriptingFixture fixture)
    {
        Type type = fixture.ResolveActiveType("SnapshotPayload");
        var payload = (ISerializable)Activator.CreateInstance(type)!;
        return (fixture.host.serialization.CaptureSnapshot(new CapturedScriptPayload(payload)), new WeakReference(type));
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

internal sealed class CapturedScriptPayload(ISerializable payload) : ISerializable
{
    internal ISerializable payload { get; } = payload;
}

internal sealed class CapturedScriptPayloadConverter : SerializationConverter<CapturedScriptPayload>
{
    public override void Write(
        SerializationWriter writer,
        CapturedScriptPayload value
    ) => writer.WritePropertiesData("state", value.payload);

    public override CapturedScriptPayload Read(SerializationReader reader)
        => throw new NotSupportedException("This fixture only captures an existing generation.");
}
