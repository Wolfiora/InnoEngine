using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;
/// <summary>
/// Transfers aggregate native products and their exact binding generations through the dependency selection protocol.
/// </summary>
public static partial class NativeDependencyManifest
{
    private static string PlanOptions(ProductNativeBuildPlan plan) => JsonSerializer.Serialize(plan.steps.Select(step => new { step.id, step.component.nativeProject, step.component.toolchainProject, step.component.bindingDefinition, step.configuration, step.options, step.dependencies }));
    private static async Task<string> ExportAggregateAsync(
        NativeBuildContext context,
        ProductNativeBuildPlan plan,
        ProductNativeBuildResult products,
        string release,
        string output,
        CancellationToken cancellation
    ) {
        NativeBuildRecipe recipe = products.aggregateRecipe ?? throw new InvalidDataException("An aggregate export must retain its producer recipe.");
        NativeBuildProduct product = products.Single();
        var sources = await NativeDependencySource.CaptureAsync(context.engineRoot, recipe.inputs.Where(static input => input.logicalPath.StartsWith("engine/", StringComparison.Ordinal) && !input.logicalPath.StartsWith("engine/artifacts/", StringComparison.Ordinal)).Select(static input => input.logicalPath[7..]).Concat(context.inputState.CaptureRecordedInputs(context.engineRoot).entries.Where(static input => input.logicalPath.StartsWith("engine/", StringComparison.Ordinal) && !input.logicalPath.StartsWith("engine/artifacts/", StringComparison.Ordinal)).Select(static input => input.logicalPath[7..])).Concat(plan.steps.SelectMany(static step => new[] { step.component.nativeProject, step.component.toolchainProject })).Append(recipe.owner.nativeProject).Append(recipe.owner.toolchainProject), cancellation).ConfigureAwait(false);
        var bindings = products.bindingGenerations.ToDictionary(static pair => pair.Key, pair => new BindingAsset(pair.Value.fingerprint, pair.Value.usesRuntimeImports, Relative(pair.Value.artifactDirectory), Path.GetRelativePath(pair.Value.artifactDirectory, pair.Value.bindingsPath).Replace('\\', '/'), Path.GetRelativePath(pair.Value.artifactDirectory, pair.Value.nativeDirectory).Replace('\\', '/')), StringComparer.Ordinal);
        var directories = bindings.Values.Select(static binding => binding.directory).Append(Relative(product.directory)).ToHashSet(StringComparer.Ordinal);
        Directory.CreateDirectory(output);
        string assetName = product.component + "-" + product.targetId + "-" + product.fingerprint + ".zip";
        string archive = Path.Combine(output, assetName);
        await NativeArtifactTransfer.ExportAsync(context.engineRoot, archive, cancellation, directories).ConfigureAwait(false);
        using FileStream payload = File.OpenRead(archive);
        string hash = Convert.ToHexStringLower(await SHA256.HashDataAsync(payload, cancellation).ConfigureAwait(false));
        string revision = (await ToolchainEnvironment.CaptureOutputAsync("git", ["rev-parse", "HEAD"], context.engineRoot, cancellation).ConfigureAwait(false)).Trim();
        var asset = new AggregateAsset(plan.productId, product.component, product.targetId, context.configuration, PlanOptions(plan), Relative(product.directory), product.fingerprint, Path.GetRelativePath(product.directory, products.aggregateDirectory!).Replace('\\', '/'), product.sharedLibraryExtension, bindings, sources, context.RequireToolchain().declarations.ToArray(), release, assetName, hash, revision);
        string path = Path.Combine(output, plan.productId + "-" + product.targetId + "-" + context.configuration + ".json");
        await VerifySourcesAsync(context.engineRoot, sources, cancellation).ConfigureAwait(false);
        AtomicFile.WriteAllBytes(path, JsonSerializer.SerializeToUtf8Bytes(new Selection([], [asset]), new JsonSerializerOptions { WriteIndented = true }));
        return path;
        string Relative(string directory) => Path.GetRelativePath(Path.Combine(context.engineRoot, "artifacts"), directory).Replace('\\', '/');
    }

