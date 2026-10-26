using Inno.Native.Bgfx;

namespace Inno.Adapter.Rendering.Bgfx;

/// <summary>
/// Adapts borrowed backend values to the generated pointer ABI for one synchronous call.
/// </summary>
internal static unsafe class BgfxNativeCalls
{
    internal static void AttachmentInit(
        bgfx.Attachment* attachment,
        bgfx.TextureHandle handle,
        bgfx.Access access,
        ushort layer,
        ushort numLayers,
        ushort mip,
        byte resolve
    ) {
        bgfx.Raw.attachment_init(attachment, &handle, access, layer, numLayers, mip, resolve);
    }

    internal static bgfx.DynamicVertexBufferHandle CreateDynamicVertexBuffer(
        uint count,
        bgfx.VertexLayout* layout,
        ushort flags
    ) {
        bgfx.DynamicVertexBufferHandle result = default;
        bgfx.Raw.create_dynamic_vertex_buffer(count, layout, flags, &result);
        return result;
    }

    internal static bgfx.FrameBufferHandle CreateFrameBufferFromAttachment(
        byte count,
        bgfx.Attachment* attachment,
        byte destroyTexture
    ) {
        bgfx.FrameBufferHandle result = default;
        bgfx.Raw.create_frame_buffer_from_attachment(count, attachment, destroyTexture, &result);
        return result;
    }

    internal static bgfx.FrameBufferHandle CreateFrameBufferFromNwh(
        void* nwh,
        ushort width,
        ushort height,
        bgfx.TextureFormat format,
        bgfx.TextureFormat depthFormat
    ) {
        bgfx.FrameBufferHandle result = default;
        bgfx.Raw.create_frame_buffer_from_nwh(nwh, width, height, format, depthFormat, &result);
        return result;
    }

    internal static bgfx.IndexBufferHandle CreateIndexBuffer(
        bgfx.Memory* memory,
        ushort flags
    ) {
        bgfx.IndexBufferHandle result = default;
        bgfx.Raw.create_index_buffer(memory, flags, &result);
        return result;
    }

    internal static bgfx.ShaderHandle CreateShader(bgfx.Memory* memory) {
        bgfx.ShaderHandle result = default;
        bgfx.Raw.create_shader(memory, &result);
        return result;
    }

    internal static bgfx.TextureHandle CreateTexture(
        bgfx.Memory* memory,
        ulong flags,
        byte skip,
        bgfx.TextureInfo* info
    ) {
        bgfx.TextureHandle result = default;
        bgfx.Raw.create_texture(memory, flags, skip, info, &result);
        return result;
    }

    internal static bgfx.TextureHandle CreateTexture2d(
        ushort width,
        ushort height,
        byte hasMips,
        ushort numLayers,
        bgfx.TextureFormat format,
        ulong flags,
        bgfx.Memory* memory,
        ulong external
    ) {
        bgfx.TextureHandle result = default;
        bgfx.Raw.create_texture_2d(width, height, hasMips, numLayers, format, flags, memory, external, &result);
        return result;
    }

    internal static bgfx.TextureHandle CreateTexture3d(
        ushort width,
        ushort height,
        ushort depth,
        byte hasMips,
        bgfx.TextureFormat format,
        ulong flags,
        bgfx.Memory* memory,
        ulong external
    ) {
        bgfx.TextureHandle result = default;
        bgfx.Raw.create_texture_3d(width, height, depth, hasMips, format, flags, memory, external, &result);
        return result;
    }

    internal static bgfx.TextureHandle CreateTextureCube(
        ushort size,
        byte hasMips,
        ushort numLayers,
        bgfx.TextureFormat format,
        ulong flags,
        bgfx.Memory* memory,
        ulong external
    ) {
        bgfx.TextureHandle result = default;
        bgfx.Raw.create_texture_cube(size, hasMips, numLayers, format, flags, memory, external, &result);
        return result;
    }

    internal static bgfx.VertexBufferHandle CreateVertexBuffer(
        bgfx.Memory* memory,
        bgfx.VertexLayout* layout,
        ushort flags
    ) {
        bgfx.VertexBufferHandle result = default;
        bgfx.Raw.create_vertex_buffer(memory, layout, flags, &result);
        return result;
    }

    internal static bgfx.VertexLayoutHandle CreateVertexLayout(bgfx.VertexLayout* layout) {
        bgfx.VertexLayoutHandle result = default;
        bgfx.Raw.create_vertex_layout(layout, &result);
        return result;
    }

    internal static void EncoderBlit(
        bgfx.Encoder* encoder,
        ushort id,
        bgfx.TextureHandle dst,
        byte dstMip,
        ushort dstX,
        ushort dstY,
        ushort dstZ,
        bgfx.TextureHandle src,
        byte srcMip,
        ushort srcX,
        ushort srcY,
        ushort srcZ,
        ushort width,
        ushort height,
        ushort depth
    ) {
        bgfx.Raw.encoder_blit(
            encoder,
            id,
            &dst,
            dstMip,
            dstX,
            dstY,
            dstZ,
            &src,
            srcMip,
            srcX,
            srcY,
            srcZ,
            width,
            height,
            depth);
    }

