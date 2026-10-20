using Inno.Testing.Source;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeBindingSelectionBuildTests
{
    [Fact]
    public async Task ANewSelectionDocumentDoesNotRecompileANeutralProject()
    {
        string repository = TestRepository.GetRoot(typeof(NativeBindingSelectionBuildTests).Assembly);
        string root = Path.Combine(Path.GetTempPath(), "InnoSelectionTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            foreach (string name in new[] { "Directory.Build.props", "Directory.Build.targets" })
                File.WriteAllText(Path.Combine(root, name), "<Project><Import Project=\""
                    + SecurityElement.Escape(Path.Combine(repository, name)) + "\" /></Project>");
            File.WriteAllText(Path.Combine(root, "Probe.csproj"), """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup><TargetFramework>net10.0</TargetFramework><ImplicitUsings>disable</ImplicitUsings></PropertyGroup>
                </Project>
                """);
            File.WriteAllText(Path.Combine(root, "Probe.cs"), "public static class Probe { public static int Value => 7; }");
            string first = Path.Combine(root, "first-selection.props");
            string second = Path.Combine(root, "second-selection.props");
            File.WriteAllText(first, "<Project />");
            await Build(root, first);
            string assembly = Path.Combine(root, "bin", "Debug", "net10.0", "Probe.dll");
            DateTime written = File.GetLastWriteTimeUtc(assembly);
            File.WriteAllText(second, "<Project />");
            await Build(root, second);
            Assert.Equal(written, File.GetLastWriteTimeUtc(assembly));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static async Task Build(
        string root,
        string selection
    ) {
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
        foreach (string argument in new[] { "build", "Probe.csproj", "--disable-build-servers", "--nologo", "-m:1",
            "-nodeReuse:false", "-p:InnoNativeBindingSelection=" + selection })
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start) ?? throw new InvalidOperationException("MSBuild did not start.");
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> errors = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(2));
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
        Assert.True(process.ExitCode == 0, await output + await errors);
    }
}
