using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using BGCS.Runtime;
using Xunit;
using Inno.Editor.Assets.ImGui;
using Inno.Native.ImGui;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using GuiChildFlags = Inno.Editor.ImGui.ImGuiChildFlags;
using GuiCol = Inno.Editor.ImGui.ImGuiCol;
using GuiCond = Inno.Editor.ImGui.ImGuiCond;
using GuiMouseButton = Inno.Editor.ImGui.ImGuiMouseButton;
using GuiStyleVar = Inno.Editor.ImGui.ImGuiStyleVar;
using GuiTableRowFlags = Inno.Editor.ImGui.ImGuiTableRowFlags;
using GuiTreeNodeOptions = Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions;
using GuiTreeNodeResult = Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult;
using GuiWindowFlags = Inno.Editor.ImGui.ImGuiWindowFlags;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.ImGui.Native.Tests;

public sealed unsafe class EditorStyleMetricsTests
{
    [Fact]
    public void BrowserLayoutCapturesTheSharedScaleWithoutRetainingMutableMetrics()
    {
        var metrics = new EditorStyleMetrics();
        var browser = new AssetBrowserStyle(metrics);
        Assert.Equal(32f, browser.gridMinimumCellSize);
        metrics.SetZoom(1.5f);
        Assert.Equal(32f, browser.gridMinimumCellSize);
        var scaled = new AssetBrowserStyle(metrics);
        Assert.Equal(48f, scaled.gridMinimumCellSize);
        Assert.Equal(browser.gridDefaultScale, scaled.gridDefaultScale);
        Assert.Equal(browser.listNameSeparatorPosition, scaled.listNameSeparatorPosition);
        Assert.Equal(32f, default(AssetBrowserStyle).gridMinimumCellSize);
        Assert.Equal(7.5f, metrics.toolbarSectionSpacing);
        Assert.Equal(24f, metrics.paneMinimumVisibleWidth);
        Assert.Throws<ArgumentNullException>(() => new AssetBrowserStyle(null!));
    }

    [Fact]
    public void ZoomScalesDimensionsAndFontWhilePreservingRatios()
    {
        var metrics = new EditorStyleMetrics();
        Assert.True(metrics.SetZoom(1.5f));
        Assert.Equal(1.5f, metrics.zoom, 3);
        Assert.Equal(12f, metrics.windowPadding.X, 3);
        Assert.Equal(10.5f, metrics.windowPadding.Y, 3);
        Assert.Equal(3f, metrics.frameRounding, 3);
        Assert.Equal(1.8f, metrics.fontScale, 3);
        Assert.Equal(0.4f, metrics.propertyLabelRatio, 3);
        Assert.Equal(3f, new AssetBrowserStyle(metrics).gridDefaultScale, 3);
        Assert.Equal(new Vector2(7.5f, 4.5f), metrics.cellPadding);
    }

    [Fact]
    public void ZoomCommandsClampAndResetToTheSupportedRange()
    {
        var metrics = new EditorStyleMetrics();
        Assert.True(metrics.SetZoom(100f));
        Assert.Equal(EditorStyleMetrics.C_MAX_ZOOM, metrics.zoom);
        Assert.False(metrics.ZoomIn());
        Assert.True(metrics.SetZoom(-100f));
        Assert.Equal(EditorStyleMetrics.C_MIN_ZOOM, metrics.zoom);
        Assert.False(metrics.ZoomOut());
        Assert.True(metrics.ResetZoom());
        Assert.Equal(1f, metrics.zoom);
        Assert.Throws<ArgumentOutOfRangeException>(() => metrics.SetZoom(float.NaN));
    }

