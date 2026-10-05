using System;
using System.IO;
using System.Reflection;

namespace Inno.Build.Toolchains;

/// <summary>
/// Identifies the checkout and configuration owned by one native build operation.
/// </summary>
public sealed class NativeBuildContext
{
    private readonly string? m_buildOwner;
    private readonly string? m_targetId;
    private readonly string? m_fingerprint;
    private readonly string? m_toolDirectory;
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

    private NativeBuildContext(
        NativeBuildContext origin,
        string buildOwner,
        string targetId,
        string fingerprint
    ) : this(origin.engineRoot, origin.configuration) {
        m_buildOwner = buildOwner;
        m_targetId = targetId;
        m_fingerprint = fingerprint;
        hostToolchain = origin.hostToolchain;
    }

    private NativeBuildContext(
        NativeBuildContext origin,
        HostNativeToolchain toolchain
    ) : this(origin.engineRoot, origin.configuration) {
        hostToolchain = toolchain;
        m_buildOwner = origin.m_buildOwner;
        m_targetId = origin.m_targetId;
        m_fingerprint = origin.m_fingerprint;
        m_toolDirectory = origin.m_toolDirectory;
    }

    private NativeBuildContext(
        NativeBuildContext origin,
        string toolDirectory
    ) : this(origin.engineRoot, origin.configuration) {
        m_buildOwner = origin.m_buildOwner;
        m_targetId = origin.m_targetId;
        m_fingerprint = origin.m_fingerprint;
        hostToolchain = origin.hostToolchain;
        m_toolDirectory = toolDirectory;
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
    /// Gets the frozen host compiler and SDK selection, or null before host discovery.
    /// Browser toolchains resolve their own workload-specific SDK independently.
    /// </summary>
    public HostNativeToolchain? hostToolchain { get; }

    /// <summary>
    /// Resolves native intermediates under the owning toolchain project in this checkout.
    /// </summary>
    /// <param name="toolchainAssembly">
    /// The component assembly whose project owns the native build tree.
    /// </param>
    /// <returns>
    /// The absolute project-local obj/native path, without creating the directory. During publication,
    /// the path also contains the operation's target and input fingerprint. A Windows producer receives
    /// a temporary junction to the same physical directory so SDK tools can use bounded paths.
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
        string root = Path.Combine(projectRoot, "obj", "native");
        if (m_buildOwner is null)
            return root;
        if (projectName != m_buildOwner)
            throw new InvalidOperationException($"The native operation belongs to '{m_buildOwner}', not '{projectName}'.");
        return m_toolDirectory ?? Path.Combine(root, m_targetId!, m_fingerprint!);
    }

    internal NativeBuildContext WithIdentity(
        Assembly owner,
        string targetId,
        string fingerprint
    ) => new(this, owner.GetName().Name!, targetId, fingerprint);

    internal NativeBuildContext WithHostToolchain(HostNativeToolchain toolchain) => new(this, toolchain);

    internal NativeBuildContext WithToolDirectory(string toolDirectory) => new(this, toolDirectory);
}