    internal static void EncoderDispatch(
        bgfx.Encoder* encoder,
        ushort id,
        bgfx.ProgramHandle program,
        uint numX,
        uint numY,
        uint numZ,
        byte flags
    ) {
        bgfx.Raw.encoder_dispatch(encoder, id, &program, numX, numY, numZ, flags);
    }

    internal static void EncoderDispatchIndirect(
        bgfx.Encoder* encoder,
        ushort id,
        bgfx.ProgramHandle program,
        bgfx.IndirectBufferHandle indirectHandle,
        uint start,
        uint count,
        byte flags
    ) {
        bgfx.Raw.encoder_dispatch_indirect(encoder, id, &program, &indirectHandle, start, count, flags);
    }

    internal static void EncoderSetComputeDynamicIndexBuffer(
        bgfx.Encoder* encoder,
        byte stage,
        bgfx.DynamicIndexBufferHandle handle,
        bgfx.Access access
    ) {
        bgfx.Raw.encoder_set_compute_dynamic_index_buffer(encoder, stage, &handle, access);
    }

    internal static void EncoderSetComputeDynamicVertexBuffer(
        bgfx.Encoder* encoder,
        byte stage,
        bgfx.DynamicVertexBufferHandle handle,
        bgfx.Access access
    ) {
        bgfx.Raw.encoder_set_compute_dynamic_vertex_buffer(encoder, stage, &handle, access);
    }

    internal static void EncoderSetComputeIndexBuffer(
        bgfx.Encoder* encoder,
        byte stage,
        bgfx.IndexBufferHandle handle,
        bgfx.Access access
    ) {
        bgfx.Raw.encoder_set_compute_index_buffer(encoder, stage, &handle, access);
    }

    internal static void EncoderSetComputeIndirectBuffer(
        bgfx.Encoder* encoder,
        byte stage,
        bgfx.IndirectBufferHandle handle,
        bgfx.Access access
    ) {
        bgfx.Raw.encoder_set_compute_indirect_buffer(encoder, stage, &handle, access);
    }

    internal static void EncoderSetComputeVertexBuffer(
        bgfx.Encoder* encoder,
        byte stage,
        bgfx.VertexBufferHandle handle,
        bgfx.Access access
    ) {
        bgfx.Raw.encoder_set_compute_vertex_buffer(encoder, stage, &handle, access);
    }

    internal static void EncoderSetDynamicIndexBuffer(
        bgfx.Encoder* encoder,
        bgfx.DynamicIndexBufferHandle handle,
        uint firstIndex,
        uint numIndices
    ) {
        bgfx.Raw.encoder_set_dynamic_index_buffer(encoder, &handle, firstIndex, numIndices);
    }

    internal static void EncoderSetDynamicVertexBuffer(
        bgfx.Encoder* encoder,
        byte stream,
        bgfx.DynamicVertexBufferHandle handle,
        uint startVertex,
        uint numVertices
    ) {
        bgfx.Raw.encoder_set_dynamic_vertex_buffer(encoder, stream, &handle, startVertex, numVertices);
    }

    internal static void EncoderSetDynamicVertexBufferWithLayout(
        bgfx.Encoder* encoder,
        byte stream,
        bgfx.DynamicVertexBufferHandle handle,
        uint startVertex,
        uint numVertices,
        bgfx.VertexLayoutHandle layoutHandle
    ) {
        bgfx.Raw.encoder_set_dynamic_vertex_buffer_with_layout(
            encoder,
            stream,
            &handle,
            startVertex,
            numVertices,
            &layoutHandle);
    }

    internal static void EncoderSetImage(
        bgfx.Encoder* encoder,
        byte stage,
        bgfx.TextureHandle handle,
        byte mip,
        bgfx.Access access,
        bgfx.TextureFormat format
    ) {
        bgfx.Raw.encoder_set_image(encoder, stage, &handle, mip, access, format);
    }

    internal static void EncoderSetIndexBuffer(
        bgfx.Encoder* encoder,
        bgfx.IndexBufferHandle handle,
        uint firstIndex,
        uint numIndices
    ) {
        bgfx.Raw.encoder_set_index_buffer(encoder, &handle, firstIndex, numIndices);
    }

    internal static void EncoderSetInstanceDataFromDynamicVertexBuffer(
        bgfx.Encoder* encoder,
        bgfx.DynamicVertexBufferHandle handle,
        uint startVertex,
        uint count
    ) {
        bgfx.Raw.encoder_set_instance_data_from_dynamic_vertex_buffer(encoder, &handle, startVertex, count);
    }

    internal static void EncoderSetInstanceDataFromVertexBuffer(
        bgfx.Encoder* encoder,
        bgfx.VertexBufferHandle handle,
        uint startVertex,
        uint count
    ) {
        bgfx.Raw.encoder_set_instance_data_from_vertex_buffer(encoder, &handle, startVertex, count);
    }

