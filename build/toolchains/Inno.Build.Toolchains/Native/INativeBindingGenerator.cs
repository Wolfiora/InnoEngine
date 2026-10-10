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
    /// A complete immutable map indexed by Native owner project, never a partial closure.
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
    ValueTask<IReadOnlyDictionary<string, NativeBindingGenerationDescriptor>> GenerateAsync(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        CancellationToken cancellationToken
    );
}
