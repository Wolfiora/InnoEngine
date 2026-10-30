using System;
using System.Runtime.InteropServices;
using Inno.Core.Logging;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using Xunit;
using Xunit.Abstractions;
using NativeGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.PlayMode.Tests;

public sealed unsafe class ConsoleVirtualizationNativeTests(ITestOutputHelper output)
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RetiringTheContextRecordClosesItsPopup(bool clear)
    {
        using var host = new NativePanelHost("Inno.Editor.Panel.Logging");
        host.console.capacity = 16;
        var logger = host.logs.CreateLogger<ConsoleVirtualizationNativeTests>();
        EditorPanelExtension panel = host.GetPanel("diagnostics.console");
        logger.Write(LogLevel.Info, "Original context record");
        host.logs.Flush();
        host.DrawPanel(panel);
        host.DrawPanel(panel);
        ImGuiNativeContext* context = NativeGui.Raw.GetCurrentContext();
        ImGuiWindow* card = null;
        for (int index = 0; index < context->Windows.Size; index++)
        {
            ImGuiWindow* window = context->Windows.Data[index];
            if (window->Active != 0 && Marshal.PtrToStringUTF8((nint)window->Name)?.Contains("##ConsoleEntryCard", StringComparison.Ordinal) == true)
                card = window;
        }
        Assert.True(card is not null);
        ImGuiIO* io = NativeGui.Raw.GetIO();
        NativeGui.Raw.AddMousePosEvent(io, card->Pos.X + 230f, card->Pos.Y + 12f);
        host.DrawPanel(panel);
        NativeGui.Raw.AddMouseButtonEvent(io, 1, 1);
        host.DrawPanel(panel);
        NativeGui.Raw.AddMouseButtonEvent(io, 1, 0);
        host.DrawPanel(panel);
        Assert.True(context->OpenPopupStack.Size > 0);
        if (clear)
            host.console.Clear();
        else
        {
            for (int index = 0; index < host.console.capacity; index++)
                logger.Write(LogLevel.Info, "Replacement context record " + index);
            host.logs.Flush();
        }
        Assert.DoesNotContain(host.console.Capture().occurrences, static entry => entry.message == "Original context record");
        for (int frame = 0; frame < 3; frame++)
            host.DrawPanel(panel);
        Assert.Equal(0, context->OpenPopupStack.Size);
    }

    [Fact]
    public void RepeatedContextMenusDoNotAccumulateRetiredOccurrenceWindows()
    {
        using var host = new NativePanelHost("Inno.Editor.Panel.Logging");
        host.console.capacity = 16;
        var logger = host.logs.CreateLogger<ConsoleVirtualizationNativeTests>();
        EditorPanelExtension panel = host.GetPanel("diagnostics.console");
        ImGuiIO* io = NativeGui.Raw.GetIO();
        for (int iteration = 0; iteration < 100; iteration++)
        {
            logger.Write(LogLevel.Info, "Context entry " + iteration);
            host.logs.Flush();
            host.DrawPanel(panel);
            host.DrawPanel(panel);
            ImGuiWindow* card = null;
            ImGuiNativeContext* context = NativeGui.Raw.GetCurrentContext();
            for (int index = 0; index < context->Windows.Size; index++)
            {
                ImGuiWindow* window = context->Windows.Data[index];
                if (window->Active != 0 && Marshal.PtrToStringUTF8((nint)window->Name)?.Contains("##ConsoleEntryCard", StringComparison.Ordinal) == true)
                    card = window;
            }
            Assert.True(card is not null);
            NativeGui.Raw.AddMousePosEvent(io, card->Pos.X + 230f, card->Pos.Y + 12f);
            host.DrawPanel(panel);
            NativeGui.Raw.AddMouseButtonEvent(io, 1, 1);
            host.DrawPanel(panel);
            NativeGui.Raw.AddMouseButtonEvent(io, 1, 0);
            host.DrawPanel(panel);
            Assert.True(context->OpenPopupStack.Size > 0);
            NativePanelInput.Key(host, panel, ImGuiKey.Escape);
        }
        output.WriteLine("Context menus: 100; retained records: " + host.console.Capture().occurrences.Count
            + "; native windows: " + NativeGui.Raw.GetCurrentContext()->Windows.Size);
        Assert.InRange(NativeGui.Raw.GetCurrentContext()->Windows.Size, 1, 32);
    }

    [Fact]
    public void TenThousandOccurrencesSubmitOnlyViewportCardsAndKeepKeyboardNavigation()
    {
        using var host = new NativePanelHost("Inno.Editor.Panel.Logging");
        host.console.capacity = 10_000;
        var logger = host.logs.CreateLogger<ConsoleVirtualizationNativeTests>();
        for (int index = 0; index < 10_000; index++)
        {
            logger.Write(LogLevel.Info, "Entry " + index.ToString("D5") + "\nDetails / 资源信息");
            if (index % 128 == 0)
                host.logs.Flush();
        }
        host.logs.Flush();
        Assert.Equal(10_000, host.console.Capture().occurrences.Count);
        EditorPanelExtension panel = host.GetPanel("diagnostics.console");
        for (int frame = 0; frame < 4; frame++)
            host.DrawPanel(panel);
        Assert.InRange(NativeGui.Raw.GetCurrentContext()->Windows.Size, 1, 128);
        NativePanelInput.Focus(host, panel, "ConsoleRegion");
        NativePanelInput.Key(host, panel, ImGuiKey.Home);
        NativePanelInput.Key(host, panel, ImGuiKey.Enter);
        for (int frame = 0; frame < 3; frame++)
            host.DrawPanel(panel);
        Assert.InRange(NativeGui.Raw.GetCurrentContext()->Windows.Size, 1, 128);
    }

    [Fact]
    public void RetiredOccurrencesDoNotAccumulateNativeChildWindows()
    {
        using var host = new NativePanelHost("Inno.Editor.Panel.Logging");
        host.console.capacity = 16;
        var logger = host.logs.CreateLogger<ConsoleVirtualizationNativeTests>();
        EditorPanelExtension panel = host.GetPanel("diagnostics.console");
        for (int iteration = 0; iteration < 150; iteration++)
        {
            logger.Write(LogLevel.Info, "Retained occurrence " + iteration);
            host.logs.Flush();
            host.DrawPanel(panel);
            host.DrawPanel(panel);
        }
        Assert.Equal(16, host.console.Capture().occurrences.Count);
        output.WriteLine("Retained records: 16; completed updates: 150; native windows: " + NativeGui.Raw.GetCurrentContext()->Windows.Size);
        Assert.InRange(NativeGui.Raw.GetCurrentContext()->Windows.Size, 1, 64);
    }
}
