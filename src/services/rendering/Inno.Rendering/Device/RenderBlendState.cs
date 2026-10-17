using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Selects one source or destination blend multiplier.
/// </summary>
public enum RenderBlendFactor
{
    /// <summary>
    /// Multiplies by zero.
    /// </summary>
    Zero,
    /// <summary>
    /// Multiplies by one.
    /// </summary>
    One,
    /// <summary>
    /// Multiplies by source color.
    /// </summary>
    SourceColor,
    /// <summary>
    /// Multiplies by one minus source color.
    /// </summary>
    InverseSourceColor,
    /// <summary>
    /// Multiplies by source alpha.
    /// </summary>
    SourceAlpha,
    /// <summary>
    /// Multiplies by one minus source alpha.
    /// </summary>
    InverseSourceAlpha,
    /// <summary>
    /// Multiplies by destination alpha.
    /// </summary>
    DestinationAlpha,
    /// <summary>
    /// Multiplies by one minus destination alpha.
    /// </summary>
    InverseDestinationAlpha,
    /// <summary>
    /// Multiplies by destination color.
    /// </summary>
    DestinationColor,
    /// <summary>
    /// Multiplies by one minus destination color.
    /// </summary>
    InverseDestinationColor,
    /// <summary>
    /// Uses the saturated source-alpha factor.
    /// </summary>
    SourceAlphaSaturate,
    /// <summary>
    /// Multiplies by the packed constant blend color.
    /// </summary>
    Constant,
    /// <summary>
    /// Multiplies by one minus the packed constant blend color.
    /// </summary>
    InverseConstant
}

/// <summary>
/// Selects the arithmetic operation combining source and destination blend terms.
/// </summary>
public enum RenderBlendEquation
{
    /// <summary>
    /// Adds source and destination terms.
    /// </summary>
    Add,
    /// <summary>
    /// Subtracts the destination term from the source term.
    /// </summary>
    Subtract,
    /// <summary>
    /// Subtracts the source term from the destination term.
    /// </summary>
    ReverseSubtract,
    /// <summary>
    /// Selects the component-wise minimum.
    /// </summary>
    Minimum,
    /// <summary>
    /// Selects the component-wise maximum.
    /// </summary>
    Maximum
}

/// <summary>
/// Describes independent color and alpha blending without backend-native flags.
/// </summary>
public struct RenderBlendState
{
    /// <summary>
    /// Gets the disabled opaque blend state.
    /// </summary>
    public static RenderBlendState opaque => new()
    {
        enabled = false,
        colorSource = RenderBlendFactor.One,
        colorDestination = RenderBlendFactor.Zero,
        alphaSource = RenderBlendFactor.One,
        alphaDestination = RenderBlendFactor.Zero
    };

    /// <summary>
    /// Gets conventional straight-alpha blending.
    /// </summary>
    public static RenderBlendState alpha => new()
    {
        enabled = true,
        colorSource = RenderBlendFactor.SourceAlpha,
        colorDestination = RenderBlendFactor.InverseSourceAlpha,
        alphaSource = RenderBlendFactor.One,
        alphaDestination = RenderBlendFactor.InverseSourceAlpha
    };

    /// <summary>
    /// Gets additive source-alpha blending.
    /// </summary>
    public static RenderBlendState additive => new()
    {
        enabled = true,
        colorSource = RenderBlendFactor.SourceAlpha,
        colorDestination = RenderBlendFactor.One,
        alphaSource = RenderBlendFactor.One,
        alphaDestination = RenderBlendFactor.One
    };

    /// <summary>
    /// Gets conventional premultiplied-alpha blending.
    /// </summary>
    public static RenderBlendState premultiplied => new()
    {
        enabled = true,
        colorSource = RenderBlendFactor.One,
        colorDestination = RenderBlendFactor.InverseSourceAlpha,
        alphaSource = RenderBlendFactor.One,
        alphaDestination = RenderBlendFactor.InverseSourceAlpha
    };

    /// <summary>
    /// Gets or sets whether blending is enabled.
    /// </summary>
    public bool enabled { get; set; }

    /// <summary>
    /// Gets or sets the source multiplier for RGB channels.
    /// </summary>
    public RenderBlendFactor colorSource { get; set; }

    /// <summary>
    /// Gets or sets the destination multiplier for RGB channels.
    /// </summary>
    public RenderBlendFactor colorDestination { get; set; }

    /// <summary>
    /// Gets or sets the RGB combination equation.
    /// </summary>
    public RenderBlendEquation colorEquation { get; set; }

    /// <summary>
    /// Gets or sets the source multiplier for alpha.
    /// </summary>
    public RenderBlendFactor alphaSource { get; set; }

    /// <summary>
    /// Gets or sets the destination multiplier for alpha.
    /// </summary>
    public RenderBlendFactor alphaDestination { get; set; }

    /// <summary>
    /// Gets or sets the alpha combination equation.
    /// </summary>
    public RenderBlendEquation alphaEquation { get; set; }

    /// <summary>
    /// Gets or sets the packed RGBA8 constant used by constant blend factors.
    /// </summary>
    public uint constantRgba { get; set; }

    /// <summary>
    /// Gets or sets whether alpha-to-coverage is enabled.
    /// </summary>
    public bool alphaToCoverage { get; set; }
}

