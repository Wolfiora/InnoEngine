using System;
using System.Threading;
using Xunit;
using Inno.Editor.ImGui;

namespace Inno.Editor.ImGui.Tests;

public sealed class EditorUISessionTests
{
    [Fact]
    public void StyleAndControlsBelongToOneSession()
    {
        using var first = new EditorUISession();
        using var second = new EditorUISession();
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
        using var first = new EditorUISession();
        using var second = new EditorUISession();
        IDisposable outer = first.Enter();
        IDisposable inner = second.Enter();
        Assert.Throws<InvalidOperationException>(outer.Dispose);
        Assert.Same(second, EditorUISession.current);
        Assert.Throws<InvalidOperationException>(first.Dispose);
        inner.Dispose();
        Assert.Same(first, EditorUISession.current);
        outer.Dispose();
        Assert.Throws<InvalidOperationException>(() => EditorUISession.current);
    }

    [Fact]
    public void RetiredSessionsRejectActivationAndControlUse()
    {
        var session = new EditorUISession();
        session.Dispose();
        Assert.True(session.isDisposed);
        Assert.Throws<ObjectDisposedException>(() => session.Enter());
        Assert.Throws<ObjectDisposedException>(() => session.widgets.CenteredButton("Retired"));
    }

    [Fact]
    public void ASessionCannotBeActivatedFromAnotherThread()
    {
        using var session = new EditorUISession();
        Exception? failure = null;
        var thread = new Thread(() => failure = Record.Exception(() => session.Enter()));
        thread.Start();
        Assert.True(thread.Join(TimeSpan.FromSeconds(5)));
        Assert.IsType<InvalidOperationException>(failure);
    }
}
