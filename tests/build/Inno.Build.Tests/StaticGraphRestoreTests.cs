using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class StaticGraphRestoreTests
{
    [Fact]
    public async Task ProductRestoreAcceptsLargeInheritedProperties()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        IReadOnlyDictionary<string, string?> environment = DotNetSdkEnvironment.Create(host);
        string owner = Path.Combine(Path.GetTempPath(), "InnoStaticGraphRestoreTests");
        string fixture = PathBoundary.Resolve(owner, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(fixture);
        string project = Path.Combine(root, "platforms", "Windows", "editor", "Inno.Editor.Windows", "Inno.Editor.Windows.csproj");
        string driver = Path.Combine(fixture, "Restore.proj");
        string inheritedMetadata = new('R', 65536);
        try
        {
            await File.WriteAllTextAsync(driver, $"""
                <Project>
                  <Target Name="Verify">
                    <MSBuild Projects="{SecurityElement.Escape(project)}"
                             Targets="RestoreProductTargetGraph"
                             Properties="InnoRestoreFixtureMetadata={inheritedMetadata}"
                             BuildInParallel="false" />
                  </Target>
                </Project>
                """);
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            await ToolchainEnvironment.RunAsync(host,
                ["msbuild", driver, "-t:Verify", "-m:1", "-nodeReuse:false", "-nologo", "-v:minimal"],
                root, cancellation.Token, environment);
        }
        finally
        {
            Directory.Delete(PathBoundary.RequireUnlinkedPath(owner, fixture), recursive: true);
        }
    }
}
