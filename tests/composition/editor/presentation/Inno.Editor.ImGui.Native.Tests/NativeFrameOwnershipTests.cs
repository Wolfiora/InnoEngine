using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Xunit;
using Inno.Core.Diagnostics;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;

using EditorImGui = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUISession;
using GuiMouseButton = Inno.Editor.ImGui.ImGuiMouseButton;
using GuiWindowFlags = Inno.Editor.ImGui.ImGuiWindowFlags;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using Widget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;

namespace Inno.Editor.ImGui.Native.Tests;

public sealed unsafe class NativeFrameOwnershipTests : WidgetLayoutFixture
{
    [Fact]
    public void StandaloneMetricsApplyImmediatelyAndRetainedMetricsDoNotRootTheirRetiredSession()
    {
        var standalone = new EditorStyleMetrics();
        Assert.True(standalone.SetZoom(1.25f));
        Assert.True(standalone.SetCompactMode(true));
        Assert.Equal(1.25f, standalone.zoom);
        Assert.True(standalone.isCompact);
        EditorStyleMetrics retained = RetireStyleOwner(out WeakReference owner);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(owner.IsAlive);
        Assert.Throws<ObjectDisposedException>(() => retained.SetZoom(1.1f));
        Assert.Throws<ObjectDisposedException>(() => retained.SetCompactMode(true));
    }

    [Fact]
    public void StyleRequestsDoNotChangeTheActiveNativeFrame()
    {
        using var context = new NativeContext(1f);
        Widget widgets = EditorUI.current.widgets;
        for (int frame = 0; frame < 2; frame++)
        {
            context.Frame(() =>
            {
                widgets.SetupStyle();
                BeginWindow("Stable style input", new Vector2(400f, 300f));
                try
                {
                    if (frame == 0)
                    {
                        float padding = widgets.style.cardBodyPadding.X;
                        Assert.True(widgets.style.SetZoom(1.25f));
                        Assert.True(widgets.style.SetCompactMode(true));
                        Assert.Equal(1f, widgets.style.zoom);
                        Assert.False(widgets.style.isCompact);
                        widgets.SetupStyle();
                        Assert.Equal(1f, widgets.style.zoom);
                        Assert.Equal(padding, widgets.style.cardBodyPadding.X);
                    }
                    else
                    {
                        Assert.Equal(1.25f, widgets.style.zoom);
                        Assert.True(widgets.style.isCompact);
                    }
                    widgets.PropertyLabel("Stable frame / 同帧尺度");
                }
                finally
                {
                    NativeImGui.End();
                }
            });
        }
    }

    [Fact]
    public void AForeignNativeContextCannotPublishChangesIntoASuspendedOwnerFrame()
    {
        using var context = new NativeContext(1f);
        Widget widgets = EditorUI.current.widgets;
        context.Frame(() =>
        {
            widgets.SetupStyle();
            BeginWindow("Suspended style owner", new Vector2(400f, 300f));
            try
            {
                ImGuiNativeContext* own = NativeImGui.Raw.GetCurrentContext();
                ImGuiNativeContext* foreign = NativeImGui.Raw.CreateContext(null);
                try
                {
                    NativeImGui.Raw.SetCurrentContext(foreign);
                    Assert.True(widgets.style.SetZoom(1.25f));
                    Assert.Equal(1f, widgets.style.zoom);
                }
                finally
                {
                    NativeImGui.Raw.SetCurrentContext(own);
                    NativeImGui.Raw.DestroyContext(foreign);
                    NativeImGui.Raw.SetCurrentContext(own);
                }
                widgets.SetupStyle();
                Assert.Equal(1f, widgets.style.zoom);
            }
            finally
            {
                NativeImGui.End();
            }
        });
        context.Frame(() =>
        {
            widgets.SetupStyle();
            Assert.Equal(1.25f, widgets.style.zoom);
        });
    }

    [Fact]
    public void CopiedWindowGeometryRequiresAnActiveNativeFrame()
    {
        using var context = new NativeContext(1f);
        Assert.Throws<InvalidOperationException>(() => EditorImGui.GetClipBounds());
        Assert.Throws<InvalidOperationException>(() => EditorImGui.GetWindowBounds());
        context.Frame(() =>
        {
            BeginWindow("Geometry owner", new Vector2(400f, 300f));
            try
            {
                (Vector2 minimum, Vector2 maximum) clip = EditorImGui.GetClipBounds();
                Assert.True(clip.maximum.X > clip.minimum.X && clip.maximum.Y > clip.minimum.Y);
                Assert.True(EditorImGui.GetWindowBounds().maximumX > EditorImGui.GetWindowBounds().minimumX);
            }
            finally
            {
                NativeImGui.End();
            }
        });
        Assert.Throws<InvalidOperationException>(() => EditorImGui.GetClipBounds());
        Assert.Throws<InvalidOperationException>(() => EditorImGui.GetWindowBounds());
    }

