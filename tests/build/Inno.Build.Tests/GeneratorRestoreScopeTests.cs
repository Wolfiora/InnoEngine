using Inno.Testing.Source;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class GeneratorRestoreScopeTests
{
    [Theory]
    [InlineData("src/runtime/generators/Inno.Runtime.Generators/Inno.Runtime.Generators.csproj")]
    [InlineData("src/foundation/core/Inno.Core.Serialization.Generators/Inno.Core.Serialization.Generators.csproj")]
    public async Task ExplicitRuntimeIdentityDoesNotOverwriteTheNeutralCompilerLock(string relative)
    {
        string root = TestRepository.GetRoot(typeof(GeneratorRestoreScopeTests).Assembly);
        string project = Path.Combine(root, relative);
        using Process process = new() { StartInfo = new ProcessStartInfo("dotnet")
        {
            UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true
        } };
        foreach (string argument in new[]
        {
            "msbuild", project, "-getProperty:NuGetLockFilePath,BaseIntermediateOutputPath,IsRoslynComponent",
            "-p:InnoNativeTarget=windows-x64", "-p:RuntimeIdentifier=win-x64", "-nodeReuse:false"
        })
            process.StartInfo.ArgumentList.Add(argument);
        Assert.True(process.Start());
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        try
        {
            await process.WaitForExitAsync(cancellation.Token);
        }
        finally
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync();
            }
        }
        Assert.Equal(0, process.ExitCode);
        Assert.Empty(await error);
        using JsonDocument document = JsonDocument.Parse(await output);
        JsonElement properties = document.RootElement.GetProperty("Properties");
        Assert.Equal("true", properties.GetProperty("IsRoslynComponent").GetString());
        Assert.Equal("obj/", properties.GetProperty("BaseIntermediateOutputPath").GetString()!.Replace('\\', '/'));
        Assert.Equal(Path.Combine(Path.GetDirectoryName(project)!, "packages.rid-win-x64.lock.json"),
            Path.GetFullPath(properties.GetProperty("NuGetLockFilePath").GetString()!));
    }
}
