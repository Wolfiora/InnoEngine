using Inno.Rendering;
using System;

namespace Inno.Rendering.Assets.Authoring;

[Inno.Extensibility.Types.StableTypeId("d9c542b3-2def-59a3-a8a5-7115eaa1e0ea")]
internal sealed class RenderingAssetFormatException : FormatException
{
    internal RenderingAssetFormatException(
        string path,
        string message
    )
        : base($"{path}: {message}")
    {
    }
}