    [Fact]
    public void WrappedTextRendersThroughTheSafeLiteralTextPath()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            NativeImGui.Raw.GetIO()->IniFilename = null;
            using var uiSession = new EditorUI();
            using IDisposable uiScope = uiSession.Enter();
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(260f, 180f), (int)(GuiCond.Always));
            using NativeStringLease nativeTestText0 = Utf8Codec.EncodeNullTerminated("Wrapped Text Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText0.pointer, null, 0);
            float lineHeight = NativeImGui.GetTextLineHeight();
            EditorUI.current.widgets.WrappedText(
                "Literal %s text remains safe and wraps. This deliberately long compilation status " + "must stay inside the modal content width instead of extending beyond its right edge."
            );
            Assert.True(NativeImGui.GetItemRectSize().Y > lineHeight);
            ImDrawList* windowDrawList = NativeImGui.Raw.GetWindowDrawList();
            ImDrawList* foregroundDrawList = NativeImGui.Raw.GetForegroundDrawList(NativeImGui.Raw.GetMainViewport());
            int windowVertexCount = windowDrawList->VtxBuffer.Size;
            int foregroundVertexCount = foregroundDrawList->VtxBuffer.Size;
            EditorUI.current.widgets.DropTargetHighlight(new Vector2(20f, 20f), new Vector2(120f, 80f));
            Assert.Equal(windowVertexCount, windowDrawList->VtxBuffer.Size);
            Assert.True(foregroundDrawList->VtxBuffer.Size > foregroundVertexCount);
            NativeImGui.End();
            NativeImGui.Render();
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void CenteredWrappedTextStaysInsidePaddingAndUsesMultipleLines()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            NativeImGui.Raw.GetIO()->IniFilename = null;
            using var uiSession = new EditorUI();
            using IDisposable uiScope = uiSession.Enter();
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(420f, 260f), (int)(GuiCond.Always));
            using NativeStringLease nativeTestText1 = Utf8Codec.EncodeNullTerminated("Centered Wrapped Text Test", allowsNull: false);
            _ = NativeImGui.Raw.Begin((sbyte*)nativeTestText1.pointer, null, 0);
            Vector2 origin = NativeImGui.GetCursorScreenPos();
            var area = new Vector2(320f, 180f);
            var padding = new Vector2(36f, 24f);
            ImDrawList* drawList = NativeImGui.Raw.GetWindowDrawList();
            int vertexStart = drawList->VtxBuffer.Size;
            uint textColor = NativeImGui.GetColorU32((int)(GuiCol.Text));
            EditorUI.current.widgets.CenteredWrappedText(
                "A deliberately long viewport diagnostic wraps into several centered lines " + "without touching either horizontal edge of the presentation area.",
                area,
                padding
            );
            Assert.Equal(area, NativeImGui.GetItemRectSize());
            var minimum = new Vector2(float.MaxValue);
            var maximum = new Vector2(float.MinValue);
            for (int i = vertexStart; i < drawList->VtxBuffer.Size; i++)
            {
                ImDrawVert vertex = drawList->VtxBuffer.Data[i];
                if (vertex.Col != textColor)
                {
                    continue;
                }
                minimum = Vector2.Min(minimum, Unsafe.BitCast<NativeVector2, Vector2>(vertex.Pos));
                maximum = Vector2.Max(maximum, Unsafe.BitCast<NativeVector2, Vector2>(vertex.Pos));
            }

            Assert.NotEqual(float.MaxValue, minimum.X);
            Assert.True(minimum.X >= origin.X + padding.X - 1f);
            Assert.True(maximum.X <= origin.X + area.X - padding.X + 1f);
            Assert.True(minimum.Y >= origin.Y + padding.Y - 1f);
            Assert.True(maximum.Y <= origin.Y + area.Y - padding.Y + 1f);
            Assert.True(maximum.Y - minimum.Y > NativeImGui.GetTextLineHeight());
            NativeImGui.End();
            NativeImGui.Render();
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    [Fact]
    public void InspectorContentPreservesStandardPaddingWithoutArtificialHorizontalOverflow()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.CreateContext(null);
        try
        {
            NativeImGui.Raw.GetIO()->IniFilename = null;
            using var uiSession = new EditorUI();
            using IDisposable uiScope = uiSession.Enter();
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640f, 480f));
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            EditorUI.current.widgets.SetupStyle();
            DrawInspectorCardFrame(assertHorizontalRange: false);
            DrawInspectorCardFrame(assertHorizontalRange: true);
        }
        finally
        {
            NativeImGui.Raw.DestroyContext(context);
        }
    }

    private static void DrawInspectorCardFrame(bool assertHorizontalRange)
    {
        NativeImGui.NewFrame();
        NativeImGui.SetNextWindowSize(new Vector2(320f, 260f), (int)(GuiCond.Always));
        bool isOpen = true;
        EditorUI.current.widgets.PanelWindow(
            "Inspector Width Test",
            ref isOpen,
            () => DrawInspectorCardBody(assertHorizontalRange),
            GuiWindowFlags.NoSavedSettings,
            useWindowPadding: false
        );
        NativeImGui.Render();
    }

    private static void DrawInspectorCardBody(bool assertHorizontalRange)
    {
        ImGuiWindow* panelWindow = NativeImGui.Raw.GetCurrentWindow();
        Vector2 panelBodyMinimum = NativeImGui.GetCursorScreenPos();
        Vector2 panelBodySize = NativeImGui.GetContentRegionAvail();
        if (assertHorizontalRange)
        {
            Assert.Equal(Vector2.Zero, Unsafe.BitCast<NativeVector2, Vector2>(panelWindow->WindowPadding));
            Assert.Equal(NativeImGui.GetWindowPos().X, panelBodyMinimum.X);
        }

        if (NativeImGui.BeginChild("##inspector_scroll", Vector2.Zero, 0, 0))
        {
            if (assertHorizontalRange)
            {
                Assert.Equal(panelBodyMinimum, NativeImGui.GetWindowPos());
                Assert.Equal(panelBodySize, NativeImGui.GetWindowSize());
            }

            EditorUI.current.widgets.ConstrainedContent("##inspector_content", () =>
            {
                if (assertHorizontalRange)
                {
                    Assert.Equal(
                        EditorUI.current.widgets.style.windowPadding,
                        Unsafe.BitCast<NativeVector2, Vector2>(NativeImGui.Raw.GetCurrentWindow()->WindowPadding)
                    );
                    Assert.Equal(
                        NativeImGui.GetWindowPos() + EditorUI.current.widgets.style.windowPadding,
                        NativeImGui.GetCursorScreenPos()
                    );
                }

                EditorUI.current.widgets.LabelChip("Tag", new Vector4(0.34f, 0.29f, 0.44f, 1f));
                float tagMaximumX = NativeImGui.GetItemRectMax().X;
                NativeImGui.SameLine(0f, 0f);
                NativeImGui.Dummy(new Vector2(80f, NativeImGui.GetFrameHeight()));
                if (assertHorizontalRange)
                {
                    Assert.Equal(tagMaximumX, NativeImGui.GetItemRectMin().X, 3);
                }
                NativeImGui.SameLine(0f, EditorUI.current.widgets.style.headerSectionSpacing);
                EditorUI.current.widgets.LabelChip("Layer", new Vector4(0.24f, 0.34f, 0.4f, 1f));
                float layerMaximumX = NativeImGui.GetItemRectMax().X;
                NativeImGui.SameLine(0f, 0f);
                NativeImGui.Dummy(new Vector2(80f, NativeImGui.GetFrameHeight()));
                if (assertHorizontalRange)
                {
                    Assert.Equal(layerMaximumX, NativeImGui.GetItemRectMin().X, 3);
                }
                bool open = EditorUI.current.widgets.CollapsingCard(
                    "long_behavior",
                    "BehaviorWithAnIntentionallyLongTypeNameThatMustBeClipped",
                    drawLeadingControl: static () => NativeImGui.Dummy(new Vector2(NativeImGui.GetFrameHeight())),
                    drawTrailingControl: static () => NativeImGui.Dummy(new Vector2(48f, NativeImGui.GetFrameHeight())),
                    trailingControlWidth: 48f
                );
                if (open)
                {
                    NativeImGui.Unindent();
                    EditorUI.current.widgets.CardBody(
                        "long_behavior",
                        static () => EditorUI.current.widgets.PropertyRow(
                            "long_property",
                            "Property With A Very Long Display Name",
                            static () => NativeImGui.Dummy(
                                new Vector2(MathF.Max(1f, NativeImGui.GetContentRegionAvail().X), NativeImGui.GetFrameHeight())
                            )
                        )
                    );
                    NativeImGui.Indent();
                    NativeImGui.TreePop();
                    NativeImGui.Dummy(Vector2.Zero);
                }

                if (assertHorizontalRange)
                {
                    Assert.Equal(0f, NativeImGui.GetScrollMaxX(), 3);
                }
            });
            if (assertHorizontalRange)
            {
                Assert.Equal(0f, NativeImGui.GetScrollMaxX(), 3);
            }
        }

        NativeImGui.EndChild();
    }
}
