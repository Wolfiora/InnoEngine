using Inno.Integration.Windows.Bgfx;
using System;
using Inno.Adapter.Rendering;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering.Assets;
using Xunit;
using Inno.Rendering.Assets.Authoring;

namespace Inno.Rendering.Shaders.Tests;

public sealed class RenderingBackendRegistrationTests
{
    [Fact]
    public void RuntimeAndAuthoringRegistrationsAreOpenAndPairedBeforeDeviceCreation()
    {
        var id = new RenderingBackendId("tests.independent-renderer");
        var provider = new RuntimeProvider(id);
        var runtime = new RenderingBackendCatalog([provider]);
        var authoring = new RenderingAuthoringBackendCatalog(runtime, [new AuthoringProvider(id)]);
        Assert.Equal(id, Assert.Single(runtime.supportedBackends));
        Assert.Equal(id, Assert.Single(authoring.supportedBackends));
        Assert.False(provider.wasCreated);
        Assert.Throws<NotSupportedException>(() => runtime.CreateDevice(RenderingBackendId.bgfx, new RenderingBackendOptions()));
        Assert.False(provider.wasCreated);
    }

    [Fact]
    public void DuplicateUnassignedAndUnpairedRegistrationsFail()
    {
        var id = new RenderingBackendId("tests.renderer");
        Assert.Throws<ArgumentException>(() => new RenderingBackendCatalog([new RuntimeProvider(id), new RuntimeProvider(id)]));
        Assert.Throws<ArgumentException>(() => new RenderingBackendCatalog([new RuntimeProvider(default)]));
        var runtime = new RenderingBackendCatalog([new RuntimeProvider(id)]);
        Assert.Throws<ArgumentException>(() => new RenderingAuthoringBackendCatalog(runtime, []));
        Assert.Throws<ArgumentException>(() => new RenderingAuthoringBackendCatalog(runtime, [new AuthoringProvider(RenderingBackendId.bgfx)]));
        Assert.Throws<ArgumentException>(() => new RenderingBackendId("invalid id"));
        Assert.False(default(RenderingBackendId).isValid);
    }

    private sealed class RuntimeProvider(RenderingBackendId id) : RenderingBackendProvider(id)
    {
        public bool wasCreated { get; private set; }
        public override IRenderDevice CreateDevice(RenderingBackendOptions options)
        { wasCreated = true; throw new InvalidOperationException("These registration tests must not create a GPU device."); }
        public override IRenderLayerCompositionProgramProvider CreateCompositionProgramProvider()
            => throw new InvalidOperationException("These registration tests do not create composition programs.");
    }

    private sealed class AuthoringProvider(RenderingBackendId id) : RenderingAuthoringBackendProvider(id)
    {
        public override IShaderCompilerToolchain CreateShaderCompilerToolchain() => new BgfxShadercToolchain(Inno.Integration.Windows.Bgfx.WindowsBgfxShaderProfiles.target);
        public override ITextureTargetCompiler CreateTextureTargetCompiler() => new BgfxTextureTargetCompiler();
    }
}
