using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Build.Tests;

public sealed class DevelopmentEntryBuildTests
{
    [Theory]
    [InlineData("win-x64", false)]
    [InlineData("win-x64", true)]
    [InlineData("osx-arm64", false)]
    [InlineData("osx-arm64", true)]
    public async Task EditorEntryOwnsItsReferenceBuildWithoutEnablingDesignTimeBuilds(
        string sdkHost,
        bool designTime
    ) {
        string root = TestRepository.GetRoot(typeof(DevelopmentEntryBuildTests).Assembly);
        string dotnet = ToolchainEnvironment.ResolveExecutable("dotnet");
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        string output = await ToolchainEnvironment.CaptureOutputAsync(dotnet,
            [
                "msbuild", "entry/InnoEngine.Editor/InnoEngine.Editor.csproj", "-nologo",
                "-p:BuildProjectReferences=false", "-p:OnlyReferenceAndBuildProjectsEnabledInSolutionConfiguration=true",
                "-p:BuildingInsideVisualStudio=true",
                "-p:NETCoreSdkRuntimeIdentifier=" + sdkHost,
                "-p:DesignTimeBuild=" + designTime.ToString().ToLowerInvariant(),
                "-getProperty:BuildProjectReferences,BuildingInsideVisualStudio,OnlyReferenceAndBuildProjectsEnabledInSolutionConfiguration",
                "-getItem:ProjectReference"
            ],
            root, timeout.Token, DotNetSdkEnvironment.Create(dotnet));
        using JsonDocument document = JsonDocument.Parse(output);
        JsonElement properties = document.RootElement.GetProperty("Properties");
        Assert.Equal(designTime ? "false" : "true", properties.GetProperty("BuildProjectReferences").GetString());
        Assert.Equal(designTime ? "true" : "false", properties.GetProperty("BuildingInsideVisualStudio").GetString());
        Assert.Equal("false", properties.GetProperty("OnlyReferenceAndBuildProjectsEnabledInSolutionConfiguration").GetString());
        JsonElement[] references = document.RootElement.GetProperty("Items").GetProperty("ProjectReference")
            .EnumerateArray().ToArray();
        JsonElement product = Assert.Single(references.Where(reference => reference.GetProperty("Filename").GetString()!
            .StartsWith("Inno.Editor.", StringComparison.Ordinal)));
        Assert.Equal(sdkHost == "win-x64" ? "Inno.Editor.WindowsX64" : "Inno.Editor.MacOSArm64",
            product.GetProperty("Filename").GetString());
        foreach (JsonElement reference in references)
        {
            string declarations = reference.TryGetProperty("AdditionalProperties", out JsonElement value)
                ? value.GetString() ?? string.Empty : string.Empty;
            Assert.Equal(!designTime, declarations.Split(';').Contains("BuildProjectReferences=true", StringComparer.Ordinal));
            Assert.Equal(!designTime, declarations.Split(';').Contains("BuildingInsideVisualStudio=false", StringComparer.Ordinal));
        }
    }
}
