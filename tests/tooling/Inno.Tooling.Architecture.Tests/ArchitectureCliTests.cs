using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

public sealed class ArchitectureCliTests
{
    [Theory]
    [InlineData("backends/Interop/native/Inno.Native.Probe", false)]
    [InlineData("src/services/input/Inno.Input.Probe", true)]
    public async Task CliChecksExplicitInteropRuntimeOwnership(
        string projectLocation,
        bool rejected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoInteropOwnershipTests", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            string project = Path.Combine(root, projectLocation);
            Directory.CreateDirectory(project);
            File.WriteAllText(Path.Combine(project, Path.GetFileName(project) + ".csproj"),
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup>"
                + "<ItemGroup><ProjectReference Include=\"$(BGCSRuntimeProject)\" /></ItemGroup></Project>");
            (_, string output) = await Run(root);
            Assert.Equal(rejected, output.Contains("the interop runtime dependency belongs", StringComparison.Ordinal));
            Assert.DoesNotContain("does not resolve to a repository project", output, StringComparison.Ordinal);
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("reference", "effective Foundation dependency")]
    [InlineData("source", "effective hand-authored Compile source")]
    [InlineData("target", "effective product target and native ABI")]
    public async Task CliChecksImportedMSBuildOwnership(
        string scenario,
        string expected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoEvaluatedOwnershipTests", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            string relative = scenario == "target"
                ? "platforms/Windows/player/Inno.Player.Windows"
                : "src/foundation/core/Inno.Core.Probe";
            string project = Path.Combine(root, relative);
            Directory.CreateDirectory(project);
            string properties = scenario == "target"
                ? "<OutputType>Exe</OutputType><InnoProductId>player</InnoProductId><InnoProductTarget>windows-x64</InnoProductTarget><InnoNativeTarget>windows-x64</InnoNativeTarget>"
                : string.Empty;
            File.WriteAllText(Path.Combine(project, Path.GetFileName(project) + ".csproj"),
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework>"
                + properties + "</PropertyGroup></Project>");
            string imported;
            if (scenario == "reference")
            {
                string upper = Path.Combine(root, "platforms/MacOS/runtime/Probe");
                Directory.CreateDirectory(upper);
                File.WriteAllText(Path.Combine(upper, "Probe.csproj"),
                    "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup></Project>");
                imported = "<ItemGroup Condition=\"'$(MSBuildProjectName)' == 'Inno.Core.Probe'\"><ProjectReference Include=\"$(MSBuildThisFileDirectory)platforms/MacOS/runtime/Probe/Probe.csproj\" /></ItemGroup>";
            }
            else if (scenario == "source")
            {
                string upper = Path.Combine(root, "src/content/Probe");
                Directory.CreateDirectory(upper);
                File.WriteAllText(Path.Combine(upper, "Probe.cs"), "internal class ImportedSource { }");
                imported = "<ItemGroup><Compile Include=\"$(MSBuildThisFileDirectory)src/content/Probe/Probe.cs\" Link=\"ImportedSource.cs\" /></ItemGroup>";
            }
            else
                imported = "<PropertyGroup><InnoNativeTarget>macos-arm64</InnoNativeTarget></PropertyGroup>";
            File.WriteAllText(Path.Combine(root, "Directory.Build.targets"), "<Project>" + imported + "</Project>");
            string dotnet = Path.GetFullPath("../../../dotnet", RuntimeEnvironment.GetRuntimeDirectory());
            if (OperatingSystem.IsWindows())
                dotnet += ".exe";
            string graph = Path.Combine(root, "evaluated.json");
            (int code, string output) = await Run(root, "--dotnet", dotnet, "--project-graph", graph);
            Assert.Equal(1, code);
            Assert.Contains(expected, output, StringComparison.Ordinal);
            Assert.True(File.Exists(graph));
            Assert.False(Directory.Exists(Path.Combine(project, "bin")));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("internal sealed class Probe { void Apply(int first, int second) { } }", true)]
    [InlineData("internal sealed class Probe(int first, int second) { }", true)]
    [InlineData("internal sealed class Probe { System.Func<int,int,int> factory = (first, second) => first + second; }", true)]
    [InlineData("internal sealed class Probe\n{\n    void Apply(\n        int first,\n        int second\n    ) { }\n}", false)]
    public async Task CliChecksHandwrittenMultiParameterDeclarations(
        string source,
        bool rejected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoDeclarationStyleTests", Guid.NewGuid().ToString("N"));
        try
        {
            foreach (string folder in new[] { "src", "native", "build", "tools", "tests" })
                Directory.CreateDirectory(Path.Combine(root, folder));
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            File.WriteAllText(Path.Combine(root, "src", "Probe.cs"), source);

            (_, string output) = await Run(root);

            Assert.Equal(rejected, output.Contains("place each parameter", StringComparison.Ordinal));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("src/services/audio/Probe.cs", "global using System;", "global using", true)]
    [InlineData("src/composition/player/Inno.Player.Runtime/Probe.cs", "internal class Probe { private BgfxDevice device; }", "backend-neutral", true)]
    [InlineData("src/content/assets/Probe.cs", "internal class Probe { void Read() { context.With<IAssetReferenceResolver>(resolver); } }", "owner-complete", true)]
    [InlineData("src/services/audio/Probe.cs", "public class Probe { public MaEngine engine; }", "native implementation", true)]
    [InlineData("src/services/audio/Probe.cs", "internal class Probe { void Release() { OnCleanupFailed(phase, failure); } }", "diagnostic-only", true)]
    [InlineData("src/services/audio/Probe.cs", "internal class Probe { void Release() { try { } catch (RetirementPendingException) { throw; } } }", "wrapped pending ownership", true)]
    [InlineData("src/services/audio/Probe.cs", "internal class Probe { void Release() { try { } catch (Inno.Core.Execution.RetirementTimeoutException failure) { throw; } } }", "wrapped pending ownership", true)]
    [InlineData("src/services/audio/Probe.cs", "internal class Probe { void Release() { try { } catch (Exception error) when (RetirementPendingException.Find(error) is not null) { throw; } } }", "wrapped pending ownership", false)]
    public async Task CliValidatesSourceContracts(
        string relative,
        string source,
        string expected,
        bool rejected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoArchitectureTests", Guid.NewGuid().ToString("N"));
        try
        {
            foreach (string folder in new[] { "src", "native", "build", "tools", "tests" })
                Directory.CreateDirectory(Path.Combine(root, folder));
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), """
                Microsoft Visual Studio Solution File, Format Version 12.00
                Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "src", "src", "{BBD55508-6095-4D53-B0E4-59B04BFB9376}"
                EndProject
                Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "tests", "tests", "{A98FE84D-9721-4B99-950D-05CF68C303F2}"
                EndProject
                Global
                EndGlobal
                """);
            (int baselineCode, string baseline) = await Run(root);
            // This isolated source fixture intentionally has no product projects or compiled assemblies.
            Assert.Equal(1, baselineCode);
            Assert.Contains("failed with 3 violation(s)", baseline);
            Assert.DoesNotContain(expected, baseline, StringComparison.OrdinalIgnoreCase);
            string path = Path.Combine(root, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, source);
            (int code, string output) = await Run(root);
            Assert.Equal(1, code);
            if (rejected)
                Assert.Contains(expected, output, StringComparison.OrdinalIgnoreCase);
            else
            {
                Assert.DoesNotContain(expected, output, StringComparison.OrdinalIgnoreCase);
                Assert.Contains("failed with 3 violation(s)", output);
            }
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("../Generated", "must not overlap", true)]
    [InlineData("../Generated/target", "must not overlap", true)]
    [InlineData("../../AnotherOwner/Generated", "inside its native owner", true)]
    [InlineData("../obj/browser-wasm/Generated", "generated outputs", false)]
    public async Task CliValidatesBindingOutputOwnership(
        string targetOutput,
        string expected,
        bool rejected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoBindingArchitectureTests", Guid.NewGuid().ToString("N"));
        try
        {
            foreach (string folder in new[] { "src", "native", "build", "tools", "tests" })
                Directory.CreateDirectory(Path.Combine(root, folder));
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), """
                Microsoft Visual Studio Solution File, Format Version 12.00
                Global
                EndGlobal
                """);
            string bindings = Path.Combine(root, "backends", "Probe", "native", "Inno.Native.Probe", "Bindings");
            Directory.CreateDirectory(bindings);
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(bindings)!, "Inno.Native.Probe.csproj"),
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup></Project>");
            File.WriteAllText(Path.Combine(bindings, "bindgen.json"), JsonSerializer.Serialize(new { outputPath = "../Generated" }));
            File.WriteAllText(Path.Combine(bindings, "bindgen.browser-wasm.json"), JsonSerializer.Serialize(new { outputPath = targetOutput }));

            (int code, string output) = await Run(root);

            Assert.Equal(1, code);
            Assert.Equal(rejected, output.Contains(expected, StringComparison.Ordinal));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    private static async Task<(int, string)> Run(
        string root,
        params string[] arguments
    ) {
        DirectoryInfo? repository = new(AppContext.BaseDirectory);
        while (repository is not null && !File.Exists(Path.Combine(repository.FullName, "InnoEngine.sln")))
            repository = repository.Parent;
        Assert.NotNull(repository);
        string dotnet = Path.GetFullPath("../../../dotnet", RuntimeEnvironment.GetRuntimeDirectory());
        if (OperatingSystem.IsWindows())
            dotnet += ".exe";
        var start = new ProcessStartInfo(dotnet)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        start.ArgumentList.Add(Path.Combine(repository!.FullName, "build/cli/Inno.Build.Cli/bin/Debug/net9.0/Inno.Build.Cli.dll"));
        start.ArgumentList.Add("verify");
        start.ArgumentList.Add(root);
        foreach (string argument in arguments)
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start)!;
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        return (process.ExitCode, await output + await error);
    }
}
