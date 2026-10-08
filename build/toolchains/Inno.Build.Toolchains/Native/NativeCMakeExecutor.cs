using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Executes component-owned CMake definitions using an already frozen target SDK selection.
/// </summary>
public static class NativeCMakeExecutor
{
    /// <summary>
    /// Configures and builds an explicit component target without detecting the current platform.
    /// </summary>
    /// <param name="context">
    /// The operation owning tools, environment and target identity.
    /// </param>
    /// <param name="component">
    /// The unique component owner of target-scoped intermediate output.
    /// </param>
    /// <param name="sourceDirectory">
    /// The component-owned CMake source directory.
    /// </param>
    /// <param name="target">
    /// The declared CMake build target, or null for the complete selected project.
    /// </param>
    /// <param name="componentArguments">
    /// Semantic component options; SDK and generator arguments come from the selection.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels and drains the complete tool process tree.
    /// </param>
    /// <returns>
    /// The owned CMake build directory after successful compilation.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Tools are unassigned or configuration or compilation fails.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Product definitions conflict with a semantic definition owned by the component recipe.
    /// </exception>
    public static async Task<string> BuildAsync(
        NativeBuildContext context,
        NativeComponentDescriptor component,
        string sourceDirectory,
        string? target,
        IReadOnlyList<string> componentArguments,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(componentArguments);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceDirectory);
        if (target is not null)
            ArgumentException.ThrowIfNullOrWhiteSpace(target);
        NativeToolchainSelection selection = context.RequireToolchain();
        NativeComponentBuildOptions options = context.RequireComponentOptions(component);
        var reserved = new HashSet<string>(StringComparer.Ordinal);
        foreach (string argument in componentArguments)
        {
            if (argument.StartsWith("-D", StringComparison.Ordinal) && argument.Contains('='))
                reserved.Add(argument[2..argument.IndexOf('=')].Split(':')[0]);
        }
        foreach (string argument in options.cmakeArguments)
        {
            if (reserved.Contains(argument[2..argument.IndexOf('=')].Split(':')[0]))
                throw new ArgumentException("Product configuration cannot override a recipe-owned component definition.", nameof(componentArguments));
        }
        string buildType = context.configuration == "debug" ? "Debug" : "Release";
        string buildDirectory = Path.Combine(context.GetNativeBuildRoot(component), "CMake");
        List<string> configure = ["-S", sourceDirectory, "-B", buildDirectory];
        configure.AddRange(selection.cmakeArguments);
        configure.Add("-DINNO_ROOT=" + context.engineRoot);
        configure.Add("-DINNO_NATIVE_TARGET=" + selection.targetId);
        if (!selection.multiConfiguration)
            configure.Add("-DCMAKE_BUILD_TYPE=" + buildType);
        configure.Add("-DINNO_LIBRARY_KIND=" + options.libraryKind.ToString().ToUpperInvariant());
        configure.AddRange(componentArguments);
        configure.AddRange(options.cmakeArguments);
        await ToolchainEnvironment.RunAsync(context, "cmake", configure,
            context.engineRoot, cancellationToken).ConfigureAwait(false);
        List<string> build = ["--build", buildDirectory, "--config", buildType];
        if (target is not null)
            build.AddRange(["--target", target]);
        await ToolchainEnvironment.RunAsync(context, "cmake", build,
            context.engineRoot, cancellationToken).ConfigureAwait(false);
        return buildDirectory;
    }

    /// <summary>
    /// Selects one declared output while excluding CMake's compiler probes and other build configurations.
    /// </summary>
    /// <param name="context">
    /// The selected configuration and target generator policy.
    /// </param>
    /// <param name="buildDirectory">
    /// The component-owned CMake intermediate directory.
    /// </param>
    /// <param name="filePattern">
    /// The component's exact basename or bounded filename pattern.
    /// </param>
    /// <returns>
    /// The sole matching output, or an explicit failure for a missing or ambiguous result.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// The component did not produce exactly one expected output.
    /// </exception>
    public static string FindOutput(
        NativeBuildContext context,
        string buildDirectory,
        string filePattern
    ) {
        NativeToolchainSelection selection = context.RequireToolchain();
        string buildType = context.configuration == "debug" ? "Debug" : "Release";
        string[] candidates = Directory.EnumerateFiles(buildDirectory, filePattern, SearchOption.AllDirectories)
            .Where(path => !Path.GetRelativePath(buildDirectory, path).Split(Path.DirectorySeparatorChar).Contains("CMakeFiles"))
            .Where(path => !selection.multiConfiguration
                || Path.GetRelativePath(buildDirectory, path).Split(Path.DirectorySeparatorChar).Contains(buildType))
            .ToArray();
        return candidates.Length == 1 ? candidates[0]
            : throw new FileNotFoundException($"Expected one native output '{filePattern}' but found {candidates.Length}.");
    }
}
