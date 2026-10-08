using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Build.Toolchains;

namespace Inno.Build.Browser;

/// <summary>
/// Validates the platform runtime or linker closure before compilation and installation.
/// </summary>
public sealed class BrowserSupportPackValidator : IPlayerSupportPackValidator
{
    private readonly string[] m_archives;
    private readonly string[] m_bindings;

    /// <summary>
    /// Captures the selected static closure so validation and publication use the same backend declarations.
    /// </summary>
    /// <param name="nativePlan">
    /// The immutable Player closure whose component owners declare archives and generated assemblies.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The plan is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A selected component does not provide a static build definition.
    /// </exception>
    public BrowserSupportPackValidator(ProductNativeBuildPlan nativePlan)
    {
        ArgumentNullException.ThrowIfNull(nativePlan);
        if (nativePlan.steps.Any(static step => step.component.staticBuild is null))
            throw new ArgumentException("A browser Support Pack requires a complete static component closure.", nameof(nativePlan));
        m_archives = nativePlan.steps.SelectMany(step => step.component.staticBuild!.archiveNames
            .Select(archive => Path.Combine(step.id, BuildTargetId.browserWasm.value, archive))).ToArray();
        m_bindings = nativePlan.steps.Where(static step => step.component.bindingConfig is not null)
            .Select(static step => Path.GetFileNameWithoutExtension(step.component.nativeProject) + ".dll").ToArray();
    }

    /// <summary>
    /// Rejects incomplete or foreign platform inputs in the supplied Support Pack.
    /// </summary>
    /// <param name="directory">
    /// The isolated or installed target directory to validate.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// A required input is absent or the closure contains a foreign native binary.
    /// </exception>
    public void Validate(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        string linker = Path.Combine(directory, "PlayerLink");
        foreach (string input in new[]
        {
            "Player.csproj", "Program.cs", "BrowserPlayerComposition.cs", "HttpPlayerContentSource.cs", "BrowserBridge.cs", "global.json",
            Path.Combine("Analyzers", "Inno.Runtime.Generators.dll"),
            Path.Combine("Analyzers", "Inno.Core.Serialization.Generators.dll"),
            Path.Combine("wwwroot", "index.html"), Path.Combine("wwwroot", "main.js"),
            Path.Combine("Native", "wasm_sjlj_shim.c"),
            Path.Combine("References", "Inno.Player.Runtime.dll")
        })
        {
            if (!File.Exists(Path.Combine(linker, input)))
                throw new InvalidDataException($"The browser Support Pack lacks linker input '{input}'.");
        }
        foreach (string binding in m_bindings)
            if (!File.Exists(Path.Combine(linker, "References", binding)))
                throw new InvalidDataException($"The browser Support Pack lacks generated binding '{binding}'.");
        foreach (string archive in m_archives)
        {
            if (!File.Exists(Path.Combine(linker, "Native", archive)))
                throw new InvalidDataException($"The browser Support Pack lacks native archive '{archive}'.");
        }
        string nativeDirectory = Path.Combine(linker, "Native");
        var actualArchives = new HashSet<string>(Directory.EnumerateFiles(nativeDirectory, "*.a", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(nativeDirectory, path)), StringComparer.Ordinal);
        if (!actualArchives.SetEquals(m_archives))
            throw new InvalidDataException("The browser Support Pack contains archives outside its selected static component closure.");
        string[] foreignBinaries = Directory.EnumerateFiles(nativeDirectory, "*", SearchOption.AllDirectories)
            .Where(path => new[] { ".dll", ".so", ".dylib", ".lib" }
                .Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();
        if (foreignBinaries.Length != 0)
            throw new InvalidDataException($"The browser Support Pack contains a foreign native binary '{Path.GetFileName(foreignBinaries[0])}'.");
    }
}
