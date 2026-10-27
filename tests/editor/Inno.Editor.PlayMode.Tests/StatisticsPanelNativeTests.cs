using System;
using System.Numerics;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using Xunit;
using NativeImGui = Inno.Native.ImGui.ImGui;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.PlayMode.Tests;

public sealed unsafe class StatisticsPanelNativeTests
{
    [Fact]
    public void ActualPanelSamplesAtHalfSecondIntervalsAndResetsAfterHiddenFrames()
    {
        using var host = new NativePanelHost("Inno.Editor.Panel.Stats");
        EditorPanelExtension panel = host.GetPanel("diagnostics.stats");
        Assert.Equal(0, DrawAndCountTrendVertices(host, panel, 0.1f));
        for (int frame = 0; frame < 4; frame++)
            Assert.Equal(0, DrawAndCountTrendVertices(host, panel, 0.1f));
        int twoSampleVertices = DrawAndCountTrendVertices(host, panel, 0.1f);
        Assert.True(twoSampleVertices > 0);
        panel.isOpen = false;
        host.DrawPanel(panel, deltaTime: 0.1f);
        host.DrawPanel(panel, deltaTime: 0.1f);
        panel.isOpen = true;
        Assert.Equal(0, DrawAndCountTrendVertices(host, panel, 0.1f));
    }

    [Theory]
    [InlineData(0.5f, 120)]
    [InlineData(0.6f, 101)]
    public void ActualPanelBoundsCapacityAndElapsedSampleWindow(
        float deltaTime,
        int maximumSamples
    ) {
        using var host = new NativePanelHost("Inno.Editor.Panel.Stats");
        EditorPanelExtension panel = host.GetPanel("diagnostics.stats");
        Assert.Equal(0, DrawAndCountTrendVertices(host, panel, deltaTime));
        int verticesPerInterval = DrawAndCountTrendVertices(host, panel, deltaTime);
        Assert.True(verticesPerInterval > 0);
        int maximum = 0;
        for (int frame = 0; frame < 360; frame++)
        {
            int vertices = DrawAndCountTrendVertices(host, panel, deltaTime);
            maximum = Math.Max(maximum, vertices);
            Assert.InRange(vertices, 0, (maximumSamples - 1) * verticesPerInterval);
        }
        Assert.True(maximum >= (maximumSamples - 3) * verticesPerInterval);
    }

    private static int DrawAndCountTrendVertices(
        NativePanelHost host,
        EditorPanelExtension panel,
        float deltaTime
    ) {
        int count = 0;
        host.Frame(() =>
        {
            NativeImGui.SetNextWindowPos(new(20f, 20f), (int)ImGuiCond.Always);
            NativeImGui.SetNextWindowSize(new(1000f, 800f), (int)ImGuiCond.Always);
            bool open = true;
            _ = NativeImGui.Begin("Statistics acceptance", ref open, (int)ImGuiWindowFlags.NoSavedSettings);
            Vector4 color = new(0.93f, 0.17f, 0.73f, 1f);
            uint packed = Ui.ToPackedColor(color);
            Ui.PushStyleColor(Inno.Editor.ImGui.ImGuiCol.PlotLines, color);
            try
            {
                Assert.True(panel.Draw(host.runtime.context));
                ImDrawList* draw = NativeImGui.Raw.GetWindowDrawList();
                for (int index = 0; index < draw->VtxBuffer.Size; index++)
                {
                    uint vertex = draw->VtxBuffer.Data[index].Col;
                    if ((vertex & 0x00ffffff) == (packed & 0x00ffffff) && (vertex >> 24) != 0)
                        count++;
                }
            }
            finally
            {
                Ui.PopStyleColor();
                NativeImGui.End();
            }
        }, deltaTime);
        return count;
    }
}
