using System;
using System.Threading.Tasks;

using Inno.Adapter.Platform.Sdl3;
using Inno.Adapter.Presentation.ImGui;
using Inno.Native.ImGui;
using Inno.Native.Sdl3;
using Inno.Platform;
using Xunit;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.Tests;

public sealed unsafe class TextInputOwnershipTests
{
    [Fact]
    public void PublicImeCallbackSharesTheWindowLeaseAndUsesMeasuredPlacement()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(TextInputOwnershipTests) + "." + nameof(PublicImeCallbackSharesTheWindowLeaseAndUsesMeasuredPlacement)))
            return;
        using var app = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = app.CreateWindow(new PlatformWindowOptions
        { title = "Input method ownership", width = 320, height = 240, visible = false });
        using var context = app.CreateImGuiContext(window, ImGuiContextFlags.None,
            new ImGuiInteractionOptions(false, 1), new NoOutputRenderer());
        using var ui = new EditorUiSession();
        context.SetIniFile(null);
        while (app.PollEvent(out _)) { }
        PushWindow(window.windowId, SDLEventType.WindowShown);
        PushWindow(window.windowId, SDLEventType.WindowFocusGained);
        while (app.PollEvent(out _)) { }
        var input = Assert.IsAssignableFrom<IPlatformTextInputWindow>(window);
        context.RenderFrame(() =>
        {
            using IDisposable scope = ui.Enter();
            ImGuiPlatformIO* platform = NativeImGui.Raw.GetPlatformIO();
            ImGuiViewport* viewport = NativeImGui.Raw.GetMainViewport();
            ImGuiNativeContext* native = NativeImGui.Raw.GetCurrentContext();
            Assert.True(platform->PlatformSetImeDataFn != null);
            var request = new ImGuiPlatformImeData
            {
                WantVisible = 1, WantTextInput = 1,
                InputPos = new ImVec2C { X = viewport->Pos.X + 12.25f, Y = viewport->Pos.Y + 20.5f },
                InputLineHeight = 17.25f
            };
            for (int index = 0; index < 100; index++)
            {
                platform->PlatformSetImeDataFn(native, viewport, &request);
                Assert.Null(input.TryBeginTextInput());
                SDLRect area = default;
                int cursor = 0;
                Assert.NotEqual(0, SDL.Raw.GetTextInputArea(SDL.Raw.GetWindowFromId(window.windowId), &area, &cursor));
                Assert.Equal((12, 20, 2, 18, 0), (area.X, area.Y, area.W, area.H, cursor));
                var released = new ImGuiPlatformImeData();
                platform->PlatformSetImeDataFn(native, viewport, &released);
                using IPlatformTextInputSession other = input.TryBeginTextInput()
                    ?? throw new InvalidOperationException("The presentation owner did not release its input lease.");
                platform->PlatformSetImeDataFn(native, viewport, &request);
                Assert.True(other.isActive);
                platform->PlatformSetImeDataFn(native, viewport, &released);
                Assert.True(other.isActive);
            }
        });
    }

    private static void PushWindow(
        uint windowId,
        SDLEventType type
    ) {
        var evnt = new SDLEvent { Window = new SDLWindowEvent { Type = type, WindowID = windowId } };
        Assert.NotEqual(0, SDL.Raw.PushEvent(&evnt));
    }

    private sealed class NoOutputRenderer : IPlatformImGuiRenderer
    {
        public bool supportsViewports => false;
        public bool supportsVertexOffsets => false;
        public void RenderMain(IntPtr drawData) { }
        public void SynchronizeMainOutput(
            int pixelWidth,
            int pixelHeight
        ) { }
        public void CreateViewport(PlatformImGuiViewportTarget target) => throw new NotSupportedException();
        public void ResizeViewport(PlatformImGuiViewportTarget target) => throw new NotSupportedException();
        public void RenderViewport(
            PlatformImGuiViewportTarget target,
            IntPtr drawData
        ) => throw new NotSupportedException();
        public void PresentViewport(PlatformImGuiViewportTarget target) => throw new NotSupportedException();
        public Task RetireViewport(PlatformImGuiViewportTarget target) => Task.CompletedTask;
        public void DrainViewportRetirements() { }
        public void Dispose() { }
    }
}
