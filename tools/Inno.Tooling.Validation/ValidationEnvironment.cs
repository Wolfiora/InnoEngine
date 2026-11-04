using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Tooling.Validation;

internal static class ValidationEnvironment
{
    internal static async Task<string> CaptureAsync(
        string root,
        string dotnet,
        CancellationToken cancellation,
        IEnumerable<ValidationContext>? contexts = null
    ) {
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveDirectoryAsync(dotnet, root, cancellation).ConfigureAwait(false);
        var inputs = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["os"] = RuntimeInformation.OSDescription,
            ["architecture"] = RuntimeInformation.ProcessArchitecture.ToString(),
            ["sdk"] = sdk.sdkIdentity,
            ["host"] = sdk.hostPath
        };
        foreach (string name in new[] { "MSBuildSDKsPath", "DOTNET_ROLL_FORWARD", "DOTNET_ROOT", "NUGET_PACKAGES", "CC", "CXX", "CMAKE_GENERATOR", "SDKROOT", "MACOSX_DEPLOYMENT_TARGET" })
            inputs.Add(name, Environment.GetEnvironmentVariable(name) ?? string.Empty);
        string sdkRoot = Path.GetDirectoryName(sdk.cliPath)!;
        string hostRoot = Path.GetDirectoryName(sdk.hostPath)!;
        var files = new HashSet<string>(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal)
        {
            sdk.hostPath, sdkRoot, RuntimeEnvironment.GetRuntimeDirectory()
        };
        foreach (string directory in new[] { "shared", "host" })
            if (Directory.Exists(Path.Combine(hostRoot, directory)))
                files.Add(Path.Combine(hostRoot, directory));
        string shell = ToolchainEnvironment.ResolveExecutable("pwsh");
        shell = File.ResolveLinkTarget(shell, returnFinalTarget: true)?.FullName ?? shell;
        files.Add(shell);
        string automation = Path.Combine(Path.GetDirectoryName(shell)!, "System.Management.Automation.dll");
        if (File.Exists(automation))
            files.Add(automation);
        foreach (ValidationContext context in contexts ?? [])
        foreach (JsonElement snapshot in context.sdkGraph.EnumerateArray())
        foreach (JsonElement variant in snapshot.GetProperty("Variants").EnumerateArray().Append(snapshot))
        {
            foreach (JsonElement import in variant.GetProperty("Imports").EnumerateArray())
                IncludeExternal(import.GetString()!);
            foreach (JsonProperty group in variant.GetProperty("Items").EnumerateObject())
            foreach (JsonElement item in group.Value.EnumerateArray())
                if (item.TryGetProperty("FullPath", out JsonElement fullPath) && !string.IsNullOrWhiteSpace(fullPath.GetString()))
                    IncludeExternal(fullPath.GetString()!);
        }
        inputs.Add("tool-content", NativeBuildFingerprint.Create([], files.Order(StringComparer.Ordinal)
            .Select(path => NativeBuildInput.FromPath(root, path)), cancellation));
        return ValidationInputs.Identity(inputs);

        void IncludeExternal(string declared)
        {
            string path = Path.GetFullPath(declared);
            string relative = Path.GetRelativePath(root, path);
            if (!Path.IsPathRooted(relative) && relative != ".." && !relative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                return;
            if (File.Exists(path) || Directory.Exists(path))
                files.Add(path);
            else
                inputs["absent-external:" + path] = "missing";
        }
    }
}
