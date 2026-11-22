using System;
using System.Collections.Generic;
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
using Inno.Tooling.Validation;

namespace Inno.Tooling.Validation.Tests;

[Collection(nameof(ValidationPlanFixture))]
public sealed class ValidationSelectionTests : ValidationPlanFixture
{
    [Fact]
    public async Task BuildDefinitionAffectsOnlyTheEntryThatImportsIt()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Optional.props", "<Project />");
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project("""
            <Import Project="Optional.props" Condition="'$(UseOptionalDefinition)' == 'true' and Exists('Optional.props')" />
            """)
        );
        repository.Write(
            "tests/Enabled/Enabled.csproj",
            ValidationRepository.Project("""
            <PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>domain</InnoValidationLayer></PropertyGroup>
            <ItemGroup><ProjectReference Include="../../src/Example/Example.csproj" AdditionalProperties="UseOptionalDefinition=true" /></ItemGroup>
            """)
        );
        repository.Write(
            "tools/Inno.Tooling.Validation/Inno.Tooling.Validation.csproj",
            ValidationRepository.Project("""
            <ItemGroup><ProjectReference Include="../../src/Example/Example.csproj" /></ItemGroup>
            """)
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Register distinct SDK entries");
        using JsonDocument baseline = await repository.PlanAsync();
        string previous = repository.PathOf("artifacts/previous.json");
        File.WriteAllText(previous, baseline.RootElement.GetRawText());
        repository.Write("src/Example/Optional.props", "<Project><PropertyGroup><ExampleValue>2</ExampleValue></PropertyGroup></Project>");
        using JsonDocument plan = await repository.PlanAsync();
        Dictionary<string, JsonElement> tests = plan.RootElement.GetProperty("checks")
            .EnumerateArray()
            .Where(check => check.GetProperty("kind").GetString() == "test")
            .ToDictionary(check => check.GetProperty("owner").GetString()!);
        Assert.Equal(JsonValueKind.Null, tests["tests/Enabled/Enabled.csproj"].GetProperty("exclusion").ValueKind);
        Assert.Equal(JsonValueKind.String, tests["tests/Example.Tests/Example.Tests.csproj"].GetProperty("exclusion").ValueKind);
        Assert.Contains(plan.RootElement.GetProperty("diagnostics").EnumerateArray(), reason =>
            reason.GetString()!.Contains("src/Example/Optional.props is consumed by", StringComparison.Ordinal));
        File.Delete(repository.PathOf("src/Example/Optional.props"));
        using JsonDocument removed = await repository.PlanAsync(baselinePlan: previous);
        Dictionary<string, JsonElement> afterDelete = removed.RootElement.GetProperty("checks")
            .EnumerateArray()
            .Where(check => check.GetProperty("kind").GetString() == "test")
            .ToDictionary(check => check.GetProperty("owner").GetString()!);
        Assert.Equal(JsonValueKind.Null, afterDelete["tests/Enabled/Enabled.csproj"].GetProperty("exclusion").ValueKind);
        Assert.Equal(JsonValueKind.String, afterDelete["tests/Example.Tests/Example.Tests.csproj"].GetProperty("exclusion").ValueKind);
    }

    [Fact]
    public async Task ConsolePresentationKeepsItsContractsWithoutSelectingBuildIntegration()
    {
        using var repository = new ValidationRepository(TestRepository.GetRoot(typeof(ValidationPlanFixture).Assembly));
        const string C_STYLE = "src/composition/editor/panels/Inno.Editor.Panel.Logging/Styling/ConsoleStyle.cs";
        string original = File.ReadAllText(repository.PathOf(C_STYLE));
        string changed = original.Replace("new(0.8f, 0.9f, 0.85f", "new(0.81f, 0.9f, 0.85f", StringComparison.Ordinal);
        Assert.NotEqual(original, changed);
        repository.Write(C_STYLE, changed);
        // Consume-only planning establishes scope without fetching source dependencies or preparing an Editor.
        string output = repository.PathOf("artifacts/console-selection");
        int status = await ValidationCommand.ExecuteAsync([
            "plan",
            "--engine-root",
            repository.PathOf("."),
            "--dotnet",
            repository.dotnet,
            "--base",
            "HEAD",
            "--output",
            output,
            "--backend-store",
            "artifacts/backend-store"
        ]);
        Assert.Equal(0, status);
        Assert.False(File.Exists(Path.Combine(output, "preparation.json")));
        using JsonDocument selection = JsonDocument.Parse(File.ReadAllText(Path.Combine(output, "plan.json")));
        string[] owners = selection.RootElement.GetProperty("owners").EnumerateArray().Select(value => value.GetString()!).ToArray();
        Assert.Contains("tests/composition/editor/framework/Inno.Editor.Settings.Tests/Inno.Editor.Settings.Tests.csproj", owners);
        Assert.DoesNotContain("tests/build/pipeline/Inno.Build.Tests/Inno.Build.Tests.csproj", owners);
        Assert.DoesNotContain("tests/build/pipeline/Inno.Build.Integration.Tests/Inno.Build.Integration.Tests.csproj", owners);
        Assert.DoesNotContain("tests/services/input/Inno.Input.Tests/Inno.Input.Tests.csproj", owners);
        Assert.False(Directory.Exists(repository.PathOf("artifacts/backend-store")));
    }

    [Fact]
    public async Task HostRoutingProofContainsOnlyExclusiveSourcesOfUnreferencedHostTests()
    {
        using var repository = new ValidationRepository();
        string otherHost = OperatingSystem.IsWindows() ? "macos" : "windows";
        string settings = $"<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>contract</InnoValidationLayer><InnoValidationEnvironment>{otherHost}</InnoValidationEnvironment></PropertyGroup>";
        repository.Write("tests/Foreign/Foreign.csproj", ValidationRepository.Project(settings));
        repository.Write("tests/Foreign/Private.cs", "class PrivateTest {}");
        repository.Write("tests/Foreign/Shared.cs", "class SharedTest {}");
        repository.Write("tests/Referenced/Referenced.csproj", ValidationRepository.Project(settings));
        repository.Write("tests/Referenced/Utility.cs", "class ReferencedTest {}");
        repository.Write(
            "tests/Consumer/Consumer.csproj",
            ValidationRepository.Project(
                "<ItemGroup><Compile Include=\"../Foreign/Shared.cs\"/><ProjectReference Include=\"../Referenced/Referenced.csproj\"/></ItemGroup>"
            )
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare independent and shared host test inputs");
        using JsonDocument plan = await repository.PlanAsync();
        string?[] excluded = plan.RootElement
            .GetProperty("hostExcludedSources")
            .EnumerateArray()
            .Select(static item => item.GetString())
            .ToArray();
        Assert.Contains("tests/Foreign/Private.cs", excluded);
        Assert.DoesNotContain("tests/Foreign/Shared.cs", excluded);
        Assert.DoesNotContain("tests/Referenced/Utility.cs", excluded);
    }

    [Fact]
    public async Task SharedTestExecutionChangesSelectAllTestOwnersAndInvalidateOnlyTheirExecutionInputs()
    {
        using var repository = new ValidationRepository();
        const string C_EXECUTOR = "tests/Invoke-IsolatedTests.ps1";
        const string C_UNRELATED = "tests/Unrelated.Tests/Unrelated.Tests.csproj";
        repository.Write(
            C_UNRELATED,
            ValidationRepository.Project(
                "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>contract</InnoValidationLayer></PropertyGroup>"
            )
        );
        repository.Write(C_EXECUTOR, "Write-Host 'before'");
        repository.Git("add", C_EXECUTOR, C_UNRELATED);
        repository.Git("commit", "-m", "Declare the shared test executor");
        using JsonDocument initial = await repository.PlanAsync();
        string original = initial.RootElement.GetProperty("inputs").GetProperty(C_EXECUTOR).GetString()!;
        repository.Write(C_EXECUTOR, "Write-Host 'after'");
        using JsonDocument changed = await repository.PlanAsync();
        Assert.False(changed.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains("tests/Example.Tests/Example.Tests.csproj", Owners(changed));
        Assert.Contains(C_UNRELATED, Owners(changed));
        Assert.DoesNotContain("src/Other/Other.csproj", Owners(changed));
        Assert.NotEqual(original, changed.RootElement.GetProperty("inputs").GetProperty(C_EXECUTOR).GetString());
        JsonElement[] checks = changed.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        Assert.All(checks.Where(check => check.GetProperty("kind").GetString() == "test"), test =>
        {
            Assert.Equal(JsonValueKind.Null, test.GetProperty("exclusion").ValueKind);
            Assert.Contains(C_EXECUTOR, test.GetProperty("inputs").EnumerateArray().Select(static value => value.GetString()));
        });
        Assert.All(checks.Where(check => check.GetProperty("kind").GetString() == "build"), build =>
            Assert.DoesNotContain(C_EXECUTOR, build.GetProperty("inputs").EnumerateArray().Select(static value => value.GetString())));
        Assert.DoesNotContain(
            C_EXECUTOR,
            changed.RootElement.GetProperty("inputSets").GetProperty("tooling").EnumerateArray().Select(static value => value.GetString())
        );
    }

    [Fact]
    public async Task AnotherHostOwnerDoesNotRequestNativePreparationAndUnknownHostsFail()
    {
        using var repository = new ValidationRepository();
        string other = OperatingSystem.IsWindows() ? "macos" : "windows";
        const string C_OWNER = "tests/Host.Tests/Host.Tests.csproj";
        string definition = ValidationRepository.Project(
            "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>native</InnoValidationLayer>"
            + "<InnoValidationEnvironment>" + other + "</InnoValidationEnvironment><InnoTestNativeProduct>editor</InnoTestNativeProduct></PropertyGroup>"
        );
        repository.Write(C_OWNER, definition);
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare native host requirement");
        repository.Write("tests/Host.Tests/Changed.cs", "// Actual host contract input.");
        using (JsonDocument plan = await repository.PlanAsync())
        {
            Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
            JsonElement check = plan.RootElement
                .GetProperty("checks")
                .EnumerateArray()
                .Single(item => item.GetProperty("id").GetString() == "test:" + C_OWNER);
            Assert.Contains("requires OS environment", check.GetProperty("exclusion").GetString());
        }
        string current = OperatingSystem.IsWindows() ? "windows" : OperatingSystem.IsMacOS() ? "macos" : "linux";
        repository.Write(
            C_OWNER,
            definition.Replace(
                ">" + other + "</InnoValidationEnvironment>",
                ">" + current + ";unknown</InnoValidationEnvironment>",
                StringComparison.Ordinal
            )
        );
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
    }

    [Fact]
    public async Task TrackedContentSelectsConsumersDespitePreservedTimestampAndCachedIndex()
    {
        using var repository = new ValidationRepository();
        const string C_PATH = "src/Example/Unicode 名称.cs";
        const string C_ORIGINAL = "namespace Example; public class Changed { public const int Value = 1; }\n";
        repository.Write(C_PATH, C_ORIGINAL);
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Track content probe");
        repository.Git("update-index", "--assume-unchanged", C_PATH);
        string path = repository.PathOf(C_PATH);
        DateTime timestamp = File.GetLastWriteTimeUtc(path);
        long length = new FileInfo(path).Length;
        repository.Write(C_PATH, C_ORIGINAL.Replace("= 1", "= 2", StringComparison.Ordinal));
        File.SetLastWriteTimeUtc(path, timestamp);
        Assert.Equal(length, new FileInfo(path).Length);
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Contains(C_PATH, Changes(plan));
        Assert.Contains("tests/Example.Tests/Example.Tests.csproj", Owners(plan));
    }

    [Fact]
    public async Task StagedChangesRemainSelectedWhenWorktreeMatchesHead()
    {
        using var repository = new ValidationRepository();
        const string C_PATH = "src/Example/Staged.cs";
        const string C_ORIGINAL = "namespace Example; public class Staged { public const int Value = 1; }\n";
        repository.Write(C_PATH, C_ORIGINAL);
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Track staged probe");
        repository.Write(C_PATH, C_ORIGINAL.Replace("= 1", "= 2", StringComparison.Ordinal));
        repository.Git("add", C_PATH);
        repository.Write(C_PATH, C_ORIGINAL);
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Contains(C_PATH, Changes(plan));
        Assert.Contains("tests/Example.Tests/Example.Tests.csproj", Owners(plan));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GitLineEndingConversionDoesNotSelectUnchangedOwners(bool committedCarriageReturns)
    {
        using var repository = new ValidationRepository();
        repository.Git("config", "core.autocrlf", "false");
        if (!committedCarriageReturns)
        {
            repository.Write(".gitattributes", "*.cs text eol=crlf\n");
        }
        repository.Write("src/Example/Unchanged.cs", "namespace Example;\r\npublic class Unchanged { }\r\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare checkout line endings");
        repository.Git("config", "core.autocrlf", "true");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Empty(Changes(plan));
        Assert.Empty(Owners(plan));
    }

    [Fact]
    public async Task EmptyCiContractDiscoveryCannotCertifySuccess()
    {
        using var repository = new ValidationRepository();
        repository.Write(".github/workflows/change.yml", "name: Changed workflow\n");
        Directory.CreateDirectory(repository.PathOf("tests/ci"));
        using JsonDocument plan = await repository.PlanAsync();
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        using JsonDocument results = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
        JsonElement contracts = results.RootElement
            .GetProperty("results")
            .EnumerateArray()
            .Single(result => result.GetProperty("id").GetString() == "ci-contracts");
        Assert.Equal("Failed", contracts.GetProperty("status").GetString());
        Assert.Contains("Required CI contract discovery is empty", contracts.GetProperty("detail").GetString(), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("tests/ci/Scope.Tests.ps1", false)]
    [InlineData("tests/ci/Invoke-ValidationGate.ps1", false)]
    [InlineData("tests/ci/Invoke-ValidationGate.Tests.ps1", false)]
    [InlineData("tests/ci/Test-AgentRules.ps1", false)]
    [InlineData("tests/ci/Prepare-Unknown.ps1", true)]
    [InlineData("tests/ci/nested/Scope.Tests.ps1", true)]
    public async Task StaticContractOwnersFollowActualDiscoveryWithoutSelectingProductBuilds(
        string path,
        bool unknown
    ) {
        using var repository = new ValidationRepository();
        repository.Write(path, "Write-Output 'Changed contract'\n");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Equal(unknown, plan.RootElement.GetProperty("full").GetBoolean());
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        JsonElement contract = Assert.Single(checks.Where(check => check.GetProperty("id").GetString() == "ci-contracts"));
        if (path.Contains("Invoke-ValidationGate", StringComparison.Ordinal))
        {
            Assert.Equal("gate-contracts", contract.GetProperty("kind").GetString());
            Assert.False(plan.RootElement.GetProperty("backendRequired").GetBoolean());
        }
        Assert.Contains(path, contract.GetProperty("inputs").EnumerateArray().Select(input => input.GetString()));
        if (!unknown)
        {
            Assert.Empty(Owners(plan));
            Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString() == "build");
            Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        }
        else
        {
            Assert.Contains(
                plan.RootElement.GetProperty("diagnostics").EnumerateArray(),
                diagnostic => diagnostic.GetString()!.Contains("Unknown or shared input", StringComparison.Ordinal)
            );
        }
    }

    [Fact]
    public async Task StaticContractRecognitionDoesNotHideAnExplicitProjectConsumer()
    {
        using var repository = new ValidationRepository();
        const string C_SCRIPT = "tests/ci/Invoke-ValidationGate.ps1";
        const string C_OWNER = "tests/Example.Tests/Example.Tests.csproj";
        repository.Write(C_SCRIPT, "Write-Output 'Original'\n");
        repository.Write(
            C_OWNER,
            File.ReadAllText(repository.PathOf(C_OWNER)).Replace(
                "</Project>",
                "<ItemGroup><InnoValidationInput Include=\"../ci/Invoke-ValidationGate.ps1\" /></ItemGroup></Project>",
                StringComparison.Ordinal
            )
        );
        repository.Git("add", C_SCRIPT, C_OWNER);
        repository.Git("commit", "-m", "Declare the contract's additional runtime consumer");
        repository.Write(C_SCRIPT, "Write-Output 'Changed'\n");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.False(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains(C_OWNER, Owners(plan));
        Assert.DoesNotContain(
            C_SCRIPT,
            plan.RootElement.GetProperty("hostExcludedSources").EnumerateArray().Select(item => item.GetString())
        );
        Assert.Contains(
            plan.RootElement.GetProperty("checks").EnumerateArray(),
            check => (
                check.GetProperty("id").GetString() == "build:" + C_OWNER
                && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null
            )
        );
    }

    [Theory]
    [InlineData("tests/product/fixtures/editor-ui/ManagedMemorySample.editor.cs")]
    [InlineData("tests/product/Invoke-EditorUiProductJourney.ps1")]
    public async Task JourneySourceChangesSelectCiContractsAndInvalidateTheirEvidence(string path)
    {
        using var repository = new ValidationRepository();
        repository.Write(path, "// Before\n");
        repository.Git("add", path);
        repository.Git("commit", "-m", "Declare shared journey input");
        using JsonDocument initial = await repository.PlanAsync();
        Assert.DoesNotContain(
            initial.RootElement.GetProperty("checks").EnumerateArray(),
            check => check.GetProperty("id").GetString() == "ci-contracts"
        );
        repository.Write(path, "// After!\n");
        using JsonDocument changed = await repository.PlanAsync();
        JsonElement contracts = changed.RootElement.GetProperty("checks")
            .EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "ci-contracts");
        Assert.Contains(path, contracts.GetProperty("inputs").EnumerateArray().Select(static input => input.GetString()));
        Assert.NotEqual(
            initial.RootElement.GetProperty("inputs").GetProperty(path).GetString(),
            changed.RootElement.GetProperty("inputs").GetProperty(path).GetString()
        );
        Assert.DoesNotContain(
            path,
            changed.RootElement.GetProperty("inputSets")
                .GetProperty("tooling")
                .EnumerateArray()
                .Select(static input => input.GetString())
        );
    }

    [Fact]
    public async Task AddedMovedAndDeletedInputsSelectTheirRealConsumers()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/New.cs", "namespace Example; public sealed class New { }");
        using JsonDocument added = await repository.PlanAsync();
        Assert.Contains("src/Example/New.cs", Changes(added));
        Assert.Contains("tests/Example.Tests/Example.Tests.csproj", Owners(added));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Add source");
        using JsonDocument baseline = await repository.PlanAsync();
        string baselinePlan = repository.PathOf("artifacts/prior.json");
        File.WriteAllText(baselinePlan, baseline.RootElement.GetRawText());
        File.Move(repository.PathOf("src/Example/New.cs"), repository.PathOf("src/Other/Moved.cs"));
        using JsonDocument moved = await repository.PlanAsync(baselinePlan: baselinePlan);
        Assert.False(moved.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains("src/Example/New.cs", Changes(moved));
        Assert.Contains("src/Other/Moved.cs", Changes(moved));
        Assert.Contains("src/Example/Example.csproj", Owners(moved));
        Assert.Contains("src/Other/Other.csproj", Owners(moved));
    }

    [Fact]
    public async Task ImportsAndDynamicInputsSelectOwnersWithoutASecondProjectList()
    {
        using var repository = new ValidationRepository();
        repository.Write("Shared.props", "<Project><PropertyGroup><DefineConstants>CHANGED</DefineConstants></PropertyGroup></Project>");
        using JsonDocument imports = await repository.PlanAsync();
        Assert.Contains("src/Example/Example.csproj", Owners(imports));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Change import");
        repository.Write("fixtures/plugin.txt", "changed dynamic discovery input");
        using JsonDocument dynamicInput = await repository.PlanAsync();
        Assert.Contains("tests/Example.Tests/Example.Tests.csproj", Owners(dynamicInput));
    }

    [Fact]
    public async Task TestImplementationChangesKeepBehaviorWithoutRecheckingUnchangedProductionApi()
    {
        using var repository = new ValidationRepository();
        repository.Write("tests/Example.Tests/Changed.cs", "namespace Example.Tests; public sealed class Changed { }");
        using JsonDocument tests = await repository.PlanAsync();
        Assert.True(tests.RootElement.GetProperty("portableTestChanges").GetBoolean());
        JsonElement api = Assert.Single(
            tests.RootElement.GetProperty("checks").EnumerateArray().Where(check => check.GetProperty("kind").GetString() == "api")
        );
        Assert.False(string.IsNullOrWhiteSpace(api.GetProperty("exclusion").GetString()));
        Assert.Contains(tests.RootElement.GetProperty("checks").EnumerateArray(), check => (
            check.GetProperty("kind").GetString() == "test"
            && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null
        ));
        repository.Write("src/Example/Changed.cs", "namespace Example; public sealed class Changed { }");
        using JsonDocument production = await repository.PlanAsync();
        Assert.False(production.RootElement.GetProperty("portableTestChanges").GetBoolean());
        api = Assert.Single(
            production.RootElement.GetProperty("checks").EnumerateArray().Where(check => check.GetProperty("kind").GetString() == "api")
        );
        Assert.Equal(JsonValueKind.Null, api.GetProperty("exclusion").ValueKind);
        Assert.Contains(api.GetProperty("projects").EnumerateArray(), project => project.GetString() == "src/Example/Example.csproj");
        Assert.DoesNotContain(
            api.GetProperty("projects").EnumerateArray(),
            project => project.GetString() == "tests/Example.Tests/Example.Tests.csproj"
        );
    }

    [Fact]
    public async Task ReferencePropertyVariantsPreserveConditionalSourceInputsAndNativeRequirements()
    {
        using var repository = new ValidationRepository();
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project("""
            <ItemGroup Condition="'$(Flavor)' == 'selected'"><Compile Include="../../fixtures/Selected.cs" /></ItemGroup>
            <PropertyGroup Condition="'$(Flavor)' == 'native'"><InnoProductId>player</InnoProductId></PropertyGroup>
            """)
        );
        repository.Write(
            "tests/Example.Tests/Example.Tests.csproj",
            ValidationRepository.Project("""
            <PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>domain</InnoValidationLayer></PropertyGroup>
            <ItemGroup><ProjectReference Include="../../src/Example/Example.csproj" AdditionalProperties="Flavor=selected" /></ItemGroup>
            """)
        );
        repository.Write("fixtures/Selected.cs", "namespace Example; public sealed class Selected { }");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare conditional reference inputs");
        repository.Write("fixtures/Selected.cs", "namespace Example; public sealed class Selected { public int value; }");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.False(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains("src/Example/Example.csproj", Owners(plan));
        Assert.Contains("tests/Example.Tests/Example.Tests.csproj", Owners(plan));
        string tests = repository.PathOf("tests/Example.Tests/Example.Tests.csproj");
        File.WriteAllText(tests, File.ReadAllText(tests).Replace("Flavor=selected", "Flavor=native", StringComparison.Ordinal));
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
    }

    [Fact]
    public async Task SolutionTargetsInvalidateCompilationAndExpandSharedAcceptance()
    {
        using var repository = new ValidationRepository();
        repository.Write("Directory.Solution.targets", "<Project />");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.True(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains(
            plan.RootElement.GetProperty("inputSets").GetProperty("tooling").EnumerateArray(),
            input => input.GetString() == "Directory.Solution.targets"
        );
        Assert.Contains(plan.RootElement.GetProperty("checks").EnumerateArray(), check => (
            check.GetProperty("kind").GetString() == "build" && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null
        ));
    }

    [Fact]
    public async Task UnknownInputsExpandAndReportTheirReason()
    {
        using var repository = new ValidationRepository();
        repository.Write("unknown.input", "unknown relation");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.True(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains(
            plan.RootElement.GetProperty("diagnostics").EnumerateArray(),
            value => value.GetString()!.Contains("unknown.input", StringComparison.Ordinal)
        );
    }

    [Fact]
    public async Task EmptyBehaviorSelectionCannotCertifyProductionChanges()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Other/Other.cs", "namespace Other; public sealed class Other { }");
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
    }
}
