using Inno.Core.Graphs;
using Inno.Core.Serialization;

namespace Inno.Rendering.Shaders;

[ShaderGraphTemplate(ShaderBuiltInIDs.C_RASTER_TEMPLATE, "Raster")]
internal sealed class RasterShaderGraphTemplate : ShaderGraphTemplate
{
    /// <summary>
    /// Creates and validates a caller-owned value value.
    /// </summary>
    /// <param name="serialization">
    /// The serialization consumed by create; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// The validated graph document that represents the completed operation.
    /// </returns>
public override GraphDocument Create(
    SerializationRegistry serialization,
    SerializationContext context
) => ShaderGraphTemplates.CreateRaster(serialization, context);
}
