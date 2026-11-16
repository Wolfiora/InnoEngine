using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Tooling.Validation;

namespace Inno.Tooling.Validation.Tests;

internal sealed class ValidationRepository : IDisposable
{
    private readonly string m_root = Path.Combine(new PhysicalPathResolver().Resolve(Path.GetTempPath()), "InnoValidation", Guid.NewGuid().ToString("N"));
    private readonly string m_dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
        ?? Path.Combine(Environment.GetEnvironmentVariable("DOTNET_ROOT") ?? throw new InvalidOperationException("The test requires its selected SDK host."),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");

    internal ValidationRepository(string? source = null)
    {
        Directory.CreateDirectory(m_root);
        if (source is not null)
        {
            Git("clone", "--shared", source, ".");
            Git("config", "user.name", "Aaron Liao");
            Git("config", "user.email", "hsuankailiao@gmail.com");
            string[] changed = Git("-C", source, "diff", "--name-only", "HEAD")
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string[] added = Git("-C", source, "ls-files", "--others", "--exclude-standard")
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);
            // Pair the isolated checkout with the current tool inputs, including uncommitted layout migrations.
            foreach (string relative in changed.Concat(added).Distinct(StringComparer.Ordinal))
            {
                string original = PathBoundary.RequireUnlinkedPath(source, Path.Combine(source, relative));
                string destination = PathBoundary.RequireUnlinkedPath(m_root, PathOf(relative));
                if (File.Exists(original))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                    File.Copy(original, destination, overwrite: true);
                }
                else
                    File.Delete(destination);
            }
            if (changed.Length != 0 || added.Length != 0)
            {
                Git("add", "--all");
                Git("commit", "-m", "Use the checkout inputs paired with the validation assembly");
            }
            return;
        }
        Write(".gitignore", "artifacts/\n**/obj/\n**/bin/\n");
        Write("InnoEngine.sln", "Microsoft Visual Studio Solution File, Format Version 12.00\n");
        Write("Shared.props", "<Project />");
        Write("src/Example/Example.csproj", Project("<Import Project=\"../../Shared.props\" />"));
        Write("src/Other/Other.csproj", Project());
        Write("tests/Example.Tests/Example.Tests.csproj", Project("""
            <PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>domain</InnoValidationLayer></PropertyGroup>
            <ItemGroup><ProjectReference Include="../../src/Example/Example.csproj" /><InnoValidationInput Include="../../fixtures/plugin.txt" /></ItemGroup>
            """));
        Write("fixtures/plugin.txt", "initial input");
        Git("init", "-b", "master");
        Git("config", "user.name", "Aaron Liao");
        Git("config", "user.email", "hsuankailiao@gmail.com");
        Git("add", ".");
        Git("commit", "-m", "Validation fixture baseline");
    }

    public void Dispose()
    {
        string root = Path.GetFullPath(m_root);
        string parent = Path.Combine(new PhysicalPathResolver().Resolve(Path.GetTempPath()), "InnoValidation") + Path.DirectorySeparatorChar;
        if (!root.StartsWith(parent, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("The fixture owner escaped its temporary boundary.");
        foreach (string file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
            if (new FileInfo(file).LinkTarget is null)
                File.SetAttributes(file, FileAttributes.Normal);
        Directory.Delete(root, recursive: true);
    }

    internal string PathOf(string path) => Path.GetFullPath(path, m_root);

    internal string dotnet => m_dotnet;

    internal void Write(
        string path,
        string content
    ) {
        string destination = PathOf(path);
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        File.WriteAllText(destination, content);
    }

    internal string Git(params string[] arguments)
    {
        var start = new ProcessStartInfo("git") { WorkingDirectory = m_root, UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (string argument in arguments)
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start)!;
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        process.WaitForExit();
        if (process.ExitCode != 0)
            throw new InvalidOperationException(output.Result + error.Result);
        return output.Result.Trim();
    }

    internal async Task<JsonDocument> PlanAsync(
        string baseline = "HEAD",
        bool extended = false,
        string configuration = "Debug",
        string? documentationCache = null,
        string? documentationSnapshot = null,
        string? documentationBaseline = null
    )
    {
        string output = PathOf("artifacts/plan");
        await ValidationCommand.ExecuteAsync(["plan", "--engine-root", m_root, "--dotnet", m_dotnet, "--base", baseline, "--output", output,
            "--extended", extended.ToString(), "--configuration", configuration,
            ..documentationCache is null ? Array.Empty<string>() : new[] { "--docs-cache-root", documentationCache },
            ..documentationSnapshot is null ? Array.Empty<string>() : new[] { "--docs-snapshot", documentationSnapshot },
            ..documentationBaseline is null ? Array.Empty<string>() : new[] { "--docs-base-snapshot", documentationBaseline }]);
        return JsonDocument.Parse(File.ReadAllText(Path.Combine(output, "plan.json")));
    }

    internal Task<int> EvidenceAsync(string evidence, string plan = "artifacts/plan/plan.json")
        => ValidationCommand.ExecuteAsync(["evidence", "--engine-root", m_root, "--dotnet", m_dotnet,
            "--plan", PathOf(plan), "--evidence", evidence]);

    internal Task<int> RunCurrentAsync(
        CancellationToken cancellation,
        string output
    )
        => ValidationCommand.ExecuteAsync(["run", "--engine-root", m_root, "--dotnet", m_dotnet,
            "--base", "HEAD", "--output", PathOf(output)], cancellation);

    internal Task<int> RunAsync(
        CancellationToken cancellation = default,
        string output = "artifacts/run",
        string? evidence = null
    )
        => ValidationCommand.ExecuteAsync(["run", "--engine-root", m_root, "--dotnet", m_dotnet,
            "--plan", PathOf("artifacts/plan/plan.json"), "--output", PathOf(output),
            ..evidence is null ? Array.Empty<string>() : new[] { "--evidence", PathOf(evidence) }], cancellation);

    internal Task BuildAsync(params string[] arguments)
        => SdkAsync(["build", "--disable-build-servers", "-m:1", "-nr:false", ..arguments]);

    internal async Task SdkAsync(params string[] arguments)
    {
        var start = new ProcessStartInfo(m_dotnet) { WorkingDirectory = m_root, UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (string argument in arguments)
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start)!;
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
            throw new InvalidOperationException(await output + await error);
    }

    internal static string Project(string content = "")
        => "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework><ImplicitUsings>disable</ImplicitUsings></PropertyGroup>" + content + "</Project>";
}
