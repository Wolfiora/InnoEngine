using System;
using System.Numerics;
using BGCS.Runtime;
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

public sealed unsafe class TreeRowTests : WidgetLayoutFixture
{
    [Fact]
    public void FlattenedRowsRetainExpansionWithoutOwningNativeTreeScopes()
    {
        using var context = new NativeContext(1f);
        for (int frame = 0; frame < 3; frame++)
        {
            context.Frame(() =>
            {
                BeginWindow("Flattened tree", new Vector2(400f, 300f));
                try
                {
                    Widget widgets = EditorUI.current.widgets;
                    widgets.SetTreeNodeOpen("root", true);
                    ulong revision = widgets.treeExpansionRevision;
                    widgets.SetTreeNodeOpen("root", true);
                    Assert.Equal(revision, widgets.treeExpansionRevision);
                    int nativeDepth = NativeImGui.Raw.GetCurrentWindow()->DC.TreeDepth;
                    TreeNodeResult root = widgets.TreeNode(
                        "root",
                        _ => NativeImGui.TextUnformatted("Root"),
                        new TreeNodeOptions() { depth = 0 }
                    );
                    Assert.True(root.isOpen);
                    Assert.True(widgets.IsTreeNodeOpen("root"));
                    Assert.Equal(nativeDepth, NativeImGui.Raw.GetCurrentWindow()->DC.TreeDepth);
                    TreeNodeResult child = widgets.TreeNode(
                        "child",
                        _ => NativeImGui.TextUnformatted("Deep child"),
                        new TreeNodeOptions() { depth = 7, isLeaf = true }
                    );
                    Assert.True(child.contentMin.X > root.contentMin.X);
                    var failure = new InvalidOperationException("Rejected flat content");
                    Assert.Same(
                        failure,
                        Assert.Throws<InvalidOperationException>(
                            () => widgets.TreeNode("failed", _ => throw failure, new TreeNodeOptions() { depth = 2 })
                        )
                    );
                    Assert.Equal(nativeDepth, NativeImGui.Raw.GetCurrentWindow()->DC.TreeDepth);
                    widgets.SetTreeNodeOpen("root", false);
                    Assert.True(widgets.treeExpansionRevision > revision);
                    Assert.False(widgets.IsTreeNodeOpen("root"));
                }
                finally
                {
                    NativeImGui.End();
                }
            });
        }
    }

    [Fact]
    public void TenThousandTreeRowsSubmitCustomContentOnlyWithinTheViewport()
    {
        using var context = new NativeContext(1f);
        for (int frame = 0; frame < 3; frame++)
        {
            context.Frame(() =>
            {
                BeginWindow("Large tree", new Vector2(400f, 300f));
                int drawn = 0;
                Action<TreeNodeDrawContext> draw = _ =>
                {
                    drawn++;
                    NativeImGui.TextUnformatted("Visible content");
                };
                for (int row = 0; row < 10_000; row++)
                {
                    _ = EditorUI.current.widgets.TreeNode("row-" + row, draw, new TreeNodeOptions() { isLeaf = true });
                }
                Assert.InRange(drawn, 1, 64);
                if (frame == 1)
                {
                    NativeImGui.SetScrollY(100_000f);
                }
                NativeImGui.End();
            });
        }
    }

    [Fact]
    public void FixedRowsRetainExplicitRowsAndBalanceTheirNativeOwner()
    {
        using var context = new NativeContext(1f);
        context.Frame(() =>
        {
            BeginWindow("Clipped rows", new Vector2(400f, 300f));
            int submitted = 0;
            bool preserved = false;
            float height = NativeImGui.GetTextLineHeightWithSpacing();
            EditorUI.current.widgets.ClippedRows(10_000, height, 0, (
                state,
                start,
                end
            ) =>
            {
                submitted += end - start;
                preserved |= start <= 9999 && end > 9999;
                for (int row = start; row < end; row++)
                {
                    NativeImGui.Dummy(new Vector2(100f, NativeImGui.GetTextLineHeight()));
                }
            }, [9999]);
            Assert.InRange(submitted, 1, 64);
            Assert.True(preserved);
            NativeImGui.End();
        });
    }

    [Fact]
    public void FixedRowFailureReleasesTheClipperBeforeTheNextFrame()
    {
        using var context = new NativeContext(1f);
        for (int frame = 0; frame < 2; frame++)
        {
            context.Frame(() =>
            {
                BeginWindow("Clipper failure", new Vector2(400f, 300f));
                int before = NativeImGui.Raw.GetCurrentContext()->ClipperTempDataStacked;
                var failure = new InvalidOperationException("Row extension rejected drawing.");
                Assert.Same(
                    failure,
                    Assert.Throws<InvalidOperationException>(() => EditorUI.current.widgets.ClippedRows(
                        10_000,
                        NativeImGui.GetTextLineHeightWithSpacing(),
                        0,
                        (
                            state,
                            start,
                            end
                        ) => throw failure
                    ))
                );
                Assert.Equal(before, NativeImGui.Raw.GetCurrentContext()->ClipperTempDataStacked);
                NativeImGui.End();
            });
        }
    }

