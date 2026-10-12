using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Cli;
using Xunit;

namespace Inno.Build.Tests;

[Collection("Build pipeline serialization")]
public sealed class ArchitectureVerificationCommandTests
{
    [Fact]
    public async Task VerifyRoutesToTheArchitectureCommandWithoutPreparingAProduct()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoVerifyCommand", Guid.NewGuid().ToString("N"));
        using var output = new StringWriter(CultureInfo.InvariantCulture);
        TextWriter previousOutput = Console.Out;
        TextWriter previousError = Console.Error;
        try
        {
            Directory.CreateDirectory(Path.Combine(root, "src", "services", "audio"));
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"),
                "Microsoft Visual Studio Solution File, Format Version 12.00");
            File.WriteAllText(Path.Combine(root, "src", "services", "audio", "Probe.cs"), "global using System;");
            Console.SetOut(output);
            Console.SetError(output);
            int result = await BuildApplication.Run(["verify", root], CancellationToken.None);
            Assert.Equal(1, result);
            Assert.Contains("global using", output.ToString(), StringComparison.Ordinal);
            Assert.DoesNotContain("INNO-TASK", output.ToString(), StringComparison.Ordinal);
            Assert.False(Directory.Exists(Path.Combine(root, "artifacts")));
        }
        finally
        {
            Console.SetOut(previousOutput);
            Console.SetError(previousError);
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }
}
