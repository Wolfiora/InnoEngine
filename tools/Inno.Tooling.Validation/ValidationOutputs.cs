using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;

namespace Inno.Tooling.Validation;

internal static class ValidationOutputs
{
    internal static Dictionary<string, string> Capture(
        ValidationPlan plan,
        ValidationCheck check,
        CancellationToken cancellation,
        Dictionary<string, string>? directoryCache = null
    ) {
        var outputs = new Dictionary<string, string>(StringComparer.Ordinal);
        if (check.kind is not ("build" or "test" or "api" or "wiki" or "products" or "support-pack" or "release-bundle") || check.kind == "wiki" && !check.inputs.Any(static input => input.StartsWith('@') && input is not ("@tooling" or "@native-sources")))
            return outputs;
        var directories = new HashSet<string>(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        if (check.kind is "support-pack" or "release-bundle")
        {
            foreach (string name in check.kind == "support-pack" ? new[] { "PlayerSupportPack.zip" }
                         : new[] { "Editor.zip", "PlayerSupportPack.zip", "deliverable-manifest.json" })
            {
                string archive = Path.Combine(plan.artifacts, "deliverables", name);
                if (!File.Exists(archive))
                    throw new InvalidDataException($"Required verified deliverable is absent: {name}");
                using FileStream stream = File.OpenRead(archive);
                outputs["deliverables/" + name] = Convert.ToHexStringLower(SHA256.HashData(stream));
            }
        }
        foreach (string target in ValidationCompiledGraph.Select(plan, check)
            .Select(static instance => instance.GetProperty("TargetPath").GetString()!).Distinct(StringComparer.Ordinal))
        {
            if (string.IsNullOrEmpty(target) || !File.Exists(target))
                throw new InvalidDataException($"Required compiled output is absent for {check.id}: {target}");
            directories.Add(Path.GetDirectoryName(Path.GetFullPath(target))!);
        }
        if (directories.Count == 0)
            throw new InvalidDataException($"No SDK output directory was discovered for {check.id}.");
        foreach (string directory in directories.Order(StringComparer.Ordinal))
        {
            string relativeDirectory = Path.GetRelativePath(plan.artifacts, directory);
            ValidationInputs.Resolve(plan.artifacts, relativeDirectory);
            string key = relativeDirectory.Replace('\\', '/');
            if (directoryCache is not null && directoryCache.TryGetValue(key, out string? existing))
            {
                outputs[key] = existing;
                continue;
            }
            var files = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (string file in EnumerateFiles(directory).Order(StringComparer.Ordinal))
            {
                cancellation.ThrowIfCancellationRequested();
                using FileStream stream = File.OpenRead(file);
                files[Path.GetRelativePath(directory, file).Replace('\\', '/')] = Convert.ToHexStringLower(SHA256.HashData(stream));
            }
            string identity = ValidationInputs.Identity(files);
            outputs[key] = identity;
            if (directoryCache is not null)
                directoryCache[key] = identity;
        }
        return outputs;
    }

    internal static bool Matches(
        ValidationPlan plan,
        ValidationCheck check,
        ValidationResult result,
        CancellationToken cancellation,
        Dictionary<string, string>? directoryCache = null
    ) {
        if (result.outputs is null)
            return false;
        try
        {
            return ValidationInputs.Identity(Capture(plan, check, cancellation, directoryCache)) == ValidationInputs.Identity(result.outputs);
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }
    private static IEnumerable<string> EnumerateFiles(string directory)
    {
        if ((File.GetAttributes(directory) & FileAttributes.ReparsePoint) != 0)
            throw new InvalidDataException($"Output evidence cannot follow a linked directory: {directory}");
        foreach (string entry in Directory.EnumerateFileSystemEntries(directory))
        {
            FileAttributes attributes = File.GetAttributes(entry);
            if ((attributes & FileAttributes.ReparsePoint) != 0)
                throw new InvalidDataException($"Output evidence cannot follow a linked entry: {entry}");
            if ((attributes & FileAttributes.Directory) != 0)
            {
                foreach (string file in EnumerateFiles(entry))
                    yield return file;
            }
            else
                yield return entry;
        }
    }

}
