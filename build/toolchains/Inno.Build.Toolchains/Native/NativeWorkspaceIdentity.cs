using System;
using System.IO;
using System.Linq;

namespace Inno.Build.Toolchains;

internal static class NativeWorkspaceIdentity
{
    internal static string Create(
        NativeBuildContext context,
        NativeBuildRecipe recipe,
        NativeInputSnapshot snapshot
    ) {
        string[] tools = (recipe.toolchain ?? context.toolchain)?.inputPaths.Select(Path.GetFullPath).ToArray() ?? [];
        StringComparison comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var configuration = new NativeInputSnapshot(snapshot.entries.Where(entry => tools.Any(tool =>
            string.Equals(entry.physicalPath, tool, comparison)
            || entry.physicalPath.StartsWith(Path.TrimEndingDirectorySeparator(tool) + Path.DirectorySeparatorChar, comparison))));
        var bindings = new NativeBindingInputPaths(context);
        return NativeBuildFingerprint.Create(recipe.declarations.Where(static value =>
            !value.StartsWith("bindings=", StringComparison.Ordinal))
            .Select(bindings.NormalizeDeclaration)
            .Prepend("executionRoot=" + (OperatingSystem.IsWindows() ? ToolchainWorkingDirectory.aliasRoot : context.engineRoot))
            .Prepend(context.configuration).Prepend(recipe.targetId).Prepend("workspace"), configuration);
    }
}
