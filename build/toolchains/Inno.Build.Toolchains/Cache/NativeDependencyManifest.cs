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
/// Publishes exact native dependency selections and acquires their immutable payloads without invoking producers.
/// </summary>
public static partial class NativeDependencyManifest
{
    /// <summary>
    /// Selects only the archives needed by a declared product and verifies source provenance before downloading.
    /// </summary>
    /// <param name = "engineRoot">
    /// The current consuming checkout.
    /// </param>
    /// <param name = "selectionPath">
    /// The repository's exact dependency selection file.
    /// </param>
    /// <param name = "target">
    /// The explicit requested target.
    /// </param>
    /// <param name = "configuration">
    /// The normalized debug or release configuration.
    /// </param>
    /// <param name = "plan">
    /// The current composition's dependency closure.
    /// </param>
    /// <param name = "cancellationToken">
    /// Cancels source verification.
    /// </param>
    /// <returns>
    /// The distinct exact assets required for consumption; no downloads or compilation occur.
    /// </returns>
    /// <exception cref = "InvalidDataException">
    /// The selection is incomplete, ambiguous, or stale.
    /// </exception>
    /// <exception cref = "IOException">
    /// The selection or source checkout cannot be read.
    /// </exception>
    public static async Task<IReadOnlyList<NativeDependencyAsset>> SelectAsync(
        string engineRoot,
        string selectionPath,
        string target,
        string configuration,
        ProductNativeBuildPlan plan,
        CancellationToken cancellationToken = default
    ) {
        ToolchainEnvironment.ValidateConfiguration(configuration);
        Selection selection = ReadSelection(selectionPath);
        AggregateAsset[] aggregate = selection.aggregates
            .Where(
                asset => (
                    asset.product == plan.productID
                    && asset.target == target
                    && asset.configuration == configuration
                    && asset.options == PlanOptions(plan)
                )
            )
            .ToArray();
        if (aggregate.Length > 1)
        {
            throw new InvalidDataException("Native aggregate dependency selection is ambiguous.");
        }
        if (aggregate.Length == 1)
        {
            await VerifySourcesAsync(engineRoot, aggregate[0].sources, cancellationToken).ConfigureAwait(false);
            RequireDigest(aggregate[0].sha256);
            return[new NativeDependencyAsset(aggregate[0].asset, aggregate[0].sha256)];
        }

        List<NativeDependencyAsset> assets = [];
        var sources = new Dictionary<string, string>(StringComparer.Ordinal);
        var selectedComponents = new Dictionary<string, DependencyAsset>(StringComparer.Ordinal);
        foreach (ProductNativeBuildStep step in plan.steps)
        {
            DependencyAsset[] matches = selection.assets
                .Where(
                    asset => (
                        asset.product == plan.productID
                        && asset.component == step.id
                        && asset.target == target
                        && asset.configuration == (step.configuration ?? configuration)
                        && asset.options == Options(step)
                        && asset.nativeProject == step.component.nativeProject
                        && asset.toolchainProject == step.component.toolchainProject
                    )
                )
                .ToArray();
            if (matches.Length != 1)
            {
                throw new InvalidDataException(
                    $"Missing or ambiguous native dependency: {step.id}/{target}/{step.configuration ?? configuration}. Produce the matching backend package for the requested target."
                );
            }
            DependencyAsset selected = matches[0];
            if ((selected.binding is null) != (step.component.bindingDefinition is null))
            {
                throw new InvalidDataException("Native dependency binding ownership differs: " + step.id);
            }
            selectedComponents.Add(step.id, selected);
            RequireDigest(selected.sha256);
            assets.Add(new NativeDependencyAsset(selected.asset, selected.sha256));
            foreach (KeyValuePair<string, string> pair in selected.sources)
            {
                if (sources.TryGetValue(pair.Key, out string? previous) && previous != pair.Value)
                {
                    throw new InvalidDataException("Native packages disagree about their shared source inputs.");
                }
                else
                {
                    sources[pair.Key] = pair.Value;
                }
            }
        }

        foreach (ProductNativeBuildStep step in plan.steps)
        {
            DependencyAsset selected = selectedComponents[step.id];
            if (selected.dependencies.Count != step.dependencies.Count
                || step.dependencies.Any(
                    id => (
                        !selected.dependencies.TryGetValue(id, out string? fingerprint)
                        || selectedComponents[id].fingerprint != fingerprint
                    )
                )
            ) {
                throw new InvalidDataException("Native dependency was built against a different closure: " + step.id);
            }
        }

        await VerifySourcesAsync(engineRoot, sources, cancellationToken).ConfigureAwait(false);
        return assets.Distinct().ToArray();
    }

