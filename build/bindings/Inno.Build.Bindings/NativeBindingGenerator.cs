using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

/// <summary>
/// Executes the shared facade-to-C-to-managed binding workflow independently of MSBuild.
/// </summary>
public sealed partial class NativeBindingGenerator : INativeBindingGenerator
{
    /// <summary>
    /// Creates a generator that obtains target tools and input ownership from each build operation.
    /// </summary>
    public NativeBindingGenerator()
    {
    }

    /// <inheritdoc />
    public async ValueTask<NativeBindingGenerationResult> GenerateAsync(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        using var operation = new BindingGenerationOperation(context, request);
        return await operation.ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }
}
