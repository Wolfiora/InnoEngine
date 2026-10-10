using System;
using Inno.Build.Linux;
using System.Collections.Generic;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;

namespace Inno.Integration.Linux.Bgfx;

internal sealed class LinuxBgfxBuildProfile : BgfxNativeBuildProfile
{
    private readonly string m_targetId;

    internal LinuxBgfxBuildProfile(string targetId) => m_targetId = targetId;

    /// <inheritdoc />
    public override string targetId => m_targetId;
    /// <inheritdoc />
    public override IReadOnlyList<string> generatorArguments => m_targetId == "linux-x64" ? ["--gcc=linux-gcc", "gmake"] : ["--gcc=linux-arm-gcc", "gmake"];
    /// <inheritdoc />
    public override string artifactPathToken => m_targetId == "linux-x64" ? "/linux64_gcc/bin/" : "/linux32_arm_gcc/bin/";

    /// <inheritdoc />
    public override BgfxBuildInvocation CreateBuildInvocation(
        NativeBuildContext context,
        bool includeTools
    ) {
        bool x64 = m_targetId == "linux-x64";
        string compiler = x64 ? "linux-gcc" : "linux-arm-gcc";
        string[] arguments = ["-R", "-C", ".build/projects/gmake-" + compiler,
            "config=" + context.configuration + (x64 ? "64" : string.Empty)];
        return new("make", includeTools
            ? [.. arguments, "shaderc", "texturec", "geometryc", "geometryv", "texturev"] : arguments);
    }
}
