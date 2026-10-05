using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

public sealed class ArchitectureSymbolTests
{
    [Theory]
    [InlineData("src/services/platform/Inno.Platform", true)]
    [InlineData("src/adapters/platform/Inno.Adapter.Platform", false)]
    public async Task NativeSurfaceAbiRemainsInsideAdapterBoundary(
        string relative,
        bool rejected
    ) {
        using var fixture = new SymbolFixture();
        fixture.Compile(relative, Path.GetFileName(relative), "internal class Probe { }");
        fixture.WriteSource(relative, "internal class Probe { PlatformNativeHandles value; }");
        string output = await fixture.Run();
        Assert.Equal(rejected, output.Contains("native surface ABI belongs to platform adapters", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("implementation", true)]
    [InlineData("override", true)]
    [InlineData("chain", true)]
    [InlineData("standalone", false)]
    [InlineData("wrong-cref", false)]
    [InlineData("missing-summary", false)]
    [InlineData("missing-parameter", false)]
    [InlineData("missing-returns", false)]
    public async Task InheritedDocumentationRequiresACompleteImplementedContract(
        string scenario,
        bool accepted
    ) {
        using var fixture = new SymbolFixture();
        string summary = scenario == "missing-summary" ? "" : """
            /// <summary>
            /// Combines two values using the implementation's operation.
            /// </summary>
            """;
        string right = scenario == "missing-parameter" ? "" : """
            /// <param name="right">
            /// The second value supplied to the operation.
            /// </param>
            """;
        string returns = scenario == "missing-returns" ? "" : """
            /// <returns>
            /// The combined value produced by the operation.
            /// </returns>
            """;
        string contract = summary + "\n" + """
            /// <param name="left">
            /// The first value supplied to the operation.
            /// </param>
            """ + "\n" + right + "\n" + returns + "\n";
        string source;
        if (scenario is "override" or "chain")
        {
            source = "namespace Inno.Editor.Probe; public class Base {\n" + contract
                + "public virtual int Combine(int left, int right) => left + right; }\n";
            if (scenario == "chain")
            {
                source += "public class Middle : Base {\n/// <inheritdoc />\n"
                    + "public override int Combine(int left, int right) => base.Combine(left, right); }\n";
            }
            source += "public class Probe : " + (scenario == "chain" ? "Middle" : "Base")
                + " {\n/// <inheritdoc />\npublic override int Combine(int left, int right) => left + right; }";
        }
        else
        {
            source = "namespace Inno.Editor.Probe; public interface IContract {\n" + contract
                + "int Combine(int left, int right); }\npublic class Probe"
                + (scenario == "standalone" ? "" : " : IContract") + " {\n"
                + (scenario == "wrong-cref" ? "/// <inheritdoc cref=\"string.ToString()\" />\n" : "/// <inheritdoc />\n")
                + "public int Combine(int left, int right) => left + right; }";
        }
        string relative = "src/composition/editor/framework/Inno.Editor.Probe";
        fixture.Compile(relative, "Inno.Editor.Probe", source);
        fixture.WriteSource(relative, source);

        string output = await fixture.Run();
        Assert.Equal(!accepted, output.Contains(
            "inheritdoc requires a documented overridden or implemented contract", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("public enum InputBackend { BuiltIn }", true)]
    [InlineData("public readonly record struct InputBackendId(string value);", false)]
    public async Task ReplaceableBackendContractsUseOpenIdentifiers(
        string declaration,
        bool rejected
    ) {
        using var fixture = new SymbolFixture();
        fixture.Compile("src/adapters/input/Inno.Adapter.Input", "Inno.Adapter.Input",
            "namespace Inno.Adapter.Input; " + declaration);
        string output = await fixture.Run();
        Assert.Equal(rejected, output.Contains("replaceable backend selections require open IDs", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("public class Probe { public Raw value; }", true)]
    [InlineData("public class Probe { public Raw value { get; set; } }", true)]
    [InlineData("public class Probe { protected Raw Read() => default; }", true)]
    [InlineData("public class Probe { protected internal void Write(Raw value) { } }", true)]
    [InlineData("public class Probe { public event Action<Raw> Changed { add { } remove { } } }", true)]
    [InlineData("public delegate Raw Probe(Raw input);", true)]
    [InlineData("public class Probe : Inno.Native.MiniAudio.MaBase { }", true)]
    [InlineData("public class Probe : Inno.Native.MiniAudio.IMaBackend { }", true)]
    [InlineData("public class Probe<T> where T : Inno.Native.MiniAudio.IMaBackend { }", true)]
    [InlineData("public class Probe { public void Read<T>() where T : Inno.Native.MiniAudio.IMaBackend { } }", true)]
    [InlineData("public class Probe { protected class Nested { public Raw value; } }", true)]
    [InlineData("public class Probe { public Raw[] Read() => null; }", true)]
    [InlineData("public class Probe { public Task<IReadOnlyList<Raw[]>> Read() => null; }", true)]
    [InlineData("public class Probe { public (int count, Raw value) Read() => default; }", true)]
    [InlineData("public unsafe class Probe { public Raw* pointer; }", true)]
    [InlineData("public unsafe class Probe { public delegate* unmanaged<Raw, int> callback; }", true)]
    [InlineData("public unsafe class Probe { public delegate* unmanaged<int, Raw> callback; }", true)]
    [InlineData("public class Outer<T> { public class Inner { } } public class Probe { public Outer<Raw>.Inner value; }", true)]
    [InlineData("public class Probe { private Raw value; }", false)]
    [InlineData("public class Probe { private protected Raw Read() => default; }", false)]
    [InlineData("internal class Probe { public Raw value; }", false)]
    [InlineData("public class Probe { private class Nested { public Raw value; } }", false)]
    [InlineData("public class Probe { public int Read() => 0; }", false)]
    public async Task SymbolAuditFollowsPublicShapesAndRespectsEffectiveAccessibility(string declaration, bool rejected)
    {
        using var fixture = new SymbolFixture();
        string native = fixture.Compile("native/Inno.Native.MiniAudio", "Inno.Native.MiniAudio", """
            namespace Inno.Native.MiniAudio;
            public struct MaEngine { public int value; }
            public class MaBase { }
            public interface IMaBackend { }
            """);
        fixture.Compile("src/services/audio/Inno.Audio", "Inno.Audio", """
            using System;
            using System.Collections.Generic;
            using System.Threading.Tasks;
            using Raw = Inno.Native.MiniAudio.MaEngine;
            namespace Inno.Audio;
            """ + declaration, native);

        string output = await fixture.Run();
        Assert.Equal(rejected, output.Contains("exposes native symbol", StringComparison.Ordinal));
        Assert.DoesNotContain("output is missing", output, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("Inno.Native.Bgfx")]
    [InlineData("Inno.Native.Sdl3")]
    [InlineData("Inno.Native.MiniAudio")]
    public async Task NativePolicyUsesAssemblyIdentityRatherThanSourceTypeNames(string assemblyName)
    {
        using var fixture = new SymbolFixture();
        string native = fixture.Compile("native/" + assemblyName, assemblyName,
            "namespace Renamed; public struct NeutralLookingName { public int value; }");
        fixture.Compile("src/services/audio/Inno.Audio", "Inno.Audio",
            "namespace Inno.Audio; public class Probe { public Renamed.NeutralLookingName value; }", native);
        Assert.Contains("exposes native symbol Renamed.NeutralLookingName", await fixture.Run(), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("Inno.Shell", "Inno.Adapter.Audio.MiniAudio", true)]
    [InlineData("Inno.Player", "Inno.Adapter.Rendering.Bgfx", true)]
    [InlineData("Inno.Editor.Application", "Inno.Adapter.Platform.Sdl3", true)]
    [InlineData("Inno.Shell", "Inno.Adapter.Audio", false)]
    public async Task CompositionOnlyExposesNeutralAdapterContracts(string owner, string adapter, bool rejected)
    {
        using var fixture = new SymbolFixture();
        string implementation = fixture.Compile("src/adapters/audio/" + adapter, adapter,
            "namespace Backend; public class Device { }");
        fixture.Compile("src/composition/" + owner, owner,
            "namespace Product; public class Probe { public Backend.Device device; }", implementation);
        string output = await fixture.Run();
        Assert.Equal(rejected, output.Contains("exposes concrete adapter", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("internal class Probe { public Value value; }", true, false)]
    [InlineData("public class Probe { public Value value; }", true, true)]
    [InlineData("public class Probe { protected Value Read() => null; }", true, true)]
    [InlineData("public class Probe { public Value value; }", false, false)]
    public async Task EditorPublicReferencePolicyFollowsCompiledVisibility(string declaration, bool privateReference, bool rejected)
    {
        using var fixture = new SymbolFixture();
        string dependency = fixture.Compile("src/foundation/core/Inno.Core.Values", "Inno.Core.Values",
            "namespace Values; public class Value { }");
        fixture.Compile("src/composition/editor/framework/Inno.Editor.Probe", "Inno.Editor.Probe",
            "using Values; namespace Inno.Editor.Probe; " + declaration, dependency);
        fixture.AddReference("src/composition/editor/framework/Inno.Editor.Probe/Inno.Editor.Probe.csproj",
            "src/foundation/core/Inno.Core.Values/Inno.Core.Values.csproj", privateReference);
        string output = await fixture.Run();
        Assert.Equal(rejected, output.Contains("declare a direct public ProjectReference", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("src/services/audio/Inno.Audio", true)]
    [InlineData("src/adapters/platform/Inno.Adapter.Platform.Sdl3", false)]
    [InlineData("src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Sdl3", false)]
    [InlineData("build/toolchains/Inno.Build.Toolchains.Sdl3", false)]
    public async Task SdlReferencesAreLimitedToTheActualPlatformAndPresentationOwners(string relative, bool rejected)
    {
        using var fixture = new SymbolFixture();
        string native = fixture.Compile("native/Inno.Native.Sdl3", "Inno.Native.Sdl3", "public struct Raw { }");
        string projectName = Path.GetFileName(relative);
        fixture.Compile(relative, projectName, "internal class Probe { private Raw value; }", native);
        fixture.AddReference(relative + "/" + projectName + ".csproj", "native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj", true);
        string output = await fixture.Run();
        Assert.Equal(rejected, output.Contains("SDL3 native code is restricted", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task EditorImplementationReferencesRequireCompilePrivacy(
        bool privateReference,
        bool rejected
    ) {
        using var fixture = new SymbolFixture();
        string dependency = fixture.Compile("src/foundation/core/Inno.Core.Values", "Inno.Core.Values",
            "namespace Values; public class Value { }");
        string relative = "src/composition/editor/framework/Inno.Editor.Probe";
        fixture.Compile(relative, "Inno.Editor.Probe",
            "using Values; namespace Inno.Editor.Probe; public class Probe { private Value value; }", dependency);
        fixture.AddReference(relative + "/Inno.Editor.Probe.csproj",
            "src/foundation/core/Inno.Core.Values/Inno.Core.Values.csproj", privateReference);
        string output = await fixture.Run();
        Assert.Equal(rejected, output.Contains("is an implementation dependency", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(2, true)]
    public async Task EditorReferenceGroupsKeepImplementationBeforePublicApi(
        int layout,
        bool rejected
    ) {
        using var fixture = new SymbolFixture();
        string hidden = fixture.Compile("src/foundation/core/Inno.Core.Hidden", "Inno.Core.Hidden",
            "namespace Hidden; public class Value { }");
        string visible = fixture.Compile("src/foundation/core/Inno.Core.Visible", "Inno.Core.Visible",
            "namespace Visible; public class Value { }");
        string relative = "src/composition/editor/framework/Inno.Editor.Probe";
        fixture.Compile(relative, "Inno.Editor.Probe",
            "namespace Inno.Editor.Probe; public class Probe { private Hidden.Value hidden; public Visible.Value visible; }",
            hidden, visible);
        string owner = relative + "/Inno.Editor.Probe.csproj";
        if (layout == 1)
        {
            fixture.AddReference(owner, "src/foundation/core/Inno.Core.Visible/Inno.Core.Visible.csproj", false);
            fixture.AddReference(owner, "src/foundation/core/Inno.Core.Hidden/Inno.Core.Hidden.csproj", true);
        }
        else
        {
            fixture.AddReference(owner, "src/foundation/core/Inno.Core.Hidden/Inno.Core.Hidden.csproj", true);
            fixture.AddReference(owner, "src/foundation/core/Inno.Core.Visible/Inno.Core.Visible.csproj", false);
            if (layout == 2)
            {
                string path = fixture.PathFor(owner);
                XDocument document = XDocument.Load(path);
                XElement[] groups = document.Root!.Elements("ItemGroup").ToArray();
                groups[0].Add(groups[1].Elements().ToArray());
                groups[1].Remove();
                document.Save(path);
            }
        }
        string output = await fixture.Run();
        Assert.Equal(rejected, output.Contains("one implementation group followed by one public API group", StringComparison.Ordinal));
    }

    private sealed class SymbolFixture : IDisposable
    {
        private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoArchitectureSymbolTests", Guid.NewGuid().ToString("N"));

        internal SymbolFixture()
        {
            foreach (string folder in new[] { "src", "native", "build", "tools", "tests" })
                Directory.CreateDirectory(Path.Combine(m_root, folder));
            File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), """
                Microsoft Visual Studio Solution File, Format Version 12.00
                Global
                EndGlobal
                """);
        }

        internal string Compile(string relative, string assemblyName, string source, params string[] dependencies)
        {
            string directory = Path.Combine(m_root, relative);
            string output = Path.Combine(directory, "bin", "Debug", "net9.0", assemblyName + ".dll");
            Directory.CreateDirectory(Path.GetDirectoryName(output)!);
            new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup", new XElement("TargetFramework", "net9.0"))))
                .Save(Path.Combine(directory, assemblyName + ".csproj"));
            string[] platform = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
            MetadataReference[] references = platform.Concat(dependencies).Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(path => MetadataReference.CreateFromFile(path)).ToArray();
            CSharpCompilation compilation = CSharpCompilation.Create(assemblyName,
                [CSharpSyntaxTree.ParseText(source)], references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
            var result = compilation.Emit(output);
            Assert.True(result.Success, string.Join(Environment.NewLine, result.Diagnostics));
            return output;
        }

        internal void AddReference(string owner, string target, bool privateReference)
        {
            string path = Path.Combine(m_root, owner);
            XDocument document = XDocument.Load(path);
            var reference = new XElement("ProjectReference", new XAttribute("Include",
                Path.GetRelativePath(Path.GetDirectoryName(path)!, Path.Combine(m_root, target))));
            if (privateReference)
                reference.Add(new XAttribute("PrivateAssets", "compile"));
            document.Root!.Add(new XElement("ItemGroup", reference));
            document.Save(path);
        }

        internal void WriteSource(
            string relative,
            string source
        ) => File.WriteAllText(Path.Combine(m_root, relative, "Probe.cs"), source);

        internal string PathFor(string relative) => Path.Combine(m_root, relative);

        internal async Task<string> Run()
        {
            DirectoryInfo? repository = new(AppContext.BaseDirectory);
            while (repository is not null && !File.Exists(Path.Combine(repository.FullName, "InnoEngine.sln")))
                repository = repository.Parent;
            Assert.NotNull(repository);
            string dotnet = Path.GetFullPath("../../../dotnet", RuntimeEnvironment.GetRuntimeDirectory());
            if (OperatingSystem.IsWindows()) dotnet += ".exe";
            var start = new ProcessStartInfo(dotnet)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            start.ArgumentList.Add(Path.Combine(repository!.FullName,
                "build/cli/Inno.Build.Cli/bin/Debug/net9.0/Inno.Build.Cli.dll"));
            start.ArgumentList.Add("verify");
            start.ArgumentList.Add(m_root);
            using Process process = Process.Start(start)!;
            Task<string> output = process.StandardOutput.ReadToEndAsync();
            Task<string> error = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            // The fixture deliberately omits product composition projects; only the audited boundary is asserted.
            Assert.Equal(1, process.ExitCode);
            return await output + await error;
        }

        public void Dispose()
        {
            if (Directory.Exists(m_root))
                Directory.Delete(m_root, recursive: true);
        }
    }
}
