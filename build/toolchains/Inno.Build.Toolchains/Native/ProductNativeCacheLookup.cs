using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

internal static class ProductNativeCacheLookup
{
    internal static async Task<IReadOnlyList<NativeBuildProduct>?> TryAcquireAsync(
        NativeBuildContext context,
        IReadOnlyList<ProductNativeBuildStep> steps,
        CancellationToken cancellationToken
    ) {
        if (steps.Any(static step => (
            step.createRecipe is null
            || step.dependencies.Count != 0 && step.bindDependencies is null
        ))) {
            return null;
        }
        var cache = new BuildCachePublisher(
            Path.Combine(context.engineRoot, "artifacts", "native"),
            BuildCacheArtifactRole.ImmutableArtifact
        );
        BuildCacheOwnerCatalog.Register(
            context.engineRoot,
            Path.Combine(context.engineRoot, "artifacts", "native"),
            "native",
            cache.role,
            cancellationToken
        );
        List<FileLease> activities = [];
        List<FileLease> ownership = [];
        var products = new Dictionary<string, NativeBuildProduct>(StringComparer.Ordinal);
        List<Candidate> candidates = [];
        bool transferred = false;
        try
        {
            activities.Add(await cache.AcquireUseAsync(cancellationToken).ConfigureAwait(false));
            foreach (string root in steps.Select(step => Path.Combine(step.component.GetToolchainRoot(context.engineRoot), "obj", "native"))
                .Distinct(pathComparer)
                .Order(pathComparer)
            ) {
                activities.Add(
                    await new BuildCachePublisher(root, BuildCacheArtifactRole.ReusableWorkspace)
                        .AcquireUseAsync(cancellationToken)
                        .ConfigureAwait(false)
                );
            }
            foreach (ProductNativeBuildStep step in steps)
            {
                cancellationToken.ThrowIfCancellationRequested();
                NativeBuildContext selected = (step.configuration is string configuration
                                               ? context.WithConfiguration(configuration)
                                               : context).WithComponentOptions(step.component, step.options);
                NativeBuildRecipe recipe = step.createRecipe!(selected);
                if (recipe.component != step.id || recipe.targetID != context.RequireToolchain().targetID
                    || recipe.owner.nativeProject != step.component.nativeProject
                    || recipe.owner.toolchainProject != step.component.toolchainProject
                ) {
                    throw new InvalidOperationException($"Native step '{step.id}' returned a foreign source declaration.");
                }
                if (step.bindDependencies is not null)
                {
                    recipe = step.bindDependencies(selected, recipe, products);
                }
                if (recipe.component != step.id || recipe.targetID != context.RequireToolchain().targetID
                    || recipe.owner.nativeProject != step.component.nativeProject
                    || recipe.owner.toolchainProject != step.component.toolchainProject
                ) {
                    throw new InvalidOperationException($"Native step '{step.id}' returned a foreign dependency declaration.");
                }
                NativeBindingGenerationDescriptor? binding = step.component.bindingDefinition is null
                                                             ? null
                                                             : selected.RequireBindings(step.component);
                if (binding is not null)
                {
                    recipe = recipe.WithBindingGeneration(context.engineRoot, binding);
                }
                NativeInputSnapshot snapshot = context.inputState.CaptureInitial(recipe.inputs, cancellationToken);
                string fingerprint = NativeBuildFingerprint.Create(recipe.declarations, snapshot);
                string identity = NativeWorkspaceIdentity.Create(selected, recipe, snapshot);
                string workspace = selected.WithIdentity(step.component, recipe.targetID, identity).GetNativeBuildRoot(step.component);
                string directory = Path.Combine(context.engineRoot, "artifacts", "native", step.id, recipe.targetID, fingerprint);
                var candidate = new Candidate(selected, recipe, fingerprint, directory, workspace, binding);
                NativeBuildProduct? product = await AcquireProductAsync(candidate, cancellationToken).ConfigureAwait(false);
                if (product is null)
                {
                    return null;
                }
                products.Add(step.id, product);
                candidates.Add(candidate);
            }
            // Preliminary readers freeze dependency selection. Release them before waiting for
            // compiler ownership, so repair holding a workspace never waits for this batch's reader.
            foreach (NativeBuildProduct product in products.Values)
            {
                product.Dispose();
            }
            products.Clear();
            foreach (string workspace in candidates
                .Select(static candidate => candidate.workspace)
                .Distinct(pathComparer)
                .Order(pathComparer)
            ) {
                ownership.Add(
                    await FileLease.AcquireAsync(workspace + ".lock", Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false)
                );
            }
            foreach (Candidate candidate in candidates)
            {
                NativeBuildProduct? product = await AcquireProductAsync(candidate, cancellationToken).ConfigureAwait(false);
                if (product is null)
                {
                    return null;
                }
                products.Add(candidate.recipe.component, product);
            }
            NativeBuildContext verification = context.BeginInputVerification("native-product-lock");
            foreach (Candidate candidate in candidates)
            {
                if (NativeBuildFingerprint.Create(
                    verification,
                    candidate.recipe.declarations,
                    candidate.recipe.inputs,
                    cancellationToken
                ) != candidate.fingerprint
                ) {
                    throw new InvalidOperationException(
                        $"Native inputs for '{candidate.recipe.component}' changed during product ownership acquisition."
                    );
                }
            }
            foreach (Candidate candidate in candidates)
            {
                cache.Record(
                    candidate.directory,
                    candidate.directory + ".read.lock",
                    candidate.binding is { artifactDirectory.Length: > 0 } ? [candidate.binding.artifactDirectory] : [],
                    candidate.recipe.component + "/" + candidate.recipe.targetID + "/" + candidate.context.configuration,
                    candidate.recipe.targetID,
                    cancellationToken
                );
            }
            transferred = true;
            return steps.Select(step => products[step.id]).ToArray();
        }
        finally
        {
            if (!transferred)
            {
                foreach (NativeBuildProduct product in products.Values)
                {
                    product.Dispose();
                }
            }
            for (int index = ownership.Count - 1; index >= 0; index--)
            {
                ownership[index].Dispose();
            }
            for (int index = activities.Count - 1; index >= 0; index--)
            {
                activities[index].Dispose();
            }
        }
    }

    private static StringComparer pathComparer => OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

    private static async ValueTask<NativeBuildProduct?> AcquireProductAsync(
        Candidate candidate,
        CancellationToken cancellationToken
    ) {
        BuildArtifactReadLease? reading = await BuildArtifactReadLease.TryAcquireAsync(
            candidate.directory,
            Path.Combine(candidate.context.engineRoot, "artifacts", "native"),
            candidate.fingerprint,
            ["Outputs", "Metadata"],
            candidate.context,
            cancellationToken
        ).ConfigureAwait(false);
        return reading is null ? null : new NativeBuildProduct(
            candidate.recipe.component,
            candidate.recipe.targetID,
            candidate.fingerprint,
            candidate.directory,
            candidate.binding,
            candidate.context.RequireToolchain().linkPlan.sharedLibraryExtension,
            reading
        );
    }

    private sealed record Candidate(
        NativeBuildContext context,
        NativeBuildRecipe recipe,
        string fingerprint,
        string directory,
        string workspace,
        NativeBindingGenerationDescriptor? binding
    );
}
