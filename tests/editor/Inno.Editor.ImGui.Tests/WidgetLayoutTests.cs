using System.Runtime.CompilerServices;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using BGCS.Runtime;
using System;
using System.Numerics;

using Inno.Native.ImGui;
using Xunit;
using NativeImGui = Inno.Native.ImGui.ImGui;
using Widget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;

namespace Inno.Editor.ImGui.Tests;

public sealed unsafe class WidgetLayoutTests
{
    [Theory]
    [InlineData(0.75f)]
    [InlineData(1f)]
    [InlineData(1.5f)]
    public void PropertyRows_WrapWithinTwoToThreeColumnsWithoutScrollbars(float zoom)
    {
        using NativeContext context = new(zoom);
        Vector2 labelSize = default;
        float labelWidth = 0f;
        float valueWidth = 0f;
        float fontHeight = 0f;
        Vector2 scroll = default;
        for (int frame = 0; frame < 5; frame++)
        {
            context.Frame(() =>
            {
                BeginWindow("Property layout", new Vector2(320f, 270f));
                fontHeight = NativeImGui.GetTextLineHeight();
                Widget.PropertyRow(
                    "persistence",
                    () =>
                    {
                        labelWidth = NativeImGui.GetContentRegionAvail().X;
                        Widget.PropertyLabel("Persistent data directory for this exported application");
                        labelSize = NativeImGui.GetItemRectSize();
                    },
                    () =>
                    {
                        valueWidth = NativeImGui.GetContentRegionAvail().X;
                        string value = "Application data";
                        Widget.SearchInput("directory", "Directory", ref value);
                    });
                scroll = new Vector2(NativeImGui.GetScrollMaxX(), NativeImGui.GetScrollMaxY());
                NativeImGui.End();
            });
        }

        Assert.InRange(labelWidth / (labelWidth + valueWidth), 0.37f, 0.42f);
        Assert.True(labelSize.Y > fontHeight * 1.5f);
        Assert.True(labelSize.X <= labelWidth + 1f);
        Assert.Equal(Vector2.Zero, scroll);
    }

    [Theory]
    [InlineData(2, false, 0.75f)]
    [InlineData(2, false, 0.85f)]
    [InlineData(2, false, 0.9f)]
    [InlineData(2, false, 1f)]
    [InlineData(2, false, 1.1f)]
    [InlineData(2, false, 1.5f)]
    [InlineData(100, true, 1f)]
    [InlineData(100, true, 1.5f)]
    public void BoundedSelectors_OpenDownwardAndScrollOnlyWhenRequired(
        int itemCount,
        bool shouldScroll,
        float zoom
    ) {
        using NativeContext context = new(zoom);
        Vector2 controlMinimum = default;
        Vector2 controlMaximum = default;
        Vector2 popupPosition = default;
        Vector2 popupSize = default;
        Vector2 scroll = default;
        bool opened = false;
        Action draw = () =>
        {
            BeginWindow("Selector layout", new Vector2(400f, 360f));
            NativeImGui.SetNextItemWidth(260f);
            bool visible = Widget.BeginBoundedCombo("tags", "Untagged");
            if (visible)
            {
                opened = true;
                for (int index = 0; index < itemCount; index++)
                    NativeImGui.Selectable($"Tag {index}");
                popupPosition = NativeImGui.GetWindowPos();
                popupSize = NativeImGui.GetWindowSize();
                scroll = new Vector2(NativeImGui.GetScrollMaxX(), NativeImGui.GetScrollMaxY());
                Widget.EndBoundedCombo();
            }
            else
            {
                controlMinimum = NativeImGui.GetItemRectMin();
                controlMaximum = NativeImGui.GetItemRectMax();
            }
            NativeImGui.End();
        };

        context.Frame(draw);
        context.Click((controlMinimum + controlMaximum) * 0.5f, 0, draw);
        for (int frame = 0; frame < 6; frame++)
            context.Frame(draw);

        Assert.True(opened);
        Assert.InRange(popupPosition.Y, controlMaximum.Y - 1f, controlMaximum.Y + 1f);
        Assert.True(popupPosition.X >= 20f);
        Assert.True(popupPosition.X + popupSize.X <= 421f);
        Assert.True(popupPosition.Y + popupSize.Y <= 381f);
        Assert.True(popupSize.Y <= 360f * 0.45f + 1f);
        Assert.Equal(0f, scroll.X);
        Assert.Equal(shouldScroll, scroll.Y > 0f);
    }

