using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;
using Inno.Core.IO;
using Inno.Testing.Source;
using Inno.Testing.Validation;
using Inno.Tooling.Validation;

namespace Inno.Tooling.Validation.Tests;

[Collection(nameof(ValidationPlanFixture))]
public sealed class ValidationProfileTests : ValidationPlanFixture
{
    [Fact]
    public async Task HostedManagedAllocationDoesNotChangeIndependentLocalPlans()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Changed.cs", "namespace Example; public class Changed { }");
        string output = repository.PathOf("artifacts/secondary");
        Assert.Equal(
            0,
            await ValidationCommand.ExecuteAsync([
                "plan",
                "--engine-root",
                repository.PathOf("."),
                "--dotnet",
                repository.dotnet,
                "--shared-managed",
                "false",
                "--output",
                output
            ])
        );
        using JsonDocument secondary = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(output, "plan.json")));
        Assert.Equal(JsonValueKind.String, ExampleTest(secondary).GetProperty("exclusion").ValueKind);
        using JsonDocument local = await repository.PlanAsync();
        Assert.Equal(JsonValueKind.Null, ExampleTest(local).GetProperty("exclusion").ValueKind);

        static JsonElement ExampleTest(JsonDocument plan)
            => plan.RootElement.GetProperty("checks").EnumerateArray().Single(check => (
                check.GetProperty("kind").GetString() == "test"
                && check.GetProperty("owner").GetString() == "tests/Example.Tests/Example.Tests.csproj"
            ));
    }

    [Fact]
    public async Task ScheduledPlanRebindsOnlyOnItsCapturedEnvironment()
    {
        using var repository = new ValidationRepository();
        using JsonDocument planned = await repository.PlanAsync(profile: "fast");
        string source = repository.PathOf("artifacts/scheduled.json");
        string output = repository.PathOf("artifacts/bound");
        JsonNode plan = JsonNode.Parse(planned.RootElement.GetRawText())!;
        File.WriteAllText(source, plan.ToJsonString());
        Task<int> BindAsync()
            => ValidationCommand.ExecuteAsync([
                "plan",
                "--engine-root",
                repository.PathOf("."),
                "--dotnet",
                repository.dotnet,
                "--plan",
                source,
                "--profile",
                "fast",
                "--output",
                output
            ]);
        Assert.Equal(0, await BindAsync());
        using JsonDocument bound = JsonDocument.Parse(File.ReadAllText(Path.Combine(output, "plan.json")));
        Assert.Equal(planned.RootElement.GetProperty("environment").GetString(), bound.RootElement.GetProperty("environment").GetString());
        File.Delete(Path.Combine(output, "plan.json"));
        plan["environment"] = new string('0', 64);
        File.WriteAllText(source, plan.ToJsonString());
        Assert.Equal(3, await BindAsync());
        Assert.False(File.Exists(Path.Combine(output, "plan.json")));
    }

    [Fact]
    public async Task ManualProducerProfileSelectsOnlyDeclaredProducerRegressions()
    {
        using var repository = new ValidationRepository();
        repository.Write(
            "tests/Producer/Suite.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>integration</InnoValidationLayer>"
                + "<InnoValidationRequiresNativeProducer>true</InnoValidationRequiresNativeProducer></PropertyGroup>"
            )
        );
        using JsonDocument plan = await repository.PlanAsync(profile: "native-producer");
        Assert.False(plan.RootElement.GetProperty("full").GetBoolean());
        JsonElement selected = Assert.Single(
            plan.RootElement.GetProperty("checks")
                .EnumerateArray()
                .Where(
                    check => (
                        check.GetProperty("kind").GetString() == "test"
                        && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null
                    )
                )
        );
        Assert.Equal("tests/Producer/Suite.csproj", selected.GetProperty("owner").GetString());
        await Assert.ThrowsAsync<ArgumentException>(
            () => repository.PlanAsync(profile: "native-producer", nativeDependencies: "artifacts/backend-store/selection.json")
        );
    }

    [Fact]
    public async Task ProducerRegressionsStayExcludedWithoutExplicitSourceProduction()
    {
        using var repository = new ValidationRepository();
        const string C_OWNER = "tests/Producer/Suite.csproj";
        repository.Write(
            C_OWNER,
            ValidationRepository.Project(
                "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>integration</InnoValidationLayer>"
                + "<InnoValidationRequiresNativeProducer>true</InnoValidationRequiresNativeProducer></PropertyGroup>"
            )
        );
        foreach (string? nativeDependencies in new[] { "artifacts/backend-store/selection.json", null })
        {
            using JsonDocument plan = await repository.PlanAsync(nativeDependencies: nativeDependencies);
            JsonElement check = plan.RootElement.GetProperty("checks")
                .EnumerateArray()
                .Single(item => item.GetProperty("id").GetString() == "test:" + C_OWNER);
            Assert.Contains("producer regression", check.GetProperty("exclusion").GetString());
            Assert.DoesNotContain(
                plan.RootElement.GetProperty("checks").EnumerateArray(),
                item => item.GetProperty("kind").GetString() == "cold-inputs"
            );
        }
    }

    [Fact]
    public async Task FastProfileDefersRealIntegrationAndNativeProductsWithoutDownloadingDependencies()
    {
        using var repository = new ValidationRepository();
        foreach (string layer in new[] { "contract", "integration", "native" })
        {
            repository.Write(
                "tests/" + layer + "/Suite.csproj",
                ValidationRepository.Project(
                    "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>" + layer
                + "</InnoValidationLayer>" + (layer == "native" ? "<InnoTestNativeProduct>editor</InnoTestNativeProduct>" : string.Empty)
                + "</PropertyGroup><ItemGroup><ProjectReference Include=\"../../src/Example/Example.csproj\" /></ItemGroup>"
                )
            );
        }
        repository.Write(
            "tests/Pack/Suite.csproj",
            ValidationRepository.Project("""
            <PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>contract</InnoValidationLayer></PropertyGroup>
            <ItemGroup><InnoNativeDependency Include="player" TargetId="cross-target" Configuration="Release" /></ItemGroup>
            """)
        );
        using JsonDocument plan = await repository.PlanAsync(profile: "fast", nativeDependencies: "artifacts/backend-store/selection.json");
        Assert.False(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        Assert.Empty(plan.RootElement.GetProperty("nativeRequests").EnumerateArray());
        JsonElement[] tests = plan.RootElement
            .GetProperty("checks")
            .EnumerateArray()
            .Where(check => check.GetProperty("kind").GetString() == "test")
            .ToArray();
        Assert.Contains(
            tests,
            check => check.GetProperty("layer").GetString() == "contract" && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null
        );
        Assert.All(
            tests.Where(check => check.GetProperty("layer").GetString() is "native" or "integration"),
            check => Assert.Equal(JsonValueKind.String, check.GetProperty("exclusion").ValueKind)
        );
        Assert.False(Directory.Exists(repository.PathOf("artifacts/dependencies")));
        Assert.Equal(
            JsonValueKind.String,
            tests.Single(check => check.GetProperty("owner").GetString() == "tests/Pack/Suite.csproj")
                .GetProperty("exclusion").ValueKind
        );
    }

    [Fact]
    public async Task IncrementalProfilesLimitArchitectureSourceRulesToChangedSources()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Changed.cs", "namespace Example;\n");
        repository.Write("src/Example/Changed.txt", "Text.\n");
        foreach (string? profile in new[] { null, "fast", "master" })
        {
            using JsonDocument incremental = await repository.PlanAsync(profile: profile);
            Assert.Equal(
                ["src/Example/Changed.cs"],
                Architecture(incremental).GetProperty("changedSources").EnumerateArray().Select(static source => source.GetString())
            );
        }
        repository.Write("InnoEngine.sln", "Microsoft Visual Studio Solution File, Format Version 12.00\n\n");
        using JsonDocument tooling = await repository.PlanAsync();
        Assert.False(IsIncremental(tooling));

        static bool IsIncremental(JsonDocument plan)
            => Architecture(plan).TryGetProperty("changedSources", out JsonElement sources) && sources.ValueKind != JsonValueKind.Null;

        static JsonElement Architecture(JsonDocument plan)
            => plan.RootElement
                .GetProperty("checks")
                .EnumerateArray()
                .Single(static check => check.GetProperty("id").GetString() == "architecture");
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

    [Theory]
    [InlineData("Debug")]
    [InlineData("Release")]
    public async Task ExtendedAcceptanceIncludesColdWarmLifecycleAndReleaseProductRequirements(string configuration)
    {
        using var repository = new ValidationRepository();
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Build/InnoEngine.Build.csproj", ValidationRepository.Project());
        repository.Write("src/PlayerOnly/PlayerOnly.csproj", ValidationRepository.Project());
        repository.Write(
            "platforms/Fixture/player/Player.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><InnoProductId>player</InnoProductId><InnoProductTarget>fixture-target</InnoProductTarget></PropertyGroup>"
                + "<ItemGroup><ProjectReference Include=\"../../../src/PlayerOnly/PlayerOnly.csproj\" /></ItemGroup>"
            )
        );
        // Native tests use the product preparation properties without becoming a publishable Player.
        repository.Write(
            "tests/Player.Tests/Player.Tests.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>integration</InnoValidationLayer>"
                + "<InnoTestNativeProduct>player</InnoTestNativeProduct><InnoTestNativeTarget>fixture-target</InnoTestNativeTarget>"
                + "<InnoProductId>player</InnoProductId><InnoProductTarget>fixture-target</InnoProductTarget></PropertyGroup>"
            )
        );
        repository.Write(
            "tests/fixtures/PlayerFixture/PlayerFixture.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><IsTestSupportProject>true</IsTestSupportProject><InnoProductId>player</InnoProductId>"
                + "<InnoProductTarget>fixture-target</InnoProductTarget></PropertyGroup>"
            )
        );
        repository.Write(
            "entry/InnoEngine.Editor/InnoEngine.Editor.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><InnoProductId>editor</InnoProductId><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
                + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"
            )
        );
        using JsonDocument plan = await repository.PlanAsync(extended: true, configuration: configuration);
        Assert.True(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.True(plan.RootElement.GetProperty("extended").GetBoolean());
        JsonElement.ArrayEnumerator checks = plan.RootElement.GetProperty("checks").EnumerateArray();
        foreach (string required in new[] {
            "cold-inputs",
            "native-hot-builds",
            "editor-products",
            "support-pack",
            "native-aot-restore"
        }) {
            Assert.Contains(
                checks,
                check => check.GetProperty("id").GetString() == required && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null
            );
        }
        JsonElement pack = checks.Single(check => check.GetProperty("id").GetString() == "support-pack");
        string releaseContext = pack.GetProperty("context").GetString()!;
        Assert.Equal(
            "Release",
            plan.RootElement
                .GetProperty("contexts")
                .GetProperty(releaseContext)
                .GetProperty("properties")
                .GetProperty("Configuration")
                .GetString()
        );
        if (configuration != "Release")
        {
            Assert.Contains(checks, check => (
                check.GetProperty("id").GetString() == "editor-release-products"
                && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null
            ));
            Assert.NotEqual(
                plan.RootElement
                    .GetProperty("contexts")
                    .GetProperty("native")
                    .GetProperty("properties")
                    .GetProperty("ArtifactsPath")
                    .GetString(),
                plan.RootElement
                    .GetProperty("contexts")
                    .GetProperty(releaseContext)
                    .GetProperty("properties")
                    .GetProperty("ArtifactsPath")
                    .GetString()
            );
        }
        Assert.Equal(
            "platforms/Fixture/player/Player.csproj",
            checks.Single(check => check.GetProperty("id").GetString() == "native-aot-restore").GetProperty("owner").GetString()
        );
        Assert.All(
            checks.Where(check => check.GetProperty("kind").GetString() == "test"),
            check => Assert.Equal(
                check.GetProperty("owner").GetString() == "tests/Player.Tests/Player.Tests.csproj"
                ? "integration"
                : "domain",
                check.GetProperty("layer").GetString()
            )
        );
    }
}
