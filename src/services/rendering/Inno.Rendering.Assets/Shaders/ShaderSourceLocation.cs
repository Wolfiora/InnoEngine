using Inno.Core.Diagnostics;
using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Maps a shader diagnostic back to an asset, pass, stage, and source position.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("ab6474ed-51d0-570c-b15d-c477b5e9fecc")]
public readonly record struct ShaderSourceLocation
{
    /// <summary>
    /// Creates a shader source location.
    /// </summary>
    /// <param name="assetPath">
    /// Project-relative source asset path.
    /// </param>
    /// <param name="passName">
    /// Stable pass name.
    /// </param>
    /// <param name="stage">
    /// Shader stage.
    /// </param>
    /// <param name="line">
    /// One-based source line, or zero when unavailable.
    /// </param>
    /// <param name="column">
    /// One-based source column, or zero when unavailable.
    /// </param>
    public ShaderSourceLocation(
        string assetPath,
        string passName,
        ShaderStage stage,
        int line = 0,
        int column = 0
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(assetPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(passName);
        ArgumentOutOfRangeException.ThrowIfNegative(line);
        ArgumentOutOfRangeException.ThrowIfNegative(column);
        this.assetPath = assetPath;
        this.passName = passName;
        this.stage = stage;
        this.line = line;
        this.column = column;
    }

    /// <summary>
    /// Gets the project-relative source asset path.
    /// </summary>
    public string assetPath { get; }

    /// <summary>
    /// Gets the stable pass name.
    /// </summary>
    public string passName { get; }

    /// <summary>
    /// Gets the shader stage.
    /// </summary>
    public ShaderStage stage { get; }

    /// <summary>
    /// Gets the one-based line, or zero when unavailable.
    /// </summary>
    public int line { get; }

    /// <summary>
    /// Gets the one-based column, or zero when unavailable.
    /// </summary>
    public int column { get; }

}

