using System;
using System.Collections.Generic;
using System.IO;
using Inno.Build.Toolchains;

namespace Inno.Integration.MacOS.Bgfx;

internal static class MacOSBgfxToolContribution
{
    internal static NativeToolchainSelection Select(NativeBuildContext context)
    {
        NativeToolchainSelection sdk = context.RequireToolchain();
        if (sdk.host.system != "MacOS")
            throw new PlatformNotSupportedException("This BGFX integration requires a MacOS tool host.");
        string generator;
        if (!sdk.TryResolveExecutable("genie", out generator))
        {
            generator = Path.Combine(context.engineRoot, "extern", "bx", "tools", "bin", "darwin", "genie");
        }
        string owner = Path.Combine(context.engineRoot, "platforms", "MacOS", "integrations", "Inno.Integration.MacOS.Bgfx");
        return sdk.WithTools(new Dictionary<string, string> { ["genie"] = generator },
            [Path.Combine(owner, "MacOSBgfxToolContribution.cs"), Path.Combine(owner, "MacOSBgfxBuildProfile.cs")]);
    }
}
