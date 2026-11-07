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
        Task<string> fileCapture = ValidationProcess.RunAsync("git", ["ls-files", "-z", "--cached", "--others", "--exclude-standard"], root, null, cancellation);
        Task<string> stageCapture = ValidationProcess.RunAsync("git", ["ls-files", "--stage", "-z"], root, null, cancellation);
        await Task.WhenAll(fileCapture, stageCapture).ConfigureAwait(false);
        string files = await fileCapture.ConfigureAwait(false);
        string staged = await stageCapture.ConfigureAwait(false);
        var submodules = new Dictionary<string, string>(StringComparer.Ordinal);
        var links = new HashSet<string>(StringComparer.Ordinal);
        foreach (string entry in staged.Split('\0', StringSplitOptions.RemoveEmptyEntries).Where(static entry => entry.StartsWith("120000 ", StringComparison.Ordinal)))
            links.Add(entry[(entry.IndexOf('\t') + 1)..]);
        foreach (string entry in staged.Split('\0', StringSplitOptions.RemoveEmptyEntries).Where(static entry => entry.StartsWith("160000 ", StringComparison.Ordinal)))
        {
            int separator = entry.IndexOf('\t');
            if (separator < 0 || !submodules.TryAdd(entry[(separator + 1)..], entry.Split(' ', 3)[1]))
                throw new InvalidDataException("The submodule index contains an invalid or conflicted identity.");
        }
        string[] paths = files.Split('\0', StringSplitOptions.RemoveEmptyEntries).Distinct().Order(StringComparer.Ordinal).ToArray();
        var identities = new string[paths.Length];
        // Independent owners may read concurrently, but every scan still reads current bytes and drains all workers.
        await Parallel.ForAsync(0, paths.Length, new ParallelOptions
        {
            MaxDegreeOfParallelism = Math.Min(Environment.ProcessorCount, 4), CancellationToken = cancellation
        }, async (
            index,
            token
        ) =>
        {
            token.ThrowIfCancellationRequested();
            string file = paths[index];
            if (links.Contains(file))
            {
                byte[]? link = ReadLink(root, file);
                identities[index] = link is null ? "missing" : Convert.ToHexStringLower(SHA256.HashData(link));
                return;
            }
            string path = Resolve(root, file);
            if (submodules.TryGetValue(file, out string? expectedRevision))
                identities[index] = await CaptureSubmoduleAsync(path, expectedRevision, token).ConfigureAwait(false);
            else
                identities[index] = File.Exists(path) ? Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(path))) : "missing";
        }).ConfigureAwait(false);
        return paths.Select((
            path,
            index
        ) => new KeyValuePair<string, string>(path, identities[index]))
            .ToDictionary(StringComparer.Ordinal);
    }

    internal static async Task<string[]> FindContentChangesAsync(
        string root,
        string commit,
        IReadOnlyDictionary<string, string> inputs,
        CancellationToken cancellation
    ) {
        string tree = await ValidationProcess.RunAsync("git", ["ls-tree", "-r", "-z", commit], root, null, cancellation).ConfigureAwait(false);
        var changes = new HashSet<string>(StringComparer.Ordinal);
        var blobs = new List<KeyValuePair<string, string>>();
        foreach (string entry in tree.Split('\0', StringSplitOptions.RemoveEmptyEntries))
        {
            int separator = entry.IndexOf('\t');
            if (separator < 0)
                throw new InvalidDataException("The committed input tree contains an invalid entry.");
            string[] metadata = entry[..separator].Split(' ');
            if (metadata.Length != 3)
                throw new InvalidDataException("The committed input tree contains invalid object metadata.");
            string path = entry[(separator + 1)..];
            if (metadata[1] == "commit")
            {
                string current = inputs.TryGetValue(path, out string? captured) ? captured
                    : await CaptureSubmoduleAsync(Resolve(root, path), metadata[2], cancellation).ConfigureAwait(false);
                if (current != metadata[2] && current != "unavailable:" + metadata[2])
                    changes.Add(path);
            }
            else if (metadata[1] != "blob")
                throw new InvalidDataException($"Unsupported committed input type: {metadata[1]}");
            else if (metadata[0] == "120000")
            {
                byte[]? link = ReadLink(root, path);
                if (link is null)
                    changes.Add(path);
                else
                {
                    string identity = await ValidationProcess.RunAsync("git", ["hash-object", "--stdin", "--no-filters"], root, null, cancellation,
                        new UTF8Encoding(false, true).GetString(link)).ConfigureAwait(false);
                    if (identity.Trim() != metadata[2])
                        changes.Add(path);
                }
            }
            else if (!File.Exists(Resolve(root, path)))
                changes.Add(path);
            else
                blobs.Add(new(path, metadata[2]));
        }
        if (blobs.Count != 0)
        {
            // Read actual bytes through Git's clean filters; diff alone can reuse stale size/time index metadata.
            string content = await ValidationProcess.RunAsync("git", ["hash-object", "--stdin-paths"], root, null, cancellation,
                string.Join('\n', blobs.Select(static pair => QuoteGitPath(pair.Key))) + "\n").ConfigureAwait(false);
            string[] identities = content.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            if (identities.Length != blobs.Count)
                throw new InvalidDataException("Git did not return one content identity for every committed input.");
            KeyValuePair<string, string>[] different = blobs.Where((
                pair,
                index
            ) => identities[index] != pair.Value).ToArray();
            if (different.Length != 0)
            {
                // A committed CRLF blob can already equal its checkout, even when today's clean filter normalizes it.
                string raw = await ValidationProcess.RunAsync("git", ["hash-object", "--no-filters", "--stdin-paths"], root, null, cancellation,
                    string.Join('\n', different.Select(static pair => QuoteGitPath(pair.Key))) + "\n").ConfigureAwait(false);
                string[] rawIdentities = raw.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
                if (rawIdentities.Length != different.Length)
                    throw new InvalidDataException("Git did not return one raw identity for every differing input.");
                for (int index = 0; index < different.Length; index++)
                    if (rawIdentities[index] != different[index].Value)
                        changes.Add(different[index].Key);
            }
        }
        return changes.Order(StringComparer.Ordinal).ToArray();
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

    private static async Task<string> CaptureSubmoduleAsync(
        string path,
        string expectedRevision,
        CancellationToken cancellation
    ) {
        // Check checkout contents, never the mutable Git object store or its size/time index cache.
        if (!File.Exists(Path.Combine(path, ".git")) && !Directory.Exists(Path.Combine(path, ".git")))
            return "unavailable:" + expectedRevision;
        string revision = (await ValidationProcess.RunAsync("git", ["rev-parse", "HEAD"], path, null, cancellation).ConfigureAwait(false)).Trim();
        string dirty = await ValidationProcess.RunAsync("git", ["status", "--porcelain"], path, null, cancellation).ConfigureAwait(false);
        if (dirty.Length != 0 || (await FindContentChangesAsync(path, revision, new Dictionary<string, string>(), cancellation).ConfigureAwait(false)).Length != 0)
            throw new InvalidDataException($"Submodule '{path}' has uncommitted inputs; commit or restore its explicit owner before certifying evidence.");
        return revision;
    }

    private static string QuoteGitPath(string path)
    {
        var quoted = new StringBuilder("\"");
        foreach (char character in path)
            if (character is '\\' or '"')
                quoted.Append('\\').Append(character);
            else if (character < ' ' || character == '\u007f')
                quoted.Append('\\').Append(Convert.ToString(character, 8).PadLeft(3, '0'));
            else
                quoted.Append(character);
        return quoted.Append('"').ToString();
    }

    private static byte[]? ReadLink(
        string root,
        string relative
    ) {
        // Git stores the link text, not the target's bytes. Never traverse the final entry.
        string path = PathBoundary.Resolve(root, relative);
        PathBoundary.RequireUnlinkedPath(root, Path.GetDirectoryName(path)!);
        string? target = new FileInfo(path).LinkTarget;
        if (target is not null)
            // Git for Windows stores slash-separated link text but creates native backslash-separated targets.
            return Encoding.UTF8.GetBytes(OperatingSystem.IsWindows() ? target.Replace('\\', '/') : target);
        PathBoundary.RequireUnlinkedPath(root, path);
        return File.Exists(path) ? File.ReadAllBytes(path) : null;
    }
}
