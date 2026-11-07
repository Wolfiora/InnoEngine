using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationSolutionBuildTests
{
    [Fact]
    public async Task ParallelSolutionRootsShareOneFreshInvocationOwner()
    {
        using var repository = new ValidationRepository();
        string source = TestRepository.GetRoot(typeof(ValidationSolutionBuildTests).Assembly);
        foreach (string relative in new[] { "Directory.Solution.targets", "Directory.Build.targets", "build/msbuild/Inno.Build.Tasks.targets" })
            repository.Write(relative, File.ReadAllText(Path.Combine(source, relative)));
        string targets = repository.PathOf("Directory.Build.targets");
        File.WriteAllText(targets, File.ReadAllText(targets).Replace("</Project>", """
              <Target Name="RecordFixtureInvocationOwner" AfterTargets="Build">
                <WriteLinesToFile File="$(InnoOwnerTraceDirectory)/$(MSBuildProjectName).$(InnoTaskBootstrapOwner).owner"
                                  Lines="$(InnoTaskBootstrapOwner)" Overwrite="true" />
              </Target>
            </Project>
            """, StringComparison.Ordinal));
        repository.Write("src/Shared/Shared.csproj", ValidationRepository.Project());
        repository.Write("src/Shared/Shared.cs", "namespace Shared; public static class Value { public static int Read() => 17; }");
        foreach (string name in new[] { "RootA", "RootB" })
        {
            repository.Write("src/" + name + "/" + name + ".csproj", ValidationRepository.Project(
                "<ItemGroup><ProjectReference Include=\"../Shared/Shared.csproj\" /></ItemGroup>"));
            repository.Write("src/" + name + "/Value.cs", "namespace " + name + "; public static class Value { public static int Read() => Shared.Value.Read(); }");
        }
        repository.Write("artifacts/owners.slnx", """
            <Solution><Project Path="../src/RootA/RootA.csproj" /><Project Path="../src/RootB/RootB.csproj" /></Solution>
            """);
        string previous = string.Empty;
        for (int iteration = 0; iteration < 2; iteration++)
        {
            string traces = repository.PathOf("artifacts/owners-" + iteration);
            Directory.CreateDirectory(traces);
            await repository.SdkAsync("build", "artifacts/owners.slnx", "-t:Rebuild", "--disable-build-servers", "-m:4", "-nr:false",
                "-p:ArtifactsPath=" + repository.PathOf("artifacts/compilation"), "-p:InnoOwnerTraceDirectory=" + traces);
            string[] files = Directory.GetFiles(traces, "*.owner");
            Assert.Equal(3, files.Length);
            string owner = Assert.Single(files.Select(File.ReadAllText).Select(static value => value.Trim()).Distinct(StringComparer.Ordinal));
            Assert.Matches("^[a-f0-9]{32}$", owner);
            Assert.NotEqual(previous, owner);
            Assert.Single(files.Where(path => Path.GetFileName(path).StartsWith("Shared.", StringComparison.Ordinal)));
            previous = owner;
        }
    }
}
