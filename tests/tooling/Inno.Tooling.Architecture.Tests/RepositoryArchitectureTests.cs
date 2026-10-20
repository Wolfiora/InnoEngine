using Inno.Testing.Source;
using System.Reflection;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class RepositoryArchitectureTests
{
    [Fact]
    public void CurrentRepositoryPassesArchitectureRules()
    {
        string root = TestRepository.GetRoot(typeof(RepositoryArchitectureTests).Assembly);
        string configuration = typeof(RepositoryArchitectureTests).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()!.Configuration;
        (int exitCode, string output) = ArchitectureCommandFixture.Run(root, "--configuration", configuration);
        Assert.True(exitCode == 0, output);
    }
}
