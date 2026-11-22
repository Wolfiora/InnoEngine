using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

internal static class NativeDependencySource
{
    internal static async Task<Dictionary<string, string>> CaptureAsync(
        string root,
        IEnumerable<string> paths,
        CancellationToken cancellationToken
    ) {
        string index = await GitAsync(["ls-files", "--stage", "-z"]).ConfigureAwait(false);
        string[] modules = index
            .Split('\0', StringSplitOptions.RemoveEmptyEntries)
            .Where(static item => item.StartsWith("160000 ", StringComparison.Ordinal))
            .Select(static item => item[(item.IndexOf('\t') + 1)..])
            .ToArray();
        string[] roots = paths
            .Select(
                path => modules.FirstOrDefault(module => path == module || path.StartsWith(module + "/", StringComparison.Ordinal)) ?? path
            )
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal)
            .ToArray();
        if (roots.Length == 0)
        {
            throw new InvalidDataException("A native dependency must declare tracked source inputs.");
        }
        foreach (string path in roots)
        {
            _ = PathBoundary.Resolve(root, path);
        }
        string dirty = await GitAsync(
            ["status", "--porcelain", "--untracked-files=all", "--ignore-submodules=none", "--", ..roots]
        ).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(dirty))
        {
            throw new InvalidDataException("Native dependency sources must match their committed input: " + dirty);
        }
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string[] batch in roots.Chunk(80))
        {
            string tree = await GitAsync(["ls-tree", "-t", "-z", "HEAD", "--", ..batch]).ConfigureAwait(false);
            Dictionary<string, string> entries = tree
                .Split('\0', StringSplitOptions.RemoveEmptyEntries)
                .ToDictionary(entry => entry[(entry.IndexOf('\t') + 1)..], StringComparer.Ordinal);
            foreach (string path in batch)
            {
                if (!entries.TryGetValue(path, out string? entry))
                {
                    throw new InvalidDataException("Native dependency input has no committed owner: " + path);
                }
                result.Add(path, NativeBuildFingerprint.Create([entry], []));
            }
        }

        await VerifyWorkingTreeAsync(root, "HEAD", roots, cancellationToken).ConfigureAwait(false);
        return result;
        Task<string> GitAsync(string[] arguments)
            => ToolchainEnvironment.CaptureOutputAsync("git", ["--literal-pathspecs", ..arguments], root, cancellationToken);
    }

    private static async Task VerifyWorkingTreeAsync(
        string root,
        string revision,
        string[] paths,
        CancellationToken cancellationToken
    ) {
        var blobs = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string[] batch in paths.Chunk(80))
        {
            string tree = await GitAsync(["ls-tree", "-r", "-z", revision, "--", ..batch]).ConfigureAwait(false);
            foreach (string entry in tree.Split('\0', StringSplitOptions.RemoveEmptyEntries))
            {
                if (string.IsNullOrWhiteSpace(entry))
                {
                    continue;
                }
                int separator = entry.IndexOf('\t');
                if (separator < 0)
                {
                    throw new InvalidDataException("Native source tree returned invalid object metadata.");
                }
                string[] metadata = entry[..separator].Split(' ');
                if (metadata.Length != 3)
                {
                    throw new InvalidDataException("Native source tree returned incomplete object metadata.");
                }
                string relative = entry[(separator + 1)..];
                string path = PathBoundary.Resolve(root, relative);
                PathBoundary.RequireUnlinkedPath(root, Path.GetDirectoryName(path)!);
                if (metadata[1] == "commit")
                {
                    PathBoundary.RequireUnlinkedPath(root, path);
                    // A consumer does not need a third-party checkout. An initialized checkout is mutable and must be checked.
                    if (File.Exists(Path.Combine(path, ".git")) || Directory.Exists(Path.Combine(path, ".git")))
                    {
                        string head = await ToolchainEnvironment.CaptureOutputAsync(
                            "git",
                            ["rev-parse", "HEAD"],
                            path,
                            cancellationToken
                        ).ConfigureAwait(false);
                        if (head.Trim() != metadata[2])
                        {
                            throw new InvalidDataException("Native submodule revision differs from its committed owner: " + relative);
                        }
                        await VerifyWorkingTreeAsync(path, metadata[2], ["."], cancellationToken).ConfigureAwait(false);
                    }
                }
                else if (metadata[0] == "120000")
                {
                    string? target = new FileInfo(path).LinkTarget;
                    byte[] bytes = target is null ? File.ReadAllBytes(path) : Encoding.UTF8.GetBytes(target.Replace('\\', '/'));
                    byte[] prefix = Encoding.ASCII.GetBytes("blob " + bytes.Length + "\0");
                    byte[] content = [..prefix, ..bytes];
                    string identity = Convert.ToHexStringLower(
                        metadata[2].Length == 40 ? SHA1.HashData(content) : SHA256.HashData(content)
                    );
                    if (identity != metadata[2])
                    {
                        throw new InvalidDataException("Native source link changed: " + relative);
                    }
                }
                else
                {
                    PathBoundary.RequireUnlinkedPath(root, path);
                    if (!File.Exists(path))
                    {
                        throw new InvalidDataException("Native source is missing: " + relative);
                    }
                    blobs[relative] = metadata[2];
                }
            }
        }
        foreach (KeyValuePair<string, string>[] batch in blobs.ToArray().Chunk(80))
        {
            // Read actual bytes through clean filters; index timestamps and assume-unchanged flags are not content evidence.
            string[] hashes = (await GitAsync(["hash-object", "--", ..batch.Select(static pair => pair.Key)]).ConfigureAwait(false))
                .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            if (hashes.Length != batch.Length)
            {
                throw new InvalidDataException("Native source hashing returned an incomplete input inventory.");
            }
            KeyValuePair<string, string>[] changed = batch.Where((
                pair,
                index
            ) => hashes[index] != pair.Value).ToArray();
            if (changed.Length == 0)
            {
                continue;
            }
            // Existing CRLF blobs can already match their checkout even when the current clean filter normalizes them.
            string[] raw = (await GitAsync(
                ["hash-object", "--no-filters", "--", ..changed.Select(static pair => pair.Key)]
            ).ConfigureAwait(false))
                .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            if (raw.Length != changed.Length || changed.Where((
                pair,
                index
            ) => raw[index] != pair.Value).Any()
            ) {
                throw new InvalidDataException(
                    "Native dependency source bytes changed. Produce the matching backend package for the requested target."
                );
            }
        }
        Task<string> GitAsync(string[] arguments)
            => ToolchainEnvironment.CaptureOutputAsync("git", ["--literal-pathspecs", ..arguments], root, cancellationToken);
    }
}
