using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Core.Diagnostics;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Interactions;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;

using EditorImGui = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUISession;
using GuiMouseButton = Inno.Editor.ImGui.ImGuiMouseButton;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeKey = Inno.Native.ImGui.ImGuiKey;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using Widget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;

namespace Inno.Editor.ImGui.Native.Tests;

public sealed unsafe class WidgetLayoutTests : WidgetLayoutFixture
{
    [Fact]
    public void PackedColorConversionMatchesNativeSaturationRoundingAndChannelOrder()
    {
        using (var context = new NativeContext(1f))
        {
            context.Frame(() =>
            {
                BeginWindow("Color format contract", new Vector2(400f, 300f));
                try
                {
                    float[] boundaries = [float.NegativeInfinity, -1f, -0f, 0f, 0.5f, 1f, 2f, float.PositiveInfinity, float.NaN];
                    foreach (float value in boundaries)
                    {
                        Verify(new Vector4(value, 0f, 0f, 0f));
                        Verify(new Vector4(0f, value, 0f, 0f));
                        Verify(new Vector4(0f, 0f, value, 0f));
                        Verify(new Vector4(0f, 0f, 0f, value));
                    }
                    for (int channel = 0; channel < 255; channel++)
                    {
                        float half = (channel + 0.5f) / 255f;
                        Verify(new Vector4(MathF.BitDecrement(half), half, MathF.BitIncrement(half), 1f));
                    }
                    var random = new Random(0x2DCA);
                    for (int index = 0; index < 4096; index++)
                    {
                        Verify(
                            new Vector4(
                                random.NextSingle() * 3f - 1f,
                                random.NextSingle() * 3f - 1f,
                                random.NextSingle() * 3f - 1f,
                                random.NextSingle() * 3f - 1f
                            )
                        );
                    }
                }
                finally
                {
                    NativeImGui.End();
                }
            });
        }

        Assert.Throws<InvalidOperationException>(() => EditorImGui.ToPackedColor(Vector4.One));
        static void Verify(Vector4 value) => Assert.Equal(NativeImGui.ColorConvertFloat4ToU32(value), EditorImGui.ToPackedColor(value));
    }

    [Theory]
    [InlineData(0.75f)]
    [InlineData(1f)]
    [InlineData(1.5f)]
    public void TrendUsesOneBoundedRegionAndFiniteGeometry(float zoom)
    {
        using var context = new NativeContext(zoom);
        context.Frame(() =>
        {
            BeginWindow("Trend bounds", new Vector2(400f, 400f));
            Widget widgets = EditorUI.current.widgets;
            widgets.Trend("Finite extremes", [float.MinValue, 0f, float.MaxValue], offset: 1);
            Assert.Equal(widgets.style.trendHeight, NativeImGui.GetItemRectSize().Y, 3);
            Assert.True(NativeImGui.GetItemRectSize().X <= 400f);
            ImDrawList* draw = NativeImGui.Raw.GetWindowDrawList();
            for (int index = 0; index < draw->VtxBuffer.Size; index++)
            {
                Vector2 point = Unsafe.BitCast<NativeVector2, Vector2>(draw->VtxBuffer.Data[index].Pos);
                Assert.True(float.IsFinite(point.X) && float.IsFinite(point.Y));
            }
            widgets.Trend("Empty", []);
            Assert.Throws<ArgumentOutOfRangeException>(() => widgets.Trend("Offset", [1f], offset: 1));
            Assert.Throws<ArgumentException>(() => widgets.Trend("Invalid", [float.NaN]));
            NativeImGui.End();
        });
    }

