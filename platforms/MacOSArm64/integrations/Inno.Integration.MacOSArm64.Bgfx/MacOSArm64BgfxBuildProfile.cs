using System;
using Inno.Build.MacOSArm64;
using System.Collections.Generic;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;

namespace Inno.Integration.MacOSArm64.Bgfx;

internal sealed class MacOSArm64BgfxBuildProfile : BgfxNativeBuildProfile
{
    /// <inheritdoc />
    public override NativeToolchainSelection SelectTools(
        NativeBuildContext context,
        bool includeTools
    ) => MacOSArm64BgfxToolContribution.Select(context, includeTools);

    /// <inheritdoc />
    public override string targetId => MacOSArm64BuildModule.target.id.value;
    /// <inheritdoc />
    public override IReadOnlyList<string> generatorArguments => ["--gcc=osx-arm64", "gmake"];
    /// <inheritdoc />
    public override string artifactPathToken => "/osx-arm64/bin/";

    /// <inheritdoc />
    public override IReadOnlyList<string> bindingCmakeArguments => [];

    /// <inheritdoc />
    public override (string reference, IReadOnlyList<string> arguments) CreateBindingLinkSettings(string archivePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(archivePath);
        return (archivePath, ["-Xlinker", "-force_load", "-Xlinker", "\"" + archivePath + "\""]);
    }

    /// <inheritdoc />
    public override BgfxBuildInvocation CreateBuildInvocation(
        NativeBuildContext context,
        bool includeTools
    ) {
        string[] arguments = ["-C", ".build/projects/gmake-osx-arm64", "config=" + context.configuration];
        return new("make", includeTools
            ? [.. arguments, "shaderc", "texturec", "geometryc", "geometryv", "texturev"] : arguments);
    }
}
