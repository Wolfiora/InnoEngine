using System;
using Inno.Build.Toolchains.Bgfx.Shaders;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;
using Microsoft.Build.Framework;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Inno.Build.Tasks;

/// <summary>
/// Connects MSBuild to the same graph compiler used by the unified build workflow.
/// </summary>
public sealed class CompileShaderTask : BuildTask
{
    /// <summary>
    /// Gets or sets the authoring source directory.
    /// </summary>
    [Required]
    public string AssetRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source-local shader graph path.
    /// </summary>
    [Required]
    public string ShaderPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the graphics compiler target platform.
    /// </summary>
    [Required]
    public string Platform { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the graphics API identifier.
    /// </summary>
    [Required]
    public string Renderer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the compiled artifact destination.
    /// </summary>
    [Required]
    public string OutputFile { get; set; } = string.Empty;

    /// <summary>
    /// Compiles the requested graph artifact and reports compilation failures through MSBuild.
    /// </summary>
    /// <returns>
    /// True after writing the artifact; false when validation or compilation fails.
    /// </returns>
    public override bool Execute()
    {
        try
        {
            if (!Enum.TryParse(Platform, out BgfxShaderTargetPlatform platform)
                || !GraphicsApi.TryParse(Renderer, out GraphicsApi renderer))
                throw new ArgumentException("The shader platform or rendering API is invalid.");
            ShaderArtifactBuilder.Compile(AssetRoot, ShaderPath, platform, renderer, OutputFile);
            return true;
        }
        catch (Exception failure)
        {
            Log.LogErrorFromException(failure, showStackTrace: true);
            return false;
        }
    }
}
