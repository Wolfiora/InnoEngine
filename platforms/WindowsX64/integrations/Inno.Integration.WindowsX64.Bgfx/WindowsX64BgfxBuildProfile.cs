using System;
using Inno.Build.WindowsX64;
using System.Collections.Generic;
using System.IO;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;

namespace Inno.Integration.WindowsX64.Bgfx;

internal sealed class WindowsX64BgfxBuildProfile : BgfxNativeBuildProfile
{
    /// <inheritdoc />
    public override NativeToolchainSelection SelectTools(
        NativeBuildContext context,
        bool includeTools
    ) => WindowsX64BgfxToolContribution.Select(context, includeTools);

    /// <inheritdoc />
    public override string targetId => WindowsX64BuildModule.target.id.value;
    /// <inheritdoc />
    public override IReadOnlyList<string> generatorArguments => ["vs2022"];
    /// <inheritdoc />
    public override string artifactPathToken => "/win64_vs2022/bin/";

    /// <inheritdoc />
    public override IReadOnlyList<string> bindingCmakeArguments =>
        ["-DCMAKE_MSVC_RUNTIME_LIBRARY=MultiThreaded$<$<CONFIG:Debug>:Debug>"];

    /// <inheritdoc />
    public override (string reference, IReadOnlyList<string> arguments) CreateBindingLinkSettings(string archivePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(archivePath);
        // The Visual Studio generator appends the target's archive suffix to library references.
        return (Path.ChangeExtension(archivePath, null), ["/WHOLEARCHIVE:\"" + archivePath + "\""]);
    }

    /// <inheritdoc />
    public override BgfxBuildInvocation CreateBuildInvocation(
        NativeBuildContext context,
        bool includeTools
    ) {
        string sdk = context.RequireToolchain().environment["WindowsSDKVersion"].TrimEnd('\\', '/');
        string configuration = context.configuration == "debug" ? "Debug" : "Release";
        return new("msbuild", [".build/projects/vs2022/bgfx.sln", "/m:1", "/nodeReuse:false",
            "/p:Configuration=" + configuration, "/p:Platform=x64", "/p:WindowsTargetPlatformVersion=" + sdk]);
    }
}
