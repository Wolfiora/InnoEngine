using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Build.Toolchains;
using Inno.Build.BrowserWasm32;

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
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "entry", "workers"), BuildCacheArtifactRole.ProcessWorkingDirectory).Describe("entry-workers"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "native"), BuildCacheArtifactRole.ImmutableArtifact).Describe("native"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "bindings"), BuildCacheArtifactRole.ImmutableArtifact).Describe("bindings"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "support-packs"), BuildCacheArtifactRole.ImmutableArtifact).Describe("support-packs"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "managed", "workspaces"), BuildCacheArtifactRole.ReusableWorkspace).Describe("managed-workspaces"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "managed-inputs", "evaluators"), BuildCacheArtifactRole.ImmutableArtifact).Describe("managed-evaluators"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "managed-inputs", "requests"), BuildCacheArtifactRole.ProcessWorkingDirectory).Describe("managed-input-requests"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "bindings", "extensions"), BuildCacheArtifactRole.ImmutableArtifact).Describe("binding-extensions"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "bindings", "intermediate"), BuildCacheArtifactRole.ReusableWorkspace).Describe("binding-workspaces"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "cli", "hosts"), BuildCacheArtifactRole.ImmutableArtifact).Describe("build-tool-hosts"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "cli", "readers"), BuildCacheArtifactRole.ProcessWorkingDirectory).Describe("build-tool-readers"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "cli", "workspaces"), BuildCacheArtifactRole.ReusableWorkspace).Describe("build-tool-workspaces"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "hosts"), BuildCacheArtifactRole.ImmutableArtifact).Describe("task-hosts"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "loads"), BuildCacheArtifactRole.ProcessWorkingDirectory).Describe("task-loads"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "publishers"), BuildCacheArtifactRole.ProcessWorkingDirectory).Describe("task-publishers"),
            new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "workspaces"), BuildCacheArtifactRole.ReusableWorkspace).Describe("task-workspaces")
        };
        NativeComponentDescriptor[] workspaceComponents = components.Append(BrowserWasm32Toolchain.componentDescriptor).ToArray();
        foreach (string project in workspaceComponents.Select(static component => component.toolchainProject).Distinct(StringComparer.Ordinal))
        {
            NativeComponentDescriptor component = workspaceComponents.First(value => value.toolchainProject == project);
            owners.Add(new BuildCachePublisher(Path.Combine(component.GetToolchainRoot(engineRoot), "obj", "native"), BuildCacheArtifactRole.ReusableWorkspace)
                .Describe("workspace:" + project));
        }
        return owners.AsReadOnly();
    }
}
