using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Prepares an entire binding closure without coupling native recipes to a generator or build host.
/// </summary>
public interface INativeBindingGenerator
{
    /// <summary>
    /// Computes the selected generator source input identity without producing bindings or compiling a product.
    /// </summary>
    /// <param name="context">
    /// The borrowed operation with frozen target tools and phase-local input reads.
    /// </param>
    /// <param name="request">
    /// The exact component, configuration target and output mode whose source closure is planned.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels configuration loading, dependency evaluation and content hashing.
    /// </param>
    /// <returns>
    /// The complete source preparation identity, distinct from any generated artifact fingerprint.
    /// It remains computable when reconstructible generation outputs have been retired.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// The selected tools, configuration, generator or extension inputs cannot be resolved.
    /// </exception>
    /// <exception cref="System.OperationCanceledException">
    /// Input planning was canceled.
    /// </exception>
    ValueTask<string> ComputeInputFingerprintAsync(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Generates or verifies the requested closure under publication ownership.
    /// </summary>
    /// <param name="context">
    /// The borrowed operation owning frozen inputs, tool selection and statistics.
    /// Configuration path variables must resolve from its selected toolchain without changing process state.
    /// </param>
    /// <param name="request">
    /// The immutable target, component closure and publication policy.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels preparation; active work must drain before returning.
    /// </param>
    /// <returns>
    /// An owned complete immutable map indexed by Native owner project, never a partial closure.
    /// Dispose the result only after all generated-source and native-bridge consumers finish.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// Inputs change during preparation, a required frozen SDK value is absent,
    /// or required generator capabilities are unavailable.
    /// </exception>
    /// <exception cref="System.IO.InvalidDataException">
    /// A definition or generated output fails its declared contract.
    /// </exception>
    /// <exception cref="System.OperationCanceledException">
    /// The request was canceled and its active work has completed cleanup.
    /// </exception>
    ValueTask<NativeBindingGenerationResult> GenerateAsync(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        CancellationToken cancellationToken
    );
}
