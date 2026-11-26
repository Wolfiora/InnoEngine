using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Inno.Testing.Source;

namespace Inno.Build.Toolchains.Integration.Tests;

public sealed class BackendSdkConsumptionTests : IDisposable
{
    private readonly string m_engine = TestRepository.GetRoot(typeof(BackendSdkConsumptionTests).Assembly);
    private readonly string m_work;

    private readonly string m_sdk = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
                                    ?? ToolchainEnvironment.ResolveExecutable("dotnet");

    private readonly Dictionary<string, string?> m_environment = new(StringComparer.Ordinal)
    {
        ["InnoBackendPackageDirectory"] = null,
        ["InnoBackendPrebuiltRoot"] = null,
        ["InnoBackendHostTarget"] = null,
        ["InnoNativeDependencyManifest"] = null,
        ["MSBUILDDISABLENODEREUSE"] = "1",
        ["DOTNET_CLI_USE_MSBUILD_SERVER"] = "0",
        ["UseSharedCompilation"] = "false"
    };

    public BackendSdkConsumptionTests()
    {
        m_work = Directory.CreateDirectory(
            Path.Combine(m_engine, "artifacts/validation/backend-probes", Guid.NewGuid().ToString("N")[..12])
        ).FullName;
    }

    [Fact]
    public async Task ACompiledReferenceAndItsRuntimeDependenciesAreRequiredForConsumption()
    {
        string project = Path.Combine(m_engine, "backends/Sdl3/native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj");
        string source = Path.Combine(m_work, "source");
        string stage = Path.Combine(m_work, "staging");
        string outputs = Path.Combine(m_work, "consumer");
        await Run(
            "build",
            project,
            "-c",
            "Debug",
            "--disable-build-servers",
            "--artifacts-path",
            source,
            "-p:InnoBackendExportRoot=" + stage,
            "-p:InnoBackendExportArtifactsPath=" + source,
            "-m:2",
            "--nologo"
        );
        string[] receipts = Directory.GetFiles(Path.Combine(stage, "receipts"), "files.txt", SearchOption.AllDirectories);
        Assert.Equal(2, receipts.Length);
        string fixture = Path.Combine(m_work, "Probe.csproj");
        new XElement(
            "Project",
            new XAttribute("Sdk", "Microsoft.NET.Sdk"),
            new XElement(
                "PropertyGroup",
                new XElement("TargetFramework", "net10.0"),
                new XElement("OutputType", "Exe"),
                new XElement("EnableDefaultCompileItems", "false")
            ),
            new XElement(
                "ItemGroup",
                new XElement("Compile", new XAttribute("Include", "Program.cs")),
                new XElement("ProjectReference", new XAttribute("Include", project))
            )
        ).Save(fixture);
        File.WriteAllText(Path.Combine(m_work, "Program.cs"), """
            using System;
            using System.Reflection;
            using Inno.Native.Sdl3;
            var backend = typeof(SDL).Assembly;
            foreach (var dependency in backend.GetReferencedAssemblies()) { Assembly.Load(dependency); }
            Console.WriteLine(backend.GetName().Name);
            """);
        string[] build =
        [
            "build",
            fixture,
            "--disable-build-servers",
            "--artifacts-path",
            outputs,
            "-p:InnoBackendPrebuiltRoot=" + stage,
            "-m:2",
            "--nologo"
        ];
        await Run(build);
        Assert.Equal("Inno.Native.Sdl3", (await Run(Path.Combine(outputs, "bin/Probe/debug/Probe.dll"))).Trim());
        string receipt = Assert.Single(
            receipts.SelectMany(File.ReadAllLines).Where(line => line.Contains("/ref/Inno.Native.Sdl3.dll|", StringComparison.Ordinal))
        );
        string reference = receipt.Split('|')[2];
        Assert.StartsWith("managed/", reference);
        File.Delete(Path.Combine(stage, reference));
        Exception? failure = await Record.ExceptionAsync(() => Run(build));
        Assert.NotNull(failure);
        Assert.Contains("Missing backend prebuilt", failure.Message);
    }

    [Fact]
    public async Task StaticBindingVariantsSurviveSkippingTheCompilerAndTrackLinkageAndContent()
    {
        string probe = Path.Combine(m_work, "compiler.targets");
        File.WriteAllText(probe, """
            <Project>
              <Target Name="RunBindingCompiler" Condition="'$(SourceCompilation)' == 'true'">
                <PropertyGroup><DefineConstants>$(DefineConstants);INNO_STATIC_NATIVE</DefineConstants></PropertyGroup>
              </Target>
              <Target Name="ReadBackendVariant" DependsOnTargets="PrepareForBuild;RunBindingCompiler;GetInnoBackendCompilerOutputs" />
            </Project>
            """);
        string source = await VariantAsync("true", "Static", 'a');
        string consumer = await VariantAsync("false", "Static", 'a');
        Assert.Equal(source, consumer);
        Assert.NotEqual(consumer, await VariantAsync("false", "Shared", 'a'));
        Assert.NotEqual(consumer, await VariantAsync("false", "Static", 'b'));

        async Task<string> VariantAsync(
            string compile,
            string kind,
            char fingerprint
        ) {
            string output = (await Run(
                "msbuild",
                Path.Combine(m_engine, "backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj"),
                "-nologo",
                "-verbosity:quiet",
                "-target:ReadBackendVariant",
                "-getProperty:_InnoBackendVariant",
                "-p:CustomAfterMicrosoftCommonTargets=" + probe,
                "-p:ArtifactsPath=" + Path.Combine(m_work, "sdk"),
                "-p:InnoBackendExportRoot=" + Path.Combine(m_work, "export"),
                "-p:SourceCompilation=" + compile,
                "-p:InnoNativeLibraryKind=" + kind,
                "-p:BindGenExpectedFingerprint=" + new string(fingerprint, 64)
            )).Trim();
            Assert.Matches(new Regex("/[a-f0-9]{64}$"), output);
            return output;
        }
    }

    public void Dispose() => Directory.Delete(m_work, recursive: true);

    private Task<string> Run(params string[] arguments)
        => ToolchainEnvironment.CaptureOutputAsync(m_sdk, arguments, m_engine, m_environment);
}
