using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inno.Native.Bgfx;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed class BgfxSurfaceRetirement
{
    internal readonly List<bgfx.FrameBufferHandle> pending = [];
    internal readonly TaskCompletionSource completion = new(TaskCreationOptions.RunContinuationsAsynchronously);
    internal ulong lastSubmission;
    internal bool closing;
    internal Exception? failure;
}
