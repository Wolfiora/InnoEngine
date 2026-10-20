using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Inno.Build.Toolchains;

/// <summary>
/// Maps only the selected binding inputs to stable, owner-specific workspace locations.
/// Artifact publication continues to fingerprint the original paths and complete input bytes.
/// </summary>
internal sealed class NativeBindingInputPaths
{
    private readonly List<(string physical, string relative, bool file)> m_roots = [];
    private readonly HashSet<string> m_generationDeclarations = new(StringComparer.Ordinal);
    private readonly List<string> m_sourceRoots = [];
    private readonly string m_engineRoot;
    private readonly StringComparison m_comparison = OperatingSystem.IsWindows()
        ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    internal NativeBindingInputPaths(NativeBuildContext context)
    {
        m_engineRoot = context.engineRoot;
        foreach (KeyValuePair<string, NativeBindingGenerationDescriptor> entry in context.selectedBindings)
        {
            NativeBindingGenerationDescriptor generation = entry.Value;
            string root = ".inno-bindings/" + Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(entry.Key)));
            m_generationDeclarations.Add(entry.Key + "=" + generation.fingerprint);
            if (generation.artifactDirectory.Length > 0)
                Add(generation.artifactDirectory, root, false);
            else
            {
                Add(generation.bindingsPath, root + "/Generated/Bindings.cs", true);
                if (generation.nativeDirectory.Length > 0)
                    Add(generation.nativeDirectory, root + "/Native", false);
            }
            if (generation.nativeDirectory.Length > 0)
            {
                NativeBindingCompilation compilation = NativeBindingCompilation.Load(generation.nativeDirectory);
                foreach (NativeBindingSourceRoot source in compilation.roots.Where(static source => source.frozen && !source.generated))
                    m_sourceRoots.Add(compilation.ResolveRoot(context.engineRoot, generation.nativeDirectory, source.id));
            }
        }
    }

    internal string? ResolveRelative(string path)
    {
        string absolute = Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
        foreach (var entry in m_roots)
        {
            if (string.Equals(absolute, entry.physical, m_comparison))
                return entry.relative;
            if (!entry.file && absolute.StartsWith(entry.physical + Path.DirectorySeparatorChar, m_comparison))
                return entry.relative + "/" + Path.GetRelativePath(entry.physical, absolute).Replace('\\', '/');
        }
        string checkoutRelative = Path.GetRelativePath(m_engineRoot, absolute);
        if ((checkoutRelative == ".." || checkoutRelative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal)
            || Path.IsPathRooted(checkoutRelative)) && m_sourceRoots.Any(root => string.Equals(root, absolute, m_comparison)
                || absolute.StartsWith(Path.TrimEndingDirectorySeparator(root) + Path.DirectorySeparatorChar, m_comparison)))
        {
            string volume = Path.GetPathRoot(absolute)!;
            return ".inno-source-roots/" + Base64Url.EncodeToString(SHA256.HashData(Encoding.UTF8.GetBytes(volume)))
                + "/" + Path.GetRelativePath(volume, absolute).Replace('\\', '/');
        }
        return null;
    }

    internal string NormalizeDeclaration(string declaration)
    {
        if (m_generationDeclarations.Contains(declaration))
            return declaration[..(declaration.IndexOf('=') + 1)] + "selected-generation";
        const string nativePrefix = "bindingNativeRoot=";
        if (declaration.StartsWith(nativePrefix, StringComparison.Ordinal))
        {
            string path = declaration[nativePrefix.Length..];
            return path.Length > 0 && ResolveRelative(path) is { } native
                ? nativePrefix + "workspace-input:" + native : declaration;
        }
        int start = declaration.IndexOf("-D", StringComparison.Ordinal);
        if (start < 0 || start > 0 && declaration[start - 1] != '=')
            return declaration;
        int separator = declaration.IndexOf('=', start);
        if (separator < 0)
            return declaration;
        string[] values = declaration[(separator + 1)..].Split(';');
        for (int index = 0; index < values.Length; index++)
        {
            if (Path.IsPathFullyQualified(values[index]) && ResolveRelative(values[index]) is { } relative)
                values[index] = "workspace-input:" + relative;
        }
        return declaration[..(separator + 1)] + string.Join(';', values);
    }

    private void Add(
        string physical,
        string relative,
        bool file
    ) {
        string absolute = Path.TrimEndingDirectorySeparator(Path.GetFullPath(physical));
        if (m_roots.Any(entry => string.Equals(entry.physical, absolute, m_comparison)
            || !entry.file && absolute.StartsWith(entry.physical + Path.DirectorySeparatorChar, m_comparison)
            || !file && entry.physical.StartsWith(absolute + Path.DirectorySeparatorChar, m_comparison)))
            throw new InvalidOperationException("Selected binding owners cannot share or contain each other's output roots.");
        m_roots.Add((absolute, relative, file));
    }
}
