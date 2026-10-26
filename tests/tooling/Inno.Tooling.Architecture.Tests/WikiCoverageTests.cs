using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class WikiCoverageTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoWikiTests", Guid.NewGuid().ToString("N"));

    public WikiCoverageTests()
    {
        Directory.CreateDirectory(m_root);
        Write("InnoEngine.sln", "Microsoft Visual Studio Solution File, Format Version 12.00");
        Write("src/foundation/core/Inno.Core.Probe/Inno.Core.Probe.csproj", """
            <Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>
            """);
        Write("src/foundation/core/Inno.Core.Probe/Probe.cs", """
            namespace Inno.Core.Probe;
            /// <summary>Provides a nullable probe value.</summary>
            public static class Probe
            {
                /// <summary>Reads a value without taking ownership.</summary>
                /// <returns>The current value, or null when unavailable.</returns>
                public static string? Read() => null;
            }
            """);
        Write("docs/core/README.md", "[Probe](Inno.Core.Probe.md)");
        Write("docs/core/Inno.Core.Probe.md", "# Probe\n");
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    [Theory]
    [InlineData("Inno.Canvas", true)]
    [InlineData("InnoEngine.Rendering2D", true)]
    [InlineData("Serialization/Rendering2D", true)]
    [InlineData("innoengine.canvas/README.md", true)]
    [InlineData("Browser Canvas and .iplugin use Inno.Plugins.", false)]
    [InlineData("entry/Rebuild/Program.cs", true)]
    [InlineData("Inno.Build.Cli engine", true)]
    [InlineData("InnoEngine.Build.dll rebuild", true)]
    [InlineData("dotnet build/cli/Inno.Build.Cli/bin/Release/net10.0/Inno.Build.Cli.dll game", true)]
    [InlineData("Inno.Build.Cli.dll contains the public BuildApplication contract.", false)]
    [InlineData("Rebuild the registry after generation activation.", false)]
    public void ContentBoundariesDistinguishConsumersFromEngineConcepts(
        string content,
        bool rejected
    ) {
        Write("docs/core/Inno.Core.Probe.md", content);
        Assert.Equal(rejected, Run().Contains("external consumer reference", StringComparison.Ordinal));
    }

    [Fact]
    public void ProjectPagesMustBeUniqueAndIndexed()
    {
        File.Delete(Path.Combine(m_root, "docs/core/Inno.Core.Probe.md"));
        Assert.Contains("requires one authoritative page; found 0", Run(), StringComparison.Ordinal);
        Write("docs/core/Inno.Core.Probe.md", "# Probe");
        Write("docs/duplicate/Inno.Core.Probe.md", "# Probe");
        Assert.Contains("requires one authoritative page; found 2", Run(), StringComparison.Ordinal);
        File.Delete(Path.Combine(m_root, "docs/duplicate/Inno.Core.Probe.md"));
        Write("docs/core/README.md", "# Core");
        Assert.Contains("absent from its category index", Run(), StringComparison.Ordinal);
    }

    [Fact]
    public void LinksValidatePathsAndAnchorsOutsideCodeFences()
    {
        Write("docs/core/Inno.Core.Probe.md", """
            # Probe
            ## 生命周期
            [Existing](#生命周期)
            [Missing](#missing)
            [Absent](absent.md)
            ```text
            [Example syntax](example-only.md)
            ```
            """);
        string diagnostics = Run();
        Assert.Contains("missing anchor '#missing'", diagnostics, StringComparison.Ordinal);
        Assert.Contains("missing path 'absent.md'", diagnostics, StringComparison.Ordinal);
        Assert.DoesNotContain("example-only.md", diagnostics, StringComparison.Ordinal);
        Assert.DoesNotContain("missing anchor '#生命周期'", diagnostics, StringComparison.Ordinal);
    }

    [Fact]
    public void SignatureMappingDetectsNullableChangesAndDeletedOverloads()
    {
        string inventory = Path.Combine(m_root, "api.json");
        Assert.Contains("no current signature mapping", Run(inventory), StringComparison.Ordinal);
        using JsonDocument document = JsonDocument.Parse(File.ReadAllText(inventory));
        string markers = string.Join('\n', document.RootElement.EnumerateArray().Select(entry =>
            $"<!-- api: {entry.GetProperty("id").GetString()} {entry.GetProperty("signatureHash").GetString()} -->"));
        Write("docs/core/Inno.Core.Probe.md", markers);
        Assert.DoesNotContain("no current signature mapping", Run(), StringComparison.Ordinal);
        string source = Path.Combine(m_root, "src/foundation/core/Inno.Core.Probe/Probe.cs");
        File.WriteAllText(source, File.ReadAllText(source).Replace("string? Read() => null", "string Read() => string.Empty", StringComparison.Ordinal));
        Assert.Contains("no current signature mapping for 'M:Inno.Core.Probe.Probe.Read'", Run(), StringComparison.Ordinal);
        Write("docs/core/Inno.Core.Probe.md", markers + "\n<!-- api: M:Inno.Core.Probe.Probe.Read(System.Int32) "
            + Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes("missing"))) + " -->");
        Assert.Contains("documents deleted symbol", Run(), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("int value = 1;", false)]
    [InlineData("int value = \"invalid\";", true)]
    [InlineData("AbsentType.Run();", true)]
    public void ExecutableExamplesAreCompiledAgainstProjectReferences(
        string example,
        bool rejected
    ) {
        Write("docs/core/Inno.Core.Probe.md", "# Probe\n\n```csharp\n" + example + "\n```\n");
        Assert.Equal(rejected, Run().Contains("example 1 at line", StringComparison.Ordinal));
    }

    [Fact]
    public async Task ExplicitWikiTargetRecordsForeignProductsWithoutSubstitutingTheirReferences()
    {
        string project = Path.Combine(m_root, "src/foundation/core/Inno.Core.Probe/Inno.Core.Probe.csproj");
        File.WriteAllText(project, File.ReadAllText(project).Replace("</PropertyGroup>",
            "<InnoProductTarget>fixture-foreign</InnoProductTarget></PropertyGroup>", StringComparison.Ordinal));
        Write("docs/core/Inno.Core.Probe.md", "# Probe\n\n```csharp\nAbsentType.Run();\n```\n");
        string host = Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "..", "..", "..",
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"));
        string examples = Path.Combine(m_root, "scoped-examples.json");
        string excluded = Run(Path.Combine(m_root, "scoped-api.json"), "--dotnet", host,
            "--wiki-target", "fixture-selected", "--wiki-examples", examples);
        Assert.Contains("outside the selected target", excluded, StringComparison.Ordinal);
        using JsonDocument skipped = JsonDocument.Parse(File.ReadAllText(examples));
        JsonElement example = skipped.RootElement.EnumerateArray().Single();
        Assert.Equal("foreign-product", example.GetProperty("referenceMode").GetString());
        Assert.Contains("Not evaluated", example.GetProperty("notEvaluatedReason").GetString(), StringComparison.Ordinal);
        Assert.Equal(0, example.GetProperty("errors").GetArrayLength());

        var start = new ProcessStartInfo(host)
        {
            UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true
        };
        foreach (string argument in new[] { "build", project, "--disable-build-servers", "-m:1", "-nr:false", "--nologo", "-v:quiet" })
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start)!;
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        try
        {
            await process.WaitForExitAsync(timeout.Token);
            Assert.True(process.ExitCode == 0, await output + await error);
        }
        finally
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync();
            await Task.WhenAll(output, error);
        }
        string selected = Run(null, "--dotnet", host, "--wiki-target", "fixture-foreign", "--wiki-examples", examples);
        Assert.Contains("example 1 at line", selected, StringComparison.Ordinal);
        Assert.DoesNotContain("outside the selected target", selected, StringComparison.Ordinal);
    }

    [Fact]
    public void OwnedApiFragmentsContributeExactSignaturesWithoutDuplicatingProjectPages()
    {
        string inventory = Path.Combine(m_root, "api.json");
        _ = Run(inventory);
        using JsonDocument document = JsonDocument.Parse(File.ReadAllText(inventory));
        string markers = string.Join('\n', document.RootElement.EnumerateArray().Select(entry =>
            $"<!-- api: {entry.GetProperty("id").GetString()} {entry.GetProperty("signatureHash").GetString()} -->"));
        Write("docs/core/Inno.Core.Probe.Api/ABI-01.md", markers);
        Write("docs/core/Inno.Core.Probe.md", "<!-- api-include: Inno.Core.Probe.Api/ABI-01.md -->\r\n");
        string output = Run();
        Assert.DoesNotContain("no current signature mapping", output, StringComparison.Ordinal);
        Assert.DoesNotContain("requires one authoritative page", output, StringComparison.Ordinal);
        Write("docs/core/Inno.Core.Probe.Api/ABI-01.md", markers
            + "\n<!-- api-include: ../Inno.Core.Probe.md -->");
        Assert.Contains("unique Markdown page", Assert.Throws<InvalidDataException>(() => Run()).Message, StringComparison.Ordinal);
        Write("docs/core/Inno.Core.Probe.md", "<!-- api-include: ../../outside.md -->");
        Assert.Contains("within the current Wiki", Assert.Throws<InvalidDataException>(() => Run()).Message, StringComparison.Ordinal);
    }

    [Fact]
    public void TypeModifiersAndRefReturnsParticipateInSignatureIdentity()
    {
        const string sourcePath = "src/foundation/core/Inno.Core.Probe/Probe.cs";
        const string source = """
            namespace Inno.Core.Probe;
            public sealed class Probe
            {
                private int m_value;
                public ref int Value => ref m_value;
                public ref readonly int Read() => ref m_value;
            }
            public readonly ref struct BorrowedValue;
            public static class Factory;
            """;
        Write(sourcePath, source);
        string inventory = Path.Combine(m_root, "api.json");
        _ = Run(inventory);
        using JsonDocument document = JsonDocument.Parse(File.ReadAllText(inventory));
        JsonElement[] declarations = document.RootElement.EnumerateArray().ToArray();
        string Signature(string id) => declarations.Single(entry => entry.GetProperty("id").GetString() == id)
            .GetProperty("signature").GetString()!;
        Assert.Contains("public sealed class", Signature("T:Inno.Core.Probe.Probe"), StringComparison.Ordinal);
        Assert.Contains("public readonly ref struct", Signature("T:Inno.Core.Probe.BorrowedValue"), StringComparison.Ordinal);
        Assert.Contains("public static class", Signature("T:Inno.Core.Probe.Factory"), StringComparison.Ordinal);
        Assert.Contains("ref int", Signature("P:Inno.Core.Probe.Probe.Value"), StringComparison.Ordinal);
        Assert.Contains("ref readonly int", Signature("M:Inno.Core.Probe.Probe.Read"), StringComparison.Ordinal);
        Write("docs/core/Inno.Core.Probe.md", string.Join('\n', declarations.Select(entry =>
            $"<!-- api: {entry.GetProperty("id").GetString()} {entry.GetProperty("signatureHash").GetString()} -->")));
        Assert.DoesNotContain("no current signature mapping", Run(), StringComparison.Ordinal);
        Write(sourcePath, source.Replace("sealed class Probe", "class Probe", StringComparison.Ordinal)
            .Replace("ref readonly int Read()", "ref int Read()", StringComparison.Ordinal));
        string changed = Run();
        Assert.Contains("no current signature mapping for 'T:Inno.Core.Probe.Probe'", changed, StringComparison.Ordinal);
        Assert.Contains("no current signature mapping for 'M:Inno.Core.Probe.Probe.Read'", changed, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("docs/architecture/GUIDE.md", "int value = 1;", false)]
    [InlineData("docs/architecture/GUIDE.md", "int value = missing;", true)]
    [InlineData("docs/core/README.md", "int value = missing;", true)]
    public void ExamplesOutsideProjectPagesAreAlsoCompiled(
        string page,
        string example,
        bool rejected
    ) {
        Write(page, "```csharp\n" + example + "\n```\n");
        string inventory = Path.Combine(m_root, "examples.json");
        Assert.Equal(rejected, Run(null, "--wiki-examples", inventory).Contains("example 1 at line", StringComparison.Ordinal));
        using JsonDocument results = JsonDocument.Parse(File.ReadAllText(inventory));
        Assert.Equal(page, results.RootElement[0].GetProperty("page").GetString());
        Assert.Equal("framework", results.RootElement[0].GetProperty("referenceMode").GetString());
    }

    [Fact]
    public void InventoryIncludesPrimaryRecordMembersAndInheritedContracts()
    {
        Write("src/foundation/core/Inno.Core.Probe/Probe.cs", """
            namespace Inno.Core.Probe;
            /// <summary>Reads a borrowed value.</summary>
            public interface IReader
            {
                /// <summary>Returns the current borrowed value.</summary>
                /// <returns>The value, or null when none is selected.</returns>
                string? Read();
            }
            /// <summary>Captures one immutable value.</summary>
            /// <param name="Value">The captured optional value.</param>
            public sealed record Probe(string? Value) : IReader
            {
                /// <inheritdoc />
                public string? Read() => Value;
            }
            /// <summary>Transforms one optional value.</summary>
            /// <param name="value">The borrowed input.</param>
            /// <returns>The transformed value.</returns>
            public delegate string Transform(string? value);
            """);
        string inventory = Path.Combine(m_root, "api.json");
        _ = Run(inventory);
        using JsonDocument document = JsonDocument.Parse(File.ReadAllText(inventory));
        JsonElement[] members = document.RootElement.EnumerateArray().ToArray();
        Assert.Contains(members, member => member.GetProperty("id").GetString() == "P:Inno.Core.Probe.Probe.Value");
        Assert.Contains(members, member => member.GetProperty("id").GetString() == "M:Inno.Core.Probe.Probe.#ctor(System.String)");
        Assert.Contains("IReader", members.Single(member => member.GetProperty("id").GetString() == "T:Inno.Core.Probe.Probe")
            .GetProperty("signature").GetString(), StringComparison.Ordinal);
        Assert.Contains("borrowed value", members.Single(member => member.GetProperty("id").GetString() == "M:Inno.Core.Probe.Probe.Read")
            .GetProperty("documentation").GetString(), StringComparison.Ordinal);
        Assert.Contains("string? value", members.Single(member => member.GetProperty("id").GetString() == "T:Inno.Core.Probe.Transform")
            .GetProperty("signature").GetString(), StringComparison.Ordinal);
    }

    [Fact]
    public void CompileOnlyPackageReferencesResolveWithoutRuntimeDeployment()
    {
        Write("src/foundation/core/Inno.Core.Probe/Probe.cs", """
            namespace Inno.Core.Probe;
            /// <summary>Exposes a contract supplied by a compile-only package.</summary>
            public abstract class Probe : PackageContract.BaseContract;
            """);
        Assert.Contains("cannot resolve the complete public signature", Run(), StringComparison.Ordinal);
        string packageRoot = Path.Combine(m_root, "packages");
        string reference = Path.Combine(packageRoot, "contract/1.0/ref/net10.0/PackageContract.dll");
        Directory.CreateDirectory(Path.GetDirectoryName(reference)!);
        var compilation = CSharpCompilation.Create("PackageContract",
            [CSharpSyntaxTree.ParseText("namespace PackageContract { public abstract class BaseContract {} }")],
            ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
                .Select(static path => MetadataReference.CreateFromFile(path)),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        Assert.True(compilation.Emit(reference).Success);
        Write("src/foundation/core/Inno.Core.Probe/obj/project.assets.json", $$"""
            {
              "targets": { "net10.0": { "Contract/1.0": {
                "type": "package",
                "compile": { "ref/net10.0/PackageContract.dll": {} },
                "runtime": { "lib/net10.0/_._": {} }
              } } },
              "libraries": { "Contract/1.0": { "path": "contract/1.0" } },
              "packageFolders": { {{JsonSerializer.Serialize(packageRoot)}}: {} }
            }
            """);
        Assert.DoesNotContain("cannot resolve the complete public signature", Run(), StringComparison.Ordinal);
        string assets = Path.Combine(m_root, "src/foundation/core/Inno.Core.Probe/obj/project.assets.json");
        string restored = File.ReadAllText(assets).Replace("\"targets\": { \"net10.0\"", "\"targets\": { \".NETCoreApp,Version=v10.0\"", StringComparison.Ordinal);
        restored = restored.Insert(restored.LastIndexOf('}'), ", \"project\": { \"frameworks\": { \"net10.0\": {} } }");
        File.WriteAllText(assets, restored);
        Assert.DoesNotContain("cannot resolve the complete public signature", Run(), StringComparison.Ordinal);
        File.WriteAllText(assets, restored.Replace(".NETCoreApp,Version=v10.0", "net10.0-browser1.0", StringComparison.Ordinal)
            .Replace("\"frameworks\": { \"net10.0\": {} }", "\"frameworks\": { \"net10.0-browser1.0\": { \"targetAlias\": \"net10.0\" } }", StringComparison.Ordinal));
        Assert.DoesNotContain("cannot resolve the complete public signature", Run(), StringComparison.Ordinal);
        File.Delete(reference);
        Assert.Throws<FileNotFoundException>(() => Run());
    }

    [Fact]
    public void UnresolvedPublicTypesCannotBeReportedAsCompleteApiCoverage()
    {
        Write("src/foundation/core/Inno.Core.Probe/Probe.cs", """
            namespace Inno.Core.Probe;
            public static class Probe
            {
                public static MissingDependency Read() => throw new System.NotSupportedException();
            }
            """);
        Assert.Contains("cannot resolve the complete public signature", Run(), StringComparison.Ordinal);
    }

    [Fact]
    public void LogicalExamplesRequireExplicitReferencesAndExcludeHostImplementations()
    {
        Write("docs/core/Inno.Core.Probe.md", """
            # Probe
            ```csharp
            using InnoEngine.Probe;
            public static class Example { public static int Read() => Value.Read(); }
            ```
            """);
        Assert.Contains("Logical script examples require explicit current", Run(), StringComparison.Ordinal);
        string reference = EmitScriptReference("Inno.ScriptApi.Runtime",
            "namespace InnoEngine.Probe { public static class Value { public static int Read() => 7; } }");
        string validation = Run(null, "--wiki-runtime-reference", reference);
        Assert.True(!validation.Contains("example 1 at line", StringComparison.Ordinal), validation);
        Write("docs/core/Inno.Core.Probe.md", """
            # Probe
            ```csharp
            using InnoEngine.Probe;
            using Inno.Core.Probe;
            public static class Example { public static string? Read() => Probe.Read(); }
            ```
            """);
        Assert.Contains("example 1 at line", Run(null, "--wiki-runtime-reference", reference), StringComparison.Ordinal);
    }

    [Fact]
    public void EditorExamplesUseBothLogicalScopesAndRecordTheirFrozenIdentity()
    {
        string runtime = EmitScriptReference("Inno.ScriptApi.Runtime",
            "namespace InnoEngine.Probe { public static class Value { public static int Read() => 7; } }");
        string editor = EmitScriptReference("Inno.ScriptApi.Editor",
            "namespace InnoEditor.Probe { public static class Action { public static int Read() => 9; } }");
        Write("docs/core/Inno.Core.Probe.md", """
            # Probe
            ```csharp
            using InnoEngine.Probe;
            using InnoEditor.Probe;
            public static class Example { public static int Read() => Value.Read() + Action.Read(); }
            ```
            """);
        Assert.Contains("Logical script examples require explicit current", Run(null, "--wiki-runtime-reference", runtime), StringComparison.Ordinal);
        string inventory = Path.Combine(m_root, "examples.json");
        string validation = Run(null,
            "--wiki-runtime-reference", runtime, "--wiki-editor-reference", editor,
            "--wiki-examples", inventory);
        Assert.True(!validation.Contains("example 1 at line", StringComparison.Ordinal), validation);
        using JsonDocument results = JsonDocument.Parse(File.ReadAllText(inventory));
        JsonElement example = results.RootElement[0];
        Assert.Equal("editor-script", example.GetProperty("referenceMode").GetString());
        Assert.Contains(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(runtime))),
            example.GetProperty("referenceIdentity").GetString(), StringComparison.Ordinal);
        Assert.Contains(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(editor))),
            example.GetProperty("referenceIdentity").GetString(), StringComparison.Ordinal);
    }

    [Fact]
    public void ScriptReferenceRoleRejectsAnUnrelatedAssembly()
    {
        string reference = EmitScriptReference("Unrelated", "public sealed class Unrelated {}");
        Assert.Throws<InvalidDataException>(() => Run(null, "--wiki-runtime-reference", reference));
        Assert.Throws<ArgumentException>(() => Run(null, "--wiki-editor-reference", reference));
    }

    [Fact]
    public async Task SdkExamplesRejectAssembliesOutsideTheOwningCompileReferences()
    {
        string projectDirectory = Path.Combine(m_root, "src/foundation/core/Inno.Core.Probe");
        string project = Path.Combine(projectDirectory, "Inno.Core.Probe.csproj");
        File.WriteAllText(project, """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <DefineConstants>SELECTED_CONTRACT</DefineConstants>
              </PropertyGroup>
              <ItemGroup><Compile Remove="Excluded.cs" /><Compile Include="obj/SelectedBindings.cs" /></ItemGroup>
            </Project>
            """);
        Write("src/foundation/core/Inno.Core.Probe/Conditional.cs", """
            namespace Inno.Core.Probe;
            #if SELECTED_CONTRACT
            /// <summary>Belongs to the selected compilation.</summary>
            public sealed class Selected;
            #else
            public sealed class WrongBranch;
            #endif
            """);
        Write("src/foundation/core/Inno.Core.Probe/Excluded.cs", "public sealed class Excluded;");
        Write("src/foundation/core/Inno.Core.Probe/obj/SelectedBindings.cs", """
            namespace Inno.Core.Probe;
            /// <summary>Represents a generated contract selected by the SDK.</summary>
            public sealed class GeneratedContract
            {
                /// <summary>Identifies the generated nested value contract.</summary>
                public enum ValueKind { None, Present }
            }
            """);
        Write("src/foundation/core/Inno.Core.Probe/obj/UnselectedBindings.cs", "public sealed class UnselectedContract;");
        string output = Path.Combine(projectDirectory, "bin/Debug/net10.0");
        Directory.CreateDirectory(output);
        File.Copy(EmitScriptReference("Inno.Core.Probe",
            "namespace Inno.Core.Probe { public static class Probe { public static string Read() => string.Empty; } }"),
            Path.Combine(output, "Inno.Core.Probe.dll"));
        // Unused companion documentation must not prevent reference-only compilation.
        File.WriteAllText(Path.Combine(output, "Inno.Core.Probe.xml"), "<p></th>");
        Write("src/foundation/core/Inno.Core.Unrelated/Inno.Core.Unrelated.csproj",
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>");
        string unrelated = Path.Combine(m_root, "src/foundation/core/Inno.Core.Unrelated/bin/Debug/net10.0");
        Directory.CreateDirectory(unrelated);
        File.Copy(EmitScriptReference("Inno.Core.Unrelated", "public static class ForeignContract { public static int Read() => 7; }"),
            Path.Combine(unrelated, "Inno.Core.Unrelated.dll"));
        string runtime = RuntimeEnvironment.GetRuntimeDirectory();
        string host = Path.GetFullPath(Path.Combine(runtime, "..", "..", "..",
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"));
        var start = new ProcessStartInfo(host)
        {
            UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true
        };
        foreach (string argument in new[] { "restore", project, "--disable-parallel", "--nologo", "-m:1", "-nr:false" })
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start)!;
        Task<string> standardOutput = process.StandardOutput.ReadToEndAsync();
        Task<string> standardError = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        try
        {
            await process.WaitForExitAsync(timeout.Token);
            Assert.True(process.ExitCode == 0, await standardOutput + await standardError);
        }
        finally
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync();
            await Task.WhenAll(standardOutput, standardError);
        }
        Write("docs/core/Inno.Core.Probe.md",
            "# Probe\n```csharp\npublic static class Example { public static string Read() => Inno.Core.Probe.Probe.Read(); "
            + "public static int Count(params System.ReadOnlySpan<int> values) => values.Length; }\n```\n");
        string inventory = Path.Combine(m_root, "sdk-api.json");
        Assert.DoesNotContain("example 1 at line", Run(inventory, "--dotnet", host), StringComparison.Ordinal);
        string api = File.ReadAllText(inventory);
        Assert.Contains("T:Inno.Core.Probe.Selected", api, StringComparison.Ordinal);
        Assert.Contains("T:Inno.Core.Probe.GeneratedContract", api, StringComparison.Ordinal);
        Assert.Contains("F:Inno.Core.Probe.GeneratedContract.ValueKind.Present", api, StringComparison.Ordinal);
        Assert.DoesNotContain("UnselectedContract", api, StringComparison.Ordinal);
        Assert.DoesNotContain("WrongBranch", api, StringComparison.Ordinal);
        Assert.DoesNotContain("T:Excluded", api, StringComparison.Ordinal);
        Write("docs/core/Inno.Core.Probe.md",
            "# Probe\n```csharp\npublic static class Example { public static int Read() => ForeignContract.Read(); }\n```\n");
        Assert.Contains("example 1 at line", Run(null, "--dotnet", host), StringComparison.Ordinal);
        string validProject = File.ReadAllText(project);
        File.WriteAllText(project, validProject.Replace("</Project>",
            "<Target Name=\"SelectUnavailableReference\" AfterTargets=\"ResolveReferences\">"
            + "<ItemGroup><ReferencePath Include=\"absent-contract.dll\" /></ItemGroup></Target></Project>", StringComparison.Ordinal));
        Write("docs/architecture/GUIDE.md", "```csharp\nint value = missing;\n```\n");
        string unavailable = Run(inventory, "--dotnet", host,
            "--wiki-examples", Path.Combine(m_root, "unavailable-examples.json"));
        Assert.Contains("requires its evaluated compile reference", unavailable, StringComparison.Ordinal);
        Assert.Contains("GUIDE.md' example 1", unavailable, StringComparison.Ordinal);
        Assert.True(File.Exists(inventory));
        using (JsonDocument unavailableApi = JsonDocument.Parse(File.ReadAllText(inventory)))
            Assert.Equal(0, unavailableApi.RootElement.GetArrayLength());
        File.WriteAllText(project, validProject);
        File.WriteAllText(project, File.ReadAllText(project).Replace("</DefineConstants>",
            "</DefineConstants><LangVersion>99.0</LangVersion>", StringComparison.Ordinal));
        Assert.Contains("unsupported C# language version", Assert.Throws<InvalidDataException>(
            () => Run(null, "--dotnet", host)).Message, StringComparison.Ordinal);
    }

    private string EmitScriptReference(
        string name,
        string source
    ) {
        string path = Path.Combine(m_root, name + ".dll");
        // Runtime patch selection is independent of the SDK's compile reference pack.
        string manifest = Path.Combine(AppContext.BaseDirectory, "WikiFrameworkReferences.txt");
        Assert.True(File.Exists(manifest), "Build the Wiki fixture with its SDK-evaluated framework references.");
        string[] references = File.ReadAllLines(manifest);
        Assert.NotEmpty(references);
        Assert.All(references, static reference => Assert.True(Path.IsPathFullyQualified(reference) && File.Exists(reference),
            "The evaluated framework reference is unavailable: " + reference));
        var compilation = CSharpCompilation.Create(name,
            [CSharpSyntaxTree.ParseText(source)],
            references.Select(static reference => MetadataReference.CreateFromFile(reference)),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var result = compilation.Emit(path);
        Assert.True(result.Success, string.Join(Environment.NewLine, result.Diagnostics));
        return path;
    }

    private string Run(
        string? inventory = null,
        params string[] extraArguments
    )
    {
        TextWriter output = Console.Out;
        TextWriter error = Console.Error;
        using var captured = new StringWriter();
        try
        {
            Console.SetOut(captured);
            Console.SetError(captured);
            string[] arguments = inventory is null ? [m_root, "--wiki"] : [m_root, "--wiki", "--wiki-api", inventory];
            _ = ArchitectureValidator.Execute(arguments.Concat(extraArguments).ToArray());
            return captured.ToString();
        }
        finally
        {
            Console.SetOut(output);
            Console.SetError(error);
        }
    }

    private void Write(
        string relative,
        string value
    ) {
        string file = Path.Combine(m_root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, value);
    }
}
