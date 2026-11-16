using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Tooling.Validation;

internal static class ValidationEnvironment
{
    internal sealed record ToolSnapshot(
        DotNetSdkDescriptor sdk,
        Dictionary<string, string> inputs,
        Dictionary<string, string> contents
    );

    internal static async Task<ToolSnapshot> CaptureToolsAsync(
        string root,
        string dotnet,
        CancellationToken cancellation
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
        // The CLI output directory can also contain Editor, backend and documentation assemblies.
        // Only this tool's actual assembly-reference closure is an execution-environment input.
        var pending = new Queue<Assembly>();
        var examined = new HashSet<Assembly>();
        pending.Enqueue(typeof(ValidationCommand).Assembly);
        while (pending.TryDequeue(out Assembly? assembly))
        {
            cancellation.ThrowIfCancellationRequested();
            if (!examined.Add(assembly))
                continue;
            if (assembly.IsDynamic || string.IsNullOrWhiteSpace(assembly.Location))
                throw new InvalidDataException("Validation requires file-backed tool dependencies: " + assembly.FullName);
            files.Add(assembly.Location);
            AssemblyLoadContext context = AssemblyLoadContext.GetLoadContext(assembly)
                ?? throw new InvalidDataException("Validation dependency has no loading context: " + assembly.FullName);
            foreach (AssemblyName dependency in assembly.GetReferencedAssemblies())
                pending.Enqueue(context.LoadFromAssemblyName(dependency));
        }
        foreach (string directory in new[] { "shared", "host" })
            if (Directory.Exists(Path.Combine(hostRoot, directory)))
                files.Add(Path.Combine(hostRoot, directory));
        string shell = ToolchainEnvironment.ResolveExecutable("pwsh");
        shell = File.ResolveLinkTarget(shell, returnFinalTarget: true)?.FullName ?? shell;
        files.Add(shell);
        string automation = Path.Combine(Path.GetDirectoryName(shell)!, "System.Management.Automation.dll");
        if (File.Exists(automation))
            files.Add(automation);
        return new(sdk, inputs, CaptureContents(root, files, cancellation));
    }

    internal static async Task<string> CaptureAsync(
        string root,
        string dotnet,
        CancellationToken cancellation,
        IEnumerable<ValidationContext>? contexts = null,
        ToolSnapshot? tools = null
    ) {
        tools ??= await CaptureToolsAsync(root, dotnet, cancellation).ConfigureAwait(false);
        var inputs = new Dictionary<string, string>(tools.inputs, StringComparer.Ordinal);
        foreach (var content in tools.contents)
            inputs.Add("tool-content:" + content.Key, content.Value);
        var files = new HashSet<string>(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        var examined = new HashSet<string>(files.Comparer);
        foreach (ValidationContext context in contexts ?? [])
        foreach (JsonElement snapshot in context.sdkGraph.EnumerateArray())
        foreach (JsonElement variant in snapshot.GetProperty("Variants").EnumerateArray().Append(snapshot))
        {
            foreach (JsonElement pack in variant.GetProperty("FrameworkPacks").EnumerateArray())
                IncludeExternal(pack.GetString()!);
            foreach (JsonElement import in variant.GetProperty("Imports").EnumerateArray())
                IncludeExternal(import.GetString()!);
            foreach (JsonProperty group in variant.GetProperty("Items").EnumerateObject())
            foreach (JsonElement item in group.Value.EnumerateArray())
                if (item.TryGetProperty("FullPath", out JsonElement fullPath) && !string.IsNullOrWhiteSpace(fullPath.GetString()))
                    IncludeExternal(fullPath.GetString()!);
        }
        foreach (var content in CaptureContents(root, files, cancellation))
            inputs.Add("external-content:" + content.Key, content.Value);
        return ValidationInputs.Identity(inputs);

        void IncludeExternal(string declared)
        {
            string path = Path.GetFullPath(declared);
            if (!examined.Add(path))
                return;
            string relative = Path.GetRelativePath(root, path);
            if (!Path.IsPathRooted(relative) && relative != ".." && !relative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                return;
            if (tools.contents.Keys.Any(parent => Covers(parent, path)))
                return;
            if (File.Exists(path) || Directory.Exists(path))
                files.Add(path);
            else
                inputs["absent-external:" + path] = "missing";
        }
    }

    private static Dictionary<string, string> CaptureContents(
        string root,
        IEnumerable<string> files,
        CancellationToken cancellation
    ) {
        var roots = new List<string>();
        foreach (string path in files.Select(Path.GetFullPath).OrderBy(static path => path.Length).ThenBy(static path => path, StringComparer.Ordinal))
            if (!roots.Any(parent => Covers(parent, path)))
                roots.Add(path);
        var identities = new string[roots.Count];
        // Each worker owns its input snapshot. All bytes and inventories are read again at the closing freshness check.
        Parallel.For(0, roots.Count, new ParallelOptions { MaxDegreeOfParallelism = Math.Min(Environment.ProcessorCount, 4), CancellationToken = cancellation },
            index => identities[index] = NativeBuildFingerprint.Create([], [NativeBuildInput.FromPath(root, roots[index])], cancellation));
        var contents = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 0; index < roots.Count; index++)
            contents.Add(roots[index], identities[index]);
        return contents;
    }

    private static bool Covers(
        string parent,
        string path
    ) {
        StringComparison comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return path.Equals(parent, comparison) || Directory.Exists(parent)
            && path.StartsWith(parent.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, comparison);
    }
}
