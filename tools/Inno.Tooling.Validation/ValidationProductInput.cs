using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
        string checkout = Path.Combine(directory, "product-consumer");
        await ValidationProcess.RunAsync("git", ["clone", "--no-checkout", "--filter=blob:none", repository, checkout],
            plan.root, Path.Combine(directory, "consumer-clone.log"), cancellation).ConfigureAwait(false);
        await ValidationProcess.RunAsync("git", ["-C", checkout, "fetch", "origin", commit], plan.root,
            Path.Combine(directory, "consumer-fetch.log"), cancellation).ConfigureAwait(false);
        await ValidationProcess.RunAsync("git", ["-C", checkout, "checkout", "--detach", commit], plan.root,
            Path.Combine(directory, "consumer-checkout.log"), cancellation).ConfigureAwait(false);
        string revision = await ValidationProcess.RunAsync("git", ["-C", checkout, "rev-parse", "HEAD"], plan.root, null, cancellation).ConfigureAwait(false);
        if (revision.Trim() != commit)
            throw new InvalidDataException("The fetched consumer does not match its declared revision.");
        string files = await ValidationProcess.RunAsync("git", ["-C", checkout, "ls-files", "-z", "--", prefix], plan.root, null, cancellation).ConfigureAwait(false);
        foreach (string file in files.Split('\0', StringSplitOptions.RemoveEmptyEntries))
        {
            cancellation.ThrowIfCancellationRequested();
            if (!file.StartsWith(prefix, StringComparison.Ordinal))
                throw new InvalidDataException("The consumer project selection escaped its declared prefix.");
            string input = ValidationInputs.Resolve(checkout, file);
            Inno.Core.IO.PathBoundary.RequireUnlinkedPath(checkout, input);
            string destination = ValidationInputs.Resolve(project, file[prefix.Length..]);
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            File.Copy(input, destination, overwrite: false);
        }
        if (!File.Exists(Path.Combine(project, "Assets", "FlappyBird", "FlappyBird.iscene")))
            throw new InvalidDataException("The declared product consumer does not supply the required real scene.");
    }
}
