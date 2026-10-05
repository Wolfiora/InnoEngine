using System;
using System.Collections.Generic;
using Inno.Adapter;
using Inno.Adapter.Platform;
using Inno.Adapter.Input;
using Inno.Adapter.Storage;
using Inno.Adapter.Audio;
using Inno.Adapter.Text;
using Inno.Adapter.Rendering;
using Inno.Adapter.UI;
using Inno.Adapter.Presentation;
using Inno.Platform;
using Inno.Storage;
using Inno.Audio;
using Inno.Text;
using Inno.Rendering;
using Inno.UI;
using Xunit;

namespace Inno.Runtime.Tests;

public sealed class AdapterCatalogTests
{
    [Fact]
    public void PresentationRegistrationUsesAnImmutableOpenCatalog()
    {
        var first = new TestPresentationProvider(new PresentationBackendId("test.presentation.first"));
        var second = new TestPresentationProvider(new PresentationBackendId("test.presentation.second"));
        Assert.Throws<ArgumentException>(() => new PresentationBackendCatalog([first, first]));
        Assert.Throws<ArgumentException>(() => new PresentationBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestPresentationProvider(default));
        var providers = new List<PresentationBackendProvider> { first };
        var catalog = new PresentationBackendCatalog(providers);
        providers.Clear();
        providers.Add(second);
        Assert.Equal(first.id, Assert.Single(catalog.supportedBackends));
        Assert.Equal(0, first.creationCount);
        // This provider intentionally does not consume host resources; the catalog validates its return contract.
        var options = new PresentationBackendOptions { window = null!, platformApplication = null!, renderDevice = null! };
        Assert.Throws<NotSupportedException>(() => catalog.CreateContext(second.id, options));
        Assert.Throws<ArgumentNullException>(() => catalog.CreateContext(first.id, null!));
        Assert.Throws<InvalidOperationException>(() => catalog.CreateContext(first.id, options));
        Assert.Equal(1, first.creationCount);
        Assert.Equal(0, second.creationCount);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<PresentationBackendId>)catalog.supportedBackends).Add(second.id));
    }

    [Fact]
    public void CustomIdentitiesAreAcceptedWithoutCreatingServices()
    {
        var catalog = new TestCatalog();
        catalog.selection.Validate(catalog);
        Assert.Equal(0, catalog.creationCount);
        Assert.Throws<NotSupportedException>(() =>
            new AdapterSelection { platform = new PlatformBackendId("missing.platform") }.Validate(catalog));
        Assert.Equal(0, catalog.creationCount);
    }

    [Fact]
    public void AllDomainsRejectDuplicateAndNullProviders()
    {
        var platform = new TestPlatformProvider(new PlatformBackendId("test.platform"));
        Assert.Throws<ArgumentException>(() => new PlatformBackendCatalog([platform, platform]));
        Assert.Throws<ArgumentException>(() => new PlatformBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestPlatformProvider(default));
        var input = new TestInputProvider(new InputBackendId("test.input"));
        Assert.Throws<ArgumentException>(() => new InputBackendCatalog([input, input]));
        Assert.Throws<ArgumentException>(() => new InputBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestInputProvider(default));
        var storage = new TestStorageProvider(new StorageBackendId("test.storage"));
        Assert.Throws<ArgumentException>(() => new StorageBackendCatalog([storage, storage]));
        Assert.Throws<ArgumentException>(() => new StorageBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestStorageProvider(default));
        var audio = new TestAudioProvider(new AudioBackendId("test.audio"));
        Assert.Throws<ArgumentException>(() => new AudioBackendCatalog([audio, audio]));
        Assert.Throws<ArgumentException>(() => new AudioBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestAudioProvider(default));
        var text = new TestTextProvider(new TextBackendId("test.text"));
        Assert.Throws<ArgumentException>(() => new TextBackendCatalog([text, text]));
        Assert.Throws<ArgumentException>(() => new TextBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestTextProvider(default));
        var rendering = new TestRenderingProvider(new RenderingBackendId("test.rendering"));
        Assert.Throws<ArgumentException>(() => new RenderingBackendCatalog([rendering, rendering]));
        Assert.Throws<ArgumentException>(() => new RenderingBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestRenderingProvider(default));
        var ui = new TestUiProvider(new UiBackendId("test.ui"));
        Assert.Throws<ArgumentException>(() => new UiBackendCatalog([ui, ui]));
        Assert.Throws<ArgumentException>(() => new UiBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestUiProvider(default));
    }

    [Fact]
    public void CapturedRegistrationsCannotBeChangedByTheirSourceCollection()
    {
        var first = new TestStorageProvider(new StorageBackendId("test.storage.first"));
        var second = new TestStorageProvider(new StorageBackendId("test.storage.second"));
        var providers = new List<StorageBackendProvider> { first };
        var catalog = new StorageBackendCatalog(providers);
        providers.Clear();
        providers.Add(second);
        Assert.Equal(first.id, Assert.Single(catalog.supportedBackends));
        Assert.Throws<NotSupportedException>(() => catalog.CreateStorage(second.id, "application"));
        Assert.Equal(0, first.creationCount);
        Assert.Throws<InvalidOperationException>(() => catalog.CreateStorage(first.id, "application"));
        Assert.Equal(1, first.creationCount);
        Assert.Equal(0, second.creationCount);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<StorageBackendId>)catalog.supportedBackends).Add(second.id));
    }

    [Fact]
    public void InvalidIdentifiersAndUnassignedSelectionsFailAtTheBoundary()
    {
        Assert.Throws<ArgumentException>(() => new PlatformBackendId("invalid id"));
        Assert.False(default(PlatformBackendId).isValid);
        Assert.Throws<ArgumentException>(() => new InputBackendId("invalid id"));
        Assert.False(default(InputBackendId).isValid);
        Assert.Throws<ArgumentException>(() => new StorageBackendId("invalid id"));
        Assert.False(default(StorageBackendId).isValid);
        Assert.Throws<ArgumentException>(() => new AudioBackendId("invalid id"));
        Assert.False(default(AudioBackendId).isValid);
        Assert.Throws<ArgumentException>(() => new TextBackendId("invalid id"));
        Assert.False(default(TextBackendId).isValid);
        Assert.Throws<ArgumentException>(() => new RenderingBackendId("invalid id"));
        Assert.False(default(RenderingBackendId).isValid);
        Assert.Throws<ArgumentException>(() => new UiBackendId("invalid id"));
        Assert.False(default(UiBackendId).isValid);
        Assert.Throws<NotSupportedException>(() => default(AdapterSelection).Validate(new TestCatalog()));
    }

    private sealed class TestCatalog : IAdapterCatalog
    {
        private readonly TestPlatformProvider m_platform = new(new PlatformBackendId("test.platform"));
        private readonly TestInputProvider m_input = new(new InputBackendId("test.input"));
        private readonly TestStorageProvider m_storage = new(new StorageBackendId("test.storage"));
        private readonly TestAudioProvider m_audio = new(new AudioBackendId("test.audio"));
        private readonly TestTextProvider m_text = new(new TextBackendId("test.text"));
        private readonly TestRenderingProvider m_rendering = new(new RenderingBackendId("test.rendering"));
        private readonly TestUiProvider m_ui = new(new UiBackendId("test.ui"));

        public TestCatalog()
        {
            platform = new PlatformBackendCatalog([m_platform]);
            input = new InputBackendCatalog([m_input]);
            storage = new StorageBackendCatalog([m_storage]);
            audio = new AudioBackendCatalog([m_audio]);
            text = new TextBackendCatalog([m_text]);
            rendering = new RenderingBackendCatalog([m_rendering]);
            ui = new UiBackendCatalog([m_ui]);
        }

        public AdapterSelection selection => new()
        {
            platform = m_platform.id,
            input = m_input.id,
            storage = m_storage.id,
            audio = m_audio.id,
            text = m_text.id,
            rendering = m_rendering.id,
            ui = m_ui.id
        };

        public int creationCount => m_platform.creationCount + m_input.creationCount + m_storage.creationCount + m_audio.creationCount + m_text.creationCount + m_rendering.creationCount + m_ui.creationCount;

        public IPlatformBackendFactory platform { get; }
        public IInputBackendFactory input { get; }
        public IStorageBackendFactory storage { get; }
        public IAudioBackendFactory audio { get; }
        public ITextBackendFactory text { get; }
        public IRenderingBackendFactory rendering { get; }
        public IUiBackendFactory ui { get; }
    }

    private sealed class TestPlatformProvider : PlatformBackendProvider
    {
        public TestPlatformProvider(PlatformBackendId id) : base(id) { }

        public int creationCount { get; private set; }

        public override IPlatformApplication CreateApplication()
        {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestInputProvider : InputBackendProvider
    {
        public TestInputProvider(InputBackendId id) : base(id) { }

        public int creationCount { get; private set; }

        public override IInputEventSource CreateEventSource(
            IPlatformWindow window,
            bool acceptAllWindows
        ) {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestStorageProvider : StorageBackendProvider
    {
        public TestStorageProvider(StorageBackendId id) : base(id) { }

        public int creationCount { get; private set; }

        public override IApplicationStorage CreateStorage(string rootDirectory)
        {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestAudioProvider : AudioBackendProvider
    {
        public TestAudioProvider(AudioBackendId id) : base(id) { }

        public int creationCount { get; private set; }

        public override IAudioDevice CreateDevice(AudioBackendOptions options)
        {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestTextProvider : TextBackendProvider
    {
        public TestTextProvider(TextBackendId id) : base(id) { }

        public int creationCount { get; private set; }

        public override ITextBackend CreateBackend()
        {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestRenderingProvider : RenderingBackendProvider
    {
        public TestRenderingProvider(RenderingBackendId id) : base(id) { }

        public int creationCount { get; private set; }

        public override IRenderDevice CreateDevice(RenderingBackendOptions options)
        {
            creationCount++;
            return null!;
        }

        public override IRenderLayerCompositionProgramProvider CreateCompositionProgramProvider()
            => throw new InvalidOperationException("Registration validation must not create composition programs.");
    }

    private sealed class TestUiProvider : UiBackendProvider
    {
        public TestUiProvider(UiBackendId id) : base(id) { }

        public int creationCount { get; private set; }

        public override IUiBackend CreateBackend()
        {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestPresentationProvider : PresentationBackendProvider
    {
        public TestPresentationProvider(PresentationBackendId id) : base(id) { }

        public int creationCount { get; private set; }

        public override IPresentationContext CreateContext(PresentationBackendOptions options)
        {
            creationCount++;
            return null!;
        }
    }
}

