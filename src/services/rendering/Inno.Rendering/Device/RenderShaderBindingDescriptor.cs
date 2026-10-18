using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Identifies a shader interface binding domain.
/// </summary>
public enum RenderShaderBindingKind
{
    /// <summary>
    /// Vector or matrix uniform data.
    /// </summary>
    Uniform,
    /// <summary>
    /// Sampled texture and sampler state.
    /// </summary>
    Texture,
    /// <summary>
    /// Shader-readable or writable storage texture.
    /// </summary>
    StorageTexture,
    /// <summary>
    /// Compute-readable or writable buffer.
    /// </summary>
    StorageBuffer
}

/// <summary>
/// Identifies the native-independent shape of one uniform binding.
/// </summary>
public enum RenderUniformType
{
    /// <summary>
    /// Four-component 32-bit floating-point vector.
    /// </summary>
    Vector4,
    /// <summary>
    /// Three-by-three 32-bit floating-point matrix.
    /// </summary>
    Matrix3x3,
    /// <summary>
    /// Four-by-four 32-bit floating-point matrix.
    /// </summary>
    Matrix4x4
}

/// <summary>
/// Selects unordered storage-resource access for one shader binding.
/// </summary>
public enum RenderStorageAccess
{
    /// <summary>
    /// Shader read-only access.
    /// </summary>
    Read,
    /// <summary>
    /// Shader write-only access.
    /// </summary>
    Write,
    /// <summary>
    /// Shader read and write access.
    /// </summary>
    ReadWrite
}

/// <summary>
/// Declares one manifest-derived shader binding used for reflection validation.
/// </summary>
public sealed class RenderShaderBindingDescriptor
{
    /// <summary>
    /// Creates a shader binding descriptor.
    /// </summary>
    /// <param name="id">
    /// Stable manifest binding name.
    /// </param>
    /// <param name="kind">
    /// Binding domain.
    /// </param>
    /// <param name="slot">
    /// Backend-neutral texture or storage slot.
    /// </param>
    /// <param name="uniformType">
    /// Uniform shape when <paramref name="kind"/> is Uniform.
    /// </param>
    /// <param name="count">
    /// Uniform array element count.
    /// </param>
    /// <param name="storageAccess">
    /// Storage texture or buffer access.
    /// </param>
    /// <param name="nativeName">
    /// Adapter-generated reflected symbol, or null when the logical ID is also the symbol.
    /// </param>
    public RenderShaderBindingDescriptor(
        RenderBindingId id,
        RenderShaderBindingKind kind,
        int slot = 0,
        RenderUniformType uniformType = RenderUniformType.Vector4,
        int count = 1,
        RenderStorageAccess storageAccess = RenderStorageAccess.Read,
        string? nativeName = null
    ) {
        if (!id.isValid)
        {
            throw new ArgumentException("A shader binding requires a stable manifest name.", nameof(id));
        }

        ArgumentOutOfRangeException.ThrowIfNegative(slot);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);
        if (!Enum.IsDefined(kind))
            throw new ArgumentOutOfRangeException(nameof(kind));
        if (!Enum.IsDefined(uniformType))
            throw new ArgumentOutOfRangeException(nameof(uniformType));
        if (!Enum.IsDefined(storageAccess))
            throw new ArgumentOutOfRangeException(nameof(storageAccess));
        this.id = id;
        this.kind = kind;
        this.slot = slot;
        this.uniformType = uniformType;
        this.count = count;
        this.storageAccess = storageAccess;
        this.nativeName = nativeName ?? id.value;
        ArgumentException.ThrowIfNullOrWhiteSpace(this.nativeName);
    }

    /// <summary>
    /// Gets the stable manifest binding name.
    /// </summary>
    public RenderBindingId id { get; }

    /// <summary>
    /// Gets the binding domain.
    /// </summary>
    public RenderShaderBindingKind kind { get; }

    /// <summary>
    /// Gets the backend-neutral texture or storage slot.
    /// </summary>
    public int slot { get; }

    /// <summary>
    /// Gets the uniform shape.
    /// </summary>
    public RenderUniformType uniformType { get; }

    /// <summary>
    /// Gets uniform array element count.
    /// </summary>
    public int count { get; }

    /// <summary>
    /// Gets storage texture or buffer access.
    /// </summary>
    public RenderStorageAccess storageAccess { get; }

    /// <summary>
    /// Gets the exact adapter-generated symbol used only for reflection and native resource creation.
    /// </summary>
    public string nativeName { get; }
}