    [Theory]
    [InlineData(280f)]
    [InlineData(320f)]
    public void SelectorsNearWindowBottom_UseOnlyRemainingDownwardSpace(float verticalOffset)
    {
        using NativeContext context = new(1f);
        Vector2 controlMinimum = default;
        Vector2 controlMaximum = default;
        Vector2 popupPosition = default;
        Vector2 popupSize = default;
        bool opened = false;
        Action draw = () =>
        {
            BeginWindow("Bottom selector", new Vector2(400f, 360f));
            NativeImGui.SetCursorPosY(verticalOffset);
            NativeImGui.SetNextItemWidth(260f);
            if (Widget.BeginBoundedCombo("objects", "Selected object"))
            {
                opened = true;
                for (int index = 0; index < 100; index++)
                    NativeImGui.Selectable($"Object {index}");
                popupPosition = NativeImGui.GetWindowPos();
                popupSize = NativeImGui.GetWindowSize();
                Widget.EndBoundedCombo();
            }
            else
            {
                controlMinimum = NativeImGui.GetItemRectMin();
                controlMaximum = NativeImGui.GetItemRectMax();
            }
            NativeImGui.End();
        };

        context.Frame(draw);
        context.Click((controlMinimum + controlMaximum) * 0.5f, 0, draw);
        for (int frame = 0; frame < 6; frame++)
            context.Frame(draw);

        Assert.True(opened);
        Assert.InRange(popupPosition.Y, controlMaximum.Y - 1f, controlMaximum.Y + 1f);
        Assert.True(popupPosition.Y + popupSize.Y <= 381f);
    }

    [Theory]
    [InlineData(0.85f)]
    [InlineData(0.9f)]
    [InlineData(1f)]
    [InlineData(1.1f)]
    public void ContextMenu_SearchFillsItsActualContentWidthWithoutScrollbars(float zoom)
    {
        using NativeContext context = new(zoom);
        Vector2 controlMinimum = default;
        Vector2 controlMaximum = default;
        Vector2 scroll = default;
        float availableWidth = 0f;
        float searchWidth = 0f;
        bool opened = false;
        string query = string.Empty;
        Action draw = () =>
        {
            BeginWindow("Hierarchy context", new Vector2(500f, 360f));
            NativeImGui.InvisibleButton("Scene", new Vector2(200f, 30f));
            controlMinimum = NativeImGui.GetItemRectMin();
            controlMaximum = NativeImGui.GetItemRectMax();
            if (Widget.BeginContextMenu("scene-actions"))
            {
                opened = true;
                availableWidth = NativeImGui.GetContentRegionAvail().X;
                Widget.SearchInput("commands", "Search commands...", ref query);
                searchWidth = NativeImGui.GetItemRectSize().X;
                NativeImGui.MenuItem("Set Active Scene", string.Empty, false, true);
                NativeImGui.MenuItem("Create", string.Empty, false, true);
                NativeImGui.MenuItem("Rename", "F2", false, true);
                NativeImGui.MenuItem("Unload", "Delete", false, true);
                scroll = new Vector2(NativeImGui.GetScrollMaxX(), NativeImGui.GetScrollMaxY());
                Widget.EndContextMenu();
            }
            NativeImGui.End();
        };

        context.Frame(draw);
        context.Click((controlMinimum + controlMaximum) * 0.5f, 1, draw);
        for (int frame = 0; frame < 6; frame++)
            context.Frame(draw);

        Assert.True(opened);
        Assert.InRange(availableWidth - searchWidth, 0f, 1.1f);
        Assert.Equal(Vector2.Zero, scroll);
    }

    [Fact]
    public void OverlappingWindows_OnlyForegroundClickableTextReceivesPress()
    {
        using NativeContext context = new(1f);
        int backgroundPresses = 0;
        int foregroundPresses = 0;
        Vector2 hit = default;
        Action draw = () =>
        {
            BeginWindow("Background panel", new Vector2(400f, 300f));
            NativeImGui.SetCursorScreenPos(new Vector2(100f, 100f));
            if (Widget.ClickableText("close", "X", new Vector2(40f, 40f)))
                backgroundPresses++;
            NativeImGui.End();
            BeginWindow("Foreground shader", new Vector2(400f, 300f));
            NativeImGui.SetCursorScreenPos(new Vector2(100f, 100f));
            if (Widget.ClickableText("action", "Edit", new Vector2(40f, 40f)))
                foregroundPresses++;
            hit = (NativeImGui.GetItemRectMin() + NativeImGui.GetItemRectMax()) * 0.5f;
            NativeImGui.End();
        };

        context.Frame(draw);
        context.Frame(draw);
        context.Click(hit, 0, draw);

        Assert.Equal(0, backgroundPresses);
        Assert.Equal(1, foregroundPresses);
    }

    [Theory]
    [InlineData(true, 1f)]
    [InlineData(false, 1f)]
    [InlineData(true, 1.5f)]
    [InlineData(false, 1.5f)]
    public void TreeRows_ContentClickIsDeliveredOnce(
        bool isLeaf,
        float zoom
    ) {
        using NativeContext context = new(zoom);
        int presses = 0;
        Vector2 hit = default;
        Action draw = () =>
        {
            BeginWindow("Tree content", new Vector2(400f, 300f));
            var result = Widget.TreeNode(
                $"content-{isLeaf}-{zoom}",
                _ => Widget.IconText("O", "Selected object", false),
                new()
                {
                    isLeaf = isLeaf,
                    drawViewportOverlay = () =>
                    {
                        float rowY = NativeImGui.GetItemRectMin().Y;
                        NativeImGui.SetCursorScreenPos(new Vector2(360f, rowY));
                        _ = Widget.ClickableIcon("visibility", "V", "Visibility");
                    }
                });
            hit = new Vector2(result.contentMin.X + 80f, (result.min.Y + result.max.Y) * 0.5f);
            if (result.isClicked)
                presses++;
            if (result.isOpen)
                NativeImGui.TreePop();
            NativeImGui.End();
        };

        context.Frame(draw);
        context.Frame(draw);
        context.Click(hit, 0, draw, settleHover: false);

        Assert.Equal(1, presses);
    }

