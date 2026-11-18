using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Build.Integration.Tests;

public sealed class ProductBindingSelectionTests
{
    [Theory]
    [InlineData("WindowsX64", "windows-x64", "EditorProduct.props", "editor")]
    [InlineData("WindowsX64", "windows-x64", "ProductBuild.props", "player")]
    [InlineData("MacOSArm64", "macos-arm64", "EditorProduct.props", "editor")]
    [InlineData("MacOSArm64", "macos-arm64", "ProductBuild.props", "player")]
    [InlineData("BrowserWasm32", "browser-wasm", "ProductBuild.props", "player")]
    public async Task StandaloneBindingCompilationDeclaresItsDependencyWithoutProductDeployment(
        string platform,
        string target,
        string properties,
        string product
    ) {
        string root = TestRepository.GetRoot(typeof(ProductBindingSelectionTests).Assembly);
        string dotnet = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        var start = new ProcessStartInfo(dotnet)
        {
            WorkingDirectory = root,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (string argument in new[]
        {
            "msbuild", "backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj", "-nologo",
            "-getProperty:InnoBindingProductId,InnoProductId,InnoProductRestoreProject,BindGenGeneratedBindings",
            "-p:Configuration=Debug", "-p:InnoNativeTarget=" + target,
            "-p:InnoProductBuildProperties=" + Path.Combine(root, "platforms", platform, "build", "Inno.Build." + platform, properties)
        })
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start) ?? throw new InvalidOperationException("MSBuild did not start.");
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> errors = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        try
        {
            await process.WaitForExitAsync(timeout.Token);
        }
        catch
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync();
            await output;
            await errors;
            throw;
        }
        string result = await output;
        Assert.True(process.ExitCode == 0, result + await errors);
        using JsonDocument document = JsonDocument.Parse(result);
        JsonElement declared = document.RootElement.GetProperty("Properties");
        Assert.Equal(product, declared.GetProperty("InnoBindingProductId").GetString());
        Assert.Empty(declared.GetProperty("InnoProductId").GetString()!);
        Assert.Empty(declared.GetProperty("InnoProductRestoreProject").GetString()!);
        Assert.Equal("true", declared.GetProperty("BindGenGeneratedBindings").GetString());
    }
}
