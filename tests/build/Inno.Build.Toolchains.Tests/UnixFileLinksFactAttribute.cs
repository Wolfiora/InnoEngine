using System;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class UnixFileLinksFactAttribute : FactAttribute
{
    public UnixFileLinksFactAttribute()
    {
        if (OperatingSystem.IsWindows())
        {
            this.Skip = "Unix compiler file-link semantics require a Unix filesystem; Windows directory-link rejection is tested separately.";
        }
    }
}