    [Theory]
    [InlineData(true, 1f)]
    [InlineData(false, 1f)]
    [InlineData(true, 1.5f)]
    [InlineData(false, 1.5f)]
    public void TreeRowsContentClickIsDeliveredOnce(
        bool isLeaf,
        float zoom
    ) {
        using var context = new NativeContext(zoom);
        int presses = 0;
        Vector2 hit = default;
        Action draw = () =>
        {
            BeginWindow("Tree content", new Vector2(400f, 300f));
            TreeNodeResult result = EditorUI.current.widgets.TreeNode(
                $"content-{isLeaf}-{zoom}",
                _ => EditorUI.current.widgets.IconText("O", "Selected object", false),
                new TreeNodeOptions()
                {
                    isLeaf = isLeaf,
                    drawViewportOverlay = () =>
                    {
                        float rowY = NativeImGui.GetItemRectMin().Y;
                        NativeImGui.SetCursorScreenPos(new Vector2(360f, rowY));
                        _ = EditorUI.current.widgets.ClickableIcon("visibility", "V", "Visibility");
                    }
                }
            );
            hit = new Vector2(result.contentMin.X + 80f, (result.min.Y + result.max.Y) * 0.5f);
            if (result.isClicked)
            {
                presses++;
            }
            if (result.isOpen)
            {
                NativeImGui.TreePop();
            }
            NativeImGui.End();
        };
        context.Frame(draw);
        context.Frame(draw);
        context.Click(hit, 0, draw, settleHover: false);
        Assert.Equal(1, presses);
    }

    [Fact]
    public void TreeRowsDisclosureClickExpandsWithoutSelectingContent()
    {
        using var context = new NativeContext(1f);
        int presses = 0;
        bool opened = false;
        Vector2 hit = default;
        Action draw = () =>
        {
            BeginWindow("Tree disclosure", new Vector2(400f, 300f));
            TreeNodeResult result = EditorUI.current.widgets.TreeNode(
                "disclosure",
                _ => NativeImGui.TextUnformatted("Parent"),
                new TreeNodeOptions() { isLeaf = false }
            );
            hit = new Vector2(result.contentMin.X - NativeImGui.GetTreeNodeToLabelSpacing() * 0.5f, (result.min.Y + result.max.Y) * 0.5f);
            opened = result.isOpen;
            if (result.isClicked)
            {
                presses++;
            }
            if (result.isOpen)
            {
                NativeImGui.TreePop();
            }
            NativeImGui.End();
        };
        context.Frame(draw);
        context.Frame(draw);
        context.Click(hit, 0, draw);
        context.Frame(draw);
        Assert.True(opened);
        Assert.Equal(0, presses);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void TreeRowsOverlayConsumesItsOwnClick(bool settleHover)
    {
        using var context = new NativeContext(1f);
        int rowPresses = 0;
        int overlayPresses = 0;
        Vector2 overlayHit = default;
        Action draw = () =>
        {
            BeginWindow("Tree overlay", new Vector2(400f, 300f));
            TreeNodeResult result = EditorUI.current.widgets.TreeNode(
                $"overlay-{settleHover}",
                _ => NativeImGui.TextUnformatted("Object"),
                new TreeNodeOptions()
                {
                    isLeaf = true,
                    drawViewportOverlay = () =>
                    {
                        float rowY = NativeImGui.GetItemRectMin().Y;
                        NativeImGui.SetCursorScreenPos(new Vector2(360f, rowY));
                        if (NativeImGui.SmallButton("Eye"))
                        {
                            overlayPresses++;
                        }
                        overlayHit = (NativeImGui.GetItemRectMin() + NativeImGui.GetItemRectMax()) * 0.5f;
                    }
                }
            );
            if (result.isClicked)
            {
                rowPresses++;
            }
            NativeImGui.End();
        };
        context.Frame(draw);
        context.Frame(draw);
        context.Click(overlayHit, 0, draw, settleHover);
        Assert.Equal(1, overlayPresses);
        Assert.Equal(0, rowPresses);
    }

    [Fact]
    public void TreeRowsContentDoubleClickExpandsOnce()
    {
        using var context = new NativeContext(1f);
        int presses = 0;
        int doublePresses = 0;
        bool opened = false;
        Vector2 hit = default;
        Action draw = () =>
        {
            BeginWindow("Tree double click", new Vector2(400f, 300f));
            TreeNodeResult result = EditorUI.current.widgets.TreeNode(
                "double-click",
                _ => NativeImGui.TextUnformatted("Parent"),
                new TreeNodeOptions() { isLeaf = false }
            );
            hit = new Vector2(result.contentMin.X + 80f, (result.min.Y + result.max.Y) * 0.5f);
            opened = result.isOpen;
            if (result.isClicked)
            {
                presses++;
            }
            if (result.isDoubleClicked)
            {
                doublePresses++;
            }
            if (result.isOpen)
            {
                NativeImGui.TreePop();
            }
            NativeImGui.End();
        };
        context.Frame(draw);
        context.Frame(draw);
        context.Click(hit, 0, draw);
        context.Click(hit, 0, draw);
        context.Frame(draw);
        Assert.Equal(2, presses);
        Assert.Equal(1, doublePresses);
        Assert.True(opened);
    }
}
