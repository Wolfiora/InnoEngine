using System;
using System.Diagnostics;
using Inno.Adapter.UI.RmlUi;
using Inno.Rendering.Assets;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiServiceTests
{
    [Fact]
    public void StableImportedImagesDoNotRepublishResourcesOrAllocateMoreThanPixelImages()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle pixels = backend.CreateContext(new("pixels", 200, 100));
        UiContextHandle imported = backend.CreateContext(new("imported", 200, 100));
        byte[] data = new byte[16 * 16 * 4];
        Array.Fill(data, (byte)255);
        backend.RegisterTexture(pixels, "pixels", new(16, 16, data));
        using IUiImageRegistration registration = backend.RegisterImage(imported,
            new(new(Guid.NewGuid(), 1, new("color", "image", TextureColorSpace.Linear)), 16, 16));
        Show(backend, pixels, "pixels");
        Show(backend, imported, registration.source);
        for (int frame = 0; frame < 100; frame++)
        {
            backend.Render(pixels);
            backend.Render(imported);
        }
        for (int sample = 0; sample < 5; sample++)
        {
            var first = Measure(backend, sample % 2 == 0 ? pixels : imported);
            var second = Measure(backend, sample % 2 == 0 ? imported : pixels);
            var pixelResult = sample % 2 == 0 ? first : second;
            var imageResult = sample % 2 == 0 ? second : first;
            m_output.WriteLine($"Sample {sample}: pixels={pixelResult.milliseconds:F3} ms/{pixelResult.bytes} bytes; "
                + $"imported={imageResult.milliseconds:F3} ms/{imageResult.bytes} bytes; frames=1000; "
                + $"draws={imageResult.draws}; resourceUpdates={imageResult.updates}");
            Assert.True(imageResult.bytes <= pixelResult.bytes);
            Assert.Equal(pixelResult.draws, imageResult.draws);
            Assert.True(imageResult.draws >= 1000);
            Assert.Equal(0, pixelResult.updates);
            Assert.Equal(0, imageResult.updates);
        }
        backend.DestroyContext(pixels);
        backend.DestroyContext(imported);
    }

    private static void Show(
        IUiBackend backend,
        UiContextHandle context,
        string source
    ) {
        UiDocumentHandle document = backend.LoadDocument(context, new(RmlUiIdentifiers.documentLanguage,
            $"<rml><head><style>body {{ margin: 0; }}</style></head><body><img src='{source}'/></body></rml>"));
        backend.ShowDocument(context, document);
        backend.RefreshLayout(context);
    }

    private static (double milliseconds, long bytes, int draws, int updates) Measure(
        IUiBackend backend,
        UiContextHandle context
    ) {
        int draws = 0;
        int updates = 0;
        long allocated = GC.GetAllocatedBytesForCurrentThread();
        long started = Stopwatch.GetTimestamp();
        for (int frame = 0; frame < 1000; frame++)
        {
            UiRenderFrame result = backend.Render(context);
            draws += result.commands.Count;
            updates += result.textureUpdates.Count + result.meshUpdates.Count;
        }
        return (Stopwatch.GetElapsedTime(started).TotalMilliseconds,
            GC.GetAllocatedBytesForCurrentThread() - allocated, draws, updates);
    }
}
