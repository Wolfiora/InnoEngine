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
    private readonly NativeBindingProfile m_profile;

    /// <summary>
    /// Creates a generator that obtains target tools and input ownership from each build operation.
    /// </summary>
    /// <param name="profile">
    /// The explicitly registered target profile.
    /// </param>
    public NativeBindingGenerator(NativeBindingProfile profile)
    {
        m_profile = profile ?? throw new ArgumentNullException(nameof(profile));
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
        using var operation = new BindingGenerationOperation(context, request, m_profile);
        return await operation.ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }
}
