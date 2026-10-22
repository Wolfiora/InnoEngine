using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Integration.WindowsX64.Bgfx;

namespace Inno.Build.Tests;

public sealed class ShaderToolSelectionTests
{
    [Fact]
    public void NativeGraphicsSelectionDoesNotDependOnTheManagedSdkExecutable()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoShaderToolSelection", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "fixture");
            string owner = Path.Combine(root, "platforms", "WindowsX64", "integrations", "Inno.Integration.WindowsX64.Bgfx");
            Directory.CreateDirectory(owner);
            foreach (string file in new[] { "WindowsX64BgfxToolContribution.cs", "WindowsX64BgfxBuildProfile.cs" })
                File.WriteAllText(Path.Combine(owner, file), "declared integration input");
            var tools = new Dictionary<string, string>();
            foreach (string name in new[] { "cl", "msbuild", "genie", "cmake", "dotnet" })
            {
                tools[name] = Path.Combine(root, name + ".exe");
                File.WriteAllText(tools[name], "frozen executable input");
            }
            var sdk = new NativeToolchainSelection("windows-x64", new BuildHostDescriptor("Windows", "x64"),
                tools, new Dictionary<string, string>(), [], [], new NativeLinkPlan(".dll"), true);
            NativeToolchainSelection first = WindowsX64BgfxIntegration.nativeProfile.SelectTools(new NativeBuildContext(root, "release").WithToolchain(sdk));
            tools["dotnet"] = Path.Combine(root, "another-dotnet.exe");
            File.WriteAllText(tools["dotnet"], "another managed SDK");
            var otherSdk = new NativeToolchainSelection("windows-x64", sdk.host, tools, sdk.environment, [], [], new NativeLinkPlan(".dll"), true);
            NativeToolchainSelection second = WindowsX64BgfxIntegration.nativeProfile.SelectTools(new NativeBuildContext(root, "release").WithToolchain(otherSdk));

            Assert.Equal(first.declarations, second.declarations);
            Assert.Equal(first.inputPaths, second.inputPaths);
            Assert.False(first.TryResolveExecutable("dotnet", out _));
            Assert.False(first.TryResolveExecutable("cmake", out _));
            Assert.Equal(tools["msbuild"], first.ResolveExecutable("msbuild"));
            Assert.DoesNotContain(first.inputPaths, path => Path.GetFileName(path).Contains("dotnet", StringComparison.Ordinal));
            Assert.Empty(first.cmakeArguments);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
