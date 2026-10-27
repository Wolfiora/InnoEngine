using System;
using System.Linq;
using Inno.Core.Input;
using Inno.Editor.Interactions;
using Xunit;

namespace Inno.Editor.Interactions.Tests;

public sealed partial class EditorRuntimeTests
{
    [Fact]
    public void StaticMenuTemplatesKeepAvailabilityLabelsAndReasonsFresh()
    {
        var target = new MenuStateTarget();
        var interaction = m_runtime.interactions.For("tests/static-menu", target);
        for (int index = 0; index < 3; index++)
        {
            target.label = "Current " + index;
            target.enabled = index != 1;
            target.isChecked = index == 2;
            EditorMenuItem leaf = Assert.Single(Assert.Single(interaction.BuildMenu().items).children);
            Assert.Equal(target.label, leaf.label);
            Assert.Equal(target.enabled, leaf.status.isEnabled);
            Assert.Equal(target.isChecked, leaf.status.isChecked);
            Assert.Equal(target.enabled ? null : "Current owner is read-only.", leaf.status.unavailableReason);
        }
        target.visible = false;
        Assert.Empty(interaction.BuildMenu().items);
        m_types.Rebuild();
        target.visible = true;
        target.label = "After registry refresh";
        Assert.Equal(target.label, Assert.Single(Assert.Single(interaction.BuildMenu().items).children).label);
        GC.KeepAlive(target);
    }

    [Fact]
    public void HiddenHigherOrderPlacementDoesNotEraseTheVisibleAction()
    {
        var target = new MenuStateTarget { visible = false };
        var interaction = m_runtime.interactions.For("tests/shared-menu", target);
        Assert.Equal("tests.menu-fallback", Assert.Single(interaction.BuildMenu().items).actionId);
        target.visible = true;
        Assert.Equal("tests.menu-conditional", Assert.Single(interaction.BuildMenu().items).actionId);
        target.visible = false;
        Assert.Equal("tests.menu-fallback", Assert.Single(interaction.BuildMenu().items).actionId);
        GC.KeepAlive(target);
    }

    [Fact]
    public void FailedDynamicSourceCannotPublishItsPartialMenu()
    {
        Assert.Empty(m_runtime.interactions.For("tests/failed-menu").BuildMenu().items);
        Assert.NotEmpty(m_runtime.interactions.For("tests/menu").BuildMenu().items);
    }

    [Fact]
    public void ShortcutIndexRefreshPreservesDispatchAndRejectsRepeats()
    {
        m_runtime.interactions.For("tests/multiple-shortcuts").Focus();
        m_runtime.HandleKeyPressed(new(0, KeyCode.F3));
        Assert.Equal(1, MultiShortcutAction.executeCount);
        m_types.Rebuild();
        m_runtime.HandleKeyPressed(new(0, KeyCode.F4));
        Assert.Equal(2, MultiShortcutAction.executeCount);
        m_runtime.HandleKeyPressed(new(0, KeyCode.F4, repeat: true));
        Assert.Equal(2, MultiShortcutAction.executeCount);
    }
}

public sealed class MenuStateTarget
{
    public bool visible = true;
    public bool enabled = true;
    public bool isChecked;
    public string label = "Current";
}

[EditorAction("tests.menu-state", "tests/static-menu")]
[EditorMenu("tests/static-menu", "Tools/State")]
public sealed class MenuStateAction : EditorAction
{
    protected override EditorActionState Query(EditorActionContext context)
        => context.target is MenuStateTarget target
            ? new(target.visible, target.enabled, target.isChecked, target.label, "Current owner is read-only.")
            : EditorActionState.hidden;

    protected override void Execute(EditorActionContext context) { }
}

[EditorAction("tests.menu-fallback", "tests/shared-menu")]
[EditorMenu("tests/shared-menu", "Shared", order: 100)]
public sealed class MenuFallbackAction : EditorAction
{
    protected override void Execute(EditorActionContext context) { }
}

[EditorAction("tests.menu-conditional", "tests/shared-menu")]
[EditorMenu("tests/shared-menu", "Shared", order: 200)]
public sealed class MenuConditionalAction : EditorAction
{
    protected override EditorActionState Query(EditorActionContext context)
        => context.target is MenuStateTarget { visible: true } ? EditorActionState.enabled : EditorActionState.hidden;

    protected override void Execute(EditorActionContext context) { }
}

[EditorMenuSource("tests/failed-menu")]
public sealed class RejectedMenuSource : EditorMenuSource
{
    public override void Build(
        EditorMenuContext context,
        EditorMenuBuilder builder
    ) {
        builder.Add("Partial", "tests.menu");
        throw new InvalidOperationException("Candidate menu source failed.");
    }
}
