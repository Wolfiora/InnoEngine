using System;
using System.Threading;
using Inno.Editor.ImGui;
using Xunit;

namespace Inno.Editor.ImGui.Tests;
public sealed class EditorUiSessionTests
{
    [Fact]
    public void StyleAndControlsBelongToOneSession()
    {
        using var first = new EditorUiSession();
        using var second = new EditorUiSession();
        first.widgets.style.SetZoom(1.5f);
        second.widgets.style.SetCompactMode(true);
        Assert.NotSame(first.widgets, second.widgets);
        Assert.Equal(1.5f, first.widgets.style.zoom);
        Assert.Equal(1f, second.widgets.style.zoom);
        Assert.False(first.widgets.style.isCompact);
        Assert.True(second.widgets.style.isCompact);
    }

    [Fact]
    public void RejectedScopeRetirementPreservesTheActiveSession()
    {
        using var first = new EditorUiSession();
        using var second = new EditorUiSession();
        IDisposable outer = first.Enter();
        IDisposable inner = second.Enter();
        Assert.Throws<InvalidOperationException>(outer.Dispose);
        Assert.Same(second, EditorUiSession.current);
        Assert.Throws<InvalidOperationException>(first.Dispose);
        inner.Dispose();
        Assert.Same(first, EditorUiSession.current);
        outer.Dispose();
        Assert.Throws<InvalidOperationException>(() => EditorUiSession.current);
    }

    [Fact]
    public void RetiredSessionsRejectActivationAndControlUse()
    {
        var session = new EditorUiSession();
        session.Dispose();
        Assert.True(session.isDisposed);
        Assert.Throws<ObjectDisposedException>(() => session.Enter());
        Assert.Throws<ObjectDisposedException>(() => session.widgets.CenteredButton("Retired"));
    }

    [Fact]
    public void ASessionCannotBeActivatedFromAnotherThread()
    {
        using var session = new EditorUiSession();
        Exception? failure = null;
        var thread = new Thread(() => failure = Record.Exception(() => session.Enter()));
        thread.Start();
        Assert.True(thread.Join(TimeSpan.FromSeconds(5)));
        Assert.IsType<InvalidOperationException>(failure);
    }
}