    /// <summary>
    /// Exports the selected product's component generations while the caller retains their reading ownership.
    /// </summary>
    /// <param name = "context">
    /// The producer's frozen source and toolchain context.
    /// </param>
    /// <param name = "plan">
    /// The product definition used by the producer.
    /// </param>
    /// <param name = "products">
    /// The complete borrowed result, in plan order.
    /// </param>
    /// <param name = "outputDirectory">
    /// An explicit checkout-owned export directory.
    /// </param>
    /// <param name = "cancellationToken">
    /// Cancels unpublished export work.
    /// </param>
    /// <returns>
    /// The complete selection fragment path; publishing the fragment remains the caller's responsibility.
    /// </returns>
    /// <exception cref = "InvalidDataException">
    /// The source, product closure or exported payload is inconsistent.
    /// </exception>
    public static async Task<string> ExportAsync(
        NativeBuildContext context,
        ProductNativeBuildPlan plan,
        ProductNativeBuildResult products,
        string outputDirectory,
        CancellationToken cancellationToken = default
    ) {
        string root = context.engineRoot;
        string output = PathBoundary.RequireUnlinkedPath(root, Path.GetFullPath(outputDirectory));
        if (products.aggregateDirectory is not null)
        {
            return await ExportAggregateAsync(context, plan, products, output, cancellationToken).ConfigureAwait(false);
        }
        if (products.Count != plan.steps.Count)
        {
            throw new InvalidDataException("Dependency export requires the exact complete product closure.");
        }
        Directory.CreateDirectory(output);
        IReadOnlyList<NativeBuildRecipe> recipes = plan.CreateRecipes(context);
        string revision = (await ToolchainEnvironment.CaptureOutputAsync(
            "git",
            ["rev-parse", "HEAD"],
            root,
            cancellationToken: cancellationToken
        ).ConfigureAwait(false)).Trim();
        List<DependencyAsset> assets = [];
        for (int index = 0; index < products.Count; index++)
        {
            ProductNativeBuildStep step = plan.steps[index];
            NativeBuildProduct product = products[index];
            if (product.component != step.id || product.targetID != context.RequireToolchain().targetID)
            {
                throw new InvalidDataException("Dependency export encountered a foreign product.");
            }
            string[] sourcePaths = recipes[index].inputs
                .Where(
                    static input => (
                        input.logicalPath.StartsWith("engine/", StringComparison.Ordinal)
                        && !input.logicalPath.StartsWith("engine/artifacts/", StringComparison.Ordinal)
                    )
                )
                .Select(static input => input.logicalPath["engine/".Length..])
                .Append(step.component.nativeProject)
                .Append(step.component.toolchainProject)
                .Concat(step.component.bindingDefinition is string definition ? [definition] : [])
                .Distinct(StringComparer.Ordinal)
                .ToArray();
            if (step.component.bindingDefinition is not null)
            {
                NativeBuildContext bindingContext = new NativeBuildContext(root, step.configuration ?? context.configuration)
                    .WithToolchain(context.RequireToolchain())
                    .WithBindingGenerator(context.RequireBindingGenerator());
                await bindingContext
                    .RequireBindingGenerator()
                    .ComputeInputFingerprintAsync(
                        bindingContext,
                        new NativeBindingGenerationRequest(
                            [step.component],
                            product.targetID,
                            NativeBindingOutputMode.TargetArtifacts,
                            componentOptions: new Dictionary<string, NativeComponentBuildOptions>
                            {
                                [step.component.nativeProject] = step.options
                            }
                        ),
                        cancellationToken
                    )
                    .ConfigureAwait(false);
                sourcePaths = sourcePaths
                    .Concat(
                        bindingContext.inputState
                            .CaptureRecordedInputs(root).entries
                            .Where(
                                input => (
                                    input.logicalPath.StartsWith("engine/", StringComparison.Ordinal)
                                    && !input.logicalPath.StartsWith("engine/artifacts/", StringComparison.Ordinal)
                                )
                            )
                            .Select(static input => input.logicalPath[7..])
                    )
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();
            }

            Dictionary<string, string> sources = await NativeDependencySource.CaptureAsync(
                root,
                sourcePaths,
                cancellationToken
            ).ConfigureAwait(false);
            string directory = Relative(product.directory);
            var selected = new HashSet<string>(StringComparer.Ordinal)
            {
                directory
            };
            BindingAsset? binding = null;
            if (product.bindingGeneration is { } generation)
            {
                if (generation.artifactDirectory.Length == 0)
                {
                    throw new InvalidDataException("Dependency publication requires immutable target binding generations.");
                }
                string bindingDirectory = Relative(generation.artifactDirectory);
                selected.Add(bindingDirectory);
                binding = new(
                    generation.fingerprint,
                    generation.usesRuntimeImports,
                    bindingDirectory,
                    Path.GetRelativePath(generation.artifactDirectory, generation.bindingsPath).Replace('\\', '/'),
                    Path.GetRelativePath(generation.artifactDirectory, generation.nativeDirectory).Replace('\\', '/')
                );
            }

            string asset = step.id + "-" + product.targetID + "-" + product.fingerprint + ".zip";
            string archive = Path.Combine(output, asset);
            await NativeArtifactTransfer.ExportAsync(root, archive, selected, cancellationToken).ConfigureAwait(false);
            using FileStream payload = File.OpenRead(archive);
            string digest = Convert.ToHexStringLower(await SHA256.HashDataAsync(payload, cancellationToken).ConfigureAwait(false));
            assets.Add(
                new DependencyAsset(
                    plan.productID,
                    step.id,
                    product.targetID,
                    step.configuration ?? context.configuration,
                    step.component.nativeProject,
                    step.component.toolchainProject,
                    Options(step),
                    directory,
                    product.fingerprint,
                    product.sharedLibraryExtension,
                    binding,
                    sources,
                    context.RequireToolchain().declarations.ToArray(),
                    step.dependencies.ToDictionary(
                        id => id,
                        id => products.Single(product => product.component == id).fingerprint,
                        StringComparer.Ordinal
                    ),
                    asset,
                    digest,
                    revision
                )
            );
        }

        foreach (DependencyAsset asset in assets)
        {
            await VerifySourcesAsync(root, asset.sources, cancellationToken).ConfigureAwait(false);
        }
        string destination = Path.Combine(
            output,
            plan.productID + "-" + context.RequireToolchain().targetID + "-" + context.configuration + ".json"
        );
        AtomicFile.WriteAllBytes(
            destination,
            JsonSerializer.SerializeToUtf8Bytes(
                new Selection(assets.ToArray(), [], "platform-packages"),
                new JsonSerializerOptions { WriteIndented = true }
            )
        );
        return destination;
        string Relative(string path) => Path.GetRelativePath(Path.Combine(root, "artifacts"), path).Replace('\\', '/');
    }

