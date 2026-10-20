using Inno.Testing.Source;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Build.Tests;

public sealed class TaskPublisherBootstrapTests
{
    [Fact]
    public async Task ProjectGraphSharesCompilationButKeepsPrivateLoadsAndFreshBuildOwnership()
    {
        string repository = TestRepository.GetRoot(typeof(TaskPublisherBootstrapTests).Assembly);
        string root = Path.Combine(Path.GetTempPath(), "InnoPublisherTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(Path.Combine(root, "Directory.Build.props"),
                "<Project><Import Project=\"" + SecurityElement.Escape(Path.Combine(repository, "Directory.Build.props"))
                + "\" /></Project>");
            File.WriteAllText(Path.Combine(root, "Directory.Build.targets"), $$"""
                <Project>
                  <Import Project="{{SecurityElement.Escape(Path.Combine(repository, "Directory.Build.targets"))}}" />
                  <Target Name="ReportPublisher" AfterTargets="Build" DependsOnTargets="PrepareInnoBuildTaskHost">
                    <Message Importance="high" Text="INNO-PUBLISHER-PROBE|$(MSBuildProjectName)|$(InnoTaskBootstrapDirectory)|$(InnoTaskPublisherAssembly)" />
                  </Target>
                </Project>
                """);
            foreach (string name in new[] { "Root", "Left", "Right", "Shared" })
            {
                string directory = Path.Combine(root, name);
                Directory.CreateDirectory(directory);
                string[] dependencies = name switch
                {
                    "Root" => ["Left", "Right"],
                    "Left" or "Right" => ["Shared"],
                    _ => []
                };
                string references = string.Join(string.Empty, dependencies.Select(static name =>
                    $"<ProjectReference Include=\"../{name}/{name}.csproj\" />"));
                File.WriteAllText(Path.Combine(directory, name + ".csproj"),
                    "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework>"
                    + "<ImplicitUsings>disable</ImplicitUsings></PropertyGroup><ItemGroup>" + references + "</ItemGroup></Project>");
            }
            string first = await Build(root);
            string firstOwner = Validate(first);
            string second = await Build(root);
            string secondOwner = Validate(second);
            Assert.NotEqual(firstOwner, secondOwner);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static string Validate(string output)
    {
        string[][] reports = output.Split('\n').Select(static line => line.Trim())
            .Where(static line => line.StartsWith("INNO-PUBLISHER-PROBE|", StringComparison.Ordinal))
            .Select(static line => line.Split('|')).ToArray();
        Assert.True(reports.Length == 4, output);
        Assert.Equal(4, reports.Select(static report => report[1]).Distinct().Count());
        Assert.Equal(4, reports.Select(static report => report[3]).Distinct().Count());
        string owner = Assert.Single(reports.Select(static report => report[2]).Distinct());
        Assert.True(output.Split('\n').Count(static line => line.Contains("Inno.Build.TaskHosting ->", StringComparison.Ordinal)) == 1, output);
        foreach (string[] report in reports)
            Assert.True(File.Exists(report[3]), "The private publisher was not retained for its loading process: " + report[3]);
        return owner;
    }

    private static async Task<string> Build(string root)
    {
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
        foreach (string argument in new[] { "build", "Root/Root.csproj", "--disable-build-servers", "--nologo", "-m:2", "-nodeReuse:false" })
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start) ?? throw new InvalidOperationException("MSBuild did not start.");
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> errors = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(10));
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
        string diagnostics = await output + await errors;
        Assert.True(process.ExitCode == 0, diagnostics);
        return diagnostics;
    }
}
