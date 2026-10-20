using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

/// <summary>
/// Provides preparation input planning for the shared binding generator without producing generated artifacts.
/// </summary>
public sealed partial class NativeBindingGenerator
{
    /// <inheritdoc />
    /// <exception cref="System.NotSupportedException">
    /// Configuration inheritance uses a remote input without local build ownership.
    /// </exception>
    public ValueTask<string> ComputeInputFingerprintAsync(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        CancellationToken cancellationToken
    ) => BindingInputPlanning.ComputeAsync(context, request, cancellationToken);
}
