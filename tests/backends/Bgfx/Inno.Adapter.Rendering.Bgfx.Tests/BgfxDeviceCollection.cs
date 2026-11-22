using Xunit;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

[CollectionDefinition(C_NAME, DisableParallelization = true)]
public sealed class BgfxDeviceCollection : ICollectionFixture<BgfxDeviceFixture>
{
    public const string C_NAME = "BGFX device";
}
