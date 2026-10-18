using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class DotNetSdkEnvironmentTests
{
    [Fact]
    public async Task SelectedSdkIgnoresAnInvokingBuildsSdkIdentityAndPreservesItsNativeEnvironment()
    {
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        string? parent = Environment.GetEnvironmentVariable("MSBuildSDKsPath");
        var inherited = new Dictionary<string, string>
        {
            ["MSBuildSDKsPath"] = "not-an-installed-sdk",
            ["MSBUILD_EXE_PATH"] = "not-a-loaded-msbuild",
            ["DOTNET_MSBUILD_SDK_RESOLVER_SDKS_DIR"] = "not-a-resolver-sdk",
            ["INNO_SELECTED_NATIVE_SDK"] = "explicit-native-sdk"
        };
        IReadOnlyDictionary<string, string?> selected = DotNetSdkEnvironment.Create(host, inherited);
        Assert.Null(selected["MSBuildSDKsPath"]);
        Assert.Null(selected["MSBUILD_EXE_PATH"]);
        Assert.Equal("explicit-native-sdk", selected["INNO_SELECTED_NATIVE_SDK"]);
        Assert.Equal("not-an-installed-sdk", inherited["MSBuildSDKsPath"]);
        string root = Path.Combine(Path.GetTempPath(), "InnoManagedSdk-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            string project = Path.Combine(root, "Sdk.csproj");
            await File.WriteAllTextAsync(project,
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup></Project>");
            string output = await ToolchainEnvironment.CaptureOutputAsync(host,
                ["msbuild", project, "-nologo", "-nodeReuse:false", "-getProperty:TargetFramework"],
                root, CancellationToken.None, selected);
            Assert.Equal("net9.0", output.Trim());
            string resolvedHost = ToolchainEnvironment.ResolveExecutable(host);
            string relativeHost = Path.GetRelativePath(Environment.CurrentDirectory, resolvedHost);
            DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(relativeHost, project);
            Assert.Equal(resolvedHost, sdk.hostPath);
            string sdkVersion = await ToolchainEnvironment.CaptureOutputAsync(sdk.hostPath,
                [sdk.cliPath, "--version"], root, CancellationToken.None,
                DotNetSdkEnvironment.Create(sdk.hostPath));
            Assert.Equal(sdk.sdkIdentity, sdkVersion.Trim());
            Assert.Equal(parent, Environment.GetEnvironmentVariable("MSBuildSDKsPath"));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
