using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains.Bgfx.Platforms;

internal sealed class WindowsX64BgfxBuilder : BgfxBuilder
{
    internal override string outputPlatform => "windows-x64";
    internal override string artifactPathToken => "/win64_vs2022/bin/";
    internal override bool IsSupported() => OperatingSystem.IsWindows()
        && RuntimeInformation.ProcessArchitecture == Architecture.X64;

    internal override async Task BuildAsync(
        string source,
        string genie,
        NativeBuildContext context,
        bool includeTools,
        CancellationToken cancellationToken
    ) {
        string[] generate = includeTools ? ["--with-shared-lib", "--with-tools", "vs2022"]
            : ["--with-shared-lib", "vs2022"];
        await ToolchainEnvironment.RunAsync(context, genie, generate, source, cancellationToken).ConfigureAwait(false);
        string buildType = context.configuration == "debug" ? "Debug" : "Release";
        string sdk = context.hostToolchain!.environment["WindowsSDKVersion"].TrimEnd('\\', '/');
        await ToolchainEnvironment.RunAsync(context, "msbuild",
            [Path.Combine(source, ".build", "projects", "vs2022", "bgfx.sln"), "/m:1", "/nodeReuse:false",
                "/p:Configuration=" + buildType, "/p:Platform=x64", "/p:WindowsTargetPlatformVersion=" + sdk],
            source, cancellationToken).ConfigureAwait(false);
    }
}
