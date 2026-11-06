using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationSolutionTests
{
    [Theory]
    [InlineData("Debug")]
    [InlineData("SourceDebug")]
    [InlineData("Release")]
    public async Task PlannedUnionBuildsOwnersExcludedByTheIdeSolution(string configuration)
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project(
            "<ItemGroup><ProjectReference Include=\"../Other/Other.csproj\" /></ItemGroup>"));
        repository.Write("src/Other/Other.csproj", ValidationRepository.Project($"""
            <Target Name="RequireSelectedConfiguration" BeforeTargets="CoreCompile">
              <Error Condition="'$(Configuration)' != '{configuration}'" Text="The unselected dependency lost the planned configuration." />
            </Target>
            """));
        repository.Write("InnoEngine.sln", """
            Microsoft Visual Studio Solution File, Format Version 12.00
            Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Example", "src/Example/Example.csproj", "{11111111-1111-1111-1111-111111111111}"
            EndProject
            Global
                GlobalSection(SolutionConfigurationPlatforms) = preSolution
                    Debug|Any CPU = Debug|Any CPU
                    SourceDebug|Any CPU = SourceDebug|Any CPU
                    Release|Any CPU = Release|Any CPU
                EndGlobalSection
                GlobalSection(ProjectConfigurationPlatforms) = postSolution
                    {11111111-1111-1111-1111-111111111111}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                    {11111111-1111-1111-1111-111111111111}.SourceDebug|Any CPU.ActiveCfg = Debug|Any CPU
                    {11111111-1111-1111-1111-111111111111}.Release|Any CPU.ActiveCfg = Release|Any CPU
                EndGlobalSection
            EndGlobal
            """);
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Exclude an owner from the IDE default build");
        repository.Write("src/Example/Change.cs", "namespace Example; public sealed class Change { }");
        using JsonDocument plan = await repository.PlanAsync(configuration: configuration);
        JsonElement context = plan.RootElement.GetProperty("contexts").GetProperty("managed");
        string[] properties = context.GetProperty("properties").EnumerateObject().Select(pair => "-p:" + pair.Name + "=" + pair.Value.GetString()).ToArray();
        string target = context.GetProperty("sdkGraph").EnumerateArray().Single(snapshot =>
            snapshot.GetProperty("Path").GetString() == repository.PathOf("src/Example/Example.csproj"))
            .GetProperty("Properties").GetProperty("TargetPath").GetString()!;
        repository.Write("artifacts/ide.slnf", JsonSerializer.Serialize(new
        {
            solution = new { path = repository.PathOf("InnoEngine.sln"), projects = new[] { "src/Example/Example.csproj" } }
        }));
        await repository.BuildAsync([repository.PathOf("artifacts/ide.slnf"), ..properties]);
        Assert.False(File.Exists(target));

        // The deliberately incomplete repository fails policy, but must retain the exact planned build selection.
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        string solution = repository.PathOf("artifacts/run/managed-Build.slnx");
        XDocument union = XDocument.Load(solution);
        string[] expected = plan.RootElement.GetProperty("checks").EnumerateArray()
            .Where(check => check.GetProperty("kind").GetString() == "build" && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null)
            .Select(check => repository.PathOf(check.GetProperty("owner").GetString()!)).Order(StringComparer.Ordinal).ToArray();
        Assert.Equal(expected, union.Descendants("Project").Select(project => Path.GetFullPath(project.Attribute("Path")!.Value))
            .Order(StringComparer.Ordinal).ToArray());
        Assert.Equal(configuration, union.Descendants("BuildType").Single().Attribute("Name")!.Value);
        Assert.DoesNotContain(union.Descendants("Project"), project =>
            Path.GetFullPath(project.Attribute("Path")!.Value) == repository.PathOf("src/Other/Other.csproj"));
        Assert.False(File.Exists(target));
        await repository.BuildAsync([solution, ..properties]);
        Assert.True(File.Exists(target));
    }
}
