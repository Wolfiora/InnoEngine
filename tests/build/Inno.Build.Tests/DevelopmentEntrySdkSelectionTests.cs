using System;
using System.IO;
using System.Threading.Tasks;
using Inno.Entry.Bootstrap;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class DevelopmentEntrySdkSelectionTests
{
    [Theory]
    [InlineData("--dotnet")]
    [InlineData("--dotnet", "--configuration", "Release")]
    [InlineData("--dotnet", "dotnet", "--dotnet", "dotnet")]
    [InlineData("--dotnet", "inno-missing-sdk-executable")]
    public async Task InvalidSdkSelectionFailsBeforeCheckoutOwnership(params string[] options)
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string ownership = Path.Combine(root, "artifacts", "entry", "checkout.lock");
        DateTime? original = File.Exists(ownership) ? File.GetLastWriteTimeUtc(ownership) : null;
        Assert.Equal(1, await DevelopmentEntry.RunBuildAsync(["editor", .. options]));
        Assert.Equal(original, File.Exists(ownership) ? File.GetLastWriteTimeUtc(ownership) : null);
    }
}
