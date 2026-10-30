using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

public sealed class TestRepositoryInputsTests
{
    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task SdkRecordsItsEvaluatedArtifactsOwner(
        bool useArtifacts,
        bool explicitOwner
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoTestInputs", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            string repository = TestRepository.GetRoot(typeof(TestRepositoryInputsTests).Assembly);
            string import = SecurityElement.Escape(Path.Combine(repository, "tests/RepositoryInput.targets"));
            string project = Path.Combine(root, "Probe.csproj");
            await File.WriteAllTextAsync(project, $"""
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup><TargetFramework>net10.0</TargetFramework><ImplicitUsings>disable</ImplicitUsings></PropertyGroup>
                  <Import Project="{import}" />
                </Project>
                """);
            string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
            var arguments = new List<string>
            {
                "msbuild", project, "-nologo", "-nr:false", "-t:GetAssemblyAttributes",
                "-getProperty:ArtifactsPath", "-getItem:AssemblyAttribute",
                "-p:UseArtifactsOutput=" + useArtifacts.ToString().ToLowerInvariant()
            };
            if (explicitOwner)
                arguments.Add("-p:ArtifactsPath=" + Path.Combine(root, "independent"));
            using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            string output = await ToolchainEnvironment.CaptureOutputAsync(dotnet, arguments,
                root, timeout.Token, DotNetSdkEnvironment.Create(dotnet));
            using JsonDocument result = JsonDocument.Parse(output);
            string? artifacts = result.RootElement.GetProperty("Properties").GetProperty("ArtifactsPath").GetString();
            JsonElement[] declarations = result.RootElement.GetProperty("Items").GetProperty("AssemblyAttribute").EnumerateArray()
                .Where(item => item.TryGetProperty("_Parameter1", out var key) && key.GetString() == "Inno.Testing.ArtifactsPath").ToArray();
            if (useArtifacts)
            {
                Assert.False(string.IsNullOrWhiteSpace(artifacts));
                Assert.Equal(Path.GetFullPath(artifacts!), Path.TrimEndingDirectorySeparator(
                    Assert.Single(declarations).GetProperty("_Parameter2").GetString()!));
            }
            else
            {
                Assert.Empty(declarations);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void AbsentArtifactsMetadataPreservesNormalSdkOutputSelection()
        => Assert.Null(TestRepository.GetArtifactsPath(CreateAssembly()));

    [Fact]
    public void ExplicitArtifactsMetadataPreservesTheRecordedAbsoluteOwner()
    {
        string owner = Path.Combine(Path.GetTempPath(), "owned-sdk-output");
        Assert.Equal(Path.GetFullPath(owner), TestRepository.GetArtifactsPath(CreateAssembly(owner)));
    }

    [Fact]
    public void RelativeEmptyOrDuplicateArtifactsDeclarationsAreRejected()
    {
        Assert.Throws<InvalidOperationException>(() => TestRepository.GetArtifactsPath(CreateAssembly("relative-owner")));
        Assert.Throws<InvalidOperationException>(() => TestRepository.GetArtifactsPath(CreateAssembly(string.Empty)));
        string owner = Path.Combine(Path.GetTempPath(), "owned-sdk-output");
        Assert.Throws<InvalidOperationException>(() => TestRepository.GetArtifactsPath(CreateAssembly(owner, owner)));
        Assert.Throws<ArgumentNullException>(() => TestRepository.GetArtifactsPath(null!));
    }

    private static Assembly CreateAssembly(params string[] owners)
    {
        AssemblyBuilder assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName("RepositoryInputs." + Guid.NewGuid().ToString("N")), AssemblyBuilderAccess.RunAndCollect);
        ConstructorInfo constructor = typeof(AssemblyMetadataAttribute).GetConstructor([typeof(string), typeof(string)])!;
        foreach (string owner in owners)
            assembly.SetCustomAttribute(new CustomAttributeBuilder(constructor, ["Inno.Testing.ArtifactsPath", owner]));
        return assembly;
    }
}
