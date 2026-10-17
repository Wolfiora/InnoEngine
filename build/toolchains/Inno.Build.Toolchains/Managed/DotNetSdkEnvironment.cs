using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Isolates a managed tool invocation from the invoking IDE's SDK and MSBuild process identity.
/// </summary>
public static class DotNetSdkEnvironment
{
    private static readonly string[] AmbientSdkVariables =
    [
        "MSBuildSDKsPath", "MSBUILD_EXE_PATH", "MSBuildExtensionsPath",
        "MSBuildExtensionsPath32", "MSBuildExtensionsPath64",
        "DOTNET_MSBUILD_SDK_RESOLVER_CLI_DIR", "DOTNET_MSBUILD_SDK_RESOLVER_SDKS_DIR",
        "DOTNET_MSBUILD_SDK_RESOLVER_SDKS_VER"
    ];

    /// <summary>
    /// Freezes child-only overrides so the selected executable resolves the project's own SDK.
    /// Null SDK overrides remove inherited variables rather than borrowing a parent's loaded MSBuild.
    /// </summary>
    /// <param name="hostPath">
    /// The explicitly selected managed executable, resolved to an absolute path before invocation.
    /// </param>
    /// <param name="environment">
    /// Additional selected toolchain variables to copy, or null when no native SDK environment is required.
    /// </param>
    /// <returns>
    /// A read-only child environment that preserves selected toolchain settings and never changes the parent.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The managed executable is blank.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// The selected managed executable cannot be resolved.
    /// </exception>
    public static IReadOnlyDictionary<string, string?> Create(
        string hostPath,
        IReadOnlyDictionary<string, string>? environment = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostPath);
        string host = ToolchainEnvironment.ResolveExecutable(hostPath);
        var selected = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        if (environment is not null)
            foreach ((string name, string value) in environment)
                selected[name] = value;
        foreach (string variable in AmbientSdkVariables)
            selected[variable] = null;
        selected["DOTNET_HOST_PATH"] = host;
        selected["DOTNET_ROOT"] = Path.GetDirectoryName(host)!;
        return new ReadOnlyDictionary<string, string?>(selected);
    }
}
