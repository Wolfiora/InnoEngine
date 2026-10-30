using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Inno.Editor.ImGui;
using Inno.Native.ImGui;
using Xunit;
using NativeGui = Inno.Native.ImGui.ImGui;
using NativeVector = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.PlayMode.Tests;

public sealed unsafe class VisualScaleMatrixTests
{
    public static IEnumerable<object[]> Metrics()
    {
        foreach (float zoom in new[] { 0.75f, 0.85f, 0.90f, 1f, 1.10f, 1.25f, 1.50f })
            foreach (float density in new[] { 1f, 1.25f, 1.50f, 2f })
                foreach (Vector2 size in new[] { new Vector2(1280f, 720f), new Vector2(1920f, 1080f) })
                    yield return [zoom, density, size.X, size.Y];
    }

    [Theory]
    [MemberData(nameof(Metrics))]
    [Trait("Acceptance", "Visual")]
    public void NativeFormGeometryRemainsBalancedAcrossSimulatedDisplayMetrics(
        float zoom,
        float density,
        float width,
        float height
    ) {
        using var host = new NativePanelHost();
        ImGuiIO* io = NativeGui.Raw.GetIO();
        io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector>(new(width, height));
        io->DisplayFramebufferScale = Unsafe.BitCast<Vector2, NativeVector>(new(density));
        host.ui.widgets.style.SetZoom(zoom);
        float labelWidth = 0f;
        float valueWidth = 0f;
        for (int frame = 0; frame < 4; frame++)
        {
            host.Frame(() =>
            {
                NativeGui.SetNextWindowPos(new Vector2(20f, 20f), (int)Inno.Editor.ImGui.ImGuiCond.Always);
                NativeGui.SetNextWindowSize(new Vector2(320f, 500f), (int)Inno.Editor.ImGui.ImGuiCond.Always);
                bool open = true;
                _ = NativeGui.Begin("Narrow inspector matrix", ref open, (int)Inno.Editor.ImGui.ImGuiWindowFlags.NoSavedSettings);
                try
                {
                    var widgets = host.ui.widgets;
                    widgets.PropertyRow("reference", () =>
                    {
                        labelWidth = NativeGui.GetContentRegionAvail().X;
                        widgets.PropertyLabel("Resource reference / 资源引用 / Missing");
                    }, () =>
                    {
                        valueWidth = NativeGui.GetContentRegionAvail().X;
                        string value = "Unassigned / 未指定";
                        widgets.Disabled(true, () => NativeGui.TextUnformatted(value));
                    });
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
}
