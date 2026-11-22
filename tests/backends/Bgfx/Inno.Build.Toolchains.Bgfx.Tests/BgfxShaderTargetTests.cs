using System;
using Xunit;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;
using Inno.Testing.Bgfx;

using static Inno.Testing.Bgfx.ShaderProfileFixture;

namespace Inno.Build.Toolchains.Bgfx.Tests;

public sealed class BgfxShaderTargetTests
{
    [Fact]
    public void MetalProfileIsOwnedByMacBgfxToolchain()
    {
        GraphicsCapabilities capabilities = CreateCapabilities(GraphicsApi.metal, GraphicsCapability.Compute);
        var toolchain = new BgfxShadercToolchain(ShaderProfileFixture.mac);

        ShaderCompileTarget target = toolchain.CreateTarget(capabilities);

        Assert.Contains("bgfx-shaderc:fixture-mac:", target.profileKey, StringComparison.Ordinal);
        Assert.Throws<NotSupportedException>(() =>
            new BgfxShadercToolchain(ShaderProfileFixture.windows).CreateTarget(capabilities));
    }

    [Fact]
    public void Direct3DAndMetalTargetsSelectProfilesWithoutForkingShaderSource()
    {
        ShaderCompileTarget metal = new BgfxShadercToolchain(ShaderProfileFixture.mac)
            .CreateTarget(CreateCapabilities(GraphicsApi.metal, GraphicsCapability.Compute));
        ShaderCompileTarget direct3D = new BgfxShadercToolchain(ShaderProfileFixture.windows)
            .CreateTarget(CreateCapabilities(GraphicsApi.direct3D11, GraphicsCapability.Compute));

        Assert.Equal(GraphicsApi.metal, metal.capabilities.backend);
        Assert.Equal(GraphicsApi.direct3D11, direct3D.capabilities.backend);
        Assert.NotEqual(metal.profileKey, direct3D.profileKey);
    }

    [Fact]
    public void BrowserProfileRequiresWebGlWithoutCompute()
    {
        var toolchain = new BgfxShadercToolchain(ShaderProfileFixture.browser);

        ShaderCompileTarget target = toolchain.CreateTarget(
            CreateCapabilities(GraphicsApi.openGLES, GraphicsCapability.None)
        );

        Assert.Contains("bgfx-shaderc:fixture-browser:", target.profileKey, StringComparison.Ordinal);
        Assert.Throws<NotSupportedException>(() => toolchain.CreateTarget(
            CreateCapabilities(GraphicsApi.openGLES, GraphicsCapability.Compute)
        ));
        Assert.Throws<NotSupportedException>(() => toolchain.CreateTarget(
            CreateCapabilities(GraphicsApi.openGL, GraphicsCapability.None)
        ));
    }
}
