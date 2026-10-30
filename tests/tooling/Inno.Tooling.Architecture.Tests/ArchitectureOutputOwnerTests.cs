using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class ArchitectureOutputOwnerTests
{
    [Fact]
    public async Task PublicContractsUseTheEvaluatedOutputOwnerInsteadOfStaleDefaultAssemblies()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoArchitectureOutputTests", Guid.NewGuid().ToString("N"));
        string host = Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "..", "..", "..",
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"));
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(2));
        try
        {
            string contract = Path.Combine(root, "src/foundation/core/Inno.Core.Contract");
            string editor = Path.Combine(root, "src/composition/editor/framework/Inno.Editor.Probe");
            Directory.CreateDirectory(contract);
            Directory.CreateDirectory(editor);
            File.WriteAllText(Path.Combine(root, "AGENTS.md"), "# Fixture");
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), string.Empty);
            File.WriteAllText(Path.Combine(contract, "Inno.Core.Contract.csproj"), """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>
                </Project>
                """);
            File.WriteAllText(Path.Combine(contract, "Contract.cs"), "namespace Inno.Core.Contract; public sealed class Contract { }");
            string project = Path.Combine(editor, "Inno.Editor.Probe.csproj");
            File.WriteAllText(project, """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>
                  <ItemGroup><ProjectReference Include="../../../../foundation/core/Inno.Core.Contract/Inno.Core.Contract.csproj" /></ItemGroup>
                </Project>
                """);
            string source = Path.Combine(editor, "Probe.cs");
            File.WriteAllText(source, "namespace Inno.Editor.Probe; public sealed class Probe { }");
            await Build([]);
            File.WriteAllText(source, "namespace Inno.Editor.Probe; public sealed class Probe { public Inno.Core.Contract.Contract value { get; } = new(); }");
            string artifacts = Path.Combine(root, "owned-output");
            await Build(["-p:ArtifactsPath=" + artifacts]);
            string graph = Path.Combine(root, "graph.json");

            string correct = ArchitectureCommandFixture.Run(root, "--dotnet", host,
                "--artifacts-path", artifacts, "--project-graph", graph).output;
            Assert.DoesNotContain("Inno.Core.Contract is an implementation dependency", correct, StringComparison.Ordinal);
            Assert.Contains(artifacts.Replace('\\', '/'), File.ReadAllText(graph).Replace('\\', '/').Replace("//", "/"), StringComparison.OrdinalIgnoreCase);

            File.WriteAllText(project, File.ReadAllText(project).Replace("Include=", "PrivateAssets=\"compile\" Include=", StringComparison.Ordinal));
            string rejected = ArchitectureCommandFixture.Run(root, "--dotnet", host, "--artifacts-path", artifacts).output;
            Assert.Contains("public/protected API exposes Inno.Core.Contract", rejected, StringComparison.Ordinal);

            async Task Build(string[] properties)
            {
                await ToolchainEnvironment.RunAsync(host, ["build", project, "--disable-build-servers", "-m:1", "-nr:false", "-v:quiet", .. properties],
                    root, cancellation.Token, DotNetSdkEnvironment.Create(host));
            }
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }
}
