using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BGCS.Runtime;
using Inno.Core.Diagnostics;
using Inno.Core.Logging;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using Xunit;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.PlayMode.Tests;

public sealed unsafe class ConsoleSearchNativeTests
{
    [Fact]
    public void ActualConsoleFiltersMessagesFilesSourcesAndLongUnicodeQueries()
    {
        using var host = new NativePanelHost("Inno.Editor.Panel.Logging");
        EditorPanelExtension panel = host.GetPanel("diagnostics.console");
        var logger = host.logs.CreateLogger<ConsoleSearchNativeTests>();
        for (int occurrence = 0; occurrence < 2; occurrence++)
            logger.Write(LogLevel.Info, "Needle message");
        logger.Write(LogLevel.Warn, "Unrelated message");
        string longMessage = new('x', 300);
        logger.Write(LogLevel.Info, longMessage);
        host.logs.Flush();
        using var reporter = host.diagnostics.CreateReporter(new("search-source", "SOURCE-NAME"));
        reporter.Publish(new("SEARCH_CODE", "A diagnostic", DiagnosticSeverity.Error, location: new DiagnosticLocation("Assets/漢字.shader", 42, 3)));
        host.DrawPanel(panel);
        host.DrawPanel(panel);
        Assert.Equal(5, host.console.Capture().occurrences.Count);
        Assert.Equal(4, CountCards());
        foreach (string query in new[] { "nEeDlE", "漢字", "source-name", "search_code", longMessage })
        {
            Search(host, panel, query);
            Assert.Equal(1, CountCards());
            Assert.Equal(5, host.console.Capture().occurrences.Count);
        }
        Search(host, panel, "no matching content");
        Assert.Equal(0, CountCards());
        Search(host, panel, string.Empty);
        Assert.Equal(4, CountCards());
    }

    [Fact]
    public void FilteredKeyboardTargetCopiesTheSameGroupAndClearReleasesIt()
    {
        using var host = new NativePanelHost("Inno.Editor.Panel.Logging");
        EditorPanelExtension panel = host.GetPanel("diagnostics.console");
        var logger = host.logs.CreateLogger<ConsoleSearchNativeTests>();
        for (int occurrence = 0; occurrence < 2; occurrence++)
            logger.Write(LogLevel.Info, "Needle message");
        logger.Write(LogLevel.Warn, "Other entry");
        host.logs.Flush();
        host.DrawPanel(panel);
        host.DrawPanel(panel);
        Search(host, panel, "needle");
        ImGuiWindow* card = FindActiveWindow("##ConsoleEntryCard");
        Vector2 position = Unsafe.BitCast<NativeVector2, Vector2>(card->Pos) + new Vector2(230f, 12f);
        Click(host, panel, position);
        Assert.Equal("panel/diagnostics.console/entry", host.runtime.interactions.focusedArea);
        Assert.NotNull(host.runtime.interactions.focusedTarget);
        float collapsedHeight = card->Size.Y;
        NativeImGui.Raw.AddKeyEvent(NativeImGui.Raw.GetIO(), ImGuiKey.Enter, 1);
        host.DrawPanel(panel);
        NativeImGui.Raw.AddKeyEvent(NativeImGui.Raw.GetIO(), ImGuiKey.Enter, 0);
        host.DrawPanel(panel);
        host.DrawPanel(panel);
        Assert.True(card->Size.Y > collapsedHeight + 1f);
        host.DrawPanel(panel, () =>
        {
            var context = host.runtime.interactions.For(host.runtime.interactions.focusedArea, host.runtime.interactions.focusedTarget);
            Assert.True(context.Execute("console/copy-message"));
            Assert.Equal("Needle message", Marshal.PtrToStringUTF8((nint)NativeImGui.Raw.GetClipboardText()));
            Assert.True(context.Execute("console/copy-details"));
            string? details = Marshal.PtrToStringUTF8((nint)NativeImGui.Raw.GetClipboardText());
            Assert.Contains("Needle message", details);
            Assert.Contains("(x2)", details);
        });
        host.console.Clear();
        host.DrawPanel(panel);
        Assert.Null(host.runtime.interactions.focusedTarget);
        Assert.Equal(0, CountCards());
    }

    private static void Search(
        NativePanelHost host,
        EditorPanelExtension panel,
        string query
    ) {
        ImGuiWindow* child = FindActiveWindow("ConsoleChild", "ConsoleRegion");
        Vector2 position = Unsafe.BitCast<NativeVector2, Vector2>(child->DC.CursorStartPos) + new Vector2(80f, 10f);
        Click(host, panel, position);
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, 1);
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.A, 1);
        host.DrawPanel(panel);
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.A, 0);
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, 0);
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.Backspace, 1);
        host.DrawPanel(panel);
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.Backspace, 0);
        using NativeStringLease text = Utf8Codec.EncodeNullTerminated(query, allowsNull: false);
        NativeImGui.Raw.AddInputCharactersUTF8(io, (sbyte*)text.pointer);
        host.DrawPanel(panel);
        host.DrawPanel(panel);
    }

    private static void Click(
        NativePanelHost host,
        EditorPanelExtension panel,
        Vector2 position
    ) {
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        NativeImGui.Raw.AddMousePosEvent(io, position.X, position.Y);
        host.DrawPanel(panel);
        NativeImGui.Raw.AddMouseButtonEvent(io, 0, 1);
        host.DrawPanel(panel);
        NativeImGui.Raw.AddMouseButtonEvent(io, 0, 0);
        host.DrawPanel(panel);
    }

    private static int CountCards()
    {
        ImGuiNativeContext* context = NativeImGui.Raw.GetCurrentContext();
        int count = 0;
        for (int index = 0; index < context->Windows.Size; index++)
        {
            ImGuiWindow* window = context->Windows.Data[index];
            if (window->Active != 0 && Marshal.PtrToStringUTF8((nint)window->Name)?.Contains("##ConsoleEntryCard", StringComparison.Ordinal) == true)
                count++;
        }
        return count;
    }

    private static ImGuiWindow* FindActiveWindow(
        string containing,
        string? excluding = null
    ) {
        ImGuiNativeContext* context = NativeImGui.Raw.GetCurrentContext();
        for (int index = 0; index < context->Windows.Size; index++)
        {
            ImGuiWindow* window = context->Windows.Data[index];
            string name = Marshal.PtrToStringUTF8((nint)window->Name) ?? string.Empty;
            if (window->Active != 0 && name.Contains(containing, StringComparison.Ordinal)
                && (excluding is null || !name.Contains(excluding, StringComparison.Ordinal)))
                return window;
        }
        throw new InvalidOperationException("The actual control window was not submitted: " + containing);
    }
}
