using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains.Bgfx.Platforms;

internal sealed class LinuxBgfxBuilder : BgfxBuilder
{
    internal override string outputPlatform => RuntimeInformation.ProcessArchitecture == Architecture.X64
        ? "linux-x64" : "linux-arm64";
    internal override string artifactPathToken => RuntimeInformation.ProcessArchitecture == Architecture.X64
        ? "/linux64_gcc/bin/" : "/linux32_arm_gcc/bin/";
    internal override bool IsSupported() => OperatingSystem.IsLinux()
        && RuntimeInformation.ProcessArchitecture is Architecture.X64 or Architecture.Arm64;

    internal override async Task BuildAsync(
        string source,
        string genie,
        NativeBuildContext context,
        bool includeTools,
        CancellationToken cancellationToken
    ) {
        bool x64 = RuntimeInformation.ProcessArchitecture == Architecture.X64;
        string compiler = x64 ? "linux-gcc" : "linux-arm-gcc";
        var generate = new List<string> { "--with-shared-lib", "--gcc=" + compiler, "gmake" };
        if (includeTools)
            generate.Insert(0, "--with-tools");
        await ToolchainEnvironment.RunAsync(context, genie, generate, source, cancellationToken).ConfigureAwait(false);
        var compile = new List<string> { "-R", "-C", ".build/projects/gmake-" + compiler,
            "config=" + context.configuration + (x64 ? "64" : string.Empty) };
        if (includeTools)
            compile.AddRange(["shaderc", "texturec", "geometryc", "geometryv", "texturev"]);
        await ToolchainEnvironment.RunAsync(context, "make", compile, source, cancellationToken).ConfigureAwait(false);
    }
}