    [Fact]
    public void ListNavigationUsesVisibleIndicesAndRespectsTextAndPopupOwnership()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoListNavigation", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            var diagnostics = new DiagnosticHub();
            using IDisposable diagnosticScope = diagnostics.EnterScope();
            using var modules = new ModuleHost(new ModuleHostOptions()
            {
                catalogSource = new DotNetAssemblyCatalogSource(typeof(WidgetLayoutTests).Assembly)
            });
            using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
            using var logs = new LogRouter();
            using var runtime = new EditorInteractionRuntime(
                root,
                types,
                logs,
                new EditorKeyboardPolicy(KeyModifier.Control, "Super")
            );
            runtime.Start();
            using var context = new NativeContext(1f);
            NativeImGui.Raw.GetIO()->ConfigFlags |= (int)ImGuiConfigFlags.NavEnableKeyboard;
            int focused = 4;
            bool textInput = false;
            bool popup = false;
            string query = string.Empty;
            ListNavigationResult observed = default;
            Action draw = () =>
            {
                BeginWindow("List navigation", new Vector2(500f, 400f));
                if (textInput)
                {
                    EditorImGui.FocusNextItem();
                    _ = EditorUI.current.widgets.SearchInput("keyboard-query", "Search", ref query, 1024);
                }
                if (popup)
                {
                    NativeImGui.OpenPopup("Input owner");
                    if (NativeImGui.BeginPopup("Input owner"))
                    {
                        NativeImGui.TextUnformatted("Exclusive popup input");
                        NativeImGui.EndPopup();
                    }
                }
                observed = EditorUI.current.widgets.NavigateList(runtime.interactions, 100, focused, pageSize: 10);
                focused = observed.focusedIndex;
                NativeImGui.End();
            };
            context.Frame(draw);
            context.Frame(draw);
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            NativeImGui.Raw.AddKeyEvent(io, NativeKey.PageDown, 1);
            context.Frame(draw);
            Assert.Equal(14, focused);
            Assert.True(observed.selectionRequested);
            NativeImGui.Raw.AddKeyEvent(io, NativeKey.PageDown, 0);
            context.Frame(draw);
            NativeImGui.Raw.AddKeyEvent(io, NativeKey.End, 1);
            context.Frame(draw);
            Assert.Equal(99, focused);
            NativeImGui.Raw.AddKeyEvent(io, NativeKey.End, 0);
            context.Frame(draw);
            textInput = true;
            context.Frame(draw);
            context.Frame(draw);
            NativeImGui.Raw.AddKeyEvent(io, NativeKey.Home, 1);
            context.Frame(draw);
            Assert.Equal(99, focused);
            Assert.False(observed.selectionRequested);
            NativeImGui.Raw.AddKeyEvent(io, NativeKey.Home, 0);
            context.Frame(draw);
            textInput = false;
            popup = true;
            context.Frame(draw);
            NativeImGui.Raw.AddKeyEvent(io, NativeKey.UpArrow, 1);
            context.Frame(draw);
            Assert.Equal(99, focused);
            Assert.False(observed.selectionRequested);
            NativeImGui.Raw.AddKeyEvent(io, NativeKey.UpArrow, 0);
            context.Frame(draw);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void TextInputPreservesUnicodeOnSubmission()
    {
        using var context = new NativeContext(1f);
        string value = "Unicode: 漢字 😀";
        context.Frame(() =>
        {
            BeginWindow("Unicode text", new Vector2(400f, 300f));
            _ = EditorImGui.InputText("Value", ref value, 1024);
            NativeImGui.End();
        });
        Assert.Equal("Unicode: 漢字 😀", value);
    }

    [Fact]
    public void TimelineWorkspaceUsesOneBoundedChildScrollOwner()
    {
        using var context = new NativeContext(1f);
        float childScroll = 0f;
        float parentScroll = 0f;
        for (int frame = 0; frame < 4; frame++)
        {
            context.Frame(() =>
            {
                BeginWindow("Timeline workspace", new Vector2(400f, 240f));
                bool visible = EditorImGui.BeginChild("workspace");
                try
                {
                    if (visible)
                    {
                        EditorUI.current.widgets.SectionLayout(() =>
                        {
                            if (EditorUI.current.widgets.SectionHeader("Frames"))
                            {
                                for (int index = 0; index < 40; index++)
                                {
                                    EditorImGui.Text("Frame " + index);
                                }
                            }
                        });
                    }

                    childScroll = NativeImGui.GetScrollMaxY();
                }
                finally
                {
                    EditorImGui.EndChild();
                }

                parentScroll = NativeImGui.GetScrollMaxY();
                NativeImGui.End();
            });
        }

        Assert.True(childScroll > 200f);
        Assert.Equal(0f, parentScroll);
    }

    [Fact]
    public void TimelinePointerUsesCumulativeUIDragUntilRelease()
    {
        using var context = new NativeContext(1f);
        Vector2 hit = default;
        Vector2 drag = default;
        bool held = false;
        bool active = false;
        Action draw = () =>
        {
            BeginWindow("Timeline pointer", new Vector2(400f, 260f));
            EditorImGui.Button("Frame", new Vector2(180f, 60f));
            hit = (EditorImGui.GetItemMinimum() + EditorImGui.GetItemMaximum()) * 0.5f;
            active = EditorImGui.IsItemActive();
            held = EditorImGui.IsMouseButtonDown();
            drag = EditorImGui.GetMouseDragDelta(GuiMouseButton.Left, 0f);
            NativeImGui.End();
        };
        context.Frame(draw);
        context.Frame(draw);
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        NativeImGui.Raw.AddMousePosEvent(io, hit.X, hit.Y);
        context.Frame(draw);
        NativeImGui.Raw.AddMouseButtonEvent(io, 0, 1);
        context.Frame(draw);
        Assert.True(active);
        Assert.True(held);
        NativeImGui.Raw.AddMousePosEvent(io, hit.X + 30f, hit.Y + 10f);
        context.Frame(draw);
        Assert.Equal(new Vector2(30f, 10f), drag);
        context.Frame(draw);
        Assert.Equal(new Vector2(30f, 10f), drag);
        NativeImGui.Raw.AddMouseButtonEvent(io, 0, 0);
        context.Frame(draw);
        Assert.False(held);
        Assert.False(active);
    }

