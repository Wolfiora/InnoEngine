using Inno.Build.Bindings;
using System;
using Inno.Build.Distribution.Standard;
using Inno.Build.Composition;
using Inno.Build.Toolchains;
using Inno.Integration.Windows.Bgfx;
using Inno.Integration.MacOS.Bgfx;
using Inno.Integration.Browser.Bgfx;
using Inno.Integration.Linux.Bgfx;
using Xunit;

namespace Inno.Build.Tests;

public sealed class PlatformBackendIntegrationTests
{
    [Fact]
    public void IntegrationsPreserveExactShaderAndNativeTargetIdentities()
    {
        Assert.Equal("windows-x64", WindowsBgfxIntegration.nativeProfile.targetId);
        Assert.Equal("macos-arm64", MacOSBgfxIntegration.nativeProfile.targetId);
        Assert.Equal("linux-x64", LinuxBgfxIntegration.CreateNativeProfile("linux-x64").targetId);
        Assert.Equal("linux-arm64", LinuxBgfxIntegration.CreateNativeProfile("linux-arm64").targetId);
        Assert.Throws<NotSupportedException>(() => LinuxBgfxIntegration.CreateNativeProfile("windows-x64"));
        var context = new BuildCompositionContext("explicit-sdk", System.IO.Path.GetTempPath(),
            new BuildHostDescriptor("Windows", "x64"), BuildTargetId.windowsX64, new NativeBindingGenerator("explicit-sdk"));
        var standard = StandardBuildDistribution.Create(context);
        Assert.Same(WindowsBgfxIntegration.shaderProfile, standard.ResolveShaderTarget(BuildTargetId.windowsX64));
        Assert.Same(MacOSBgfxIntegration.shaderProfile, standard.ResolveShaderTarget(BuildTargetId.macOSArm64));
        Assert.Same(BrowserBgfxIntegration.shaderProfile, standard.ResolveShaderTarget(BuildTargetId.browserWasm));
    }
}