    internal static void EncoderSetTexture(
        bgfx.Encoder* encoder,
        byte stage,
        bgfx.UniformHandle sampler,
        bgfx.TextureHandle handle,
        uint flags
    ) {
        bgfx.Raw.encoder_set_texture(encoder, stage, &sampler, &handle, flags);
    }

    internal static void EncoderSetUniform(
        bgfx.Encoder* encoder,
        bgfx.UniformHandle handle,
        void* value,
        ushort count
    ) {
        bgfx.Raw.encoder_set_uniform(encoder, &handle, value, count);
    }

    internal static void EncoderSetVertexBuffer(
        bgfx.Encoder* encoder,
        byte stream,
        bgfx.VertexBufferHandle handle,
        uint startVertex,
        uint numVertices
    ) {
        bgfx.Raw.encoder_set_vertex_buffer(encoder, stream, &handle, startVertex, numVertices);
    }

    internal static void EncoderSetVertexBufferWithLayout(
        bgfx.Encoder* encoder,
        byte stream,
        bgfx.VertexBufferHandle handle,
        uint startVertex,
        uint numVertices,
        bgfx.VertexLayoutHandle layoutHandle
    ) {
        bgfx.Raw.encoder_set_vertex_buffer_with_layout(
            encoder,
            stream,
            &handle,
            startVertex,
            numVertices,
            &layoutHandle);
    }

    internal static void EncoderSubmit(
        bgfx.Encoder* encoder,
        ushort id,
        bgfx.ProgramHandle program,
        uint depth,
        byte flags
    ) {
        bgfx.Raw.encoder_submit(encoder, id, &program, depth, flags);
    }

    internal static void EncoderSubmitIndirect(
        bgfx.Encoder* encoder,
        ushort id,
        bgfx.ProgramHandle program,
        bgfx.IndirectBufferHandle indirectHandle,
        uint start,
        uint count,
        uint depth,
        byte flags
    ) {
        bgfx.Raw.encoder_submit_indirect(encoder, id, &program, &indirectHandle, start, count, depth, flags);
    }

    internal static ushort GetShaderUniforms(
        bgfx.ShaderHandle handle,
        bgfx.UniformHandle* uniforms,
        ushort max
    ) {
        return bgfx.Raw.get_shader_uniforms(&handle, uniforms, max);
    }

    internal static void GetUniformInfo(
        bgfx.UniformHandle handle,
        bgfx.UniformInfo* info
    ) {
        bgfx.Raw.get_uniform_info(&handle, info);
    }

    internal static uint ReadTexture(
        bgfx.TextureHandle handle,
        void* data,
        byte mip
    ) {
        return bgfx.Raw.read_texture(&handle, data, mip);
    }

    internal static void SetIndexBufferName(
        bgfx.IndexBufferHandle handle,
        sbyte* name,
        int len
    ) {
        bgfx.Raw.set_index_buffer_name(&handle, name, len);
    }

    internal static void SetShaderName(
        bgfx.ShaderHandle handle,
        sbyte* name,
        int len
    ) {
        bgfx.Raw.set_shader_name(&handle, name, len);
    }

    internal static void SetTextureName(
        bgfx.TextureHandle handle,
        sbyte* name,
        int len
    ) {
        bgfx.Raw.set_texture_name(&handle, name, len);
    }

    internal static void SetVertexBufferName(
        bgfx.VertexBufferHandle handle,
        sbyte* name,
        int len
    ) {
        bgfx.Raw.set_vertex_buffer_name(&handle, name, len);
    }

    internal static void UpdateDynamicIndexBuffer(
        bgfx.DynamicIndexBufferHandle handle,
        uint startIndex,
        bgfx.Memory* memory
    ) {
        bgfx.Raw.update_dynamic_index_buffer(&handle, startIndex, memory);
    }

    internal static void UpdateDynamicVertexBuffer(
        bgfx.DynamicVertexBufferHandle handle,
        uint startVertex,
        bgfx.Memory* memory
    ) {
        bgfx.Raw.update_dynamic_vertex_buffer(&handle, startVertex, memory);
    }

    internal static void UpdateTexture2d(
        bgfx.TextureHandle handle,
        ushort layer,
        byte mip,
        ushort x,
        ushort y,
        ushort width,
        ushort height,
        bgfx.Memory* memory,
        ushort pitch
    ) {
        bgfx.Raw.update_texture_2d(&handle, layer, mip, x, y, width, height, memory, pitch);
    }

    internal static void UpdateTexture3d(
        bgfx.TextureHandle handle,
        byte mip,
        ushort x,
        ushort y,
        ushort z,
        ushort width,
        ushort height,
        ushort depth,
        bgfx.Memory* memory
    ) {
        bgfx.Raw.update_texture_3d(&handle, mip, x, y, z, width, height, depth, memory);
    }

    internal static void UpdateTextureCube(
        bgfx.TextureHandle handle,
        ushort layer,
        byte side,
        byte mip,
        ushort x,
        ushort y,
        ushort width,
        ushort height,
        bgfx.Memory* memory,
        ushort pitch
    ) {
        bgfx.Raw.update_texture_cube(&handle, layer, side, mip, x, y, width, height, memory, pitch);
    }

}
