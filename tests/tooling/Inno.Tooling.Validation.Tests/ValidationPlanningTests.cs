using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Tooling.Validation;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationPlanningTests
{
    [Fact]
    public async Task SharedTestExecutionChangesSelectAllTestOwnersAndInvalidateOnlyTheirExecutionInputs()
    {
        using var repository = new ValidationRepository();
        const string C_EXECUTOR = "tests/Invoke-IsolatedTests.ps1";
        const string C_PERFORMANCE = "tests/Performance.Tests/Performance.Tests.csproj";
        repository.Write(C_PERFORMANCE, ValidationRepository.Project(
            "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>performance</InnoValidationLayer></PropertyGroup>"));
        repository.Write(C_EXECUTOR, "Write-Host 'before'");
        repository.Git("add", C_EXECUTOR, C_PERFORMANCE);
        repository.Git("commit", "-m", "Declare the shared test executor");
        using JsonDocument initial = await repository.PlanAsync();
        string original = initial.RootElement.GetProperty("inputs").GetProperty(C_EXECUTOR).GetString()!;
        repository.Write(C_EXECUTOR, "Write-Host 'after'");
        using JsonDocument changed = await repository.PlanAsync();
        Assert.False(changed.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains("tests/Example.Tests/Example.Tests.csproj", Owners(changed));
        Assert.Contains(C_PERFORMANCE, Owners(changed));
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
        Assert.DoesNotContain(C_EXECUTOR, changed.RootElement.GetProperty("inputSets").GetProperty("tooling").EnumerateArray().Select(static value => value.GetString()));
    }

    [Fact]
    public async Task PerformanceOwnersAreExplicitlyExcludedUntilTheirExecutionInputsChange()
    {
        using var repository = new ValidationRepository();
        const string C_OWNER = "tests/Cost.Tests/Cost.Tests.csproj";
        repository.Write(C_OWNER, ValidationRepository.Project("""
            <PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>performance</InnoValidationLayer></PropertyGroup>
            <ItemGroup><ProjectReference Include="../../src/Example/Example.csproj" /></ItemGroup>
            """));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare cost sampling owner");
        repository.Write("src/Example/Changed.cs", "namespace Example; public class Changed { }");
        using (JsonDocument plan = await repository.PlanAsync())
        {
            JsonElement check = plan.RootElement.GetProperty("checks").EnumerateArray().Single(item => item.GetProperty("id").GetString() == "test:" + C_OWNER);
            Assert.Contains("Performance sampling requires", check.GetProperty("exclusion").GetString());
        }
        repository.Write("build/Changed.props", "<Project />");
        using (JsonDocument plan = await repository.PlanAsync())
        {
            JsonElement check = plan.RootElement.GetProperty("checks").EnumerateArray().Single(item => item.GetProperty("id").GetString() == "test:" + C_OWNER);
            Assert.Equal(JsonValueKind.Null, check.GetProperty("exclusion").ValueKind);
            Assert.False(check.GetProperty("reusable").GetBoolean());
        }
    }

    [Fact]
    public async Task AnotherHostOwnerDoesNotRequestNativePreparationAndUnknownHostsFail()
    {
        using var repository = new ValidationRepository();
        string other = OperatingSystem.IsWindows() ? "macos" : "windows";
        const string C_OWNER = "tests/Host.Tests/Host.Tests.csproj";
        string definition = ValidationRepository.Project("<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>native</InnoValidationLayer>"
            + "<InnoValidationEnvironment>" + other + "</InnoValidationEnvironment><InnoTestNativeProduct>editor</InnoTestNativeProduct></PropertyGroup>");
        repository.Write(C_OWNER, definition);
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare native host requirement");
        repository.Write("tests/Host.Tests/Changed.cs", "// Actual host contract input.");
        using (JsonDocument plan = await repository.PlanAsync())
        {
            Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
            JsonElement check = plan.RootElement.GetProperty("checks").EnumerateArray().Single(item => item.GetProperty("id").GetString() == "test:" + C_OWNER);
            Assert.Contains("requires OS environment", check.GetProperty("exclusion").GetString());
        }
        string current = OperatingSystem.IsWindows() ? "windows" : OperatingSystem.IsMacOS() ? "macos" : "linux";
        repository.Write(C_OWNER, definition.Replace(">" + other + "</InnoValidationEnvironment>", ">" + current + ";unknown</InnoValidationEnvironment>", StringComparison.Ordinal));
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
            repository.Write(".gitattributes", "*.cs text eol=crlf\n");
        repository.Write("src/Example/Unchanged.cs", "namespace Example;\r\npublic class Unchanged { }\r\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare checkout line endings");
        repository.Git("config", "core.autocrlf", "true");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Empty(Changes(plan));
        Assert.Empty(Owners(plan));
    }

    [Fact]
    public async Task SdkSelectedReferencePackContentInvalidatesEvidenceDespitePreservedTimestamp()
    {
        using var repository = new ValidationRepository();
        using var external = new ValidationRepository();
        string packs = external.PathOf("packs");
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project(
            "<PropertyGroup><NetCoreTargetingPackRoot>" + System.Security.SecurityElement.Escape(packs)
            + "</NetCoreTargetingPackRoot></PropertyGroup>"));
        using JsonDocument initial = await repository.PlanAsync();
        JsonElement owner = initial.RootElement.GetProperty("contexts").GetProperty("managed").GetProperty("sdkGraph").EnumerateArray()
            .Single(project => project.GetProperty("Path").GetString() == repository.PathOf("src/Example/Example.csproj"));
        string path = Path.Combine(Assert.Single(owner.GetProperty("FrameworkPacks").EnumerateArray()).GetString()!, "reference.dll");
        external.Write(path, "first");
        using JsonDocument plan = await repository.PlanAsync();
        DateTime timestamp = File.GetLastWriteTimeUtc(path);
        external.Write(path, "other");
        File.SetLastWriteTimeUtc(path, timestamp);
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf("artifacts/unread-evidence.json")));
        Assert.Contains("another SDK or execution environment", failure.Message, StringComparison.Ordinal);
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
        JsonElement contracts = results.RootElement.GetProperty("results").EnumerateArray().Single(result => result.GetProperty("id").GetString() == "ci-contracts");
        Assert.Equal("Failed", contracts.GetProperty("status").GetString());
        Assert.Contains("Required CI contract discovery is empty", contracts.GetProperty("detail").GetString(), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("tests/Example.Tests/Changed.cs", "global using System;")]
    [InlineData("docs/Example.cs.txt", "global using System;")]
    [InlineData("Policy.props", "<Project><PropertyGroup><ImplicitUsings>enable</ImplicitUsings></PropertyGroup></Project>")]
    public async Task RepositoryWideSourcePolicyInputsParticipateInEvidenceIdentity(
        string path,
        string content
    ) {
        using var repository = new ValidationRepository();
        repository.Write(path, content);
        using JsonDocument plan = await repository.PlanAsync();
        JsonElement source = plan.RootElement.GetProperty("checks").EnumerateArray().Single(check => check.GetProperty("id").GetString() == "architecture");
        Assert.Contains(source.GetProperty("inputs").EnumerateArray(), input => input.GetString() == path);
        Assert.True(plan.RootElement.GetProperty("inputs").TryGetProperty(path, out _));
    }

    [Fact]
    public async Task ExternalSourceContentInvalidatesEvidenceEvenWhenLengthAndTimestampMatch()
    {
        using var repository = new ValidationRepository();
        using var external = new ValidationRepository();
        string path = external.PathOf("Linked.cs");
        const string original = "namespace Example; public class Linked { public const int Value = 1; }";
        external.Write("Linked.cs", original);
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project(
            "<ItemGroup><Compile Include=\"" + System.Security.SecurityElement.Escape(path) + "\" /></ItemGroup>"));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare external source input");
        using JsonDocument plan = await repository.PlanAsync();
        DateTime timestamp = File.GetLastWriteTimeUtc(path);
        external.Write("Linked.cs", original.Replace("= 1", "= 2", StringComparison.Ordinal));
        File.SetLastWriteTimeUtc(path, timestamp);
        Assert.Equal(original.Length, new FileInfo(path).Length);
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf("artifacts/unread-evidence.json")));
        Assert.Contains("another SDK or execution environment", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExternalImportContentInvalidatesEvidenceEvenWhenLengthAndTimestampMatch()
    {
        using var repository = new ValidationRepository();
        using var external = new ValidationRepository();
        string path = external.PathOf("Shared.props");
        const string original = "<Project><PropertyGroup><WarningLevel>4</WarningLevel></PropertyGroup></Project>";
        external.Write("Shared.props", original);
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project(
            "<Import Project=\"" + System.Security.SecurityElement.Escape(path) + "\" />"));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare external SDK import");
        using JsonDocument plan = await repository.PlanAsync();
        DateTime timestamp = File.GetLastWriteTimeUtc(path);
        external.Write("Shared.props", original.Replace(">4<", ">3<", StringComparison.Ordinal));
        File.SetLastWriteTimeUtc(path, timestamp);
        Assert.Equal(original.Length, new FileInfo(path).Length);
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf("artifacts/unread-evidence.json")));
        Assert.Contains("another SDK or execution environment", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ClosingFreshnessRejectsBothSourceAndExternalChangesDuringChecks()
    {
        using var repository = new ValidationRepository();
        using var external = new ValidationRepository();
        const string original = "<Project><PropertyGroup><WarningLevel>4</WarningLevel></PropertyGroup></Project>";
        external.Write("Shared.props", original);
        repository.Write("Shared.props", original);
        string dependency = external.PathOf("Shared.props");
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project(
            "<Import Project=\"" + System.Security.SecurityElement.Escape(dependency) + "\" />"));
        string script = string.Join('\n', new[] { repository.PathOf("Shared.props"), dependency }.Select(path =>
            "$path = '" + path.Replace("'", "''", StringComparison.Ordinal) + "'; "
            + "$stamp = [IO.File]::GetLastWriteTimeUtc($path); "
            + "[IO.File]::WriteAllText($path, [IO.File]::ReadAllText($path).Replace('>4<', '>3<')); "
            + "[IO.File]::SetLastWriteTimeUtc($path, $stamp)"));
        repository.Write("tests/ci/Test-AgentRules.ps1", script);
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare check-time source and environment mutations");
        using JsonDocument plan = await repository.PlanAsync();
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        using JsonDocument evidence = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
        JsonElement[] results = evidence.RootElement.GetProperty("results").EnumerateArray().ToArray();
        Assert.Contains(results, result => result.GetProperty("id").GetString() == "instructions"
            && result.GetProperty("status").GetString() == "Passed");
        foreach (string id in new[] { "input-stability", "environment-stability" })
            Assert.Contains(results, result => result.GetProperty("id").GetString() == id
                && result.GetProperty("status").GetString() == "Failed");
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
        File.Move(repository.PathOf("src/Example/New.cs"), repository.PathOf("src/Other/Moved.cs"));
        using JsonDocument moved = await repository.PlanAsync();
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
    public async Task DocumentationDoesNotSelectNativeOrManagedBuilds()
    {
        using var repository = new ValidationRepository();
        repository.Write("docs/README.md", "# Changed documentation\n");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Empty(Owners(plan));
        Assert.DoesNotContain(plan.RootElement.GetProperty("checks").EnumerateArray(), check => check.GetProperty("kind").GetString() == "build");
        JsonElement wiki = plan.RootElement.GetProperty("checks").EnumerateArray().Single(check => check.GetProperty("kind").GetString() == "wiki");
        Assert.Contains(wiki.GetProperty("prerequisites").EnumerateArray(), prerequisite => prerequisite.GetString() == "documentation");
    }

    [Fact]
    public async Task SourceSelectedWikiRequiresStructureEvidenceIncludingProjectAndLinkTargets()
    {
        using var repository = new ValidationRepository();
        repository.Write("docs/Example.md", "# Example\n");
        repository.Write("src/Example/Existing.cs", "namespace Example; public class Existing { }\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare project documentation and link target");
        repository.Write("src/Example/Changed.cs", "namespace Example; public class Changed { }\n");
        using JsonDocument plan = await repository.PlanAsync();
        JsonElement structure = plan.RootElement.GetProperty("checks").EnumerateArray().Single(check => check.GetProperty("id").GetString() == "documentation");
        Assert.Contains(structure.GetProperty("inputs").EnumerateArray(), input => input.GetString() == "src/Example/Example.csproj");
        Assert.Contains(structure.GetProperty("inputs").EnumerateArray(), input => input.GetString() == "src/Example/Existing.cs");
        JsonElement wiki = plan.RootElement.GetProperty("checks").EnumerateArray().Single(check => check.GetProperty("kind").GetString() == "wiki");
        Assert.Contains(wiki.GetProperty("prerequisites").EnumerateArray(), prerequisite => prerequisite.GetString() == "documentation");
    }

    [Fact]
    public async Task TestImplementationChangesKeepBehaviorWithoutRecheckingUnchangedProductionApi()
    {
        using var repository = new ValidationRepository();
        repository.Write("tests/Example.Tests/Changed.cs", "namespace Example.Tests; public sealed class Changed { }");
        using JsonDocument tests = await repository.PlanAsync();
        JsonElement api = Assert.Single(tests.RootElement.GetProperty("checks").EnumerateArray().Where(check => check.GetProperty("kind").GetString() == "api"));
        Assert.False(string.IsNullOrWhiteSpace(api.GetProperty("exclusion").GetString()));
        Assert.Contains(tests.RootElement.GetProperty("checks").EnumerateArray(), check => check.GetProperty("kind").GetString() == "test"
            && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null);
        repository.Write("src/Example/Changed.cs", "namespace Example; public sealed class Changed { }");
        using JsonDocument production = await repository.PlanAsync();
        api = Assert.Single(production.RootElement.GetProperty("checks").EnumerateArray().Where(check => check.GetProperty("kind").GetString() == "api"));
        Assert.Equal(JsonValueKind.Null, api.GetProperty("exclusion").ValueKind);
        Assert.Contains(api.GetProperty("projects").EnumerateArray(), project => project.GetString() == "src/Example/Example.csproj");
        Assert.DoesNotContain(api.GetProperty("projects").EnumerateArray(), project => project.GetString() == "tests/Example.Tests/Example.Tests.csproj");
    }

    [Fact]
    public async Task ReferencePropertyVariantsPreserveConditionalSourceInputsAndNativeRequirements()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project("""
            <ItemGroup Condition="'$(Flavor)' == 'selected'"><Compile Include="../../fixtures/Selected.cs" /></ItemGroup>
            <PropertyGroup Condition="'$(Flavor)' == 'native'"><InnoProductId>player</InnoProductId></PropertyGroup>
            """));
        repository.Write("tests/Example.Tests/Example.Tests.csproj", ValidationRepository.Project("""
            <PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>domain</InnoValidationLayer></PropertyGroup>
            <ItemGroup><ProjectReference Include="../../src/Example/Example.csproj" AdditionalProperties="Flavor=selected" /></ItemGroup>
            """));
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
    public async Task UnknownInputsExpandAndReportTheirReason()
    {
        using var repository = new ValidationRepository();
        repository.Write("unknown.input", "unknown relation");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.True(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains(plan.RootElement.GetProperty("diagnostics").EnumerateArray(), value => value.GetString()!.Contains("unknown.input", StringComparison.Ordinal));
    }

    [Fact]
    public async Task DomainSuiteCannotTransitivelyReachNativePreparation()
    {
        using var repository = new ValidationRepository();
        repository.Write("backends/Example/native/Native/Native.csproj", ValidationRepository.Project("<PropertyGroup><BindGenGeneratedBindings>true</BindGenGeneratedBindings></PropertyGroup>"));
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project("<ItemGroup><ProjectReference Include=\"../../backends/Example/native/Native/Native.csproj\" /></ItemGroup>"));
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
    }

    [Fact]
    public async Task EmptyBehaviorSelectionCannotCertifyProductionChanges()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Other/Other.cs", "namespace Other; public sealed class Other { }");
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
    }

    [Fact]
    public async Task AManagedInteropOwnerDoesNotBecomeNativePreparationBecauseOfItsDirectoryName()
    {
        using var repository = new ValidationRepository();
        repository.Write("backends/Example/native/Loader/Loader.csproj", ValidationRepository.Project());
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project(
            "<ItemGroup><ProjectReference Include=\"../../backends/Example/native/Loader/Loader.csproj\" /></ItemGroup>"));
        using JsonDocument plan = await repository.PlanAsync();
        Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        JsonElement domain = plan.RootElement.GetProperty("checks").EnumerateArray().Single(check => check.GetProperty("id").GetString() == "test:tests/Example.Tests/Example.Tests.csproj");
        Assert.Equal("managed", domain.GetProperty("context").GetString());
    }

    [Fact]
    public async Task MissingOrStaleResultsCannotBecomeGreenEvidence()
    {
        using var repository = new ValidationRepository();
        using JsonDocument plan = await repository.PlanAsync();
        string evidence = repository.PathOf("artifacts/empty.json");
        File.WriteAllText(evidence, JsonSerializer.Serialize(new
        {
            commit = plan.RootElement.GetProperty("commit").GetString(),
            tree = plan.RootElement.GetProperty("tree").GetString(),
            environment = plan.RootElement.GetProperty("environment").GetString(),
            started = DateTimeOffset.UtcNow,
            results = Array.Empty<object>()
        }));
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(evidence));
        repository.Write("tests/Example.Tests/Changed.cs", "namespace Example.Tests; public sealed class Changed { }");
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(evidence));
    }

    [Fact]
    public async Task BaseAdvancementUsesTheMergeBaseOfTheActualCandidate()
    {
        using var repository = new ValidationRepository();
        repository.Git("checkout", "-b", "feature");
        repository.Write("src/Example/Feature.cs", "namespace Example; public sealed class Feature { }");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Add feature");
        repository.Git("checkout", "master");
        repository.Write("docs/Base.md", "# Base branch advancement\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Advance base");
        repository.Git("checkout", "feature");
        repository.Git("merge", "master", "--no-edit");
        using JsonDocument plan = await repository.PlanAsync("master");
        Assert.Contains("src/Example/Feature.cs", Changes(plan));
        Assert.DoesNotContain("docs/Base.md", Changes(plan));
        Assert.Contains("docs/Base.md", plan.RootElement.GetProperty("inputs").EnumerateObject().Select(static property => property.Name));
    }

    [Fact]
    public async Task NativeContextUsesTheDeclaredDevelopmentProductAndKeepsDomainOwnersIndependent()
    {
        using var repository = new ValidationRepository();
        string properties = repository.PathOf("Product.props");
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Editor/InnoEngine.Editor.csproj", ValidationRepository.Project(
            "<PropertyGroup><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
            + properties + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"));
        repository.Write("backends/Example/native/Native/Native.csproj", ValidationRepository.Project("<PropertyGroup><BindGenGeneratedBindings>true</BindGenGeneratedBindings></PropertyGroup>"));
        repository.Write("backends/Example/tests/Native.Tests/Native.Tests.csproj", ValidationRepository.Project(
            "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>native</InnoValidationLayer></PropertyGroup>"
            + "<ItemGroup><ProjectReference Include=\"../../native/Native/Native.csproj\" /></ItemGroup>"));
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Equal("fixture-target", plan.RootElement.GetProperty("contexts").GetProperty("native").GetProperty("properties").GetProperty("InnoNativeTarget").GetString());
        JsonElement domain = plan.RootElement.GetProperty("checks").EnumerateArray().Single(check => check.GetProperty("id").GetString() == "test:tests/Example.Tests/Example.Tests.csproj");
        Assert.Equal("managed", domain.GetProperty("context").GetString());
        JsonElement native = plan.RootElement.GetProperty("checks").EnumerateArray().Single(check => check.GetProperty("id").GetString() == "test:backends/Example/tests/Native.Tests/Native.Tests.csproj");
        Assert.Equal("native", native.GetProperty("context").GetString());
        JsonElement contexts = plan.RootElement.GetProperty("contexts");
        string Output(string context) => contexts.GetProperty(context).GetProperty("sdkGraph").EnumerateArray()
            .Single(node => node.GetProperty("Path").GetString() == repository.PathOf("src/Example/Example.csproj"))
            .GetProperty("Properties").GetProperty("TargetPath").GetString()!;
        Assert.NotEqual(Output("managed"), Output("native"));
        Assert.StartsWith(contexts.GetProperty("native").GetProperty("properties").GetProperty("ArtifactsPath").GetString()!, Output("native"), StringComparison.Ordinal);
        string[] sequence = plan.RootElement.GetProperty("checks").EnumerateArray().Select(check => check.GetProperty("id").GetString()!).ToArray();
        const string C_NATIVE_BUILD = "build:backends/Example/native/Native/Native.csproj";
        Assert.True(Array.IndexOf(sequence, domain.GetProperty("id").GetString()!) < Array.IndexOf(sequence, C_NATIVE_BUILD));
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        using JsonDocument results = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
        JsonElement[] recorded = results.RootElement.GetProperty("results").EnumerateArray().ToArray();
        Assert.Equal(sequence, recorded.Select(result => result.GetProperty("id").GetString()!)
            .Where(id => !id.StartsWith("test-execution:", StringComparison.Ordinal)).ToArray());
        Assert.All(recorded.Where(result => result.GetProperty("id").GetString()!.StartsWith("test-execution:", StringComparison.Ordinal)),
            result => Assert.Equal("Failed", result.GetProperty("status").GetString()));
        Assert.Equal("Blocked", results.RootElement.GetProperty("results").EnumerateArray().Single(result => result.GetProperty("id").GetString() == C_NATIVE_BUILD)
            .GetProperty("status").GetString());
    }

    [Fact]
    public async Task NativeDocumentationFragmentsUseTheirOwnerExecutionContext()
    {
        using var repository = new ValidationRepository();
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Editor/InnoEngine.Editor.csproj", ValidationRepository.Project(
            "<PropertyGroup><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
            + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"));
        const string C_OWNER = "backends/Example/native/Native/Native.csproj";
        repository.Write(C_OWNER, ValidationRepository.Project("<PropertyGroup><BindGenGeneratedBindings>true</BindGenGeneratedBindings></PropertyGroup>"));
        repository.Write("docs/Native.md", "<!-- api-include: Native/arbitrary-contract.md -->\n");
        repository.Write("docs/Native/arbitrary-contract.md", "# Native contract\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare a native project with split documentation");
        repository.Write("docs/Native/arbitrary-contract.md", "# Native contract\nUpdated usage constraints.\n");
        using JsonDocument plan = await repository.PlanAsync();
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        JsonElement wiki = Assert.Single(checks.Where(check => check.GetProperty("kind").GetString() == "wiki"));
        Assert.Equal("native", wiki.GetProperty("context").GetString());
        Assert.Contains("docs/Native/arbitrary-contract.md", wiki.GetProperty("inputs").EnumerateArray().Select(item => item.GetString()));
        Assert.Contains("docs/Native.md", wiki.GetProperty("inputs").EnumerateArray().Select(item => item.GetString()));
        Assert.Contains("build:" + C_OWNER, wiki.GetProperty("prerequisites").EnumerateArray().Select(item => item.GetString()));
    }

    [Fact]
    public async Task ExecutionFailureRetainsAnExactPlanReceiptAndCannotCertifySuccess()
    {
        using var repository = new ValidationRepository();
        using JsonDocument plan = await repository.PlanAsync();
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        using JsonDocument results = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
        Assert.Contains(results.RootElement.GetProperty("results").EnumerateArray(), result => result.GetProperty("status").GetString() == "Failed");
        Assert.True(results.RootElement.GetProperty("completed").GetBoolean());
        string identity = Convert.ToHexStringLower(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(repository.PathOf("artifacts/run/plan.json"))));
        Assert.Equal(identity, results.RootElement.GetProperty("planIdentity").GetString());
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf("artifacts/run/results.json")));
    }

    [Theory]
    [InlineData("```")]
    [InlineData("~~~")]
    public async Task ScriptFragmentsKeepTheirWholeProjectDocumentationInTheProjectionContext(string fence)
    {
        using var repository = new ValidationRepository();
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Build/InnoEngine.Build.csproj", ValidationRepository.Project());
        repository.Write("entry/InnoEngine.Editor/InnoEngine.Editor.csproj", ValidationRepository.Project(
            "<PropertyGroup><InnoProductId>editor</InnoProductId><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
            + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare the script projection prerequisites");
        repository.Write("docs/Example.md", "# Example\n<!-- api-include: Example/api.md -->\n");
        repository.Write("docs/Example/api.md", "# API\n" + fence + "csharp\nusing InnoEngine;\n" + fence + "\n");
        using JsonDocument plan = await repository.PlanAsync();
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        JsonElement wiki = Assert.Single(checks.Where(check => check.GetProperty("kind").GetString() == "wiki"));
        Assert.Equal("native", wiki.GetProperty("context").GetString());
        Assert.Contains("docs/Example.md", wiki.GetProperty("inputs").EnumerateArray().Select(item => item.GetString()));
        Assert.Contains("docs/Example/api.md", wiki.GetProperty("inputs").EnumerateArray().Select(item => item.GetString()));
        Assert.Equal("native", checks.Single(check => check.GetProperty("id").GetString() == "build:src/Example/Example.csproj")
            .GetProperty("context").GetString());
        Assert.Contains("build:entry/InnoEngine.Build/InnoEngine.Build.csproj", wiki.GetProperty("prerequisites").EnumerateArray().Select(item => item.GetString()));
    }

    [Fact]
    public async Task ScriptWikiPreparesItsDeclaredProductEvenWithoutAProductionSourceChange()
    {
        using var repository = new ValidationRepository();
        string properties = repository.PathOf("Product.props");
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Build/InnoEngine.Build.csproj", ValidationRepository.Project());
        repository.Write("entry/InnoEngine.Editor/InnoEngine.Editor.csproj", ValidationRepository.Project(
            "<PropertyGroup><InnoProductId>editor</InnoProductId><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
            + properties + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare product and build entry");
        repository.Write("docs/Guide.md", "# Script guide\n```csharp\nusing InnoEngine;\n```\n");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Empty(Owners(plan));
        JsonElement wiki = plan.RootElement.GetProperty("checks").EnumerateArray().Single(check => check.GetProperty("id").GetString() == "wiki:native");
        string[] prerequisites = wiki.GetProperty("prerequisites").EnumerateArray().Select(value => value.GetString()!).ToArray();
        Assert.Contains("build:entry/InnoEngine.Build/InnoEngine.Build.csproj", prerequisites);
        Assert.Contains("build:entry/InnoEngine.Editor/InnoEngine.Editor.csproj", prerequisites);
        Assert.Equal("fixture-target", plan.RootElement.GetProperty("contexts").GetProperty("native").GetProperty("properties").GetProperty("InnoToolTarget").GetString());
    }

    [Fact]
    public async Task ACompetingOutputWriterCanCancelWithoutAdoptingOrReleasingAnotherOwner()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Change.cs", "namespace Example; public sealed class Change { }");
        using JsonDocument plan = await repository.PlanAsync();
        string artifacts = plan.RootElement.GetProperty("artifacts").GetString()!;
        string parent = Path.GetDirectoryName(artifacts)!;
        using FileLease writer = await FileLease.AcquireAsync(artifacts + ".lock", TimeSpan.FromSeconds(5));
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        Task<int> competing = repository.RunAsync(cancellation.Token);
        while (!BuildCacheOwnerCatalog.ReadOwners(repository.PathOf(".")).Any(owner => owner.directory == parent))
        {
            cancellation.Token.ThrowIfCancellationRequested();
            Assert.False(competing.IsCompleted);
            await Task.Delay(20, cancellation.Token);
        }
        Assert.False(competing.IsCompleted);
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await competing);
        Assert.False(Directory.Exists(artifacts));
        Assert.False(File.Exists(repository.PathOf("artifacts/run/results.json")));
        await Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            using FileLease unavailable = await FileLease.AcquireAsync(artifacts + ".lock", TimeSpan.Zero);
        });
    }

    [Fact]
    public async Task ExistingUnregisteredBuildOutputCannotBeSilentlyAdopted()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Change.cs", "namespace Example; public sealed class Change { }");
        using JsonDocument plan = await repository.PlanAsync();
        string artifacts = plan.RootElement.GetProperty("artifacts").GetString()!;
        Directory.CreateDirectory(artifacts);
        string unrelated = Path.Combine(artifacts, "unrelated-owner.txt");
        File.WriteAllText(unrelated, "preserve this owner");
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.RunAsync());
        Assert.Equal("preserve this owner", File.ReadAllText(unrelated));
        Assert.False(File.Exists(repository.PathOf("artifacts/run/results.json")));
    }

    [Fact]
    public async Task AnUninitializedSubmoduleDoesNotBorrowItsParentRepositoryIdentity()
    {
        using var repository = new ValidationRepository();
        using JsonDocument initial = await repository.PlanAsync();
        string revision = initial.RootElement.GetProperty("commit").GetString()!;
        repository.Git("update-index", "--add", "--cacheinfo", "160000," + revision + ",extern/Uninitialized");
        repository.Git("commit", "-m", "Declare unavailable native source");
        Directory.CreateDirectory(repository.PathOf("extern/Uninitialized"));
        repository.Write("tests/Example.Tests/Change.cs", "// Domain input does not require native source preparation.");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Equal("unavailable:" + revision, plan.RootElement.GetProperty("inputs").GetProperty("extern/Uninitialized").GetString());
        Assert.False(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains("tests/Example.Tests/Example.Tests.csproj", Owners(plan));
    }

    [Fact]
    public async Task CachedSubmoduleStatusCannotCertifyMutatedNativeSource()
    {
        using var source = new ValidationRepository();
        using var repository = new ValidationRepository();
        repository.Git("-c", "protocol.file.allow=always", "submodule", "add", source.PathOf("."), "extern/Before");
        repository.Git("-c", "protocol.file.allow=always", "submodule", "add", source.PathOf("."), "extern/Native");
        repository.Git("-c", "protocol.file.allow=always", "submodule", "add", source.PathOf("."), "extern/After");
        repository.Git("commit", "-am", "Pin native source fixture");
        repository.Git("-C", "extern/Native", "update-index", "--assume-unchanged", "fixtures/plugin.txt");
        string path = repository.PathOf("extern/Native/fixtures/plugin.txt");
        DateTime timestamp = File.GetLastWriteTimeUtc(path);
        long length = new FileInfo(path).Length;
        File.WriteAllText(path, "mutated input");
        File.SetLastWriteTimeUtc(path, timestamp);
        Assert.Equal(length, new FileInfo(path).Length);
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
        Assert.Contains("uncommitted inputs", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task IgnoredCompilerInputsCannotDisappearFromValidationIdentity()
    {
        using var repository = new ValidationRepository();
        File.AppendAllText(repository.PathOf(".gitignore"), "Local.cs\n");
        repository.Git("commit", "-am", "Ignore a local file");
        repository.Write("src/Example/Local.cs", "namespace Example; public class Local { }\n");
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
        Assert.Contains("untracked ignored compiler input", failure.Message, StringComparison.Ordinal);
        Assert.Contains("src/Example/Local.cs", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task SubmoduleLinksAreVerifiedAsLinkTextWithoutFollowingTheirTargets()
    {
        using var source = new ValidationRepository();
        source.Write("link.txt", "../../outside-owner.txt");
        string blob = source.Git("hash-object", "-w", "link.txt");
        source.Git("update-index", "--add", "--cacheinfo", "120000," + blob + ",link.txt");
        source.Git("commit", "-m", "Record a link without importing its target");
        using var repository = new ValidationRepository();
        repository.Git("-c", "protocol.file.allow=always", "submodule", "add", source.PathOf("."), "extern/Native");
        repository.Git("commit", "-am", "Pin source with a symbolic link");
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project(
            "<ItemGroup><Compile Include=\"../../extern/Native/src/Probe.cs\" /></ItemGroup>"));
        source.Write("src/Probe.cs", "namespace Native; public class Probe { }");
        source.Git("add", "src/Probe.cs");
        source.Git("commit", "-m", "Declare a compiler input");
        repository.Git("-C", "extern/Native", "fetch", "origin");
        repository.Git("-C", "extern/Native", "checkout", "origin/master");
        repository.Git("add", "extern/Native");
        repository.Git("commit", "-m", "Update compiler input owner");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Contains(plan.RootElement.GetProperty("inputSets").GetProperty("src/Example/Example.csproj").EnumerateArray(),
            input => input.GetString() == "extern/Native");
        Assert.Contains(plan.RootElement.GetProperty("inputSets").GetProperty("native-sources").EnumerateArray(),
            input => input.GetString() == "extern/Native");
        string path = repository.PathOf("extern/Native/link.txt");
        repository.Git("-C", "extern/Native", "update-index", "--assume-unchanged", "link.txt");
        if (new FileInfo(path).LinkTarget is not null)
        {
            File.Delete(path);
            File.CreateSymbolicLink(path, "../../another-owner.txt");
        }
        else
            File.WriteAllText(path, "../../another-owner.txt");
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
        Assert.Contains("uncommitted inputs", failure.Message, StringComparison.Ordinal);
    }

    private static string[] Owners(JsonDocument document)
        => document.RootElement.GetProperty("owners").EnumerateArray().Select(value => value.GetString()!).ToArray();

    [Fact]
    public async Task ExtendedAcceptanceIncludesColdWarmLifecycleAndReleaseProductRequirements()
    {
        using var repository = new ValidationRepository();
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Build/InnoEngine.Build.csproj", ValidationRepository.Project());
        repository.Write("platforms/Fixture/player/Player.csproj", ValidationRepository.Project(
            "<PropertyGroup><InnoProductId>player</InnoProductId><InnoProductTarget>fixture-target</InnoProductTarget></PropertyGroup>"));
        repository.Write("entry/InnoEngine.Editor/InnoEngine.Editor.csproj", ValidationRepository.Project(
            "<PropertyGroup><InnoProductId>editor</InnoProductId><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
            + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"));
        using JsonDocument plan = await repository.PlanAsync(extended: true);
        Assert.True(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.True(plan.RootElement.GetProperty("extended").GetBoolean());
        JsonElement.ArrayEnumerator checks = plan.RootElement.GetProperty("checks").EnumerateArray();
        foreach (string required in new[] { "cold-inputs", "native-hot-builds", "editor-products", "editor-release-products", "support-pack", "native-aot-restore" })
            Assert.Contains(checks, check => check.GetProperty("id").GetString() == required && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null);
        Assert.Equal("Release", plan.RootElement.GetProperty("contexts").GetProperty("native-release").GetProperty("properties").GetProperty("Configuration").GetString());
        Assert.NotEqual(plan.RootElement.GetProperty("contexts").GetProperty("native").GetProperty("properties").GetProperty("ArtifactsPath").GetString(),
            plan.RootElement.GetProperty("contexts").GetProperty("native-release").GetProperty("properties").GetProperty("ArtifactsPath").GetString());
        Assert.All(checks.Where(check => check.GetProperty("kind").GetString() == "test"),
            check => Assert.Equal("domain", check.GetProperty("layer").GetString()));
    }

    private static string[] Changes(JsonDocument document)
        => document.RootElement.GetProperty("changes").EnumerateArray().Select(value => value.GetString()!).ToArray();
}
