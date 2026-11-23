using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Core.IO;
using Inno.Testing.Source;
using Inno.Testing.Validation;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Validation.Tests;

[Collection(nameof(ValidationPlanFixture))]
public sealed class ValidationNativePreparationTests : ValidationPlanFixture
{
    [Fact]
    public async Task NativeTestOwnersCannotOmitTheirProductDeployment()
    {
        using var repository = new ValidationRepository();
        repository.Write(
            "tests/Native.Tests/Native.Tests.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>native</InnoValidationLayer></PropertyGroup>"
            )
        );
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
        Assert.Contains("Native test owner must declare its product deployment", failure.Message);
        Assert.Contains("tests/Native.Tests/Native.Tests.csproj", failure.Message);
    }

    [Fact]
    public async Task DomainSuiteCannotTransitivelyReachNativePreparation()
    {
        using var repository = new ValidationRepository();
        repository.Write(
            "backends/Example/native/Native/Native.csproj",
            ValidationRepository.Project("<PropertyGroup><InnoProductId>fixture</InnoProductId></PropertyGroup>")
        );
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project(
                "<ItemGroup><ProjectReference Include=\"../../backends/Example/native/Native/Native.csproj\" /></ItemGroup>"
            )
        );
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
    }

    [Fact]
    public async Task CommittedBindingDeclarationsDoNotRequireRuntimeDeployment()
    {
        using var repository = new ValidationRepository();
        repository.Write(
            "backends/Example/native/Native/Native.csproj",
            ValidationRepository.Project("<PropertyGroup><BindGenGeneratedBindings>true</BindGenGeneratedBindings></PropertyGroup>")
        );
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project(
                "<ItemGroup><ProjectReference Include=\"../../backends/Example/native/Native/Native.csproj\" /></ItemGroup>"
            )
        );
        using JsonDocument plan = await repository.PlanAsync();
        Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        JsonElement check = plan.RootElement
            .GetProperty("checks")
            .EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "test:tests/Example.Tests/Example.Tests.csproj");
        Assert.Equal("managed", check.GetProperty("context").GetString());
        Assert.Equal(JsonValueKind.Null, check.GetProperty("exclusion").ValueKind);
    }

    [Fact]
    public async Task AManagedInteropOwnerDoesNotBecomeNativePreparationBecauseOfItsDirectoryName()
    {
        using var repository = new ValidationRepository();
        repository.Write("backends/Example/native/Loader/Loader.csproj", ValidationRepository.Project());
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project(
                "<ItemGroup><ProjectReference Include=\"../../backends/Example/native/Loader/Loader.csproj\" /></ItemGroup>"
            )
        );
        using JsonDocument plan = await repository.PlanAsync();
        Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        JsonElement domain = plan.RootElement
            .GetProperty("checks")
            .EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "test:tests/Example.Tests/Example.Tests.csproj");
        Assert.Equal("managed", domain.GetProperty("context").GetString());
    }

    [Fact]
    public async Task NativeContextUsesTheDeclaredDevelopmentProductAndKeepsDomainOwnersIndependent()
    {
        using var repository = new ValidationRepository();
        string properties = repository.PathOf("Product.props");
        repository.Write("Product.props", "<Project />");
        repository.Write(
            "entry/InnoEngine.Editor/InnoEngine.Editor.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
                + properties + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"
            )
        );
        repository.Write(
            "backends/Example/native/Native/Native.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><BindGenGeneratedBindings>true</BindGenGeneratedBindings></PropertyGroup>"
            + "<ItemGroup Condition=\"'$(InnoNativeTarget)' != ''\"><InnoNativeDependency Include=\"fixture-tools\"><TargetId>fixture-target</TargetId><Configuration>Release</Configuration></InnoNativeDependency></ItemGroup>"
            )
        );
        repository.Write(
            "tests/backends/Example/Native.Tests/Native.Tests.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>native</InnoValidationLayer><InnoTestNativeProduct>editor</InnoTestNativeProduct><InnoTestNativeTarget>cross-target</InnoTestNativeTarget></PropertyGroup>"
                + "<ItemGroup><ProjectReference Include=\"../../../../backends/Example/native/Native/Native.csproj\" /></ItemGroup>"
            )
        );
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Equal(
            "fixture-target",
            plan.RootElement
                .GetProperty("contexts")
                .GetProperty("native")
                .GetProperty("properties")
                .GetProperty("InnoNativeTarget")
                .GetString()
        );
        JsonElement domain = plan.RootElement
            .GetProperty("checks")
            .EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "test:tests/Example.Tests/Example.Tests.csproj");
        Assert.Equal("managed", domain.GetProperty("context").GetString());
        JsonElement native = plan.RootElement
            .GetProperty("checks")
            .EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "test:tests/backends/Example/Native.Tests/Native.Tests.csproj");
        Assert.Equal("native", native.GetProperty("context").GetString());
        Assert.Contains(plan.RootElement.GetProperty("nativeRequests").EnumerateArray(), request => (
            request.GetProperty("product").GetString() == "fixture-tools"
            && request.GetProperty("configuration").GetString() == "Release"
            && request.GetProperty("target").GetString() == "fixture-target"
        ));
        Assert.Contains(plan.RootElement.GetProperty("nativeRequests").EnumerateArray(), request => (
            request.GetProperty("product").GetString() == "editor" && request.GetProperty("target").GetString() == "cross-target"
        ));
        Assert.DoesNotContain(plan.RootElement.GetProperty("nativeRequests").EnumerateArray(), request => (
            request.GetProperty("product").GetString() == "editor" && request.GetProperty("target").GetString() == "fixture-target"
        ));
        JsonElement contexts = plan.RootElement.GetProperty("contexts");
        string Output(string context)
            => contexts.GetProperty(context)
                .GetProperty("sdkGraph")
                .EnumerateArray()
                .Single(
                    node => (
                        repository.BindWorkspace(
                            node.GetProperty("Path").GetString()!
                        ) == repository.PathOf("src/Example/Example.csproj")
                    )
                )
                .GetProperty("Properties")
                .GetProperty("TargetPath")
                .GetString()!;
        Assert.NotEqual(Output("managed"), Output("native"));
        Assert.StartsWith(
            contexts.GetProperty("native").GetProperty("properties").GetProperty("ArtifactsPath").GetString()!,
            Output("native"),
            StringComparison.Ordinal
        );
        string[] sequence = plan.RootElement
            .GetProperty("checks")
            .EnumerateArray()
            .Select(check => check.GetProperty("id").GetString()!)
            .ToArray();
        const string C_NATIVE_BUILD = "build:tests/backends/Example/Native.Tests/Native.Tests.csproj";
        Assert.Equal(
            "managed",
            plan.RootElement.GetProperty("checks")
                .EnumerateArray()
                .Single(check => check.GetProperty("id").GetString() == "build:backends/Example/native/Native/Native.csproj")
                .GetProperty("context")
                .GetString()
        );
        Assert.True(Array.IndexOf(sequence, domain.GetProperty("id").GetString()!) < Array.IndexOf(sequence, C_NATIVE_BUILD));
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        using JsonDocument results = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
        JsonElement[] recorded = results.RootElement.GetProperty("results").EnumerateArray().ToArray();
        Assert.Equal(sequence, recorded.Select(result => result.GetProperty("id").GetString()!).ToArray());
        Assert.All(
            results.RootElement.GetProperty("failures").EnumerateArray(),
            fault => Assert.Equal("test-execution", fault.GetProperty("stage").GetString())
        );
        Assert.Equal(
            "Blocked",
            results.RootElement
                .GetProperty("results")
                .EnumerateArray()
                .Single(result => result.GetProperty("id").GetString() == C_NATIVE_BUILD)
                .GetProperty("status")
                .GetString()
        );
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BindingConsumptionUsesItsProductOwnerOrItsExplicitStandaloneSelection(bool standalone)
    {
        using var repository = new ValidationRepository();
        repository.Write("Product.props", "<Project />");
        repository.Write(
            "entry/InnoEngine.Editor/InnoEngine.Editor.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
                + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"
            )
        );
        const string C_BINDING = "backends/Example/native/Native/Native.csproj";
        repository.Write(
            C_BINDING,
            ValidationRepository.Project(
                "<PropertyGroup><BindGenGeneratedBindings>true</BindGenGeneratedBindings><InnoBindingProductId>editor</InnoBindingProductId></PropertyGroup>"
            )
        );
        string consumer = standalone
                          ? "backends/Example/runtime/Adapter/Adapter.csproj"
                          : "tests/backends/Example/Native.Tests/Native.Tests.csproj";
        string reference = Path.GetRelativePath(Path.GetDirectoryName(repository.PathOf(consumer))!, repository.PathOf(C_BINDING))
            .Replace('\\', '/');
        string prerequisite = standalone
            ? "<ItemGroup><InnoNativeDependency Include=\"fixture-tools\" TargetId=\"fixture-target\" Configuration=\"Release\" /></ItemGroup>"
            : "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>native</InnoValidationLayer><InnoTestNativeProduct>player</InnoTestNativeProduct><InnoTestNativeTarget>fixture-target</InnoTestNativeTarget></PropertyGroup>";
        repository.Write(
            consumer,
            ValidationRepository.Project(
                prerequisite
                + "<ItemGroup><ProjectReference Include=\"" + reference + "\" /></ItemGroup>"
            )
        );
        if (standalone)
        {
            repository.Write(
                "tests/backends/Example/Adapter.Tests/Adapter.Tests.csproj",
                ValidationRepository.Project(
                    "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>native</InnoValidationLayer><InnoTestNativeProduct>player</InnoTestNativeProduct><InnoTestNativeTarget>fixture-target</InnoTestNativeTarget></PropertyGroup>"
                    + "<ItemGroup><ProjectReference Include=\"../../../../backends/Example/runtime/Adapter/Adapter.csproj\" /></ItemGroup>"
                )
            );
        }
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare a native binding and its consumer ownership");
        repository.Write(Path.GetDirectoryName(consumer) + "/Changed.cs", "public sealed class Changed { }");
        using JsonDocument plan = await repository.PlanAsync();
        string[] products = plan.RootElement.GetProperty("nativeRequests")
            .EnumerateArray()
            .Select(request => request.GetProperty("product").GetString()!)
            .ToArray();
        if (standalone)
        {
            Assert.Contains("editor", products);
            Assert.Contains("fixture-tools", products);
        }
        else
        {
            Assert.Contains("player", products);
            Assert.DoesNotContain("editor", products);
        }
    }

    [Theory]
    [InlineData("plan", false)]
    [InlineData("plan", true)]
    [InlineData("run", false)]
    [InlineData("run", true)]
    [InlineData("full", false)]
    [InlineData("full", true)]
    public async Task MissingNativeSourcesProducePreparationRequirementsInsteadOfExecutableEvidence(
        string operation,
        bool documentation
    ) {
        using var repository = new ValidationRepository();
        const string C_OWNER = "src/Native/Native.csproj";
        repository.Write(C_OWNER, ValidationRepository.Project("<PropertyGroup><InnoProductId>fixture</InnoProductId></PropertyGroup>"));
        string revision = repository.Git("rev-parse", "HEAD");
        repository.Git("update-index", "--add", "--cacheinfo", "160000," + revision + ",extern/Uninitialized");
        repository.Git("add", C_OWNER);
        repository.Git("commit", "-m", "Declare independently owned native inputs");
        Directory.CreateDirectory(repository.PathOf("extern/Uninitialized"));
        var docs = new ValidationDocumentationFixture(repository);
        DocumentationResult? snapshot = null;
        if (documentation)
        {
            docs.Initialize();
            snapshot = await docs.IndexAsync();
        }
        repository.Write("src/Native/Changed.cs", "// An affected native owner requires explicit preparation.");
        string output = repository.PathOf("artifacts/preflight");
        int status = await ValidationCommand.ExecuteAsync([
            operation,
            "--engine-root",
            repository.PathOf("."),
            "--dotnet",
            repository.dotnet,
            "--base",
            "HEAD",
            "--output",
            output,
            ..snapshot is null ? Array.Empty<string>() : ["--docs-cache-root", docs.cache, "--docs-snapshot", snapshot.snapshot!]
        ]);

        Assert.Equal(2, status);
        using JsonDocument report = JsonDocument.Parse(File.ReadAllText(Path.Combine(output, "preparation.json")));
        JsonElement preparation = report.RootElement;
        Assert.Equal("Blocked", preparation.GetProperty("status").GetString());
        Assert.Equal("NativeSources", preparation.GetProperty("kind").GetString());
        Assert.Equal(repository.Git("rev-parse", "HEAD"), preparation.GetProperty("commit").GetString());
        Assert.Contains(C_OWNER, preparation.GetProperty("owners").EnumerateArray().Select(owner => owner.GetString()));
        JsonElement sources = preparation.GetProperty("sources");
        Assert.Equal(revision, sources.GetProperty("extern/Uninitialized").GetString());
        Assert.False(File.Exists(Path.Combine(output, "plan.json")));
        Assert.False(File.Exists(Path.Combine(output, "results.json")));
        Assert.False(Directory.Exists(repository.PathOf("extern/Uninitialized/.git")));
    }
}
