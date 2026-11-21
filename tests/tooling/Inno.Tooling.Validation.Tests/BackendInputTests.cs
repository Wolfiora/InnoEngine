using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Inno.Testing.Validation;

namespace Inno.Tooling.Validation.Tests;

public sealed class BackendInputTests
{
    [Fact]
    public async Task BindingRejectsMissingOwnersAndChangedInventoryBeforeWritingTheNativeSelection()
    {
        using var repository = new ValidationRepository();
        const string C_STORE = "artifacts/backend-packages";
        const string C_PROJECT = "backends/Probe/Probe.csproj";
        repository.Write(
            "entry/InnoEngine.Editor/InnoEngine.Editor.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><InnoDevelopmentTarget>fixture</InnoDevelopmentTarget></PropertyGroup>"
            )
        );
        repository.Write(C_PROJECT, ValidationRepository.Project());
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Register host and backend");
        repository.Write("backends/Probe/Changed.cs", "class BackendChange {}");
        using JsonDocument plan = await repository.PlanAsync(profile: "fast", backendStore: C_STORE);
        repository.Write("artifacts/packages.json", "[]");
        await Assert.ThrowsAsync<InvalidDataException>(Bind);
        Assert.False(File.Exists(repository.PathOf(C_STORE + "/selection.json")));
        string inputPath = repository.PathOf("artifacts/current.json");
        Assert.Equal(
            0,
            await ValidationCommand.ExecuteAsync([
                "backend-inputs",
                "--target",
                "fixture",
                "--plan",
                repository.PathOf("artifacts/plan/plan.json"),
                "--engine-root",
                repository.PathOf("."),
                "--dotnet",
                repository.dotnet,
                "--output",
                inputPath
            ])
        );
        using JsonDocument inputs = JsonDocument.Parse(File.ReadAllBytes(inputPath));
        string input = inputs.RootElement.GetProperty("projects").GetProperty(C_PROJECT).GetString()!;
        string native = """{"assets":[],"aggregates":[],"requests":[]}""";
        string inventory = JsonSerializer.Serialize(new
        {
            target = "fixture",
            files = new[]
            {
                new
                {
                    owner = "fixture",
                    configuration = "all",
                    input = Digest(native),
                    path = "native/selection.json",
                    sha256 = Digest(native)
                },
                new { owner = C_PROJECT, configuration = "Debug", input, path = "managed/probe.dll", sha256 = Digest("probe") }
            }
        });
        string receipt = JsonSerializer.Serialize(
            new { target = "fixture", sha256 = Digest("archive"), manifestSha256 = Digest(inventory) }
        );
        repository.Write("artifacts/packages.json", "[" + receipt + "]");
        repository.Write(C_STORE + "/fixture/receipt.json", receipt);
        repository.Write(C_STORE + "/fixture/native/selection.json", native);
        repository.Write(C_STORE + "/fixture/manifest.json", inventory + " ");
        await Assert.ThrowsAsync<InvalidDataException>(Bind);
        repository.Write(C_STORE + "/fixture/manifest.json", inventory);
        Assert.Equal(0, await Bind());
        using JsonDocument bound = JsonDocument.Parse(File.ReadAllBytes(repository.PathOf("artifacts/bound.json")));
        JsonElement owners = bound.RootElement.GetProperty("backendOwnerPayloads");
        Assert.Equal("fixture/Debug/" + C_PROJECT, Assert.Single(owners.EnumerateObject()).Name);
        Assert.Equal(Digest("archive"), bound.RootElement.GetProperty("backendPackages")[0].GetProperty("sha256").GetString());

