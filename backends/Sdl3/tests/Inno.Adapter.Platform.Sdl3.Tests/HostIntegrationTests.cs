using System;
using Inno.Integration.Windows.Sdl3;
using Inno.Adapter.Platform;
using Inno.Native.Sdl3;
using Inno.Platform;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Inno.Adapter.Platform.Sdl3.Tests;

public sealed unsafe class HostIntegrationTests
{
    [Fact]
    public void MissingHostIsRejectedBeforeSdkInitialization()
    {
        Assert.Throws<ArgumentNullException>(() => new Sdl3PlatformApplication(null!));
        Assert.Throws<ArgumentNullException>(() => new Sdl3PlatformBackendProvider(null!));
    }

    [Fact]
    public void WrappingFailureDestroysOnlyTheNewlyOwnedWindow()
    {
        var host = new FailingHost();
        using var app = new Sdl3PlatformApplication(host);
        Assert.Throws<InvalidOperationException>(() => app.CreateWindow(new PlatformWindowOptions
        { title = "Failed surface", width = 40, height = 40, visible = false }));
        Assert.Empty(app.GetWindows());
        Assert.True(SDL.GetWindowFromID(host.lastId).IsNull);
    }

    [Fact]
    public void SingleWindowCapabilityRejectsCreationBeforeCallingHostAgain()
    {
        var host = new FailingHost { failSurface = false };
        using var app = new Sdl3PlatformApplication(host);
        using var window = app.CreateWindow(new PlatformWindowOptions
        { title = "Single surface", width = 40, height = 40, visible = false });
        Assert.Throws<NotSupportedException>(() => app.CreateWindow(new PlatformWindowOptions()));
        Assert.Equal(1, host.calls);
    }

    private sealed class FailingHost : ISdl3HostIntegration
    {
        private readonly WindowsSdl3HostIntegration m_inner = new();
        internal bool failSurface = true;
        internal uint lastId;
        internal int calls;
        public Sdl3HostCapabilities capabilities { get; } = new(multipleWindows: false, liveResize: false);
        public void ConfigureInitialization() => m_inner.ConfigureInitialization();
        public nint CreateWindow(PlatformWindowOptions options)
        {
            calls++;
            nint handle = m_inner.CreateWindow(options);
            lastId = SDL.GetWindowID(new SDLWindow(handle));
            return handle;
        }
        public PlatformNativeHandles ResolveNativeSurface(nint windowHandle) => failSurface
            ? throw new InvalidOperationException("Injected surface inspection failure.") : m_inner.ResolveNativeSurface(windowHandle);
        public bool GetInitialFocus(nint windowHandle) => false;
    }
}
