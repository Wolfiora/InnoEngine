using ImGuiWindowFlags = Inno.Editor.ImGui.ImGuiWindowFlags;
using ImGuiMouseButton = Inno.Editor.ImGui.ImGuiMouseButton;
using ImGuiCond = Inno.Editor.ImGui.ImGuiCond;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System.Runtime.CompilerServices;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using BGCS.Runtime;
using System;
using System.Numerics;
using Inno.Native.ImGui;
using Xunit;
using NativeImGui = Inno.Native.ImGui.ImGui;
using Widget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using EditorImGui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.ImGui.Tests;
public sealed unsafe class WidgetLayoutTests
{
    [Fact]
    public void PackedColorConversionMatchesNativeSaturationRoundingAndChannelOrder()
    {
        using (NativeContext context = new(1f))
        {
            context.Frame(() =>
            {
                BeginWindow("Color format contract", new(400f, 300f));
                try
                {
                    float[] boundaries = [float.NegativeInfinity, -1f, -0f, 0f, 0.5f, 1f, 2f, float.PositiveInfinity, float.NaN];
                    foreach (float value in boundaries)
                    {
                        Verify(new(value, 0f, 0f, 0f));
                        Verify(new(0f, value, 0f, 0f));
                        Verify(new(0f, 0f, value, 0f));
                        Verify(new(0f, 0f, 0f, value));
                    }
                    for (int channel = 0; channel < 255; channel++)
                    {
                        float half = (channel + 0.5f) / 255f;
                        Verify(new(MathF.BitDecrement(half), half, MathF.BitIncrement(half), 1f));
                    }
                    var random = new Random(0x2DCA);
                    for (int index = 0; index < 4096; index++)
                        Verify(new(random.NextSingle() * 3f - 1f, random.NextSingle() * 3f - 1f,
                            random.NextSingle() * 3f - 1f, random.NextSingle() * 3f - 1f));
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
        using NativeContext context = new(1f);
        var widgets = EditorUI.current.widgets;
        for (int frame = 0; frame < 2; frame++)
        {
            context.Frame(() =>
            {
                widgets.SetupStyle();
                BeginWindow("Stable style input", new(400f, 300f));
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
        using NativeContext context = new(1f);
        var widgets = EditorUI.current.widgets;
        context.Frame(() =>
        {
            widgets.SetupStyle();
            BeginWindow("Suspended style owner", new(400f, 300f));
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
        using NativeContext context = new(1f);
        Assert.Throws<InvalidOperationException>(() => EditorImGui.GetClipBounds());
        Assert.Throws<InvalidOperationException>(() => EditorImGui.GetWindowBounds());
        context.Frame(() =>
        {
            BeginWindow("Geometry owner", new(400f, 300f));
            try
            {
                var clip = EditorImGui.GetClipBounds();
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

    [Fact]
    public void FlattenedRowsRetainExpansionWithoutOwningNativeTreeScopes()
    {
        using NativeContext context = new(1f);
        for (int frame = 0; frame < 3; frame++)
        {
            context.Frame(() =>
            {
                BeginWindow("Flattened tree", new(400f, 300f));
                try
                {
                    var widgets = EditorUI.current.widgets;
                    widgets.SetTreeNodeOpen("root", true);
                    ulong revision = widgets.treeExpansionRevision;
                    widgets.SetTreeNodeOpen("root", true);
                    Assert.Equal(revision, widgets.treeExpansionRevision);
                    int nativeDepth = NativeImGui.Raw.GetCurrentWindow()->DC.TreeDepth;
                    var root = widgets.TreeNode("root", _ => NativeImGui.TextUnformatted("Root"), new() { depth = 0 });
                    Assert.True(root.isOpen);
                    Assert.True(widgets.IsTreeNodeOpen("root"));
                    Assert.Equal(nativeDepth, NativeImGui.Raw.GetCurrentWindow()->DC.TreeDepth);
                    var child = widgets.TreeNode("child", _ => NativeImGui.TextUnformatted("Deep child"), new() { depth = 7, isLeaf = true });
                    Assert.True(child.contentMin.X > root.contentMin.X);
                    var failure = new InvalidOperationException("Rejected flat content");
                    Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => widgets.TreeNode("failed", _ => throw failure, new() { depth = 2 })));
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
        using NativeContext context = new(1f);
        for (int frame = 0; frame < 3; frame++)
        {
            context.Frame(() =>
            {
                BeginWindow("Large tree", new(400f, 300f));
                int drawn = 0;
                Action<Inno.Editor.ImGui.ImGuiWidget.TreeNodeDrawContext> draw = _ =>
                {
                    drawn++;
                    NativeImGui.TextUnformatted("Visible content");
                };
                for (int row = 0; row < 10_000; row++)
                    _ = EditorUI.current.widgets.TreeNode("row-" + row, draw, new() { isLeaf = true });
                Assert.InRange(drawn, 1, 64);
                if (frame == 1)
                    NativeImGui.SetScrollY(100_000f);
                NativeImGui.End();
            });
        }
    }

    [Fact]
    public void FixedRowsRetainExplicitRowsAndBalanceTheirNativeOwner()
    {
        using NativeContext context = new(1f);
        context.Frame(() =>
        {
            BeginWindow("Clipped rows", new(400f, 300f));
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
                    NativeImGui.Dummy(new(100f, NativeImGui.GetTextLineHeight()));
            }, [9999]);
            Assert.InRange(submitted, 1, 64);
            Assert.True(preserved);
            NativeImGui.End();
        });
    }

    [Fact]
    public void FixedRowFailureReleasesTheClipperBeforeTheNextFrame()
    {
        using NativeContext context = new(1f);
        for (int frame = 0; frame < 2; frame++)
        {
            context.Frame(() =>
            {
                BeginWindow("Clipper failure", new(400f, 300f));
                int before = NativeImGui.Raw.GetCurrentContext()->ClipperTempDataStacked;
                var failure = new InvalidOperationException("Row extension rejected drawing.");
                Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => EditorUI.current.widgets.ClippedRows(
                    10_000, NativeImGui.GetTextLineHeightWithSpacing(), 0, (
                        state,
                        start,
                        end
                    ) => throw failure)));
                Assert.Equal(before, NativeImGui.Raw.GetCurrentContext()->ClipperTempDataStacked);
                NativeImGui.End();
            });
        }
    }

    [Theory]
    [InlineData(0.75f)]
    [InlineData(1f)]
    [InlineData(1.5f)]
    public void TrendUsesOneBoundedRegionAndFiniteGeometry(float zoom)
    {
        using NativeContext context = new(zoom);
        context.Frame(() =>
        {
            BeginWindow("Trend bounds", new Vector2(400f, 400f));
            var widgets = EditorUI.current.widgets;
            widgets.Trend("Finite extremes", new[] { float.MinValue, 0f, float.MaxValue }, offset: 1);
            Assert.Equal(widgets.style.trendHeight, NativeImGui.GetItemRectSize().Y, 3);
            Assert.True(NativeImGui.GetItemRectSize().X <= 400f);
            var draw = NativeImGui.Raw.GetWindowDrawList();
            for (int index = 0; index < draw->VtxBuffer.Size; index++)
            {
                Vector2 point = Unsafe.BitCast<NativeVector2, Vector2>(draw->VtxBuffer.Data[index].Pos);
                Assert.True(float.IsFinite(point.X) && float.IsFinite(point.Y));
            }
            widgets.Trend("Empty", ReadOnlySpan<float>.Empty);
            Assert.Throws<ArgumentOutOfRangeException>(() => widgets.Trend("Offset", new[] { 1f }, offset: 1));
            Assert.Throws<ArgumentException>(() => widgets.Trend("Invalid", new[] { float.NaN }));
            NativeImGui.End();
        });
    }

    [Fact]
    public void ListNavigationUsesVisibleIndicesAndRespectsTextAndPopupOwnership()
    {
        string root = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "InnoListNavigation", Guid.NewGuid().ToString("N"));
        System.IO.Directory.CreateDirectory(root);
        try
        {
            var diagnostics = new Inno.Core.Diagnostics.DiagnosticHub();
            using IDisposable diagnosticScope = diagnostics.EnterScope();
            using var modules = new Inno.Extensibility.Modules.ModuleHost(new()
            {
                catalogSource = new Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource(typeof(WidgetLayoutTests).Assembly)
            });
            using var types = new Inno.Extensibility.Types.TypeCatalog(modules, new Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource());
            using var logs = new Inno.Core.Logging.LogRouter();
            using var runtime = new Inno.Editor.Interactions.EditorInteractionRuntime(
                root,
                types,
                logs,
                new Inno.Editor.Core.EditorKeyboardPolicy(Inno.Core.Input.KeyModifier.Control, "Super"));
            runtime.Start();
            using NativeContext context = new(1f);
            NativeImGui.Raw.GetIO()->ConfigFlags |= (int)Inno.Native.ImGui.ImGuiConfigFlags.NavEnableKeyboard;
            int focused = 4;
            bool textInput = false;
            bool popup = false;
            string query = string.Empty;
            Inno.Editor.ImGui.ImGuiWidget.ListNavigationResult observed = default;
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
            var io = NativeImGui.Raw.GetIO();
            NativeImGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.PageDown, 1);
            context.Frame(draw);
            Assert.Equal(14, focused);
            Assert.True(observed.selectionRequested);
            NativeImGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.PageDown, 0);
            context.Frame(draw);
            NativeImGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.End, 1);
            context.Frame(draw);
            Assert.Equal(99, focused);
            NativeImGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.End, 0);
            context.Frame(draw);
            textInput = true;
            context.Frame(draw);
            context.Frame(draw);
            NativeImGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.Home, 1);
            context.Frame(draw);
            Assert.Equal(99, focused);
            Assert.False(observed.selectionRequested);
            NativeImGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.Home, 0);
            context.Frame(draw);
            textInput = false;
            popup = true;
            context.Frame(draw);
            NativeImGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.UpArrow, 1);
            context.Frame(draw);
            Assert.Equal(99, focused);
            Assert.False(observed.selectionRequested);
            NativeImGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.UpArrow, 0);
            context.Frame(draw);
        }
        finally
        {
            System.IO.Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("card-leading")]
    [InlineData("card-trailing")]
    [InlineData("card-menu")]
    [InlineData("tree-content")]
    [InlineData("tree-overlay")]
    public void ExtensionDrawingFailureRestoresTheEnclosingWindow(string failure)
    {
        using NativeContext context = new(1f);
        for (int frame = 0; frame < 2; frame++)
        {
            context.Frame(() =>
            {
                BeginWindow("Failure owner", new Vector2(400f, 300f));
                nint window = (nint)NativeImGui.Raw.GetCurrentWindow();
                int depth = NativeImGui.Raw.GetCurrentWindow()->DC.TreeDepth;
                Vector2 padding = Unsafe.BitCast<NativeVector2, Vector2>(NativeImGui.Raw.GetStyle()->FramePadding);
                var widgets = EditorUI.current.widgets;
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
                            drawContextMenu: failure == "card-menu" ? rejected : null);
                    }
                    else
                    {
                        widgets.SetNextTreeNodeOpen(true);
                        _ = widgets.TreeNode("failed-tree", _ =>
                        {
                            if (failure == "tree-content")
                                rejected();
                            EditorImGui.Text("Extension");
                        }, new() { drawViewportOverlay = failure == "tree-overlay" ? rejected : null });
                    }
                });
                Assert.Equal(window, (nint)NativeImGui.Raw.GetCurrentWindow());
                Assert.Equal(depth, NativeImGui.Raw.GetCurrentWindow()->DC.TreeDepth);
                Assert.Equal(padding, Unsafe.BitCast<NativeVector2, Vector2>(NativeImGui.Raw.GetStyle()->FramePadding));
                widgets.SetNextTreeNodeOpen(true);
                var next = widgets.TreeNode("next-tree", _ => EditorImGui.Text("Next valid row"), new());
                Assert.True(next.isOpen);
                NativeImGui.TreePop();
                EditorImGui.GetDrawList().Line(new(30f, 30f), new(60f, 60f), uint.MaxValue);
                NativeImGui.End();
            });
        }
    }

    [Fact]
    public void DrawingBorrowRejectsAnotherWindowOrFrame()
    {
        using NativeContext context = new(1f);
        Inno.Editor.ImGui.EditorDrawList borrow = default;
        context.Frame(() =>
        {
            BeginWindow("Drawing owner", new Vector2(400f, 300f));
            borrow = EditorImGui.GetDrawList();
            borrow.Line(new(30f, 30f), new(60f, 60f), uint.MaxValue);
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
        Assert.Throws<InvalidOperationException>(() => default(Inno.Editor.ImGui.EditorDrawList).Line(Vector2.Zero, Vector2.One, 0));
    }

    [Fact]
    public void BoundedTextControlsDoNotAllocateOneBufferAndEncoderPerSubmission()
    {
        using NativeContext context = new(1f);
        string value = "Unicode: 漢字 😀";
        long allocated = 0;
        Action draw = () =>
        {
            BeginWindow("Text allocation", new Vector2(400f, 300f));
            long before = GC.GetAllocatedBytesForCurrentThread();
            for (int index = 0; index < 128; index++)
            {
                EditorImGui.PushId(index);
                _ = EditorImGui.InputText("Value", ref value, 1024);
                EditorImGui.PopId();
            }

            allocated = GC.GetAllocatedBytesForCurrentThread() - before;
            NativeImGui.End();
        };
        context.Frame(draw);
        context.Frame(draw);
        Assert.Equal("Unicode: 漢字 😀", value);
        Assert.True(allocated < 128L * 512, $"Unexpected steady-state allocation: {allocated} bytes for 128 fields.");
    }

    [Fact]
    public void TimelineWorkspace_UsesOneBoundedChildScrollOwner()
    {
        using NativeContext context = new(1f);
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
                                    EditorImGui.Text("Frame " + index);
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
    public void TimelinePointer_UsesCumulativeUiDragUntilRelease()
    {
        using NativeContext context = new(1f);
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
            drag = EditorImGui.GetMouseDragDelta(ImGuiMouseButton.Left, 0f);
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
                EditorUI.current.widgets.PropertyRow("persistence", () =>
                {
                    labelWidth = NativeImGui.GetContentRegionAvail().X;
                    EditorUI.current.widgets.PropertyLabel("Persistent data directory for this exported application");
                    labelSize = NativeImGui.GetItemRectSize();
                }, () =>
                {
                    valueWidth = NativeImGui.GetContentRegionAvail().X;
                    string value = "Application data";
                    EditorUI.current.widgets.SearchInput("directory", "Directory", ref value);
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
            bool visible = EditorUI.current.widgets.BeginBoundedCombo("tags", "Untagged");
            if (visible)
            {
                opened = true;
                for (int index = 0; index < itemCount; index++)
                    NativeImGui.Selectable($"Tag {index}");
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
            if (EditorUI.current.widgets.BeginBoundedCombo("objects", "Selected object"))
            {
                opened = true;
                for (int index = 0; index < 100; index++)
                    NativeImGui.Selectable($"Object {index}");
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
            if (EditorUI.current.widgets.ClickableText("close", "X", new Vector2(40f, 40f)))
                backgroundPresses++;
            NativeImGui.End();
            BeginWindow("Foreground shader", new Vector2(400f, 300f));
            NativeImGui.SetCursorScreenPos(new Vector2(100f, 100f));
            if (EditorUI.current.widgets.ClickableText("action", "Edit", new Vector2(40f, 40f)))
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
            var result = EditorUI.current.widgets.TreeNode($"content-{isLeaf}-{zoom}", _ => EditorUI.current.widgets.IconText(
                "O",
                "Selected object",
                false
            ), new() { isLeaf = isLeaf, drawViewportOverlay = () =>
            {
                float rowY = NativeImGui.GetItemRectMin().Y;
                NativeImGui.SetCursorScreenPos(new Vector2(360f, rowY));
                _ = EditorUI.current.widgets.ClickableIcon("visibility", "V", "Visibility");
            } });
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
            var result = EditorUI.current.widgets.TreeNode("disclosure", _ => NativeImGui.TextUnformatted("Parent"), new() { isLeaf = false });
            hit = new Vector2(result.contentMin.X - NativeImGui.GetTreeNodeToLabelSpacing() * 0.5f, (result.min.Y + result.max.Y) * 0.5f);
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
            var result = EditorUI.current.widgets.TreeNode($"overlay-{settleHover}", _ => NativeImGui.TextUnformatted("Object"), new() { isLeaf = true, drawViewportOverlay = () =>
            {
                float rowY = NativeImGui.GetItemRectMin().Y;
                NativeImGui.SetCursorScreenPos(new Vector2(360f, rowY));
                if (NativeImGui.SmallButton("Eye"))
                    overlayPresses++;
                overlayHit = (NativeImGui.GetItemRectMin() + NativeImGui.GetItemRectMax()) * 0.5f;
            } });
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
            var result = EditorUI.current.widgets.TreeNode("double-click", _ => NativeImGui.TextUnformatted("Parent"), new() { isLeaf = false });
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

    [Fact]
    public void MixedValueRestoresOuterStateAfterNestedGroupsAndFailures()
    {
        using NativeContext context = new(1f);
        context.Frame(() =>
        {
            BeginWindow("Mixed scope", new Vector2(400f, 300f));
            var widgets = EditorUI.current.widgets;
            Assert.False(widgets.isMixedValue);
            widgets.MixedValue(true, () =>
            {
                Assert.True(widgets.isMixedValue);
                widgets.MixedValue(false, () => Assert.True(widgets.isMixedValue));
                Assert.True(widgets.isMixedValue);
            });
            Assert.False(widgets.isMixedValue);
            Assert.Throws<InvalidOperationException>(() => widgets.MixedValue(true, () => throw new InvalidOperationException("rejected draw")));
            Assert.False(widgets.isMixedValue);
            NativeImGui.End();
        });
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static EditorStyleMetrics RetireStyleOwner(out WeakReference owner)
    {
        using var session = new EditorUiSession();
        owner = new WeakReference(session);
        return session.widgets.style;
    }

    private static void BeginWindow(
        string name,
        Vector2 size
    ) {
        NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), (int)(ImGuiCond.Always), default(Vector2));
        NativeImGui.SetNextWindowSize(size, (int)(ImGuiCond.Always));
        using NativeStringLease nativeTestText0 = Utf8Codec.EncodeNullTerminated(name, allowsNull: false);
        NativeImGui.Raw.Begin(
            (sbyte*)nativeTestText0.pointer,
            null,
            (int)(ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoSavedSettings)
        );
    }

    private sealed class NativeContext : IDisposable
    {
        private readonly ImGuiNativeContext* m_context;
        private readonly Inno.Editor.ImGui.EditorUiSession m_ui = new();
        private readonly IDisposable m_uiScope;
        public NativeContext(float zoom)
        {
            m_context = NativeImGui.Raw.CreateContext(null);
            m_uiScope = m_ui.Enter();
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(1024f, 768f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            io->IniFilename = null;
            EditorUI.current.widgets.style.SetZoom(zoom);
            EditorUI.current.widgets.SetupStyle();
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
            m_uiScope.Dispose();
            m_ui.Dispose();
            NativeImGui.Raw.DestroyContext(m_context);
        }
    }
}
