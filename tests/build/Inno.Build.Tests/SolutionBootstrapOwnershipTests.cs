using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class SolutionBootstrapOwnershipTests
{
    [Fact]
    public async Task SolutionSharesOneBootstrapOwnerAndRenewsItForTheNextBuild()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string fixture = Path.Combine(Path.GetTempPath(), "InnoSolutionOwnerTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(fixture);
        try
        {
            File.Copy(Path.Combine(root, "Directory.Solution.targets"), Path.Combine(fixture, "Directory.Solution.targets"));
            foreach (string name in new[] { "First", "Second" })
                await File.WriteAllTextAsync(Path.Combine(fixture, name + ".csproj"), """
                    <Project DefaultTargets="Build">
                      <Target Name="Build">
                        <Error Condition="'$(InnoTaskBootstrapOwner)' == ''" Text="Missing solution build owner." />
                        <WriteLinesToFile File="$(MSBuildProjectDirectory)/$(MSBuildProjectName).owner"
                                          Lines="$(InnoTaskBootstrapOwner)" Overwrite="true" />
                      </Target>
                    </Project>
                    """);
            string solution = Path.Combine(fixture, "Fixture.sln");
            await File.WriteAllTextAsync(solution, """
                Microsoft Visual Studio Solution File, Format Version 12.00
                Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "First", "First.csproj", "{BF56473A-8C5D-4E17-8A0A-67289A364EB0}"
                EndProject
                Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Second", "Second.csproj", "{4DA642A3-4E80-4CBC-9E57-22F5BA4356D0}"
                EndProject
                Global
                  GlobalSection(SolutionConfigurationPlatforms) = preSolution
                    Debug|Any CPU = Debug|Any CPU
                  EndGlobalSection
                  GlobalSection(ProjectConfigurationPlatforms) = postSolution
                    {BF56473A-8C5D-4E17-8A0A-67289A364EB0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                    {BF56473A-8C5D-4E17-8A0A-67289A364EB0}.Debug|Any CPU.Build.0 = Debug|Any CPU
                    {4DA642A3-4E80-4CBC-9E57-22F5BA4356D0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                    {4DA642A3-4E80-4CBC-9E57-22F5BA4356D0}.Debug|Any CPU.Build.0 = Debug|Any CPU
                  EndGlobalSection
                EndGlobal
                """);
            string host = ToolchainEnvironment.ResolveExecutable("dotnet");
            string? previous = null;
            for (int build = 0; build < 2; build++)
            {
                using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(1));
                await ToolchainEnvironment.RunAsync(host, ["msbuild", solution, "-m:2", "-nodeReuse:false", "-v:quiet"],
                    fixture, cancellation.Token, DotNetSdkEnvironment.Create(host));
                string first = File.ReadAllText(Path.Combine(fixture, "First.owner")).Trim();
                Assert.True(Guid.TryParseExact(first, "N", out _));
                Assert.Equal(first, File.ReadAllText(Path.Combine(fixture, "Second.owner")).Trim());
                Assert.NotEqual(previous, first);
                previous = first;
            }
        }
        finally
        {
            Directory.Delete(fixture, recursive: true);
        }
    }
}
