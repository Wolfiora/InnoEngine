using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class ArchitectureCommandTests
{
    [Theory]
    [InlineData("backends/Interop/native/Inno.Native.Probe", false)]
    [InlineData("src/services/input/Inno.Input.Probe", true)]
    public void CliChecksExplicitInteropRuntimeOwnership(
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
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>"
                + "<ItemGroup><PackageReference Include=\"BGCS.Runtime\" Version=\"[4.1.0]\" /></ItemGroup></Project>");
            (_, string output) = ArchitectureCommandFixture.Run(root);
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
    public void CliChecksImportedMSBuildOwnership(
        string scenario,
        string expected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoEvaluatedOwnershipTests", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            string relative = scenario == "target"
                ? "platforms/WindowsX64/player/Inno.Player.WindowsX64"
                : "src/foundation/core/Inno.Core.Probe";
            string project = Path.Combine(root, relative);
            Directory.CreateDirectory(project);
            string properties = scenario == "target"
                ? "<OutputType>Exe</OutputType><InnoProductId>player</InnoProductId><InnoProductTarget>windows-x64</InnoProductTarget><InnoNativeTarget>windows-x64</InnoNativeTarget>"
                : string.Empty;
            File.WriteAllText(Path.Combine(project, Path.GetFileName(project) + ".csproj"),
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework>"
                + properties + "</PropertyGroup></Project>");
            string imported;
            if (scenario == "reference")
            {
                string upper = Path.Combine(root, "platforms/MacOSArm64/runtime/Probe");
                Directory.CreateDirectory(upper);
                File.WriteAllText(Path.Combine(upper, "Probe.csproj"),
                    "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>");
                imported = "<ItemGroup Condition=\"'$(MSBuildProjectName)' == 'Inno.Core.Probe'\"><ProjectReference Include=\"$(MSBuildThisFileDirectory)platforms/MacOSArm64/runtime/Probe/Probe.csproj\" /></ItemGroup>";
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
            (int code, string output) = ArchitectureCommandFixture.Run(root, "--dotnet", dotnet, "--project-graph", graph);
            Assert.Equal(1, code);
            Assert.Contains(expected, output, StringComparison.Ordinal);
            Assert.True(File.Exists(graph));
            Assert.False(Directory.Exists(Path.Combine(project, "bin")));
            Assert.False(Directory.Exists(Path.Combine(project, "obj")));
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
    public void CliChecksHandwrittenMultiParameterDeclarations(
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

            (_, string output) = ArchitectureCommandFixture.Run(root);

            Assert.Equal(rejected, output.Contains("place each parameter", StringComparison.Ordinal));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("Directory.Build.props", "<Project><PropertyGroup><ImplicitUsings>true</ImplicitUsings></PropertyGroup></Project>", "implicit usings", true)]
    [InlineData("build/Probe.targets", "<Project><ItemGroup><Using Include=\"System\" /></ItemGroup></Project>", "MSBuild Using", true)]
    [InlineData("tests/Probe.cs", "global using System;", "global using", true)]
    [InlineData("build/Probe.cs.txt", "global using System;", "global using", true)]
    [InlineData("tests/Probe.cs", "using System; // global using is prohibited\ninternal class Probe { }", "global using", false)]
    [InlineData("tests/Probe.cs", "internal class Probe { const string Message = \"global using System;\"; }", "global using", false)]
    public void CliRejectsCompilationWideImportsAcrossSourceAndBuildInputs(
        string relative,
        string content,
        string diagnostic,
        bool rejected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoUsingPolicyTests", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            string path = Path.Combine(root, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, content);
            (_, string output) = ArchitectureCommandFixture.Run(root);
            Assert.Equal(rejected, output.Contains(diagnostic, StringComparison.OrdinalIgnoreCase));
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
    public void CliValidatesSourceContracts(
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
            (int baselineCode, string baseline) = ArchitectureCommandFixture.Run(root);
            // This isolated source fixture intentionally has no product projects or compiled assemblies.
            Assert.Equal(1, baselineCode);
            Assert.Contains("failed with 3 violation(s)", baseline);
            Assert.DoesNotContain(expected, baseline, StringComparison.OrdinalIgnoreCase);
            string path = Path.Combine(root, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, source);
            (int code, string output) = ArchitectureCommandFixture.Run(root);
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
    [InlineData("valid", "binding Config", false)]
    [InlineData("missing-definition", "Bindings/bindings.props owner", true)]
    [InlineData("missing-import", "import its binding definition exactly once", true)]
    [InlineData("escaped-input", "literal relative children", true)]
    [InlineData("missing-input", "binding Config input 'missing.json' is missing", true)]
    [InlineData("unknown-property", "unknown or repeated property", true)]
    [InlineData("conditional", "cannot evaluate conditions", true)]
    [InlineData("platform-table", "executable build logic", true)]
    public void CliValidatesBindingDefinitionOwnership(
        string scenario,
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
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework>"
                + "<BindGenGeneratedBindings>true</BindGenGeneratedBindings></PropertyGroup>"
                + (scenario == "missing-import" ? string.Empty : "<Import Project=\"Bindings/bindings.props\" />")
                + "</Project>");
            File.WriteAllText(Path.Combine(bindings, "bindgen.json"), "{}");
            string definition = """
                <Project>
                  <PropertyGroup>
                    <InnoNativeComponentId>probe</InnoNativeComponentId>
                    <InnoBindingConfig>bindgen.json</InnoBindingConfig>
                  </PropertyGroup>
                </Project>
                """;
            definition = scenario switch
            {
                "escaped-input" => definition.Replace("bindgen.json", "../outside.json", StringComparison.Ordinal),
                "missing-input" => definition.Replace("bindgen.json", "missing.json", StringComparison.Ordinal),
                "unknown-property" => definition.Replace("</PropertyGroup>", "<InnoBindingBridgeConfig>bridge.json</InnoBindingBridgeConfig></PropertyGroup>", StringComparison.Ordinal),
                "conditional" => definition.Replace("<PropertyGroup>", "<PropertyGroup Condition=\"'$(Target)' != ''\">", StringComparison.Ordinal),
                "platform-table" => definition.Replace("</Project>", "<ItemGroup /></Project>", StringComparison.Ordinal),
                _ => definition
            };
            if (scenario != "missing-definition")
                File.WriteAllText(Path.Combine(bindings, "bindings.props"), definition);

            (int code, string output) = ArchitectureCommandFixture.Run(root);

            Assert.Equal(1, code);
            Assert.Equal(rejected, output.Contains(expected, StringComparison.Ordinal));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

}
