using System;
using System.Runtime.InteropServices;
using Xunit;
using Inno.Core.Logging;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using Inno.Testing.Editor.Native;

using NativeGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.Panels.Native.Tests;

public sealed unsafe class ConsoleVirtualizationNativeTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RetiringTheContextRecordClosesItsPopup(bool clear)
    {
        using var host = new NativePanelHost(typeof(ConsoleVirtualizationNativeTests).Assembly, "Inno.Editor.Panel.Logging");
        host.console.capacity = 16;
        Logger logger = host.logs.CreateLogger<ConsoleVirtualizationNativeTests>();
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
            if (window->Active != 0
                && Marshal.PtrToStringUTF8((nint)window->Name)?.Contains("##ConsoleEntryCard", StringComparison.Ordinal) == true
            ) {
                card = window;
            }
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
        {
            host.console.Clear();
        }
        else
        {
            for (int index = 0; index < host.console.capacity; index++)
            {
                logger.Write(LogLevel.Info, "Replacement context record " + index);
            }
            host.logs.Flush();
        }
        Assert.DoesNotContain(host.console.Capture().occurrences, static entry => entry.message == "Original context record");
        for (int frame = 0; frame < 3; frame++)
        {
            host.DrawPanel(panel);
        }
        Assert.Equal(0, context->OpenPopupStack.Size);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ReplacingConsoleRecordsReusesNativeWindows(bool openMenu)
    {
        using var host = new NativePanelHost(typeof(ConsoleVirtualizationNativeTests).Assembly, "Inno.Editor.Panel.Logging");
        host.console.capacity = 16;
        Logger logger = host.logs.CreateLogger<ConsoleVirtualizationNativeTests>();
        EditorPanelExtension panel = host.GetPanel("diagnostics.console");
        ImGuiIO* io = NativeGui.Raw.GetIO();
        int warmedWindowCount = 0;
        int firstReplacement = host.console.capacity;
        for (int iteration = 0; iteration < firstReplacement + 4; iteration++)
        {
            logger.Write(LogLevel.Info, "Context entry " + iteration);
            host.logs.Flush();
            host.DrawPanel(panel);
            host.DrawPanel(panel);
            if (openMenu)
            {
                ImGuiWindow* card = null;
                ImGuiNativeContext* context = NativeGui.Raw.GetCurrentContext();
                for (int index = 0; index < context->Windows.Size; index++)
                {
                    ImGuiWindow* window = context->Windows.Data[index];
                    if (window->Active != 0
                        && Marshal.PtrToStringUTF8((nint)window->Name)?.Contains("##ConsoleEntryCard", StringComparison.Ordinal) == true
                    ) {
                        card = window;
                    }
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
            int windows = NativeGui.Raw.GetCurrentContext()->Windows.Size;
            if (iteration == firstReplacement + 1)
            {
                warmedWindowCount = windows;
            }
            if (iteration > firstReplacement + 1)
            {
                Assert.InRange(windows, 1, warmedWindowCount);
            }
        }
        Assert.Equal(host.console.capacity, host.console.Capture().occurrences.Count);
    }

    [Fact]
    public void OffscreenOccurrencesKeepKeyboardNavigationWithoutSubmittingEveryCard()
    {
        using var host = new NativePanelHost(typeof(ConsoleVirtualizationNativeTests).Assembly, "Inno.Editor.Panel.Logging");
        host.console.capacity = 256;
        Logger logger = host.logs.CreateLogger<ConsoleVirtualizationNativeTests>();
        for (int index = 0; index < 256; index++)
        {
            logger.Write(LogLevel.Info, "Entry " + index.ToString("D5") + "\nDetails / 资源信息");
            if (index % 128 == 0)
            {
                host.logs.Flush();
            }
        }
        host.logs.Flush();
        Assert.Equal(256, host.console.Capture().occurrences.Count);
        EditorPanelExtension panel = host.GetPanel("diagnostics.console");
        for (int frame = 0; frame < 4; frame++)
        {
            host.DrawPanel(panel);
        }
        Assert.InRange(NativeGui.Raw.GetCurrentContext()->Windows.Size, 1, 128);
        NativePanelInput.Focus(host, panel, "ConsoleRegion");
        NativePanelInput.Key(host, panel, ImGuiKey.Home);
        NativePanelInput.Key(host, panel, ImGuiKey.Enter);
        for (int frame = 0; frame < 3; frame++)
        {
            host.DrawPanel(panel);
        }
        Assert.InRange(NativeGui.Raw.GetCurrentContext()->Windows.Size, 1, 128);
    }
}
