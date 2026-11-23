using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Freezes a parsed implementation and its complete source inputs without retaining a frontend or resolver.
/// </summary>
public sealed class ShaderSourceImplementationAnalysis
{
    internal ShaderSourceImplementationAnalysis(
        ShaderSourceImplementationRequest request,
        ShaderSourceAnalysis analysis,
        IEnumerable<ShaderSourceFile> files,
        IEnumerable<ShaderSourceInclude> includes
    ) {
        this.implementationID = request.implementationID;
        this.languageID = request.languageID;
        this.sourcePath = request.source.source.assetPath;
        this.entryPoint = request.source.entryPoint;
        this.defines = request.source.defines;
        this.analysis = analysis;
        this.sources = Array.AsReadOnly(files.OrderBy(static file => file.assetPath, StringComparer.Ordinal).ToArray());
        this.includes = Array.AsReadOnly(
            includes.OrderBy(static edge => edge.includingFile, StringComparer.Ordinal)
                .ThenBy(static edge => edge.include, StringComparer.Ordinal)
                .ToArray()
        );
        this.contentHash = ComputeContentHash();
    }

    /// <summary>
    /// Gets the stable implementation/configuration key.
    /// </summary>
    public string implementationID { get; }

    /// <summary>
    /// Gets the selected source language.
    /// </summary>
    public string languageID { get; }

    /// <summary>
    /// Gets the root source path in the frozen source set.
    /// </summary>
    public string sourcePath { get; }

    /// <summary>
    /// Gets the implementation's selected callable function name.
    /// </summary>
    public string entryPoint { get; }

    /// <summary>
    /// Gets the immutable preprocessing inputs used for this implementation.
    /// </summary>
    public IReadOnlyDictionary<string, string> defines { get; }

    /// <summary>
    /// Gets every successfully read source, including the root, ordered by path.
    /// </summary>
    public IReadOnlyList<ShaderSourceFile> sources { get; }

    /// <summary>
    /// Gets the original resolver's frozen include edges, including aliases and mounted source paths.
    /// </summary>
    public IReadOnlyList<ShaderSourceInclude> includes { get; }

    /// <summary>
    /// Gets the parsed interface and implementation-local diagnostics.
    /// </summary>
    public ShaderSourceAnalysis analysis { get; }

    /// <summary>
    /// Gets a deterministic source-input hash. A compiler cache must additionally include its toolchain,
    /// target, graph semantics and binding layout; this hash alone is not a compiled artifact key.
    /// </summary>
    public string contentHash { get; }

    /// <summary>
    /// Recreates a compiler input backed only by immutable captured sources and resolution edges.
    /// </summary>
    /// <returns>
    /// A request with no filesystem access or reference to the original frontend, resolver or generation.
    /// </returns>
    public ShaderSourceRequest CreateSourceRequest()
        => new(sources.Single(file => file.assetPath == sourcePath), entryPoint, new FrozenResolver(sources, includes), defines);

    private string ComputeContentHash()
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(languageID);
        writer.Write(sourcePath);
        writer.Write(entryPoint);
        writer.Write(defines.Count);
        foreach ((string key, string value) in defines.OrderBy(static item => item.Key, StringComparer.Ordinal))
        {
            writer.Write(key);
            writer.Write(value);
        }
        writer.Write(sources.Count);
        foreach (ShaderSourceFile file in sources)
        {
            writer.Write(file.assetPath);
            writer.Write(file.text);
        }
        writer.Write(includes.Count);
        foreach (ShaderSourceInclude edge in includes)
        {
            writer.Write(edge.includingFile);
            writer.Write(edge.include);
            writer.Write(edge.resolvedPath);
        }
        writer.Flush();
        return Convert.ToHexString(SHA256.HashData(stream.GetBuffer().AsSpan(0, checked((int)stream.Length))));
    }

    private sealed class FrozenResolver(
        IReadOnlyList<ShaderSourceFile> sources,
        IReadOnlyList<ShaderSourceInclude> includes
    ) : IShaderSourceResolver
    {
        /// <summary>
        /// Reads and validates the include value from its authoritative source.
        /// </summary>
        /// <param name="includingFile">
        /// The including file text validated by the read include operation.
        /// </param>
        /// <param name="include">
        /// The include text validated by the read include operation.
        /// </param>
        /// <returns>
        /// The validated shader source file that represents the completed operation.
        /// </returns>
public ShaderSourceFile ReadInclude(
    string includingFile,
    string include
) {
            ShaderSourceInclude edge = includes.FirstOrDefault(value => value.includingFile == includingFile && value.include == include)
                                       ?? throw new InvalidOperationException(
                                           $"Include '{include}' from '{includingFile}' was not captured during source analysis."
                                       );
            return sources.Single(file => file.assetPath == edge.resolvedPath);
        }
    }
}
