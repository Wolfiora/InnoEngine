using Xunit;
using Inno.Assets;

namespace Inno.Assets.Tests;

public sealed class AssetTypesTests
{
    [Fact]
    public void TextAssetCtorAssignsValues()
    {
        var asset = new TextAsset("hello", "plain");
        Assert.Equal("hello", asset.content);
        Assert.Equal("plain", asset.languageHint);
    }

    [Fact]
    public void BinaryAssetCtorAssignsValues()
    {
        var asset = new BinaryAsset(10);
        Assert.Equal(10, asset.byteLength);
    }
}
