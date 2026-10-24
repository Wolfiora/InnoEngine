using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

internal static class NativeBindingCMake
{
    internal static void Write(
        NativeBuildContext context,
        NativeCMakeSource sources,
        string path,
        NativeComponentDescriptor owner,
        string? target,
        CancellationToken cancellationToken
    ) {
        var text = new StringBuilder("include_guard(GLOBAL)\nfunction(inno_apply_binding target owner)\n");
        foreach (var entry in context.selectedBindings.Where(entry => target is null || entry.Key == owner.nativeProject)
            .OrderBy(static entry => entry.Key, StringComparer.Ordinal))
        {
            NativeBindingGenerationDescriptor generation = entry.Value;
            if (generation.nativeDirectory.Length == 0)
                continue;
            NativeBindingCompilation compilation = NativeBindingCompilation.Load(generation.nativeDirectory);
            string ResolveRoot(string id) => sources.ResolvePath(compilation.ResolveRoot(context.engineRoot, generation.nativeDirectory, id));
            compilation.VerifyInputs(ResolveRoot, cancellationToken);
            text.AppendLine("if(owner STREQUAL " + Literal(Path.GetFileNameWithoutExtension(entry.Key)) + ")");
            NativeBindingSourceFile[] units = compilation.files.Where(static file => file.compile).ToArray();
            if (units.Length > 0)
            {
                // Generated sources consume the public header contract, never the producer's private build macros.
                text.AppendLine("set(binding_target \"${target}_inno_binding\")");
                text.AppendLine("add_library(${binding_target} OBJECT)");
                text.AppendLine("set_property(TARGET ${binding_target} PROPERTY POSITION_INDEPENDENT_CODE ON)");
                foreach (NativeBindingSourceFile file in units)
                {
                    string source = Literal(PathBoundary.Resolve(ResolveRoot(file.rootId), file.relativePath));
                    text.AppendLine("target_sources(${binding_target} PRIVATE " + source + ")");
                }
                foreach (string define in compilation.defines)
                    text.AppendLine("target_compile_definitions(${binding_target} PRIVATE " + Literal(define) + ")");
                foreach (NativeBindingIncludeDirectory include in compilation.includeDirectories)
                {
                    NativeBindingSourceRoot root = compilation.roots.Single(root => root.id == include.rootId);
                    // SDK search roots are requirements of the selected compiler, not duplicate -isystem flags.
                    if (!root.frozen && include.system && !include.framework)
                        continue;
                    string directory = include.relativePath.Length == 0 ? ResolveRoot(include.rootId)
                        : sources.ResolvePath(PathBoundary.Resolve(compilation.ResolveRoot(context.engineRoot, generation.nativeDirectory, include.rootId), include.relativePath));
                    if (include.framework)
                        text.AppendLine("target_compile_options(${binding_target} PRIVATE " + Literal("SHELL:-iframework \"" + directory.Replace('\\', '/') + "\"") + ")");
                    else
                        text.AppendLine("target_include_directories(${binding_target} " + (include.system ? "SYSTEM " : string.Empty)
                            + "PRIVATE " + Literal(directory) + ")");
                }
                if (compilation.cxxStandard > 0)
                    text.AppendLine("target_compile_features(${binding_target} PRIVATE cxx_std_" + compilation.cxxStandard + ")");
                if (compilation.cStandard > 0)
                    text.AppendLine("target_compile_features(${binding_target} PRIVATE c_std_" + compilation.cStandard + ")");
                text.AppendLine("target_sources(${target} PRIVATE $<TARGET_OBJECTS:${binding_target}>)");
            }
            text.Append("set_property(TARGET ${target} PROPERTY INNO_REQUIRED_EXPORTS");
            foreach (string symbol in compilation.requiredExports.Concat(compilation.staticExportSymbols).Distinct(StringComparer.Ordinal))
                text.Append(' ').Append(Literal(symbol));
            text.AppendLine(")\nreturn()\nendif()");
        }
        text.AppendLine("message(FATAL_ERROR \"No selected native binding requirements for ${owner}\")\nendfunction()");
        if (target is not null && context.selectedBindings.ContainsKey(owner.nativeProject))
            text.AppendLine("cmake_language(DEFER DIRECTORY \"${CMAKE_SOURCE_DIR}\" CALL inno_apply_binding "
                + Literal(target) + " " + Literal(Path.GetFileNameWithoutExtension(owner.nativeProject)) + ")");
        AtomicFile.WriteAllBytes(path, Encoding.UTF8.GetBytes(text.ToString()));
    }

    private static string Literal(string value)
    {
        if (value.Contains(';') || value.AsSpan().IndexOfAnyInRange('\0', '\x1f') >= 0)
            throw new InvalidDataException("A native CMake requirement cannot contain a list separator or control character.");
        string portable = value.Replace('\\', '/');
        string delimiter = "=";
        while (portable.Contains("]" + delimiter + "]", StringComparison.Ordinal))
            delimiter += "=";
        return "[" + delimiter + "[" + portable + "]" + delimiter + "]";
    }
}
