using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Supplies shared environment isolation and SDK input validation without selecting a platform.
/// </summary>
public static class NativeToolchainPreparation
{
    private static readonly string[] AmbientCompilerVariables =
    [
        "CL", "_CL_", "LINK", "_LINK_", "CFLAGS", "CXXFLAGS", "CPPFLAGS", "LDFLAGS",
        "CPATH", "C_INCLUDE_PATH", "CPLUS_INCLUDE_PATH", "LIBRARY_PATH",
        "CMAKE_GENERATOR", "CMAKE_GENERATOR_PLATFORM", "CMAKE_GENERATOR_TOOLSET", "CMAKE_GENERATOR_INSTANCE"
    ];

    /// <summary>
    /// Creates a child-process environment that clears undeclared ambient compiler policy.
    /// </summary>
    /// <returns>
    /// A caller-owned dictionary to populate with explicitly selected SDK variables.
    /// </returns>
    public static Dictionary<string, string> CreateEnvironment() => AmbientCompilerVariables.ToDictionary(
        static variable => variable, static _ => string.Empty, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Validates and freezes a provider's complete tool selection before product staging.
    /// </summary>
    /// <param name="targetId">
    /// The explicit requested target.
    /// </param>
    /// <param name="host">
    /// The declared tool execution host.
    /// </param>
    /// <param name="tools">
    /// Selected executable paths.
    /// </param>
    /// <param name="environment">
    /// Isolated child environment.
    /// </param>
    /// <param name="inputs">
    /// Selected compiler and SDK inputs.
    /// </param>
    /// <param name="cmakeArguments">
    /// Ordered target configuration arguments.
    /// </param>
    /// <param name="sharedLibraryExtension">
    /// The target's native output suffix.
    /// </param>
    /// <param name="multiConfiguration">
    /// Whether configuration is chosen during build.
    /// </param>
    /// <returns>
    /// An immutable selection with validated physical tool and SDK inputs.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// A selected SDK input or executable is absent.
    /// </exception>
    public static NativeToolchainSelection Freeze(
        string targetId,
        BuildHostDescriptor host,
        IReadOnlyDictionary<string, string> tools,
        IReadOnlyDictionary<string, string> environment,
        IEnumerable<string> inputs,
        IEnumerable<string> cmakeArguments,
        string sharedLibraryExtension,
        bool multiConfiguration
    ) {
        var complete = new List<string>(inputs);
        if (tools.TryGetValue("cmake", out string? cmake))
        {
            string modules = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(cmake)!, "..", "share"));
            if (Directory.Exists(modules))
                complete.Add(modules);
        }
        complete.AddRange(tools.Values);
        foreach (string input in complete)
            if (!File.Exists(input) && !Directory.Exists(input))
                throw new FileNotFoundException("A selected native toolchain input is unavailable.", input);
        return new(targetId, host, tools, environment, complete, cmakeArguments,
            sharedLibraryExtension, multiConfiguration);
    }
}
