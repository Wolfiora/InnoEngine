using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class ToolchainDiagnosticTests
{
    [Fact]
    public async Task CapturedSdkFailuresRetainTheirOriginalDiagnostic()
    {
        string parent = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "InnoToolDiagnostics"));
        string root = Path.Combine(parent, Guid.NewGuid().ToString("N"));
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? Path.Combine(Environment.GetEnvironmentVariable("DOTNET_ROOT") ?? throw new InvalidOperationException("The test requires its selected SDK."),
                OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        Directory.CreateDirectory(root);
        try
        {
            string project = Path.Combine(root, "Failure.proj");
            File.WriteAllText(project, """
                <Project>
                  <Target Name="Diagnose">
                    <Message Importance="High" Text="Original SDK diagnostic before failure." />
                    <Error Text="The declared fixture input is unavailable." />
                  </Target>
                </Project>
                """);
            InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                ToolchainEnvironment.CaptureOutputAsync(host, ["msbuild", project, "-t:Diagnose", "-nologo", "-nr:false"],
                    root, CancellationToken.None, DotNetSdkEnvironment.Create(host)));
            Assert.Contains("failed with exit code", failure.Message);
            Assert.Contains("Original SDK diagnostic before failure.", failure.Message);
            Assert.Contains("The declared fixture input is unavailable.", failure.Message);
            Assert.IsType<InvalidOperationException>(failure.InnerException);
        }
        finally
        {
            if (!Path.GetFullPath(root).StartsWith(parent + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The diagnostic fixture escaped its owner.");
            Directory.Delete(root, recursive: true);
        }
    }
}
