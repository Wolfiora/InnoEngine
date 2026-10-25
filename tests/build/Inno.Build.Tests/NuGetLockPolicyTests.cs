using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.Toolchains;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NuGetLockPolicyTests
{
    [Theory]
    [InlineData("PublishAot=true", ".aot")]
    [InlineData("PublishTrimmed=true", ".link")]
    [InlineData("IsAotCompatible=true", ".link")]
    [InlineData("EnableTrimAnalyzer=true", ".link")]
    [InlineData("PublishSingleFile=true", ".link")]
    [InlineData("PublishSingleFile=true;EnableSingleFileAnalyzer=false", "")]
    public async Task SdkInferredToolPackagesOwnDistinctLockScopes(
        string properties,
        string expectedScope
    ) {
        string root = TestRepository.GetRoot(typeof(NuGetLockPolicyTests).Assembly);
        string host = ToolchainEnvironment.ResolveExecutable("dotnet");
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        string selected = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", Path.Combine(root, "src", "foundation", "core", "Inno.Core.IO", "Inno.Core.IO.csproj"),
                "-p:" + properties, "-p:RuntimeIdentifier=win-x64", "-getProperty:NuGetLockFilePath", "-nologo", "-nr:false"],
            root, cancellation.Token, DotNetSdkEnvironment.Create(host));
        Assert.Equal("packages.rid-win-x64" + expectedScope + ".lock.json", Path.GetFileName(selected.Trim()));
    }

    [Theory]
    [InlineData("win-x64", "", "packages.host-win-x64.lock.json")]
    [InlineData("osx-arm64", "", "packages.host-osx-arm64.lock.json")]
    [InlineData("win-x64", "win-x64", "packages.host-win-x64.rid-win-x64.lock.json")]
    public async Task EditorRoutingOwnsItsHostContext(
        string sdkHost,
        string runtime,
        string expectedFile
    ) {
        string root = TestRepository.GetRoot(typeof(NuGetLockPolicyTests).Assembly);
        string host = ToolchainEnvironment.ResolveExecutable("dotnet");
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        string selected = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", Path.Combine(root, "entry", "InnoEngine.Editor", "InnoEngine.Editor.csproj"),
                "-p:NETCoreSdkRuntimeIdentifier=" + sdkHost, "-p:RuntimeIdentifier=" + runtime,
                "-getProperty:NuGetLockFilePath", "-nologo", "-nr:false"],
            root, cancellation.Token, DotNetSdkEnvironment.Create(host));
        Assert.Equal(expectedFile, Path.GetFileName(selected.Trim()));
    }

    [Fact]
    public async Task OfficialRestoreReusesProductLabelsAndKeepsRuntimeGraphsIndependent()
    {
        string root = TestRepository.GetRoot(typeof(NuGetLockPolicyTests).Assembly);
        string host = ToolchainEnvironment.ResolveExecutable("dotnet");
        string workspace = Path.Combine(Path.GetTempPath(), "InnoNuGetLockPolicy", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(workspace);
        try
        {
            string project = Path.Combine(workspace, "Fixture.csproj");
            var package = new XElement("PackageReference", new XAttribute("Include", "BGCS.Runtime"),
                new XAttribute("Version", "[4.1.2]"));
            var document = new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup", new XElement("TargetFramework", "net10.0"),
                    new XElement("RestorePackagesWithLockFile", "true"),
                    new XElement("AfterMicrosoftNETSdkTargets", Path.Combine(root, "build", "msbuild", "Inno.NuGetLockFiles.targets"))),
                new XElement("ItemGroup", package)));
            document.Save(project);
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            await RestoreAsync(host, project, "", "fixture-a", locked: false, cancellation.Token);
            string neutral = Path.Combine(workspace, "packages.lock.json");
            byte[] original = SHA256.HashData(File.ReadAllBytes(neutral));
            foreach (string runtime in new[] { "", "win-x64", "osx-arm64" })
                await RestoreAsync(host, project, runtime, "fixture-b", locked: runtime.Length == 0, cancellation.Token);
            Assert.Equal(original, SHA256.HashData(File.ReadAllBytes(neutral)));
            Assert.Equal(3, Directory.GetFiles(workspace, "packages*.lock.json").Length);
            Assert.True(File.Exists(Path.Combine(workspace, "packages.rid-win-x64.lock.json")));
            Assert.True(File.Exists(Path.Combine(workspace, "packages.rid-osx-arm64.lock.json")));
            foreach (string runtime in new[] { "", "win-x64", "osx-arm64" })
                await RestoreAsync(host, project, runtime, "fixture-c", locked: true, cancellation.Token);

            package.SetAttributeValue("Version", "[4.1.1]");
            document.Save(project);
            InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                RestoreAsync(host, project, "", "fixture-c", locked: true, cancellation.Token));
            Assert.Contains("exit code", failure.Message, StringComparison.Ordinal);
            Assert.Equal(original, SHA256.HashData(File.ReadAllBytes(neutral)));
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    private static Task<string> RestoreAsync(
        string host,
        string project,
        string runtime,
        string target,
        bool locked,
        CancellationToken cancellation
    ) => ToolchainEnvironment.CaptureOutputAsync(host,
        ["restore", project, "-p:RuntimeIdentifier=" + runtime, "-p:InnoNativeTarget=" + target,
            "-p:RestoreLockedMode=" + locked, "-v:quiet", "--disable-build-servers"],
        Path.GetDirectoryName(project)!, cancellation, DotNetSdkEnvironment.Create(host));
}
