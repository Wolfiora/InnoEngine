using System;
using Inno.Rendering.Shaders;

namespace Inno.Build.Toolchains.Bgfx.Tools;

internal sealed class BgfxSourceSyntaxException(
    string message,
    ShaderSourcePosition position
) : Exception(message)
{
    internal ShaderSourcePosition position { get; } = position;
}
