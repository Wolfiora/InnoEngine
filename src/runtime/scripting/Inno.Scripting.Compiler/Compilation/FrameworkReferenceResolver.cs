using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Scripting.Compiler;

internal static class FrameworkReferenceResolver
{
    internal static ScriptCompilationEnvironment Resolve(
        IScriptCompilationEnvironmentProvider provider,
        CancellationToken cancellationToken = default
    ) {
        ScriptCompilationInputs inputs = provider.Capture(cancellationToken);
        var references = new List<MetadataReference>();
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "System.Private.CoreLib" };
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        foreach (string identity in new[]
                 {
                     ScriptCompilationEnvironment.C_TARGET_FRAMEWORK,
                     ScriptCompilationEnvironment.C_LANGUAGE_VERSION,
                     typeof(CSharpCompilation).Module.ModuleVersionId.ToString("N"),
                     typeof(FrameworkReferenceResolver).Module.ModuleVersionId.ToString("N"),
                     inputs.sdkIdentity,
                     string.Join(";", inputs.predefinedSymbols)
                 })
        {
            hash.AppendData(Encoding.UTF8.GetBytes(identity));
            hash.AppendData([0]);
        }
        foreach (string path in inputs.frameworkReferences)
        {
            cancellationToken.ThrowIfCancellationRequested();
            hash.AppendData(Encoding.UTF8.GetBytes(Path.GetFileName(path)));
            hash.AppendData([0]);
            byte[] image = File.ReadAllBytes(path);
            hash.AppendData(SHA256.HashData(image));
            references.Add(MetadataReference.CreateFromImage(
                ImmutableCollectionsMarshal.AsImmutableArray(image), filePath: path));
            names.Add(Path.GetFileNameWithoutExtension(path));
        }
        return new ScriptCompilationEnvironment(
            references.AsReadOnly(), names, Convert.ToHexStringLower(hash.GetHashAndReset()), inputs);
    }

}
