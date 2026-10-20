using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Cli;
using Xunit;

namespace Inno.Build.Tests;

[Collection("Build pipeline serialization")]
public sealed class ProjectCommandSourceTests
{
    [Fact]
    public async Task InvalidExplicitSourceIsRejectedBeforeToolsAndProjectStateAreCreated()
    {
        string owner = Path.Combine(Path.GetTempPath(), "InnoProjectCommandSources", Guid.NewGuid().ToString("N"));
        string project = Path.Combine(owner, "Project");
        string output = Path.Combine(owner, "Scripts");
        Directory.CreateDirectory(owner);
        TextWriter previous = Console.Error;
        using var errors = new StringWriter();
        try
        {
            Console.SetError(errors);
            int result = await BuildApplication.Run(
                ["scripts", "--engine-root", owner, "--tools-target", "unresolved-tools",
                    "--project", project, "--output", output],
                CancellationToken.None);

            Assert.Equal(1, result);
            Assert.Contains("Engine checkout is unavailable", errors.ToString());
            Assert.Contains(owner, errors.ToString());
            Assert.False(Directory.Exists(project));
            Assert.False(Directory.Exists(output));
        }
        finally
        {
            Console.SetError(previous);
            Directory.Delete(owner, recursive: true);
        }
    }
}
