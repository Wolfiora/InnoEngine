using System.Runtime.CompilerServices;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using BGCS.Runtime;
using System.Threading.Tasks;
using System;
using System.Numerics;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Adapter.Presentation.ImGui;
using Inno.Integration.WindowsX64.Sdl3;
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
        if (NativeWindowTestHost.RunIfRequired(nameof(ViewportMetricsTests) + "." + nameof(FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback) + "." + commandKeys))
            return;

        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions
        { title = "Viewport metrics", width = 320, height = 240, visible = false, highPixelDensity = true });
        using var context = application.CreateImGuiContext(window, ImGuiContextFlags.EnableViewports,
            new ImGuiInteractionOptions(commandKeys, 1), new FixtureRenderer());
        context.SetIniFile(null);
        context.RenderFrame(() => {
            Assert.Equal(commandKeys, (NativeImGui.Raw.GetIO()->ConfigMacOsxBehaviors != 0));
            Assert.True(NativeImGui.Raw.GetPlatformIO()->PlatformGetWindowFramebufferScale == default);
            int logicalWidth = 0;
            int logicalHeight = 0;
            int pixelWidth = 0;
            int pixelHeight = 0;
            SDL.Raw.GetWindowSize((SDLWindow*)window.sdlWindowHandle, &logicalWidth, &logicalHeight);
            SDL.Raw.GetWindowSizeInPixels((SDLWindow*)window.sdlWindowHandle, &pixelWidth, &pixelHeight);
            Vector2 expected = new(pixelWidth / (float)logicalWidth, pixelHeight / (float)logicalHeight);
            Assert.Equal(expected, Unsafe.BitCast<NativeVector2, Vector2>(NativeImGui.Raw.GetMainViewport()->FramebufferScale));
            Assert.Equal(SDL.Raw.GetWindowDisplayScale((SDLWindow*)window.sdlWindowHandle)
                / SDL.Raw.GetWindowPixelDensity((SDLWindow*)window.sdlWindowHandle), NativeImGui.Raw.GetMainViewport()->DpiScale);
            Assert.Equal(new Vector2(logicalWidth, logicalHeight), Unsafe.BitCast<NativeVector2, Vector2>(NativeImGui.Raw.GetIO()->DisplaySize));
        });
        SDL.Raw.SetWindowSize((SDLWindow*)window.sdlWindowHandle, 450, 300);
        context.RenderFrame(() => Assert.Equal(new Vector2(450, 300), Unsafe.BitCast<NativeVector2, Vector2>(NativeImGui.Raw.GetIO()->DisplaySize)));
    }

    [Fact]
    public void MetricRefreshPreservesRequestedViewportGeometry()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(ViewportMetricsTests) + "." + nameof(MetricRefreshPreservesRequestedViewportGeometry)))
            return;

        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions
        { title = "Viewport geometry owner", width = 320, height = 240, visible = false });
        using var context = application.CreateImGuiContext(window, ImGuiContextFlags.EnableViewports,
            new ImGuiInteractionOptions(false, 1), new FixtureRenderer());
        context.SetIniFile(null);
        var viewport = new ImGuiViewport { ID = 4218, Size = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(100, 90)), Pos = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(30, 30)) };
        var io = NativeImGui.Raw.GetPlatformIO();
        io->PlatformCreateWindow(&viewport);
        try
        {
            // A drag owns a requested position before UpdatePlatformWindows applies it to SDL.
            Vector2 requestedPosition = new(180, 140);
            Vector2 requestedSize = new(140, 110);
            viewport.Pos = viewport.WorkPos = Unsafe.BitCast<Vector2, NativeVector2>(requestedPosition);
            viewport.Size = viewport.WorkSize = Unsafe.BitCast<Vector2, NativeVector2>(requestedSize);
            context.RenderFrame(() => { });
            Assert.Equal(requestedPosition, Unsafe.BitCast<NativeVector2, Vector2>(viewport.Pos));
            Assert.Equal(requestedPosition, Unsafe.BitCast<NativeVector2, Vector2>(viewport.WorkPos));
            Assert.Equal(requestedSize, Unsafe.BitCast<NativeVector2, Vector2>(viewport.Size));
            Assert.Equal(requestedSize, Unsafe.BitCast<NativeVector2, Vector2>(viewport.WorkSize));
            Assert.Equal((byte)0, viewport.PlatformRequestMove);
            Assert.Equal((byte)0, viewport.PlatformRequestResize);
            Assert.True(viewport.FramebufferScale.X > 0);
        }
        finally
        {
            io->PlatformDestroyWindow(&viewport);
        }
    }

    [Fact]
    public void DetachedWindowAppliesRequestsAfterPlatformEventReconciliation()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(ViewportMetricsTests) + "." + nameof(DetachedWindowAppliesRequestsAfterPlatformEventReconciliation)))
            return;

        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions
        { title = "Viewport movement", width = 320, height = 240, visible = false });
        using var context = application.CreateImGuiContext(window, ImGuiContextFlags.EnableViewports,
            new ImGuiInteractionOptions(false, 1), new FixtureRenderer());
        context.SetIniFile(null);
        var io = NativeImGui.Raw.GetIO();
        io->ConfigViewportsNoAutoMerge = (true ? (byte)1 : (byte)0);
        for (int frame = 0; frame < 24; frame++)
        {
            Vector2 requested = new(500 + frame * 7, 200 + frame * 3);
            uint viewportId = 0;
            context.RenderFrame(() => {
                NativeImGui.SetNextWindowPos(requested);
                NativeImGui.SetNextWindowSize(new Vector2(200, 120));
                using NativeStringLease nativeTestText0 = Utf8Codec.EncodeNullTerminated("Detached movement", allowsNull: false);
                NativeImGui.Raw.Begin((sbyte*)nativeTestText0.pointer, null, (int)(ImGuiWindowFlags.NoFocusOnAppearing));
                viewportId = NativeImGui.Raw.GetWindowViewport()->ID;
                NativeImGui.TextUnformatted("Moving viewport");
                NativeImGui.End();
            });
            Assert.True(context.TryGetWindowId(viewportId, out uint windowId));
            int x = 0;
            int y = 0;
            Assert.True(SDL.Raw.GetWindowPosition(SDL.Raw.GetWindowFromId(windowId), &x, &y) != 0);
            Assert.Equal(requested, new Vector2(x, y));

            // A real OS move acknowledgement owns reconciliation in the following frame.
            // Complete that frame before submitting the next application position request.
            while (application.PollEvent(out _)) { }
            context.RenderFrame(() => {
                using NativeStringLease nativeTestText1 = Utf8Codec.EncodeNullTerminated("Detached movement", allowsNull: false);
                NativeImGui.Raw.Begin((sbyte*)nativeTestText1.pointer, null, (int)(ImGuiWindowFlags.NoFocusOnAppearing));
                Assert.Equal(requested, NativeImGui.GetWindowPos());
                NativeImGui.TextUnformatted("Moving viewport");
                NativeImGui.End();
            });
        }
    }

    [Fact]
    public void LogicalDpiTransitionRetainsThePreviousScaleUntilImGuiStartsItsFrame()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(ViewportMetricsTests) + "." + nameof(LogicalDpiTransitionRetainsThePreviousScaleUntilImGuiStartsItsFrame)))
            return;

        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions
        { title = "Logical DPI transition", width = 320, height = 240, visible = false });
        using var context = application.CreateImGuiContext(window, ImGuiContextFlags.EnableViewports,
            new ImGuiInteractionOptions(false, 1), new FixtureRenderer());
        context.SetIniFile(null);
        float previous = 0f;
        context.RenderFrame(() => previous = NativeImGui.Raw.GetMainViewport()->DpiScale);
        var io = NativeImGui.Raw.GetPlatformIO();
        Assert.True(io->Monitors.Size > 0);
        for (int index = 0; index < io->Monitors.Size; index++)
            io->Monitors.Data[index].DpiScale = previous * 2f;
        Assert.Equal(previous, NativeImGui.Raw.GetMainViewport()->DpiScale);
        context.RenderFrame(() => Assert.Equal(previous * 2f, NativeImGui.Raw.GetMainViewport()->DpiScale));
    }

    [Fact]
    public void ViewportCallbacksRegisterOneSurfaceAndReleaseItBeforeNativeDestruction()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(ViewportMetricsTests) + "." + nameof(ViewportCallbacksRegisterOneSurfaceAndReleaseItBeforeNativeDestruction)))
            return;

        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions
        { title = "Viewport ownership", width = 320, height = 240, visible = false });
        var renderer = new FixtureRenderer();
        using var context = application.CreateImGuiContext(window, ImGuiContextFlags.EnableViewports,
            new ImGuiInteractionOptions(false, 1), renderer);
        var viewport = new ImGuiViewport { ID = 4217, Size = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(100, 90)), Pos = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(30, 30)) };
        var io = NativeImGui.Raw.GetPlatformIO();
        io->PlatformCreateWindow(&viewport);
        try
        {
            Assert.Equal(2, application.GetWindows().Count);
            var target = Assert.IsType<PlatformImGuiViewportTarget>(renderer.target);
            Assert.True(context.TryGetWindowId(viewport.ID, out uint id));
            Assert.Equal(target.windowId, id);
            Assert.Equal(target.nativeHandles.windowHandle, (nint)viewport.PlatformHandleRaw);
            Assert.Equal(window.nativeHandles.handleKind, target.nativeHandles.handleKind);
        }
        finally
        {
            io->PlatformDestroyWindow(&viewport);
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
