using System;
using System.IO;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Text;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiServiceTests
{
    [Fact]
    public void FullyLoadedFontTracksItsSourceThroughRenameCopyAndRecovery()
    {
        string source = Path.Combine(m_assets, "Interface.ttf");
        File.Copy(FontPath(), source);
        using AssetLoader loader = CreateLoader();
        FontAsset font = Assert.IsType<FontAsset>(loader.Load(AssetPath.Project("Interface.ttf"), typeof(FontAsset)));
        Guid identity = font.identity.persistentId;
        Assert.Equal(AssetPath.Project("Interface.ttf"), font.assetPath);
        Assert.True(font.metadata!.Value.faceCount > 0);
        Assert.False(font.isMissing);
        Assert.DoesNotContain(m_serialization.GetProperties(font), property => property.name == nameof(FontAsset.assetPath));

        string renamed = Path.Combine(m_assets, "Renamed.ttf");
        File.Move(source, renamed);
        File.Move(source + ".imeta", renamed + ".imeta");
        loader.Rescan();
        FontAsset moved = Assert.IsType<FontAsset>(loader.Load(AssetPath.Project("Renamed.ttf"), typeof(FontAsset)));
        Assert.Equal(identity, moved.identity.persistentId);
        Assert.Equal(AssetPath.Project("Renamed.ttf"), moved.assetPath);
        Assert.True(loader.TryGetInfo(identity, out AssetInfo? info));
        Assert.Equal(moved.assetPath, info!.assetPath);

        File.Copy(renamed, Path.Combine(m_assets, "Copy.ttf"));
        loader.Rescan();
        FontAsset copy = Assert.IsType<FontAsset>(loader.Load(AssetPath.Project("Copy.ttf"), typeof(FontAsset)));
        Assert.NotEqual(identity, copy.identity.persistentId);
        Assert.Equal(AssetPath.Project("Copy.ttf"), copy.assetPath);

        byte[] original = File.ReadAllBytes(renamed);
        byte[] originalMetadata = File.ReadAllBytes(renamed + ".imeta");
        File.Delete(renamed);
        loader.Rescan();
        Assert.True(moved.isMissing);
        File.WriteAllBytes(renamed, original);
        File.WriteAllBytes(renamed + ".imeta", originalMetadata);
        loader.Rescan();
        FontAsset restored = Assert.IsType<FontAsset>(loader.Load(identity, typeof(FontAsset)));
        Assert.Equal(identity, restored.identity.persistentId);
        Assert.Equal(AssetPath.Project("Renamed.ttf"), restored.assetPath);
        Assert.False(restored.isMissing);
        Assert.True(restored.metadata!.Value.faceCount > 0);
    }

    [Fact]
    public void ReadOnlyFontMetadataRetainsItsSourceIdentityWithoutWritingTheMount()
    {
        string package = Path.Combine(m_root, "Package");
        Directory.CreateDirectory(package);
        string fontPath = Path.Combine(package, "Interface.ttf");
        File.Copy(FontPath(), fontPath);
        Guid identity;
        using (var author = new AssetLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs,
            package, Path.Combine(m_root, "AuthorLibrary")))
        {
            var font = Assert.IsType<FontAsset>(author.Load(AssetPath.Project("Interface.ttf"), typeof(FontAsset)));
            identity = font.identity.persistentId;
        }
        byte[] metadata = File.ReadAllBytes(fontPath + ".imeta");
        var source = new AssetSourceId("tests.font-package");
        using var loader = new AssetLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs,
            [new AssetSourceMount(AssetSourceId.project, m_assets, false), new AssetSourceMount(source, package, true)], m_library);
        var path = new AssetPath(source, "Interface.ttf");
        var mounted = Assert.IsType<FontAsset>(loader.Load(path, typeof(FontAsset)));
        Assert.Equal(identity, mounted.identity.persistentId);
        Assert.Equal(path, mounted.assetPath);
        Assert.True(mounted.metadata!.Value.faceCount > 0);
        Assert.True(loader.TryGetInfo(identity, out AssetInfo? info));
        Assert.Equal(path, info!.assetPath);
        Assert.Equal(metadata, File.ReadAllBytes(fontPath + ".imeta"));
    }
}
