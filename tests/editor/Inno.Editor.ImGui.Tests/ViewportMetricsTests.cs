using System.Threading.Tasks;
using System;
using System.Numerics;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Adapter.Presentation.ImGui;
using Inno.Integration.Windows.Sdl3;
using Inno.Native.ImGui;
using Inno.Native.Sdl3;
using Inno.Platform;
using Xunit;
using NativeImGui = Inno.Native.ImGui.ImGui;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Inno.Editor.ImGui.Tests;

public sealed unsafe class ViewportMetricsTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback(bool commandKeys)
    {
        using var application = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        using var window = application.CreateWindow(new PlatformWindowOptions
        { title = "Viewport metrics", width = 320, height = 240, visible = false, highPixelDensity = true });
        using var context = application.CreateImGuiContext(window, ImGuiContextFlags.EnableViewports,
            new ImGuiInteractionOptions(commandKeys, 1), new FixtureRenderer());
        context.SetIniFile(null);
        context.RenderFrame(() => {
            Assert.Equal(commandKeys, NativeImGui.GetIO().ConfigMacOSXBehaviors);
            Assert.True(NativeImGui.GetPlatformIO().PlatformGetWindowFramebufferScale == null);
            int logicalWidth = 0;
            int logicalHeight = 0;
            int pixelWidth = 0;
            int pixelHeight = 0;
            SDL.GetWindowSize(new SDLWindow(window.sdlWindowHandle), ref logicalWidth, ref logicalHeight);
            SDL.GetWindowSizeInPixels(new SDLWindow(window.sdlWindowHandle), ref pixelWidth, ref pixelHeight);
            Vector2 expected = new(pixelWidth / (float)logicalWidth, pixelHeight / (float)logicalHeight);
            Assert.Equal(expected, NativeImGui.GetMainViewport().FramebufferScale);
            Assert.Equal(SDL.GetWindowDisplayScale(new SDLWindow(window.sdlWindowHandle))
                / SDL.GetWindowPixelDensity(new SDLWindow(window.sdlWindowHandle)), NativeImGui.GetMainViewport().DpiScale);
            Assert.Equal(new Vector2(logicalWidth, logicalHeight), NativeImGui.GetIO().DisplaySize);
        });
        SDL.SetWindowSize(new SDLWindow(window.sdlWindowHandle), 450, 300);
        context.RenderFrame(() => Assert.Equal(new Vector2(450, 300), NativeImGui.GetIO().DisplaySize));
    }

    [Fact]
    public void LogicalDpiTransitionRetainsThePreviousScaleUntilImGuiStartsItsFrame()
    {
        using var application = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        using var window = application.CreateWindow(new PlatformWindowOptions
        { title = "Logical DPI transition", width = 320, height = 240, visible = false });
        using var context = application.CreateImGuiContext(window, ImGuiContextFlags.EnableViewports,
            new ImGuiInteractionOptions(false, 1), new FixtureRenderer());
        context.SetIniFile(null);
        float previous = 0f;
        context.RenderFrame(() => previous = NativeImGui.GetMainViewport().DpiScale);
        var io = NativeImGui.GetPlatformIO();
        Assert.True(io.Monitors.Size > 0);
        for (int index = 0; index < io.Monitors.Size; index++)
            io.Monitors.Data[index].DpiScale = previous * 2f;
        Assert.Equal(previous, NativeImGui.GetMainViewport().DpiScale);
        context.RenderFrame(() => Assert.Equal(previous * 2f, NativeImGui.GetMainViewport().DpiScale));
    }

    [Fact]
    public void ViewportCallbacksRegisterOneSurfaceAndReleaseItBeforeNativeDestruction()
    {
        using var application = new Sdl3PlatformApplication(new WindowsSdl3HostIntegration());
        using var window = application.CreateWindow(new PlatformWindowOptions
        { title = "Viewport ownership", width = 320, height = 240, visible = false });
        var renderer = new FixtureRenderer();
        using var context = application.CreateImGuiContext(window, ImGuiContextFlags.EnableViewports,
            new ImGuiInteractionOptions(false, 1), renderer);
        var viewport = new ImGuiViewport { ID = 4217, Size = new Vector2(100, 90), Pos = new Vector2(30, 30) };
        var io = NativeImGui.GetPlatformIO();
        io.PlatformCreateWindow(&viewport);
        try
        {
            Assert.Equal(2, application.GetWindows().Count);
            var target = Assert.IsType<PlatformImGuiViewportTarget>(renderer.target);
            Assert.True(context.TryGetWindowId(viewport.ID, out uint id));
            Assert.Equal(target.windowId, id);
            Assert.Equal(target.nativeHandles.windowHandle, (nint)viewport.PlatformHandleRaw);
            Assert.Equal(PlatformNativeHandleId.win32, target.nativeHandles.handleKind);
        }
        finally
        {
            io.PlatformDestroyWindow(&viewport);
        }
        Assert.Single(application.GetWindows());
        Assert.Null(renderer.target);
        Assert.True(viewport.PlatformUserData == null);
    }

    private sealed class FixtureRenderer : IPlatformImGuiRenderer
    {
        internal PlatformImGuiViewportTarget? target;
        public bool supportsViewports => true;
        public void RenderMain(IntPtr drawData) { }
        public void SynchronizeMainOutput(
            int pixelWidth,
            int pixelHeight
        ) { }
        public void CreateViewport(PlatformImGuiViewportTarget target) => this.target = target;
        public void ResizeViewport(PlatformImGuiViewportTarget target) { }
        public void RenderViewport(
            PlatformImGuiViewportTarget target,
            IntPtr drawData
        ) { }
        public void PresentViewport(PlatformImGuiViewportTarget target) { }
        public Task RetireViewport(PlatformImGuiViewportTarget target)
        {
            this.target = null;
            return Task.CompletedTask;
        }
        public void DrainViewportRetirements() { }
        public void Dispose() { }
    }
}
