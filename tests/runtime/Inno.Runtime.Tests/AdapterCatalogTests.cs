using System;
using System.Collections.Generic;
using Xunit;
using Inno.Adapter;
using Inno.Adapter.Audio;
using Inno.Adapter.Input;
using Inno.Adapter.Platform;
using Inno.Adapter.Presentation;
using Inno.Adapter.Rendering;
using Inno.Adapter.Storage;
using Inno.Adapter.Text;
using Inno.Adapter.UI;
using Inno.Audio;
using Inno.Platform;
using Inno.Rendering;
using Inno.Storage;
using Inno.Text;
using Inno.UI;

namespace Inno.Runtime.Tests;

public sealed class AdapterCatalogTests
{
    [Fact]
    public void PresentationRegistrationUsesAnImmutableOpenCatalog()
    {
        var first = new TestPresentationProvider(new PresentationBackendID("test.presentation.first"));
        var second = new TestPresentationProvider(new PresentationBackendID("test.presentation.second"));
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
        var options = new PresentationBackendOptions
        {
            window = null!,
            platformApplication = null!,
            renderDevice = null!
        };
        Assert.Throws<NotSupportedException>(() => catalog.CreateContext(second.id, options));
        Assert.Throws<ArgumentNullException>(() => catalog.CreateContext(first.id, null!));
        Assert.Throws<InvalidOperationException>(() => catalog.CreateContext(first.id, options));
        Assert.Equal(1, first.creationCount);
        Assert.Equal(0, second.creationCount);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<PresentationBackendID>)catalog.supportedBackends).Add(second.id));
    }

    [Fact]
    public void CustomIdentitiesAreAcceptedWithoutCreatingServices()
    {
        var catalog = new TestCatalog();
        catalog.selection.Validate(catalog);
        Assert.Equal(0, catalog.creationCount);
        Assert.Throws<NotSupportedException>(() =>
            new AdapterSelection { platform = new PlatformBackendID("missing.platform") }.Validate(catalog));
        Assert.Equal(0, catalog.creationCount);
    }

    [Fact]
    public void AllDomainsRejectDuplicateAndNullProviders()
    {
        var platform = new TestPlatformProvider(new PlatformBackendID("test.platform"));
        Assert.Throws<ArgumentException>(() => new PlatformBackendCatalog([platform, platform]));
        Assert.Throws<ArgumentException>(() => new PlatformBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestPlatformProvider(default));
        var input = new TestInputProvider(new InputBackendID("test.input"));
        Assert.Throws<ArgumentException>(() => new InputBackendCatalog([input, input]));
        Assert.Throws<ArgumentException>(() => new InputBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestInputProvider(default));
        var storage = new TestStorageProvider(new StorageBackendID("test.storage"));
        Assert.Throws<ArgumentException>(() => new StorageBackendCatalog([storage, storage]));
        Assert.Throws<ArgumentException>(() => new StorageBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestStorageProvider(default));
        var audio = new TestAudioProvider(new AudioBackendID("test.audio"));
        Assert.Throws<ArgumentException>(() => new AudioBackendCatalog([audio, audio]));
        Assert.Throws<ArgumentException>(() => new AudioBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestAudioProvider(default));
        var text = new TestTextProvider(new TextBackendID("test.text"));
        Assert.Throws<ArgumentException>(() => new TextBackendCatalog([text, text]));
        Assert.Throws<ArgumentException>(() => new TextBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestTextProvider(default));
        var rendering = new TestRenderingProvider(new RenderingBackendID("test.rendering"));
        Assert.Throws<ArgumentException>(() => new RenderingBackendCatalog([rendering, rendering]));
        Assert.Throws<ArgumentException>(() => new RenderingBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestRenderingProvider(default));
        var ui = new TestUIProvider(new UIBackendID("test.ui"));
        Assert.Throws<ArgumentException>(() => new UIBackendCatalog([ui, ui]));
        Assert.Throws<ArgumentException>(() => new UIBackendCatalog([null!]));
        Assert.Throws<ArgumentException>(() => new TestUIProvider(default));
    }

    [Fact]
    public void CapturedRegistrationsCannotBeChangedByTheirSourceCollection()
    {
        var first = new TestStorageProvider(new StorageBackendID("test.storage.first"));
        var second = new TestStorageProvider(new StorageBackendID("test.storage.second"));
        var providers = new List<StorageBackendProvider> { first };
        var catalog = new StorageBackendCatalog(providers);
        providers.Clear();
        providers.Add(second);
        Assert.Equal(first.id, Assert.Single(catalog.supportedBackends));
        Assert.Throws<NotSupportedException>(() => catalog.CreateStorage(second.id, new StorageScope("application")));
        Assert.Equal(0, first.creationCount);
        Assert.Throws<InvalidOperationException>(() => catalog.CreateStorage(first.id, new StorageScope("application")));
        Assert.Equal(1, first.creationCount);
        Assert.Equal(0, second.creationCount);
        Assert.Throws<NotSupportedException>(() => ((IList<StorageBackendID>)catalog.supportedBackends).Add(second.id));
    }

    [Fact]
    public void InvalidIdentifiersAndUnassignedSelectionsFailAtTheBoundary()
    {
        Assert.Throws<ArgumentException>(() => new PlatformBackendID("invalid id"));
        Assert.False(default(PlatformBackendID).isValid);
        Assert.Throws<ArgumentException>(() => new InputBackendID("invalid id"));
        Assert.False(default(InputBackendID).isValid);
        Assert.Throws<ArgumentException>(() => new StorageBackendID("invalid id"));
        Assert.False(default(StorageBackendID).isValid);
        Assert.Throws<ArgumentException>(() => new AudioBackendID("invalid id"));
        Assert.False(default(AudioBackendID).isValid);
        Assert.Throws<ArgumentException>(() => new TextBackendID("invalid id"));
        Assert.False(default(TextBackendID).isValid);
        Assert.Throws<ArgumentException>(() => new RenderingBackendID("invalid id"));
        Assert.False(default(RenderingBackendID).isValid);
        Assert.Throws<ArgumentException>(() => new UIBackendID("invalid id"));
        Assert.False(default(UIBackendID).isValid);
        Assert.Throws<NotSupportedException>(() => default(AdapterSelection).Validate(new TestCatalog()));
    }

    private sealed class TestCatalog : IAdapterCatalog
    {
        private readonly TestPlatformProvider m_platform = new(new PlatformBackendID("test.platform"));
        private readonly TestInputProvider m_input = new(new InputBackendID("test.input"));
        private readonly TestStorageProvider m_storage = new(new StorageBackendID("test.storage"));
        private readonly TestAudioProvider m_audio = new(new AudioBackendID("test.audio"));
        private readonly TestTextProvider m_text = new(new TextBackendID("test.text"));
        private readonly TestRenderingProvider m_rendering = new(new RenderingBackendID("test.rendering"));
        private readonly TestUIProvider m_ui = new(new UIBackendID("test.ui"));

        public TestCatalog()
        {
            this.platform = new PlatformBackendCatalog([m_platform]);
            this.input = new InputBackendCatalog([m_input]);
            this.storage = new StorageBackendCatalog([m_storage]);
            this.audio = new AudioBackendCatalog([m_audio]);
            this.text = new TextBackendCatalog([m_text]);
            this.rendering = new RenderingBackendCatalog([m_rendering]);
            this.ui = new UIBackendCatalog([m_ui]);
        }

        public AdapterSelection selection
            => new()
        {
            platform = m_platform.id,
            input = m_input.id,
            storage = m_storage.id,
            audio = m_audio.id,
            text = m_text.id,
            rendering = m_rendering.id,
            ui = m_ui.id
        };

        public int creationCount
            => m_platform.creationCount
               + m_input.creationCount
               + m_storage.creationCount
               + m_audio.creationCount
               + m_text.creationCount
               + m_rendering.creationCount
               + m_ui.creationCount;

        public IPlatformBackendFactory platform { get; }
        public IInputBackendFactory input { get; }
        public IStorageBackendFactory storage { get; }
        public IAudioBackendFactory audio { get; }
        public ITextBackendFactory text { get; }
        public IRenderingBackendFactory rendering { get; }
        public IUIBackendFactory ui { get; }
    }

    private sealed class TestPlatformProvider : PlatformBackendProvider
    {
        public TestPlatformProvider(PlatformBackendID id)
            : base(id) {}

        public int creationCount { get; private set; }

        public override IPlatformApplication CreateApplication()
        {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestInputProvider : InputBackendProvider
    {
        public TestInputProvider(InputBackendID id)
            : base(id) {}

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
        public TestStorageProvider(StorageBackendID id)
            : base(id) {}

        public int creationCount { get; private set; }

        public override IApplicationStorage CreateStorage(StorageScope scope)
        {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestAudioProvider : AudioBackendProvider
    {
        public TestAudioProvider(AudioBackendID id)
            : base(id) {}

        public int creationCount { get; private set; }

        public override IAudioDevice CreateDevice(AudioBackendOptions options)
        {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestTextProvider : TextBackendProvider
    {
        public TestTextProvider(TextBackendID id)
            : base(id) {}

        public int creationCount { get; private set; }

        public override ITextBackend CreateBackend()
        {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestRenderingProvider : RenderingBackendProvider
    {
        public TestRenderingProvider(RenderingBackendID id)
            : base(id) {}

        public int creationCount { get; private set; }

        public override IRenderDevice CreateDevice(RenderingBackendOptions options)
        {
            creationCount++;
            return null!;
        }

        public override IRenderLayerCompositionProgramProvider CreateCompositionProgramProvider()
            => throw new InvalidOperationException("Registration validation must not create composition programs.");
    }

    private sealed class TestUIProvider : UIBackendProvider
    {
        public TestUIProvider(UIBackendID id)
            : base(id) {}

        public int creationCount { get; private set; }

        public override IUIBackend CreateBackend()
        {
            creationCount++;
            return null!;
        }
    }

    private sealed class TestPresentationProvider : PresentationBackendProvider
    {
        public TestPresentationProvider(PresentationBackendID id)
            : base(id) {}

        public int creationCount { get; private set; }

        public override IPresentationContext CreateContext(PresentationBackendOptions options)
        {
            creationCount++;
            return null!;
        }
    }
}
