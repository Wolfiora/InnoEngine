using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Xunit;

namespace Inno.Native.ImGui.Tests;

[Collection("ImGuiContext")]
public sealed unsafe class ImGuiPlatformCallbackTests
{
    private struct CallbackState
    {
        internal void* registration;
        internal int positions;
        internal int sizes;
        internal bool invalid;
    }

    [Fact]
    public void RegistrationRejectsForeignOwnersAndRequiresItsIdleContextThread()
    {
        ImGuiNativeContext* first = ImGui.Raw.CreateContext(null);
        void* registration = null;
        try
        {
            Assert.True(ImGui.Raw.InnoCreatePlatformWindowCallbacks(null, &SetPosition, &SetSize) == null);
            Assert.True(ImGui.Raw.InnoCreatePlatformWindowCallbacks(first, null, &SetSize) == null);
            ImGui.Raw.GetIO()->BackendPlatformUserData = (void*)1;
            Assert.True(ImGui.Raw.InnoCreatePlatformWindowCallbacks(first, &SetPosition, &SetSize) == null);
            Assert.Equal((nint)1, (nint)ImGui.Raw.GetIO()->BackendPlatformUserData);
            ImGui.Raw.GetIO()->BackendPlatformUserData = null;

            registration = ImGui.Raw.InnoCreatePlatformWindowCallbacks(first, &SetPosition, &SetSize);
            Assert.True(registration != null);
            Assert.Equal((nint)registration, (nint)ImGui.Raw.GetIO()->BackendPlatformUserData);
            Assert.True(ImGui.Raw.InnoCreatePlatformWindowCallbacks(first, &SetPosition, &SetSize) == null);
            Assert.Equal(1, ImGui.Raw.InnoCanDestroyPlatformWindowCallbacks(registration));
            nint address = (nint)registration;
            int foreignThreadResult = -1;
            Thread worker = new(() => foreignThreadResult = ImGui.Raw.InnoCanDestroyPlatformWindowCallbacks((void*)address))
            {
                IsBackground = true
            };
            worker.Start();
            Assert.True(worker.Join(TimeSpan.FromSeconds(10)));
            Assert.Equal(0, foreignThreadResult);

            ImGuiNativeContext* second = ImGui.Raw.CreateContext(null);
            try
            {
                ImGui.Raw.SetCurrentContext(second);
                Assert.Equal(0, ImGui.Raw.InnoCanDestroyPlatformWindowCallbacks(registration));
                Assert.True(ImGui.Raw.InnoCreatePlatformWindowCallbacks(first, &SetPosition, &SetSize) == null);
                void* other = ImGui.Raw.InnoCreatePlatformWindowCallbacks(second, &SetPosition, &SetSize);
                Assert.True(other != null);
                Assert.NotEqual((nint)registration, (nint)other);
                ImGui.Raw.InnoDestroyPlatformWindowCallbacks(other);
                Assert.True(ImGui.Raw.GetIO()->BackendPlatformUserData == null);
            }
            finally
            {
                ImGui.Raw.DestroyContext(second);
                ImGui.Raw.SetCurrentContext(first);
            }
        }
        finally
        {
            ImGui.Raw.SetCurrentContext(first);
            ImGui.Raw.InnoDestroyPlatformWindowCallbacks(registration);
            Assert.True(ImGui.Raw.GetIO()->BackendPlatformUserData == null);
            ImGui.Raw.DestroyContext(first);
        }
    }

