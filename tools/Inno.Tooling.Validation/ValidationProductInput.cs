using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

internal static class ValidationProductInput
{
    internal static async Task PrepareAsync(
        ValidationPlan plan,
        string directory,
        string project,
        CancellationToken cancellation
    ) {
        using JsonDocument document = JsonDocument.Parse(await File.ReadAllTextAsync(
            Path.Combine(plan.root, "tests", "fixtures", "editor-ui", "consumer.json"), cancellation).ConfigureAwait(false));
        string repository = document.RootElement.GetProperty("repository").GetString()!;
        string commit = document.RootElement.GetProperty("commit").GetString()!;
        string prefix = document.RootElement.GetProperty("project").GetString()!.TrimEnd('/') + "/";
        if (!Uri.TryCreate(repository, UriKind.Absolute, out Uri? source) || source.Scheme != "https"
            || source.Host != "github.com" || !System.Text.RegularExpressions.Regex.IsMatch(commit, "\\A[0-9a-f]{40}\\z"))
            throw new InvalidDataException("The product consumer must declare an exact HTTPS repository revision.");
        if (prefix.StartsWith('/') || prefix.Contains('\\') || Array.Exists(prefix.TrimEnd('/').Split('/'),
                static segment => segment.Length == 0 || segment is "." or ".."))
            throw new InvalidDataException("The consumer project must declare a relative repository directory.");
        // The validation workspace already owns the exclusive lease. Logs never determine dependency-cache identity.
        string identity = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(repository + "\n" + commit)));
        string checkout = PathBoundary.RequireUnlinkedPath(plan.artifacts, Path.Combine(plan.artifacts, "consumer-inputs", identity));
        if (Directory.Exists(checkout))
        {
            string origin = await ValidationProcess.RunAsync("git", ["-C", checkout, "remote", "get-url", "origin"], plan.root, null, cancellation).ConfigureAwait(false);
            string bare = await ValidationProcess.RunAsync("git", ["-C", checkout, "rev-parse", "--is-bare-repository"], plan.root, null, cancellation).ConfigureAwait(false);
            if (origin.Trim() != repository || bare.Trim() != "true")
                throw new InvalidDataException("The consumer dependency cache has a different repository or storage owner.");
            try
            {
                await ValidationProcess.RunAsync("git", ["-C", checkout, "fsck", "--full", "--strict"], plan.root,
                    Path.Combine(directory, "consumer-integrity.log"), cancellation).ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                string rejected = PathBoundary.RequireUnlinkedPath(plan.artifacts, checkout + ".rejected-" + Guid.NewGuid().ToString("N"));
                Directory.Move(checkout, rejected);
                Console.WriteLine("Rejected corrupt consumer dependency cache; retained diagnostic and quarantined bytes at " + rejected);
            }
        }
        if (!Directory.Exists(checkout))
        {
            string staging = PathBoundary.RequireUnlinkedPath(plan.artifacts, checkout + ".staging-" + Guid.NewGuid().ToString("N"));
            await ValidationProcess.RunAsync("git", ["-c", "core.longpaths=true", "init", "--bare", staging], plan.root,
                Path.Combine(directory, "consumer-initialize.log"), cancellation).ConfigureAwait(false);
            await ValidationProcess.RunAsync("git", ["-c", "core.longpaths=true", "-C", staging, "config", "core.longpaths", "true"],
                plan.root, null, cancellation).ConfigureAwait(false);
            await ValidationProcess.RunAsync("git", ["-C", staging, "remote", "add", "origin", repository], plan.root, null, cancellation).ConfigureAwait(false);
            cancellation.ThrowIfCancellationRequested();
            Directory.Move(staging, checkout);
        }
        if (!await HasCommitAsync(checkout).ConfigureAwait(false))
        {
            string input = repository;
            string adjacent = Path.Combine(Path.GetDirectoryName(plan.root)!, Path.GetFileNameWithoutExtension(source.AbsolutePath));
            if (Directory.Exists(Path.Combine(adjacent, ".git")) || File.Exists(Path.Combine(adjacent, ".git")))
            {
                string origin = await ValidationProcess.RunAsync("git", ["-C", adjacent, "remote", "get-url", "origin"], plan.root, null, cancellation).ConfigureAwait(false);
                if (origin.Trim().Equals(repository, StringComparison.OrdinalIgnoreCase) && await HasCommitAsync(adjacent).ConfigureAwait(false))
                    input = adjacent;
            }
            Console.WriteLine($"Importing exact consumer commit {commit} from {input}; no consumer working-tree files are read.");
            await File.WriteAllTextAsync(Path.Combine(directory, "consumer-source.json"),
                JsonSerializer.Serialize(new { repository, commit, source = input }), cancellation).ConfigureAwait(false);
            await ValidationProcess.RunAsync("git", ["-C", checkout, "fetch", "--no-tags", "--depth=1", input, commit], plan.root,
                Path.Combine(directory, "consumer-fetch.log"), cancellation).ConfigureAwait(false);
            await ValidationProcess.RunAsync("git", ["-C", checkout, "fsck", "--full", "--strict"], plan.root,
                Path.Combine(directory, "consumer-import-integrity.log"), cancellation).ConfigureAwait(false);
        }
        string revision = await ValidationProcess.RunAsync("git", ["-C", checkout, "rev-parse", commit + "^{commit}"], plan.root, null, cancellation).ConfigureAwait(false);
        if (revision.Trim() != commit)
            throw new InvalidDataException("The consumer object does not match its declared revision.");
        string archive = Path.Combine(directory, "consumer-source.zip");
        await ValidationProcess.RunAsync("git", ["-C", checkout, "archive", "--format=zip", "--output=" + archive, commit, "--", prefix],
            plan.root, Path.Combine(directory, "consumer-archive.log"), cancellation).ConfigureAwait(false);
        using ZipArchive contents = ZipFile.OpenRead(archive);
        foreach (ZipArchiveEntry entry in contents.Entries)
        {
            cancellation.ThrowIfCancellationRequested();
            string file = entry.FullName;
            if (!file.StartsWith(prefix, StringComparison.Ordinal) || file.Contains('\\') || ((entry.ExternalAttributes >> 16) & 0xF000) == 0xA000)
                throw new InvalidDataException("The consumer project selection escaped its declared prefix.");
            if (file.EndsWith('/'))
                continue;
            string destination = PathBoundary.RequireUnlinkedPath(project, ValidationInputs.Resolve(project, file[prefix.Length..]));
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            entry.ExtractToFile(destination, overwrite: false);
        }
        if (!File.Exists(Path.Combine(project, "Assets", "FlappyBird", "FlappyBird.iscene")))
            throw new InvalidDataException("The declared product consumer does not supply the required real scene.");

        async Task<bool> HasCommitAsync(string input)
        {
            try
            {
                await ValidationProcess.RunAsync("git", ["-C", input, "cat-file", "-e", commit + "^{commit}"], plan.root, null, cancellation).ConfigureAwait(false);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
