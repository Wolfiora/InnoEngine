using System;
using Xunit;

namespace Inno.Build.Tests;

public sealed class UnixFileLinksFactAttribute : FactAttribute
{
    public UnixFileLinksFactAttribute()
    {
        if (OperatingSystem.IsWindows())
            Skip = "Unix compiler file-link semantics require a Unix filesystem; Windows directory-link rejection is tested separately.";
    }
}
