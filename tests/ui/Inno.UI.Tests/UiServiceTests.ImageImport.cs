using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Adapter.UI.RmlUi;
using Inno.Adapter.UI.RmlUi.Authoring;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Content.Testing;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Rendering.Assets;
using Inno.UI.Assets;
using Inno.UI.Runtime;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiServiceTests
{
    [Fact]
    public void ExportedImageDocumentLoadsThroughRuntimeContentWithoutAuthoringServices()
    {
        File.WriteAllBytes(Path.Combine(m_assets, "Icon.tga"), ImageBytes());
        File.WriteAllText(Path.Combine(m_assets, "Image.rml"),
            "<rml><head/><body><img src='Icon.tga'/></body></rml>");
        string contentRoot = Path.Combine(m_root, "DeployedContent");
        Guid textureId;
        using (AssetLoader loader = CreateLoader())
        {
            var authored = Assert.IsType<UiDocumentAsset>(loader.Load(AssetPath.Project("Image.rml"), typeof(UiDocumentAsset)));
            textureId = Assert.Single(authored.images).image.artifact.assetId;
            loader.ExportRuntimeArtifacts(contentRoot);
        }
        File.Delete(Path.Combine(m_assets, "Image.rml"));
        File.Delete(Path.Combine(m_assets, "Icon.tga"));
        using SerializationGeneration serialization = m_serialization.CaptureGeneration();
        using ContentTestStore content = ContentTestStore.FromDirectory(contentRoot);
        using var database = new AssetDatabase(content, serialization, m_types.current, new IdentityAllocator());
        UiDocumentAsset document = database.Load<UiDocumentAsset>(AssetPath.Project("Image.rml"));
        Assert.Equal(textureId, Assert.Single(document.images).image.artifact.assetId);
        using var runtime = new UiRuntime(new RmlUiBackend(), database);
        UiContextHandle context = runtime.CreateContext(new("deployed-image", 200, 100));
        UiDocumentHandle handle = runtime.LoadDocument(context, document);
        runtime.ShowDocument(context, handle);
        runtime.RefreshLayout(context);
        UiTextureUpdate update = Assert.Single(runtime.Render(context).textureUpdates);
        Assert.Equal(textureId, update.image!.artifact.assetId);
        Assert.True(update.pixels.IsEmpty);
        runtime.DestroyContext(context);
    }

    [Fact]
    public void ReadOnlyImageDocumentUsesMountedDependenciesWithoutChangingSourceFiles()
    {
        string mount = Path.Combine(m_root, "ReadOnlyImages");
        Directory.CreateDirectory(mount);
        File.WriteAllBytes(Path.Combine(mount, "Icon.tga"), ImageBytes());
        File.WriteAllText(Path.Combine(mount, "Image.rml"),
            "<rml><head/><body><img src='Icon.tga'/></body></rml>");
        Guid identity;
        using (var author = new AssetLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs,
            mount, Path.Combine(m_root, "ImageAuthorLibrary")))
        {
            var image = Assert.IsType<TextureAsset>(author.Load(AssetPath.Project("Icon.tga"), typeof(TextureAsset)));
            identity = image.identity.persistentId;
            author.Load(AssetPath.Project("Image.rml"), typeof(UiDocumentAsset));
        }
        var before = Directory.GetFiles(mount).ToDictionary(path => Path.GetFileName(path)!, File.ReadAllBytes);
        var source = new AssetSourceId("tests.images");
        using var loader = new AssetLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs,
            [new AssetSourceMount(AssetSourceId.project, m_assets, false), new AssetSourceMount(source, mount, true)], m_library);
        var document = Assert.IsType<UiDocumentAsset>(loader.Load(new AssetPath(source, "Image.rml"), typeof(UiDocumentAsset)));
        Assert.Equal(identity, Assert.Single(document.images).image.artifact.assetId);
        using var runtime = new UiRuntime(new RmlUiBackend(), loader);
        UiContextHandle context = runtime.CreateContext(new("mounted-image", 200, 100));
        UiDocumentHandle handle = runtime.LoadDocument(context, document);
        runtime.ShowDocument(context, handle);
        runtime.RefreshLayout(context);
        Assert.Equal(identity, Assert.Single(runtime.Render(context).textureUpdates).image!.artifact.assetId);
        runtime.DestroyContext(context);
        Assert.Equal(before.Count, Directory.GetFiles(mount).Length);
        foreach ((string name, byte[] bytes) in before)
            Assert.Equal(bytes, File.ReadAllBytes(Path.Combine(mount, name)));
    }

    [Fact]
    public void ImageReferencesRemainFrozenAcrossReplacementMissingAndRecovery()
    {
        string path = Path.Combine(m_assets, "Icon.tga");
        File.WriteAllBytes(path, ImageBytes());
        using AssetLoader loader = CreateLoader();
        var texture = Assert.IsType<TextureAsset>(loader.Load(AssetPath.Project("Icon.tga"), typeof(TextureAsset)));
        UiImageSource firstSource = UiImageSource.FromTexture(texture);
        byte[] metadata = File.ReadAllBytes(path + ".imeta");
        var artifacts = new RetainingArtifactLookup(loader);
        using var runtime = new UiRuntime(new RmlUiBackend(), artifacts);
        UiContextHandle firstContext = runtime.CreateContext(new("first", 200, 100));
        using IUiImageRegistration first = runtime.RegisterImage(firstContext, firstSource);
        UiDocumentHandle firstDocument = runtime.LoadDocument(firstContext, Rml(
            $"<rml><head/><body><img src='{first.source}'/></body></rml>"));
        runtime.ShowDocument(firstContext, firstDocument);
        runtime.RefreshLayout(firstContext);
        Assert.Same(firstSource, Assert.Single(runtime.Render(firstContext).textureUpdates).image);

        byte[] replacement = ImageBytes();
        replacement[18] = 42;
        File.WriteAllBytes(path, replacement);
        loader.Rescan();
        var updated = Assert.IsType<TextureAsset>(loader.Load(firstSource.artifact.assetId, typeof(TextureAsset)));
        UiImageSource secondSource = UiImageSource.FromTexture(updated);
        Assert.NotEqual(firstSource.artifact.contentRevision, secondSource.artifact.contentRevision);
        UiContextHandle secondContext = runtime.CreateContext(new("second", 200, 100));
        using IUiImageRegistration second = runtime.RegisterImage(secondContext, secondSource);
        Assert.Equal(2, artifacts.retainedKeys.Count);
        File.Delete(path);
        loader.Rescan();
        Assert.True(updated.isMissing);
        Assert.Throws<InvalidOperationException>(() => UiImageSource.FromTexture(updated));
        Assert.NotEmpty(runtime.Render(firstContext).commands);
        File.WriteAllBytes(path, replacement);
        File.WriteAllBytes(path + ".imeta", metadata);
        loader.Rescan();
        var restored = Assert.IsType<TextureAsset>(loader.Load(firstSource.artifact.assetId, typeof(TextureAsset)));
        Assert.False(restored.isMissing);
        Assert.Equal(firstSource.artifact.assetId, UiImageSource.FromTexture(restored).artifact.assetId);
        runtime.DestroyContext(firstContext);
        Assert.Single(artifacts.retainedKeys);
        runtime.DestroyContext(secondContext);
        Assert.Empty(artifacts.retainedKeys);
    }

    [Fact]
    public void DocumentImporterCapturesImageDependenciesAndRegistersThemThroughTheRuntime()
    {
        File.WriteAllBytes(Path.Combine(m_assets, "Icon.tga"), ImageBytes());
        File.WriteAllText(Path.Combine(m_assets, "Image.rml"), """
            <rml><head><style>body { margin: 0; }</style></head>
            <body><img id="image" src="Icon.tga" /></body></rml>
            """);
        using AssetLoader loader = CreateLoader();
        var document = Assert.IsType<UiDocumentAsset>(loader.Load(AssetPath.Project("Image.rml"), typeof(UiDocumentAsset)));
        var texture = Assert.IsType<TextureAsset>(loader.Load(AssetPath.Project("Icon.tga"), typeof(TextureAsset)));
        UiDocumentImage declaration = Assert.Single(document.images);
        Assert.Equal(texture.GetTextureArtifactReference(), declaration.image.artifact);
        Assert.Equal(2, declaration.image.width);
        var artifacts = new RetainingArtifactLookup(loader);
        using var runtime = new UiRuntime(new RmlUiBackend(), artifacts);
        UiContextHandle context = runtime.CreateContext(new("imported-image", 200, 100));
        UiDocumentHandle handle = runtime.LoadDocument(context, document);
        Assert.Single(artifacts.retainedKeys);
        runtime.ShowDocument(context, handle);
        runtime.RefreshLayout(context);
        UiTextureUpdate update = Assert.Single(runtime.Render(context).textureUpdates);
        Assert.Equal(texture.GetTextureArtifactReference(), update.image!.artifact);
        Assert.True(update.pixels.IsEmpty);
        runtime.CloseDocument(context, handle);
        Assert.Empty(artifacts.retainedKeys);
        Assert.Contains(update.texture, runtime.Render(context).releasedTextures);
        runtime.DestroyContext(context);
    }

    [Fact]
    public void FrontendResolvesRelativeImageAndSpriteSheetSourcesWithoutReadingPixels()
    {
        var frontend = new RmlUiDocumentFrontend();
        const string sheet = "@spritesheet controls { src: ../Art/Icon.tga; resolution: 1x; panel: 0px 0px 2px 1px; }";
        UiDocumentAnalysis result = frontend.Analyze(new(
            "Screens/Hud.rml",
            """
            <rml><head><link href="../Styles/theme.rcss" /></head>
            <body><!-- <img src="NotAnAsset.tga" /> --><img src="../Art/Icon.tga" /></body></rml>
            """,
            path => path.localPath == "Styles/theme.rcss" ? sheet : throw new InvalidOperationException(path.ToString())));
        Assert.True(result.succeeded, string.Join("\n", result.diagnostics.Select(value => value.message)));
        UiDocumentImageDeclaration image = Assert.Single(result.images);
        Assert.Equal("Art/Icon.tga", image.assetPath);
        Assert.Contains("src: " + image.source + ";", result.text);
        Assert.Contains("src=\"" + image.source + "\"", result.text);
        Assert.Contains("NotAnAsset.tga", result.text);
        UiDocumentAnalysis invalid = frontend.Analyze(new("Hud.rml",
            "<rml><head/><body><img src='../escape.tga' /></body></rml>"));
        Assert.False(invalid.succeeded);
        Assert.Contains(invalid.diagnostics, value => value.code == "RML_IMAGE_ASSET_INVALID");
    }

    [Fact]
    public void RuntimeResourceBindingsAreFrozenAndCannotEnterAnImportedPayload()
    {
        var bindings = new Dictionary<string, string> { ["declared"] = "inno-image://1/2" };
        var source = new UiDocumentSource(RmlUiIdentifiers.documentLanguage,
            "<rml><head/><body/></rml>", resourceBindings: bindings);
        bindings["declared"] = "changed";
        Assert.Equal("inno-image://1/2", source.resourceBindings["declared"]);
        Assert.Throws<ArgumentException>(() => UiDocumentAsset.CreateRuntimePayload(
            RmlUiIdentifiers.backend.value, source));
    }

    private static byte[] ImageBytes()
    {
        byte[] bytes = new byte[26];
        bytes[2] = 2;
        bytes[12] = 2;
        bytes[14] = 1;
        bytes[16] = 32;
        bytes[17] = 0x28;
        bytes[18] = 255;
        bytes[21] = 128;
        bytes[23] = 255;
        bytes[25] = 255;
        return bytes;
    }
}
