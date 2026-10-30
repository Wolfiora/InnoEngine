using Inno.Testing.Source;
using Inno.Build.Toolchains;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class RepositoryArchitectureTests
{
    [Fact]
    [Trait("BuildGraph", "Complete")]
    public void CurrentRepositoryPassesArchitectureRules()
    {
        string root = TestRepository.GetRoot(typeof(RepositoryArchitectureTests).Assembly);
        string configuration = typeof(RepositoryArchitectureTests).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()!.Configuration;
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        string? artifacts = TestRepository.GetArtifactsPath(typeof(RepositoryArchitectureTests).Assembly);
        var arguments = new List<string> { "--configuration", configuration, "--dotnet", dotnet };
        if (!string.IsNullOrWhiteSpace(artifacts))
            arguments.AddRange(["--artifacts-path", artifacts]);
        (int exitCode, string output) = ArchitectureCommandFixture.RunComplete(root, arguments.ToArray());
        Assert.True(exitCode == 0, output);
    }
}
