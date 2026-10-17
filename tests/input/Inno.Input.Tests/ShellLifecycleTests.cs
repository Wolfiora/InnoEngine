using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter;
using Inno.Adapter.Audio;
using Inno.Adapter.Input;
using Inno.Adapter.Platform;
using Inno.Adapter.Rendering;
using Inno.Adapter.Storage;
using Inno.Adapter.Text;
using Inno.Adapter.UI;
using Inno.Audio;
using Inno.Core.Events;
using Inno.Input;
using Inno.Platform;
using Inno.Rendering;
using Inno.Shell;
using Inno.Storage;
using Inno.Text;
using Inno.UI;
using Xunit;

namespace Inno.Input.Tests;

public sealed class ShellLifecycleTests
{
    [Fact]
    public async Task SuspensionReasonsComposeWithoutFramesOrBackgroundClockCatchUp()
    {
        var catalog = new TestCatalog();
        var elapsed = Stopwatch.StartNew();
        using var shell = new TestShell(catalog, suspendWhenHidden: true);
        Assert.Equal(ShellState.Ready, shell.state);
        int opportunities = 0;
        var driver = new ScheduledShellFrameDriver(_ =>
        {
            switch (opportunities++)
            {
                case 1:
                    catalog.application.events.Enqueue(new WindowVisibilityChangedEvent(1, false));
                    catalog.application.events.Enqueue(new ApplicationSuspensionChangedEvent(true));
                    break;
                case 2:
                    Assert.Equal(ShellState.Suspended, shell.state);
                    Assert.Single(shell.frames);
                    catalog.application.events.Enqueue(new ApplicationSuspensionChangedEvent(false));
                    catalog.application.RequestRedraw();
                    Thread.Sleep(150);
                    break;
                case 3:
                    Assert.Equal(ShellState.Suspended, shell.state);
                    Assert.Single(shell.frames);
                    catalog.application.events.Enqueue(new WindowVisibilityChangedEvent(1, true));
                    break;
            }
            return ValueTask.CompletedTask;
        });
        Assert.Equal(0, await shell.RunAsync(driver, smokeFrameLimit: 2));
        Assert.Equal(4, opportunities);
        Assert.Equal([true, false], shell.suspensions);
        Assert.Equal(2, shell.frames.Count);
        Assert.True(shell.frames[1].totalTime < elapsed.Elapsed.TotalSeconds - 0.1);
        Assert.Equal(2, shell.smokeFrames);
        Assert.Equal(ShellState.Stopped, shell.state);
        Assert.Equal(0, catalog.application.redrawSubscribers);
        await Assert.ThrowsAsync<InvalidOperationException>(() => shell.RunAsync(driver));
        shell.Dispose();
        Assert.Equal(ShellState.Disposed, shell.state);
        Assert.Equal(["render", "input", "window", "platform"], catalog.retired);
    }

    [Fact]
    public async Task SecondaryWindowAndVisibilityPolicyDoNotSuspendTheHost()
    {
        var catalog = new TestCatalog();
        using var shell = new TestShell(catalog, suspendWhenHidden: false);
        catalog.application.events.Enqueue(new WindowVisibilityChangedEvent(1, false));
        catalog.application.events.Enqueue(new WindowVisibilityChangedEvent(2, false));
        await shell.RunAsync(new PollingShellFrameDriver(), smokeFrameLimit: 1);
        Assert.Empty(shell.suspensions);
        Assert.Single(shell.frames);
        Assert.Equal(ShellState.Stopped, shell.state);
    }

    [Fact]
    public async Task ExecutionAndStoppingFailuresAreBothRetainedAndCallbacksDetach()
    {
        var catalog = new TestCatalog();
        using var shell = new TestShell(catalog, suspendWhenHidden: true)
        {
            frameFailure = new InvalidOperationException("frame failure"),
            stoppingFailure = new InvalidOperationException("stopping failure")
        };
        AggregateException failure = await Assert.ThrowsAsync<AggregateException>(
            () => shell.RunAsync(new PollingShellFrameDriver(), smokeFrameLimit: 1));
        Assert.Contains(shell.frameFailure, failure.InnerExceptions);
        Assert.Contains(shell.stoppingFailure, failure.InnerExceptions);
        Assert.Equal(ShellState.Faulted, shell.state);
        Assert.Equal(0, catalog.application.redrawSubscribers);
        Assert.Equal(0, shell.smokeFrames);
        shell.Dispose();
        Assert.Equal(["render", "input", "window", "platform"], catalog.retired);
    }

