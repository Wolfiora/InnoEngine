using Inno.Rendering;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Assets;

/// <summary>
/// Exposes one or more portable image artifacts owned by an imported asset.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("a067587f-d6a5-56ed-860b-9ad0c9e1f8cb")]
public interface IRenderTextureArtifactSource
{
    /// <summary>
    /// Gets the complete immutable set of stable texture slots owned by the current asset content.
    /// </summary>
    IReadOnlyList<RenderTextureArtifactSlot> textureArtifacts { get; }
}

