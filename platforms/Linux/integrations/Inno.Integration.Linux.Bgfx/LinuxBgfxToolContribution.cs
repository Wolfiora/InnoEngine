using System;
using System.Collections.Generic;
using System.IO;
using Inno.Build.Toolchains;

namespace Inno.Integration.Linux.Bgfx;

internal static class LinuxBgfxToolContribution
{
    internal static NativeToolchainSelection Select(NativeBuildContext context)
    {
        NativeToolchainSelection sdk = context.RequireToolchain();
        if (sdk.host.system != "Linux")
            throw new PlatformNotSupportedException("This BGFX integration requires a Linux tool host.");
        string generator;
        if (!sdk.TryResolveExecutable("genie", out generator))
        {
            if (sdk.host.architecture != "x64")
                throw new PlatformNotSupportedException("This host requires an explicitly supplied native GENie executable.");
            generator = Path.Combine(context.engineRoot, "extern", "bx", "tools", "bin", "linux", "genie");
        }
        string owner = Path.Combine(context.engineRoot, "platforms", "Linux", "integrations", "Inno.Integration.Linux.Bgfx");
        return sdk.WithTools(new Dictionary<string, string> { ["genie"] = generator },
            [Path.Combine(owner, "LinuxBgfxToolContribution.cs"), Path.Combine(owner, "LinuxBgfxBuildProfile.cs")]);
    }
}
