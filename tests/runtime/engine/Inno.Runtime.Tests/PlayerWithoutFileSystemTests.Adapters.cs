using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
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
using Inno.Core.Mathematics;
using Inno.Input;
using Inno.Platform;
using Inno.Rendering;
using Inno.Storage;
using Inno.Text;
using Inno.UI;

namespace Inno.Runtime.Tests;

public sealed partial class PlayerWithoutFileSystemTests
{
    private sealed class MemoryAdapters : IAdapterCatalog, IPlatformBackendFactory, IInputBackendFactory,
        IRenderingBackendFactory, IStorageBackendFactory, IAudioBackendFactory, ITextBackendFactory, IUIBackendFactory
    {
        private readonly MemoryRenderDevice m_render;

        public MemoryAdapters(bool failFrame)
        {
            this.application = new MemoryApplication(retired);
            m_render = new MemoryRenderDevice(retired, failFrame);
        }

        public MemoryApplication application { get; }
        public List<string> retired { get; } = [];
        public IPlatformBackendFactory platform => this;
        public IInputBackendFactory input => this;
        public IRenderingBackendFactory rendering => this;
        public IStorageBackendFactory storage => this;
        public IAudioBackendFactory audio => this;
        public ITextBackendFactory text => this;
        public IUIBackendFactory ui => this;
        IReadOnlyList<PlatformBackendID> IPlatformBackendFactory.supportedBackends => [PlatformBackendID.sdl3];
        IReadOnlyList<InputBackendID> IInputBackendFactory.supportedBackends => [InputBackendID.events];
        IReadOnlyList<RenderingBackendID> IRenderingBackendFactory.supportedBackends => [RenderingBackendID.bgfx];
        IReadOnlyList<StorageBackendID> IStorageBackendFactory.supportedBackends => [StorageBackendID.fileSystem];
        IReadOnlyList<AudioBackendID> IAudioBackendFactory.supportedBackends => [AudioBackendID.miniAudio];
        IReadOnlyList<TextBackendID> ITextBackendFactory.supportedBackends => [TextBackendID.freeTypeHarfBuzz];
        IReadOnlyList<UIBackendID> IUIBackendFactory.supportedBackends => [UIBackendID.rmlUI];

        public IPlatformApplication CreateApplication(PlatformBackendID backend) => application;

        public IInputEventSource CreateEventSource(
            InputBackendID backend,
            IPlatformWindow window,
            bool acceptAllWindows
        ) => new MemoryInputSource(retired);

        public IRenderDevice CreateDevice(
            RenderingBackendID backend,
            RenderingBackendOptions options
        ) => m_render;

        public IAudioDevice CreateDevice(
            AudioBackendID backend,
            AudioBackendOptions options = default
        ) => new MutedAudioDevice();

        public IApplicationStorage CreateStorage(
            StorageBackendID backend,
            StorageScope scope
        ) => new MemoryStorage();

        public ITextBackend CreateBackend(TextBackendID backend) => new MemoryText(retired);

        public IUIBackend CreateBackend(UIBackendID backend) => new MemoryUI(retired);

        public IRenderLayerCompositionProgramProvider CreateCompositionProgramProvider(RenderingBackendID backend)
            => new UnusedCompositionPrograms();

        internal int completedFrames => m_render.completedFrames;
    }

    private sealed class MemoryApplication(List<string> retired) : IPlatformApplication
    {
        private Action<uint>? m_redraw;
        public Queue<Event> events { get; } = new();
        public int redrawSubscribers { get; private set; }

        public event Action<uint>? redrawRequested
        {
            add
            {
                m_redraw += value;
                redrawSubscribers++;
            }
            remove
            {
                m_redraw -= value;
                redrawSubscribers--;
            }
        }

        public IPlatformWindow CreateWindow(PlatformWindowOptions options) => new MemoryWindow(retired);

        public bool PollEvent(out Event? evnt) => events.TryDequeue(out evnt);

        public IReadOnlyList<IPlatformWindow> GetWindows() => [];

        public void RequestRedraw() => m_redraw?.Invoke(1);

        public void Dispose() => retired.Add("platform");
    }

    private sealed class MemoryWindow(List<string> retired) : IPlatformWindow
    {
        public uint windowID => 1;
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

        public GraphicsCapabilities capabilities { get; } = new(
            GraphicsApi.noop,
            GraphicsCapability.None,
            new GraphicsLimits(64, 4, 4096, 0),
            [RenderTextureFormat.RGBA8],
            [RenderTextureFormat.RGBA8],
            [],
            [],
            false,
            false
        );

        public uint generation => 1;
        public RenderPresentationSize? primaryPresentationSize { get; private set; }
        public bool primaryPresentationEncodesSrgb => true;

        public void SetVerticalSync(bool enabled) {}

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

    private sealed class MemoryUI(List<string> retired) : IUIBackend
    {
        public string implementationID => "tests.ui";
        public UIBackendCapabilities capabilities { get; } = new([new UIDocumentLanguageID("tests.ui-language")], true);

        public UIContextHandle CreateContext(UIContextOptions options) => new(1);

        public void DestroyContext(UIContextHandle context) {}

        public void SetViewport(
            UIContextHandle context,
            int width,
            int height,
            float density
        ) {}

        public UIDocumentHandle LoadDocument(
            UIContextHandle context,
            UIDocumentSource source
        ) => new(1);

        public void ShowDocument(
            UIContextHandle context,
            UIDocumentHandle document
        ) {}

        public void HideDocument(
            UIContextHandle context,
            UIDocumentHandle document
        ) {}

        public void CloseDocument(
            UIContextHandle context,
            UIDocumentHandle document
        ) {}

        public bool SetText(
            UIContextHandle context,
            UIDocumentHandle document,
            string elementID,
            string text
        ) => true;

        public bool SetContent(
            UIContextHandle context,
            UIDocumentHandle document,
            string elementID,
            UIDocumentFragment content
        ) => true;

        public bool SetAttribute(
            UIContextHandle context,
            UIDocumentHandle document,
            string elementID,
            string name,
            string value
        ) => true;

        public bool SetClass(
            UIContextHandle context,
            UIDocumentHandle document,
            string elementID,
            string className,
            bool active
        ) => true;

        public void RegisterFont(UIFontRegistration registration) {}

        public void RegisterTexture(
            UIContextHandle context,
            string source,
            UITextureData texture
        ) {}

        public bool HasElementAtPoint(
            UIContextHandle context,
            Vector2 position
        ) => false;

        public void Update(
            UIContextHandle context,
            UIInputSnapshot input
        ) => lastInput = input;

        public UIRenderFrame Render(UIContextHandle context) => UIRenderFrame.empty;

        public IReadOnlyList<UIEvent> DrainEvents(UIContextHandle context) => [];

        public void Dispose() => retired.Add("ui");

        internal UIInputSnapshot? lastInput { get; private set; }
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
            uint glyphID,
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
