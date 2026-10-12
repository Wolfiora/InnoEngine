using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Build.Toolchains;

namespace Inno.Build.Distribution.Standard;

/// <summary>
/// Registers the standard distribution's actual immutable artifact publishers for coordinated cleanup.
/// </summary>
public static class StandardBuildCaches
{
    /// <summary>
    /// Registers exact Native, binding and Support Pack ownership recorded by their publishers.
    /// Unrecorded directories remain outside the cleanup authority.
    /// </summary>
    /// <param name="engineRoot">
    /// The absolute engine checkout containing the artifact roots.
    /// </param>
    /// <param name="components">
    /// The actual registered component owners; shared toolchain roots are registered once.
    /// </param>
    /// <returns>
    /// Immutable owner registrations with current-generation and transitive dependency protection.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The root is blank or relative.
    /// </exception>
    public static IReadOnlyList<BuildCacheOwnerDescriptor> CreateOwners(
        string engineRoot,
        IReadOnlyList<NativeComponentDescriptor> components
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        ArgumentNullException.ThrowIfNull(components);
        if (!Path.IsPathFullyQualified(engineRoot))
            throw new ArgumentException("Cache registration requires an absolute checkout.", nameof(engineRoot));
        var owners = new List<BuildCacheOwnerDescriptor>
        {
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "native")).Describe("native"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "bindings")).Describe("bindings"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "support-packs")).Describe("support-packs"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "managed", "workspaces")).Describe("managed-workspaces"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "managed-inputs", "evaluators")).Describe("managed-evaluators"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "bindings", "extensions")).Describe("binding-extensions"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "bindings", "intermediate")).Describe("binding-workspaces"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "hosts")).Describe("task-hosts"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "loads")).Describe("task-loads"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "publishers")).Describe("task-publishers"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "workspaces")).Describe("task-workspaces")
        };
        foreach (string project in components.Select(static component => component.toolchainProject).Distinct(StringComparer.Ordinal))
        {
            NativeComponentDescriptor component = components.First(value => value.toolchainProject == project);
            owners.Add(new BuildCachePublisher(Path.Combine(component.GetToolchainRoot(engineRoot), "obj", "native"))
                .Describe("workspace:" + project));
        }
        return owners.AsReadOnly();
    }
}
