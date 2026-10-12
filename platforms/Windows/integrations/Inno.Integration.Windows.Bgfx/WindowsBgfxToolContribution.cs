using System;
using System.Collections.Generic;
using System.IO;
using Inno.Build.Toolchains;

namespace Inno.Integration.Windows.Bgfx;

internal static class WindowsBgfxToolContribution
{
    internal static NativeToolchainSelection Select(NativeBuildContext context)
    {
        NativeToolchainSelection sdk = context.RequireToolchain();
        if (sdk.host.system != "Windows")
            throw new PlatformNotSupportedException("This BGFX integration requires a Windows tool host.");
        string generator;
        if (!sdk.TryResolveExecutable("genie", out generator))
        {
            generator = Path.Combine(context.engineRoot, "extern", "bx", "tools", "bin", "windows", "genie.exe");
        }
        string owner = Path.Combine(context.engineRoot, "platforms", "Windows", "integrations", "Inno.Integration.Windows.Bgfx");
        return sdk.WithTools(new Dictionary<string, string> { ["genie"] = generator },
            [Path.Combine(owner, "WindowsBgfxToolContribution.cs"), Path.Combine(owner, "WindowsBgfxBuildProfile.cs")]);
    }
}
