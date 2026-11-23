using System;
using System.Collections.Generic;

namespace Inno.Rendering.Shaders;

internal sealed class ShaderSourceSnapshotResolver : IShaderSourceResolver
{
    private readonly IShaderSourceResolver m_resolver;
    private readonly Dictionary<string, ShaderSourceFile> m_files = new(StringComparer.Ordinal);
    private readonly Dictionary<(string, string), ShaderSourceInclude> m_includes = [];

    internal ShaderSourceSnapshotResolver(ShaderSourceRequest request)
    {
        m_resolver = request.resolver;
        m_files.Add(request.source.assetPath, request.source);
    }

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
        if (!m_files.ContainsKey(includingFile))
        {
            throw new InvalidOperationException("An include must originate from an already captured source.");
        }
        ShaderSourceFile file = m_resolver.ReadInclude(includingFile, include)
                                ?? throw new InvalidOperationException("The source resolver returned no source snapshot.");
        ArgumentException.ThrowIfNullOrWhiteSpace(file.assetPath);
        ArgumentNullException.ThrowIfNull(file.text);
        if (m_files.TryGetValue(file.assetPath, out ShaderSourceFile? previous) && previous.text != file.text)
        {
            throw new InvalidOperationException($"Source '{file.assetPath}' changed during module analysis.");
        }
        if (m_includes.TryGetValue((includingFile, include), out ShaderSourceInclude? edge) && edge.resolvedPath != file.assetPath)
        {
            throw new InvalidOperationException($"Include '{include}' changed resolution during module analysis.");
        }
        m_includes[(includingFile, include)] = new(includingFile, include, file.assetPath);
        m_files[file.assetPath] = file;
        return file;
    }

    internal IEnumerable<ShaderSourceFile> files => m_files.Values;
    internal IEnumerable<ShaderSourceInclude> includes => m_includes.Values;
}
