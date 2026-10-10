using System;
using System.IO;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Declares the unique source and build owners of a reusable native component.
/// </summary>
public sealed class NativeComponentDescriptor
{
    /// <summary>
    /// Freezes portable checkout-relative owner locations without assembly-name or directory inference.
    /// </summary>
    /// <param name="id">
    /// The portable component artifact identity.
    /// </param>
    /// <param name="nativeProject">
    /// The checkout-relative native project file.
    /// </param>
    /// <param name="toolchainProject">
    /// The checkout-relative build implementation project file.
    /// </param>
    /// <param name="staticBuild">
    /// The backend-owned static recipe, or null when this component has none.
    /// </param>
    /// <param name="bindingDefinition">
    /// The portable binding definition, or null when the component generates no bindings.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An identity or path is empty, absolute, or escapes its checkout.
    /// </exception>
    public NativeComponentDescriptor(
        string id,
        string nativeProject,
        string toolchainProject,
        NativeStaticBuildDefinition? staticBuild = null,
        string? bindingDefinition = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        if (id is "." or "..")
            throw new ArgumentException("A component identity must be a portable path segment.", nameof(id));
        foreach (char character in id)
            if (!char.IsAsciiLetterOrDigit(character) && character is not ('-' or '_' or '.'))
                throw new ArgumentException("A component identity must be a portable path segment.", nameof(id));
        this.id = id;
        this.nativeProject = ValidateProject(nativeProject);
        this.toolchainProject = ValidateProject(toolchainProject);
        this.staticBuild = staticBuild;
        if (bindingDefinition is not null && (bindingDefinition.StartsWith('/') || bindingDefinition.Contains(':')
            || bindingDefinition.Split('/').Any(static segment => segment is "" or "." or "..")))
            throw new ArgumentException("A binding definition must remain within its checkout.", nameof(bindingDefinition));
        this.bindingDefinition = bindingDefinition;
    }

    /// <summary>
    /// Gets the portable artifact identity.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the explicit checkout-relative Native owner project.
    /// </summary>
    public string nativeProject { get; }

    /// <summary>
    /// Gets the explicit checkout-relative recipe owner project.
    /// </summary>
    public string toolchainProject { get; }

    /// <summary>
    /// Gets the declared static build capability without requiring a platform-specific component project.
    /// </summary>
    public NativeStaticBuildDefinition? staticBuild { get; }

    /// <summary>
    /// Gets the declared binding definition, or null for a component without generated bindings.
    /// </summary>
    public string? bindingDefinition { get; }

    /// <summary>
    /// Resolves the declared native owner within an explicitly supplied checkout.
    /// </summary>
    /// <param name="engineRoot">
    /// The absolute operation-owned checkout.
    /// </param>
    /// <returns>
    /// The absolute native project directory, without creating it.
    /// </returns>
    public string GetNativeRoot(string engineRoot) => ResolveRoot(engineRoot, nativeProject);

    /// <summary>
    /// Resolves the declared recipe owner within an explicitly supplied checkout.
    /// </summary>
    /// <param name="engineRoot">
    /// The absolute operation-owned checkout.
    /// </param>
    /// <returns>
    /// The absolute toolchain project directory, without creating it.
    /// </returns>
    public string GetToolchainRoot(string engineRoot) => ResolveRoot(engineRoot, toolchainProject);

    private static string ResolveRoot(
        string engineRoot,
        string project
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        if (!Path.IsPathFullyQualified(engineRoot))
            throw new ArgumentException("The operation checkout must be absolute.", nameof(engineRoot));
        return Path.GetDirectoryName(Path.GetFullPath(project, engineRoot))!;
    }

    private static string ValidateProject(string project)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(project);
        string portable = project.Replace('\\', '/');
        if (portable.StartsWith('/') || portable.Contains(':') || !portable.EndsWith(".csproj", StringComparison.Ordinal))
            throw new ArgumentException("A component owner must be a checkout-relative project file.", nameof(project));
        foreach (string segment in portable.Split('/'))
            if (segment is "" or "." or "..")
                throw new ArgumentException("A component owner cannot escape its checkout.", nameof(project));
        return portable;
    }
}
