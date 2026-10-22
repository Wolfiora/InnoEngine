using System;
using System.IO;

namespace Inno.Integration.MacOSArm64.Build;

internal static class MacOSArm64NativeLayout
{
    internal static string? Map(
        string component,
        string relative,
        string target
    ) {
        if (relative.Split(Path.DirectorySeparatorChar)[0] == "Link")
            return null;
        if (component == "bgfx-tools")
            return relative.StartsWith("includes" + Path.DirectorySeparatorChar, StringComparison.Ordinal)
                ? Path.Combine("bgfx", relative) : Path.Combine("bgfx", target, "tools", relative);
        return Path.Combine(component, target, relative);
    }
}
