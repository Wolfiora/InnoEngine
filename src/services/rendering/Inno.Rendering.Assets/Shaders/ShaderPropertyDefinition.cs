using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Declares one shader property and its reflected stage visibility.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("4b05e49c-b844-535e-aec9-c2201cfdd93f")]
public struct ShaderPropertyDefinition
{
    /// <summary>
    /// Creates a shader property definition.
    /// </summary>
    /// <param name="id">
    /// Stable property identifier.
    /// </param>
    /// <param name="displayName">
    /// Artist-facing property name.
    /// </param>
    /// <param name="type">
    /// Property type.
    /// </param>
    /// <param name="stages">
    /// Stages that access the property.
    /// </param>
    /// <param name="defaultValue">
    /// Native serializable default value.
    /// </param>
    /// <param name="bindingKind">
    /// Optional explicit binding domain. When omitted, numeric values become uniforms, textures become sampled
    /// textures, and buffers become storage buffers.
    /// </param>
    /// <param name="storageAccess">
    /// Required access for storage texture or buffer bindings.
    /// </param>
    /// <param name="bindingOwner">
    /// Layer responsible for supplying the binding value at execution time.
    /// </param>
    public ShaderPropertyDefinition(
        ShaderPropertyId id,
        string displayName,
        ShaderPropertyType type,
        ShaderStage stages,
        MaterialValue defaultValue,
        ShaderPropertyBindingKind? bindingKind = null,
        RenderStorageAccess storageAccess = RenderStorageAccess.Read,
        ShaderPropertyBindingOwner bindingOwner = ShaderPropertyBindingOwner.Material
    ) {
        if (!id.isValid)
            throw new ArgumentException("A shader property ID must be valid.", nameof(id));
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        this.id = id;
        this.displayName = displayName;
        this.type = type;
        this.stages = stages;
        this.defaultValue = defaultValue;
        this.bindingKind = bindingKind ?? InferBindingKind(type);
        this.storageAccess = storageAccess;
        this.bindingOwner = bindingOwner;
        if (!Enum.IsDefined(storageAccess))
            throw new ArgumentOutOfRangeException(nameof(storageAccess));
        if (!Enum.IsDefined(bindingOwner))
            throw new ArgumentOutOfRangeException(nameof(bindingOwner));
        ValidateBindingKind(type, this.bindingKind);
    }

    /// <summary>
    /// Gets or sets the stable property identifier.
    /// </summary>
    public ShaderPropertyId id { get; set; }

    /// <summary>
    /// Gets or sets the artist-facing property name.
    /// </summary>
    public string displayName { get; set; }

    /// <summary>
    /// Gets or sets the property type.
    /// </summary>
    public ShaderPropertyType type { get; set; }

    /// <summary>
    /// Gets or sets stages that access the property.
    /// </summary>
    public ShaderStage stages { get; set; }

    /// <summary>
    /// Gets or sets the native serializable default value.
    /// </summary>
    public MaterialValue defaultValue { get; set; }

    /// <summary>
    /// Gets or sets how the property enters the shader resource interface.
    /// </summary>
    public ShaderPropertyBindingKind bindingKind { get; set; }

    /// <summary>
    /// Gets or sets required access for storage texture or buffer bindings.
    /// </summary>
    public RenderStorageAccess storageAccess { get; set; }

    /// <summary>
    /// Gets or sets the layer responsible for supplying this binding at execution time.
    /// </summary>
    public ShaderPropertyBindingOwner bindingOwner { get; set; }

    internal static bool IsBindingKindCompatible(
        ShaderPropertyType type,
        ShaderPropertyBindingKind bindingKind
    )
        => bindingKind switch
        {
            ShaderPropertyBindingKind.Uniform => !IsTexture(type) && type != ShaderPropertyType.Buffer,
            ShaderPropertyBindingKind.SampledTexture => IsTexture(type),
            ShaderPropertyBindingKind.StorageTexture => IsTexture(type),
            ShaderPropertyBindingKind.StorageBuffer => type == ShaderPropertyType.Buffer,
            _ => false
        };

    private static ShaderPropertyBindingKind InferBindingKind(ShaderPropertyType type)
        => IsTexture(type)
            ? ShaderPropertyBindingKind.SampledTexture
            : type == ShaderPropertyType.Buffer
                ? ShaderPropertyBindingKind.StorageBuffer
                : ShaderPropertyBindingKind.Uniform;

    private static bool IsTexture(ShaderPropertyType type)
        => type is ShaderPropertyType.Texture2D
            or ShaderPropertyType.Texture2DArray
            or ShaderPropertyType.Texture3D
            or ShaderPropertyType.TextureCube;

    private static void ValidateBindingKind(
        ShaderPropertyType type,
        ShaderPropertyBindingKind bindingKind
    ) {
        if (!IsBindingKindCompatible(type, bindingKind))
        {
            throw new ArgumentException(
                $"Shader property type '{type}' is incompatible with binding kind '{bindingKind}'.",
                nameof(bindingKind));
        }
    }
}

