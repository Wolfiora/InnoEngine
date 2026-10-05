using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Inno.Build.Toolchains.Bgfx.Platforms;

internal sealed class OsxArm64BgfxBuilder : BgfxBuilder
{
    internal override string outputPlatform => "osx-arm64";
    internal override string artifactPathToken => "/osx-arm64/bin/";
    internal override bool IsSupported() => OperatingSystem.IsMacOS()
        && RuntimeInformation.ProcessArchitecture == Architecture.Arm64;

    internal override async Task BuildAsync(
        string source,
        string genie,
        NativeBuildContext context,
        bool includeTools,
        CancellationToken cancellationToken
    ) {
        string[] generate = includeTools ? ["--with-shared-lib", "--with-tools", "--gcc=osx-arm64", "gmake"]
            : ["--with-shared-lib", "--gcc=osx-arm64", "gmake"];
        await ToolchainEnvironment.RunAsync(context, genie, generate, source, cancellationToken).ConfigureAwait(false);
        string[] compile = includeTools
            ? ["-C", ".build/projects/gmake-osx-arm64", "config=" + context.configuration,
                "shaderc", "texturec", "geometryc", "geometryv", "texturev"]
            : ["-C", ".build/projects/gmake-osx-arm64", "config=" + context.configuration];
        await ToolchainEnvironment.RunAsync(context, "make", compile, source, cancellationToken).ConfigureAwait(false);
    }
}
