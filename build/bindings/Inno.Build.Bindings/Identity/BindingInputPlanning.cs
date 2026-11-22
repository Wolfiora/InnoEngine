using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class BindingInputPlanning
{
    internal static ValueTask<string> ComputeAsync(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        CancellationToken cancellationToken,
        NativeBindingProfile profile
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        List<string> declarations = ["binding-source-inputs", request.targetID, request.outputMode.ToString()];
        foreach (NativeComponentDescriptor component in request.components)
        {
            BindingGenerationConfiguration definition = BindingDefinitionReader.Read(context, component, request, profile);
            declarations.Add(
                NativeBindingGenerationIdentity.Compute(definition.Load(context, cancellationToken), definition, context, cancellationToken)
            );
        }
        return ValueTask.FromResult(NativeBuildFingerprint.Create(context, declarations, [], cancellationToken));
    }
}
