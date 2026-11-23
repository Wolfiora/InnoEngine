using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Encodes a resource-ready draw into the owning model's scene pass.
/// </summary>
[StableTypeID("d17ebef7-c2f5-543e-ad05-c38c6396ac3b")]
public interface IPreparedViewDrawable
{
    /// <summary>
    /// Encodes commands while the owning model controls attachments and view state.
    /// </summary>
    /// <param name="commands">
    /// Backend-neutral scene pass encoder.
    /// </param>
    void Encode(RenderCommandEncoder commands);
}
