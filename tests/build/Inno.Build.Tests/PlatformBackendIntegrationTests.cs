using System;
using Xunit;
using Inno.Build.Bindings;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Integration.BrowserWasm32.Bgfx;
using Inno.Integration.MacOSArm64.Bgfx;
using Inno.Integration.WindowsX64.Bgfx;

namespace Inno.Build.Tests;

public sealed class PlatformBackendIntegrationTests
{
    [Fact]
    public void IntegrationsPreserveExactShaderAndNativeTargetIdentities()
    {
        Assert.Equal("windows-x64", WindowsX64BgfxIntegration.nativeProfile.targetId);
        Assert.Equal("macos-arm64", MacOSArm64BgfxIntegration.nativeProfile.targetId);
        var context = new BuildCompositionContext("explicit-sdk", System.IO.Path.GetTempPath(),
            new BuildHostDescriptor("Windows", "x64"), new BuildTargetId("windows-x64"));
        var standard = StandardBuildDistribution.Create(context);
        Assert.Same(WindowsX64BgfxIntegration.shaderProfile, standard.ResolveShaderTarget(new BuildTargetId("windows-x64")));
        Assert.Same(MacOSArm64BgfxIntegration.shaderProfile, standard.ResolveShaderTarget(new BuildTargetId("macos-arm64")));
        Assert.Same(BrowserWasm32BgfxIntegration.shaderProfile, standard.ResolveShaderTarget(new BuildTargetId("browser-wasm")));
    }
}
