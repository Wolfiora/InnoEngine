using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Inno.Build.Toolchains;

/// <summary>
/// Freezes a component's input closure, ordered tool selection and intermediate owner.
/// </summary>
public sealed class NativeBuildRecipe
{
    /// <summary>
    /// Creates a complete recipe without inferring implementation identity from an assembly MVID.
    /// </summary>
    /// <param name="owner">
    /// The toolchain assembly identifying the project that owns native intermediates.
    /// </param>
    /// <param name="component">
    /// The component's portable artifact path segment.
    /// </param>
    /// <param name="targetId">
    /// The ABI's portable artifact path segment.
    /// </param>
    /// <param name="inputs">
    /// The complete source, generator, executor and SDK input closure.
    /// </param>
    /// <param name="declarations">
    /// Ordered compiler, linker, include-search and non-file inputs; order is significant.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A component or target is not a portable segment, or an input is null.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// A required owner or collection is null.
    /// </exception>
    public NativeBuildRecipe(
        Assembly owner,
        string component,
        string targetId,
        IEnumerable<NativeBuildInput> inputs,
        IEnumerable<string> declarations
    ) {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(declarations);
        ValidateSegment(component);
        ValidateSegment(targetId);
        NativeBuildInput[] frozenInputs = inputs.ToArray();
        string[] frozenDeclarations = declarations.ToArray();
        if (frozenInputs.Any(static input => input is null)
            || frozenDeclarations.Any(static declaration => declaration is null))
            throw new ArgumentException("A recipe cannot contain null inputs or declarations.");
        this.owner = owner;
        this.component = component;
        this.targetId = targetId;
        this.inputs = Array.AsReadOnly(frozenInputs);
        this.declarations = Array.AsReadOnly(frozenDeclarations);
    }

    /// <summary>
    /// Gets the assembly identifying the intermediate owner, without using its MVID as a recipe input.
    /// </summary>
    public Assembly owner { get; }

    /// <summary>
    /// Gets the component's artifact path segment.
    /// </summary>
    public string component { get; }

    /// <summary>
    /// Gets the target ABI's artifact path segment.
    /// </summary>
    public string targetId { get; }

    /// <summary>
    /// Gets the frozen declarations of physical inputs and their logical identities.
    /// </summary>
    public IReadOnlyList<NativeBuildInput> inputs { get; }

    /// <summary>
    /// Gets ordered non-file inputs; callers must explicitly sort unordered metadata before construction.
    /// </summary>
    public IReadOnlyList<string> declarations { get; }

    /// <summary>
    /// Declares a built-in component with its own implementation, common executor and frozen host SDK.
    /// </summary>
    /// <param name="context">
    /// The operation's checkout and previously selected host tools.
    /// </param>
    /// <param name="owner">
    /// The component project whose implementation participates in recipe identity.
    /// </param>
    /// <param name="component">
    /// The native component artifact segment.
    /// </param>
    /// <param name="targetId">
    /// The selected target ABI.
    /// </param>
    /// <param name="inputPaths">
    /// All native sources, generated bridges, component tools and build definitions.
    /// </param>
    /// <param name="declarations">
    /// Ordered component arguments and generation identities.
    /// </param>
    /// <returns>
    /// The complete recipe; unrelated build and Editor implementation assemblies do not affect it.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// A required executor or component project is absent from the checkout.
    /// </exception>
    public static NativeBuildRecipe CreateForComponent(
        NativeBuildContext context,
        Assembly owner,
        string component,
        string targetId,
        IEnumerable<string> inputPaths,
        IEnumerable<string> declarations
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(inputPaths);
        ArgumentNullException.ThrowIfNull(declarations);
        string projectName = owner.GetName().Name!;
        string projectRoot = Path.Combine(context.engineRoot, "build", "toolchains", projectName);
        string projectFile = Path.Combine(projectRoot, projectName + ".csproj");
        if (!File.Exists(projectFile))
            throw new FileNotFoundException("A native recipe owner project is unavailable.", projectFile);
        List<string> paths = [..inputPaths, ..(context.hostToolchain?.inputPaths ?? []), projectFile];
        paths.AddRange(EnumerateImplementation(projectRoot));
        string commonRoot = Path.Combine(context.engineRoot, "build", "toolchains", "Inno.Build.Toolchains");
        paths.AddRange(new[] { "NativeBuildContext.cs", "ToolchainEnvironment.cs", "ToolchainLayout.cs", "BuildArtifactManifest.cs" }
            .Select(file => Path.Combine(commonRoot, file)));
        paths.AddRange(new[]
        {
            "NativeArtifactPublisher.cs", "NativeBuildFingerprint.cs", "NativeBuildInput.cs",
            "NativeBuildInputState.cs", "NativeInputSnapshot.cs", "NativeBuildProduct.cs",
            "NativeBuildStatistics.cs", "HostNativeToolchain.cs", "NativeBuildRecipe.cs"
        }.Select(file => Path.Combine(commonRoot, "Native", file)));
        paths.Add(Path.Combine(commonRoot, "Platforms"));
        string ioRoot = Path.Combine(context.engineRoot, "src", "foundation", "core", "Inno.Core.IO");
        paths.AddRange(new[] { "AtomicDirectory.cs", "AtomicFile.cs", "FileSystemRename.cs", "FileLease.cs", "PathBoundary.cs" }
            .Select(file => Path.Combine(ioRoot, file)));
        foreach (string nativeRoot in inputPaths.Select(path => Path.GetFullPath(path, context.engineRoot))
            .Select(path => FindNativeRoot(context.engineRoot, path)).OfType<string>().Distinct(StringComparer.Ordinal))
        {
            paths.Add(Path.Combine(nativeRoot, "Bindings"));
            paths.Add(Path.Combine(nativeRoot, "Generated", "Bindings.cs"));
        }
        return new NativeBuildRecipe(owner, component, targetId,
            paths.Select(path => NativeBuildInput.FromPath(context.engineRoot, path)),
            new[] { component, targetId, context.configuration, "executionRoot=" + context.engineRoot }.Concat(declarations)
                .Concat(context.hostToolchain?.declarations ?? []));
    }

    private static IEnumerable<string> EnumerateImplementation(string directory)
    {
        foreach (string file in Directory.EnumerateFiles(directory, "*.cs"))
            yield return file;
        foreach (string child in Directory.EnumerateDirectories(directory))
        {
            if (Path.GetFileName(child) is "obj" or "bin" or "Native")
                continue;
            foreach (string file in EnumerateImplementation(child))
                yield return file;
        }
    }

    private static string? FindNativeRoot(
        string engineRoot,
        string path
    ) {
        string relative = Path.GetRelativePath(Path.Combine(engineRoot, "native"), path).Replace('\\', '/');
        string segment = relative.Split('/')[0];
        return segment.StartsWith("Inno.Native.", StringComparison.Ordinal)
            ? Path.Combine(engineRoot, "native", segment) : null;
    }

    private static void ValidateSegment(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (value is "." or ".." || value.Any(static character => !char.IsAsciiLetterOrDigit(character)
            && character is not ('-' or '_' or '.')))
            throw new ArgumentException("Native component and target identities must be single path segments.", nameof(value));
    }
}
