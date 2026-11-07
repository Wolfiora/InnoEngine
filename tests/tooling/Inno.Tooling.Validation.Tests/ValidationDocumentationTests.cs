using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationDocumentationTests
{
    [Theory]
    [InlineData(".agents/records/acceptance/native-reload.json", false)]
    [InlineData(".agents/records/plans/work.md", false)]
    [InlineData(".agents/skills/inno-change/SKILL.md", false)]
    [InlineData(".agents/rulebook/coverage.json", true)]
    public async Task AgentTextChangesKeepProductAndInstructionEvidenceSeparate(
        string path,
        bool policyInput
    ) {
        using var repository = new ValidationRepository();
        repository.Write("tests/Native.Tests/Native.Tests.csproj", ValidationRepository.Project(
            "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>native</InnoValidationLayer><InnoTestNativeProduct>editor</InnoTestNativeProduct></PropertyGroup>"));
        repository.Write(path, "before");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare agent document and native sentinel");
        repository.Write(path, "after!");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.False(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString() == "build");
        Assert.Contains(checks, check => check.GetProperty("id").GetString() == "agent-documents");
        Assert.DoesNotContain(checks, check => check.GetProperty("id").GetString() == "documentation");
        JsonElement instructions = checks.Single(check => check.GetProperty("id").GetString() == "instructions");
        Assert.Equal(policyInput, instructions.GetProperty("inputs").EnumerateArray().Any(input => input.GetString() == path));
        Assert.DoesNotContain(plan.RootElement.GetProperty("inputSets").GetProperty("tooling").EnumerateArray(), input => input.GetString() == path);
    }

    [Fact]
    public async Task ArbitrarilyNamedIncludedFragmentsSelectTheirActualOwnerAndExamples()
    {
        using var repository = new ValidationRepository();
        repository.Write("docs/README.md", "[Example](Example.md)");
        repository.Write("docs/Example.md", "# Example\n<!-- api-include: contracts/behavior.md -->\n");
        repository.Write("docs/contracts/behavior.md", "# Contract\nInitial guidance.");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare included behavior guidance");
        repository.Write("docs/contracts/behavior.md", "# Contract\n```csharp\nint value = 1;\n```\n");
        using JsonDocument plan = await repository.PlanAsync();
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        Assert.Contains(checks, check => check.GetProperty("kind").GetString() == "build"
            && check.GetProperty("owner").GetString() == "src/Example/Example.csproj");
        Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString() == "build"
            && check.GetProperty("owner").GetString() == "src/Other/Other.csproj");
        JsonElement wiki = checks.Single(check => check.GetProperty("kind").GetString() == "wiki");
        Assert.Contains(wiki.GetProperty("inputs").EnumerateArray(), input => input.GetString() == "docs/Example.md");
        Assert.Contains(wiki.GetProperty("inputs").EnumerateArray(), input => input.GetString() == "docs/contracts/behavior.md");
        File.Delete(repository.PathOf("docs/contracts/behavior.md"));
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
    }

    [Fact]
    public async Task UnknownExecutableAgentInputIsNotExemptedAsDocumentation()
    {
        using var repository = new ValidationRepository();
        repository.Write(".agents/check.ps1", "throw 'unknown executable input'");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.True(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains(plan.RootElement.GetProperty("diagnostics").EnumerateArray(), item => item.GetString()!.Contains(".agents/check.ps1", StringComparison.Ordinal));
    }
}
