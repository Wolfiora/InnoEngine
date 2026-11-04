using System;
using System.Linq;
using System.Threading;
using Inno.Adapter.UI.RmlUi;
using Inno.Rendering.Assets;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiServiceTests
{
    [Fact]
    public void ImportedImagesPublishReferencesWithoutPixelsAndRetireOnlyTheirContext()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle first = backend.CreateContext(new("image-a", 320, 200));
        UiContextHandle second = backend.CreateContext(new("image-b", 320, 200));
        var source = new UiImageSource(
            new(Guid.NewGuid(), 7, new("color", "image", TextureColorSpace.Srgb)), 37, 23);
        using IUiImageRegistration firstImage = backend.RegisterImage(first, source);
        using IUiImageRegistration secondImage = backend.RegisterImage(second, source);
        Assert.NotEqual(firstImage.source, secondImage.source);
        UiDocumentHandle firstDocument = ShowImage(backend, first, firstImage.source);
        UiDocumentHandle secondDocument = ShowImage(backend, second, secondImage.source);
        UiRenderFrame firstFrame = backend.Render(first);
        UiTextureUpdate firstTexture = Assert.Single(firstFrame.textureUpdates);
        UiTextureUpdate secondTexture = Assert.Single(backend.Render(second).textureUpdates);
        Assert.Same(source, firstTexture.image);
        Assert.Same(source, secondTexture.image);
        Assert.Empty(firstTexture.pixels.ToArray());
        Assert.Equal(37, firstTexture.width);
        Assert.Equal(23, firstTexture.height);
        Assert.Contains(firstFrame.commands, command => command.texture == firstTexture.texture);
        Assert.Empty(backend.Render(first).textureUpdates);
        firstImage.Dispose();
        UiRenderFrame retired = backend.Render(first);
        Assert.Contains(firstTexture.texture, retired.releasedTextures);
        Assert.Empty(retired.textureUpdates);
        UiRenderFrame surviving = backend.Render(second);
        Assert.Contains(surviving.commands, command => command.texture == secondTexture.texture);
        Assert.Empty(surviving.releasedTextures);
        backend.CloseDocument(first, firstDocument);
        backend.CloseDocument(second, secondDocument);
        backend.DestroyContext(first);
        backend.DestroyContext(second);
        secondImage.Dispose();
    }

    [Fact]
    public void ImageReplacementRetirementAndOwnerRejectionRemainDeterministic()
    {
        using var backend = new RmlUiBackend();
        var artifact = new RenderTextureArtifactReference(
            Guid.NewGuid(), 1, new("color", "image", TextureColorSpace.Linear));
        for (int iteration = 0; iteration < 100; iteration++)
        {
            UiContextHandle context = backend.CreateContext(new("cycle", 200, 100));
            IUiImageRegistration oldImage = backend.RegisterImage(context, new(artifact, 8, 8));
            UiDocumentHandle document = ShowImage(backend, context, oldImage.source);
            UiTextureHandle oldTexture = Assert.Single(backend.Render(context).textureUpdates).texture;
            using IUiImageRegistration replacement = backend.RegisterImage(context, new(
                new(artifact.assetId, 2, artifact.slot), 16, 4));
            Exception? failure = null;
            var thread = new Thread(() => failure = Record.Exception(oldImage.Dispose));
            thread.Start();
            Assert.True(thread.Join(TimeSpan.FromSeconds(10)));
            Assert.IsType<InvalidOperationException>(failure);
            Assert.True(backend.SetAttribute(context, document, "image", "src", replacement.source));
            backend.RefreshLayout(context);
            oldImage.Dispose();
            UiRenderFrame frame = backend.Render(context);
            UiTextureUpdate updated = Assert.Single(frame.textureUpdates);
            Assert.Equal(2, updated.image!.artifact.contentRevision);
            Assert.Contains(oldTexture, frame.releasedTextures);
            Assert.Equal(16, updated.width);
            backend.CloseDocument(context, document);
            backend.DestroyContext(context);
            oldImage.Dispose();
        }
    }

    [Fact]
    public void RetiringAnImageAfterLayoutInvalidatesPreparedDrawCommands()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new("prepared", 200, 100));
        using IUiImageRegistration image = backend.RegisterImage(context, new(
            new(Guid.NewGuid(), 1, new("color", "image", TextureColorSpace.Linear)), 8, 8));
        UiDocumentHandle document = ShowImage(backend, context, image.source);
        UiTextureHandle texture = Assert.Single(backend.Render(context).textureUpdates).texture;
        _ = backend.CaptureLayout(context, document, ["image"]);
        image.Dispose();
        UiRenderFrame frame = backend.Render(context);
        Assert.Contains(texture, frame.releasedTextures);
        Assert.DoesNotContain(frame.commands, command => command.texture == texture);
        backend.CloseDocument(context, document);
        backend.DestroyContext(context);
    }

    [Fact]
    public void ImportedImageBuilderPreservesIdentityAndRejectsInvalidDeclarations()
    {
        Assert.Throws<ArgumentException>(() => new UiImageSource(default, 1, 1));
        var artifact = new RenderTextureArtifactReference(
            Guid.NewGuid(), 1, new("color", "image", TextureColorSpace.Srgb));
        Assert.Throws<ArgumentOutOfRangeException>(() => new UiImageSource(artifact, 0, 1));
        var builder = new UiRenderFrameBuilder();
        var source = new UiImageSource(artifact, 1, 1);
        builder.AddImageUpdate(new(1), 2, source);
        UiTextureUpdate texture = Assert.Single(builder.Build().textureUpdates);
        Assert.Same(source, texture.image);
        Assert.True(texture.pixels.IsEmpty);
        Assert.Throws<InvalidOperationException>(() => builder.AddImageUpdate(new(2), 3, source));
    }

    private static UiDocumentHandle ShowImage(
        RmlUiBackend backend,
        UiContextHandle context,
        string source
    ) {
        UiDocumentHandle document = backend.LoadDocument(context, Rml(
            "<rml><head><style>body { margin: 0; }</style></head><body><img id='image' src='"
            + source + "' /></body></rml>"));
        backend.ShowDocument(context, document);
        backend.RefreshLayout(context);
        return document;
    }
}
