using System;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;
using Xunit;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

public sealed class BgfxShaderProfileTests
{
    [Fact]
    public void NewTargetAndRendererUseThePublicConfigurationBoundary()
    {
        var api = new GraphicsApi("FixtureGpu");
        GraphicsCapabilities capabilities = BgfxTargetCapabilities.Create(api);
        var profile = new BgfxShaderCompilerProfile(capabilities, "fixture-sdk", "vertex", "fragment", "compute");
        var configuration = new BgfxShaderTargetProfile("fixture-console", [profile]);

        var target = new BgfxShadercToolchain(configuration).CreateTarget(capabilities);

        Assert.Same(profile, configuration.Resolve(api));
        Assert.Contains("fixture-console", target.profileKey, StringComparison.Ordinal);
        Assert.Throws<NotSupportedException>(() => configuration.Resolve(GraphicsApi.Metal));
    }

    [Fact]
    public void DialectDefinesAndCapabilitiesAllParticipateInIdentity()
    {
        GraphicsCapabilities capabilities = BgfxTargetCapabilities.Create(GraphicsApi.Vulkan);
        string[] defines = ["TEST=1"];
        var original = new BgfxShaderCompilerProfile(capabilities, "fixture-sdk", "spirv", "spirv", "spirv", defines);
        defines[0] = "TEST=2";

        Assert.Equal("TEST=1", original.defines[0]);
        Assert.NotEqual(original.key, new BgfxShaderCompilerProfile(capabilities,
            "fixture-sdk", "spirv", "spirv", "spirv", defines).key);
        Assert.NotEqual(original.key, new BgfxShaderCompilerProfile(capabilities,
            "other-sdk", "spirv", "spirv", "spirv", ["TEST=1"]).key);
        Assert.NotEqual(original.key, new BgfxShaderCompilerProfile(capabilities,
            "fixture-sdk", "other-dialect", "spirv", "spirv", ["TEST=1"]).key);
        var reduced = new GraphicsCapabilities(GraphicsApi.Vulkan, capabilities.features,
            new GraphicsLimits(16, 4, 1024, 8), [], [], [], [], false, false);
        Assert.NotEqual(original.key, new BgfxShaderCompilerProfile(reduced,
            "fixture-sdk", "spirv", "spirv", "spirv", ["TEST=1"]).key);
    }

    [Fact]
    public void InvalidAndDuplicateProfilesFailBeforeCreatingACompiler()
    {
        var profile = new BgfxShaderCompilerProfile(BgfxTargetCapabilities.Create(GraphicsApi.Metal),
            "osx", "metal", "metal", "metal");

        Assert.Throws<ArgumentException>(() => new BgfxShaderTargetProfile("fixture", []));
        Assert.Throws<ArgumentException>(() => new BgfxShaderTargetProfile("fixture", [profile, profile]));
        Assert.Throws<ArgumentException>(() => new BgfxShaderCompilerProfile(profile.capabilities,
            "osx", "metal", "metal", ""));
        Assert.Throws<ArgumentNullException>(() => new BgfxShadercToolchain(null!));
    }
}
