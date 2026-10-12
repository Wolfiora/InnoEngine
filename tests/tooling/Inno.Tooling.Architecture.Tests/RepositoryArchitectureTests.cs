using System;
using System.IO;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class RepositoryArchitectureTests
{
    [Fact]
    public void CurrentRepositoryPassesArchitectureRules()
    {
        string root = FindRepository();
        (int exitCode, string output) = ArchitectureCommandFixture.Run(root);
        Assert.True(exitCode == 0, output);
    }

    private static string FindRepository()
    {
        for (DirectoryInfo? directory = new(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
            if (File.Exists(Path.Combine(directory.FullName, "InnoEngine.sln")))
                return directory.FullName;
        throw new DirectoryNotFoundException("The architecture gate requires the engine source checkout.");
    }
}