    [Fact]
    public async Task CancellationStopsWithoutClaimingSmokeCompletion()
    {
        var catalog = new TestCatalog();
        using var shell = new TestShell(catalog, suspendWhenHidden: true);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => shell.RunAsync(
            new PollingShellFrameDriver(), smokeFrameLimit: 1, cancellationToken: cancellation.Token));
        Assert.Equal(ShellState.Stopped, shell.state);
        Assert.Equal(0, shell.smokeFrames);
        Assert.Empty(shell.frames);
        Assert.Equal(0, catalog.application.redrawSubscribers);
    }

    private sealed class TestShell : Inno.Shell.Shell
    {
        public TestShell(
            TestCatalog catalog,
            bool suspendWhenHidden
        ) : base(catalog, new ShellOptions { adapters = new Inno.Adapter.AdapterSelection {
                platform = Inno.Adapter.Platform.PlatformBackendId.sdl3,
                input = Inno.Adapter.Input.InputBackendId.events,
                rendering = Inno.Adapter.Rendering.RenderingBackendId.bgfx,
                storage = Inno.Adapter.Storage.StorageBackendId.fileSystem,
                audio = Inno.Adapter.Audio.AudioBackendId.miniAudio,
                text = Inno.Adapter.Text.TextBackendId.freeTypeHarfBuzz,
                ui = Inno.Adapter.UI.UiBackendId.rmlUi
            }, suspendWhenHidden = suspendWhenHidden }) {
            InitializeAdapterResources();
        }

        public List<ShellFrame> frames { get; } = [];
        public List<bool> suspensions { get; } = [];
        public Exception? frameFailure { get; init; }
        public Exception? stoppingFailure { get; init; }
        public int smokeFrames { get; private set; }

        protected override void OnFrame(ShellFrame frame)
        {
            if (frameFailure is not null)
                throw frameFailure;
            frames.Add(frame);
        }

        protected override void OnSuspensionChanged(bool isSuspended) => suspensions.Add(isSuspended);
        protected override void OnSmokeCompleted(int frameCount) => smokeFrames = frameCount;
        protected override void OnStopping()
        {
            if (stoppingFailure is not null)
                throw stoppingFailure;
        }
    }

    private sealed class TestCatalog : IAdapterCatalog, IPlatformBackendFactory, IInputBackendFactory,
        IRenderingBackendFactory, IStorageBackendFactory, IAudioBackendFactory, ITextBackendFactory, IUiBackendFactory
    {
        public TestCatalog() => application = new TestApplication(retired);
        public TestApplication application { get; }
        public List<string> retired { get; } = [];
        public IPlatformBackendFactory platform => this;
        public IInputBackendFactory input => this;
        public IRenderingBackendFactory rendering => this;
        public IStorageBackendFactory storage => this;
        public IAudioBackendFactory audio => this;
        public ITextBackendFactory text => this;
        public IUiBackendFactory ui => this;
        IReadOnlyList<PlatformBackendId> IPlatformBackendFactory.supportedBackends => [PlatformBackendId.sdl3];
        IReadOnlyList<InputBackendId> IInputBackendFactory.supportedBackends => [InputBackendId.events];
        IReadOnlyList<RenderingBackendId> IRenderingBackendFactory.supportedBackends => [RenderingBackendId.bgfx];
        IReadOnlyList<StorageBackendId> IStorageBackendFactory.supportedBackends => [StorageBackendId.fileSystem];
        IReadOnlyList<AudioBackendId> IAudioBackendFactory.supportedBackends => [AudioBackendId.miniAudio];
        IReadOnlyList<TextBackendId> ITextBackendFactory.supportedBackends => [TextBackendId.freeTypeHarfBuzz];
        IReadOnlyList<UiBackendId> IUiBackendFactory.supportedBackends => [UiBackendId.rmlUi];
        public IPlatformApplication CreateApplication(PlatformBackendId backend) => application;
        public IInputEventSource CreateEventSource(
            InputBackendId backend,
            IPlatformWindow window,
            bool acceptAllWindows
        ) => new TestInputSource(retired);
        public IRenderDevice CreateDevice(
            RenderingBackendId backend,
            RenderingBackendOptions options
        ) => new TestRenderDevice(retired);
        public IApplicationStorage CreateStorage(
            StorageBackendId backend,
            StorageScope scope
        ) => throw new NotSupportedException();
        public IAudioDevice CreateDevice(
            AudioBackendId backend,
            AudioBackendOptions options = default
        ) => throw new NotSupportedException();
        public ITextBackend CreateBackend(TextBackendId backend) => throw new NotSupportedException();
        public IUiBackend CreateBackend(UiBackendId backend) => throw new NotSupportedException();
        public IRenderLayerCompositionProgramProvider CreateCompositionProgramProvider(RenderingBackendId backend)
            => throw new NotSupportedException();
    }

    private sealed class TestApplication(List<string> retired) : IPlatformApplication
    {
        private Action<uint>? m_redraw;
        public Queue<Event> events { get; } = new();
        public int redrawSubscribers { get; private set; }
        public event Action<uint>? redrawRequested
        {
            add { m_redraw += value; redrawSubscribers++; }
            remove { m_redraw -= value; redrawSubscribers--; }
        }
        public IPlatformWindow CreateWindow(PlatformWindowOptions options) => new TestWindow(retired);
        public bool PollEvent(out Event? evnt) => events.TryDequeue(out evnt);
        public IReadOnlyList<IPlatformWindow> GetWindows() => [];
        public void RequestRedraw() => m_redraw?.Invoke(1);
        public void Dispose() => retired.Add("platform");
    }

    private sealed class TestWindow(List<string> retired) : IPlatformWindow
    {
        public uint windowId => 1;
        public string title => "Lifecycle fixture";
        public int width => 32;
        public int height => 32;
        public int pixelWidth => 32;
        public int pixelHeight => 32;
        public bool isClosed { get; private set; }
        public bool isFocused => true;
        public void RequestClose() => isClosed = true;
        public void Dispose() => retired.Add("window");
    }

    private sealed class TestInputSource(List<string> retired) : IInputEventSource
    {
        private readonly EventInputSource m_source = new(1);
        public IInputBackend CreateBackend() => m_source.CreateBackend();
        public void ProcessEvent(Event evnt) => m_source.ProcessEvent(evnt);
        public void Dispose()
        {
            m_source.Dispose();
            retired.Add("input");
        }
    }

    private sealed class TestRenderDevice(List<string> retired) : IRenderDevice
    {
        public GraphicsCapabilities capabilities => throw new NotSupportedException();
        public uint generation => 1;
        public RenderPresentationSize? primaryPresentationSize { get; private set; }
        public bool primaryPresentationEncodesSrgb => true;
        public void SetVerticalSync(bool enabled) { }
        public void BeginFrame() => throw new NotSupportedException();
        public uint EndFrame() => throw new NotSupportedException();
        public void Execute(
            CompiledRenderGraph graph,
            ulong frameIndex
        ) => throw new NotSupportedException();
        public void SetPrimaryPresentationSize(RenderPresentationSize? size) => primaryPresentationSize = size;
        public PersistentTextureHandle CreateTexture(
            RenderTextureDescriptor descriptor,
            string name
        ) => throw new NotSupportedException();
        public void UpdateTexture(
            PersistentTextureHandle texture,
            ReadOnlySpan<byte> data,
            int mipLevel = 0,
            int arrayLayer = 0
        ) => throw new NotSupportedException();
        public void UpdateTextureRegion(
            PersistentTextureHandle texture,
            RenderTextureRegion region,
            ReadOnlySpan<byte> data
        ) => throw new NotSupportedException();
        public RenderTextureReadbackHandle BeginTextureReadback(
            PersistentTextureHandle texture,
            int mipLevel = 0
        ) => throw new NotSupportedException();
        public bool TryGetTextureReadback(
            RenderTextureReadbackHandle readback,
            out RenderTextureReadbackResult? result
        ) => throw new NotSupportedException();
        public void CancelTextureReadback(RenderTextureReadbackHandle readback) => throw new NotSupportedException();
        public void DestroyTexture(PersistentTextureHandle texture) => throw new NotSupportedException();
        public PersistentBufferHandle CreateBuffer(
            PersistentBufferDescriptor descriptor,
            ReadOnlySpan<byte> initialData,
            string name
        ) => throw new NotSupportedException();
        public void UpdateBuffer(
            PersistentBufferHandle buffer,
            ReadOnlySpan<byte> data,
            int startElement = 0
        ) => throw new NotSupportedException();
        public void DestroyBuffer(PersistentBufferHandle buffer) => throw new NotSupportedException();
        public GraphicsPipelineHandle CreateGraphicsPipeline(
            GraphicsPipelineDescriptor descriptor,
            string name
        ) => throw new NotSupportedException();
        public void DestroyGraphicsPipeline(GraphicsPipelineHandle pipeline) => throw new NotSupportedException();
        public ComputePipelineHandle CreateComputePipeline(
            ComputePipelineDescriptor descriptor,
            string name
        ) => throw new NotSupportedException();
        public void DestroyComputePipeline(ComputePipelineHandle pipeline) => throw new NotSupportedException();
        public void Dispose() => retired.Add("render");
    }
}
