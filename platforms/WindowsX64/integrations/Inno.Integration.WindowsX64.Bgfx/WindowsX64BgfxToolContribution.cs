using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Build.Toolchains;

namespace Inno.Integration.WindowsX64.Bgfx;

internal static class WindowsX64BgfxToolContribution
{
    internal static NativeToolchainSelection Select(
        NativeBuildContext context,
        bool includeTools
    ) {
        NativeToolchainSelection sdk = context.RequireToolchain();
        if (sdk.host.system != "Windows")
            throw new PlatformNotSupportedException("This BGFX integration requires a Windows tool host.");
        string generator;
        if (!sdk.TryResolveExecutable("genie", out generator))
        {
            generator = Path.Combine(context.engineRoot, "extern", "bx", "tools", "bin", "windows", "genie.exe");
        }
        string owner = Path.Combine(context.engineRoot, "platforms", "WindowsX64", "integrations", "Inno.Integration.WindowsX64.Bgfx");
        var tools = new Dictionary<string, string>
        {
            ["cl"] = sdk.ResolveExecutable("cl"),
            ["msbuild"] = sdk.ResolveExecutable("msbuild"),
            ["genie"] = generator
        };
        string dotnet = sdk.ResolveExecutable("dotnet");
        string cmake = sdk.ResolveExecutable("cmake");
        if (!includeTools)
            tools.Add("cmake", cmake);
        return new NativeToolchainSelection(sdk.targetId, sdk.host, tools, sdk.environment,
            sdk.inputPaths.Where(path => path != dotnet && (!includeTools || path != cmake))
                .Concat([Path.Combine(owner, "WindowsX64BgfxToolContribution.cs"), Path.Combine(owner, "WindowsX64BgfxBuildProfile.cs")]),
            includeTools ? [] : sdk.cmakeArguments, sdk.linkPlan, sdk.multiConfiguration, sdk.compilerInputs);
    }
}
