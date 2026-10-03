using System;
using System.IO;
using System.Reflection;

namespace Inno.Build.Toolchains;

/// <summary>
/// Identifies the checkout and configuration owned by one native build operation.
/// </summary>
public sealed class NativeBuildContext
{
    /// <summary>
    /// Creates a build context without consulting the tool assembly's checkout.
    /// </summary>
    /// <param name="engineRoot">
    /// The checkout containing InnoEngine.sln and the native source tree.
    /// </param>
    /// <param name="configuration">
    /// The normalized debug or release configuration.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The root is empty or the configuration is unsupported.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The selected checkout has no repository marker.
    /// </exception>
    public NativeBuildContext(
        string engineRoot,
        string configuration
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        ToolchainEnvironment.ValidateConfiguration(configuration);
        this.engineRoot = Path.GetFullPath(engineRoot);
        if (!File.Exists(Path.Combine(this.engineRoot, ToolchainLayout.C_REPOSITORY_MARKER_FILE)))
            throw new DirectoryNotFoundException($"Engine checkout is unavailable at '{this.engineRoot}'.");
        this.configuration = configuration;
    }

    /// <summary>
    /// Gets the absolute checkout used for every source, intermediate and output path.
    /// </summary>
    public string engineRoot { get; }

    /// <summary>
    /// Gets the normalized debug or release configuration prepared by this operation.
    /// </summary>
    public string configuration { get; }

    /// <summary>
    /// Resolves native intermediates under the owning toolchain project in this checkout.
    /// </summary>
    /// <param name="toolchainAssembly">
    /// The component assembly whose project owns the native build tree.
    /// </param>
    /// <returns>
    /// The absolute project-local obj/native path, without creating the directory.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The owning assembly is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The selected checkout does not contain the owning toolchain project.
    /// </exception>
    public string GetNativeBuildRoot(Assembly toolchainAssembly)
    {
        ArgumentNullException.ThrowIfNull(toolchainAssembly);
        string projectName = toolchainAssembly.GetName().Name!;
        string projectRoot = Path.Combine(engineRoot, "build", "toolchains", projectName);
        if (!File.Exists(Path.Combine(projectRoot, projectName + ".csproj")))
            throw new InvalidOperationException($"Native build owner '{projectName}' is unavailable in '{engineRoot}'.");
        return Path.Combine(projectRoot, "obj", "native");
    }
}
