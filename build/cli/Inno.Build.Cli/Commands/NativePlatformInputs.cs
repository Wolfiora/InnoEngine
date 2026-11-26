using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Composition;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Cli;

internal static class NativePlatformInputs
{
    internal static async Task PrepareAsync(
        BuildCommandContext request,
        string output,
        string previous,
        string[] configurations,
        CancellationToken cancellationToken
    ) {
        output = PathBoundary.RequireUnlinkedPath(request.root, Path.GetFullPath(output));
        if (Directory.Exists(output) || configurations.Any(value => value is not ("Debug" or "Release"))
            || configurations.Length != configurations.Distinct(StringComparer.Ordinal).Count()
        ) {
            throw new InvalidDataException("Platform preparation requires distinct configurations and fresh staging.");
        }
        Directory.CreateDirectory(output);
        string target = request.target.value;
        string prior = Path.Combine(previous, "native", "selection.json");
        JsonElement? seed = null;
        if (previous.Length > 0)
        {
            if (Read(Path.Combine(previous, "manifest.json")).GetProperty("target").GetString() != target)
            {
                throw new InvalidDataException("The native seed belongs to another target.");
            }
            seed = Read(prior);
        }
        BuildPlatformContribution platform = request.distribution.platforms.Single(value => value.descriptor.id == request.target);
        if (platform.nativeProducts.Count == 0)
        {
            throw new InvalidDataException("The platform has no declared native products.");
        }
        bool imported = false;
        var selected = new Dictionary<string, SortedDictionary<string, JsonElement>>(StringComparer.Ordinal)
        {
            ["assets"] = new(StringComparer.Ordinal),
            ["aggregates"] = new(StringComparer.Ordinal)
        };
        List<ProductRequest> requests = [];
        foreach (string configuration in configurations)
        {
            foreach (string id in platform.nativeProducts.Keys.Order(StringComparer.Ordinal))
            {
                NativeProductContribution product = request.distribution.ResolveNativeProduct(target, id);
                IReadOnlyList<NativeDependencyAsset>? reusable = null;
                if (seed.HasValue)
                {
                    try
                    {
                        reusable = await NativeDependencyManifest.SelectAsync(
                            request.root,
                            prior,
                            target,
                            configuration.ToLowerInvariant(),
                            product.plan,
                            cancellationToken
                        ).ConfigureAwait(false);
                    }
                    catch (InvalidDataException failure)
                    {
                        Console.WriteLine($"Preparing changed native inputs for {id}/{configuration}: {failure.Message}");
                    }
                }
                JsonElement source;
                HashSet<string> digests;
                if (reusable != null)
                {
                    source = seed!.Value;
                    digests = reusable.Select(asset => asset.sha256).ToHashSet(StringComparer.Ordinal);
                }
                else
                {
                    if (seed.HasValue && !imported)
                    {
                        foreach (string digest in Digests(seed.Value))
                        {
                            RequireDigest(digest);
                            _ = await NativeArtifactTransfer.ImportAsync(
                                request.root,
                                Path.Combine(previous, "native", digest + ".zip"),
                                cancellationToken
                            ).ConfigureAwait(false);
                        }
                        imported = true;
                    }
                    string fragment = await NativeDependenciesCommand.ProduceAsync(
                        request,
                        product,
                        configuration,
                        output,
                        cancellationToken
                    ).ConfigureAwait(false);
                    await NativeDependenciesCommand.VerifyBindingsAsync(
                        request,
                        product,
                        configuration,
                        cancellationToken
                    ).ConfigureAwait(false);
                    source = Read(fragment);
                    digests = Digests(source);
                }
                var retained = new HashSet<string>(StringComparer.Ordinal);
                foreach ((string family, SortedDictionary<string, JsonElement> members) in selected)
                {
                    foreach (JsonElement asset in source.GetProperty(family).EnumerateArray())
                    {
                        string digest = asset.GetProperty("sha256").GetString()!;
                        if (asset.GetProperty("target").GetString() != target
                            || asset.GetProperty("product").GetString() != id
                            || !digests.Contains(digest)
                        ) {
                            continue;
                        }
                        string name = asset.GetProperty("asset").GetString()!;
                        RequireDigest(digest);
                        if (!Regex.IsMatch(name, @"\A[A-Za-z0-9._-]+\.zip\z"))
                        {
                            throw new InvalidDataException("Invalid native asset name.");
                        }
                        string key = JsonSerializer.Serialize(new[]
                        {
                            id,
                            asset.GetProperty("component").GetString(),
                            asset.GetProperty("configuration").GetString(),
                            asset.GetProperty("options").GetString()
                        });
                        if (members.TryGetValue(key, out JsonElement existing) && existing.GetProperty("sha256").GetString() != digest)
                        {
                            throw new InvalidDataException("Conflicting native bytes for " + key);
                        }
                        string path = Path.Combine(output, name);
                        if (reusable != null && !File.Exists(path))
                        {
                            File.Copy(Path.Combine(previous, "native", digest + ".zip"), path);
                        }
                        using FileStream payload = File.OpenRead(path);
                        if (Convert.ToHexStringLower(
                            await SHA256.HashDataAsync(payload, cancellationToken).ConfigureAwait(false)
                        ) != digest
                        ) {
                            throw new InvalidDataException("Native bytes changed during assembly: " + name);
                        }
                        retained.Add(digest);
                        members[key] = asset;
                    }
                }
                if (digests.Count == 0 || !retained.SetEquals(digests))
                {
                    throw new InvalidDataException($"Incomplete native closure for {id}/{configuration}.");
                }
                requests.Add(new ProductRequest(target, id, configuration, digests.Order(StringComparer.Ordinal).ToArray()));
            }
        }
        AtomicFile.WriteAllBytes(
            Path.Combine(output, "selection.json"),
            JsonSerializer.SerializeToUtf8Bytes(
                new { requests, assets = selected["assets"].Values, aggregates = selected["aggregates"].Values }
            )
        );
    }

    private static HashSet<string> Digests(JsonElement inventory)
        => inventory.GetProperty("assets")
            .EnumerateArray()
            .Concat(inventory.GetProperty("aggregates").EnumerateArray())
            .Select(asset => asset.GetProperty("sha256").GetString()!)
            .ToHashSet(StringComparer.Ordinal);

    private static void RequireDigest(string digest)
    {
        if (!Regex.IsMatch(digest, @"\A[a-f0-9]{64}\z"))
        {
            throw new InvalidDataException("Invalid native payload digest.");
        }
    }

    private static JsonElement Read(string path) => JsonSerializer.Deserialize<JsonElement>(File.ReadAllBytes(path));

    private sealed record ProductRequest(
        string target,
        string product,
        string configuration,
        string[] digests
    );
}
