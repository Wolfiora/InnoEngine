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
    }

    [Fact]
    public async Task ExecutionFailureRetainsAnExactPlanReceiptAndCannotCertifySuccess()
    {
        using var repository = new ValidationRepository();
        using JsonDocument plan = await repository.PlanAsync();
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        using JsonDocument results = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
        Assert.Contains(results.RootElement.GetProperty("results").EnumerateArray(), result => result.GetProperty("status").GetString() == "Failed");
        string identity = Convert.ToHexStringLower(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(repository.PathOf("artifacts/run/plan.json"))));
        Assert.Equal(identity, results.RootElement.GetProperty("planIdentity").GetString());
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf("artifacts/run/results.json")));
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
        Assert.All(checks.Where(check => check.GetProperty("kind").GetString() == "test"),
            check => Assert.Equal("domain", check.GetProperty("layer").GetString()));
    }

    private static string[] Changes(JsonDocument document)
        => document.RootElement.GetProperty("changes").EnumerateArray().Select(value => value.GetString()!).ToArray();
}
