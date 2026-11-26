using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Integration.Tests;

public sealed class NativeVerificationPhaseTests
{
    [Fact]
    public async Task DifferentDirectoryAliasesKeepLogicalMappingsButShareOnePhysicalRead()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoPhysicalRead", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        string alias = Path.Combine(root, "alias");
        try
        {
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "fixture");
            string physical = Path.Combine(root, "source");
            Directory.CreateDirectory(physical);
            File.WriteAllText(Path.Combine(physical, "input.h"), "bytes");
            if (OperatingSystem.IsWindows())
            {
                await ToolchainEnvironment.RunAsync(
                    "cmd.exe",
                    ["/d", "/c", "mklink", "/J", alias, physical],
                    root,
                    cancellationToken: CancellationToken.None
                );
            }
            else
            {
                Directory.CreateSymbolicLink(alias, physical);
            }
            var context = new NativeBuildContext(root, "release");
            _ = NativeBuildFingerprint.Create(
                context,
                [],
                [new NativeBuildInput("physical", physical), new NativeBuildInput("alias", alias)],
                CancellationToken.None
            );
            Assert.Equal(1, context.statistics.hashedFiles);
            Assert.Equal(5, context.statistics.hashedBytes);
            Assert.True(context.statistics.phases.Single().reusedReads > 0);
        }
        finally
        {
            if (Directory.Exists(alias))
            {
                Directory.Delete(alias, recursive: false);
            }
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void FreshPhasesDetectSameLengthSameTimestampChangesAndReuseSharedReads()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoVerificationPhase", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), string.Empty);
            string file = Path.Combine(root, "input.txt");
            File.WriteAllText(file, "first");
            var context = new NativeBuildContext(root, "release");
            NativeBuildInput[] inputs = [new NativeBuildInput("first-owner", file), new NativeBuildInput("second-owner", file)];
            string initial = NativeBuildFingerprint.Create(context, ["recipe"], inputs, CancellationToken.None);
            Assert.Equal(1, context.statistics.hashedFiles);
            DateTime timestamp = File.GetLastWriteTimeUtc(file);
            File.WriteAllText(file, "other");
            File.SetLastWriteTimeUtc(file, timestamp);
            NativeBuildContext verification = context.BeginInputVerification("after-lock");
            string verified = NativeBuildFingerprint.Create(verification, ["recipe"], inputs, CancellationToken.None);
            Assert.NotEqual(initial, verified);
            Assert.Equal(verified, NativeBuildFingerprint.Create(verification, ["recipe"], inputs, CancellationToken.None));
            NativeBuildPhaseStatistics phase = context.statistics.phases.Single(phase => phase.phase == "after-lock");
            Assert.Equal(1, phase.files);
            Assert.Equal(5, phase.bytes);
            Assert.Equal(3, phase.reusedReads);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
