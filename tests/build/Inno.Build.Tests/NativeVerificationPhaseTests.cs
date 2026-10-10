using System;
using System.IO;
using System.Linq;
using System.Threading;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeVerificationPhaseTests
{
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
            var inputs = new[] { new NativeBuildInput("first-owner", file), new NativeBuildInput("second-owner", file) };
            string initial = NativeBuildFingerprint.Create(context, ["recipe"], inputs, CancellationToken.None);
            Assert.Equal(1, context.statistics.hashedFiles);
            DateTime timestamp = File.GetLastWriteTimeUtc(file);
            File.WriteAllText(file, "other");
            File.SetLastWriteTimeUtc(file, timestamp);
            var verification = context.BeginInputVerification("after-lock");
            string verified = NativeBuildFingerprint.Create(verification, ["recipe"], inputs, CancellationToken.None);
            Assert.NotEqual(initial, verified);
            Assert.Equal(verified, NativeBuildFingerprint.Create(verification, ["recipe"], inputs, CancellationToken.None));
            var phase = context.statistics.phases.Single(phase => phase.phase == "after-lock");
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
