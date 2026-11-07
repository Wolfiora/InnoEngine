using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Transfers completed native preparation across private task loads using neutral data and independent consumer leases.
/// </summary>
public static class ProductNativePreparation
{
    /// <summary>
    /// Identifies the explicitly selected recipe, SDK, configuration and component request before preparation.
    /// </summary>
    /// <param name="context">
    /// The operation with its selected toolchain and binding generator.
    /// </param>
    /// <param name="plan">
    /// The complete product closure; recipe declarations remain ordered.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels source, generator and SDK input capture before preparation starts.
    /// </param>
    /// <returns>
    /// A content identity for the frozen request. Consumers still verify freshness after acquiring reading protection.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The toolchain or recipe ownership is incomplete.
    /// </exception>
    public static async Task<string> FreezeAsync(
        NativeBuildContext context,
        ProductNativeBuildPlan plan,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(plan);
        NativeToolchainSelection tools = context.RequireToolchain();
        var identities = new List<string>
        {
            NativeBuildFingerprint.Create(context, tools.declarations,
                tools.inputPaths.Select(path => NativeBuildInput.FromPath(context.engineRoot, path)), cancellationToken)
        };
        foreach (NativeBuildRecipe recipe in plan.CreateRecipes(context))
            identities.Add(NativeBuildFingerprint.Create(context, recipe.declarations, recipe.inputs, cancellationToken));
        NativeComponentDescriptor[] components = plan.steps.Select(static step => step.component)
            .Where(static component => component.bindingDefinition is not null).DistinctBy(static component => component.nativeProject).ToArray();
        if (components.Length != 0)
        {
            var options = plan.steps.Where(static step => step.component.bindingDefinition is not null)
                .GroupBy(static step => step.component.nativeProject, StringComparer.Ordinal)
                .ToDictionary(static group => group.Key, static group => group.First().options, StringComparer.Ordinal);
            var request = new NativeBindingGenerationRequest(components, tools.targetId, NativeBindingOutputMode.TargetArtifacts, componentOptions: options);
            identities.Add(await context.RequireBindingGenerator().ComputeInputFingerprintAsync(context, request, cancellationToken).ConfigureAwait(false));
        }
        IEnumerable<string> declarations = new[] { context.engineRoot, context.configuration, plan.productId, tools.targetId }
            .Concat(tools.environment.OrderBy(static pair => pair.Key, StringComparer.Ordinal).Select(static pair => pair.Key + "=" + pair.Value))
            .Concat(tools.cmakeArguments).Concat(tools.inputPaths)
            .Concat(plan.steps.Select(static step => step.id + "|" + step.configuration + "|" + JsonSerializer.Serialize(step.options)))
            .Concat(identities);
        return NativeBuildFingerprint.Create(declarations, []);
    }

    /// <summary>
    /// Captures successful immutable publications and the content inputs actually read during their preparation.
    /// </summary>
    /// <param name="context">
    /// The completed operation, whose recorded input hashes are still owned by this preparation.
    /// </param>
    /// <param name="plan">
    /// The matching declaration-order product plan.
    /// </param>
    /// <param name="products">
    /// The live successful result, retained until the neutral capture finishes.
    /// </param>
    /// <param name="requestIdentity">
    /// The identity returned by freezing this operation before preparation.
    /// </param>
    /// <returns>
    /// BCL-only rows suitable for the MSBuild build-lifetime registry, or null for an aggregate or unowned binding publication.
    /// The rows contain no leases, delegates, plugin objects or private assembly types.
    /// </returns>
    public static string[][]? Capture(
        NativeBuildContext context,
        ProductNativeBuildPlan plan,
        ProductNativeBuildResult products,
        string requestIdentity
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(products);
        ArgumentException.ThrowIfNullOrWhiteSpace(requestIdentity);
        if (products.aggregateDirectory is not null || products.Count != plan.steps.Count
            || products.Any(static product => product.bindingGeneration is { artifactDirectory.Length: 0 }))
            return null;
        var rows = new List<string[]> { new[] { "request", requestIdentity } };
        foreach (NativeInputSnapshot.Entry input in context.inputState.CaptureRecordedInputs(context.engineRoot).entries)
            rows.Add(["input", input.physicalPath, Convert.ToHexStringLower(input.hash)]);
        foreach (string[] root in context.inputState.CaptureRootMembership())
            rows.Add(["root", ..root]);
        for (int index = 0; index < products.Count; index++)
        {
            NativeBuildProduct product = products[index];
            if (product.component != plan.steps[index].id)
                throw new ArgumentException("The prepared products do not match their declaration order.", nameof(products));
            rows.Add(["product", product.component, product.targetId, product.fingerprint, product.directory, product.sharedLibraryExtension ?? string.Empty]);
            if (product.bindingGeneration is NativeBindingGenerationDescriptor binding)
                rows.Add(["binding", product.component, plan.steps[index].component.nativeProject, JsonSerializer.Serialize(binding)]);
        }
        return rows.ToArray();
    }

