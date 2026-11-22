using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Cli;
using Inno.Testing.Source;

namespace Inno.Build.Cli.Integration.Tests;

public sealed class DevelopmentEntrySdkSelectionTests
{
    [Theory]
    [InlineData("--dotnet")]
    [InlineData("--dotnet", "--configuration", "Release")]
    [InlineData("--dotnet", "dotnet", "--dotnet", "dotnet")]
    [InlineData("--dotnet", "inno-missing-sdk-executable")]
    public async Task InvalidSdkSelectionFailsBeforeCheckoutOwnership(params string[] options)
    {
        string root = TestRepository.GetRoot(typeof(DevelopmentEntrySdkSelectionTests).Assembly);
        string ownership = Path.Combine(root, "artifacts", "entry", "checkout.lock");
        DateTime? original = File.Exists(ownership) ? File.GetLastWriteTimeUtc(ownership) : null;
        Assert.Equal(1, await BuildApplication.RunAsync(["editor", "--tools-target", "windows-x64", .. options], CancellationToken.None));
        Assert.Equal(original, File.Exists(ownership) ? File.GetLastWriteTimeUtc(ownership) : null);
    }
}
