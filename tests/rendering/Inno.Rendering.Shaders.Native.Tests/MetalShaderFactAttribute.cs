using System;
using Xunit;

namespace Inno.Rendering.Shaders.Native.Tests;

public sealed class MetalShaderFactAttribute : FactAttribute
{
    public MetalShaderFactAttribute()
    {
        if (!OperatingSystem.IsMacOS())
        {
            this.Skip = "Requires the macOS BGFX Metal compiler; Windows validation is explicitly deferred.";
        }
    }
}
