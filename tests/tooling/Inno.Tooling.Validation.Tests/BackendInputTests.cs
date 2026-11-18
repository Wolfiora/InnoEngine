using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Inno.Testing.Validation;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class BackendInputTests
{
    [Fact]
    public async Task TestEditsRetainCompilerInputsAndSharedProductionChangesReachTheirConsumers()
    {
        using var repository = new ValidationRepository();
        const string C_BACKEND = "backends/Probe/Probe.csproj";
        const string C_INDEPENDENT = "backends/Independent/Independent.csproj";
        const string C_TEST = "tests/backends/Probe.Tests/Probe.Tests.csproj";
        repository.Write(C_BACKEND, ValidationRepository.Project("<ItemGroup><ProjectReference Include=\"../../src/Example/Example.csproj\" /></ItemGroup>"));
        repository.Write(C_INDEPENDENT, ValidationRepository.Project());
        repository.Write(C_TEST, ValidationRepository.Project("""
            <PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>domain</InnoValidationLayer></PropertyGroup>
            <ItemGroup><ProjectReference Include="../../../backends/Probe/Probe.csproj" /></ItemGroup>
            """));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Add the backend fixture");
        using JsonDocument baseline = await Inputs();
        string original = Identity(baseline, C_BACKEND);
        string independent = Identity(baseline, C_INDEPENDENT);
        repository.Write("tests/backends/Probe.Tests/Changed.cs", "class TestChange {}");
        using JsonDocument testOnly = await Inputs();
        Assert.Equal(original, Identity(testOnly, C_BACKEND));
        Assert.Equal(independent, Identity(testOnly, C_INDEPENDENT));
        Assert.Equal(new[] { C_TEST }, testOnly.RootElement.GetProperty("tests").EnumerateArray().Select(static item => item.GetString()));
        Assert.Equal(new[] { C_BACKEND }, testOnly.RootElement.GetProperty("required").EnumerateArray().Select(static item => item.GetString()));
        repository.Write("src/Example/Changed.cs", "public class NewContract {}");
        using JsonDocument changed = await Inputs();
        Assert.NotEqual(original, Identity(changed, C_BACKEND));
        Assert.Equal(independent, Identity(changed, C_INDEPENDENT));

        async Task<JsonDocument> Inputs()
        {
            using JsonDocument plan = await repository.PlanAsync(profile: "fast");
            string output = repository.PathOf("artifacts/backend-inputs.json");
            int code = await ValidationCommand.ExecuteAsync(["backend-inputs", "--plan", repository.PathOf("artifacts/plan/plan.json"),
                "--target", "fixture", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet, "--output", output]);
            Assert.Equal(0, code);
            return JsonDocument.Parse(File.ReadAllBytes(output));
        }

        static string Identity(
            JsonDocument plan,
            string owner
        ) => plan.RootElement.GetProperty("projects").GetProperty(owner).GetString()!;
    }
}