    /// <summary>
    /// Acquires a matching dependency closure from explicitly downloaded assets, failing instead of invoking compilation.
    /// </summary>
    /// <param name = "engineRoot">
    /// The consuming source checkout.
    /// </param>
    /// <param name = "selectionPath">
    /// The exact dependency selection file.
    /// </param>
    /// <param name = "target">
    /// The requested native target, independent of the host.
    /// </param>
    /// <param name = "configuration">
    /// The requested debug or release configuration.
    /// </param>
    /// <param name = "plan">
    /// The current product's declared component closure.
    /// </param>
    /// <param name = "cancellationToken">
    /// Cancels acquisition and releases acquired readers.
    /// </param>
    /// <returns>
    /// Owned products and bindings protected by the existing artifact leases.
    /// </returns>
    /// <exception cref = "InvalidDataException">
    /// The selection is missing, ambiguous, stale or damaged.
    /// </exception>
    /// <exception cref = "IOException">
    /// Required dependency assets or source inputs are unavailable.
    /// </exception>
    public static async Task<ProductNativeBuildResult> AcquireAsync(
        string engineRoot,
        string selectionPath,
        string target,
        string configuration,
        ProductNativeBuildPlan plan,
        CancellationToken cancellationToken = default
    ) {
        ToolchainEnvironment.ValidateConfiguration(configuration);
        string root = Path.GetFullPath(engineRoot);
        Selection selection = ReadSelection(selectionPath);
        AggregateAsset[] aggregates = selection.aggregates
            .Where(
                asset => (
                    asset.product == plan.productID
                    && asset.target == target
                    && asset.configuration == configuration
                    && asset.options == PlanOptions(plan)
                )
            )
            .ToArray();
        if (aggregates.Length > 1)
        {
            throw new InvalidDataException("Native dependency selection contains ambiguous aggregate products.");
        }
        if (aggregates.Length == 1)
        {
            return await AcquireAggregateAsync(
                root,
                Archive(aggregates[0].sha256),
                aggregates[0],
                plan,
                cancellationToken
            ).ConfigureAwait(false);
        }
        if (selection.assets is null)
        {
            throw new InvalidDataException("Native dependency selection has no asset inventory.");
        }
        var expectedSources = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (ProductNativeBuildStep step in plan.steps)
        {
            DependencyAsset asset = Select(step);
            foreach (KeyValuePair<string, string> pair in asset.sources)
            {
                if (expectedSources.TryGetValue(pair.Key, out string? existing) && existing != pair.Value)
                {
                    throw new InvalidDataException("Dependency assets disagree about a shared source input: " + pair.Key);
                }
                else
                {
                    expectedSources[pair.Key] = pair.Value;
                }
            }
        }

        string Archive(string digest)
        {
            RequireDigest(digest);
            if (selection.layout != "platform-packages")
            {
                throw new InvalidDataException("Native acquisition requires a bound platform package store.");
            }
            if (!target.All(static value => char.IsAsciiLetterOrDigit(value) || value == '-'))
            {
                throw new InvalidDataException("The native payload requires a registered platform name.");
            }
            string store = Path.GetDirectoryName(Path.GetFullPath(selectionPath))!;
            return PathBoundary.RequireUnlinkedPath(store, PathBoundary.Resolve(store, $"{target}/native/{digest}.zip"));
        }

        await VerifySourcesAsync().ConfigureAwait(false);
        List<NativeBuildProduct> products = [];
        var bindings = new Dictionary<string, NativeBindingGenerationDescriptor>(StringComparer.Ordinal);
        List<IDisposable> reading = [];
        try
        {
            foreach (ProductNativeBuildStep step in plan.steps)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string selectedConfiguration = step.configuration ?? configuration;
                DependencyAsset asset = Select(step);
                if (asset.nativeProject != step.component.nativeProject
                    || asset.toolchainProject != step.component.toolchainProject
                    || (asset.binding is null) != (step.component.bindingDefinition is null)
                ) {
                    throw new InvalidDataException("Native dependency component ownership differs from the current product: " + step.id);
                }
                if (asset.dependencies.Count != step.dependencies.Count
                    || step.dependencies.Any(
                        id => (
                            !asset.dependencies.TryGetValue(id, out string? fingerprint)
                            || products.Single(product => product.component == id).fingerprint != fingerprint
                        )
                    )
                ) {
                    throw new InvalidDataException(
                        "Native dependency artifacts were compiled against a different dependency closure: " + step.id
                    );
                }
                RequireDigest(asset.sha256);
                RequireDigest(asset.fingerprint);
                string directory = ResolveArtifact(asset.directory, "native", asset.component, target, asset.fingerprint);
                string archive = Archive(asset.sha256);
                if (!Directory.Exists(directory)
                    || asset.binding is { } requiredBinding
                       && !Directory.Exists(PathBoundary.Resolve(Path.Combine(root, "artifacts"), requiredBinding.directory))
                ) {
                    using (FileStream payload = File.OpenRead(archive))
                    {
                        if (Convert.ToHexStringLower(
                            await SHA256.HashDataAsync(payload, cancellationToken).ConfigureAwait(false)
                        ) != asset.sha256
                        ) {
                            throw new InvalidDataException(
                                "Native dependency archive digest differs from its pinned selection: " + asset.asset
                            );
                        }
                    }
                    await NativeArtifactTransfer.ImportAsync(root, archive, cancellationToken).ConfigureAwait(false);
                }

                NativeBindingGenerationDescriptor? generation = null;
                if (asset.binding is { } binding)
                {
                    RequireDigest(binding.fingerprint);
                    string[] parts = binding.directory.Split('/');
                    if (parts.Length != 4)
                    {
                        throw new InvalidDataException("Invalid native binding artifact location.");
                    }
                    string bindingDirectory = ResolveArtifact(binding.directory, "bindings", parts[1], target, binding.fingerprint);
                    string owner = Path.Combine(root, "artifacts", "bindings");
                    BuildCacheOwnerCatalog.Register(root, owner, "bindings", BuildCacheArtifactRole.ImmutableArtifact, cancellationToken);
                    reading.Add(
                        await BuildArtifactReadLease.AcquireAsync(
                            bindingDirectory,
                            owner,
                            binding.fingerprint,
                            ["Generated", "Native"],
                            cancellationToken: cancellationToken
                        ).ConfigureAwait(false)
                    );
                    using (FileLease activity = await new BuildCachePublisher(
                        owner,
                        BuildCacheArtifactRole.ImmutableArtifact
                    ).AcquireUseAsync(cancellationToken).ConfigureAwait(false)
                    ) {
                        new BuildCachePublisher(owner, BuildCacheArtifactRole.ImmutableArtifact).Record(
                            bindingDirectory,
                            bindingDirectory + ".read.lock",
                            [],
                            null,
                            target,
                            cancellationToken
                        );
                    }
                    generation = new()
                    {
                        fingerprint = binding.fingerprint,
                        usesRuntimeImports = binding.runtimeImports,
                        artifactDirectory = bindingDirectory,
                        artifactOwnerDirectory = owner,
                        bindingsPath = PathBoundary.Resolve(bindingDirectory, binding.managed),
                        nativeDirectory = PathBoundary.Resolve(bindingDirectory, binding.native)
                    };
                    if (bindings.TryGetValue(step.component.nativeProject, out NativeBindingGenerationDescriptor? previous)
                        && previous.fingerprint != generation.fingerprint
                    ) {
                        throw new InvalidDataException("A dependency closure selected conflicting binding generations.");
                    }
                    bindings[step.component.nativeProject] = generation;
                }

                string nativeOwner = Path.Combine(root, "artifacts", "native");
                BuildCacheOwnerCatalog.Register(root, nativeOwner, "native", BuildCacheArtifactRole.ImmutableArtifact, cancellationToken);
                BuildArtifactReadLease lease = await BuildArtifactReadLease.AcquireAsync(
                    directory,
                    nativeOwner,
                    asset.fingerprint,
                    ["Outputs", "Metadata"],
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);
                try
                {
                    using FileLease activity = await new BuildCachePublisher(
                        nativeOwner,
                        BuildCacheArtifactRole.ImmutableArtifact
                    ).AcquireUseAsync(cancellationToken).ConfigureAwait(false);
                    new BuildCachePublisher(nativeOwner, BuildCacheArtifactRole.ImmutableArtifact).Record(
                        directory,
                        directory + ".read.lock",
                        generation is null ? [] : [generation.artifactDirectory],
                        step.id + "/" + target + "/" + selectedConfiguration,
                        target,
                        cancellationToken
                    );
                    products.Add(new NativeBuildProduct(step.id, target, asset.fingerprint, directory, generation, asset.extension, lease));
                }
                catch
                {
                    lease.Dispose();
                    throw;
                }
            }

            await VerifySourcesAsync().ConfigureAwait(false);
            return new(products, new NativeBindingGenerationResult(bindings, reading));
        }
        catch
        {
            foreach (NativeBuildProduct product in products)
            {
                product.Dispose();
            }
            foreach (IDisposable lease in reading)
            {
                lease.Dispose();
            }
            throw;
        }

