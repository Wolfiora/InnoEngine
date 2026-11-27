using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

internal static class BackendPackageInventory
{
    internal static void Assemble(
        string stage,
        string planList,
        string selection,
        string target,
        string archive,
        CancellationToken cancellationToken
    ) {
        string[] paths = JsonSerializer.Deserialize<string[]>(File.ReadAllBytes(planList))!;
        Dictionary<string, BackendInputPlan> plans = paths.Select(path =>
            JsonSerializer.Deserialize<BackendInputPlan>(File.ReadAllBytes(path))!).ToDictionary(
                plan => plan.configuration,
                StringComparer.Ordinal
            );
        if (plans.Count == 0 || plans.Values.Any(plan => plan.target != target || plan.projects.Count == 0))
        {
            throw new InvalidDataException("The input plans must cover the selected platform.");
        }
        var files = new SortedDictionary<string, PackageFile>(StringComparer.Ordinal);
        var compiled = new HashSet<(string configuration, string owner)>();
        foreach (string receipt in Directory.EnumerateFiles(Path.Combine(stage, "receipts"), "files.txt", SearchOption.AllDirectories))
        {
            foreach (string line in File.ReadLines(receipt))
            {
                string[] fields = line.Split('|');
                if (fields.Length != 4 || !plans.TryGetValue(fields[1], out BackendInputPlan? plan)
                    || !plan.projects.TryGetValue(fields[0], out string? identity)
                    || !fields[2].StartsWith("managed/", StringComparison.Ordinal)
                ) {
                    throw new InvalidDataException("An SDK receipt has no matching planned owner or configuration: " + line);
                }
                Add(fields[0], fields[1], identity, fields[2], fields[3].ToLowerInvariant());
                if (Regex.IsMatch(fields[2], @"/lib/[^/]+\.dll$"))
                {
                    compiled.Add((fields[1], fields[0]));
                }
            }
        }
        foreach (BackendInputPlan plan in plans.Values)
        {
            foreach (string owner in plan.projects.Keys)
            {
                if (!compiled.Contains((plan.configuration, owner)))
                {
                    throw new InvalidDataException($"Missing compiled backend owner: {plan.configuration}/{owner}");
                }
            }
        }
        using JsonDocument inputs = JsonDocument.Parse(File.ReadAllBytes(selection));
        JsonElement[] ForTarget(string key)
            => inputs.RootElement.GetProperty(key)
                .EnumerateArray()
                .Where(item => item.GetProperty("target").GetString() == target)
                .ToArray();
        JsonElement[] requests = ForTarget("requests");
        var requested = new HashSet<(string product, string digest)>();
        foreach (JsonElement request in requests)
        {
            string product = request.GetProperty("product").GetString()!;
            JsonElement digests = request.GetProperty("digests");
            if (string.IsNullOrWhiteSpace(product)
                || !plans.ContainsKey(request.GetProperty("configuration").GetString()!)
                || digests.GetArrayLength() == 0
            ) {
                throw new InvalidDataException("Missing planned native product configuration.");
            }
            foreach (JsonElement value in digests.EnumerateArray())
            {
                string digest = value.GetString()!;
                RequireDigest(digest);
                requested.Add((product, digest));
            }
        }
        JsonElement[] assets = ForTarget("assets");
        JsonElement[] aggregates = ForTarget("aggregates");
        var present = new HashSet<(string product, string digest)>();
        string directory = Path.Combine(stage, "native");
        Directory.CreateDirectory(directory);
        string nativeRoot = Path.GetDirectoryName(Path.GetFullPath(selection))!;
        foreach (JsonElement item in assets.Concat(aggregates))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string asset = item.GetProperty("asset").GetString()!;
            string digest = item.GetProperty("sha256").GetString()!;
            string product = item.GetProperty("product").GetString()!;
            if (!requested.Contains((product, digest)) || !Regex.IsMatch(asset, @"\A[A-Za-z0-9._-]+\.zip\z"))
            {
                throw new InvalidDataException("Native payload is outside the requested product closure.");
            }
            present.Add((product, digest));
            string path = "native/" + digest + ".zip";
            if (files.ContainsKey(path))
            {
                continue;
            }
            string source = PathBoundary.RequireUnlinkedPath(nativeRoot, Path.Combine(nativeRoot, asset));
            File.Copy(source, PathBoundary.RequireUnlinkedPath(stage, Path.Combine(stage, path)), true);
            Add(
                item.GetProperty("component").GetString()!,
                item.GetProperty("configuration").GetString()!,
                item.GetProperty("fingerprint").GetString()!,
                path,
                digest
            );
        }
        if (requested.Count == 0 || !requested.SetEquals(present))
        {
            throw new InvalidDataException("A certified native product references a missing payload.");
        }
        byte[] manifest = JsonSerializer.SerializeToUtf8Bytes(new { requests, assets, aggregates });
        AtomicFile.WriteAllBytes(Path.Combine(directory, "selection.json"), manifest);
        string manifestHash = Convert.ToHexStringLower(SHA256.HashData(manifest));
        Add(target, "all", manifestHash, "native/selection.json", manifestHash);
        string inventory = Path.Combine(stage, "package-inventory.json");
        AtomicFile.WriteAllBytes(inventory, JsonSerializer.SerializeToUtf8Bytes(new { version = 1, target, files = files.Values }));
        BackendPrebuiltArchive.Create(stage, inventory, archive, cancellationToken);

        void Add(
            string owner,
            string configuration,
            string input,
            string path,
            string sha256
        ) {
            RequireDigest(input);
            RequireDigest(sha256);
            files.Add(path, new PackageFile(owner, configuration, input, path, sha256));
        }
    }

    private static void RequireDigest(string digest)
    {
        if (!Regex.IsMatch(digest, @"\A[0-9a-f]{64}\z"))
        {
            throw new InvalidDataException("Invalid platform file digest: " + digest);
        }
    }

    private sealed record PackageFile(
        string owner,
        string configuration,
        string input,
        string path,
        string sha256
    );
}
