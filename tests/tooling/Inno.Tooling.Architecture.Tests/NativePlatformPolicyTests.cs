using System;
using System.IO;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class NativePlatformPolicyTests
{
    [Theory]
    [InlineData("{}", false)]
    [InlineData("{\"source\":{\"includeDirectories\":[\"../Native/include\"]}}", false)]
    [InlineData("{\"targets\":[]}", true)]
    [InlineData("{\"extends\":\"common.json\"}", true)]
    [InlineData("{\"sdk\":{}}", true)]
    [InlineData("{\"source\":{\"systemIncludeDirectories\":[]}}", true)]
    public void ComponentConfigurationCannotOwnPlatformPolicy(
        string configuration,
        bool rejected
    ) {
        Run(configuration, false, rejected);
    }

    [Fact]
    public void ComponentCannotKeepAnAdditionalTargetPreset() => Run("{}", true, true);

    private static void Run(
        string configuration,
        bool targetPreset,
        bool rejected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoNativePolicyTests", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            string bindings = Path.Combine(root, "backends/Probe/native/Inno.Native.Probe/Bindings");
            Directory.CreateDirectory(bindings);
            File.WriteAllText(Path.Combine(bindings, "bindings.props"), "<Project />");
            File.WriteAllText(Path.Combine(bindings, "bindgen.json"), configuration);
            if (targetPreset)
                File.WriteAllText(Path.Combine(bindings, "bindgen.probe-target.json"), "{}");
            (_, string output) = ArchitectureCommandFixture.Run(root);
            bool violation = output.Contains("belongs to a platform Profile", StringComparison.Ordinal)
                || output.Contains("must be supplied by the platform provider", StringComparison.Ordinal)
                || output.Contains("platform binding presets belong", StringComparison.Ordinal);
            Assert.Equal(rejected, violation);
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }
}
