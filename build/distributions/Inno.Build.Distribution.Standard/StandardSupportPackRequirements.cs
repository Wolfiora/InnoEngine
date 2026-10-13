using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;

namespace Inno.Build.Distribution.Standard;

internal static class StandardSupportPackRequirements
{
    internal static PlayerSupportPackRequirements CreateDynamic(
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

    internal static PlayerSupportPackRequirements CreateStatic(
        ProductNativeBuildPlan plan,
        BuildTargetId target
    ) {
        string[] archives = plan.steps.SelectMany(step => step.component.staticBuild?.archiveNames
            .Select(name => "PlayerLink/Native/" + step.id + "/" + target.value + "/" + name)
            ?? throw new InvalidOperationException("A static Player component has no declared archive closure.")).ToArray();
        return new(target, [.. ManagedFiles(plan), .. archives], "PlayerLink/Native",
            plan.steps.Select(step => step.id + "/" + target.value).ToArray(), ".a",
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
