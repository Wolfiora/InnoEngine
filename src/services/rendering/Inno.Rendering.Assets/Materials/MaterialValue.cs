using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Identifies the neutral value stored by a material property.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("a9af9e77-9a97-5f66-82a0-d720ac5604dc")]
public enum MaterialValueKind
{
    /// <summary>
    /// Scalar floating-point value.
    /// </summary>
    Float,
    /// <summary>
    /// Four-component vector value.
    /// </summary>
    Vector,
    /// <summary>
    /// Linear color value.
    /// </summary>
    Color,
    /// <summary>
    /// Four-by-four matrix value.
    /// </summary>
    Matrix,
    /// <summary>
    /// Texture asset reference.
    /// </summary>
    Texture
}

/// <summary>
/// Stores one native-serializable material value without a GPU binding.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("dc64bc3e-f26f-55e0-bffe-99e98bd95b4f")]
public struct MaterialValue
{
    private MaterialValue(
        MaterialValueKind kind,
        Vector4 vector,
        Matrix matrix,
        TextureAsset? texture,
        RenderSamplerState sampler
    ) {
        this.kind = kind;
        this.vector = vector;
        this.matrix = matrix;
        this.texture = texture;
        this.sampler = sampler;
    }

    /// <summary>
    /// Gets or sets the stored value kind.
    /// </summary>
    public MaterialValueKind kind { get; set; }

    /// <summary>
    /// Gets or sets scalar, vector, or color components.
    /// </summary>
    public Vector4 vector { get; set; }

    /// <summary>
    /// Gets or sets the matrix value.
    /// </summary>
    public Matrix matrix { get; set; }

    /// <summary>
    /// Gets or sets the texture reference.
    /// </summary>
    public TextureAsset? texture { get; set; }

    /// <summary>
    /// Gets or sets the sampler used when this value stores a texture.
    /// </summary>
    public RenderSamplerState sampler { get; set; }

    /// <summary>
    /// Creates a scalar material value.
    /// </summary>
    /// <param name="value">
    /// Scalar value.
    /// </param>
    /// <returns>
    /// A scalar material value.
    /// </returns>
    public static MaterialValue FromFloat(float value)
        => new(MaterialValueKind.Float, new Vector4(value, 0f, 0f, 0f), default, null, default);

    /// <summary>
    /// Creates a vector material value.
    /// </summary>
    /// <param name="value">
    /// Vector value.
    /// </param>
    /// <returns>
    /// A vector material value.
    /// </returns>
    public static MaterialValue FromVector(Vector4 value) => new(MaterialValueKind.Vector, value, default, null, default);

    /// <summary>
    /// Creates a linear color material value.
    /// </summary>
    /// <param name="value">
    /// Color value.
    /// </param>
    /// <returns>
    /// A color material value.
    /// </returns>
    public static MaterialValue FromColor(Color value)
        => new(MaterialValueKind.Color, new Vector4(value.r, value.g, value.b, value.a), default, null, default);

    /// <summary>
    /// Creates a matrix material value.
    /// </summary>
    /// <param name="value">
    /// Four-by-four matrix value.
    /// </param>
    /// <returns>
    /// A matrix material value.
    /// </returns>
    public static MaterialValue FromMatrix(Matrix value) => new(MaterialValueKind.Matrix, default, value, null, default);

    /// <summary>
    /// Creates a texture material value.
    /// </summary>
    /// <param name="value">
    /// Texture asset reference.
    /// </param>
    /// <param name="sampler">
    /// Optional sampler state; linear clamp is used when omitted.
    /// </param>
    /// <returns>
    /// A texture material value.
    /// </returns>
    public static MaterialValue FromTexture(
        TextureAsset value,
        RenderSamplerState? sampler = null
    ) {
        ArgumentNullException.ThrowIfNull(value);
        return new MaterialValue(
            MaterialValueKind.Texture,
            default,
            default,
            value,
            sampler ?? RenderSamplerState.linearClamp);
    }
}

