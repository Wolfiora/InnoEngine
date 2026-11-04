using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

internal static class ValidationInputs
{
    internal static async Task<Dictionary<string, string>> CaptureAsync(
        string root,
        CancellationToken cancellation
    ) {
        string files = await ValidationProcess.RunAsync("git", ["ls-files", "-z", "--cached", "--others", "--exclude-standard"], root, null, cancellation).ConfigureAwait(false);
        string staged = await ValidationProcess.RunAsync("git", ["ls-files", "--stage", "-z"], root, null, cancellation).ConfigureAwait(false);
        var submodules = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string entry in staged.Split('\0', StringSplitOptions.RemoveEmptyEntries).Where(static entry => entry.StartsWith("160000 ", StringComparison.Ordinal)))
        {
            int separator = entry.IndexOf('\t');
            if (separator < 0 || !submodules.TryAdd(entry[(separator + 1)..], entry.Split(' ', 3)[1]))
                throw new InvalidDataException("The submodule index contains an invalid or conflicted identity.");
        }
        var inputs = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string file in files.Split('\0', StringSplitOptions.RemoveEmptyEntries).Distinct().Order(StringComparer.Ordinal))
        {
            cancellation.ThrowIfCancellationRequested();
            string path = Resolve(root, file);
            if (submodules.TryGetValue(file, out string? expectedRevision))
            {
                // Submodules are inputs, not directories whose mutable object stores should be hashed.
                if (!File.Exists(Path.Combine(path, ".git")) && !Directory.Exists(Path.Combine(path, ".git")))
                {
                    inputs.Add(file, "unavailable:" + expectedRevision);
                    continue;
                }
                string revision = await ValidationProcess.RunAsync("git", ["-C", path, "rev-parse", "HEAD"], root, null, cancellation).ConfigureAwait(false);
                string dirty = await ValidationProcess.RunAsync("git", ["-C", path, "status", "--porcelain"], root, null, cancellation).ConfigureAwait(false);
                if (dirty.Length != 0)
                    throw new InvalidDataException($"Submodule '{file}' has uncommitted inputs; commit or restore its explicit owner before certifying evidence.");
                inputs.Add(file, revision.Trim());
            }
            else
                inputs.Add(file, File.Exists(path) ? Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(path))) : "missing");
        }
        return inputs;
    }

    internal static string Identity(IEnumerable<KeyValuePair<string, string>> inputs)
        => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join('\n',
            inputs.OrderBy(static pair => pair.Key, StringComparer.Ordinal).Select(static pair => pair.Key + "\0" + pair.Value)))));

    internal static string PlanIdentity(ValidationPlan plan)
        => Convert.ToHexStringLower(SHA256.HashData(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(plan,
            new System.Text.Json.JsonSerializerOptions { WriteIndented = true })));

    internal static string CheckIdentity(
        ValidationPlan plan,
        ValidationCheck check,
        IReadOnlyDictionary<string, string> inputs
    ) => Identity(check.inputs.SelectMany(path => path.StartsWith('@') ? plan.inputSets[path[1..]] : [path])
        .Distinct(StringComparer.Ordinal).Select(path => new KeyValuePair<string, string>(path, inputs.GetValueOrDefault(path, "missing")))
        .Append(new("environment", plan.environment)).Append(new("configuration", plan.configuration))
        .Append(new("context", check.context is null ? string.Empty : System.Text.Json.JsonSerializer.Serialize(plan.contexts.GetValueOrDefault(check.context)?.properties)))
        .Append(new("definition", System.Text.Json.JsonSerializer.Serialize(check))));

    internal static string Resolve(
        string root,
        string relative
    ) {
        string path = Path.GetFullPath(relative, root);
        if (!path.StartsWith(Path.GetFullPath(root) + Path.DirectorySeparatorChar,
                OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            throw new InvalidDataException($"Validation input escapes the repository: {relative}");
        return PathBoundary.RequireUnlinkedPath(Path.GetFullPath(root), path);
    }
}
