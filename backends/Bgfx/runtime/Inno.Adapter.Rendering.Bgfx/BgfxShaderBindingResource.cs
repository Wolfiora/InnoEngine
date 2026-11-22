using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed class BgfxShaderBindingResource
{
    /// <summary>
    /// Creates a validated bgfx shader binding resource instance.
    /// </summary>
    /// <param name="descriptor">
    /// The descriptor consumed by bgfx shader binding resource; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="uniform">
    /// The uniform consumed by bgfx shader binding resource; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public BgfxShaderBindingResource(
        RenderShaderBindingDescriptor descriptor,
        bgfx.UniformHandle uniform
    ) {
        this.descriptor = descriptor;
        this.uniform = uniform;
    }

    /// <summary>
    /// Gets the backend-neutral descriptor used to validate and recreate this resource.
    /// </summary>
    public RenderShaderBindingDescriptor descriptor { get; }

    /// <summary>
    /// Gets the BGFX uniform handle backing this shader binding.
    /// </summary>
    public bgfx.UniformHandle uniform { get; }
}
