using System;
using System.Collections.Generic;
using Inno.Core.Diagnostics;
using Inno.Rendering.Shaders;

namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Parses BGFX SC function modules without placing BGFX grammar in the common shader model.
/// </summary>
public sealed class BgfxShaderSourceFrontend : IShaderSourceFrontend
{
    /// <summary>
    /// Gets the language id text used by the current instance.
    /// </summary>
    public string languageID => "inno.shader-language.bgfx-sc";

    /// <summary>
    /// Analyzes source text and returns validated output with diagnostics.
    /// </summary>
    /// <param name="request">
    /// The validated immutable request that defines this operation.
    /// </param>
    /// <returns>
    /// The validated shader source analysis that represents the completed operation.
    /// </returns>
    public ShaderSourceAnalysis Analyze(ShaderSourceRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var preprocessor = new BgfxSourcePreprocessor(request);
        try
        {
            List<BgfxSourceToken> tokens = preprocessor.Process();
            ShaderSourceFunction function = new BgfxSourceDeclarationParser(tokens, request).Parse();
            return new(function, preprocessor.dependencies, []);
        }
        catch (BgfxSourceSyntaxException failure)
        {
            return new(
                null,
                preprocessor.dependencies,
                [new ShaderSourceDiagnostic("SHADER_SOURCE_INTERFACE", DiagnosticSeverity.Error, failure.Message, failure.position)]
            );
        }
    }
}
