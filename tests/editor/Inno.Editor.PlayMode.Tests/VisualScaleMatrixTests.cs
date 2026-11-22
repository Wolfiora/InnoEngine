using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Xunit;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Native.ImGui;
using Inno.Testing.Editor.Native;

using GuiCond = Inno.Editor.ImGui.ImGuiCond;
using GuiWindowFlags = Inno.Editor.ImGui.ImGuiWindowFlags;
using NativeGui = Inno.Native.ImGui.ImGui;
using NativeVector = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.PlayMode.Tests;

public sealed unsafe class VisualScaleMatrixTests
{
    [Theory]
    [MemberData(nameof(Metrics))]
    [Trait("Acceptance", "Visual")]
    public void NativeFormGeometryRemainsBalancedAcrossSimulatedDisplayMetrics(
        float zoom,
        float density,
        float width,
        float height
    ) {
        using var host = new NativePanelHost(typeof(VisualScaleMatrixTests).Assembly);
        ImGuiIO* io = NativeGui.Raw.GetIO();
        io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector>(new Vector2(width, height));
        io->DisplayFramebufferScale = Unsafe.BitCast<Vector2, NativeVector>(new Vector2(density));
        host.ui.widgets.style.SetZoom(zoom);
        float labelWidth = 0f;
        float valueWidth = 0f;
        for (int frame = 0; frame < 4; frame++)
        {
            host.Frame(() =>
            {
                NativeGui.SetNextWindowPos(new Vector2(20f, 20f), (int)GuiCond.Always);
                NativeGui.SetNextWindowSize(new Vector2(320f, 500f), (int)GuiCond.Always);
                bool open = true;
                _ = NativeGui.Begin("Narrow inspector matrix", ref open, (int)GuiWindowFlags.NoSavedSettings);
                try
                {
                    ImGuiWidget widgets = host.ui.widgets;
                    widgets.PropertyRow(
                        "reference",
                        () =>
                        {
                            labelWidth = NativeGui.GetContentRegionAvail().X;
                            widgets.PropertyLabel("Resource reference / 资源引用 / Missing");
                        },
                        () =>
                        {
                            valueWidth = NativeGui.GetContentRegionAvail().X;
                            string value = "Unassigned / 未指定";
                            widgets.Disabled(true, () => NativeGui.TextUnformatted(value));
                        }
                    );
                    widgets.MixedValue(true, () => Assert.True(widgets.isMixedValue));
                    Assert.False(widgets.isMixedValue);
                    Assert.Equal(zoom, widgets.style.zoom);
                    Assert.Equal(new Vector2(density), Unsafe.BitCast<NativeVector, Vector2>(io->DisplayFramebufferScale));
                    Assert.InRange(NativeGui.GetScrollMaxX(), 0f, 1f);
                }
                finally
                {
                    NativeGui.End();
                }
            });
        }
        Assert.InRange(labelWidth / (labelWidth + valueWidth), 0.37f, 0.42f);
        Assert.True(float.IsFinite(labelWidth) && float.IsFinite(valueWidth));
    }

    public static IEnumerable<object[]> Metrics()
    {
        foreach (float zoom in new[] { 0.75f, 0.85f, 0.9f, 1f, 1.1f, 1.25f, 1.5f })
        {
            foreach (float density in new[] { 1f, 1.25f, 1.5f, 2f })
            {
                foreach (Vector2 size in new[] { new Vector2(1280f, 720f), new Vector2(1920f, 1080f) })
                {
                    yield return [zoom, density, size.X, size.Y];
                }
            }
        }
    }
}
