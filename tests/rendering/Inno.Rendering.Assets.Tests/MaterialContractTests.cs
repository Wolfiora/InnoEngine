using Xunit;

namespace Inno.Rendering.Assets.Tests;

public sealed class MaterialContractTests
{
    [Fact]
    public void FrameOverridesContainOnlyLogicalValuesAndCanBeCleared()
    {
        var block = new MaterialPropertyBlock();
        var id = new ShaderPropertyId("amount");
        block.Set(id, MaterialValue.FromFloat(0.5f));
        Assert.True(block.TryGet(id, out MaterialValue value));
        Assert.Equal(MaterialValueKind.Float, value.kind);
        Assert.Equal(0.5f, value.vector.x);
        block.Clear();
        Assert.False(block.TryGet(id, out _));
        Assert.Equal(0, block.count);
    }
}
