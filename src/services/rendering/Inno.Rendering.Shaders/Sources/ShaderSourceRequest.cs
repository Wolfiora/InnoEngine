using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Carries the immutable inputs required to execute one shader source request.
/// </summary>
public sealed class ShaderSourceRequest
{
    /// <summary>
    /// Creates an analysis request for an explicitly selected function.
    /// </summary>
    /// <param name="source">
    /// Root source snapshot.
    /// </param>
    /// <param name="entryPoint">
    /// The one public function selected by source import settings.
    /// </param>
    /// <param name="resolver">
    /// Candidate-scoped include resolver.
    /// </param>
    /// <param name="defines">
    /// Target and variant preprocessing inputs, or null for no defines.
    /// </param>
    public ShaderSourceRequest(
        ShaderSourceFile source,
        string entryPoint,
        IShaderSourceResolver resolver,
        IReadOnlyDictionary<string, string>? defines = null
    ) {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(resolver);
        this.source = source;
        ArgumentException.ThrowIfNullOrWhiteSpace(source.assetPath);
        ArgumentNullException.ThrowIfNull(source.text);
        ArgumentException.ThrowIfNullOrWhiteSpace(entryPoint);
        this.entryPoint = entryPoint;
        this.resolver = resolver;
        this.defines = new ReadOnlyDictionary<string, string>(
            defines is null
            ? new Dictionary<string, string>(StringComparer.Ordinal)
            : new Dictionary<string, string>(defines, StringComparer.Ordinal)
        );
    }

    /// <summary>
    /// Gets the root source snapshot.
    /// </summary>
    public ShaderSourceFile source { get; }

    /// <summary>
    /// Gets the explicitly selected export function name.
    /// </summary>
    public string entryPoint { get; }

    /// <summary>
    /// Gets the candidate-scoped dependency resolver.
    /// </summary>
    public IShaderSourceResolver resolver { get; }

    /// <summary>
    /// Gets immutable preprocessing inputs included in analysis cache identity.
    /// </summary>
    public IReadOnlyDictionary<string, string> defines { get; }
}
