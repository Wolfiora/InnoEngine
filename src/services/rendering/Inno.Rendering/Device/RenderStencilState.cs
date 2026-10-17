using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Selects the comparison applied to stencil reference and stored values.
/// </summary>
public enum RenderStencilCompare
{
    /// <summary>
    /// Never passes.
    /// </summary>
    Never,
    /// <summary>
    /// Passes when reference is smaller.
    /// </summary>
    Less,
    /// <summary>
    /// Passes when values are equal.
    /// </summary>
    Equal,
    /// <summary>
    /// Passes when reference is smaller or equal.
    /// </summary>
    LessEqual,
    /// <summary>
    /// Passes when reference is greater.
    /// </summary>
    Greater,
    /// <summary>
    /// Passes when values differ.
    /// </summary>
    NotEqual,
    /// <summary>
    /// Passes when reference is greater or equal.
    /// </summary>
    GreaterEqual,
    /// <summary>
    /// Always passes.
    /// </summary>
    Always
}

/// <summary>
/// Selects the update applied to a stencil value.
/// </summary>
public enum RenderStencilOperation
{
    /// <summary>
    /// Keeps the stored value.
    /// </summary>
    Keep,
    /// <summary>
    /// Clears the stored value to zero.
    /// </summary>
    Zero,
    /// <summary>
    /// Replaces the stored value with the reference.
    /// </summary>
    Replace,
    /// <summary>
    /// Increments and clamps the stored value.
    /// </summary>
    IncrementClamp,
    /// <summary>
    /// Increments and wraps the stored value.
    /// </summary>
    IncrementWrap,
    /// <summary>
    /// Decrements and clamps the stored value.
    /// </summary>
    DecrementClamp,
    /// <summary>
    /// Decrements and wraps the stored value.
    /// </summary>
    DecrementWrap,
    /// <summary>
    /// Bitwise-inverts the stored value.
    /// </summary>
    Invert
}

/// <summary>
/// Describes stencil behavior for one triangle face orientation.
/// </summary>
public readonly record struct RenderStencilFaceState
{
    /// <summary>
    /// Creates one face stencil state.
    /// </summary>
    /// <param name="compare">
    /// Stencil comparison.
    /// </param>
    /// <param name="fail">
    /// Operation after stencil comparison failure.
    /// </param>
    /// <param name="depthFail">
    /// Operation after stencil success and depth failure.
    /// </param>
    /// <param name="pass">
    /// Operation after stencil and depth success.
    /// </param>
    public RenderStencilFaceState(
        RenderStencilCompare compare,
        RenderStencilOperation fail,
        RenderStencilOperation depthFail,
        RenderStencilOperation pass
    ) {
        this.compare = compare;
        this.fail = fail;
        this.depthFail = depthFail;
        this.pass = pass;
    }

    /// <summary>
    /// Gets stencil comparison.
    /// </summary>
    public RenderStencilCompare compare { get; }

    /// <summary>
    /// Gets the stencil-failure operation.
    /// </summary>
    public RenderStencilOperation fail { get; }

    /// <summary>
    /// Gets the depth-failure operation.
    /// </summary>
    public RenderStencilOperation depthFail { get; }

    /// <summary>
    /// Gets the complete-pass operation.
    /// </summary>
    public RenderStencilOperation pass { get; }
}

/// <summary>
/// Describes complete two-sided stencil state for one draw.
/// </summary>
public sealed class RenderStencilState
{
    /// <summary>
    /// Creates the default disabled stencil state.
    /// </summary>
    public RenderStencilState()
    {
    }

    /// <summary>
    /// Creates a complete immutable stencil configuration without relying on init-only setters.
    /// </summary>
    /// <param name="enabled">
    /// Whether stencil testing and updates are active.
    /// </param>
    /// <param name="reference">
    /// Eight-bit stencil reference value.
    /// </param>
    /// <param name="readMask">
    /// Mask applied while reading stored stencil.
    /// </param>
    /// <param name="writeMask">
    /// Mask applied while writing stencil.
    /// </param>
    /// <param name="front">
    /// Front-face stencil behavior.
    /// </param>
    /// <param name="back">
    /// Back-face stencil behavior.
    /// </param>
    public RenderStencilState(
        bool enabled,
        byte reference,
        byte readMask,
        byte writeMask,
        RenderStencilFaceState front,
        RenderStencilFaceState back
    ) {
        this.enabled = enabled;
        this.reference = reference;
        this.readMask = readMask;
        this.writeMask = writeMask;
        this.front = front;
        this.back = back;
    }

    /// <summary>
    /// Gets disabled stencil state.
    /// </summary>
    public static RenderStencilState disabled { get; } = new() { enabled = false };

    /// <summary>
    /// Gets whether stencil testing and updates are active.
    /// </summary>
    public bool enabled { get; init; }

    /// <summary>
    /// Gets the eight-bit stencil reference value.
    /// </summary>
    public byte reference { get; init; }

    /// <summary>
    /// Gets the mask applied while reading stored stencil.
    /// </summary>
    public byte readMask { get; init; } = byte.MaxValue;

    /// <summary>
    /// Gets the mask applied while writing stencil.
    /// </summary>
    public byte writeMask { get; init; } = byte.MaxValue;

    /// <summary>
    /// Gets front-face stencil behavior.
    /// </summary>
    public RenderStencilFaceState front { get; init; } = new(
        RenderStencilCompare.Always,
        RenderStencilOperation.Keep,
        RenderStencilOperation.Keep,
        RenderStencilOperation.Keep);

    /// <summary>
    /// Gets back-face stencil behavior.
    /// </summary>
    public RenderStencilFaceState back { get; init; } = new(
        RenderStencilCompare.Always,
        RenderStencilOperation.Keep,
        RenderStencilOperation.Keep,
        RenderStencilOperation.Keep);
}

