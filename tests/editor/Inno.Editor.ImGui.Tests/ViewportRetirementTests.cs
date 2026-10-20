using System.Runtime.CompilerServices;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Inno.Adapter.Platform.Sdl3;
using Inno.Adapter.Presentation.ImGui;
using Inno.Core.Execution;
using Inno.Integration.WindowsX64.Sdl3;
using Inno.Native.ImGui;
using Inno.Native.Sdl3;
using Inno.Platform;
using Xunit;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.Tests;

public sealed unsafe class ViewportRetirementTests
{
    [Fact]
    [Trait("ProcessIsolation", "Required")]
    public void FaultedRetirementRetainsWindowRendererAndApplicationOwners()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(ViewportRetirementTests) + "." + nameof(FaultedRetirementRetainsWindowRendererAndApplicationOwners)))
            return;

        // A failed retirement intentionally retains dependencies until the host process restarts.
        // Run this contract in its own test process, rather than forcing unsafe cleanup.
        var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        var window = application.CreateWindow(new PlatformWindowOptions
        {
            title = "Faulted retirement", width = 320, height = 240, visible = false
        });
        var renderer = new PendingRenderer();
        var context = application.CreateImGuiContext(
            window, ImGuiContextFlags.EnableViewports, new ImGuiInteractionOptions(false, 1), renderer);
        context.SetIniFile(null);
        var viewport = new ImGuiViewport { ID = 4221, Size = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(100, 90)) };
        var io = NativeImGui.Raw.GetPlatformIO();
        io->PlatformCreateWindow(&viewport);
        uint id = renderer.target!.windowId;
        io->PlatformDestroyWindow(&viewport);
        renderer.Fail();

        Assert.Throws<RetirementPendingException>(() => context.RenderFrame(static () => { }));
        Assert.Throws<RetirementPendingException>(() => application.DestroyImGuiContext(window));
        Assert.Throws<RetirementPendingException>(() => application.Dispose());
        Assert.False((SDL.Raw.GetWindowFromId(id) == null));
        Assert.Equal(0, renderer.disposeCount);
        GC.KeepAlive(context);
        GC.KeepAlive(application);
    }

    [Fact]
    public void DetachedWindowRemainsAliveUntilRenderingAcknowledgesRetirement()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(ViewportRetirementTests) + "." + nameof(DetachedWindowRemainsAliveUntilRenderingAcknowledgesRetirement)))
            return;

        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions
        {
            title = "Retirement owner", width = 320, height = 240, visible = false
        });
        var renderer = new PendingRenderer();
        using var context = application.CreateImGuiContext(
            window, ImGuiContextFlags.EnableViewports, new ImGuiInteractionOptions(false, 1), renderer);
        context.SetIniFile(null);
        var viewport = new ImGuiViewport { ID = 4218, Size = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(100, 90)), Pos = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(30, 30)) };
        var io = NativeImGui.Raw.GetPlatformIO();
        io->PlatformCreateWindow(&viewport);
        uint id = renderer.target!.windowId;

        io->PlatformDestroyWindow(&viewport);

        Assert.Single(application.GetWindows());
        Assert.False(context.TryGetWindowId(viewport.ID, out _));
        Assert.True(viewport.PlatformUserData == null);
        Assert.False((SDL.Raw.GetWindowFromId(id) == null));
        context.RenderFrame(static () => { });
        Assert.False((SDL.Raw.GetWindowFromId(id) == null));
        Assert.Equal(1, renderer.retireCount);

        renderer.Complete();
        context.RenderFrame(static () => { });
        Assert.True((SDL.Raw.GetWindowFromId(id) == null));
        context.RenderFrame(static () => { });
        Assert.Equal(1, renderer.retireCount);
    }

    [Fact]
    public void IncompleteDrainRetainsContextOwnershipAndCanFinishAtTheNextSafetyPoint()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(ViewportRetirementTests) + "." + nameof(IncompleteDrainRetainsContextOwnershipAndCanFinishAtTheNextSafetyPoint)))
            return;

        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions
        {
            title = "Pending drain", width = 320, height = 240, visible = false
        });
        var renderer = new PendingRenderer();
        var context = application.CreateImGuiContext(
            window, ImGuiContextFlags.EnableViewports, new ImGuiInteractionOptions(false, 1), renderer);
        var viewport = new ImGuiViewport { ID = 4219, Size = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(100, 90)), Pos = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(30, 30)) };
        NativeImGui.Raw.GetPlatformIO()->PlatformCreateWindow(&viewport);
        uint id = renderer.target!.windowId;

        Assert.Throws<RetirementPendingException>(() => application.DestroyImGuiContext(window));
        Assert.False((SDL.Raw.GetWindowFromId(id) == null));
        Assert.Equal(0, renderer.disposeCount);
        var replacement = new PendingRenderer();
        Assert.Throws<InvalidOperationException>(() => application.CreateImGuiContext(
            window, ImGuiContextFlags.EnableViewports, new ImGuiInteractionOptions(false, 1), replacement));
        Assert.Equal(0, replacement.disposeCount);

        renderer.Complete();
        application.DestroyImGuiContext(window);
        Assert.True((SDL.Raw.GetWindowFromId(id) == null));
        Assert.Equal(1, renderer.disposeCount);
        context.Dispose();
        application.DestroyImGuiContext(window);
        Assert.Equal(1, renderer.disposeCount);
        replacement.Dispose();
    }

    [Fact]
    public void CreationFailureIsReportedAtAManagedSafetyPointAfterCleanup()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(ViewportRetirementTests) + "." + nameof(CreationFailureIsReportedAtAManagedSafetyPointAfterCleanup)))
            return;

        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions
        {
            title = "Failed creation", width = 320, height = 240, visible = false
        });
        var renderer = new PendingRenderer { failCreation = true };
        using var context = application.CreateImGuiContext(
            window, ImGuiContextFlags.EnableViewports, new ImGuiInteractionOptions(false, 1), renderer);
        context.SetIniFile(null);
        var viewport = new ImGuiViewport { ID = 4220, Size = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(100, 90)) };
        NativeImGui.Raw.GetPlatformIO()->PlatformCreateWindow(&viewport);
        uint id = renderer.target!.windowId;
        Assert.Single(application.GetWindows());
        Assert.False((SDL.Raw.GetWindowFromId(id) == null));

        renderer.Complete();
        Assert.Throws<InvalidOperationException>(() => context.RenderFrame(static () => { }));
        Assert.True((SDL.Raw.GetWindowFromId(id) == null));
        context.RenderFrame(static () => { });
    }

    private sealed class PendingRenderer : IPlatformImGuiRenderer
    {
        private readonly TaskCompletionSource m_retirement = new(TaskCreationOptions.RunContinuationsAsynchronously);
        internal PlatformImGuiViewportTarget? target;
        internal int retireCount;
        internal int disposeCount;
        internal bool failCreation;

        public bool supportsViewports => true;
        public void RenderMain(IntPtr drawData) { }
        public void SynchronizeMainOutput(
            int pixelWidth,
            int pixelHeight
        ) { }
        public void CreateViewport(PlatformImGuiViewportTarget target)
        {
            this.target = target;
            if (failCreation)
                throw new InvalidOperationException("The fixture rejected viewport creation.");
        }
        public void ResizeViewport(PlatformImGuiViewportTarget target) { }
        public void RenderViewport(
            PlatformImGuiViewportTarget target,
            IntPtr drawData
        ) { }
        public void PresentViewport(PlatformImGuiViewportTarget target) { }
        public Task RetireViewport(PlatformImGuiViewportTarget target)
        {
            retireCount++;
            return m_retirement.Task;
        }
        public void DrainViewportRetirements()
        {
            if (retireCount != 0 && !m_retirement.Task.IsCompletedSuccessfully)
                throw new RetirementPendingException("The fixture still borrows the SDL window.");
        }
        public void Dispose() => disposeCount++;
        internal void Complete() => m_retirement.TrySetResult();
        internal void Fail() => m_retirement.TrySetException(new InvalidOperationException("The renderer failed to retire its window."));
    }
}