    /// <summary>
    /// Acquires a new consumer's complete reading protection and verifies mutable inputs after acquisition.
    /// </summary>
    /// <param name="context">
    /// The consumer operation with its own selected SDK and statistics.
    /// </param>
    /// <param name="plan">
    /// The consumer's current complete product declaration.
    /// </param>
    /// <param name="rows">
    /// Neutral data captured by a successful preparation in the same root build.
    /// </param>
    /// <param name="requestIdentity">
    /// The captured request identity, recomputed from current source, generator, SDK and recipe inputs after acquiring protection.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels acquisition or hashing; partial reading protection is always released.
    /// </param>
    /// <returns>
    /// An independently owned result, or null when inputs, membership or immutable publications no longer match.
    /// Null requires normal preparation; it is never evidence that a native build passed.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The consumer cancels before completing its reading protection.
    /// </exception>
    public static async Task<ProductNativeBuildResult?> TryAcquireAsync(
        NativeBuildContext context,
        ProductNativeBuildPlan plan,
        string[][] rows,
        string requestIdentity,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentException.ThrowIfNullOrWhiteSpace(requestIdentity);
        cancellationToken.ThrowIfCancellationRequested();
        if (rows.Length == 0 || rows.Any(static row => row is null || row.Length == 0
                || row[0] is not ("request" or "input" or "root" or "product" or "binding"))
            || rows[0].Length != 2 || rows[0][0] != "request" || rows[0][1] != requestIdentity)
            return null;
        var products = new List<NativeBuildProduct>();
        var leases = new List<IDisposable>();
        var generations = new Dictionary<string, NativeBindingGenerationDescriptor>(StringComparer.Ordinal);
        var byComponent = new Dictionary<string, NativeBindingGenerationDescriptor>(StringComparer.Ordinal);
        bool transferred = false;
        try
        {
            foreach (string[] row in rows.Where(static row => row[0] == "binding"))
            {
                if (row.Length != 4)
                    return null;
                NativeBindingGenerationDescriptor? binding = JsonSerializer.Deserialize<NativeBindingGenerationDescriptor>(row[3]);
                if (binding is null || string.IsNullOrEmpty(binding.artifactDirectory))
                    return null;
                binding.Validate();
                BuildCacheCleanupPlan.RequireChild(context.engineRoot, binding.artifactOwnerDirectory);
                if (!generations.TryGetValue(row[2], out NativeBindingGenerationDescriptor? shared))
                {
                    BuildArtifactReadLease? lease = await BuildArtifactReadLease.TryAcquireAsync(binding.artifactDirectory,
                        binding.artifactOwnerDirectory, binding.fingerprint, ["Generated", "Native"], context, cancellationToken).ConfigureAwait(false);
                    if (lease is null)
                        return null;
                    leases.Add(lease);
                    generations.Add(row[2], binding);
                }
                else if (JsonSerializer.Serialize(shared) != row[3])
                    return null;
                byComponent.Add(row[1], binding);
            }
            string[][] productRows = rows.Where(static row => row[0] == "product").ToArray();
            if (productRows.Length != plan.steps.Count)
                return null;
            for (int index = 0; index < productRows.Length; index++)
            {
                string[] row = productRows[index];
                if (row.Length != 6 || row[1] != plan.steps[index].id || row[2] != context.RequireToolchain().targetId)
                    return null;
                string owner = Path.Combine(context.engineRoot, "artifacts", "native");
                BuildArtifactReadLease? lease = await BuildArtifactReadLease.TryAcquireAsync(row[4], owner, row[3],
                    ["Outputs", "Metadata"], context, cancellationToken).ConfigureAwait(false);
                if (lease is null)
                    return null;
                products.Add(new NativeBuildProduct(row[1], row[2], row[3], row[4], byComponent.GetValueOrDefault(row[1]),
                    row[5].Length == 0 ? null : row[5], lease));
            }
            NativeBuildContext verification = context.BeginInputVerification("prepared-consumer");
            if (await FreezeAsync(verification, plan, cancellationToken).ConfigureAwait(false) != requestIdentity)
                return null;
            foreach (string[] row in rows.Where(static row => row[0] == "root"))
            {
                if (row.Length < 2)
                    return null;
                NativeBuildInput root = NativeBuildInput.FromPath(context.engineRoot, row[1]);
                NativeInputSnapshot membership = verification.CaptureInputs([root], cancellationToken);
                if (!membership.entries.Select(entry => entry.logicalPath == root.logicalPath
                        ? string.Empty : entry.logicalPath[(root.logicalPath.Length + 1)..])
                    .Order(StringComparer.Ordinal).SequenceEqual(row.Skip(2), StringComparer.Ordinal))
                    return null;
            }
            foreach (string[] row in rows.Where(static row => row[0] == "input"))
            {
                if (row.Length != 3)
                    return null;
                NativeInputSnapshot input = verification.CaptureInputs([NativeBuildInput.FromPath(context.engineRoot, row[1])], cancellationToken);
                if (input.entries.Count != 1 || Convert.ToHexStringLower(input.entries[0].hash) != row[2])
                    return null;
            }
            if (!rows.Any(static row => row[0] == "input"))
                return null;
            cancellationToken.ThrowIfCancellationRequested();
            var bindings = new NativeBindingGenerationResult(generations, leases);
            var result = new ProductNativeBuildResult(products, bindings);
            transferred = true;
            return result;
        }
        catch (Exception failure) when (failure is FileNotFoundException or DirectoryNotFoundException or InvalidDataException or JsonException)
        {
            return null;
        }
        finally
        {
            if (!transferred)
                ProductNativeBuildResult.Release(products, new NativeBindingGenerationResult(generations, leases));
        }
    }
}
