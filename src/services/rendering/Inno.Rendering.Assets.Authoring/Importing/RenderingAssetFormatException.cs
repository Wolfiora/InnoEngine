using System;
using Inno.Extensibility.Types;
using Inno.Rendering;

namespace Inno.Rendering.Assets.Authoring;

[StableTypeID("d9c542b3-2def-59a3-a8a5-7115eaa1e0ea")]
internal sealed class RenderingAssetFormatException : FormatException
{
    internal RenderingAssetFormatException(
        string path,
        string message
    ) : base($"{path}: {message}") {}
}
