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
public sealed class ValidationDocumentationPlanningTests : ValidationPlanFixture
{
    [Fact]
    public async Task DocumentationDoesNotSelectNativeOrManagedBuilds()
    {
        using var repository = new ValidationRepository();
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        DocumentationResult baseline = await docs.IndexAsync();
        docs.Write("content/en/guide.md", ValidationDocumentationFixture.Page("Example", "Updated usage guidance."));
        DocumentationResult current = await docs.IndexAsync();
        using JsonDocument plan = await docs.PlanAsync(current, baseline);
        Assert.Empty(Owners(plan));
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString() == "build");
        Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString() == "documentation-examples");
        Assert.Contains(checks, check => check.GetProperty("kind").GetString() == "documentation");
        Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
    }

    [Theory]
    [InlineData(true, false, false)]
    [InlineData(true, true, false)]
    [InlineData(true, true, true)]
    [InlineData(false, true, false)]
    public async Task TestGuideProseAvoidsCompilationWhileExamplesAndSupportLibrariesKeepTheirContracts(
        bool test,
        bool native,
        bool example
    ) {
        using var repository = new ValidationRepository();
        const string C_OWNER = "tests/fixtures/Support/Support.csproj";
        repository.Write(
            C_OWNER,
            ValidationRepository.Project(
                "<PropertyGroup>"
                + (test ? "<IsTestProject>true</IsTestProject><InnoValidationLayer>contract</InnoValidationLayer>" : string.Empty)
                + (native ? "<InnoTestNativeProduct>editor</InnoTestNativeProduct>" : string.Empty) + "</PropertyGroup>"
            )
        );
        repository.Write("Product.props", "<Project />");
        repository.Write(
            "entry/InnoEngine.Editor/InnoEngine.Editor.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
                + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"
            )
        );

        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare the test or support owner and its guide");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        DocumentationResult baseline = await docs.IndexAsync();
        docs.Write(
            "content/en/guide.md",
            ValidationDocumentationFixture.Page(
                "Support",
                "Updated execution instructions.\n"
                + (example ? "~~~csharp\nusing System;\nConsole.WriteLine(1);\n~~~\n" : "```powershell\ndotnet --info\n```\n"),
                example ? "host" : null
            )
        );
        DocumentationResult current = await docs.IndexAsync();
        using JsonDocument plan = await docs.PlanAsync(current, baseline);
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        bool compilation = example;
        Assert.Equal(compilation, checks.Any(check => check.GetProperty("id").GetString() == "build:" + C_OWNER));
        Assert.Equal(compilation && native, plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        JsonElement[] examples = checks.Where(check => check.GetProperty("kind").GetString() == "documentation-examples").ToArray();
        Assert.Equal(example ? 1 : 0, examples.Length);
        if (example)
        {
            Assert.Equal(native ? "native" : "managed", examples[0].GetProperty("context").GetString());
        }
        Assert.DoesNotContain(checks, check => (
            check.GetProperty("kind").GetString() == "test"
            && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null
        ));
        if (!test && example)
        {
            JsonElement api = Assert.Single(checks.Where(check => check.GetProperty("kind").GetString() == "api"));
            Assert.Contains(C_OWNER, api.GetProperty("projects").EnumerateArray().Select(static project => project.GetString()));
        }
    }

    [Fact]
    public async Task SourceSelectedDocumentationRequiresCanonicalStructureAndOwnerInputs()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Existing.cs", "namespace Example; public class Existing { }\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare source contract");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        DocumentationResult snapshot = await docs.IndexAsync();
        repository.Write("src/Example/Changed.cs", "namespace Example; public class Changed { }\n");
        using JsonDocument plan = await docs.PlanAsync(snapshot);
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        JsonElement structure = checks.Single(check => check.GetProperty("id").GetString() == "documentation");
        Assert.Contains(structure.GetProperty("inputs").EnumerateArray(), input => input.GetString() == "src/Example/Example.csproj");
        Assert.DoesNotContain(structure.GetProperty("inputs").EnumerateArray(), input => input.GetString() == "src/Example/Existing.cs");
        JsonElement api = checks.Single(check => check.GetProperty("kind").GetString() == "documentation-api");
        Assert.Contains(api.GetProperty("prerequisites").EnumerateArray(), prerequisite => prerequisite.GetString() == "documentation");
        Assert.Contains(api.GetProperty("inputs").EnumerateArray(), input => input.GetString() == "@src/Example/Example.csproj");
        Assert.Contains(
            plan.RootElement.GetProperty("inputSets").GetProperty("src/Example/Example.csproj").EnumerateArray(),
            input => input.GetString() == "src/Example/Existing.cs"
        );
    }

    [Fact]
    public async Task NativeDocumentationFragmentsUseTheirOwnerExecutionContext()
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
        const string C_OWNER = "backends/Example/runtime/Adapter/Adapter.csproj";
        repository.Write(
            "backends/Example/native/Native/Native.csproj",
            ValidationRepository.Project("<PropertyGroup><BindGenGeneratedBindings>true</BindGenGeneratedBindings></PropertyGroup>")
        );
        repository.Write(
            C_OWNER,
            ValidationRepository.Project(
                "<PropertyGroup><InnoProductId>editor</InnoProductId><InnoProductTarget>fixture-target</InnoProductTarget></PropertyGroup>"
                + "<ItemGroup><ProjectReference Include=\"../../native/Native/Native.csproj\" /></ItemGroup>"
            )
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare a managed adapter with native prerequisites");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        DocumentationResult baseline = await docs.IndexAsync();
        docs.Write(
            "content/en/arbitrary-contract.md",
            ValidationDocumentationFixture.Page("Adapter", "Use the managed adapter.\n\n```csharp\nint value = 1;\n```\n", "host")
        );
        DocumentationResult current = await docs.IndexAsync();
        using JsonDocument plan = await docs.PlanAsync(current, baseline);
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        JsonElement examples = Assert.Single(checks.Where(check => check.GetProperty("kind").GetString() == "documentation-examples"));
        Assert.Equal("native", examples.GetProperty("context").GetString());
        Assert.Equal(
            "task-adapter",
            Assert.Single(examples.GetProperty("documentation").GetProperty("pages").EnumerateArray()).GetString()
        );
        Assert.Contains("build:" + C_OWNER, examples.GetProperty("prerequisites").EnumerateArray().Select(item => item.GetString()));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ProductExamplesRequireTheirEvaluatedTargetWithoutPreparingForeignProducts(bool matchingTarget)
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
        const string C_OWNER = "src/Fixture/Product/Product.csproj";
        repository.Write(
            C_OWNER,
            ValidationRepository.Project(
                "<PropertyGroup><InnoProductId>editor</InnoProductId><InnoProductTarget>"
                + (matchingTarget ? "fixture-target" : "foreign-target") + "</InnoProductTarget></PropertyGroup>"
            )
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare actual product and host target prerequisites");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        DocumentationResult baseline = await docs.IndexAsync();
        docs.Write(
            "content/en/product-example.md",
            ValidationDocumentationFixture.Page("Product", "Compose this product.\n\n```csharp\nint value = 1;\n```\n", "host")
        );
        DocumentationResult current = await docs.IndexAsync();
        using JsonDocument plan = await docs.PlanAsync(current, baseline);
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        JsonElement example = Assert.Single(checks.Where(check => check.GetProperty("kind").GetString() == "documentation-examples"));
        Assert.Equal("task-product", Assert.Single(example.GetProperty("documentation").GetProperty("pages").EnumerateArray()).GetString());
        Assert.Equal(matchingTarget, plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        if (matchingTarget)
        {
            Assert.Equal(JsonValueKind.Null, example.GetProperty("exclusion").ValueKind);
            Assert.Contains("build:" + C_OWNER, example.GetProperty("prerequisites").EnumerateArray().Select(item => item.GetString()));
        }
        else
        {
            Assert.Contains("foreign-target", example.GetProperty("exclusion").GetString(), StringComparison.Ordinal);
            Assert.Contains("fixture-target", example.GetProperty("exclusion").GetString(), StringComparison.Ordinal);
            Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString() == "build");
        }
    }

    [Theory]
    [InlineData("```")]
    [InlineData("~~~")]
    public async Task ScriptFragmentsUseTheirExplicitProjectionContext(string fence)
    {
        using var repository = new ValidationRepository();
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Build/InnoEngine.Build.csproj", ValidationRepository.Project());
        repository.Write(
            "entry/InnoEngine.Editor/InnoEngine.Editor.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><InnoProductId>editor</InnoProductId><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
                + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"
            )
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare the script projection prerequisites");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        DocumentationResult baseline = await docs.IndexAsync();
        docs.Write(
            "content/en/arbitrary-script.md",
            ValidationDocumentationFixture.Page(
                "Example",
                "Read the projected contract.\n\n" + fence + "csharp\nusing InnoEngine;\n" + fence + "\n",
                "runtime-script"
            )
        );
        DocumentationResult current = await docs.IndexAsync();
        using JsonDocument plan = await docs.PlanAsync(current, baseline);
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        JsonElement examples = Assert.Single(checks.Where(check => check.GetProperty("kind").GetString() == "documentation-examples"));
        Assert.Equal("native", examples.GetProperty("context").GetString());
        Assert.Equal(
            "task-example",
            Assert.Single(examples.GetProperty("documentation").GetProperty("pages").EnumerateArray()).GetString()
        );
        Assert.Equal(
            "native",
            checks.Single(check => check.GetProperty("id").GetString() == "build:src/Example/Example.csproj")
                .GetProperty("context")
                .GetString()
        );
        Assert.Contains(
            "build:entry/InnoEngine.Build/InnoEngine.Build.csproj",
            examples.GetProperty("prerequisites").EnumerateArray().Select(item => item.GetString())
        );
        Assert.Contains(
            "build:entry/InnoEngine.Editor/InnoEngine.Editor.csproj",
            examples.GetProperty("prerequisites").EnumerateArray().Select(item => item.GetString())
        );
    }

    [Fact]
    public async Task ScriptExamplesPrepareTheirDeclaredProductWithoutAProductionSourceChange()
    {
        using var repository = new ValidationRepository();
        string properties = repository.PathOf("Product.props");
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Build/InnoEngine.Build.csproj", ValidationRepository.Project());
        repository.Write(
            "entry/InnoEngine.Editor/InnoEngine.Editor.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><InnoProductId>editor</InnoProductId><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
                + properties + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"
            )
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare product and build entry");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        DocumentationResult baseline = await docs.IndexAsync();
        docs.Write(
            "content/en/script-guide.md",
            ValidationDocumentationFixture.Page("Example", "Use the editor API.\n\n```csharp\nusing InnoEngine;\n```\n", "editor-script")
        );
        DocumentationResult current = await docs.IndexAsync();
        using JsonDocument plan = await docs.PlanAsync(current, baseline);
        Assert.Empty(Owners(plan));
        JsonElement examples = plan.RootElement
            .GetProperty("checks")
            .EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "documentation-examples:native");
        string[] prerequisites = examples.GetProperty("prerequisites").EnumerateArray().Select(value => value.GetString()!).ToArray();
        Assert.Contains("build:entry/InnoEngine.Build/InnoEngine.Build.csproj", prerequisites);
        Assert.Contains("build:entry/InnoEngine.Editor/InnoEngine.Editor.csproj", prerequisites);
        Assert.Equal(
            "fixture-target",
            plan.RootElement
                .GetProperty("contexts")
                .GetProperty("native")
                .GetProperty("properties")
                .GetProperty("InnoToolTarget")
                .GetString()
        );
    }
}
