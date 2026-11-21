using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Core.IO;

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
    public static Dictionary<string, string> CreateEnvironment()
        => AmbientCompilerVariables.ToDictionary(
            static variable => variable,
            static _ => string.Empty,
            StringComparer.OrdinalIgnoreCase
        );

    /// <summary>
    /// Validates and freezes a provider's complete tool selection before product staging,
    /// including CMake data from its physical installation rather than a shared command prefix.
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
    /// <param name="linkPlan">
    /// The sole source of target library formats and linker capabilities.
    /// </param>
    /// <param name="multiConfiguration">
    /// Whether configuration is chosen during build.
    /// </param>
    /// <param name="compilerInputs">
    /// The selected SDK parser inputs, or null for a selection without parsing capability.
    /// </param>
    /// <returns>
    /// An immutable selection with validated physical tool and SDK inputs.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// A selected SDK input or executable is absent.
    /// </exception>
    /// <exception cref="IOException">
    /// A selected CMake executable alias cannot be resolved to its installation.
    /// </exception>
    public static NativeToolchainSelection Freeze(
        string targetId,
        BuildHostDescriptor host,
        IReadOnlyDictionary<string, string> tools,
        IReadOnlyDictionary<string, string> environment,
        IEnumerable<string> inputs,
        IEnumerable<string> cmakeArguments,
        NativeLinkPlan linkPlan,
        bool multiConfiguration,
        NativeCompilerInputs? compilerInputs = null
    ) {
        var complete = new List<string>(inputs);
        if (tools.TryGetValue("cmake", out string? cmake))
        {
            string physicalCMake = new PhysicalPathResolver().Resolve(cmake);
            string modules = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(physicalCMake)!, "..", "share"));
            if (Directory.Exists(modules))
            {
                complete.Add(modules);
            }
        }
        complete.AddRange(tools.Values);
        foreach (string input in complete)
        {
            if (!File.Exists(input) && !Directory.Exists(input))
            {
                throw new FileNotFoundException("A selected native toolchain input is unavailable.", input);
            }
        }
        return new(
            targetId,
            host,
            tools,
            environment,
            complete,
            cmakeArguments,
            linkPlan,
            multiConfiguration,
            compilerInputs
        );
    }
}
