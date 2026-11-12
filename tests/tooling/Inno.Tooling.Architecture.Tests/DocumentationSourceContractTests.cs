using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class DocumentationSourceContractTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoDocumentationSourceContracts", Guid.NewGuid().ToString("N"));
    private const string C_PROJECT = "tests/support/Contract/Contract.csproj";
    private readonly string m_host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
        ?? throw new InvalidOperationException("The source contract fixture requires its selected SDK host.");

    public DocumentationSourceContractTests()
    {
        Write("InnoEngine.sln", "Microsoft Visual Studio Solution File, Format Version 12.00\n");
        Write(C_PROJECT, "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework><Nullable>enable</Nullable></PropertyGroup></Project>");
    }

    [Fact]
    public async Task SupportLibrariesRetainNullableRefGenericRecordAndInheritedContracts()
    {
        const string C_SOURCE = """
            namespace Contract;
            /// <summary>Reads a borrowed value.</summary>
            public interface IReader { /// <summary>Returns the borrowed value.</summary>
                string? Read(); }
            /// <summary>Captures an optional value.</summary>
            /// <param name="Value">The optional value.</param>
            public sealed record Snapshot(string? Value) : IReader
            {
                /// <inheritdoc />
                public string? Read() => Value;
            }
            public readonly struct Token { }
            public delegate string Transform(string? value);
            public sealed class Probe
            {
                private int m_value;
                public ref int Value => ref m_value;
                public ref readonly int Read() => ref m_value;
                public T? Select<T>(string? name = null) where T : class => null;
            }
            """;
        Write("tests/support/Contract/Contract.cs", C_SOURCE);
        await RestoreAsync();
        string facts = Path.Combine(m_root, "facts.json");
        Assert.Equal(0, DocumentationReferences.ExportApi(ExportArguments(facts)));
        using JsonDocument document = JsonDocument.Parse(File.ReadAllBytes(facts));
        JsonElement[] symbols = document.RootElement.GetProperty("projects")[0].GetProperty("symbols").EnumerateArray().ToArray();
        string Signature(string id) => symbols.Single(symbol => symbol.GetProperty("id").GetString() == id).GetProperty("signature").GetString()!;
        Assert.Contains("sealed", Signature("T:Contract.Probe"));
        Assert.Contains("readonly", Signature("T:Contract.Token"));
        Assert.Contains("delegate string", Signature("T:Contract.Transform"));
        Assert.Contains("string? value", Signature("T:Contract.Transform"));
        Assert.Contains("ref int", Signature("P:Contract.Probe.Value"));
        Assert.Contains("ref readonly int", Signature("M:Contract.Probe.Read"));
        Assert.Contains("name = null", Signature("M:Contract.Probe.Select``1(System.String)"));
        Assert.Contains("where T : class", Signature("M:Contract.Probe.Select``1(System.String)"));
        Assert.Contains("IReader", Signature("T:Contract.Snapshot"));
        Assert.Contains(symbols, symbol => symbol.GetProperty("id").GetString() == "M:Contract.Snapshot.#ctor(System.String)");
        Assert.Contains(symbols, symbol => symbol.GetProperty("id").GetString() == "P:Contract.Snapshot.Value");
        Assert.Contains("borrowed value", symbols.Single(symbol => symbol.GetProperty("id").GetString() == "M:Contract.Snapshot.Read").GetProperty("documentation").GetString());
        Assert.DoesNotContain(symbols, symbol => symbol.GetProperty("id").GetString()!.Contains("m_value", StringComparison.Ordinal));
        string comparison = Path.Combine(m_root, "comparison.json");
        string[] arguments = ["--engine-root", m_root, "--dotnet", m_host, "--reference", facts, "--output", comparison];
        Assert.Equal(0, await DocumentationReferences.ValidateApiAsync(arguments));
        Write("tests/support/Contract/Contract.cs", C_SOURCE.Replace("sealed class Probe", "class Probe", StringComparison.Ordinal)
            .Replace("ref readonly int Read()", "ref int Read()", StringComparison.Ordinal)
            .Replace("string? name = null", "string name = \"default\"", StringComparison.Ordinal));
        Assert.Equal(1, await DocumentationReferences.ValidateApiAsync(arguments));
        string failures = File.ReadAllText(comparison);
        Assert.Contains("T:Contract.Probe", failures);
        Assert.Contains("M:Contract.Probe.Read", failures);
        Assert.Contains("M:Contract.Probe.Select", failures);
    }

    [Fact]
    public async Task CompileOnlyPackageContractsNeedNoRuntimeDeploymentAndCannotDisappear()
    {
        string reference = Path.Combine(m_root, "PackageContract.dll");
        var compilation = CSharpCompilation.Create("PackageContract",
            [CSharpSyntaxTree.ParseText("namespace PackageContract; public abstract class BaseContract { }")],
            ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator).Select(static file => MetadataReference.CreateFromFile(file)),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        Assert.True(compilation.Emit(reference).Success);
        string feed = Path.Combine(m_root, "feed");
        Directory.CreateDirectory(feed);
        using (ZipArchive package = ZipFile.Open(Path.Combine(feed, "ContractFixture.1.0.0.nupkg"), ZipArchiveMode.Create))
        {
            using (var writer = new StreamWriter(package.CreateEntry("ContractFixture.nuspec").Open(), new UTF8Encoding(false)))
                writer.Write("<package><metadata><id>ContractFixture</id><version>1.0.0</version><authors>InnoEngine</authors><description>Compile-only fixture.</description></metadata></package>");
            package.CreateEntryFromFile(reference, "ref/net10.0/PackageContract.dll");
            using Stream empty = package.CreateEntry("lib/net10.0/_._").Open();
        }
        Write(C_PROJECT, "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>"
            + "<ItemGroup><PackageReference Include=\"ContractFixture\" Version=\"1.0.0\" IncludeAssets=\"compile\" /></ItemGroup></Project>");
        Write("tests/support/Contract/Contract.cs", "namespace Contract; public abstract class Probe : PackageContract.BaseContract;");
        await RestoreAsync("--source", feed, "--packages", Path.Combine(m_root, "packages"));
        string output = Path.Combine(m_root, "facts.json");
        Assert.Equal(0, DocumentationReferences.ExportApi(ExportArguments(output)));
        Assert.Contains("PackageContract.BaseContract", File.ReadAllText(output));
        Assert.False(File.Exists(Path.Combine(m_root, "tests/support/Contract/bin/Debug/net10.0/PackageContract.dll")));
        byte[] previous = File.ReadAllBytes(output);
        File.Delete(Path.Combine(m_root, "packages/contractfixture/1.0.0/ref/net10.0/PackageContract.dll"));
        Exception failure = Assert.ThrowsAny<Exception>(() => DocumentationReferences.ExportApi(ExportArguments(output)));
        Assert.True(failure is FileNotFoundException or InvalidDataException, failure.ToString());
        Assert.Contains("cannot resolve the complete public signature of 'T:Contract.Probe'", failure.Message);
        Assert.Equal(previous, File.ReadAllBytes(output));
    }

    [Fact]
    public async Task EvaluatedImportsConstantsAndGeneratedInputsDefineTheSourceContract()
    {
        Write("tests/support/Contract/Framework.props", "<Project><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>");
        const string C_PROJECT_TEXT = "<Project Sdk=\"Microsoft.NET.Sdk\"><Import Project=\"Framework.props\" />"
            + "<PropertyGroup><DefineConstants>SELECTED_CONTRACT</DefineConstants></PropertyGroup>"
            + "<ItemGroup><Compile Remove=\"Excluded.cs\" /><Compile Include=\"obj/Selected.cs\" /></ItemGroup></Project>";
        Write(C_PROJECT, C_PROJECT_TEXT);
        Write("tests/support/Contract/Contract.cs", "namespace Contract;\n#if SELECTED_CONTRACT\npublic sealed class Selected;\n#else\npublic sealed class WrongBranch;\n#endif\n");
        Write("tests/support/Contract/Excluded.cs", "public sealed class Excluded;");
        Write("tests/support/Contract/obj/Selected.cs", "namespace Contract; public sealed class Generated { public enum Kind { None, Present } }");
        Write("tests/support/Contract/obj/Unselected.cs", "public sealed class Unselected;");
        await RestoreAsync();
        string output = Path.Combine(m_root, "facts.json");
        Assert.Equal(0, DocumentationReferences.ExportApi(ExportArguments(output)));
        string facts = File.ReadAllText(output);
        Assert.Contains("T:Contract.Selected", facts);
        Assert.Contains("F:Contract.Generated.Kind.Present", facts);
        foreach (string excluded in new[] { "WrongBranch", "Excluded", "Unselected" }) Assert.DoesNotContain(excluded, facts);
        Write(C_PROJECT, C_PROJECT_TEXT.Replace("</DefineConstants>", "</DefineConstants><LangVersion>99.0</LangVersion>", StringComparison.Ordinal));
        InvalidDataException language = Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportApi(ExportArguments(output)));
        Assert.Contains("unsupported C# language version", language.Message);
        Assert.Equal(facts, File.ReadAllText(output));
        Write(C_PROJECT, C_PROJECT_TEXT.Replace("</Project>", "<Target Name=\"MissingReference\" AfterTargets=\"ResolveReferences\"><ItemGroup><ReferencePath Include=\"absent-contract.dll\" /></ItemGroup></Target></Project>", StringComparison.Ordinal));
        InvalidDataException missing = Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportApi(ExportArguments(output)));
        Assert.Contains("requires its evaluated compile reference", missing.Message);
        Assert.Equal(facts, File.ReadAllText(output));
    }

    [Fact]
    public async Task PrivateProductEntriesHaveAnExplicitEmptySurfaceWithoutCompiledDependencies()
    {
        Write(C_PROJECT, File.ReadAllText(Path.Combine(m_root, C_PROJECT)).Replace("</Project>", """
            <Target Name="MissingReference" AfterTargets="ResolveReferences">
              <ItemGroup><ReferencePath Include="absent-product-contract.dll" /></ItemGroup>
            </Target>
            <Target Name="RejectProductBuild" BeforeTargets="CoreCompile">
              <Error Text="Source API inspection must not build a product." />
            </Target>
            </Project>
            """, StringComparison.Ordinal));
        const string C_SOURCE = "namespace Contract; internal sealed class Entry : UnavailableBase { public void Run() { } }";
        Write("tests/support/Contract/Entry.cs", C_SOURCE);
        await RestoreAsync();
        string output = Path.Combine(m_root, "facts.json");
        Assert.Equal(0, DocumentationReferences.ExportApi(ExportArguments(output)));
        byte[] original = File.ReadAllBytes(output);
        using (JsonDocument facts = JsonDocument.Parse(original))
        {
            Assert.Equal("Unverified", facts.RootElement.GetProperty("sourceApiStatus").GetString());
            JsonElement owner = Assert.Single(facts.RootElement.GetProperty("projects").EnumerateArray());
            Assert.Equal("Contract", owner.GetProperty("name").GetString());
            Assert.Empty(owner.GetProperty("symbols").EnumerateArray());
        }
        Assert.False(File.Exists(Path.Combine(m_root, "tests/support/Contract/bin/Debug/net10.0/Contract.dll")));
        Write("tests/support/Contract/Entry.cs", C_SOURCE.Replace("internal sealed", "public sealed", StringComparison.Ordinal));
        InvalidDataException failure = Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportApi(ExportArguments(output)));
        Assert.Contains("requires its evaluated compile reference", failure.Message);
        Assert.Equal(original, File.ReadAllBytes(output));
    }

    [Fact]
    public async Task EmptySurfaceUsesSelectedConditionalAndGeneratedSourceInputs()
    {
        Write(C_PROJECT, File.ReadAllText(Path.Combine(m_root, C_PROJECT)).Replace("</Project>", """
            <PropertyGroup><DefineConstants>EXPOSE_CONTRACT</DefineConstants></PropertyGroup>
            <ItemGroup><Compile Include="obj/Selected.cs" /></ItemGroup>
            </Project>
            """, StringComparison.Ordinal));
        Write("tests/support/Contract/Entry.cs", "namespace Contract; internal sealed class Entry;");
        Write("tests/support/Contract/obj/Selected.cs", """
            namespace Contract;
            #if EXPOSE_CONTRACT
            public delegate void GeneratedContract();
            #else
            internal delegate void GeneratedContract();
            #endif
            """);
        await RestoreAsync();
        string output = Path.Combine(m_root, "facts.json");
        Assert.Equal(0, DocumentationReferences.ExportApi(ExportArguments(output)));
        Assert.Contains("T:Contract.GeneratedContract", File.ReadAllText(output));
        byte[] previous = File.ReadAllBytes(output);
        Write("tests/support/Contract/obj/Selected.cs", "internal class Broken {");
        InvalidDataException invalid = Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportApi(ExportArguments(output)));
        Assert.Contains("invalid C# syntax", invalid.Message);
        Assert.Equal(previous, File.ReadAllBytes(output));
        File.Delete(Path.Combine(m_root, "tests/support/Contract/obj/Selected.cs"));
        InvalidDataException missing = Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportApi(ExportArguments(output)));
        Assert.Contains("source is missing", missing.Message);
        Assert.Equal(previous, File.ReadAllBytes(output));
    }

    [Fact]
    public async Task ForeignProductSelectionCannotBorrowHostReferences()
    {
        Write(C_PROJECT, File.ReadAllText(Path.Combine(m_root, C_PROJECT)).Replace("</PropertyGroup>",
            "<InnoProductTarget>foreign-fixture</InnoProductTarget></PropertyGroup>", StringComparison.Ordinal));
        Write("tests/support/Contract/Contract.cs", "namespace Contract; public sealed class Probe;");
        await RestoreAsync();
        string output = Path.Combine(m_root, "facts.json");
        File.WriteAllText(output, "retain prior evidence");
        Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportApi([..ExportArguments(output), "--target", "host-fixture"]));
        Assert.Equal("retain prior evidence", File.ReadAllText(output));
    }

    public void Dispose()
    {
        string boundary = Path.Combine(Path.GetTempPath(), "InnoDocumentationSourceContracts") + Path.DirectorySeparatorChar;
        if (!Path.GetFullPath(m_root).StartsWith(boundary, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("Fixture ownership escaped.");
        Directory.Delete(m_root, recursive: true);
    }

    private string[] ExportArguments(string output) => ["--engine-root", m_root, "--dotnet", m_host, "--project", C_PROJECT, "--output", output];

    private async Task RestoreAsync(params string[] arguments)
    {
        var start = new ProcessStartInfo(m_host) { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (string argument in new[] { "restore", Path.Combine(m_root, C_PROJECT), "--disable-parallel", "-m:1", "-nr:false", "--disable-build-servers" }.Concat(arguments)) start.ArgumentList.Add(argument);
        using Process process = Process.Start(start)!;
        Task<string> stdout = process.StandardOutput.ReadToEndAsync();
        Task<string> stderr = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        Assert.True(process.ExitCode == 0, await stdout + await stderr);
    }

    private void Write(string relative, string text)
    {
        string file = Path.Combine(m_root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, text);
    }
}