        Task<int> Bind()
            => ValidationCommand.ExecuteAsync([
                "bind-backends",
                "--packages",
                repository.PathOf("artifacts/packages.json"),
                "--plan",
                repository.PathOf("artifacts/plan/plan.json"),
                "--engine-root",
                repository.PathOf("."),
                "--dotnet",
                repository.dotnet,
                "--output",
                repository.PathOf("artifacts/bound.json")
            ]);
        static string Digest(string value) => System.Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    }

    [Theory]
    [InlineData("Debug", true)]
    [InlineData("Release", false)]
    public async Task ProductCoverageUsesTheRequestedConfigurationWhenItsOnlyToolIsAlwaysRelease(
        string certifiedConfiguration,
        bool expected
    ) {
        using var repository = new ValidationRepository();
        repository.Write(
            "entry/InnoEngine.Editor/InnoEngine.Editor.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
                + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"
            )
        );
        repository.Write("Product.props", "<Project />");
        repository.Write(
            "tests/Tool.Tests/Tool.Tests.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>native</InnoValidationLayer>"
            + "<InnoTestNativeProduct>tool-only</InnoTestNativeProduct><InnoTestNativeTarget>fixture-target</InnoTestNativeTarget></PropertyGroup>"
            )
        );
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Contains(plan.RootElement.GetProperty("nativeRequests").EnumerateArray(), request => (
            request.GetProperty("product").GetString() == "tool-only" && request.GetProperty("configuration").GetString() == "Debug"
        ));
        string digest = new('a', 64);
        WriteSelection(digest);
        string output = repository.PathOf("artifacts/inputs.json");
        Assert.Equal(0, await Evaluate());
        using JsonDocument inputs = JsonDocument.Parse(File.ReadAllBytes(output));
        Assert.Equal(
            !expected,
            inputs.RootElement.GetProperty("missingNativeProducts").EnumerateArray().Any(item => item.GetString() == "tool-only")
        );
        if (expected)
        {
            WriteSelection(new string('b', 64));
            await Assert.ThrowsAsync<InvalidDataException>(() => Evaluate());
        }

        void WriteSelection(string certifiedDigest)
            => repository.Write(
                "artifacts/native.json",
                JsonSerializer.Serialize(new
                {
                    assets = System.Array.Empty<object>(),
                    aggregates = new[]
                    {
                        new { product = "tool-only", target = "fixture-target", configuration = "release", sha256 = digest }
                    },
                    requests = new[]
                    {
                        new
                        {
                            product = "tool-only",
                            target = "fixture-target",
                            configuration = certifiedConfiguration,
                            digests = new[] { certifiedDigest }
                        }
                    }
                })
            );

        Task<int> Evaluate()
            => ValidationCommand.ExecuteAsync([
                "backend-inputs",
                "--plan",
                repository.PathOf("artifacts/plan/plan.json"),
                "--target",
                "fixture-target",
                "--configuration",
                "Debug",
                "--engine-root",
                repository.PathOf("."),
                "--dotnet",
                repository.dotnet,
                "--selection",
                repository.PathOf("artifacts/native.json"),
                "--output",
                output
            ]);
    }

    [Fact]
    public async Task PureManagedChecksBindAnEmptyAcquisition()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Changed.cs", "class ManagedChange {}");
        using JsonDocument plan = await repository.PlanAsync(profile: "fast", backendStore: "artifacts/backend-packages");
        repository.Write("artifacts/packages.json", "[]");
        Assert.Equal(
            0,
            await ValidationCommand.ExecuteAsync([
                "bind-backends",
                "--packages",
                repository.PathOf("artifacts/packages.json"),
                "--plan",
                repository.PathOf("artifacts/plan/plan.json"),
                "--engine-root",
                repository.PathOf("."),
                "--dotnet",
                repository.dotnet,
                "--output",
                repository.PathOf("artifacts/bound.json")
            ])
        );
        using JsonDocument bound = JsonDocument.Parse(File.ReadAllBytes(repository.PathOf("artifacts/bound.json")));
        Assert.Empty(bound.RootElement.GetProperty("backendPackages").EnumerateArray());
    }

    [Fact]
    public async Task TestEditsRetainCompilerInputsAndSharedProductionChangesReachTheirConsumers()
    {
        using var repository = new ValidationRepository();
        const string C_BACKEND = "backends/Probe/Probe.csproj";
        const string C_INDEPENDENT = "backends/Independent/Independent.csproj";
        const string C_TEST = "tests/backends/Probe.Tests/Probe.Tests.csproj";
        repository.Write(
            C_BACKEND,
            ValidationRepository.Project("<ItemGroup><ProjectReference Include=\"../../src/Example/Example.csproj\" /></ItemGroup>")
        );
        repository.Write(C_INDEPENDENT, ValidationRepository.Project());
        repository.Write(
            C_TEST,
            ValidationRepository.Project("""
            <PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>domain</InnoValidationLayer></PropertyGroup>
            <ItemGroup><ProjectReference Include="../../../backends/Probe/Probe.csproj" /></ItemGroup>
            """)
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Add the backend fixture");
        using JsonDocument baseline = await Inputs(repository);
        string original = Identity(baseline, C_BACKEND);
        string independent = Identity(baseline, C_INDEPENDENT);
        repository.Write("tests/backends/Probe.Tests/Changed.cs", "class TestChange {}");
        using JsonDocument testOnly = await Inputs(repository);
        Assert.Equal(original, Identity(testOnly, C_BACKEND));
        Assert.Equal(independent, Identity(testOnly, C_INDEPENDENT));
        Assert.Equal(new[] { C_TEST }, testOnly.RootElement.GetProperty("tests").EnumerateArray().Select(static item => item.GetString()));
        Assert.Equal(
            new[] { C_BACKEND },
            testOnly.RootElement.GetProperty("required").EnumerateArray().Select(static item => item.GetString())
        );
        repository.Write("src/Example/Changed.cs", "public class NewContract {}");
        using JsonDocument changed = await Inputs(repository);
        Assert.NotEqual(original, Identity(changed, C_BACKEND));
        Assert.Equal(independent, Identity(changed, C_INDEPENDENT));
    }

    [Fact]
    public async Task PlatformBuildDefinitionsAffectTheirImportsAndCopies()
    {
        using var repository = new ValidationRepository();
        const string C_TOOL = "backends/Tool/Tool.csproj";
        const string C_IMPORT = "backends/Consumer/Consumer.csproj";
        const string C_COPY = "backends/Shipping/Shipping.csproj";
        const string C_PROPS = "backends/Tool/Platform.props";
        repository.Write(C_TOOL, ValidationRepository.Project());
        repository.Write(C_IMPORT, ValidationRepository.Project("""<Import Project="../Tool/Platform.props" />"""));
        repository.Write(
            C_COPY,
            ValidationRepository.Project("""
            <ItemGroup><None Include="../Tool/Platform.props" CopyToOutputDirectory="PreserveNewest" /></ItemGroup>
            """)
        );
        repository.Write(C_PROPS, "<Project />");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Add the platform build definition");
        using JsonDocument before = await Inputs(repository);

        repository.Write(C_PROPS, "<Project><PropertyGroup><DefineConstants>FEATURE</DefineConstants></PropertyGroup></Project>");
        using JsonDocument after = await Inputs(repository);
        Assert.Equal(Identity(before, C_TOOL), Identity(after, C_TOOL));
        Assert.NotEqual(Identity(before, C_IMPORT), Identity(after, C_IMPORT));
        Assert.NotEqual(Identity(before, C_COPY), Identity(after, C_COPY));
    }

    [Fact]
    public async Task NewPlatformProductDerivesItsConsumersFromItsRegisteredSdkEntry()
    {
        using var repository = new ValidationRepository();
        const string backend = "backends/NewAdapter/Adapter.csproj";
        const string player = "platforms/Fixture/player/Player.csproj";
        repository.Write(backend, ValidationRepository.Project());
        repository.Write("backends/Unrelated/Unrelated.csproj", ValidationRepository.Project());
        repository.Write(
            player,
            ValidationRepository.Project("""
            <PropertyGroup><InnoProductId>player</InnoProductId><InnoProductTarget>fixture</InnoProductTarget></PropertyGroup>
            <ItemGroup><ProjectReference Include="../../../backends/NewAdapter/Adapter.csproj" /></ItemGroup>
            """)
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Register the fixture product");
        using JsonDocument first = await Inputs(repository);
        JsonElement before = first.RootElement.GetProperty("consumers").GetProperty(player);
        Assert.Equal(backend, Assert.Single(before.GetProperty("backendOwners").EnumerateArray()).GetString());
        repository.Write("backends/Unrelated/Change.cs", "class Unrelated {}");
        using JsonDocument unrelated = await Inputs(repository);
        Assert.Equal(
            before.GetProperty("identity").GetString(),
            unrelated.RootElement.GetProperty("consumers").GetProperty(player).GetProperty("identity").GetString()
        );
        repository.Write("backends/NewAdapter/Change.cs", "class Changed {}");
        using JsonDocument affected = await Inputs(repository);
        Assert.NotEqual(
            before.GetProperty("identity").GetString(),
            affected.RootElement.GetProperty("consumers").GetProperty(player).GetProperty("identity").GetString()
        );
    }

    private static async Task<JsonDocument> Inputs(ValidationRepository repository)
    {
        using JsonDocument plan = await repository.PlanAsync(profile: "fast");
        string output = repository.PathOf("artifacts/backend-inputs.json");
        int code = await ValidationCommand.ExecuteAsync([
            "backend-inputs",
            "--plan",
            repository.PathOf("artifacts/plan/plan.json"),
            "--target",
            "fixture",
            "--engine-root",
            repository.PathOf("."),
            "--dotnet",
            repository.dotnet,
            "--output",
            output
        ]);
        Assert.Equal(0, code);
        return JsonDocument.Parse(File.ReadAllBytes(output));
    }

    private static string Identity(
        JsonDocument plan,
        string owner
    ) => plan.RootElement.GetProperty("projects").GetProperty(owner).GetString()!;
}
