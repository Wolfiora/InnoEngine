using System;
using Inno.Build.MacOS;
using System.Collections.Generic;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;

namespace Inno.Integration.MacOS.Bgfx;

internal sealed class MacOSBgfxBuildProfile : BgfxNativeBuildProfile
{
    /// <inheritdoc />
    public override string targetId => MacOSBuildModule.target.id.value;
    /// <inheritdoc />
    public override IReadOnlyList<string> generatorArguments => ["--gcc=osx-arm64", "gmake"];
    /// <inheritdoc />
    public override string artifactPathToken => "/osx-arm64/bin/";

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
