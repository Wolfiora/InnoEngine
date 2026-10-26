using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Build.Toolchains;

namespace Inno.Integration.MacOSArm64.Bgfx;

internal static class MacOSArm64BgfxToolContribution
{
    internal static NativeToolchainSelection Select(
        NativeBuildContext context,
        bool includeTools
    ) {
        NativeToolchainSelection sdk = context.RequireToolchain();
        if (sdk.host.system != "MacOS")
            throw new PlatformNotSupportedException("This BGFX integration requires a MacOS tool host.");
        string generator;
        if (!sdk.TryResolveExecutable("genie", out generator))
        {
            generator = Path.Combine(context.engineRoot, "extern", "bx", "tools", "bin", "darwin", "genie");
        }
        string owner = Path.Combine(context.engineRoot, "platforms", "MacOSArm64", "integrations", "Inno.Integration.MacOSArm64.Bgfx");
        var tools = new Dictionary<string, string>
        {
            ["clang"] = sdk.ResolveExecutable("clang"),
            ["clang++"] = sdk.ResolveExecutable("clang++"),
            ["make"] = sdk.ResolveExecutable("make"),
            ["genie"] = generator
        };
        string dotnet = sdk.ResolveExecutable("dotnet");
        string cmake = sdk.ResolveExecutable("cmake");
        if (!includeTools)
            tools.Add("cmake", cmake);
        return new NativeToolchainSelection(sdk.targetId, sdk.host, tools, sdk.environment,
            sdk.inputPaths.Where(path => path != dotnet && (!includeTools || path != cmake))
                .Concat([Path.Combine(owner, "MacOSArm64BgfxToolContribution.cs"), Path.Combine(owner, "MacOSArm64BgfxBuildProfile.cs")]),
            includeTools ? [] : sdk.cmakeArguments, sdk.linkPlan, sdk.multiConfiguration, sdk.compilerInputs);
    }
}
