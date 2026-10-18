using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Selects texture filtering independently from a graphics backend.
/// </summary>
public enum RenderSamplerFilter
{
    /// <summary>
    /// Uses nearest-neighbor filtering.
    /// </summary>
    Point,
    /// <summary>
    /// Uses linear filtering.
    /// </summary>
    Linear,
    /// <summary>
    /// Uses anisotropic filtering when supported.
    /// </summary>
    Anisotropic
}

/// <summary>
/// Selects texture addressing independently for each coordinate axis.
/// </summary>
public enum RenderSamplerAddressMode
{
    /// <summary>
    /// Repeats texture coordinates.
    /// </summary>
    Repeat,
    /// <summary>
    /// Mirrors repeated texture coordinates.
    /// </summary>
    Mirror,
    /// <summary>
    /// Clamps coordinates to the texture edge.
    /// </summary>
    Clamp,
    /// <summary>
    /// Samples the backend border color outside the texture.
    /// </summary>
    Border
}

/// <summary>
/// Describes one native-serializable backend-neutral sampler binding.
/// </summary>
public struct RenderSamplerState : IEquatable<RenderSamplerState>
{
    /// <summary>
    /// Gets linear filtering with clamped addressing.
    /// </summary>
    public static RenderSamplerState linearClamp => new(
        RenderSamplerFilter.Linear,
        RenderSamplerAddressMode.Clamp,
        RenderSamplerAddressMode.Clamp,
        RenderSamplerAddressMode.Clamp);

    /// <summary>
    /// Creates a sampler state.
    /// </summary>
    /// <param name="filter">
    /// Minification, magnification, and mip filtering contract.
    /// </param>
    /// <param name="addressU">
    /// Horizontal address mode.
    /// </param>
    /// <param name="addressV">
    /// Vertical address mode.
    /// </param>
    /// <param name="addressW">
    /// Depth or cube address mode.
    /// </param>
    public RenderSamplerState(
        RenderSamplerFilter filter,
        RenderSamplerAddressMode addressU,
        RenderSamplerAddressMode addressV,
        RenderSamplerAddressMode addressW
    ) {
        this.filter = filter;
        this.addressU = addressU;
        this.addressV = addressV;
        this.addressW = addressW;
    }

    /// <summary>
    /// Gets the filter contract.
    /// </summary>
    public RenderSamplerFilter filter { get; set; }

    /// <summary>
    /// Gets horizontal addressing.
    /// </summary>
    public RenderSamplerAddressMode addressU { get; set; }

    /// <summary>
    /// Gets vertical addressing.
    /// </summary>
    public RenderSamplerAddressMode addressV { get; set; }

    /// <summary>
    /// Gets depth or cube addressing.
    /// </summary>
    public RenderSamplerAddressMode addressW { get; set; }

    /// <summary>
    /// Determines whether this instance and the supplied value represent the same logical state.
    /// </summary>
    /// <param name="other">
    /// The value to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when both values represent the same logical state; otherwise, <see langword="false"/>.
    /// </returns>
    public readonly bool Equals(RenderSamplerState other)
        => filter == other.filter
           && addressU == other.addressU
           && addressV == other.addressV
           && addressW == other.addressW;

    /// <summary>
    /// Determines whether this instance and the supplied value represent the same logical state.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when both values represent the same logical state; otherwise, <see langword="false"/>.
    /// </returns>
    /// <param name="obj">
    /// The object to compare with this instance.
    /// </param>
    public override readonly bool Equals(object? obj) => obj is RenderSamplerState other && Equals(other);

    /// <summary>
    /// Computes a hash code from the fields that participate in logical equality.
    /// </summary>
    /// <returns>
    /// A hash code consistent with the implemented equality contract.
    /// </returns>
    public override readonly int GetHashCode() => HashCode.Combine(filter, addressU, addressV, addressW);

    /// <summary>
    /// Determines whether two sampler descriptions are equal.
    /// </summary>
    /// <param name="left">
    /// Left sampler description.
    /// </param>
    /// <param name="right">
    /// Right sampler description.
    /// </param>
    /// <returns>
    /// True when every filtering and addressing field is equal.
    /// </returns>
    public static bool operator ==(
        RenderSamplerState left,
        RenderSamplerState right
    ) => left.Equals(right);

    /// <summary>
    /// Determines whether two sampler descriptions differ.
    /// </summary>
    /// <param name="left">
    /// Left sampler description.
    /// </param>
    /// <param name="right">
    /// Right sampler description.
    /// </param>
    /// <returns>
    /// True when at least one filtering or addressing field differs.
    /// </returns>
    public static bool operator !=(
        RenderSamplerState left,
        RenderSamplerState right
    ) => !left.Equals(right);
}

