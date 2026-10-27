using ImGuiWindowFlags = Inno.Editor.ImGui.ImGuiWindowFlags;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System.Runtime.CompilerServices;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using NativeVector4 = Inno.Native.ImGui.ImVec4C;
using BGCS.Runtime;
using System;
using System.Numerics;
using Inno.Editor.ImGui;
using Inno.Native.ImGui;
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using NativeImGui = Inno.Native.ImGui.ImGui;
using Xunit;

namespace Inno.Editor.Scripting.Tests;
[Trait("Acceptance", "Visual")]
public sealed unsafe class InspectorPresentationLayoutTests : IDisposable
{
    private readonly ImGuiNativeContext* m_context = NativeImGui.Raw.CreateContext(null);
    private readonly Inno.Editor.ImGui.EditorUiSession m_ui = new();
    private readonly IDisposable m_uiScope;
    public InspectorPresentationLayoutTests()
    {
        m_uiScope = m_ui.Enter();
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(640, 480));
        io->DeltaTime = 1f / 60f;
        io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
        io->Fonts->RendererHasTextures = 1;
        EditorUI.current.widgets.style.ResetZoom();
        EditorUI.current.widgets.SetupStyle();
    }

    public void Dispose()
    {
        m_uiScope.Dispose();
        m_ui.Dispose();
        NativeImGui.Raw.DestroyContext(m_context);
    }

    [Fact]
    public void FloatingPanelCloseButtonClosesOnMouseRelease()
    {
        bool open = true;
        void Frame()
        {
            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowPos(new Vector2(30, 30));
            NativeImGui.SetNextWindowSize(new Vector2(320, 180));
            EditorUI.current.widgets.PanelWindow("Floating acceptance", ref open, () => NativeImGui.TextUnformatted("Body"));
            NativeImGui.Render();
        }

        Frame();
        Frame();
        using NativeStringLease nativeTestText0 = Utf8Codec.EncodeNullTerminated("Floating acceptance", allowsNull: false);
        ImGuiWindow* window = NativeImGui.Raw.FindWindowByName((sbyte*)nativeTestText0.pointer);
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        NativeImGui.Raw.AddMousePosEvent(io, window->Pos.X + window->Size.X - 12, window->Pos.Y + window->TitleBarHeight * 0.5f);
        Frame();
        NativeImGui.Raw.AddMouseButtonEvent(io, 0, (true ? (byte)1 : (byte)0));
        Frame();
        Assert.True(open);
        NativeImGui.Raw.AddMouseButtonEvent(io, 0, (false ? (byte)1 : (byte)0));
        Frame();
        Assert.False(open);
    }

    [Theory]
    [InlineData(640, 480, 1f)]
    [InlineData(360, 240, 1.5f)]
    public void TooltipAtBottomRightFitsParentViewportAndAllText(
        int width,
        int height,
        float zoom
    ) {
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(width, height));
        EditorUI.current.widgets.style.SetZoom(zoom);
        EditorUI.current.widgets.SetupStyle();
        Vector2 hover = default;
        const string text = "The profile is evaluated after this camera stack is composed. The final words must remain readable.";
        void Frame()
        {
            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowPos(new Vector2(width - 210, height - 80));
            NativeImGui.SetNextWindowSize(new Vector2(200, 70));
            using NativeStringLease nativeTestText1 = Utf8Codec.EncodeNullTerminated("Edge acceptance", allowsNull: false);
            NativeImGui.Raw.Begin((sbyte*)nativeTestText1.pointer, null, 0);
            NativeImGui.Button("Hover near edge");
            hover = (NativeImGui.GetItemRectMin() + NativeImGui.GetItemRectMax()) * 0.5f;
            EditorUI.current.widgets.DrawItemTooltip(text);
            NativeImGui.End();
            NativeImGui.Render();
        }

        Frame();
        Frame();
        NativeImGui.Raw.AddMousePosEvent(io, hover.X, hover.Y);
        for (int index = 0; index < 4; index++)
            Frame();
        using NativeStringLease nativeTestText2 = Utf8Codec.EncodeNullTerminated("##Tooltip_00", allowsNull: false);
        ImGuiWindow* tooltip = NativeImGui.Raw.FindWindowByName((sbyte*)nativeTestText2.pointer);
        Assert.False((tooltip == null));
        Assert.True((tooltip->Active != 0));
        Assert.InRange(tooltip->Pos.X, 0, width);
        Assert.InRange(tooltip->Pos.Y, 0, height);
        Assert.True(tooltip->Pos.X + tooltip->Size.X <= width);
        Assert.True(tooltip->Pos.Y + tooltip->Size.Y <= height);
        Assert.True(tooltip->ContentSize.X <= tooltip->InnerRect.Max.X - tooltip->InnerRect.Min.X + 1);
        Assert.True(tooltip->ContentSize.Y <= tooltip->InnerRect.Max.Y - tooltip->InnerRect.Min.Y + 1);
    }

    [Fact]
    public void HelpBoxUsesWrappedCardHeightWithoutHorizontalOverflow()
    {
        for (int index = 0; index < 2; index++)
        {
            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowSize(new Vector2(280, 220));
            using NativeStringLease nativeTestText3 = Utf8Codec.EncodeNullTerminated("HelpBox acceptance", allowsNull: false);
            NativeImGui.Raw.Begin((sbyte*)nativeTestText3.pointer, null, 0);
            float width = NativeImGui.GetContentRegionAvail().X;
            EditorUI.current.widgets.HelpBox("Assign a Sprite to enable atlas geometry, borders, pixel density, and sampling controls.", "i", EditorPalette.warning);
            Vector2 size = NativeImGui.GetItemRectSize();
            Assert.Equal(width, size.X, 2);
            Assert.True(size.Y > NativeImGui.GetTextLineHeight() * 2);
            NativeImGui.End();
            NativeImGui.Render();
        }
    }

    [Theory]
    [InlineData(0.9f, 1f)]
    [InlineData(0.9f, 2f)]
    [InlineData(1f, 1f)]
    [InlineData(1f, 2f)]
    [InlineData(1.25f, 1.5f)]
    [InlineData(1.5f, 2f)]
    public void RoundedWindowTitleAndBodyHaveContinuousOpaqueCoverage(
        float zoom,
        float framebufferScale
    ) {
        NativeImGui.Raw.GetIO()->DisplayFramebufferScale = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(framebufferScale));
        EditorUI.current.widgets.style.SetZoom(zoom);
        EditorUI.current.widgets.SetupStyle();
        NativeImGui.Raw.GetStyle()->WindowRounding = 6f * zoom;
        NativeImGui.Raw.GetStyle()->FrameBorderSize = 0;
        for (int frame = 0; frame < 2; frame++)
        {
            NativeImGui.NewFrame();
            NativeImGui.SetNextWindowPos(new Vector2(30, 30));
            NativeImGui.SetNextWindowSize(new Vector2(320, 180));
            using NativeStringLease nativeTestText4 = Utf8Codec.EncodeNullTerminated("Opaque surface", allowsNull: false);
            NativeImGui.Raw.Begin((sbyte*)nativeTestText4.pointer, null, (int)(ImGuiWindowFlags.NoCollapse));
            NativeImGui.End();
            NativeImGui.Render();
        }

        using NativeStringLease nativeTestText5 = Utf8Codec.EncodeNullTerminated("Opaque surface", allowsNull: false);
        ImGuiWindow* window = NativeImGui.Raw.FindWindowByName((sbyte*)nativeTestText5.pointer);
        float seam = window->Pos.Y + window->TitleBarHeight;
        // Sample subpixel positions as well as device pixel centers. A border line must not
        // conceal a transparent join between independently anti-aliased background shapes.
        for (int sample = -10; sample <= 10; sample++)
        {
            Vector2 point = new(window->Pos.X + window->Size.X * 0.47f, seam + sample * 0.1f);
            float coverage = CoverageAt(window->DrawList, point);
            Assert.True(coverage >= 0.999f, $"Transparent title/body join: zoom={zoom}, scale={framebufferScale}, y={point.Y}, alpha={coverage}");
        }

        for (int pixel = -2; pixel <= 2; pixel++)
        {
            float y = (MathF.Floor(seam * framebufferScale) + pixel + 0.5f) / framebufferScale;
            Vector2 point = new(window->Pos.X + window->Size.X * 0.47f, y);
            Assert.True(CoverageAt(window->DrawList, point) >= 0.999f, $"Transparent device pixel: zoom={zoom}, scale={framebufferScale}, y={y}");
        }

        Assert.True(NativeImGui.Raw.GetStyle()->AntiAliasedFill != 0);
        Assert.Equal(6f * zoom, window->WindowRounding);
    }

    private static float CoverageAt(
        ImDrawList* list,
        Vector2 point
    ) {
        float result = 0;
        for (int commandIndex = 0; commandIndex < list->CmdBuffer.Size; commandIndex++)
        {
            ImDrawCmd command = list->CmdBuffer.Data[commandIndex];
            Vector4 clip = Unsafe.BitCast<NativeVector4, Vector4>(command.ClipRect);
            if (point.X < clip.X || point.X >= clip.Z || point.Y < clip.Y || point.Y >= clip.W)
                continue;
            for (uint index = command.IdxOffset; index < command.IdxOffset + command.ElemCount; index += 3)
            {
                ImDrawVert a = list->VtxBuffer.Data[(int)command.VtxOffset + list->IdxBuffer.Data[(int)index]];
                ImDrawVert b = list->VtxBuffer.Data[(int)command.VtxOffset + list->IdxBuffer.Data[(int)index + 1]];
                ImDrawVert c = list->VtxBuffer.Data[(int)command.VtxOffset + list->IdxBuffer.Data[(int)index + 2]];
                Vector2 aPosition = Unsafe.BitCast<ImVec2C, Vector2>(a.Pos);
                Vector2 bPosition = Unsafe.BitCast<ImVec2C, Vector2>(b.Pos);
                Vector2 cPosition = Unsafe.BitCast<ImVec2C, Vector2>(c.Pos);
                float area = Cross(bPosition - aPosition, cPosition - aPosition);
                if (MathF.Abs(area) < 0.000001f)
                    continue;
                float wa = Cross(bPosition - point, cPosition - point) / area;
                float wb = Cross(cPosition - point, aPosition - point) / area;
                float wc = 1 - wa - wb;
                if (wa < 0 || wb < 0 || wc < 0)
                    continue;
                float alpha = (wa * (a.Col >> 24) + wb * (b.Col >> 24) + wc * (c.Col >> 24)) / 255f;
                result = alpha + result * (1 - alpha);
            }
        }

        return result;
    }

    private static float Cross(
        Vector2 a,
        Vector2 b
    ) => a.X * b.Y - a.Y * b.X;
}