    [Theory]
    [InlineData("card-leading")]
    [InlineData("card-trailing")]
    [InlineData("card-menu")]
    [InlineData("tree-content")]
    [InlineData("tree-overlay")]
    public void ExtensionDrawingFailureRestoresTheEnclosingWindow(string failure)
    {
        using var context = new NativeContext(1f);
        for (int frame = 0; frame < 2; frame++)
        {
            context.Frame(() =>
            {
                BeginWindow("Failure owner", new Vector2(400f, 300f));
                nint window = (nint)NativeImGui.Raw.GetCurrentWindow();
                int depth = NativeImGui.Raw.GetCurrentWindow()->DC.TreeDepth;
                Vector2 padding = Unsafe.BitCast<NativeVector2, Vector2>(NativeImGui.Raw.GetStyle()->FramePadding);
                Widget widgets = EditorUI.current.widgets;
                Action rejected = () => throw new InvalidOperationException("Rejected extension draw");
                Assert.Throws<InvalidOperationException>(() =>
                {
                    if (failure.StartsWith("card-", StringComparison.Ordinal))
                    {
                        _ = widgets.CollapsingCard(
                            "failed-card",
                            "Extension",
                            drawLeadingControl: failure == "card-leading" ? rejected : null,
                            drawTrailingControl: failure == "card-trailing" ? rejected : null,
                            dimmed: true,
                            drawContextMenu: failure == "card-menu" ? rejected : null
                        );
                    }
                    else
                    {
                        widgets.SetNextTreeNodeOpen(true);
                        _ = widgets.TreeNode("failed-tree", _ =>
                        {
                            if (failure == "tree-content")
                            {
                                rejected();
                            }
                            EditorImGui.Text("Extension");
                        }, new TreeNodeOptions() { drawViewportOverlay = failure == "tree-overlay" ? rejected : null });
                    }
                });
                Assert.Equal(window, (nint)NativeImGui.Raw.GetCurrentWindow());
                Assert.Equal(depth, NativeImGui.Raw.GetCurrentWindow()->DC.TreeDepth);
                Assert.Equal(padding, Unsafe.BitCast<NativeVector2, Vector2>(NativeImGui.Raw.GetStyle()->FramePadding));
                widgets.SetNextTreeNodeOpen(true);
                TreeNodeResult next = widgets.TreeNode("next-tree", _ => EditorImGui.Text("Next valid row"), new TreeNodeOptions());
                Assert.True(next.isOpen);
                NativeImGui.TreePop();
                EditorImGui.GetDrawList().Line(new Vector2(30f, 30f), new Vector2(60f, 60f), uint.MaxValue);
                NativeImGui.End();
            });
        }
    }

    [Fact]
    public void DrawingBorrowRejectsAnotherWindowOrFrame()
    {
        using var context = new NativeContext(1f);
        EditorDrawList borrow = default;
        context.Frame(() =>
        {
            BeginWindow("Drawing owner", new Vector2(400f, 300f));
            borrow = EditorImGui.GetDrawList();
            borrow.Line(new Vector2(30f, 30f), new Vector2(60f, 60f), uint.MaxValue);
            _ = EditorImGui.BeginChild("Other owner", new Vector2(100f, 100f));
            Assert.Throws<InvalidOperationException>(() => borrow.Line(Vector2.Zero, Vector2.One, 0));
            EditorImGui.EndChild();
            NativeImGui.End();
        });
        context.Frame(() =>
        {
            BeginWindow("Drawing owner", new Vector2(400f, 300f));
            Assert.Throws<InvalidOperationException>(() => borrow.Line(Vector2.Zero, Vector2.One, 0));
            NativeImGui.End();
        });
        Assert.Throws<InvalidOperationException>(() => default(EditorDrawList).Line(Vector2.Zero, Vector2.One, 0));
    }

    [Fact]
    public void MixedValueRestoresOuterStateAfterNestedGroupsAndFailures()
    {
        using var context = new NativeContext(1f);
        context.Frame(() =>
        {
            BeginWindow("Mixed scope", new Vector2(400f, 300f));
            Widget widgets = EditorUI.current.widgets;
            Assert.False(widgets.isMixedValue);
            widgets.MixedValue(true, () =>
            {
                Assert.True(widgets.isMixedValue);
                widgets.MixedValue(false, () => Assert.True(widgets.isMixedValue));
                Assert.True(widgets.isMixedValue);
            });
            Assert.False(widgets.isMixedValue);
            Assert.Throws<InvalidOperationException>(
                () => widgets.MixedValue(true, () => throw new InvalidOperationException("rejected draw"))
            );
            Assert.False(widgets.isMixedValue);
            NativeImGui.End();
        });
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static EditorStyleMetrics RetireStyleOwner(out WeakReference owner)
    {
        using EditorUI session = new EditorUISession();
        owner = new WeakReference(session);
        return session.widgets.style;
    }
}
