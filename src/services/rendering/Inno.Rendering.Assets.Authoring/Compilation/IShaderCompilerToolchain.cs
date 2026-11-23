using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Defines a backend-owned target compiler used by the common Shader IR pipeline.
/// </summary>
[StableTypeID("00437bae-b212-505f-8017-d59fd29fb005")]
public interface IShaderCompilerToolchain
{
    /// <summary>
    /// Gets the stable source implementation identity paired with this rendering adapter.
    /// </summary>
    string implementationID { get; }

    /// <summary>
    /// Gets the explicit source-language identities accepted by this adapter's typed IR generator.
    /// </summary>
    IReadOnlyList<string> supportedSourceLanguages { get; }

    /// <summary>
    /// Generates and compiles one typed stage, including its frozen function modules and resource layout.
    /// </summary>
    /// <param name="request">
    /// Typed stage and exact target profile.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation before publishing the candidate.
    /// </param>
    /// <returns>
    /// Immutable compiled bytes, generated binding names and structured diagnostics.
    /// </returns>
    ValueTask<ShaderStageToolResult> CompileAsync(
        ShaderStageToolRequest request,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Creates a target supported by this toolchain and capability snapshot.
    /// </summary>
    /// <param name="capabilities">
    /// Target renderer capabilities.
    /// </param>
    /// <param name="optimize">
    /// Whether release optimization is enabled.
    /// </param>
    /// <param name="debugInformation">
    /// Whether compiler debug information is emitted.
    /// </param>
    /// <returns>
    /// A stable target description accepted by this toolchain.
    /// </returns>
    ShaderCompileTarget CreateTarget(
        GraphicsCapabilities capabilities,
        bool optimize = true,
        bool debugInformation = false
    );
}
