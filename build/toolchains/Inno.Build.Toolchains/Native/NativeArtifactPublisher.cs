using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        string component = recipe.component;
        string targetId = recipe.targetId;
        NativeInputSnapshot snapshot = context.inputState.CaptureInitial(recipe.inputs, cancellationToken);
        string fingerprint = NativeBuildFingerprint.Create(recipe.declarations, snapshot);
        NativeBuildContext scoped = context.WithIdentity(owner, targetId, fingerprint);
        string intermediate = scoped.GetNativeBuildRoot(owner);
        string destination = Path.Combine(context.engineRoot, "artifacts", "native", component, targetId, fingerprint);
        using FileLease ownership = await FileLease.AcquireAsync(intermediate + ".lock",
            Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        if (NativeBuildFingerprint.Create(recipe.declarations,
            context.inputState.CaptureVerification(recipe.inputs, cancellationToken)) != fingerprint)
            throw new InvalidOperationException($"Native inputs for '{component}' changed while waiting for ownership.");
        if (BuildArtifactManifest.IsComplete(destination, fingerprint, ["Outputs"]))
            return new(component, targetId, fingerprint, destination);

        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
        string output = Path.Combine(staging, "Outputs");
        Directory.CreateDirectory(output);
        try
        {
            using (ToolchainWorkingDirectory workspace = await ToolchainWorkingDirectory.OpenAsync(
                intermediate, cancellationToken).ConfigureAwait(false))
                await build(scoped.WithToolDirectory(workspace.toolPath), output, cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            if (!PathBoundary.EnumerateFiles(output).Any())
                throw new InvalidOperationException($"Native component '{component}' produced no output files.");
            if (NativeBuildFingerprint.Create(recipe.declarations,
            context.inputState.CaptureVerification(recipe.inputs, cancellationToken)) != fingerprint)
                throw new InvalidOperationException($"Native inputs for '{component}' changed during preparation; the candidate was not published.");
            BuildArtifactManifest.Write(staging, fingerprint, ["Outputs"]);
            cancellationToken.ThrowIfCancellationRequested();
            AtomicDirectory.Install(staging, destination);
            return new(component, targetId, fingerprint, destination);
        }
        finally
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
        }
    }

}
