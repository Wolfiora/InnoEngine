using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis;

namespace Inno.Tooling.Architecture;

internal sealed class WikiScriptReferences
{
    private readonly MetadataReference? m_runtime;
    private readonly MetadataReference? m_editor;
    private readonly string? m_runtimeIdentity;
    private readonly string? m_editorIdentity;

    internal WikiScriptReferences(
        string? runtimePath,
        string? editorPath
    ) {
        if (editorPath is not null && runtimePath is null)
            throw new ArgumentException("The Editor script reference requires a Runtime script reference.");
        (m_runtime, m_runtimeIdentity) = Read(runtimePath, "Inno.ScriptApi.Runtime");
        (m_editor, m_editorIdentity) = Read(editorPath, "Inno.ScriptApi.Editor");
    }

    internal IReadOnlyList<MetadataReference> GetReferences(bool editor)
    {
        if (m_runtime is null || (editor && m_editor is null))
            throw new InvalidOperationException("Logical script examples require explicit current --wiki-runtime-reference and, for Editor scripts, --wiki-editor-reference inputs.");
        return editor ? [m_runtime, m_editor!] : [m_runtime];
    }

    internal string GetIdentity(bool editor)
        => editor ? $"{m_runtimeIdentity};{m_editorIdentity}" : m_runtimeIdentity ?? string.Empty;

    private static (MetadataReference? reference, string? identity) Read(
        string? path,
        string expectedName
    ) {
        if (path is null)
            return (null, null);
        string absolute = Path.GetFullPath(path);
        byte[] bytes = File.ReadAllBytes(absolute);
        using var stream = new MemoryStream(bytes, writable: false);
        using var pe = new PEReader(stream);
        if (!pe.HasMetadata)
            throw new InvalidDataException($"The script reference is not a managed assembly: {absolute}");
        MetadataReader metadata = pe.GetMetadataReader();
        if (!metadata.IsAssembly || metadata.GetString(metadata.GetAssemblyDefinition().Name) != expectedName)
            throw new InvalidDataException($"Expected the {expectedName} logical script reference: {absolute}");
        // Compilation and evidence use the same frozen bytes, even if the source file changes later.
        return (MetadataReference.CreateFromImage(bytes, filePath: absolute),
            expectedName + ":" + Convert.ToHexString(SHA256.HashData(bytes)));
    }
}
