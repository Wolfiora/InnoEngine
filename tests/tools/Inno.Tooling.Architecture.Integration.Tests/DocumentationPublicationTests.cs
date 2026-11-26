using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Testing.Source;

namespace Inno.Tooling.Architecture.Integration.Tests;

public sealed class DocumentationPublicationTests
{
    [Fact]
    public async Task RetainsAuthorChangesAcrossRetriesAndRejectsSupersededExports()
    {
        string engine = TestRepository.GetRoot(typeof(DocumentationPublicationTests).Assembly);
        string root = Directory.CreateTempSubdirectory("InnoPublication-").FullName;
        try
        {
            string project = Path.Combine(engine, "tools/Inno.Tooling.Cli/Inno.Tooling.Cli.csproj");
            string output = Path.Combine(root, "sdk");
            string sdk = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
            await ToolchainEnvironment.RunAsync(
                sdk,
                ["build", project, "-c", "Release", "--artifacts-path", output, "--disable-build-servers"],
                engine
            );
            string fixture = Path.Combine(
                engine,
                "tests/tools/Inno.Tooling.Architecture.Integration.Tests/fixtures/documentation_publication.py"
            );
            await ToolchainEnvironment.RunAsync(OperatingSystem.IsWindows() ? "python" : "python3", [
                "-B",
                fixture,
                Path.Combine(engine, ".github/scripts/documentation_artifacts.py"),
                Path.Combine(output, "bin/Inno.Tooling.Cli/release/Inno.Tooling.Cli.dll"),
                Path.Combine(root, "repos")
            ], engine);
        }
        finally
        {
            foreach (string file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }
            Directory.Delete(root, recursive: true);
        }
    }
}
