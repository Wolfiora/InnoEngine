using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;

namespace Inno.Build.Products.Standard;

/// <summary>
/// Composes validated standard product Support Pack requirements.
/// </summary>
public static class StandardSupportPackRequirements
{
    /// <summary>
    /// Declares the complete standard Player inputs for the selected linkage.
    /// </summary>
    /// <param name="plan">
    /// The platform-configured component graph.
    /// </param>
    /// <param name="target">
    /// The exact platform target.
    /// </param>
    /// <param name="extension">
    /// The platform runtime-library suffix.
    /// </param>
    /// <returns>
    /// Requirements matching the plan's actual component closure.
    /// </returns>
    public static PlayerSupportPackRequirements CreateDynamic(
        ProductNativeBuildPlan plan,
        BuildTargetId target,
        string extension
    ) {
        string[] groups = plan.steps.Select(step => step.deploymentPath("selected" + extension, target.value)
            ?? throw new InvalidOperationException("A Player component has no runtime deployment policy."))
            .Select(path => Path.GetDirectoryName(path)?.Replace('\\', '/')
                ?? throw new InvalidOperationException("A Player component must declare an owned deployment group."))
            .ToArray();
        return new(target, ManagedFiles(plan), "native", groups, extension);
    }

    /// <summary>
    /// Declares the complete standard Player inputs for the selected linkage.
    /// </summary>
    /// <param name="plan">
    /// The platform-configured component graph.
    /// </param>
    /// <param name="target">
    /// The exact platform target.
    /// </param>
    /// <returns>
    /// Requirements matching the plan's actual component closure.
    /// </returns>
    /// <param name="linkPlan">
    /// The explicit platform archive naming plan.
    /// </param>
    public static PlayerSupportPackRequirements CreateStatic(
        ProductNativeBuildPlan plan,
        BuildTargetId target,
        NativeLinkPlan linkPlan
    ) {
        string[] archives = plan.steps.SelectMany(step => step.component.staticBuild?.logicalOutputs
            .Select(name => "PlayerLink/Native/" + step.id + "/" + target.value + "/" + linkPlan.GetStaticFileName(name))
            ?? throw new InvalidOperationException("A static Player component has no declared archive closure.")).ToArray();
        return new(target, [.. ManagedFiles(plan), .. archives], "PlayerLink/Native",
            plan.steps.Select(step => step.id + "/" + target.value).ToArray(), linkPlan.staticLibraryExtension!,
            archives.Select(path => path["PlayerLink/Native/".Length..]).ToArray());
    }

    private static string[] ManagedFiles(ProductNativeBuildPlan plan)
        => plan.steps.Where(static step => step.component.bindingDefinition is not null)
            .SelectMany(static step => new[]
            {
                "References/" + Path.GetFileNameWithoutExtension(step.component.nativeProject) + ".dll",
                "PlayerLink/References/" + Path.GetFileNameWithoutExtension(step.component.nativeProject) + ".dll"
            }).Append("PlayerLink/References/BGCS.Runtime.dll").ToArray();
}