    private static async Task<ProductNativeBuildResult> AcquireAggregateAsync(
        string root,
        string archive,
        AggregateAsset asset,
        ProductNativeBuildPlan plan,
        CancellationToken cancellation
    ) {
        string[] owners = plan.steps.Where(static step => step.component.bindingDefinition is not null).Select(static step => step.component.nativeProject).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
        if (!owners.SequenceEqual(asset.bindings.Keys.Order(StringComparer.Ordinal), StringComparer.Ordinal))
            throw new InvalidDataException("The aggregate dependency has a different binding closure.");
        await VerifySourcesAsync(root, asset.sources, cancellation).ConfigureAwait(false);
        RequireDigest(asset.fingerprint);
        RequireDigest(asset.sha256);
        string directory = Resolve(asset.directory, "native", asset.component, asset.fingerprint);
        string[] bindingDirectories = asset.bindings.Values.Select(binding => ResolveBinding(binding)).ToArray();
        if (!Directory.Exists(directory) || bindingDirectories.Any(path => !Directory.Exists(path)))
        {
            using (FileStream payload = File.OpenRead(archive))
                if (Convert.ToHexStringLower(await SHA256.HashDataAsync(payload, cancellation).ConfigureAwait(false)) != asset.sha256)
                    throw new InvalidDataException("Native aggregate archive differs from its pinned digest.");
            await NativeArtifactTransfer.ImportAsync(root, archive, cancellation).ConfigureAwait(false);
        }

        var readers = new List<IDisposable>();
        NativeBuildProduct? product = null;
        try
        {
            var bindings = new Dictionary<string, NativeBindingGenerationDescriptor>(StringComparer.Ordinal);
            foreach (var pair in asset.bindings)
            {
                BindingAsset binding = pair.Value;
                string location = ResolveBinding(binding);
                string owner = Path.Combine(root, "artifacts", "bindings");
                readers.Add(await AcquireAsync(location, owner, "bindings", binding.fingerprint, ["Generated", "Native"], []).ConfigureAwait(false));
                bindings.Add(pair.Key, new() { fingerprint = binding.fingerprint, usesRuntimeImports = binding.runtimeImports, artifactDirectory = location, artifactOwnerDirectory = owner, bindingsPath = PathBoundary.Resolve(location, binding.managed), nativeDirectory = PathBoundary.Resolve(location, binding.native) });
            }

            BuildArtifactReadLease reading = await AcquireAsync(directory, Path.Combine(root, "artifacts", "native"), "native", asset.fingerprint, ["Outputs", "Metadata"], bindingDirectories).ConfigureAwait(false);
            try
            {
                product = new(asset.component, asset.target, asset.fingerprint, directory, null, asset.extension, reading);
            }
            catch
            {
                reading.Dispose();
                throw;
            }

            await VerifySourcesAsync(root, asset.sources, cancellation).ConfigureAwait(false);
            return new([product], new NativeBindingGenerationResult(bindings, readers), PathBoundary.Resolve(directory, asset.aggregate));
        }
        catch
        {
            product?.Dispose();
            foreach (IDisposable reader in readers)
                reader.Dispose();
            throw;
        }

        string ResolveBinding(BindingAsset binding)
        {
            RequireDigest(binding.fingerprint);
            string[] parts = binding.directory.Split('/');
            if (parts.Length != 4)
                throw new InvalidDataException("Invalid binding aggregate path.");
            return Resolve(binding.directory, "bindings", parts[1], binding.fingerprint);
        }

        string Resolve(
            string relative,
            string owner,
            string component,
            string fingerprint
        ) {
            if (component.Contains('/') || component.Contains('\\') || relative != $"{owner}/{component}/{asset.target}/{fingerprint}")
                throw new InvalidDataException("Aggregate dependency is outside its exact owner.");
            return PathBoundary.RequireUnlinkedPath(root, PathBoundary.Resolve(Path.Combine(root, "artifacts"), relative));
        }

        async Task<BuildArtifactReadLease> AcquireAsync(
            string location,
            string owner,
            string id,
            string fingerprint,
            string[] outputs,
            string[] dependencies
        ) {
            BuildCacheOwnerCatalog.Register(root, owner, id, BuildCacheArtifactRole.ImmutableArtifact, cancellation);
            BuildArtifactReadLease reading = await BuildArtifactReadLease.AcquireAsync(location, owner, fingerprint, outputs, cancellation).ConfigureAwait(false);
            try
            {
                var publisher = new BuildCachePublisher(owner, BuildCacheArtifactRole.ImmutableArtifact);
                using FileLease activity = await publisher.AcquireUseAsync(cancellation).ConfigureAwait(false);
                publisher.Record(location, location + ".read.lock", dependencies, null, asset.target, cancellation);
                return reading;
            }
            catch
            {
                reading.Dispose();
                throw;
            }
        }
    }

    private static async Task VerifySourcesAsync(
        string root,
        Dictionary<string, string> expected,
        CancellationToken cancellation
    ) {
        Dictionary<string, string> current = await NativeDependencySource.CaptureAsync(root, expected.Keys, cancellation).ConfigureAwait(false);
        if (current.Count != expected.Count || current.Any(pair => !expected.TryGetValue(pair.Key, out string? value) || value != pair.Value))
            throw new InvalidDataException("Native dependency sources changed; publish a matching manual dependency release.");
    }

    private sealed record AggregateAsset(
        string product,
        string component,
        string target,
        string configuration,
        string options,
        string directory,
        string fingerprint,
        string aggregate,
        string? extension,
        Dictionary<string, BindingAsset> bindings,
        Dictionary<string, string> sources,
        string[] producerTools,
        string release,
        string asset,
        string sha256,
        string producerRevision
    );
}