    [Fact]
    public void NativeViewportDispatchPreservesScalarsAndRejectsRetirementInsideCallbacks()
    {
        ImGuiNativeContext* context = ImGui.Raw.CreateContext(null);
        CallbackState state = default;
        try
        {
            ImGuiIO* io = ImGui.Raw.GetIO();
            io->IniFilename = null;
            io->DisplaySize = new ImVec2C { X = 640, Y = 480 };
            io->DeltaTime = 1f / 60;
            io->ConfigFlags |= (int)ImGuiConfigFlags.ViewportsEnable;
            io->ConfigViewportsNoAutoMerge = 1;
            io->BackendFlags |= (int)(ImGuiBackendFlags.PlatformHasViewports
                | ImGuiBackendFlags.RendererHasViewports | ImGuiBackendFlags.RendererHasTextures);
            io->Fonts->RendererHasTextures = 1;
            io->BackendRendererUserData = &state;
            ImGui.Raw.GetMainViewport()->PlatformHandle = (void*)1;

            ImGuiPlatformIO* platform = ImGui.Raw.GetPlatformIO();
            platform->PlatformCreateWindow = &CreateWindow;
            platform->PlatformDestroyWindow = &DestroyWindow;
            platform->PlatformShowWindow = &NoOp;
            platform->PlatformSetWindowTitle = &SetTitle;
            ImGui.Raw.PlatformIOSetPlatformGetWindowPos(platform, &GetPosition);
            ImGui.Raw.PlatformIOSetPlatformGetWindowSize(platform, &GetSize);
            var monitor = (ImGuiPlatformMonitor*)ImGui.Raw.MemAlloc((nuint)sizeof(ImGuiPlatformMonitor));
            Assert.True(monitor != null);
            *monitor = new ImGuiPlatformMonitor
            {
                MainSize = new ImVec2C { X = 1920, Y = 1080 },
                WorkSize = new ImVec2C { X = 1920, Y = 1080 },
                DpiScale = 1
            };
            platform->Monitors = new ImVectorImGuiPlatformMonitor { Data = monitor, Size = 1, Capacity = 1 };
            state.registration = ImGui.Raw.InnoCreatePlatformWindowCallbacks(context, &SetPosition, &SetSize);
            Assert.True(state.registration != null);

            for (int frame = 0; frame < 4; frame++)
            {
                ImGui.NewFrame();
                ImGui.SetNextWindowPos(new Vector2(800 + frame * 17, 600 + frame * 11));
                ImGui.SetNextWindowSize(new Vector2(160 + frame * 13, 90 + frame * 7));
                bool open = true;
                _ = ImGui.Begin("Scalar callback ABI", ref open);
                ImGui.End();
                ImGui.Render();
                ImGui.Raw.UpdatePlatformWindows();
            }

            Assert.True(state.positions > 0);
            Assert.True(state.sizes > 0);
            Assert.False(state.invalid);
            Assert.Equal(1, ImGui.Raw.InnoCanDestroyPlatformWindowCallbacks(state.registration));
        }
        finally
        {
            ImGui.Raw.DestroyPlatformWindows();
            ImGui.Raw.InnoDestroyPlatformWindowCallbacks(state.registration);
            ImGui.Raw.GetIO()->BackendRendererUserData = null;
            ImGui.Raw.DestroyContext(context);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void SetPosition(
        ImGuiViewport* viewport,
        float x,
        float y
    ) {
        var state = (CallbackState*)viewport->PlatformUserData;
        state->positions++;
        state->invalid |= x != viewport->Pos.X || y != viewport->Pos.Y
            || ImGui.Raw.InnoCanDestroyPlatformWindowCallbacks(state->registration) != 0;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void SetSize(
        ImGuiViewport* viewport,
        float width,
        float height
    ) {
        var state = (CallbackState*)viewport->PlatformUserData;
        state->sizes++;
        state->invalid |= width != viewport->Size.X || height != viewport->Size.Y
            || ImGui.Raw.InnoCanDestroyPlatformWindowCallbacks(state->registration) != 0;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void CreateWindow(ImGuiViewport* viewport)
    {
        viewport->PlatformUserData = ImGui.Raw.GetIO()->BackendRendererUserData;
        viewport->PlatformHandle = (void*)1;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void DestroyWindow(ImGuiViewport* viewport)
    {
        viewport->PlatformUserData = null;
        viewport->PlatformHandle = null;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void NoOp(ImGuiViewport* viewport) { }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void SetTitle(
        ImGuiViewport* viewport,
        sbyte* title
    ) { }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void GetPosition(
        ImGuiViewport* viewport,
        ImVec2C* position
    ) => *position = viewport->Pos;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void GetSize(
        ImGuiViewport* viewport,
        ImVec2C* size
    ) => *size = viewport->Size;
}