    [Theory]
    [InlineData(0.75f)]
    [InlineData(1f)]
    [InlineData(1.5f)]
    public void PropertyRowsWrapWithinTwoToThreeColumnsWithoutScrollbars(float zoom)
    {
        using var context = new NativeContext(zoom);
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
                EditorUI.current.widgets.PropertyRow(
                    "persistence",
                    () =>
                    {
                        labelWidth = NativeImGui.GetContentRegionAvail().X;
                        EditorUI.current.widgets.PropertyLabel("Persistent data directory for this exported application");
                        labelSize = NativeImGui.GetItemRectSize();
                    },
                    () =>
                    {
                        valueWidth = NativeImGui.GetContentRegionAvail().X;
                        string value = "Application data";
                        EditorUI.current.widgets.SearchInput("directory", "Directory", ref value);
                    }
                );
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
    public void BoundedSelectorsOpenDownwardAndScrollOnlyWhenRequired(
        int itemCount,
        bool shouldScroll,
        float zoom
    ) {
        using var context = new NativeContext(zoom);
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
            bool visible = EditorUI.current.widgets.BeginBoundedCombo("tags", "Untagged");
            if (visible)
            {
                opened = true;
                for (int index = 0; index < itemCount; index++)
                {
                    NativeImGui.Selectable($"Tag {index}");
                }
                popupPosition = NativeImGui.GetWindowPos();
                popupSize = NativeImGui.GetWindowSize();
                scroll = new Vector2(NativeImGui.GetScrollMaxX(), NativeImGui.GetScrollMaxY());
                EditorUI.current.widgets.EndBoundedCombo();
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
        {
            context.Frame(draw);
        }
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
    public void SelectorsNearWindowBottomUseOnlyRemainingDownwardSpace(float verticalOffset)
    {
        using var context = new NativeContext(1f);
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
            if (EditorUI.current.widgets.BeginBoundedCombo("objects", "Selected object"))
            {
                opened = true;
                for (int index = 0; index < 100; index++)
                {
                    NativeImGui.Selectable($"Object {index}");
                }
                popupPosition = NativeImGui.GetWindowPos();
                popupSize = NativeImGui.GetWindowSize();
                EditorUI.current.widgets.EndBoundedCombo();
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
        {
            context.Frame(draw);
        }
        Assert.True(opened);
        Assert.InRange(popupPosition.Y, controlMaximum.Y - 1f, controlMaximum.Y + 1f);
        Assert.True(popupPosition.Y + popupSize.Y <= 381f);
    }

    [Theory]
    [InlineData(0.85f)]
    [InlineData(0.9f)]
    [InlineData(1f)]
    [InlineData(1.1f)]
    public void ContextMenuSearchFillsItsActualContentWidthWithoutScrollbars(float zoom)
    {
        using var context = new NativeContext(zoom);
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
            if (EditorUI.current.widgets.BeginContextMenu("scene-actions"))
            {
                opened = true;
                availableWidth = NativeImGui.GetContentRegionAvail().X;
                EditorUI.current.widgets.SearchInput("commands", "Search commands...", ref query);
                searchWidth = NativeImGui.GetItemRectSize().X;
                NativeImGui.MenuItem("Set Active Scene", string.Empty, false, true);
                NativeImGui.MenuItem("Create", string.Empty, false, true);
                NativeImGui.MenuItem("Rename", "F2", false, true);
                NativeImGui.MenuItem("Unload", "Delete", false, true);
                scroll = new Vector2(NativeImGui.GetScrollMaxX(), NativeImGui.GetScrollMaxY());
                EditorUI.current.widgets.EndContextMenu();
            }

            NativeImGui.End();
        };
        context.Frame(draw);
        context.Click((controlMinimum + controlMaximum) * 0.5f, 1, draw);
        for (int frame = 0; frame < 6; frame++)
        {
            context.Frame(draw);
        }
        Assert.True(opened);
        Assert.InRange(availableWidth - searchWidth, 0f, 1.1f);
        Assert.Equal(Vector2.Zero, scroll);
    }

    [Fact]
    public void OverlappingWindowsOnlyForegroundClickableTextReceivesPress()
    {
        using var context = new NativeContext(1f);
        int backgroundPresses = 0;
        int foregroundPresses = 0;
        Vector2 hit = default;
        Action draw = () =>
        {
            BeginWindow("Background panel", new Vector2(400f, 300f));
            NativeImGui.SetCursorScreenPos(new Vector2(100f, 100f));
            if (EditorUI.current.widgets.ClickableText("close", "X", new Vector2(40f, 40f)))
            {
                backgroundPresses++;
            }
            NativeImGui.End();
            BeginWindow("Foreground shader", new Vector2(400f, 300f));
            NativeImGui.SetCursorScreenPos(new Vector2(100f, 100f));
            if (EditorUI.current.widgets.ClickableText("action", "Edit", new Vector2(40f, 40f)))
            {
                foregroundPresses++;
            }
            hit = (NativeImGui.GetItemRectMin() + NativeImGui.GetItemRectMax()) * 0.5f;
            NativeImGui.End();
        };
        context.Frame(draw);
        context.Frame(draw);
        context.Click(hit, 0, draw);
        Assert.Equal(0, backgroundPresses);
        Assert.Equal(1, foregroundPresses);
    }
}
