using System;
using Inno.Native.Bgfx;

namespace Inno.Adapter.Rendering.Bgfx;

internal static unsafe class BgfxSurfaceBinding
{
    internal static BgfxSurfaceDescriptor Validate(BgfxSurfaceDescriptor descriptor)
    {
        if (descriptor.windowHandle == 0)
            throw new ArgumentException("The surface integration returned an invalid window descriptor.");
        return descriptor;
    }

    internal static void Apply(
        ref bgfx.Init initialization,
        BgfxSurfaceDescriptor descriptor
    ) {
        initialization.platformData.nwh = descriptor.windowHandle.ToPointer();
        initialization.platformData.ndt = descriptor.displayHandle.ToPointer();
    }
}
