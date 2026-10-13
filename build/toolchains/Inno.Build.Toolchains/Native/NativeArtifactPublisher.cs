using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Publishes complete native products by source and toolchain identity under process-shared ownership.
/// </summary>
public static class NativeArtifactPublisher
{
    /// <summary>
    /// Reuses a validated product or builds an isolated candidate and publishes it after input stability checks.
    /// </summary>
    /// <param name="context">
    /// Checkout and configuration selected by build composition.
    /// </param>
    /// <param name="recipe">
    /// The frozen component, target, input inventory and ordered compiler recipe.
    /// </param>
    /// <param name="build">
    /// Producer receiving an identity-scoped context, private output directory and cancellation token.
    /// It must await all work and validate required outputs before returning.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels ownership acquisition or candidate preparation before publication.
    /// </param>
    /// <returns>
    /// A complete native product with an exact, read-only output closure.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// A component or target identifier is not a single path segment.
    /// </exception>
    /// <exception cref="IOException">
    /// An input, candidate or artifact cannot be read or published.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Inputs change during preparation or the producer returns no complete output.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Ownership acquisition or preparation is canceled; previous products remain installed.
    /// </exception>
    public static async Task<NativeBuildProduct> PublishAsync(
        NativeBuildContext context,
        NativeBuildRecipe recipe,
        Func<NativeBuildContext, string, CancellationToken, Task> build,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(recipe);
        ArgumentNullException.ThrowIfNull(build);
        cancellationToken.ThrowIfCancellationRequested();
        NativeComponentDescriptor owner = recipe.owner;
        NativeBindingGenerationDescriptor? bindingGeneration = owner.bindingDefinition is null
            ? null : context.RequireBindings(owner);
        if (bindingGeneration is not null)
            recipe = recipe.WithBindingGeneration(context.engineRoot, bindingGeneration);
        string component = recipe.component;
        string targetId = recipe.targetId;
        NativeInputSnapshot snapshot = context.inputState.CaptureInitial(recipe.inputs, cancellationToken);
        string fingerprint = NativeBuildFingerprint.Create(recipe.declarations, snapshot);
        string workspaceIdentity = NativeWorkspaceIdentity.Create(context, recipe, snapshot);
        NativeBuildContext scoped = context.WithIdentity(owner, targetId, workspaceIdentity);
        string intermediate = scoped.GetNativeBuildRoot(owner);
        string destination = Path.Combine(context.engineRoot, "artifacts", "native", component, targetId, fingerprint);
        var cache = new BuildCachePublisher(Path.Combine(context.engineRoot, "artifacts", "native"));
        using FileLease cacheUse = await cache.AcquireUseAsync(cancellationToken).ConfigureAwait(false);
        var workspaces = new BuildCachePublisher(Path.Combine(owner.GetToolchainRoot(context.engineRoot), "obj", "native"));
        using FileLease workspaceUse = await workspaces.AcquireUseAsync(cancellationToken).ConfigureAwait(false);
        using FileLease ownership = await FileLease.AcquireAsync(intermediate + ".lock",
            Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        if (NativeBuildFingerprint.Create(context.BeginInputVerification("native-lock:" + component),
            recipe.declarations, recipe.inputs, cancellationToken) != fingerprint)
            throw new InvalidOperationException($"Native inputs for '{component}' changed while waiting for ownership.");
        Directory.CreateDirectory(intermediate);
        AtomicFile.WriteAllBytes(Path.Combine(intermediate, "recipe.json"), JsonSerializer.SerializeToUtf8Bytes(new
        {
            component,
            targetId,
            context.configuration,
            workspaceIdentity,
            artifactFingerprint = fingerprint,
            recipe.owner.nativeProject,
            recipe.owner.toolchainProject,
            recipe.declarations,
            inputs = snapshot.entries.Select(static entry => new
            {
                entry.logicalPath,
                hash = Convert.ToHexStringLower(entry.hash)
            })
        }));
        BuildArtifactReadLease? cached = await BuildArtifactReadLease.TryAcquireAsync(
            destination, fingerprint, ["Outputs"], context, cancellationToken).ConfigureAwait(false);
        if (cached is not null)
        {
            Record(cached, workspaceChanged: false);
            return new(component, targetId, fingerprint, destination, bindingGeneration, context.toolchain?.sharedLibraryExtension, cached);
        }

        void Record(
            BuildArtifactReadLease reading,
            bool workspaceChanged
        ) {
            try
            {
                cache.Record(destination, destination + ".read.lock",
                    bindingGeneration is { artifactDirectory.Length: > 0 } ? [bindingGeneration.artifactDirectory] : [],
                    component + "/" + targetId + "/" + context.configuration, targetId, cancellationToken);
                if (workspaceChanged && Directory.Exists(intermediate))
                    workspaces.Record(intermediate, intermediate + ".lock", [],
                        targetId + "/" + context.configuration, targetId, cancellationToken);
            }
            catch
            {
                reading.Dispose();
                throw;
            }
        }

        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
        string output = Path.Combine(staging, "Outputs");
        Directory.CreateDirectory(output);
        try
        {
            using (ToolchainWorkingDirectory workspace = await ToolchainWorkingDirectory.OpenAsync(
                intermediate, cancellationToken).ConfigureAwait(false))
                await build(scoped.WithToolDirectory(workspace.toolPath).WithExecutionInputs(snapshot),
                    output, cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            if (!PathBoundary.EnumerateFiles(output).Any())
                throw new InvalidOperationException($"Native component '{component}' produced no output files.");
            if (NativeBuildFingerprint.Create(context.BeginInputVerification("native-build:" + component),
                recipe.declarations, recipe.inputs, cancellationToken) != fingerprint)
                throw new InvalidOperationException($"Native inputs for '{component}' changed during preparation; the candidate was not published.");
            BuildArtifactManifest.Write(staging, fingerprint, ["Outputs"], context);
            cancellationToken.ThrowIfCancellationRequested();
            using (BuildArtifactWriteLease publication = await BuildArtifactWriteLease.AcquireAsync(destination,
                cancellationToken).ConfigureAwait(false))
            {
                try
                {
                    AtomicDirectory.Install(staging, destination);
                }
                catch (DirectoryCleanupException cleanupFailure)
                {
                    Console.Error.WriteLine(cleanupFailure.Message);
                }
            }
            BuildArtifactReadLease reading = await BuildArtifactReadLease.TryAcquireAsync(destination, fingerprint, ["Outputs"], context, cancellationToken).ConfigureAwait(false)
                ?? throw new InvalidDataException("The published Native artifact changed before reading ownership was acquired.");
            Record(reading, workspaceChanged: true);
            return new(component, targetId, fingerprint, destination, bindingGeneration, context.toolchain?.sharedLibraryExtension, reading);
        }
        finally
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
        }
    }

}
