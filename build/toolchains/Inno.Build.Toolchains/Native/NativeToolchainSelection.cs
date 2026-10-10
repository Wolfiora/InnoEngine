using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Freezes target compiler tools and SDK inputs independently of discovery and platform policy.
/// </summary>
public sealed class NativeToolchainSelection
{
    private readonly IReadOnlyDictionary<string, string> m_tools;

    /// <summary>
    /// Captures an immutable, explicit compiler and SDK selection supplied by a platform provider.
    /// </summary>
    /// <param name="targetId">
    /// The exact native target identity.
    /// </param>
    /// <param name="host">
    /// The machine on which the selected tools execute.
    /// </param>
    /// <param name="tools">
    /// Absolute executable paths indexed by explicit command names.
    /// </param>
    /// <param name="environment">
    /// Frozen SDK values applied to owned tools and in-process binding configuration without changing process state.
    /// </param>
    /// <param name="inputPaths">
    /// Compiler, SDK and tool files participating in product identity.
    /// </param>
    /// <param name="cmakeArguments">
    /// Ordered configuration arguments pinning compiler, generator and SDK.
    /// </param>
    /// <param name="sharedLibraryExtension">
    /// The target's native shared-library suffix.
    /// </param>
    /// <param name="multiConfiguration">
    /// Whether configuration is selected during build rather than configure.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The selection is incomplete or has unassigned target facts.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// A required descriptor or collection is null.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// A selected executable or SDK input is unavailable before preparation begins.
    /// </exception>
    public NativeToolchainSelection(
        string targetId,
        BuildHostDescriptor host,
        IReadOnlyDictionary<string, string> tools,
        IReadOnlyDictionary<string, string> environment,
        IEnumerable<string> inputPaths,
        IEnumerable<string> cmakeArguments,
        string sharedLibraryExtension,
        bool multiConfiguration
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        ArgumentNullException.ThrowIfNull(host);
        ArgumentNullException.ThrowIfNull(tools);
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(inputPaths);
        ArgumentNullException.ThrowIfNull(cmakeArguments);
        ArgumentException.ThrowIfNullOrWhiteSpace(sharedLibraryExtension);
        if (tools.Count == 0 || tools.Any(static entry => string.IsNullOrWhiteSpace(entry.Key)
            || string.IsNullOrWhiteSpace(entry.Value) || !Path.IsPathFullyQualified(entry.Value)))
            throw new ArgumentException("A native tool selection requires explicit executables.", nameof(tools));
        string[] inputs = inputPaths.Concat(tools.Values).Distinct(StringComparer.Ordinal).ToArray();
        string[] arguments = cmakeArguments.ToArray();
        if (inputs.Any(static path => string.IsNullOrWhiteSpace(path) || !Path.IsPathFullyQualified(path)))
            throw new ArgumentException("Native SDK inputs require absolute locations.", nameof(inputPaths));
        if (arguments.Any(static argument => argument is null))
            throw new ArgumentException("CMake arguments cannot contain null entries.", nameof(cmakeArguments));
        if (environment.Any(static entry => string.IsNullOrWhiteSpace(entry.Key) || entry.Value is null))
            throw new ArgumentException("Native environments require assigned names and values.", nameof(environment));
        if (!sharedLibraryExtension.StartsWith('.') || sharedLibraryExtension.IndexOfAny(['/', '\\', ':']) >= 0)
            throw new ArgumentException("A native library suffix must be a file extension.", nameof(sharedLibraryExtension));
        foreach (string executable in tools.Values)
            if (!File.Exists(executable))
                throw new FileNotFoundException("A selected native executable is unavailable.", executable);
        foreach (string input in inputs)
            if (!File.Exists(input) && !Directory.Exists(input))
                throw new FileNotFoundException("A selected native SDK input is unavailable.", input);
        this.targetId = targetId;
        this.host = host;
        m_tools = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(tools, StringComparer.Ordinal));
        this.environment = new ReadOnlyDictionary<string, string>(
            new Dictionary<string, string>(environment,
                host.system == "Windows" ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal));
        this.inputPaths = Array.AsReadOnly(inputs);
        this.cmakeArguments = Array.AsReadOnly(arguments);
        this.sharedLibraryExtension = sharedLibraryExtension;
        this.multiConfiguration = multiConfiguration;
        declarations = Array.AsReadOnly(this.environment.OrderBy(static entry => entry.Key, StringComparer.Ordinal)
            .Select(static entry => entry.Key + "=" + entry.Value)
            .Concat(m_tools.OrderBy(static entry => entry.Key, StringComparer.Ordinal)
                .Select(static entry => entry.Key + "=" + entry.Value))
            .Concat(this.cmakeArguments)
            .Concat([targetId, host.system, host.architecture, sharedLibraryExtension, multiConfiguration.ToString()])
            .ToArray());
    }

    /// <summary>
    /// Gets the target identity, without inferring it from the execution host.
    /// </summary>
    public string targetId { get; }

    /// <summary>
    /// Gets the declared tool execution host.
    /// </summary>
    public BuildHostDescriptor host { get; }

    /// <summary>
    /// Gets the immutable SDK and compiler input closure.
    /// </summary>
    public IReadOnlyList<string> inputPaths { get; }

    /// <summary>
    /// Gets ordered tool and environment facts included in the recipe fingerprint.
    /// </summary>
    public IReadOnlyList<string> declarations { get; }

    /// <summary>
    /// Gets configuration arguments selecting the compiler, generator and SDK.
    /// </summary>
    public IReadOnlyList<string> cmakeArguments { get; }

    /// <summary>
    /// Gets frozen SDK values supplied to owned tools and in-process binding configuration.
    /// </summary>
    public IReadOnlyDictionary<string, string> environment { get; }

    /// <summary>
    /// Gets the target's shared-library suffix.
    /// </summary>
    public string sharedLibraryExtension { get; }

    /// <summary>
    /// Gets whether the selected generator supports build-time configuration selection.
    /// </summary>
    public bool multiConfiguration { get; }

    /// <summary>
    /// Resolves an executable from the frozen selection rather than a mutable process PATH.
    /// </summary>
    /// <param name="name">
    /// The explicit command registered by the platform provider.
    /// </param>
    /// <returns>
    /// The selected executable; an absent command fails immediately.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The provider did not register the requested command.
    /// </exception>
    public string ResolveExecutable(string name) => m_tools.TryGetValue(name, out string? executable)
        ? executable
        : throw new InvalidOperationException($"Native target '{targetId}' has no '{name}' executable.");

    /// <summary>
    /// Checks an optional tool capability without discovering a command from the process environment.
    /// </summary>
    /// <param name="name">
    /// The declared tool identity to check.
    /// </param>
    /// <param name="executable">
    /// The frozen absolute path when present, or an empty string when absent.
    /// </param>
    /// <returns>
    /// True only when this selection explicitly supplies the requested tool.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The tool identity is empty.
    /// </exception>
    public bool TryResolveExecutable(
        string name,
        out string executable
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (m_tools.TryGetValue(name, out string? selected))
        {
            executable = selected;
            return true;
        }
        executable = string.Empty;
        return false;
    }
}
