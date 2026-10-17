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
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "native"), BuildCacheArtifactRole.ImmutableArtifact), "native"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "bindings"), BuildCacheArtifactRole.ImmutableArtifact), "bindings"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "support-packs"), BuildCacheArtifactRole.ImmutableArtifact), "support-packs"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "managed", "workspaces"), BuildCacheArtifactRole.ReusableWorkspace), "managed-workspaces"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "managed-inputs", "evaluators"), BuildCacheArtifactRole.ImmutableArtifact), "managed-evaluators"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "managed-inputs", "requests"), BuildCacheArtifactRole.ProcessWorkingDirectory), "managed-input-requests"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "bindings", "extensions"), BuildCacheArtifactRole.ImmutableArtifact), "binding-extensions"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "bindings", "intermediate"), BuildCacheArtifactRole.ReusableWorkspace), "binding-workspaces"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "hosts"), BuildCacheArtifactRole.ImmutableArtifact), "task-hosts"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "loads"), BuildCacheArtifactRole.ProcessWorkingDirectory), "task-loads"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "publishers"), BuildCacheArtifactRole.ProcessWorkingDirectory), "task-publishers"),
            Describe(new BuildCachePublisher(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks", "workspaces"), BuildCacheArtifactRole.ReusableWorkspace), "task-workspaces")
        };
        NativeComponentDescriptor[] workspaceComponents = components.Append(BrowserWasm32Toolchain.componentDescriptor).ToArray();
        foreach (string project in workspaceComponents.Select(static component => component.toolchainProject).Distinct(StringComparer.Ordinal))
        {
            NativeComponentDescriptor component = workspaceComponents.First(value => value.toolchainProject == project);
            owners.Add(Describe(new BuildCachePublisher(Path.Combine(component.GetToolchainRoot(engineRoot), "obj", "native"), BuildCacheArtifactRole.ReusableWorkspace),
                "workspace:" + project));
        }
        return owners.AsReadOnly();

        BuildCacheOwnerDescriptor Describe(
            BuildCachePublisher publisher,
            string id
        ) {
            BuildCacheOwnerDescriptor owner = publisher.Describe(id);
            BuildCacheOwnerCatalog.Register(engineRoot, owner.directory, id, publisher.role);
            return owner;
        }
    }
}
