using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

/// <summary>
/// Executes the shared facade-to-C-to-managed binding workflow independently of MSBuild.
/// </summary>
public sealed class NativeBindingGenerator : INativeBindingGenerator
{
    private readonly string m_dotnetHost;

    /// <summary>
    /// Captures the managed tool executable used only when a declared generator extension must build.
    /// </summary>
    /// <param name="dotnetHost">
    /// The explicit host executable or command selected by composition.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The executable is empty.
    /// </exception>
    public NativeBindingGenerator(string dotnetHost)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        m_dotnetHost = dotnetHost;
    }

    /// <inheritdoc />
    public async ValueTask<IReadOnlyDictionary<string, NativeBindingGenerationDescriptor>> GenerateAsync(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        using var operation = new BindingGenerationOperation(context, request, m_dotnetHost);
        return await operation.ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }
}
