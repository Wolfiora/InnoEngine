using System;
using System.Collections.Generic;
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
using Inno.Audio.Runtime;
using Inno.Core.Events;
using Inno.Input;
using Inno.Platform;
using Inno.Rendering;
using Inno.Storage;
using Inno.Text;
using Inno.UI;
using Xunit;

namespace Inno.Runtime.Tests;

public sealed partial class PlayerWithoutFileSystemTests
{
    private sealed class MemoryAdapters : IAdapterCatalog, IPlatformBackendFactory, IInputBackendFactory,
        IRenderingBackendFactory, IStorageBackendFactory, IAudioBackendFactory, ITextBackendFactory, IUiBackendFactory
    {
        private readonly MemoryRenderDevice m_render;
        public MemoryAdapters(bool failFrame)
        {
            application = new MemoryApplication(retired);
            m_render = new MemoryRenderDevice(retired, failFrame);
        }
        internal int completedFrames => m_render.completedFrames;
        public MemoryApplication application { get; }
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
        ) => new MemoryInputSource(retired);
        public IRenderDevice CreateDevice(
            RenderingBackendId backend,
            RenderingBackendOptions options
        ) => m_render;
        public IApplicationStorage CreateStorage(
            StorageBackendId backend,
            StorageScope scope
        ) => new MemoryStorage();
        public IAudioDevice CreateDevice(
            AudioBackendId backend,
            AudioBackendOptions options = default
        ) => new MutedAudioDevice();
        public ITextBackend CreateBackend(TextBackendId backend) => new MemoryText(retired);
        public IUiBackend CreateBackend(UiBackendId backend) => new MemoryUi(retired);
        public IRenderLayerCompositionProgramProvider CreateCompositionProgramProvider(RenderingBackendId backend)
            => new UnusedCompositionPrograms();
    }

    private sealed class MemoryApplication(List<string> retired) : IPlatformApplication
    {
        private Action<uint>? m_redraw;
        public Queue<Event> events { get; } = new();
        public int redrawSubscribers { get; private set; }
        public event Action<uint>? redrawRequested
        {
            add { m_redraw += value; redrawSubscribers++; }
            remove { m_redraw -= value; redrawSubscribers--; }
        }
        public IPlatformWindow CreateWindow(PlatformWindowOptions options) => new MemoryWindow(retired);
        public bool PollEvent(out Event? evnt) => events.TryDequeue(out evnt);
        public IReadOnlyList<IPlatformWindow> GetWindows() => [];
        public void RequestRedraw() => m_redraw?.Invoke(1);
        public void Dispose() => retired.Add("platform");
    }

    private sealed class MemoryWindow(List<string> retired) : IPlatformWindow
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

    private sealed class MemoryInputSource(List<string> retired) : IInputEventSource
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

    private sealed class MemoryRenderDevice(
        List<string> retired,
        bool failFrame
    ) : IRenderDevice
    {
        private bool m_rejectedFrame;
        internal int completedFrames;
        public GraphicsCapabilities capabilities { get; } = new(GraphicsApi.Noop, GraphicsCapability.None,
            new GraphicsLimits(64, 4, 4096, 0), [RenderTextureFormat.RGBA8], [RenderTextureFormat.RGBA8], [], [], false, false);
        public uint generation => 1;
        public RenderPresentationSize? primaryPresentationSize { get; private set; }
        public bool primaryPresentationEncodesSrgb => true;
        public void SetVerticalSync(bool enabled) { }
        public void BeginFrame()
        {
            if (failFrame && !m_rejectedFrame)
            {
                m_rejectedFrame = true;
                throw new InvalidOperationException("Expected frame failure.");
            }
        }
        public uint EndFrame() => 1;
        public void Execute(
            CompiledRenderGraph graph,
            ulong frameIndex
        ) {
            Assert.NotNull(graph);
            completedFrames++;
        }
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
    private sealed class MemoryUi(List<string> retired) : IUiBackend
    {
        internal UiInputSnapshot? lastInput { get; private set; }

        public string implementationId => "tests.ui";
        public UiBackendCapabilities capabilities { get; } = new([new("tests.ui-language")], true);

        public UiContextHandle CreateContext(UiContextOptions options) => new(1);
        public void DestroyContext(UiContextHandle context) { }
        public void SetViewport(
            UiContextHandle context,
            int width,
            int height,
            float density
        ) { }
        public UiDocumentHandle LoadDocument(
            UiContextHandle context,
            UiDocumentSource source
        ) => new(1);
        public void ShowDocument(
            UiContextHandle context,
            UiDocumentHandle document
        ) { }
        public void HideDocument(
            UiContextHandle context,
            UiDocumentHandle document
        ) { }
        public void CloseDocument(
            UiContextHandle context,
            UiDocumentHandle document
        ) { }
        public bool SetText(
            UiContextHandle context,
            UiDocumentHandle document,
            string elementId,
            string text
        ) => true;
        public bool SetContent(
            UiContextHandle context,
            UiDocumentHandle document,
            string elementId,
            UiDocumentFragment content
        ) => true;
        public bool SetAttribute(
            UiContextHandle context,
            UiDocumentHandle document,
            string elementId,
            string name,
            string value
        ) => true;
        public bool SetClass(
            UiContextHandle context,
            UiDocumentHandle document,
            string elementId,
            string className,
            bool active
        ) => true;
        public void RegisterFont(UiFontRegistration registration) { }
        public void RegisterTexture(
            UiContextHandle context,
            string source,
            UiTextureData texture
        ) { }
        public bool HasElementAtPoint(
            UiContextHandle context,
            Inno.Core.Mathematics.Vector2 position
        ) => false;
        public void Update(
            UiContextHandle context,
            UiInputSnapshot input
        ) => lastInput = input;
        public UiRenderFrame Render(UiContextHandle context) => UiRenderFrame.empty;
        public IReadOnlyList<UiEvent> DrainEvents(UiContextHandle context) => [];
        public void Dispose() => retired.Add("ui");
    }

    private sealed class UnusedCompositionPrograms : IRenderLayerCompositionProgramProvider
    {
        public GraphicsPipelineDescriptor CreateDescriptor(
            GraphicsCapabilities capabilities,
            RenderVertexLayout vertexLayout
        ) => throw new InvalidOperationException("An empty scene has no layer composition program.");

        public GraphicsPipelineDescriptor CreateOutputTransferDescriptor(
            GraphicsCapabilities capabilities,
            RenderVertexLayout vertexLayout
        ) => throw new InvalidOperationException("An empty scene has no output transfer program.");
    }

    private sealed class MemoryText(List<string> retired) : ITextBackend
    {
        public TextFontHandle LoadFont(
            ReadOnlySpan<byte> data,
            int faceIndex
        ) => throw new NotSupportedException();
        public void ReleaseFont(TextFontHandle font) => throw new NotSupportedException();
        public TextLayout Shape(
            TextFontHandle font,
            string text,
            TextStyle style,
            TextShapingOptions options
        ) => throw new NotSupportedException();
        public GlyphBitmap Rasterize(
            TextFontHandle font,
            uint glyphId,
            float fontSize
        ) => throw new NotSupportedException();
        public void Dispose() => retired.Add("text");
    }

    private sealed class MemoryStorage : IApplicationStorage
    {
        public ValueTask<bool> ExistsAsync(
            StorageKey key,
            CancellationToken cancellationToken = default
        ) => ValueTask.FromResult(false);
        public ValueTask<byte[]?> ReadAsync(
            StorageKey key,
            CancellationToken cancellationToken = default
        ) => ValueTask.FromResult<byte[]?>(null);
        public ValueTask WriteAsync(
            StorageKey key,
            ReadOnlyMemory<byte> value,
            CancellationToken cancellationToken = default
        ) => throw new NotSupportedException();
        public ValueTask<bool> DeleteAsync(
            StorageKey key,
            CancellationToken cancellationToken = default
        ) => ValueTask.FromResult(false);
        public ValueTask<IReadOnlyList<StorageKey>> ListAsync(
            StorageKey? prefix = null,
            CancellationToken cancellationToken = default
        ) => ValueTask.FromResult<IReadOnlyList<StorageKey>>([]);
    }
}