    [Fact]
    public void TreeRows_DisclosureClickExpandsWithoutSelectingContent()
    {
        using NativeContext context = new(1f);
        int presses = 0;
        bool opened = false;
        Vector2 hit = default;
        Action draw = () =>
        {
            BeginWindow("Tree disclosure", new Vector2(400f, 300f));
            var result = Widget.TreeNode(
                "disclosure",
                _ => NativeImGui.TextUnformatted("Parent"),
                new() { isLeaf = false });
            hit = new Vector2(
                result.contentMin.X - NativeImGui.GetTreeNodeToLabelSpacing() * 0.5f,
                (result.min.Y + result.max.Y) * 0.5f);
            opened = result.isOpen;
            if (result.isClicked)
                presses++;
            if (result.isOpen)
                NativeImGui.TreePop();
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
    public void TreeRows_OverlayConsumesItsOwnClick(bool settleHover)
    {
        using NativeContext context = new(1f);
        int rowPresses = 0;
        int overlayPresses = 0;
        Vector2 overlayHit = default;
        Action draw = () =>
        {
            BeginWindow("Tree overlay", new Vector2(400f, 300f));
            var result = Widget.TreeNode(
                $"overlay-{settleHover}",
                _ => NativeImGui.TextUnformatted("Object"),
                new()
                {
                    isLeaf = true,
                    drawViewportOverlay = () =>
                    {
                        float rowY = NativeImGui.GetItemRectMin().Y;
                        NativeImGui.SetCursorScreenPos(new Vector2(360f, rowY));
                        if (NativeImGui.SmallButton("Eye"))
                            overlayPresses++;
                        overlayHit = (NativeImGui.GetItemRectMin() + NativeImGui.GetItemRectMax()) * 0.5f;
                    }
                });
            if (result.isClicked)
                rowPresses++;
            NativeImGui.End();
        };

        context.Frame(draw);
        context.Frame(draw);
        context.Click(overlayHit, 0, draw, settleHover);

        Assert.Equal(1, overlayPresses);
        Assert.Equal(0, rowPresses);
    }

    [Fact]
    public void TreeRows_ContentDoubleClickExpandsOnce()
    {
        using NativeContext context = new(1f);
        int presses = 0;
        int doublePresses = 0;
        bool opened = false;
        Vector2 hit = default;
        Action draw = () =>
        {
            BeginWindow("Tree double click", new Vector2(400f, 300f));
            var result = Widget.TreeNode(
                "double-click",
                _ => NativeImGui.TextUnformatted("Parent"),
                new() { isLeaf = false });
            hit = new Vector2(result.contentMin.X + 80f, (result.min.Y + result.max.Y) * 0.5f);
            opened = result.isOpen;
            if (result.isClicked)
                presses++;
            if (result.isDoubleClicked)
                doublePresses++;
            if (result.isOpen)
                NativeImGui.TreePop();
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

    private static void BeginWindow(
        string name,
        Vector2 size
    ) {
        NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), (int)(ImGuiCond.Always), default(Vector2));
        NativeImGui.SetNextWindowSize(size, (int)(ImGuiCond.Always));
        using NativeStringLease nativeTestText0 = Utf8Codec.EncodeNullTerminated(name, allowsNull: false);
        NativeImGui.Raw.Begin((sbyte*)nativeTestText0.pointer, null, (int)(ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoSavedSettings));
    }

    private sealed class NativeContext : IDisposable
    {
        private readonly ImGuiNativeContext* m_context;

        public NativeContext(float zoom)
        {
            m_context = NativeImGui.Raw.CreateContext(null);
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(1024f, 768f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            io->IniFilename = null;
            Widget.style.SetZoom(zoom);
            Widget.SetupStyle();
        }

        public void Frame(Action draw)
        {
            NativeImGui.NewFrame();
            draw();
            NativeImGui.Render();
        }

        public void Click(
            Vector2 position,
            int button,
            Action draw,
            bool settleHover = true
        ) {
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            NativeImGui.Raw.AddMousePosEvent(io, position.X, position.Y);
            if (settleHover)
                Frame(draw);
            NativeImGui.Raw.AddMouseButtonEvent(io, button, (true ? (byte)1 : (byte)0));
            Frame(draw);
            NativeImGui.Raw.AddMouseButtonEvent(io, button, (false ? (byte)1 : (byte)0));
            Frame(draw);
        }

        public void Dispose()
        {
            NativeImGui.Raw.DestroyContext(m_context);
            Widget.style.SetZoom(1f);
        }
    }
}