        DependencyAsset Select(ProductNativeBuildStep step)
        {
            DependencyAsset[] matches = selection.assets
                .Where(
                    asset => (
                        asset.product == plan.productID
                        && asset.component == step.id
                        && asset.target == target
                        && asset.configuration == (step.configuration ?? configuration)
                        && asset.options == Options(step)
                    )
                )
                .ToArray();
            if (matches.Length != 1)
            {
                throw new InvalidDataException(
                    $"Native dependency '{step.id}/{target}/{step.configuration ?? configuration}' requires exactly one matching artifact. Produce the matching backend package for the requested target."
                );
            }
            return matches[0];
        }

        async Task VerifySourcesAsync()
        {
            Dictionary<string, string> current = await NativeDependencySource.CaptureAsync(
                root,
                expectedSources.Keys,
                cancellationToken
            ).ConfigureAwait(false);
            if (current.Count != expectedSources.Count
                || current.Any(pair => !expectedSources.TryGetValue(pair.Key, out string? value) || value != pair.Value)
            ) {
                throw new InvalidDataException(
                    "Native dependency sources changed. Produce the matching backend package for the requested target."
                );
            }
        }

        string ResolveArtifact(
            string relative,
            string owner,
            string component,
            string abi,
            string fingerprint
        ) {
            if (relative != $"{owner}/{component}/{abi}/{fingerprint}" || component.Contains('/') || component.Contains('\\'))
            {
                throw new InvalidDataException("Native dependency artifact is outside its exact declared owner.");
            }
            return PathBoundary.RequireUnlinkedPath(root, PathBoundary.Resolve(Path.Combine(root, "artifacts"), relative));
        }
    }

    private static Selection ReadSelection(string path)
    {
        Selection selection;
        try
        {
            selection = JsonSerializer.Deserialize<Selection>(File.ReadAllBytes(path))
                        ?? throw new InvalidDataException("Native dependency selection is empty.");
        }
        catch (JsonException failure)
        {
            throw new InvalidDataException("Native dependency selection is malformed.", failure);
        }

        if (selection.layout is not (null or "platform-packages"))
        {
            throw new InvalidDataException("Unknown native payload layout.");
        }
        if (selection.assets is null || selection.aggregates is null)
        {
            throw new InvalidDataException("Native dependency selection requires component and aggregate inventories.");
        }
        foreach (DependencyAsset asset in selection.assets)
        {
            if (asset is null
                || asset.sources is null
                || asset.producerTools is null
                || asset.dependencies is null
                || string.IsNullOrWhiteSpace(asset.product)
                || string.IsNullOrWhiteSpace(asset.component)
                || string.IsNullOrWhiteSpace(asset.target)
                || string.IsNullOrWhiteSpace(asset.nativeProject)
                || string.IsNullOrWhiteSpace(asset.toolchainProject)
                || string.IsNullOrWhiteSpace(asset.options)
                || string.IsNullOrWhiteSpace(asset.directory)
            ) {
                throw new InvalidDataException("Native dependency asset is missing required provenance or ownership.");
            }
            ValidateAsset(asset.asset, asset.sha256, asset.fingerprint, asset.sources, asset.producerRevision);
        }

        foreach (AggregateAsset asset in selection.aggregates)
        {
            if (asset is null
                || asset.sources is null
                || asset.producerTools is null
                || asset.bindings is null
                || string.IsNullOrWhiteSpace(asset.product)
                || string.IsNullOrWhiteSpace(asset.component)
                || string.IsNullOrWhiteSpace(asset.target)
                || string.IsNullOrWhiteSpace(asset.options)
                || string.IsNullOrWhiteSpace(asset.directory)
                || string.IsNullOrWhiteSpace(asset.aggregate)
            ) {
                throw new InvalidDataException("Native aggregate is missing required provenance or ownership.");
            }
            ValidateAsset(asset.asset, asset.sha256, asset.fingerprint, asset.sources, asset.producerRevision);
        }

        return selection;
        static void ValidateAsset(
            string name,
            string digest,
            string fingerprint,
            Dictionary<string, string> sources,
            string revision
        ) {
            if (revision is null || revision.Length != 40 || revision.Any(static character => !char.IsAsciiHexDigitLower(character)))
            {
                throw new InvalidDataException("Native dependency producer revision must be an exact commit.");
            }
            RequireDigest(digest);
            RequireDigest(fingerprint);
            if (string.IsNullOrWhiteSpace(name)
                || name != Path.GetFileName(name)
                || name.Contains('\\')
                || !name.EndsWith(".zip", StringComparison.Ordinal)
                || sources.Count == 0
            ) {
                throw new InvalidDataException("Native dependency requires a named native archive and nonempty source provenance.");
            }
            foreach (string identity in sources.Values)
            {
                RequireDigest(identity);
            }
        }
    }

    private static string Options(ProductNativeBuildStep step) => JsonSerializer.Serialize(new { step.options, step.dependencies });

    private static void RequireDigest(string value)
    {
        if (value is null || value.Length != 64 || value.Any(static character => !char.IsAsciiHexDigitLower(character)))
        {
            throw new InvalidDataException("Native dependency digest must be a complete lowercase SHA-256 identity.");
        }
    }

    private sealed record Selection(
        DependencyAsset[] assets,
        AggregateAsset[] aggregates,
        string? layout = null
    );

    private sealed record BindingAsset(
        string fingerprint,
        bool runtimeImports,
        string directory,
        string managed,
        string native
    );

    private sealed record DependencyAsset(
        string product,
        string component,
        string target,
        string configuration,
        string nativeProject,
        string toolchainProject,
        string options,
        string directory,
        string fingerprint,
        string? extension,
        BindingAsset? binding,
        Dictionary<string, string> sources,
        string[] producerTools,
        Dictionary<string, string> dependencies,
        string asset,
        string sha256,
        string producerRevision
    );
}
