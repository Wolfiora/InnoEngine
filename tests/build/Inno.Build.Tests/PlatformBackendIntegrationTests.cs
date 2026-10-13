using Inno.Build.Bindings;
using System;
using Inno.Build.Distribution.Standard;
using Inno.Build.Composition;
using Inno.Build.Toolchains;
using Inno.Integration.WindowsX64.Bgfx;
using Inno.Integration.MacOSArm64.Bgfx;
using Inno.Integration.BrowserWasm32.Bgfx;
using Xunit;

namespace Inno.Build.Tests;

public sealed class PlatformBackendIntegrationTests
{
    [Fact]
    public void IntegrationsPreserveExactShaderAndNativeTargetIdentities()
    {
        Assert.Equal("windows-x64", WindowsX64BgfxIntegration.nativeProfile.targetId);
        Assert.Equal("macos-arm64", MacOSArm64BgfxIntegration.nativeProfile.targetId);
        var context = new BuildCompositionContext("explicit-sdk", System.IO.Path.GetTempPath(),
            new BuildHostDescriptor("Windows", "x64"), BuildTargetId.windowsX64, new NativeBindingGenerator("explicit-sdk"));
        var standard = StandardBuildDistribution.Create(context);
        Assert.Same(WindowsX64BgfxIntegration.shaderProfile, standard.ResolveShaderTarget(BuildTargetId.windowsX64));
        Assert.Same(MacOSArm64BgfxIntegration.shaderProfile, standard.ResolveShaderTarget(BuildTargetId.macOSArm64));
        Assert.Same(BrowserWasm32BgfxIntegration.shaderProfile, standard.ResolveShaderTarget(BuildTargetId.browserWasm));
    }
}
