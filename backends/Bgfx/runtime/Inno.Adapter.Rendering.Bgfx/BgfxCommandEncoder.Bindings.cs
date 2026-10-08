using System;
using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed unsafe partial class BgfxCommandEncoder

{

    /// <summary>
    /// Binds the graphics pipeline used by subsequent draw commands.
    /// </summary>
    /// <param name="pipeline">
    /// The pipeline consumed by bind graphics pipeline; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindGraphicsPipeline(GraphicsPipelineHandle pipeline)
    {
        BgfxPipelineResource resource = m_device.ResolvePipeline(pipeline);
        if (resource.compute)
        {
            throw new ArgumentException("A compute program cannot be bound as a graphics pipeline.", nameof(pipeline));
        }

        m_pipeline = resource;
        m_rasterState = null;
        m_stencilState = RenderStencilState.disabled;
        m_instanceDataBound = false;
    }

    /// <summary>
    /// Binds the compute pipeline used by subsequent dispatch commands.
    /// </summary>
    /// <param name="pipeline">
    /// The pipeline consumed by bind compute pipeline; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindComputePipeline(ComputePipelineHandle pipeline)
    {
        BgfxPipelineResource resource = m_device.ResolvePipeline(pipeline);
        if (!resource.compute)
        {
            throw new ArgumentException("A graphics program cannot be bound as a compute pipeline.", nameof(pipeline));
        }

        m_pipeline = resource;
    }

    /// <summary>
    /// Binds a texture resource to the requested shader binding.
    /// </summary>
    /// <param name="binding">
    /// The binding consumed by bind texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="texture">
    /// The texture consumed by bind texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="sampler">
    /// The sampler consumed by bind texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindTexture(
        RenderBindingId binding,
        RenderTextureHandle texture,
        RenderSamplerState sampler
    )
        => BindTexture(binding, m_device.ResolveTexture(texture), sampler);

    /// <summary>
    /// Binds a texture resource to the requested shader binding.
    /// </summary>
    /// <param name="binding">
    /// The binding consumed by bind texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="texture">
    /// The texture consumed by bind texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="sampler">
    /// The sampler consumed by bind texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindTexture(
        RenderBindingId binding,
        PersistentTextureHandle texture,
        RenderSamplerState sampler
    )
        => BindTexture(binding, m_device.ResolveTexture(texture), sampler);

    /// <summary>
    /// Binds a writable texture resource to the requested shader binding.
    /// </summary>
    /// <param name="binding">
    /// The binding consumed by bind storage texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="texture">
    /// The texture consumed by bind storage texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="mipLevel">
    /// The mip level consumed by bind storage texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindStorageTexture(
        RenderBindingId binding,
        RenderTextureHandle texture,
        int mipLevel = 0
    )
        => BindStorageTexture(
            binding,
            m_device.ResolveTexture(texture),
            m_device.ResolveTextureDescriptor(texture),
            mipLevel);

    /// <summary>
    /// Binds a writable texture resource to the requested shader binding.
    /// </summary>
    /// <param name="binding">
    /// The binding consumed by bind storage texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="texture">
    /// The texture consumed by bind storage texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="mipLevel">
    /// The mip level consumed by bind storage texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindStorageTexture(
        RenderBindingId binding,
        PersistentTextureHandle texture,
        int mipLevel = 0
    )
        => BindStorageTexture(
            binding,
            m_device.ResolveTexture(texture),
            m_device.ResolveTextureDescriptor(texture),
            mipLevel);

    /// <summary>
    /// Binds a buffer resource to the requested shader binding.
    /// </summary>
    /// <param name="binding">
    /// The binding consumed by bind buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="buffer">
    /// The buffer consumed by bind buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindBuffer(
        RenderBindingId binding,
        RenderBufferHandle buffer
    )
        => BindBuffer(binding, m_device.ResolveBuffer(buffer));

    /// <summary>
    /// Binds a buffer resource to the requested shader binding.
    /// </summary>
    /// <param name="binding">
    /// The binding consumed by bind buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="buffer">
    /// The buffer consumed by bind buffer; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void BindBuffer(
        RenderBindingId binding,
        PersistentBufferHandle buffer
    )
        => BindBuffer(binding, m_device.ResolveBuffer(buffer));

    /// <summary>
    /// Updates the uniform state and applies the resulting invariants.
    /// </summary>
    /// <param name="binding">
    /// The binding consumed by set uniform; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="value">
    /// The concrete value read or transformed by this operation.
    /// </param>
    public override void SetUniform(
        RenderBindingId binding,
        ReadOnlySpan<byte> value
    ) {
        BgfxShaderBindingResource resource = ResolveBinding(binding, RenderShaderBindingKind.Uniform);
        int elementSize = resource.descriptor.uniformType switch
        {
            RenderUniformType.Vector4 => 4 * sizeof(float),
            RenderUniformType.Matrix3x3 => 9 * sizeof(float),
            RenderUniformType.Matrix4x4 => 16 * sizeof(float),
            _ => throw new ArgumentOutOfRangeException(nameof(binding))
        };
        int expectedSize = checked(elementSize * resource.descriptor.count);
        if (value.Length != expectedSize)
        {
            throw new ArgumentException(
                $"Uniform '{binding.value}' requires exactly {expectedSize} bytes.",
                nameof(value));
        }

        fixed (byte* data = value)
        {
            bgfx.encoder_set_uniform(
                m_encoder,
                resource.uniform,
                data,
                checked((ushort)resource.descriptor.count));
        }
    }

    private void BindTexture(
        RenderBindingId binding,
        bgfx.TextureHandle texture,
        RenderSamplerState sampler
    ) {
        BgfxShaderBindingResource resource = ResolveBinding(binding, RenderShaderBindingKind.Texture);
        bgfx.encoder_set_texture(
            m_encoder,
            checked((byte)resource.descriptor.slot),
            resource.uniform,
            texture,
            SamplerFlags(sampler));
    }

    private void BindStorageTexture(
        RenderBindingId binding,
        bgfx.TextureHandle texture,
        RenderTextureDescriptor descriptor,
        int mipLevel
    ) {
        BgfxShaderBindingResource resource = ResolveBinding(binding, RenderShaderBindingKind.StorageTexture);
        if (!m_device.capabilities.Supports(GraphicsCapability.StorageTexture))
        {
            throw new NotSupportedException(
                "The active graphics backend does not support shader storage textures.");
        }
        if ((descriptor.usage & RenderTextureUsage.Storage) == 0)
        {
            throw new ArgumentException(
                $"Texture bound to '{binding.value}' was not created for storage access.",
                nameof(descriptor));
        }
        ArgumentOutOfRangeException.ThrowIfNegative(mipLevel);
        if (mipLevel >= descriptor.mipCount)
            throw new ArgumentOutOfRangeException(nameof(mipLevel));
        if (!m_device.capabilities.SupportsStorage(descriptor.format, resource.descriptor.storageAccess))
        {
            throw new NotSupportedException(
                $"Texture format '{descriptor.format}' does not support {resource.descriptor.storageAccess} storage access.");
        }

        bgfx.encoder_set_image(
            m_encoder,
            checked((byte)resource.descriptor.slot),
            texture,
            checked((byte)mipLevel),
            StorageAccess(resource.descriptor.storageAccess),
            BgfxCapabilityMapper.ToNativeFormat(descriptor.format));
    }

    private void BindBuffer(
        RenderBindingId binding,
        BgfxBufferResource buffer
    ) {
        BgfxShaderBindingResource resource = ResolveBinding(binding, RenderShaderBindingKind.StorageBuffer);
        if ((buffer.descriptor.usage & RenderBufferUsage.Storage) == 0)
        {
            throw new ArgumentException(
                $"Buffer bound to '{binding.value}' was not created for storage access.",
                nameof(buffer));
        }

        byte slot = checked((byte)resource.descriptor.slot);
        bgfx.Access access = StorageAccess(resource.descriptor.storageAccess);
        switch (buffer.kind)
        {
            case BgfxBufferKind.Vertex:
                bgfx.encoder_set_compute_vertex_buffer(
                    m_encoder,
                    slot,
                    new bgfx.VertexBufferHandle { idx = buffer.nativeIndex },
                    access);
                break;
            case BgfxBufferKind.Index:
                bgfx.encoder_set_compute_index_buffer(
                    m_encoder,
                    slot,
                    new bgfx.IndexBufferHandle { idx = buffer.nativeIndex },
                    access);
                break;
            case BgfxBufferKind.DynamicVertex:
                bgfx.encoder_set_compute_dynamic_vertex_buffer(
                    m_encoder,
                    slot,
                    new bgfx.DynamicVertexBufferHandle { idx = buffer.nativeIndex },
                    access);
                break;
            case BgfxBufferKind.DynamicIndex:
                bgfx.encoder_set_compute_dynamic_index_buffer(
                    m_encoder,
                    slot,
                    new bgfx.DynamicIndexBufferHandle { idx = buffer.nativeIndex },
                    access);
                break;
            case BgfxBufferKind.Indirect:
                bgfx.encoder_set_compute_indirect_buffer(
                    m_encoder,
                    slot,
                    new bgfx.IndirectBufferHandle { idx = buffer.nativeIndex },
                    access);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buffer));
        }
    }

    private BgfxShaderBindingResource ResolveBinding(
        RenderBindingId binding,
        RenderShaderBindingKind requiredKind
    ) {
        if (!binding.isValid)
        {
            throw new ArgumentException("A stable shader binding name is required.", nameof(binding));
        }

        BgfxPipelineResource pipeline = m_pipeline
            ?? throw new InvalidOperationException("A pipeline must be bound before shader resources.");
        if (!pipeline.bindings.TryGetValue(binding.value, out BgfxShaderBindingResource? resource))
        {
            throw new ArgumentException(
                $"Pipeline does not declare shader binding '{binding.value}'.",
                nameof(binding));
        }

        if (resource.descriptor.kind != requiredKind)
        {
            throw new ArgumentException(
                $"Shader binding '{binding.value}' is {resource.descriptor.kind}, not {requiredKind}.",
                nameof(binding));
        }

        return resource;
    }

}
